using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class LevelManager1 : MonoBehaviour
{
    [Header("Player Section")]
    public GameObject playerObject;
    private Gladiator gladiator;
    private Rigidbody gladiatorRb;
    public static bool levelSetUp = false;

    [Header("EnemySection")]
    public GameObject[] enemyPrefabs;
    private GameObject[] numOfEnemies;

    [Header("Platform Section")]
    public GameObject platformFloorPrefab;
    public GameObject platformFloorParent;
    public PlatformType[] platformTypes;
    private GameObject platformFloor;
    private PlatformType[] platforms;

    public int minNumOfTraps;
    public int maxNumOfTraps;
    private int totalTraps;
    
    public int minNumOfEnemies;
    public int maxNumOfEnemies;
    private int totalEnemies;

    private System.Random platformRandomiser;

    [Header("HUDs Section")]
    [SerializeField] RectTransform HUD;
    [SerializeField] Slider HealthBar;
    [SerializeField] FixedTouchField TouchField;
    [SerializeField] RectTransform WinPanel;
    [SerializeField] RectTransform LosePanel;
    [SerializeField] RectTransform PausePanel;
    [SerializeField] RectTransform PlatformInfoPanel;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] TextMeshProUGUI TokenText;

    private string directory;
    private int currentLevel = 1;
    private int currentToken;
    // [SerializeField] RectTransform MainMenu;

    void Awake()
    {
        currentToken = 0;
        // Instantiate(HUD, transform);
        DontDestroyOnLoad(HUD);
    }

    void Start()
    {
        directory = Environment.GetEnvironmentVariable("windir");
        platformRandomiser = new System.Random();
        //SetupDatabase("\\CurrentLevel.txt"); // Get current level
        UpdateLevelText(currentLevel);
        UpdateTokenText(currentToken);
        SetupLevel(); // call once every time to generate new level
    }

    void SetupDatabase(string fileName)
    {
        StreamReader reader = new StreamReader(directory + fileName);
        try
        {
            do
            {
                currentLevel = int.Parse(reader.ReadLine());
            }
            while (reader.Peek() != -1);
        }
        catch
        {
            Debug.Log("Failed to read database");
            currentLevel = 0;
        }
        finally
        {
            reader.Close();
        }
    }

    void UpdateDatabase(string fileName, string value)
    {
        StreamWriter writer = new StreamWriter(directory + fileName);
        try
        {
            writer.WriteLine(value);
        }
        catch
        {
            Debug.Log("Failed to update database");
        }
        finally
        {
            writer.Close();
        }
    }

    void SetupLevel()
    {
        levelSetUp = false;
        ResetPanel();
        SetupPlatform();
        SetupPlayer();
        levelSetUp = true;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!gladiator.isAlive)
        {
            // Destroy(gladiator);
            StartCoroutine(LoadLoseSequence());
        }
    }

    private void FixedUpdate()
    {
        // if (levelSetUp)
        //{
            // gladiatorRb.velocity += new Vector3(0, additionalGravity, 0);

            if (enemyPrefabs.Length <= 0)
            {
                StartCoroutine(LoadWinSequence());
            }

            // Debug.Log("Gladiator is Dashing: " + gladiator.isDashing);
            UpdatePlayerPosition();
        //}
    }

    private void LateUpdate()
    {
        numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Debug.Log("num of enemies: " + numOfEnemies);

        /*
        foreach (GameObject enemy in numOfEnemies)
        {
            Debug.Log("enemy: " + enemy);
        }
        */

        if (numOfEnemies.Length <= 0)
        {
            StartCoroutine(LoadWinSequence());
        }
    }

    void SetupPlayer()
    {
        if (gladiator == null)
        {
            gladiator = playerObject.GetComponent<Gladiator>();
            gladiatorRb = playerObject.GetComponent<Rigidbody>();
        }
        // gladiator.HealthBar = HealthBar;
    }

    /*
    void SetupPlatform()
    {
        Transform[] platformPlaceholders = platformFloor.GetComponentsInChildren<Transform>();

        // randomly set platform
        for (int i=0; i<platforms.Length; i++)
        {
            int selectedPlatform = platformRandomiser.Next(platformTypes.Length);

            if (selectedPlatform != PlatformType.Normal)
            {
                GameObject newPlatform = Instantiate(
                    platformTypes[selectedPlatform].platformPrefabModel,
                    platforms[i].transform.position,
                    Quaternion.identity
                );
                newPlatform.transform.SetParent(platformFloor.transform);
                Destroy(platforms[i]);
            }
        }
    }
    */

    void SetupPlatform()
    {
     //   if (playerObject != null) Destroy(playerObject);
        if (platformFloor != null) Destroy(platformFloor);
        platformFloor = Instantiate(platformFloorPrefab, transform);
        platformFloor.transform.SetParent(platformFloorParent.transform);
        platforms = platformFloor.GetComponentsInChildren<PlatformType>();

        // platform distribution:

        // 1 platform for gladiator
        // 4-6 platform for traps, randomised
        // 3-4 platform for enemies
        // >> total 8-11 / 30 platform used

        // Let say 8 (1 player + 4 traps + 3 enemies) platforms will be used
        // Platform: 0, 1, 2, 3, 4, 5, 6, 7
        //           ^  \        /  \     / 
        //           |   --------    \   /
        //        player     |        ---
        //                 traps       |
        //                          enemies
        // player = 0
        // traps = 1 ~ 4(totalTraps)
        // enemies = 5(totalTraps(4)+1) ~ 7(totalTraps(4)+totalEnemies(3))

        // How to get random unique numbers:
        // >> Let say 8 / 30 platforms will be used
        // 1. Create a list of 30 integers
        // 2. Shuffle the list
        // 3. Put the first 8 numbers into an array
        // 4. Use the array as indexes of the platforms that will be used

        totalTraps = platformRandomiser.Next(minNumOfTraps, maxNumOfTraps + 1);
        totalEnemies = platformRandomiser.Next(minNumOfEnemies, maxNumOfEnemies + 1);
        int totalPlatformUsed = 1 + totalTraps + totalEnemies; // gladiator + traps + 
        int[] usedPlatform = new int[totalPlatformUsed];

        // Debug.Log("Total traps: " + totalTraps + ", Total enemies: " + totalEnemies + ", Total platforms: " + totalPlatformUsed);


        List<int> shufflePlatforms = new List<int>();
        for (int i=0; i<platforms.Length; i++)
        {
            shufflePlatforms.Add(i);
        }
        CustomUtils.ShuffleElements(shufflePlatforms);

        for (int i=0; i<totalPlatformUsed; i++)
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
        GameObject gladiatorPlatform = Instantiate(
                platformTypes[PlatformType.Spawner].platformPrefabModel,
                platforms[usedPlatform[0]].transform.position,
                Quaternion.identity
        );
        gladiatorPlatform.transform.SetParent(platformFloor.transform);
        if (playerObject == null)
        {
            playerObject = gladiatorPlatform.GetComponent<Spawner>().SpawnGladiator();
        }
        else
        {
            playerObject.transform.position = gladiatorPlatform.GetComponent<Spawner>().spawnZone.position;
        }

        // 4-6 platform for traps, randomised
        for (int i = 1; i <= totalTraps; i++)
        {
            int randomSelectTrap = platformRandomiser.Next(1, platformTypes.Length - 1);
            // ^ platform type 0 = normal, platform type Length = spawner
            // ^ exclude both of the numbers

            Debug.Log("Platform: " + i + ", trap: " + randomSelectTrap);

            GameObject trapPlatform = Instantiate(
                    platformTypes[randomSelectTrap].platformPrefabModel,
                    platforms[usedPlatform[i]].transform.position,
                    Quaternion.identity
                );
            trapPlatform.transform.SetParent(platformFloor.transform);
        }

        // 3-4 platform for enemies
        enemyPrefabs = new GameObject[totalEnemies];
        int enemyCount = 0;

        for (int i = totalTraps + 1; i < (totalTraps + totalEnemies + 1); i++)
        {
            int randomSelectEnemies = platformRandomiser.Next(2); // TO-DO: FIND WAY TO OBTAIN TYPES OF ENEMIES AUTOMATICALLY

            GameObject enemyPlatform = Instantiate(
                    platformTypes[PlatformType.Spawner].platformPrefabModel,
                    platforms[usedPlatform[i]].transform.position,
                    Quaternion.identity
                );
            enemyPlatform.transform.SetParent(platformFloor.transform);
            enemyPrefabs[enemyCount] = enemyPlatform.GetComponentInChildren<Spawner>().SpawnEnemy(randomSelectEnemies);
            enemyCount++;
        }

        // Replace all used platforms
        for (int i=0; i<usedPlatform.Length; i++)
        {
            Destroy(platformFloor.GetComponentsInChildren<Transform>()[usedPlatform[i]+1].gameObject);
        }
    }

    void ResetPanel()
    {
        WinPanel.gameObject.SetActive(false);
        LosePanel.gameObject.SetActive(false);
        PausePanel.gameObject.SetActive(false);
    }

    void UpdatePlayerPosition()
    {
        float speed;

        if (gladiator.isAlive)
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
                    playerMaterial.SetColor("_EmissionColor", playerMaterial.color + Color.white);
                    gladiator.dashingParticles.Play();
                }
                else
                {
                    playerMaterial.DisableKeyword("_EMISSION");
                }
            }
        }
        // Debug.Log("Velocity: " + gladiatorRb.velocity.magnitude);
    }

    IEnumerator LoadWinSequence()
    {
        int updatedLevel = currentLevel + 1;
        int updatedCurrentToken = currentToken + totalEnemies;
        UpdateLevelText(updatedLevel);
        UpdateTokenText(updatedCurrentToken);

        WinPanel.gameObject.SetActive(true);
        gladiatorRb.velocity = Vector3.zero;
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        // UpdateDatabase("\\CurrentLevel.txt", "" + (++currentLevel));

        SceneManager.LoadScene("Play");
    }

    IEnumerator LoadLoseSequence()
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        LosePanel.gameObject.SetActive(true);
    }

    private void UpdateLevelText(int levels)
    {
        LevelText.text = "Level: " + levels;
    }

    private void UpdateTokenText(int tokens)
    {
        TokenText.text = "Token: " + tokens;
    }

    // BUTTONS MANAGER

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

        SetupLevel();
        Time.timeScale = 1;
    }
    
    public void PauseGame()
    {
        // levelSetUp = false;

        // MainMenu.gameObject.SetActive(false);
        ResetPanel();
        PausePanel.gameObject.SetActive(true);

        Time.timeScale = 0;
    }

    public void ContinueCurrentGame()
    {
        // MainMenu.gameObject.SetActive(false);
        ResetPanel();

        Time.timeScale = 1;
    }

    public void ContinueToNextLevel()
    {
        // MainMenu.gameObject.SetActive(false);
        ResetPanel();

        // Destroy(platformFloor);
        // Destroy(gladiator);

        // SetupLevel();

        SceneManager.LoadScene("Play");

        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        // levelSetUp = false;

        // MainMenu.gameObject.SetActive(false);
        ResetPanel();

        // SetupLevel();

        SceneManager.LoadScene("Play");

        Time.timeScale = 1;
    }

    public void QuitToMenu()
    {
        // MainMenu.gameObject.SetActive(true);
        ResetPanel();
        
        SceneManager.LoadScene("MainMenu");

        Time.timeScale = 0;
    }

    public void OpenPlatformInfoPanel()
    {
        Time.timeScale = 0;
        PlatformInfoPanel.gameObject.SetActive(true);
    }

    public void ClosePlatformInfoPanel()
    {
        Time.timeScale = 1;
        PlatformInfoPanel.gameObject.SetActive(false);
    }
}
