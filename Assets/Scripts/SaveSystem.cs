using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string GetSavePath(string teamName)
    {
        return Path.Combine(Application.persistentDataPath, $"{teamName}_teamData.json");
    }

    // Save team data as JSON file
    public void SaveTeam(Team team)
    {
        TeamData teamData = new TeamData(team);

        string json = JsonUtility.ToJson(teamData, true);
        string filePath = GetSavePath(team.TeamName);

        // Write the JSON data to a file
        File.WriteAllText(filePath, json);

        //Debug.Log($"Team {team.name} saved to {filePath}");
    }

    // Load team data from JSON file
    public void LoadTeam(Team team)
    {
        
        string filePath = GetSavePath(team.TeamName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            TeamData teamData = JsonUtility.FromJson<TeamData>(json);

            // Clear existing roster and repopulate from saved data
            team.playersListRoster.Clear();
            team.ClearAllPlayers();
            foreach (PlayerData playerData in teamData.playersListData)
            {
                Player newPlayer = new GameObject().AddComponent<Player>();
                newPlayer.playerFirstName = playerData.firstName;
                newPlayer.ovr = playerData.ovr;
                newPlayer.Inside = playerData.inside;
                newPlayer.Mid = playerData.mid;
                newPlayer.Outside = playerData.outside;
                team.playersListRoster.Add(newPlayer);
            }

            Debug.Log($"Team {team.TeamName} loaded successfully from {filePath}");
        }
        else
        {
            Debug.LogError($"No save file found for {team.TeamName} at {filePath}");
        }
        
    }

    // Clear saved data by deleting the JSON file
    public void ClearSave(string teamName, Team team)
    {
        string filePath = GetSavePath(teamName);

        if (File.Exists(filePath))
        {
            team.playersListRoster.Clear();
            File.Delete(filePath);
            Debug.Log($"Save file for team {teamName} has been deleted.");
        }
        else
        {
            Debug.LogError($"No save file found for team {teamName} to delete.");
        }
    }
    public void ClearAllSaves()
    {
        string saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");

        // Check if the save directory exists
        if (Directory.Exists(saveDirectory))
        {
            // Delete all files in the directory
            Directory.Delete(saveDirectory, true);
            Debug.Log("All saves have been cleared.");
        }
        else
        {
            Debug.LogWarning("No save directory found to clear.");
        }
    }
}
