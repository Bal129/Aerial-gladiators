using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class LevelManager : MonoBehaviour
{
    [Header("Player Section")]
    [HideInInspector] public GameObject playerObject;
    private Gladiator gladiator;
    private Rigidbody gladiatorRb;
    public static bool levelSetUp = false;

    [Header("Camera Section")]
    [SerializeField] private Camera mainCamera;

    [Header("Enemy Section")]
    public GameObject[] enemyPrefabs;
    private GameObject[] numOfEnemies;

    [Header("Platform Section")]
    public GameObject platformFloorPrefab;
    public GameObject platformFloorParent;
    public GameObject[] platformLevelDesign;
    // public PlatformType[] platformTypes;
    // private GameObject platformFloor;
    private List<PlatformType> emptyPlatforms; // to fill in platforms that can be used to instantiate objects

    [Header("Traps / Enemies Setup")]
    public int minNumOfTraps;
    public int maxNumOfTraps;
    // private int totalTraps;

    public int minNumOfEnemies;
    public int maxNumOfEnemies;
    private int totalEnemies;

    private System.Random randomiser;

    [Header("HUDs")]
    [SerializeField] RectTransform HUD;
    [SerializeField] Slider HealthBar;
    [SerializeField] Image HealthBarFill;
    private ScreenEffect screenEffect;

    [Header("Health")]
    [SerializeField] private Color highHealth;
    [SerializeField] private Color mediumHealth;
    [SerializeField] private Color lowHealth;

    [Header("Speed")]
    [SerializeField] private Color lowSpeed;
    [SerializeField] private Color highSpeed;
    public TextMeshProUGUI speedIndicator;

    [Header("Interactables")]
    [SerializeField] FixedTouchField TouchField;

    [Header("Panels")]
    [SerializeField] RectTransform WinPanel;
    [SerializeField] RectTransform LosePanel;
    [SerializeField] RectTransform PausePanel;
    [SerializeField] RectTransform TutorialPanel;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] TextMeshProUGUI ScoreText;

    [Header("SpeedMeter")]
    [SerializeField] RectTransform speedMeterIndicator;

    [Header("Transitions")]
    [SerializeField] ScreenTransitions screenTransitions;

    private int currentLevel;
    private int currentScore;
    // [SerializeField] RectTransform MainMenu;

    void Start()
    {
        randomiser = new System.Random();
        TouchField.enabled = true;
        screenEffect = GetComponent<ScreenEffect>();
        SetupGame(); // call once every time to generate new level
        SetupAudio();
    }

    private void FixedUpdate()
    {
        if (gladiator != null)
        {
            UpdateHealthBar(gladiator.currentHealth);
            UpdateSpeedMeter();
        }

        if (!gladiator.isAlive)
        {
            UpdateHealthBar(0);
            StartCoroutine(LoadLoseSequence());
        }

        if (enemyPrefabs.Length <= 0)
        {
            StartCoroutine(LoadWinSequence());
        }

        UpdatePlayerPosition();
    }

    /*
    private void FixedUpdate()
    {
        if (enemyPrefabs.Length <= 0)
        {
            StartCoroutine(LoadWinSequence());
        }

        UpdatePlayerPosition();
        //UpdateCamera();
    }
    */

    private void LateUpdate()
    {
        numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (numOfEnemies.Length <= 0)
        {
            StartCoroutine(LoadWinSequence());
        }
    }


    ////////////////////////// SETUPS ///////////////////////////////

    void SetupGame()
    {
        levelSetUp = false;

        SetupScore();
        SetupLevel();
        SetupPlatform();
        SetupPlayer();

        ResetPanel();
        UpdateScoreText(currentScore);
        UpdateLevelText(currentLevel);

        levelSetUp = true;

        Time.timeScale = 1;
    }

    void SetupScore()
    {
        currentScore = PlayerPrefs.GetInt("currentScore");
    }

    void SetupLevel()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel");
    }

    void SetupPlayer()
    {
        if (gladiator == null)
        {
            gladiator = playerObject.GetComponent<Gladiator>();
            gladiatorRb = playerObject.GetComponent<Rigidbody>();

            gladiator.currentHealth = PlayerPrefs.GetFloat("currentHealth", gladiator.maxHealth);

            if (gladiator.currentHealth == 0)
            {
                RestartGame();
            }

            // Debug.Log("gladiator current health (in setupPlayer): " + gladiator.currentHealth);

            // gladiator.HealthBar = HealthBar;

            HealthBar.minValue = 0;
            HealthBar.maxValue = gladiator.maxHealth;
            UpdateHealthBar(gladiator.currentHealth);

            // gladiator.SetupHealthBar();
            // Debug.Log("HealthBar value (in setupPlayer): " + HealthBar.value);
        }
    }

    void SetupPlatform()
    {
        // Steps to setup platform level:
        // 1. Setup initial level pattern (based on premade prefabs)
        // 2. Setup player(gladiator) position
        // 3. Setup enemies' position

        // if (playerObject != null) Destroy(playerObject); // to reset everything

        int platformLevel = randomiser.Next(0, platformLevelDesign.Length);
        foreach (GameObject level in platformLevelDesign)
        {
            level.SetActive(false);
        }
        platformLevelDesign[platformLevel].SetActive(true);

        platformFloorPrefab = platformLevelDesign[platformLevel];

        emptyPlatforms = new List<PlatformType>();

        foreach (PlatformType platform in platformFloorPrefab.GetComponentsInChildren<PlatformType>())
        {
            if (platform.platformID == 0)
            {
                emptyPlatforms.Add(platform);
            }
        }

        // Enemies platform distribution:
        // 1. Get the count for empty platforms (platform id == 0) in a list
        // 2. Shuffle platforms
        // 3. First few selected platforms will be replaced with enemy 

        totalEnemies = randomiser.Next(minNumOfEnemies, maxNumOfEnemies + 1);
        int[] usedPlatform = new int[totalEnemies + 1]; // enemies + player (gladiator)

        List<int> shufflePlatforms = new List<int>();
        for (int i = 0; i < emptyPlatforms.Count; i++)
        {
            shufflePlatforms.Add(i);
        }
        CustomUtils.ShuffleElements(shufflePlatforms);

        for (int i = 0; i < usedPlatform.Length; i++)
        {
            usedPlatform[i] = shufflePlatforms[i];
        }

        /*
        // debug
        string word = "Platform: ";
        foreach (int platform in usedPlatform)
        {
            word = word + ", " + platform;
        }
        Debug.Log(word);
        */

        // 1 platform for gladiator

        if (playerObject == null)
        {
            playerObject = emptyPlatforms[usedPlatform[0]].GetComponent<Spawner>().SpawnGladiator();
        }
        else
        {
            playerObject.transform.position = emptyPlatforms[usedPlatform[0]].GetComponent<Spawner>().spawnZone.position;
        }

        // 3-4 platform for enemies
        enemyPrefabs = new GameObject[totalEnemies];
        int enemyCount = 0;

        for (int i = 1; i < totalEnemies + 1; i++)
        {
            int randomSelectEnemies = randomiser.Next(2); // TO-DO: FIND WAY TO OBTAIN TYPES OF ENEMIES AUTOMATICALLY
            enemyPrefabs[enemyCount] = emptyPlatforms[usedPlatform[i]].GetComponent<Spawner>().SpawnEnemy(randomSelectEnemies);
            enemyPrefabs[enemyCount].transform.SetParent(platformFloorPrefab.transform);
            enemyCount++;
        }
    }

    void SetupAudio()
    {
        if (!PlayerPrefs.HasKey("MasterVolume"))
            PlayerPrefs.SetFloat("MasterVolume", 1);
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume");
    }

    void ResetPanel()
    {
        WinPanel.gameObject.SetActive(false);
        LosePanel.gameObject.SetActive(false);
        PausePanel.gameObject.SetActive(false);
        TutorialPanel.gameObject.SetActive(false);
    }

    ////////////////////////// PLAYER UPDATES ///////////////////////////////

    void UpdatePlayerPosition()
    {
        float speed;

        if (gladiator != null && gladiator.isAlive)
        {
            if (gladiator.isPowerDashing) speed = gladiator.dashSpeed;
            else speed = gladiator.moveSpeed;


            if (!gladiator.isGlued)
            {
                gladiatorRb.AddForce(new Vector3(TouchField.TouchDist.x, 0, TouchField.TouchDist.y) * speed, ForceMode.Force);
                var playerMaterial = playerObject.GetComponent<Renderer>().material;

                if (gladiatorRb.velocity.magnitude > gladiator.offensiveVelocity)
                {
                    playerMaterial.EnableKeyword("_EMISSION");
                    playerMaterial.SetColor("_EmissionColor", Color.red);
                    gladiator.dashingParticles.Play();
                }
                else
                {
                    playerMaterial.DisableKeyword("_EMISSION");
                    gladiator.dashingParticles.Stop();
                }
            }
        }
        // Debug.Log("Velocity: " + gladiatorRb.velocity.magnitude);
    }

    void UpdateCamera()
    {
        mainCamera.transform.LookAt(playerObject.transform);
    }

    void UpdateSpeedMeter()
    {
        float magnitude = gladiatorRb.velocity.magnitude;
        float indicatorValue = Mathf.Clamp(magnitude / gladiator.offensiveVelocity, 0, 1);

        speedIndicator.text = "Speed: " + (int)(indicatorValue * 100) + "%";

        speedMeterIndicator.localScale = new Vector3(
            indicatorValue,
            speedMeterIndicator.localScale.y,
            speedMeterIndicator.localScale.z
        );

        if (indicatorValue >= 1)
        {
            speedMeterIndicator.GetComponent<Image>().color = new Color(highSpeed.r, highSpeed.g, highSpeed.b);
        }
        else
        {
            speedMeterIndicator.GetComponent<Image>().color = new Color(lowSpeed.r, lowSpeed.g, lowSpeed.b);
        }
    }

    void UpdateHealthBar(float health)
    {
        HealthBar.value = health;
        float health50percent = gladiator.maxHealth * 0.5f;
        float health30percent = gladiator.maxHealth * 0.3f;

        if (HealthBar.value > health50percent)
        {
            HealthBarFill.color = new Color(highHealth.r, highHealth.g, highHealth.b);
        }
        else if (HealthBar.value > health30percent)
        {
            HealthBarFill.color = new Color(mediumHealth.r, mediumHealth.g, mediumHealth.b);
        }
        else
        {
            HealthBarFill.color = new Color(lowHealth.r, lowHealth.g, lowHealth.b);
        }
    }

    void UpdatePrefsHealth()
    {
        PlayerPrefs.SetFloat("currentHealth", gladiator.currentHealth);
    }

    void ResetPrefsHealth()
    {
        PlayerPrefs.SetFloat("currentHealth", gladiator.maxHealth);
    }

    ////////////////////////// LOSE WIN SEQUENCE ///////////////////////////////

    IEnumerator LoadWinSequence()
    {
        TouchField.enabled = false;

        int updatedLevel = currentLevel + 1;
        int updatedScore = currentScore + totalEnemies;
        UpdateLevelText(updatedLevel);
        UpdateScoreText(updatedScore);

        UpdateScore(updatedScore);
        UpdateHighscore();
        UpdateLevel(updatedLevel);

        UpdatePrefsHealth();

        WinPanel.gameObject.SetActive(true);

        gladiatorRb.velocity = Vector3.zero;

        screenTransitions.WinGame();

        yield return new WaitForSeconds(0);

        // Time.timeScale = 0;
        // SceneManager.LoadScene("Play");
        screenTransitions.ChangeScene("Play");
    }

    IEnumerator LoadLoseSequence()
    {
        yield return new WaitForSeconds(0);
        screenTransitions.LoseGame();

        // Time.timeScale = 0;

        UpdateHighscore();
        ResetScore();
        ResetLevel();

        ResetPrefsHealth();

        LosePanel.gameObject.SetActive(true);
    }


    ////////////////////////// SCORING SYSTEM ///////////////////////////////

    public void UpdateScore(int score)
    {
        PlayerPrefs.SetInt("currentScore", score);
    }

    public void ResetScore()
    {
        PlayerPrefs.SetInt("currentScore", 0);
    }

    public void UpdateHighscore()
    {
        int highscore = PlayerPrefs.GetInt("highscore");
        if (currentScore > highscore)
        {
            PlayerPrefs.SetInt("highscore", currentScore);
        }
    }

    private void UpdateScoreText(int score)
    {
        ScoreText.text = score.ToString();
    }

    ////////////////////////// LEVEL SYSTEM ///////////////////////////////

    public void UpdateLevel(int level)
    {
        PlayerPrefs.SetInt("currentLevel", level);
    }

    public void ResetLevel()
    {
        PlayerPrefs.SetInt("currentLevel", 1);
    }

    private void UpdateLevelText(int level)
    {
        LevelText.text = level.ToString();
    }


    ////////////////////////// BUTTONS MANAGER ///////////////////////////////

    public void Dash()
    {
        gladiator.isPowerDashing = true;
    }

    // MENUS' BUTTON MANAGER

    public void PlayGame()
    {
        /*
        MainMenu.gameObject.SetActive(false);
        WinPanel.gameObject.SetActive(false);
        LosePanel.gameObject.SetActive(false);
        PausePanel.gameObject.SetActive(false);
        */

        SetupGame();
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        // levelSetUp = false;

        // MainMenu.gameObject.SetActive(false);
        ResetPanel();
        // PausePanel.gameObject.SetActive(true);

        screenTransitions.PauseGame();

        // Time.timeScale = 0;
    }

    public void ContinueCurrentGame()
    {
        // MainMenu.gameObject.SetActive(false);
        ResetPanel();

        screenTransitions.ContinueGame();

        // Time.timeScale = 1;
    }

    public void ContinueToNextLevel()
    {
        // MainMenu.gameObject.SetActive(false);
        ResetPanel();

        // Destroy(platformFloor);
        // Destroy(gladiator);

        // SetupLevel();

        UpdateHighscore();
        UpdatePrefsHealth();

        // SceneManager.LoadScene("Play");
        screenTransitions.ChangeScene("Play");

        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        // levelSetUp = false;

        // MainMenu.gameObject.SetActive(false);
        ResetPanel();

        UpdateHighscore();
        ResetScore();
        ResetPrefsHealth();
        ResetLevel();


        // SceneManager.LoadScene("Play");
        screenTransitions.ChangeScene("Play");

        Time.timeScale = 1;
    }

    public void QuitToMenu()
    {
        // MainMenu.gameObject.SetActive(true);
        ResetPanel();

        UpdateHighscore();
        ResetScore();
        ResetPrefsHealth();
        ResetLevel();

        // Time.timeScale = 0;
        // SceneManager.LoadScene("MainMenu");

        screenTransitions.ChangeScene("MainMenu");

        Time.timeScale = 1;
    }

    public void OpenPlatformInfoPanel()
    {
        //Time.timeScale = 0;
        TutorialPanel.gameObject.SetActive(true);
    }

    public void ClosePlatformInfoPanel()
    {
        Time.timeScale = 1;
        TutorialPanel.gameObject.SetActive(false);
    }
}
