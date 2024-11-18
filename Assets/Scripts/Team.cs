using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{

    public List<Player> playersListRoster = new List<Player>();

    public string TeamName;
    public bool IsPlayerTeam = false;

    private void Awake()
    {
       
    }
    // Function to add a player to the roster
    public void AddPlayerToRoster(Player newPlayer)
    {
        if (playersListRoster.Count < 12) // Assuming a max of 12 players per team
        {
            playersListRoster.Add(newPlayer);
            Debug.Log($"Player {newPlayer.name} added to the roster.");
        }
        else
        {
            Debug.Log("Roster is full. Cannot add more players.");
        }
    }
    // Method to clear all player data
    public void ClearAllPlayers()
    {
        playersListRoster.Clear();
    }
    public void ActivatePlayerTeam()
    {
        IsPlayerTeam = true;
    }
}
