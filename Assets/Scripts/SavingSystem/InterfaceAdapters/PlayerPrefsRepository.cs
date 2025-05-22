using UnityEngine;
using Newtonsoft.Json;

public class PlayerPrefsRepository : IPlayerStatsRepository
{
    private const string KEY = "PlayerStats";

    public PlayerStats Load()
    {
        var json = PlayerPrefs.GetString(KEY, "");

        if (string.IsNullOrEmpty(json) || json == "{}")
        {
           
            return new PlayerStats(200, 50, 5f, 8f, new Vector3(0, 0, 0));
        }

        var stats = JsonConvert.DeserializeObject<PlayerStats>(json);
        if (stats == null)
            stats = new PlayerStats(200, 50, 5f, 8f, new Vector3(0, 0, 0));

        Debug.Log("Loaded PlayerStats JSON: " + json);
        Debug.Log($"Position loaded: x={stats.Position.x}, y={stats.Position.y}, z={stats.Position.z}");


        return stats;
    }


    public void Save(PlayerStats stats)
    {
        var json = JsonConvert.SerializeObject(stats);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
        Debug.Log("Saving PlayerStats JSON: " + json);
    }
}
