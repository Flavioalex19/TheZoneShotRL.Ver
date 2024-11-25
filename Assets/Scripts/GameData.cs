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
    public int inside;
    public int mid;
    public int outside;
    public float ovr;

    public PlayerData(Player player)
    {
        firstName = player.playerFirstName;
        inside = player.Inside;
        mid = player.Mid;
        outside = player.Outside;
        ovr = player.ovr;
    }
}

[System.Serializable]
public class TeamData
{
    public List<PlayerData> playersListData = new List<PlayerData>();
    public string teamName;
    public bool isPlayerControlled;

    public TeamData(Team team)
    {
        teamName = team.TeamName;  // Assuming Team script has a team name
        isPlayerControlled = team.IsPlayerTeam;
        foreach (Player player in team.playersListRoster)
        {
            playersListData.Add(new PlayerData(player));
        }
    }
}
