using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    bool isPaused;
    public AudioSource clickSound;
    public AudioSource menuMusic;
    public AudioMixer audioMixer;
    public GameObject optionsMenu;
    public GameObject pauseMenu;
    public GameObject levelSelectMenu;
    PlayerController player;
    public GameObject[] lvlButtons;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    public GameObject otherUI;
    public float textDuration;
    public Animator tutAnim;
    public TextMeshProUGUI tutorialText;
    public string[] tutorialStrings;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume", 10f));
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 10f);
        Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1 ? true : false;
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1 ? true : false;
        otherUI.SetActive(true);
        tutAnim.SetBool("Show", true);
        Pause();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) | Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            TutWait();
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        clickSound.Play();
        Time.timeScale = 0f;
        isPaused = true;
        otherUI.SetActive(false);
        menuMusic.enabled = true;
    }
    public void Resume()
    {
        LvlSelBack();
        OptionsBack();
        pauseMenu.SetActive(false);
        clickSound.Play();
        Time.timeScale = 1f;
        isPaused = false;
        otherUI.SetActive(true);
        player.startedYet = true;
        menuMusic.enabled = false;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Options()
    {
        optionsMenu.SetActive(true);
        clickSound.Play();
    }
    public void OptionsBack()
    {
        optionsMenu.SetActive(false);
        clickSound.Play();
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }
    public void Fullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        clickSound.Play();

        if (fullscreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
    public void LevelSelect()
    {
        levelSelectMenu.SetActive(true);
        clickSound.Play();
    }
    public void LvlSelBack()
    {
        levelSelectMenu.SetActive(false);
        clickSound.Play();
    }
    public void UpdateButtons()
    {
        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i < PlayerPrefs.GetInt("BestLevel", 1))
            {
                lvlButtons[i].SetActive(true);
            }
            else
            {
                lvlButtons[i].SetActive(false);
            }
        }
    }
    public void FirstStar(string text)
    {
        tutorialText.text = text;
        tutAnim.SetBool("Show", true);

        Invoke(nameof(TutWait), textDuration);
    }
    public void ShowTutorial(int level)
    {
        tutorialText.text = tutorialStrings[level];
        if (tutorialStrings[level].Length != 0)
        {
            tutAnim.SetBool("Show", true);
            Invoke(nameof(TutWait), textDuration);
        }
    }
    public void TutWait()
    {
        tutAnim.SetBool("Show", false);
    }
    public void Level1()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 1;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level2()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 2;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level3()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 3;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level4()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 4;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level5()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 5;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level6()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 6;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level7()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 7;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level8()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 8;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level9()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 9;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level10()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 10;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level11()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 11;
        clickSound.Play();
        player.GetLevel();
    }
    public void Level12()
    {
        LvlSelBack();
        OptionsBack();
        Resume();
        player.currentLevel = 12;
        clickSound.Play();
        player.GetLevel();
    }
}
