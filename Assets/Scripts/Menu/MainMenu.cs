using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider slider;

    public AudioClip clip;
    public AudioSource musicSource;

    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private string gameScene = "Demo";

    [SerializeField] private PlayerStatsController statsController;
    [SerializeField] private string menuSceneName = "MainMenu";


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
        Debug.Log(slider.value);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveAndLoadMenu();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameScene);
    }

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

    private void SaveAndLoadMenu()
    {
        Debug.Log("SaveAndLoadMenu called");
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

