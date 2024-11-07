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
    public int ovr;

    public PlayerData(Player player)
    {
        firstName = player.playerFirstName;  // Assuming Player script has name set as firstName
        ovr = player.ovr;
    }
}

[System.Serializable]
public class TeamData
{
    public List<PlayerData> playersListData = new List<PlayerData>();
    public string teamName;

    public TeamData(Team team)
    {
        teamName = team.TeamName;  // Assuming Team script has a team name
        foreach (Player player in team.playersListRoster)
        {
            playersListData.Add(new PlayerData(player));
        }
    }
}
