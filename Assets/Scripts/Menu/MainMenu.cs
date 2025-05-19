using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider slider;

    public AudioClip clip;
    public AudioSource musicSource;

    [SerializeField] private PlayerStatsController statsController;
    [SerializeField] private string menuSceneName = "MainMenu";

    void Start()
    {
        if (slider != null && musicSource != null)
        {
            // Установим начальную громкость
            musicSource.volume = slider.value;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveAndLoadMenu();
        }
    }

    void SaveAndLoadMenu()
    {
        if (statsController != null && statsController.IsReady())
        {
            statsController.SavePosition(statsController.transform.position);
            statsController.SaveHPMP();
        }
        else
        {
            Debug.LogWarning("MenuManager: statsController not ready.");
        }

        SceneManager.LoadScene(menuSceneName);
    }
    

   
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnLoadGame()
    {
        PlayerPrefs.SetInt("LoadFromSave", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Demo");
    }


    public void OnNewGame()
    {
        PlayerPrefs.SetInt("LoadFromSave", 0);
        PlayerPrefs.DeleteKey("PlayerStats");  
        PlayerPrefs.Save();
        SceneManager.LoadScene("Demo");
    }



    public void SetVolume(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
            Debug.Log("Volume set to: " + value);
        }
    }

}
