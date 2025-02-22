using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    
}
[System.Serializable]
public class PlayerData
{
    public string firstName;
    public int shooting;
    public int inside;
    public int mid;
    public int outside;
    public float ovr;
    public int awn;
    public int persona;

    public PlayerData(Player player)
    {
        firstName = player.playerFirstName;
        shooting = player.Shooting;
        inside = player.Inside;
        mid = player.Mid;
        outside = player.Outside;
        ovr = player.ovr;
        awn = player.Awareness;
        persona = player.Personality;
    }
}

[System.Serializable]
public class TeamData
{
    public List<PlayerData> playersListData = new List<PlayerData>();
    public string teamName;
    public bool isPlayerControlled;
    public List<EquipmentData> equiList = new List<EquipmentData>();

    public TeamData(Team team)
    {
        teamName = team.TeamName;  // Assuming Team script has a team name
        isPlayerControlled = team.IsPlayerTeam;
        foreach (Player player in team.playersListRoster)
        {
            playersListData.Add(new PlayerData(player));
        }
        if (team.IsPlayerTeam) // Only save equipment for player-controlled team
        {
            equiList = new List<EquipmentData>(); // Ensure list is initialized
            foreach (Equipment equipment in team.GetEquipment())
            {
                equiList.Add(new EquipmentData(equipment));
            }
        }
    }
}
[System.Serializable]
public class EquipmentData
{
    public int indexNumber;//Number of the event
    public string equipName; // Name of the equipment
    public int lvl;
    public int ShotB;
    public int InsB;
    public int MidB;
    public int OutB;
    public EquipmentData(Equipment equipment)
    {
        indexNumber = equipment.Index;
        equipName = equipment.Name;
        lvl = equipment.Level;
        ShotB = equipment.ShotBoost;
        InsB = equipment.InsBoost;
        MidB = equipment.MidBoost; 
        OutB = equipment.OutBoost;
    }
}
