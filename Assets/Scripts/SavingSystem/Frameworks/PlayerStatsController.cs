using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{

    private PlayerStats _stats;
    private SavePlayerStatsInteractor _saveInteractor;
    private LoadPlayerStatsInteractor _loadInteractor;
    private PlayerPrefsRepository repository;

    private void Awake()
    {
        repository = new PlayerPrefsRepository();
        _saveInteractor = new SavePlayerStatsInteractor(repository);
        _loadInteractor = new LoadPlayerStatsInteractor(repository);

        int loadFromSave = PlayerPrefs.GetInt("LoadFromSave", 0);
        if (loadFromSave == 1)
        {
            _stats = _loadInteractor.Execute();


        }
        else
        {
            _stats = CreateNewStats();
            Debug.Log("New Game started.");
        }
    }
    private void Start()
    {
        int loadFromSave = PlayerPrefs.GetInt("LoadFromSave", 0);

        if (_stats != null)
        {
            LoadFromSave();

        }

        if (loadFromSave == 1)
        {
            PlayerPrefs.SetInt("LoadFromSave", 0);
        }
    }



    private PlayerStats CreateNewStats()
    {
        return new PlayerStats(200, 50, 5f, 8f, Vector3.zero);
    }
    public GameObject player;

    public void LoadFromSave()
    {
        PlayerStats loadedStats = repository.Load();
        Vector3 pos = loadedStats.GetPosition();

        Debug.Log($"Position loaded: x={pos.x}, y={pos.y}, z={pos.z}");

        if (player != null)
        {
            var controller = player.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            player.transform.position = pos;
            _stats.Score = loadedStats.Score;

            if (controller != null) controller.enabled = true;
        }
    }



    public float GetCurrentHP()
    {
        if (_stats == null)
        {

            return 200;
        }

        return _stats.CurrentHP;
    }



    public void SavePosition(Vector3 position)
    {
        if (_stats == null)
        {
            Debug.LogError("SavePosition: _stats is null!");
            return;
        }
        _stats.SetPosition(position);
    }

    public bool IsReady()
    {
        return _stats != null;
    }

    public Vector3 LoadPosition()
    {
        return _stats.GetPosition();
    }



    public void SaveHPMP()
    {
        _saveInteractor.Execute(_stats);
    }
    public void SaveAll()
    {
        if (_stats == null)
        {
            Debug.LogError("SaveAll: _stats is null! Save aborted.");
            return;
        }

        SavePosition(transform.position);
        SaveHPMP();
        PlayerPrefs.SetInt("LoadFromSave", 1);
        PlayerPrefs.Save();
        Debug.Log("Game saved.");
    }



    public void SetCurrentHP(float hp)
    {
        _stats.CurrentHP = hp;
    }

    public void AddScore(float points)
    {
        if (_stats != null)
            _stats.Score += points;
    }

    public float GetScore()
    {
        return _stats?.Score ?? 0f;
    }

    public void SetScore(float score)
    {
        if (_stats != null)
            _stats.Score = score;
    }

    public bool GetPeacefulMode()
    {
        if (_stats == null) return false;
        return _stats.IsPeacefulMode;
    }

    public void SetPeacefulMode(bool value)
    {
        if (_stats != null)
        {
            _stats.IsPeacefulMode = value;
        }
    }


}
