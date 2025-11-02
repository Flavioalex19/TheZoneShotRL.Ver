using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    
}
[System.Serializable]
public class PlayerData
{
    public string firstName;
    public string secondName;
    public int playerAge;
    public int shooting;
    public int inside;
    public int mid;
    public int outside;
    public int defend;
    public int guard;
    public int steal;
    public int juk;
    public int cosis;
    public int control;
    public int pos;
    public float ovr;
    public int awn;
    public int yearsC;
    public int salaryPlayer;
    public int persona;
    public int imageIndex;
    public int jNum;
    public int zone;
    public int c_gamesPlayed;
    public int c_pointsMade;
    public int c_steals;
    public int c_fieldGoal;
    public int c_fieldGoalsMade;
    public int b_buff;
    public Player p_bond;
    

    public PlayerData(Player player)
    {
        firstName = player.playerFirstName;
        secondName = player.playerLastName;
        playerAge = player.Age;
        shooting = player.Shooting;
        inside = player.Inside;
        mid = player.Mid;
        outside = player.Outside;
        defend = player.Defending;
        guard = player.Guarding;
        steal = player.Stealing;
        juk = player.Juking;
        cosis = player.Consistency;
        control = player.Control;
        pos = player.Positioning;
        ovr = player.ovr;
        awn = player.Awareness;
        yearsC = player.ContractYears;
        salaryPlayer = player.Salary;
        persona = player.Personality;
        imageIndex = player.ImageCharacterPortrait;
        jNum = player.J_Number;
        zone = player.Zone;
        c_gamesPlayed = player.CareerGamesPlayed;
        c_pointsMade = player.CareerPoints;
        c_steals = player.CareerSteals;
        c_fieldGoal = player.CareerFieldGoalAttempted;
        c_fieldGoalsMade = player.CareerFieldGoalMade;
        b_buff = player.buff;
        p_bond = player.bondPlayer;
        
        
    }
}

[System.Serializable]
public class TeamData
{
    public List<PlayerData> playersListData = new List<PlayerData>();
    public string teamName;
    public bool isPlayerControlled;
    public List<EquipmentData> equiList = new List<EquipmentData>();
    public int teamMoral;
    public int teamFrontOffice;
    public int teamFansSupport;
    public List<Team> teamSchedule = new List<Team>();
    public List<string> scheduleTeamNames;
    public LeagueManagerData leagueData;
    public int win;
    public int lost;
    public int draw;
    public int cap;
    public int trainingP;
    public int tradindP;

    public TeamData(Team team, LeagueManager leagueManager)
    {
        teamName = team.TeamName;  // Assuming Team script has a team name
        isPlayerControlled = team.IsPlayerTeam;
        teamMoral = team.Moral;
        teamFrontOffice = team.FrontOfficePoints;
        teamFansSupport = team.FansSupportPoints;
        win = team.Wins;
        lost = team.Loses;
        draw = team.Draws;
        cap = team.CurrentSalary;
        trainingP = team.TrainingPower;
        tradindP = team.TradingPower;
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
        //teamSchedule = team.GetSchedule();
        //scheduleTeamNames = team._schedule.Select(t => t.TeamName).ToList();
        scheduleTeamNames = new List<string>();
        foreach (var opponent in team._schedule)
        {
            scheduleTeamNames.Add(opponent.TeamName);
        }
        //leagueData = new LeagueManagerData(FindObjectOfType<LeagueManager>());
        leagueData = new LeagueManagerData(leagueManager);
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
[System.Serializable]
public class LeagueManagerData
{
    public int weekNumber;
    public bool canGenEvent;
    public bool canStartANewWeek;
    public bool canTradePlayers;
    public bool canTrainPlayer;
    public bool canStartTutorial;
    public bool canNegociateContract;
    public LeagueManagerData(LeagueManager leagueManager)
    {
        weekNumber = leagueManager.Week;
        canGenEvent = leagueManager.canGenerateEvents;
        canStartANewWeek = leagueManager.canStartANewWeek;
        canTradePlayers = leagueManager.canTrade;
        canTrainPlayer = leagueManager.canTrain;
        canStartTutorial = leagueManager.CanStartTutorial;
        canNegociateContract = leagueManager.canNegociateContract;
    }
}
