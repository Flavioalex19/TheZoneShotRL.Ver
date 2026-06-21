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
        LeagueManager leagueManager = FindFirstObjectByType<LeagueManager>();
        TeamData teamData = new TeamData(team,leagueManager);

        string json = JsonUtility.ToJson(teamData, true);
        string filePath = GetSavePath(team.TeamName);

        File.WriteAllText(filePath, json);
    }
    public void SaveLeague()
    {
        LeagueManager leagueManager = FindFirstObjectByType<LeagueManager>();

        LeagueManagerData data = new LeagueManagerData(leagueManager);

        string path = Path.Combine(
            Application.persistentDataPath,
            "leagueData.json"
        );

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }
    // Load team data from JSON file
    
    public void LoadLeague()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "leagueData.json"
        );

        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        LeagueManagerData data = JsonUtility.FromJson<LeagueManagerData>(json);

        LeagueManager lm = FindFirstObjectByType<LeagueManager>();

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
        lm.CanDraftSpPlayer2 = data.spPlayer2;
        lm.CanDraftSpPlayer3 = data.spPlayer3;
        lm.CanDraftSpPlayer4 = data.spPlayer4;
        lm.FacilitieBonus0 = data.fBonus0;
        lm.FacilitieBonus1 = data.fBonus1;

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
        /*
        team.FrontOfficePoints = teamData.teamFrontOffice;
        team.FansSupportPoints = teamData.teamFansSupport;
        team.EffortPoints = teamData.teamEffort;
        */
        team.CurrentBudget = teamData.teamBudget;

        team.Wins = teamData.win;
        team.Draws = teamData.draw;
        team.Loses = teamData.lost;
        team.WinningStreak = teamData.WStreak;
        team.LosingStreak = teamData.LSteak;


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
            //go.transform.SetParent(GameManager.transform);  // Ou um objeto persistente
            DontDestroyOnLoad(go);
            //Player p = new Player();

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
            p.MatchBuff = pd.matchBuff;
            p.ImageCharacterPortrait = pd.imageIndex;
            p.J_Number = pd.jNum;

            p.Zone = pd.zone;
            p.CurrentZone = 0;

            p.CareerGamesPlayed = pd.c_gamesPlayed;
            p.CareerPoints = pd.c_pointsMade;
            p.CareerSteals = pd.c_steals;
            p.CareerBlocks = pd.c_blocks;
            p.CareerFieldGoalAttempted = pd.c_fieldGoal;
            p.CareerFieldGoalMade = pd.c_fieldGoalsMade;
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
            //team.isChampion = false;
            team.OfficeLvl = 0;
            team.FinancesLvl = 0;
            team.MarketingLvl = 0;
            team.TeamEquipmentLvl = 0;
            team.ArenaLvl = 0;
            team.MedicalLvl = 0;
            team.SalaryCap = team.fixSalaryCap;
            
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
        LeagueManager leagueManager = FindFirstObjectByType<LeagueManager>();


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
        
        //SaveLeague();
        LeagueManager leagueManager = FindFirstObjectByType<LeagueManager>();
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (leagueManager == null || gameManager == null) return;

        leagueManager.isGameOver = false;
        leagueManager.CanStartANewRun = true;

        // Proteção inteligente: só recria se realmente precisar
        if (gameManager.leagueTeams.Count != gameManager.fullTeamList.Count ||
            gameManager.leagueTeams.Count == 0)
        {
            //gameManager.NewTeamsForRun();
        }
        else
        {
            Debug.Log("leagueTeams já possui a quantidade correta de times. Não foi recriado.");
        }

        gameManager.saveSystem.SaveLeague();
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
        LeagueManager leagueManager = FindFirstObjectByType<LeagueManager>();
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        leagueManager.CanDraftlvl1 = false;
        leagueManager.CanDraftlvl2 = false;
        leagueManager.CanDraftlvl3 = false;
        leagueManager.CanDrafSpPlayer0 = false;
        leagueManager.CanDraftSpPlayer1 = false;
        leagueManager.CanDraftSpPlayer2 = false;
        leagueManager.CanDraftSpPlayer3 = false;
        leagueManager.CanDraftSpPlayer4 = false;
        leagueManager.FacilitieBonus0 = false;
        leagueManager.FacilitieBonus1 = false;
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
        return FindFirstObjectByType<GameManager>().leagueTeams
            .FirstOrDefault(t => t.TeamName == name);
    }
    public void FullResetForNewLeagueRun()
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        LeagueManager lm = FindFirstObjectByType<LeagueManager>();

        if (gm == null)
        {
            Debug.LogError("GameManager não encontrado!");
            return;
        }

        Debug.Log("=== INICIANDO FULL RESET PARA NOVA RUN ===");

        // 1. Apagar todos os arquivos de save
        foreach (Team team in gm.leagueTeams)
        {
            if (team == null) continue;
            string filePath = GetSavePath(team.TeamName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"Save deletado: {team.TeamName}");
            }
        }

        // Apagar save da liga
        string leaguePath = Path.Combine(Application.persistentDataPath, "leagueData.json");
        if (File.Exists(leaguePath))
            File.Delete(leaguePath);

        // 2. Destruir todos os objetos Team e Players existentes na cena
        gm.DestroyAllWithTag("Team");
        gm.DestroyAllWithTag("ZsPlayer");     // assumindo que seus players têm essa tag

        // 3. Limpar a lista de times
        gm.leagueTeams.Clear();

        // 4. Reset básico do LeagueManager (apenas o essencial)
        if (lm != null)
        {
            lm.Week = 0;
            lm.canGenerateEvents = true;
            lm.canStartANewWeek = true;
            lm.canTrade = true;
            lm.canTrain = true;
            lm.canNegociateContract = true;
            lm.isGameOver = false;
            lm.CanStartANewRun = true;
            lm.isOnR8 = false;
            lm.isOnR4 = false;
            lm.isOnFinals = false;

            // Limpar listas de playoffs
            lm.List_R8Teams.Clear();
            lm.List_R4Teams.Clear();
            lm.List_Finalist.Clear();
            lm.List_R8Names.Clear();
            lm.List_R4Names.Clear();
        }

        // 5. Recriar os times do zero (a partir do fullTeamList)
       // gm.NewTeamsForRun();

        // 6. Salvar estado limpo da liga
        SaveLeague();

        Debug.Log("=== FULL RESET PARA NOVA RUN CONCLUÍDO ===");
        Debug.Log("Times recriados e saves apagados. Pronto para nova run.");
    }
    public void ClearLeagueProgress()
    {
        LeagueManager lm = FindFirstObjectByType<LeagueManager>();
        if (lm == null)
        {
            Debug.LogError("LeagueManager não encontrado!");
            return;
        }

        Debug.Log("=== CLEAR LEAGUE PROGRESS (DEBUG) ===");

        // Reset de Draft Levels
        lm.CanDraftlvl1 = false;
        lm.CanDraftlvl2 = false;
        lm.CanDraftlvl3 = false;

        // Reset de Special Players
        lm.CanDrafSpPlayer0 = false;
        lm.CanDraftSpPlayer1 = false;
        lm.CanDraftSpPlayer2 = false;
        lm.CanDraftSpPlayer3 = false;
        lm.CanDraftSpPlayer4 = false;

        // Reset de Facility Bonuses
        lm.FacilitieBonus0 = false;
        lm.FacilitieBonus1 = false;

        // Reset de opções legacy (se ainda estiver usando)
        lm.isOnDraftLVL0 = false;
        lm.isOnDraftLVL1 = false;
        lm.isOnDraftLVL2 = false;

        // Opcional: Reset de algumas flags básicas
        lm.canTrade = true;
        lm.canTrain = true;
        lm.canNegociateContract = true;

        // Salva o estado limpo
        SaveLeague();

        Debug.Log("League Progress limpo com sucesso! (Todos os drafts e bônus foram resetados para false)");
    }
}
