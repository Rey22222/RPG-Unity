using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    [Header("Generation Settings")]
    public int killsToSpawnBoss = 3;
    public int killsToWin = 5;
    public TMP_Text scoreText;

    [Header("Links")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] Transform bossSpawnPoint;
    [SerializeField] AudioClip victoryMusic;
    [SerializeField] AudioSource audioSource;

    private int killCount = 0;
    private float score = 0;
    private bool bossSpawned = false;
    private bool victoryPlayed = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterKill(float points)
    {
        killCount++;
        score += points;

        if (scoreText != null)
            scoreText.text = $"Score: {score}";

        if (!bossSpawned && killCount >= killsToSpawnBoss)
            SpawnBoss();

        if (!victoryPlayed && killCount >= killsToWin)
            PlayVictory();
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossSpawned = true;
        }
        else
        {
            Debug.LogWarning("Boss spawn data not set!");
        }
    }

    private void PlayVictory()
    {
        if (victoryMusic != null && audioSource != null)
        {
            audioSource.clip = victoryMusic;
            audioSource.Play();
            Debug.Log("Victory music played!");
            victoryPlayed = true;
        }
        else
        {
            Debug.LogWarning("Victory music or AudioSource not set!");
        }
    }
}

