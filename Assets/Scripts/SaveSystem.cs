using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string GetSavePath(string teamName)
    {
        //return Path.Combine(Application.persistentDataPath, $"{teamName}_teamData.json");
        string filePath = Path.Combine(Application.persistentDataPath, $"{teamName}_teamData.json");
        Debug.Log($"Save path: {filePath}");  // Add debug log for verification
        return filePath;
    }

    // Save team data as JSON file
    public void SaveTeam(Team team)
    {
        LeagueManager leagueManager = FindObjectOfType<LeagueManager>();
        TeamData teamData = new TeamData(team, leagueManager);
        

        string json = JsonUtility.ToJson(teamData, true);
        string filePath = GetSavePath(team.TeamName);

        //Debug.Log($"Saving {team._equipmentList.Count} equipment items for {team.TeamName}");
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

            team.IsPlayerTeam = false;
            team.Moral = 0;
            team.FrontOfficePoints = 0;
            team.FansSupportPoints = 0;
            team.Wins = 0;
            team.Draws = 0;
            team.Loses = 0;

            team.IsPlayerTeam = teamData.isPlayerControlled;
            team.Moral = teamData.teamMoral;
            team.FrontOfficePoints = teamData.teamFrontOffice;
            team.FansSupportPoints = teamData.teamFansSupport;
            team.Wins = teamData.win;
            team.Draws = teamData.draw;
            team.Loses = teamData.lost;


            

            // Clear existing roster and repopulate from saved data
            team.playersListRoster.Clear();
            team._equipmentList.Clear();
            team.ClearAllPlayers();
            if (team.IsPlayerTeam) print("THS IS THE PLAYERS TEAM" + " " + team.TeamName);
            //Load Equipment
            if (team.IsPlayerTeam /*&& teamData.equiList != null && teamData.equiList.Count > 0/*&& teamData.equiList != null*/)
            {
                foreach (EquipmentData equipData in teamData.equiList)
                {
                    /*
                    Equipment newEquip = new Equipment
                    {
                        Index = equipData.indexNumber,
                        Name = equipData.equipName,
                        Level = equipData.lvl,
                        ShotBoost = equipData.ShotB,
                        InsBoost = equipData.InsB,
                        MidBoost = equipData.MidB,
                        OutBoost = equipData.OutB
                    };
                    team.GetEquipment().Add(newEquip);
                    print("THS IS THE Equip" + " " + newEquip.Name + " Level:" + newEquip.Level);
                    */
                    Equipment newEquip = new Equipment
                    {
                        Index = equipData.indexNumber,
                        Name = equipData.equipName,
                        Level = equipData.lvl,
                        ShotBoost = equipData.ShotB,
                        InsBoost = equipData.InsB,
                        MidBoost = equipData.MidB,
                        OutBoost = equipData.OutB
                    };
                    //team.GetEquipment().Add(newEquip);
                    //team._equipmentList.Add(newEquip);
                    //Debug.Log("Loaded Equip: " + newEquip.Name + " Level: " + newEquip.Level);
                    if (!team._equipmentList.Exists(e => e.Index == newEquip.Index))
                    {
                        team._equipmentList.Add(newEquip);
                        Debug.Log($"Loaded Equip: {newEquip.Name} Level: {newEquip.Level}");
                    }
                }
                // Check if the equipment list has been populated correctly
                if (team._equipmentList.Count > 0)
                {
                    Debug.Log($"Total Equipment Loaded: {team._equipmentList.Count}");
                }
                else
                {
                    Debug.LogWarning("No equipment loaded for the team.");
                }
                //print(team._equipmentList.Count + " Number of equips");
            }
            else
            {
                print("Has DATA");
            }
            //Load Players
            foreach (PlayerData playerData in teamData.playersListData)
            {
                Player newPlayer = new GameObject().AddComponent<Player>();
                newPlayer.playerFirstName = playerData.firstName;
                newPlayer.ovr = playerData.ovr;
                newPlayer.Shooting = playerData.shooting;
                newPlayer.Inside = playerData.inside;
                newPlayer.Mid = playerData.mid;
                newPlayer.Outside = playerData.outside;
                newPlayer.Awareness = playerData.awn;
                newPlayer.Personality = playerData.persona;
                team.playersListRoster.Add(newPlayer);
            }

            // Load schedule
            team._schedule.Clear();
            foreach (string teamName in teamData.scheduleTeamNames)
            {
                Team opponent = FindTeamByName(teamName); // Must return a valid Team from leagueTeams
                if (opponent != null && opponent != team)
                {
                    team._schedule.Add(opponent);
                }
            }

            if (team.IsPlayerTeam && teamData.leagueData != null)
            {
                LeagueManager leagueManager = FindObjectOfType<LeagueManager>();
                if (leagueManager != null)
                {
                    leagueManager.Week = teamData.leagueData.weekNumber;
                    leagueManager.canGenerateEvents = teamData.leagueData.canGenEvent;
                    leagueManager.canStartANewWeek = teamData.leagueData.canStartANewWeek;
                    leagueManager.canTrade = teamData.leagueData.canTradePlayers;
                }
            }


            Debug.Log($"Team {team.TeamName} loaded successfully from {filePath}");
            Debug.Log($"Loaded JSON for {team.TeamName}, equipment count: {teamData.equiList?.Count}");
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
            if (team.IsPlayerTeam)
            {
                team.IsPlayerTeam = false;
                team._equipmentList.Clear();
            }
            team.Wins = 0;
            team.Loses = 0;
            team.Draws = 0;

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
    private Team FindTeamByName(string name)
    {
        return FindObjectOfType<GameManager>().leagueTeams
            .FirstOrDefault(t => t.TeamName == name);
    }
}
