using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        None,
        MainMenu,
        Draft,
        TeamManagement,
        Match,
        PLayoffs
    }

    public GameMode mode;
    public List<Team> fullTeamList = new List<Team>();
    public List<Team> leagueTeams = new List<Team>();
    //public Team team; // Reference to the Team object
    public GameObject playerPrefab; // Prefab to instantiate new players
    public SaveSystem saveSystem; // Reference to the SaveSystem
    public static GameManager Instance { get; private set; }

    LeagueManager leagueManager;
    UiManager uiManager;
    [SerializeField] IntroManager introManager;

    public GridLayoutGroup glg_draftNames;
    int currentTeamIndex = 0;

    int count = 0;//testing the transition to onli create ince time the players

    //public Team playerTeam;
    public Team playerTeam
    {
        get
        {
            if (_playerTeamCache == null || !_playerTeamCache.IsPlayerTeam)
            {
                _playerTeamCache = leagueTeams.FirstOrDefault(t => t != null && t.IsPlayerTeam);
            }
            return _playerTeamCache;
        }
        set
        {
            _playerTeamCache = value;
            if (value != null) value.IsPlayerTeam = true;
        }
    }

    private Team _playerTeamCache;
    private void Awake()
    {
        // Singleton implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }

        //saveSystem.ClearSave(team.TeamName);
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UiManager>();
        mode = GameMode.MainMenu;
        //NewTeamsForRun();
        if (leagueManager.CanStartANewRun)
        {
            DestroyAllWithTag("ZsPlayer");
            DestroyAllWithTag("Team");
        }
        if (mode == GameMode.Draft)
        {
            glg_draftNames = GameObject.Find("DraftContent").GetComponent<GridLayoutGroup>();
        }

        //ensure that all the teams, before loading, have 0 players
        for (int i = 0; i < leagueTeams.Count; i++)
        {
            leagueTeams[i].IsPlayerTeam = false;
            leagueTeams[i].playersListRoster.Clear();
            //leagueTeams[i]._equipmentList.Clear();
            //leagueTeams[i].IsPlayerTeam = false;
        }
        ClearSchedule();
        // Check if there is a saved file for the team
        #region Loading Teams

        // Sempre limpa a lista leagueTeams no início
        // 1. Sempre limpa a lista antes de recriar
        leagueTeams.Clear();

        // 2. Recria os times a partir do fullTeamList
        NewTeamsForRun();

        if (IsSaveFileExists(leagueTeams[0].TeamName) && IsSaveFileExists(leagueTeams[1].TeamName))
        {
            Debug.Log("[GameManager] Save encontrado - Carregando dados...");

            //Loadleague
            saveSystem.LoadLeague();

            // Load the saved team data into the newly created teams
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                saveSystem.LoadTeamInfo(leagueTeams[i], playerPrefab);

                if (leagueTeams[i].IsPlayerTeam)
                    playerTeam = leagueTeams[i];
            }
        }
        else
        {
            Debug.Log("[GameManager] Nenhum save encontrado - Inicializando valores padrão");

            for (int i = 0; i < leagueTeams.Count; i++)
            {
                leagueTeams[i].Moral = leagueTeams[i].fixMoral;
                leagueTeams[i].FansSupportPoints = leagueTeams[i].fixFans;
                leagueTeams[i].FrontOfficePoints = leagueTeams[i].fixFrontOffice;
                leagueTeams[i].EffortPoints = leagueTeams[i].fixEffort;
                leagueTeams[i].IsPlayerTeam = false;
                leagueTeams[i].Wins = 0;
                leagueTeams[i].Draws = 0;
                leagueTeams[i].Loses = 0;
                leagueTeams[i]._equipmentList.Clear();
            }
        }

        // 3. Garante que o playerTeam seja atribuído corretamente
        for (int i = 0; i < leagueTeams.Count; i++)
        {
            if (leagueTeams[i].IsPlayerTeam)
            {
                playerTeam = leagueTeams[i];
                break;
            }
        }
        #endregion
    }
    private void Start()
    {
        
        
    }

    private void Update()
    {
        /*
        for (int i = 0; i < leagueTeams.Count; i++)
        {
            if (leagueTeams[i].IsPlayerTeam)
            {
                //leagueTeams[i].CreateEquips();
                playerTeam = leagueTeams[i];
                //leagueTeams[0].ActivatePlayerTeam();
            }
        }
        */
        // If the ESC key is pressed and there is a save file, clear the save
        if (Input.GetKeyDown(KeyCode.Tab) && IsSaveFileExists(leagueTeams[0].TeamName) && IsSaveFileExists(leagueTeams[1].TeamName))
        {
            //saveSystem.ClearSave(leagueTeams[0].TeamName, leagueTeams[0]);
            //saveSystem.ClearSave(leagueTeams[1].TeamName, leagueTeams[1]);
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                saveSystem.ClearSave(leagueTeams[i].TeamName, leagueTeams[i]);
                saveSystem.ResetLeagueData();
                //saveSystem.ResetForNewLeagueRun();
                print("Clear");
            }
            Application.Quit();


        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        #region Match
        
        
        #endregion
        
    }

    // Function to check if the save file exists
    private bool IsSaveFileExists(string teamName)
    {
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, $"{teamName}_teamData.json");
        return System.IO.File.Exists(filePath);
    }
    //SCHEDULE AREA
    public void ScheduleCreation(List<Team> leagueTeams)
    {
        
        int numTeams = leagueTeams.Count;

        // If odd, add a dummy "bye" team
        bool hasBye = false;
        if (numTeams % 2 != 0)
        {
            leagueTeams.Add(null);
            numTeams++;
            hasBye = true;
        }

        int numWeeks = numTeams - 1;

        // Initialize each team’s schedule
        foreach (Team team in leagueTeams)
        {
            if (team != null)
                team._schedule = new List<Team>(new Team[numWeeks]);
        }

        for (int week = 0; week < numWeeks; week++)
        {
            for (int i = 0; i < numTeams / 2; i++)
            {
                int teamAIndex = (week + i) % (numTeams - 1);
                int teamBIndex = (numTeams - 1 - i + week) % (numTeams - 1);

                if (i == 0)
                    teamBIndex = numTeams - 1;

                Team teamA = leagueTeams[teamAIndex];
                Team teamB = leagueTeams[teamBIndex];

                // Skip if one is the bye
                if (teamA == null || teamB == null)
                    continue;

                teamA._schedule[week] = teamB;
                teamB._schedule[week] = teamA;
            }
        }

        // Optional: remove the dummy bye team
        if (hasBye)
            leagueTeams.Remove(null);
    }
    // Helper to check if teams have already been matched
    private bool HasFaced(Team a, Team b)
    {
        foreach (Team opponent in a._schedule)
        {
            if (opponent == b)
                return true;
        }
        return false;
    }
    public void ClearSchedule()
    {
        foreach (Team team in leagueTeams)
        {
            team.GetSchedule().Clear();
        }
    }
    
    public void ToTitleScreen()
    {
        SceneManager.LoadScene("Title");
    }

    public void AdvanceToDraft()
    {

        StartCoroutine(AdvanceToDraftRoutine());

    }
    private IEnumerator AdvanceToDraftRoutine()
    {
        introManager = GameObject.FindFirstObjectByType<IntroManager>().GetComponent<IntroManager>();
        if (GameObject.Find("TransitionSequence"))
        {
            Animator animator = GameObject.Find("TransitionSequence").GetComponent<Animator>();
            GameObject.Find("PhaseText").GetComponent<TextMeshProUGUI>().text = " Next Phase";
            animator.SetTrigger("Go");
        }

        float timer = 2f;
        while (timer > 0)
        {
            introManager.ChangeStageTransitionTextIntro();
            timer -= Time.deltaTime;
            yield return null; // Wait a frame instead of freezing
        }
        if (leagueManager.CanStartANewRun)
        {
            saveSystem.ResetForNewLeagueRun();
            saveSystem.ResetLeagueData();
            //leagueTeams.Clear();
            //NewTeamsForRun();
        }
        if (leagueManager.CanStartANewRun == false)
        {
            mode = GameMode.TeamManagement;
            //mode = GameMode.TeamManagement;
            SceneManager.LoadScene("MyTeamScreen");
        }
        else
        {
            //mode = GameMode.Draft;
            //saveSystem.ResetForNewRun();
            leagueManager.canGenerateEvents = true;
            leagueManager.isGameOver = false;
            //reset teams
            if(leagueTeams.Count> 0)
            {
                for (int i = 0; i < leagueTeams.Count; i++)
                {
                    leagueTeams[i].playersListRoster.Clear();
                    leagueTeams[i].Moral = leagueTeams[i].fixMoral;
                    leagueTeams[i].FrontOfficePoints = leagueTeams[i].fixFrontOffice;
                    leagueTeams[i].FansSupportPoints = leagueTeams[i].fixFansSupport;
                    leagueTeams[i].EffortPoints = leagueTeams[i].fixEffort;
                    leagueTeams[i].Wins = 0;
                    leagueTeams[i].Loses = 0;
                    leagueTeams[i].Draws = 0;
                    leagueTeams[i].OfficeLvl = 0;
                    leagueTeams[i].MedicalLvl = 0;
                    leagueTeams[i].ArenaLvl = 0;
                    leagueTeams[i].FinancesLvl = 0;
                    leagueTeams[i].MarketingLvl = 0;
                    leagueTeams[i].TeamEquipmentLvl = 0;


                }

            }

            //leagueTeams.Clear();
            //NewTeamsForRun();
            CheckAndRecreateTeamsIfNeeded();
            SceneManager.LoadScene("TeamSelection");
        }
    }
    #region Transitions METHODS
    public void GoToMatch()
    {
        mode = GameMode.Match;
        SceneManager.LoadScene("Match");
    }
    public void GoToPlayoffs()
    {
        mode = GameMode.PLayoffs;
        SceneManager.LoadScene("Playoffs");
    }
    public void ReturnToTeamManegement()
    {
        mode = GameMode.TeamManagement;
        leagueManager.canGenerateEvents = true;
        leagueManager.canStartANewWeek = true;
        leagueTeams[0].Score = 0;
        leagueTeams[1].Score = 0;
        uiManager.hasbeenCreatedTheBtn = false;
        if (leagueManager.Week >= leagueTeams.Count-1) leagueManager.isOnR8 = true;
        //leagueManager.canStartANewWeek = true;
        SceneManager.LoadScene("MyTeamScreen");
    }
    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("Title");
    }
    #endregion
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
    }

    public int GetCurrentTeamIndex()
    {
        return currentTeamIndex;
    }
    IEnumerator TransitionTime()
    {
        yield return new WaitForSeconds(3f);
    }
    public void QuitAndClear()
    {
        for (int i = 0; i < leagueTeams.Count; i++)
        {
            saveSystem.ClearSave(leagueTeams[i].TeamName, leagueTeams[i]);
        }
        Application.Quit();
    }
    public void ResetRunTeams()
    {
        for (int i = 0; i < leagueTeams.Count; i++)
        {
            saveSystem.ClearSave(leagueTeams[i].TeamName, leagueTeams[i]);
            saveSystem.ResetLeagueData();
            leagueManager.isGameOver = false;
            
        }
        SceneManager.LoadScene("Title");
    }
    public void QuitApp()
    {
        Application.Quit();
    }
    //createTeams
    public void NewTeamsForRun()
    {
        
        
        leagueTeams.Clear(); // Garante que começa vazia

        if (fullTeamList == null || fullTeamList.Count == 0)
        {
            Debug.LogError("[GameManager] fullTeamList está vazia! Não é possível criar os times.");
            return;
        }

        foreach (Team teamTemplate in fullTeamList)
        {
            if (teamTemplate == null)
            {
                Debug.LogWarning("[GameManager] Template de time null encontrado. Pulando.");
                continue;
            }

            GameObject teamGO = Instantiate(teamTemplate.gameObject);
            Team newTeam = teamGO.GetComponent<Team>();

            if (newTeam == null)
            {
                Debug.LogError("[GameManager] Falha ao pegar componente Team do template!");
                Destroy(teamGO);
                continue;
            }

            teamGO.transform.SetParent(transform);
            DontDestroyOnLoad(teamGO);

            leagueTeams.Add(newTeam);

            Debug.Log($"[GameManager] Time criado e adicionado: {newTeam.TeamName}");
        }

        Debug.Log($"[GameManager] Total de times recriados e adicionados na lista: {leagueTeams.Count}");
    }
    public void ReassignPlayerTeam()
    {
        if (leagueTeams == null || leagueTeams.Count == 0)
        {
            Debug.LogError("leagueTeams está vazia! Não foi possível reatribuir playerTeam.");
            return;
        }

        foreach (Team team in leagueTeams)
        {
            if (team.IsPlayerTeam)
            {
                playerTeam = team;
                Debug.Log($"playerTeam reatribuído com sucesso: {team.TeamName}");
                return;
            }
        }

        Debug.LogError("Nenhum time com IsPlayerTeam = true foi encontrado!");
    }
    //Destroy objs by tag
    public void DestroyAllWithTag(string tag)
    {
        // Busca todos os objetos ativos na cena com essa tag
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tag);

        // Apaga todos eles
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        // Opcional: mensagem no console para confirmar
        Debug.Log($"Foram destruídos {objectsToDestroy.Length} objetos com a tag '{tag}'.");
    }
    public void CheckAndRecreateTeamsIfNeeded()
    {
        // Conta quantos filhos diretos têm a tag "Team"
        int childTeamCount = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.CompareTag("Team"))
            {
                childTeamCount++;
            }
        }

        Debug.Log($"[GameManager] Filhos com tag 'Team': {childTeamCount} | fullTeamList: {fullTeamList.Count}");

        // Se a quantidade não bater, recria
        if (childTeamCount != fullTeamList.Count)
        {
            Debug.Log($"[GameManager] Quantidade de times incorreta. Limpando e recriando...");

            leagueTeams.Clear();
            DestroyAllWithTag("Team");     // Remove todos os objetos com tag Team
            NewTeamsForRun();              // Recria corretamente
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                leagueTeams[i].IsPlayerTeam = false;
                leagueTeams[i].playersListRoster.Clear();
                //leagueManager.Standings.Add(leagueTeams[i]);
            }
            leagueManager.Standings.Clear();
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                if (transform.GetChild(i).CompareTag("Team"))
                {
                    leagueManager.Standings.Add(transform.GetChild(i).GetComponent<Team>());
                }
            }
        }
        else
        {
            Debug.Log("[GameManager] Quantidade de times está correta. Não precisa recriar.");
        }
    }
    public void DestroyAllTeams()
    {
        // Procura todos os objetos na cena com a tag "team"
        GameObject[] teams = GameObject.FindGameObjectsWithTag("team");

        int count = teams.Length;

        foreach (GameObject teamObj in teams)
        {
            Destroy(teamObj);
        }
        leagueManager.Standings.Clear();
        playerTeam = null;
        Debug.Log($"[GameManager] DestroyAllTeams() Foram destruídos {count} objetos com a tag 'team'.");
    }
}
