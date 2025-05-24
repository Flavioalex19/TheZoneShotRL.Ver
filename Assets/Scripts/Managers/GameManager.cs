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
        Match
    }

    public GameMode mode;
    public List<Team> leagueTeams = new List<Team>();
    //public Team team; // Reference to the Team object
    public GameObject playerPrefab; // Prefab to instantiate new players
    public SaveSystem saveSystem; // Reference to the SaveSystem
    public static GameManager Instance { get; private set; }

    LeagueManager leagueManager;
    UiManager uiManager;

    GridLayoutGroup glg_draftNames;
    int currentTeamIndex = 0;

    int count = 0;//testing the transition to onli create ince time the players

    public Team playerTeam;
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
    }
    private void Start()
    {
        
        //saveSystem.ClearSave(team.TeamName);
        leagueManager =GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UiManager>();
        mode = GameMode.MainMenu;

        if (mode == GameMode.Draft)
        {
            glg_draftNames = GameObject.Find("DraftContent").GetComponent<GridLayoutGroup>();
        }

        //ensure that all the teams, before loading, have 0 players
        for (int i = 0; i < leagueTeams.Count; i++)
        {
            leagueTeams[i].playersListRoster.Clear();
            //leagueTeams[i]._equipmentList.Clear();
            //leagueTeams[i].IsPlayerTeam = false;
        }
        ClearSchedule();

        //TESTING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //leagueTeams[0].CreateEquips();
        //leagueTeams[0].ActivatePlayerTeam();//This should be set on the team selection!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        

        // Check if there is a saved file for the team
        #region Loading Teams
        //Loadng teams
        if (IsSaveFileExists(leagueTeams[0].TeamName) && IsSaveFileExists(leagueTeams[1].TeamName))
        {
            // Load the saved team
            //saveSystem.LoadTeam(team);OLD!!!!!!!!!!!!!!!!!!!!!!!!!!
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                //print("TIME TO LOAD");
                saveSystem.LoadTeam(leagueTeams[i]);
                if (leagueTeams[i].IsPlayerTeam)playerTeam = leagueTeams[i];
                //print(playerTeam + "THIS IS THE PLAYER!!!!!");
            }
            PrintTeamPlayers();

            
        }
        else
        {
            print("NO SAVE");
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                if (leagueTeams[i].IsPlayerTeam)
                {
                    leagueTeams[i].Moral = 60;
                    leagueTeams[i].FansSupportPoints = 20;
                    leagueTeams[i].FrontOfficePoints = 50;
                    leagueTeams[i].IsPlayerTeam = false;
                    leagueTeams[i]._equipmentList.Clear();

                }
                leagueTeams[i].Moral = 60;
                leagueTeams[i].FansSupportPoints = 20;
                leagueTeams[i].FrontOfficePoints = 50;
                leagueTeams[i].IsPlayerTeam = false;
                leagueTeams[i]._equipmentList.Clear();
            }
           
        }
        for (int i = 0; i < leagueTeams.Count; i++)
        {
            if (leagueTeams[i].IsPlayerTeam)
            {
                //leagueTeams[i].CreateEquips();
                playerTeam = leagueTeams[i];
                //leagueTeams[0].ActivatePlayerTeam();
            }
        }
        #endregion
    }

    private void Update()
    {
        // If the ESC key is pressed and there is a save file, clear the save
        if (Input.GetKeyDown(KeyCode.Escape) && IsSaveFileExists(leagueTeams[0].TeamName) && IsSaveFileExists(leagueTeams[1].TeamName))
        {
            //saveSystem.ClearSave(leagueTeams[0].TeamName, leagueTeams[0]);
            //saveSystem.ClearSave(leagueTeams[1].TeamName, leagueTeams[1]);
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                saveSystem.ClearSave(leagueTeams[i].TeamName, leagueTeams[i]);
            }
            Application.Quit();


        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        #region Draft
        if (mode == GameMode.Draft)
        {
            if (mode == GameMode.Draft)
            {
                if (SceneManager.GetActiveScene().name == "Draft")
                {
                    glg_draftNames = GameObject.Find("DraftContent").GetComponent<GridLayoutGroup>();
                    if (count < 1)
                    {
                        GeneratePlayers(leagueTeams.Count * 8);
                        count++;
                    }
                }
                //leagueTeams[1].IsPlayerTeam = true;//TESTING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


            }
        }
        #endregion

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
        /*
        foreach (Team team in leagueTeams)
        {
            List<Team> opponents = new List<Team>(leagueTeams);
            opponents.Remove(team); // Remove itself
            opponents = opponents.OrderBy(t => Random.value).ToList(); // Shuffle
            //team._schedule = opponents;
            team.SetSchedule(opponents);
        }
        */
        int numTeams = leagueTeams.Count;
        int numWeeks = numTeams - 1;

        // Initialize schedule for each team: 1 opponent per week
        foreach (Team team in leagueTeams)
        {
            team._schedule = new List<Team>(new Team[numWeeks]);
        }

        for (int week = 0; week < numWeeks; week++)
        {
            HashSet<int> scheduled = new HashSet<int>();

            for (int i = 0; i < numTeams; i++)
            {
                if (scheduled.Contains(i)) continue;

                for (int j = i + 1; j < numTeams; j++)
                {
                    if (scheduled.Contains(j)) continue;

                    Team teamA = leagueTeams[i];
                    Team teamB = leagueTeams[j];

                    // Make sure they haven't faced each other yet
                    if (!HasFaced(teamA, teamB))
                    {
                        teamA._schedule[week] = teamB;
                        teamB._schedule[week] = teamA;

                        scheduled.Add(i);
                        scheduled.Add(j);
                        break;
                    }
                }
            }
        }
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

    #region DRAFT METHODS
    // Function to generate a specified number of players and add them to the team
    private void GeneratePlayers(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            // Instantiate a new player object
            GameObject playerObject = Instantiate(playerPrefab);

            // Access the Player component and set the attributes
            Player newPlayer = playerObject.GetComponent<Player>();
            newPlayer.GenerateRandomPlayer(); // Randomize the player's name and overall rating
            GeneratePlayerDraftButton(newPlayer);//NEW!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            
        }

        //Debug.Log($"{numberOfPlayers} players have been generated and added to team {team.name}.");
    }

    // Function to print all players' names and OVR from the team
    private void PrintTeamPlayers()
    {
        foreach (Player player in leagueTeams[0].playersListRoster)
        {
            //Debug.Log($"Player: {player.playerFirstName}, OVR: {player.ovr}" +" Has a ssaved File");
        }
    }
    void AlternateTeamsAndAddPlayers()
    {
        int teamIndex = 0; // To track which team to add the player to

        // Find all Player components in the scene
        foreach (Player player in FindObjectsOfType<Player>())
        {
            // Add the player to the current team
            leagueTeams[teamIndex].playersListRoster.Add(player);
            // Print the player's name and the team they were added to
            Debug.Log($"Player {player.playerFirstName} added to Team {leagueTeams[teamIndex].TeamName}");
            // Alternate to the next team
            teamIndex = (teamIndex + 1) % leagueTeams.Count;
        }
    }
    void GeneratePlayerDraftButton(Player player)
    {
        GameObject newButton = Instantiate(player.bt_DraftInfo,glg_draftNames.transform, false);
        newButton.GetComponent<Button>().onClick.AddListener(() => AddPlayerToTeam(player, newButton.GetComponent<Button>()));
        newButton.GetComponent<Button>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.playerFirstName.ToString();
        newButton.GetComponent<Button>().transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.ovr.ToString();
        //newButton.GetComponent<Button>().transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = player.Inside.ToString();
        //newButton.GetComponent<Button>().transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = player.Mid.ToString();
        //newButton.GetComponent<Button>().transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = player.Outside.ToString();
        Sprite sprite = null; //= Resources.Load<Sprite>("Assets/Resources/2D/Player Personalities/UI_icon_Personalite_01.png");
        switch (player.Personality)
        {
            case 1:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                break;
            case 2:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                break; 
            case 3:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                break;
                
            case 4:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                break;

            case 5:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                break;

            default:
                break;
        }
        Image myImageComponent = newButton.GetComponent<Button>().transform.GetChild(2).GetComponent<Image>();
        myImageComponent.sprite = sprite;
        

    }
    void AddPlayerToTeam(Player player, Button btn)
    {
        // Add the player to the current team
        leagueTeams[currentTeamIndex].playersListRoster.Add(player);
        // Print the player's name and the team they were added to
        Debug.Log($"Player {player.playerFirstName} added to Team {leagueTeams[currentTeamIndex].TeamName}");
        Destroy(btn.gameObject);//NEW!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // Check child count in the next frame
        StartCoroutine(CheckAndProceedAfterDestroy());

        // Alternate to the next team
        currentTeamIndex = (currentTeamIndex + 1) % leagueTeams.Count;
        
        
    }
    IEnumerator CheckAndProceedAfterDestroy()
    {
        // Wait for the end of the frame to ensure Destroy() has taken effect
        yield return new WaitForEndOfFrame();

        // Check if all buttons are removed from GridLayoutGroup
        if (glg_draftNames.transform.childCount == 0)
        {
            /*
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                saveSystem.SaveTeam(leagueTeams[i]);
            }
            */

            // Change mode and scene---THIA COULD BE A BUTTON!!!!!!!!!!!!!!!!
            mode = GameMode.TeamManagement;
            SceneManager.LoadScene("MyTeamScreen");
        }
    }


    //Testing!!!!!!!!!!!!!!!!!!!!!
    
    public void AdvanceToDraft()
    {
        
        StartCoroutine(AdvanceToDraftRoutine());

    }
    private IEnumerator AdvanceToDraftRoutine()
    {
        if (GameObject.Find("TransitionSequence"))
        {
            Animator animator = GameObject.Find("TransitionSequence").GetComponent<Animator>();
            GameObject.Find("PhaseText").GetComponent<TextMeshProUGUI>().text = " Next Phase";
            animator.SetTrigger("Go");
        }

        float timer = 3f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            print("On Transition");
            yield return null; // Wait a frame instead of freezing
        }

        if (IsSaveFileExists(leagueTeams[0].TeamName) && IsSaveFileExists(leagueTeams[1].TeamName))
        {
            mode = GameMode.TeamManagement;
            SceneManager.LoadScene("MyTeamScreen");
        }
        else
        {
            //mode = GameMode.Draft;
            SceneManager.LoadScene("TeamSelection");
        }
    }
    #endregion

    #region Transitions METHODS
    public void GoToMatch()
    {
        mode = GameMode.Match;
        SceneManager.LoadScene("Match");
    }
    public void ReturnToTeamManegement()
    {
        mode = GameMode.TeamManagement;
        leagueManager.canGenerateEvents = true;
        leagueManager.canStartANewWeek = true;
        leagueTeams[0].Score = 0;
        leagueTeams[1].Score = 0;
        uiManager.hasbeenCreatedTheBtn = false;
        //leagueManager.canStartANewWeek = true;
        SceneManager.LoadScene("MyTeamScreen");
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
}
