using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    #region Saved Variables
    public List<Player> playersListRoster = new List<Player>();
    [SerializeField] public List<Equipment> _equipmentList = new List<Equipment>(); 
    public string TeamName;
    public bool IsPlayerTeam = false;

    #endregion
    #region Match Variables
    public bool hasPossession = false;
    public int Score = 0;
    #endregion
    private void Awake()
    {
       
    }
    private void Start()
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
    public List<Equipment> GetEquipment()
    {
        return _equipmentList;
    }
    public void CreateEquips()
    {
        _equipmentList.Clear();
        //Only create if has no save file-ADD THIS LATER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _equipmentList.Add(new Equipment { Index = 0, Name = "Helmet", Level = 0, ShotBoost = 0, InsBoost = 0, MidBoost = 0, OutBoost = 0 });
        _equipmentList.Add(new Equipment { Index = 1, Name = "Pads", Level = 0, ShotBoost = 0, InsBoost = 0, MidBoost = 0, OutBoost = 0 });
        _equipmentList.Add(new Equipment { Index = 2, Name = "Gloves", Level = 0, ShotBoost = 0, InsBoost = 0, MidBoost = 0, OutBoost = 0 });
        _equipmentList.Add(new Equipment { Index = 3, Name = "Shoes", Level = 0, ShotBoost = 0, InsBoost = 0, MidBoost = 0, OutBoost = 0 });
        _equipmentList.Add(new Equipment { Index = 4, Name = "CoachSuit", Level = 0, ShotBoost = 0, InsBoost = 0, MidBoost = 0, OutBoost = 0 });
        print(name + " " + _equipmentList.Count + "TEAM EQUIPMENTS");
    }
}
[System.Serializable]
public class Equipment
{
    public int Index;//Number of the event
    public string Name; // Name of the equipment
    public int Level;
    public int ShotBoost;
    public int InsBoost;
    public int MidBoost;
    public int OutBoost;

}
