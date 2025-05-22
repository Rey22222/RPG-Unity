using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider slider;

    public AudioClip clip;
    public AudioSource musicSource;
<<<<<<< Updated upstream
    void Start()
    {
        if (slider != null && musicSource != null)
        {
            // Установим начальную громкость
            musicSource.volume = slider.value;
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Demo");
=======

    [SerializeField] private PlayerStatsController statsController;
    [SerializeField] private string menuSceneName = "MainMenu";


    void Update()
    {
        Debug.Log(slider.value);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveAndLoadMenu();
        }
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

<<<<<<< Updated upstream
    public void SetVolume(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
            Debug.Log("Volume set to: " + value);
        }
    }
=======
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

>>>>>>> Stashed changes
}
