using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider slider;

    public AudioClip clip;
    public AudioSource musicSource;
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
    }

    // Update is called once per frame
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
}
