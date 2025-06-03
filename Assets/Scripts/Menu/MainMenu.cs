using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider slider;
    public AudioSource musicSource;
    public AudioClip clip;

    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Toggle peacefulModeToggle;

    [SerializeField] private string gameScene = "Demo";
    [SerializeField] private PlayerStatsController statsController;


    [SerializeField] private Toggle peacefulModeToggle;

    void Start()
    {
        if (statsController != null)
        {
            bool peacefulMode = statsController.GetPeacefulMode();
            peacefulModeToggle.isOn = peacefulMode;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("SaveAndLoadMenu called");
        }
    }

    public void PlayGame() => SceneManager.LoadScene(gameScene);

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetVolume(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
            Debug.Log("Volume set to: " + value);
        }
    }

    public void OnLoadGame()
    {
        PlayerPrefs.SetInt("LoadFromSave", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameScene);
    }

    public void OnNewGame()
    {
        PlayerPrefs.SetInt("LoadFromSave", 0);
        PlayerPrefs.DeleteKey("PlayerStats");
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameScene);
    }

    public void OnPeacefulModeToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("PeacefulMode", isOn ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Saved PeacefulMode = " + isOn);
    }

    public void OnPeacefulModeToggleChanged(bool isOn)
    {
        if (statsController != null)
        {
            statsController.SetPeacefulMode(isOn);
            statsController.SaveAll();
        }
    }

}

