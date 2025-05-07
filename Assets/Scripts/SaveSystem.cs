using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class SaveSystem : MonoBehaviour
{
    private static string filePath => Application.persistentDataPath + "/playerdata.json";

    public static void Save(PlayerData playerData)
    {
        PlayerSaveDataList dataList = LoadAll(); // Load existing list

        // Check if this player already exists
        var existing = dataList.players.FirstOrDefault(p => p.playerName == playerData.PlayerName);
        if (existing != null)
        {
            // Update score if it's higher
            existing.highScore = Mathf.Max(existing.highScore, playerData.HighScore);
        }
        else
        {
            // Add new entry
            dataList.players.Add(new PlayerSaveData
            {
                playerName = playerData.PlayerName,
                highScore = playerData.HighScore
            });
        }

        string json = JsonUtility.ToJson(dataList, true);
        File.WriteAllText(filePath, json);
    }

    public static void Load(PlayerData playerData)
    {
        PlayerSaveDataList dataList = LoadAll();

        // Find the player in the loaded data
        var found = dataList.players.FirstOrDefault(p => p.playerName == playerData.PlayerName);

        if (found != null)
        {
            // Player found, set their name and high score
            playerData.PlayerName = found.playerName;
            playerData.HighScore = found.highScore;
        }
        else
        {
            // Player not found, keep the default values
            Debug.Log("Player not found, loading defaults.");
        }
    }

    public static PlayerSaveDataList LoadAll()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerSaveDataList>(json);
        }

        return new PlayerSaveDataList(); // Return an empty list if no file exists
    }
}

[System.Serializable]
public class PlayerSaveData
{
    public string playerName;
    public int highScore;
}

[System.Serializable]
public class PlayerSaveDataList
{
    public List<PlayerSaveData> players = new List<PlayerSaveData>();
}
