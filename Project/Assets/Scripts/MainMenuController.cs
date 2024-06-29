using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI HighscoreText;

    [Header("Windows")]
    [SerializeField] RectTransform TutorialWindow;
    [SerializeField] RectTransform SettingsWindow;

    [Header("Buttons")]
    [SerializeField] Button TutorialButton;
    [SerializeField] Button SettingsButton;

    [Header("Particles")]
    [SerializeField] ParticleSystem clouds;
    [SerializeField] ParticleSystem particles;

    [Header("Transitions")]
    [SerializeField] ScreenTransitions screenTransitions;

    [Header("Volume slider")]
    [SerializeField] Slider volumeSlider;
    [SerializeField] private string key_masterVolume = "MasterVolume";

    [Header("Sound effects")]
    [SerializeField] AudioSource tapSound;
    [SerializeField] AudioSource playGameSound;
    private BGM bgm;

    void Awake()
    {
        TutorialWindow.gameObject.SetActive(false);
        HighscoreText.text = PlayerPrefs.GetInt("highscore") + " Token(s)";
        SetupAudio();
    }

    void Start()
    {
        bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<BGM>();
        bgm.StopMusic();
        clouds.Play();
        particles.Play();
        CloseWindows();
    }

    public void PlayGame()
    {
        bgm.StartMusic();
        // SceneManager.LoadScene("Play");
        screenTransitions.ChangeScene("Play");
    }

    /////////////// Windows ////////////////////

    public void OpenTutorial()
    {
        TutorialWindow.gameObject.SetActive(true);
        DisableButtons();
    }

    public void CloseWindows()
    {
        TutorialWindow.gameObject.SetActive(false);
        SettingsWindow.gameObject.SetActive(false);
        EnableButtons();
    }

    public void OpenSettings()
    {
        SettingsWindow.gameObject.SetActive(true);
        DisableButtons();
    }

    private void EnableButtons()
    {
        TutorialButton.enabled = true;
        SettingsButton.enabled = true;
    }

    private void DisableButtons()
    {
        TutorialButton.enabled = false;
        SettingsButton.enabled = false;
    }

    /////////////// Master Audio ////////////////////

    public void SetupAudio()
    {
        if (!PlayerPrefs.HasKey(key_masterVolume))
            PlayerPrefs.SetFloat(key_masterVolume, 1);

        volumeSlider.value = PlayerPrefs.GetFloat(key_masterVolume);
        AudioListener.volume = volumeSlider.value;
    }

    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat(key_masterVolume, volume);
        AudioListener.volume = volumeSlider.value;
    }

    /////////////// Sound ////////////////////

    public void PlayTapSound()
    {
        tapSound.Play();
    }

    public void PlayTapPlayGame()
    {
        playGameSound.Play();
    }
}
