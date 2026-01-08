using DG.Tweening.Core.Easing;
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
    /*
    public void SaveTeam(Team team)
    {
        LeagueManager leagueManager = FindObjectOfType<LeagueManager>();
        TeamData teamData = new TeamData(team, leagueManager);
        

        string json = JsonUtility.ToJson(teamData, true);
        string filePath = GetSavePath(team.TeamName);

        // Write the JSON data to a file
        File.WriteAllText(filePath, json);

        //Debug.Log($"Team {team.name} saved to {filePath}");
    }
    */
    public void SaveTeamInfo(Team team)
    {
        LeagueManager leagueManager = FindObjectOfType<LeagueManager>();
        TeamData teamData = new TeamData(team,leagueManager);

        string json = JsonUtility.ToJson(teamData, true);
        string filePath = GetSavePath(team.TeamName);

        File.WriteAllText(filePath, json);
    }
    public void SaveLeague()
    {
        LeagueManager leagueManager = FindObjectOfType<LeagueManager>();

        LeagueManagerData data = new LeagueManagerData(leagueManager);

        string path = Path.Combine(
            Application.persistentDataPath,
            "leagueData.json"
        );

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }
    // Load team data from JSON file
    /*
    public void LoadTeam(Team team, GameObject playerPrefab)
    {
        
        string filePath = GetSavePath(team.TeamName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            TeamData teamData = JsonUtility.FromJson<TeamData>(json);
            LeagueManagerData leagueManagerData = JsonUtility.FromJson<LeagueManagerData>(json);//////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            team.IsPlayerTeam = false;
            team.Moral = 0;
            team.FrontOfficePoints = 0;
            team.FansSupportPoints = 0;
            team.EffortPoints = 0;
            team.Wins = 0;
            team.Draws = 0;
            team.Loses = 0;
            team.CurrentSalary = 0;
            team.FinancesLvl = 0;
            team.OfficeLvl = 0;
            team.MarketingLvl = 0;
            team.isR4 = false;
            team.isR8 = false;
            team.isFinalist = false;
            team.isChampion = false;

            team.IsPlayerTeam = teamData.isPlayerControlled;
            team.Moral = teamData.teamMoral;
            team.FrontOfficePoints = teamData.teamFrontOffice;
            team.FansSupportPoints = teamData.teamFansSupport;
            team.EffortPoints = teamData.teamEffort;
            team.Wins = teamData.win;
            team.Draws = teamData.draw;
            team.Loses = teamData.lost;
            team.CurrentSalary = teamData.cap;
            team.FinancesLvl = teamData.finances;
            team.OfficeLvl = teamData.offices;
            team.MarketingLvl = teamData.market;
            team.TeamEquipmentLvl = teamData.equip;
            team.ArenaLvl = teamData.arena;
            team.MedicalLvl = teamData.medical;
            team.isR4 = teamData.isTop4;
            team.isR8 = teamData.isTop8;
            team.isFinalist = teamData.FinalTeam;
            team.isChampion = teamData.Champion;

            

            // Clear existing roster and repopulate from saved data
            team.playersListRoster.Clear();
            team._equipmentList.Clear();
            team.ClearAllPlayers();
            if (team.IsPlayerTeam) print("THS IS THE PLAYERS TEAM" + " " + team.TeamName);
            //Load Equipment
            if (team.IsPlayerTeam )
            {
                foreach (EquipmentData equipData in teamData.equiList)
                {
                    
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
                GameObject playerGO = Instantiate(playerPrefab);
                //Player newPlayer = new GameObject().AddComponent<Player>();
                Player newPlayer = playerGO.AddComponent<Player>();

                newPlayer.playerFirstName = playerData.firstName;
                newPlayer.playerLastName = playerData.secondName;
                newPlayer.Age = playerData.playerAge;
                newPlayer.ovr = playerData.ovr;
                newPlayer.Shooting = playerData.shooting;
                newPlayer.Inside = playerData.inside;
                newPlayer.Mid = playerData.mid;
                newPlayer.Outside = playerData.outside;
                newPlayer.Defending = playerData.defend;
                newPlayer.Guarding = playerData.guard;
                newPlayer.Stealing = playerData.steal;
                newPlayer.Awareness = playerData.awn;
                newPlayer.Juking = playerData.juk;
                newPlayer.Consistency = playerData.cosis;
                newPlayer.Control = playerData.control;
                newPlayer.Positioning = playerData.pos;
                newPlayer.ContractYears = playerData.yearsC;
                newPlayer.Salary = playerData.salaryPlayer;
                newPlayer.Personality = playerData.persona;
                newPlayer.ImageCharacterPortrait = playerData.imageIndex;
                newPlayer.J_Number = playerData.jNum;
                newPlayer.Zone = playerData.zone;
                newPlayer.CareerGamesPlayed = playerData.c_gamesPlayed;
                newPlayer.CareerPoints = playerData.c_pointsMade;
                newPlayer.CareerSteals = playerData.c_steals;
                newPlayer.CareerFieldGoalAttempted = playerData.c_fieldGoal;
                newPlayer.CareerFieldGoalMade = playerData.c_fieldGoalsMade;
                newPlayer.buff = playerData.b_buff;
                newPlayer.bondPlayer = playerData.p_bond;
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
                    leagueManager.canTrain = teamData.leagueData.canTrainPlayer;
                    leagueManager.CanStartTutorial = teamData.leagueData.canStartTutorial;
                    leagueManager.canNegociateContract = teamData.leagueData.canNegociateContract;
                    leagueManager.isOnR4 = teamData.leagueData.isr4;
                    leagueManager.isOnR8 = teamData.leagueData.isr8;
                    leagueManager.isOnFinals = teamData.leagueData.isFinal;

                    //PLayoffs lists
                    leagueManager.List_R8Teams.Clear();
                    foreach (string name in leagueManagerData.R8Names)
                    {
                        Team t = FindTeamByName(name);
                        if (t != null) leagueManager.List_R8Teams.Add(t);
                    }

                    leagueManager.List_R4Teams.Clear();
                    foreach (string name in leagueManagerData.R4Names)
                    {
                        Team t = FindTeamByName(name);
                        if (t != null) leagueManager.List_R4Teams.Add(t);
                    }

                    leagueManager.List_Finalist.Clear();
                    foreach (string name in leagueManagerData.FinalNames)
                    {
                        Team t = FindTeamByName(name);
                        if (t != null) leagueManager.List_Finalist.Add(t);
                    }
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
    */
    public void LoadLeague()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "leagueData.json"
        );

        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        LeagueManagerData data = JsonUtility.FromJson<LeagueManagerData>(json);

        LeagueManager lm = FindObjectOfType<LeagueManager>();

        lm.Week = data.weekNumber;
        lm.canGenerateEvents = data.canGenEvent;
        lm.canStartANewWeek = data.canStartANewWeek;
        lm.canTrade = data.canTradePlayers;
        lm.canTrain = data.canTrainPlayer;
        lm.CanStartTutorial = data.canStartTutorial;
        lm.canNegociateContract = data.canNegociateContract;
        lm.isGameOver = data.isGO;

        lm.isOnR8 = data.isr8;
        lm.isOnR4 = data.isr4;
        lm.isOnFinals = data.isFinal;
        lm.CanStartANewRun = data.canStartNewRun;

        lm.CanDraftlvl1 = data.draftLvl1;
        lm.CanDraftlvl2 = data.draftLvl2;
        lm.CanDraftlvl3 = data.draftLvl3;
        lm.CanDraftSpPlayer1 = data.spPlayer1;
        lm.CanDrafSpPlayer0 = data.spPlayer0;

        // ---------- PLAYOFFS ----------
        lm.List_R8Teams.Clear();
        foreach (string name in data.R8Names)
        {
            Team t = FindTeamByName(name);
            if (t != null) lm.List_R8Teams.Add(t);
        }

        lm.List_R4Teams.Clear();
        foreach (string name in data.R4Names)
        {
            Team t = FindTeamByName(name);
            if (t != null) lm.List_R4Teams.Add(t);
        }

        lm.List_Finalist.Clear();
        foreach (string name in data.FinalNames)
        {
            Team t = FindTeamByName(name);
            if (t != null) lm.List_Finalist.Add(t);
        }
    }
    public void LoadTeamInfo(Team team, GameObject playerPrefab)
    {
        string filePath = GetSavePath(team.TeamName);
        if (!File.Exists(filePath)) return;

        string json = File.ReadAllText(filePath);
        TeamData teamData = JsonUtility.FromJson<TeamData>(json);

        // ---------- RESET ----------
        team.playersListRoster.Clear();
        team._equipmentList.Clear();
        team.ClearAllPlayers();

        team.IsPlayerTeam = teamData.isPlayerControlled;
        team.Moral = teamData.teamMoral;
        team.FrontOfficePoints = teamData.teamFrontOffice;
        team.FansSupportPoints = teamData.teamFansSupport;
        team.EffortPoints = teamData.teamEffort;

        team.Wins = teamData.win;
        team.Draws = teamData.draw;
        team.Loses = teamData.lost;

        team.CurrentSalary = teamData.cap;

        team.OfficeLvl = teamData.offices;
        team.FinancesLvl = teamData.finances;
        team.MarketingLvl = teamData.market;
        team.TeamEquipmentLvl = teamData.equip;
        team.ArenaLvl = teamData.arena;
        team.MedicalLvl = teamData.medical;

        team.isR8 = teamData.isTop8;
        team.isR4 = teamData.isTop4;
        team.isFinalist = teamData.FinalTeam;
        team.isChampion = teamData.WinC;

        // ---------- LOAD DOS JOGADORES ----------
        foreach (PlayerData pd in teamData.playersListData)
        {
            GameObject go = Instantiate(playerPrefab);
            Player p = go.AddComponent<Player>();

            p.playerFirstName = pd.firstName;
            p.playerLastName = pd.secondName;
            p.Age = pd.playerAge;
            p.ovr = pd.ovr;

            p.Shooting = pd.shooting;
            p.Inside = pd.inside;
            p.Mid = pd.mid;
            p.Outside = pd.outside;

            p.Defending = pd.defend;
            p.Guarding = pd.guard;
            p.Stealing = pd.steal;

            p.Awareness = pd.awn;
            p.Juking = pd.juk;
            p.Consistency = pd.cosis;
            p.Control = pd.control;
            p.Positioning = pd.pos;

            p.ContractYears = pd.yearsC;
            p.Salary = pd.salaryPlayer;

            p.Personality = pd.persona;
            p.ImageCharacterPortrait = pd.imageIndex;
            p.J_Number = pd.jNum;

            p.Zone = pd.zone;
            p.CurrentZone = 0;

            p.CareerGamesPlayed = pd.c_gamesPlayed;
            p.CareerPoints = pd.c_pointsMade;
            p.CareerSteals = pd.c_steals;

            // AQUI O JOGADOR ENTRA NO TIME 
            team.playersListRoster.Add(p);
        }

        // ---------- LOAD DO SCHEDULE ----------
        team._schedule.Clear();
        foreach (string teamName in teamData.scheduleTeamNames)
        {
            Team opp = FindTeamByName(teamName);
            if (opp != null && opp != team)
                team._schedule.Add(opp);
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
            team.isR8 = false;
            team.isR4 = false;
            team.isFinalist = false;
            team.isChampion = false;
            team.OfficeLvl = 0;
            team.FinancesLvl = 0;
            team.MarketingLvl = 0;
            team.TeamEquipmentLvl = 0;
            team.ArenaLvl = 0;
            team.MedicalLvl = 0;
            team.SalaryCap = team.SalaryCapReset;
            
            File.Delete(filePath);
            Debug.Log($"Save file for team {teamName} has been deleted.");
        }
        else
        {
            //Debug.LogError($"No save file found for team {teamName} to delete.");
        }
    }
    public void ResetLeagueData()
    {
        LeagueManager leagueManager = FindObjectOfType<LeagueManager>();


        if (leagueManager == null)
        {
            Debug.LogError("LeagueManager not found.");
            return;
        }

        // --------- RESET DE PROGRESSO DA LIGA ---------
        leagueManager.Week = 0;

        leagueManager.canGenerateEvents = true;
        leagueManager.canStartANewWeek = true;
        leagueManager.canTrade = true;
        leagueManager.canTrain = true;
        leagueManager.canNegociateContract = true;

        leagueManager.isOnR8 = false;
        leagueManager.isOnR4 = false;
        leagueManager.isOnFinals = false;
        

        // --------- RESET DE PLAYOFFS ---------
        leagueManager.List_R8Teams.Clear();
        leagueManager.List_R4Teams.Clear();
        leagueManager.List_Finalist.Clear();

        leagueManager.List_R8Names.Clear();
        leagueManager.List_R4Names.Clear();
        //leagueManager.List_FinalNames.Clear();

        

        Debug.Log("League data reset and saved successfully.");
    }
    public void ResetForNewLeagueRun()
    {
        LeagueManager leagueManager = FindObjectOfType<LeagueManager>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (leagueManager == null)
        {
            Debug.LogError("LeagueManager not found.");
            return;
        }
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            //gameManager.leagueTeams[i].isChampion = false;
        }
        leagueManager.isGameOver = false;
        leagueManager.CanStartANewRun = true;
        gameManager.saveSystem.SaveLeague();
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            gameManager.saveSystem.ClearSave(gameManager.leagueTeams[i].TeamName, gameManager.leagueTeams[i]);
        }
        //SaveLeague();
    }
    /*
    public void ResetForNewRun()
    {
        LeagueManager leagueManager = FindObjectOfType<LeagueManager>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (leagueManager == null || gameManager == null)
        {
            Debug.LogError("Managers not found.");
            return;
        }

        // -------- RESET DA LIGA --------
        //leagueManager.Week = 1;

        leagueManager.isGameOver = false;
        leagueManager.CanStartANewRun = false;

        leagueManager.isOnR8 = false;
        leagueManager.isOnR4 = false;
        leagueManager.isOnFinals = false;

        leagueManager.canGenerateEvents = true;
        leagueManager.canStartANewWeek = true;
        leagueManager.canTrade = true;
        leagueManager.canTrain = true;
        leagueManager.canNegociateContract = true;

        // -------- RESET DOS PLAYOFFS --------
        leagueManager.List_R8Teams.Clear();
        leagueManager.List_R4Teams.Clear();
        leagueManager.List_Finalist.Clear();


        // -------- RESET DOS TIMES --------
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            Team team = gameManager.leagueTeams[i];

            //team.isChampion = false;
            team.isR8 = false;
            team.isR4 = false;
            team.isFinalist = false;

            gameManager.saveSystem.ClearSave(team.TeamName, team);
        }

        // -------- SALVA O ESTADO LIMPO --------
        gameManager.saveSystem.SaveLeague();

        Debug.Log("New League Run reset completed successfully.");
    }
    */
    public void ResetLeagueHistory()
    {
        LeagueManager leagueManager = FindObjectOfType<LeagueManager>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        leagueManager.CanDraftlvl1 = false;
        leagueManager.CanDraftlvl2 = false;
        leagueManager.CanDraftlvl3 = false;
        leagueManager.CanDrafSpPlayer0 = false;
        leagueManager.CanDraftSpPlayer1 = false;
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            gameManager.leagueTeams[i].isChampion = false;
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
