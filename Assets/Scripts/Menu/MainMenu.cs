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
    void Start()
    {
        
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
