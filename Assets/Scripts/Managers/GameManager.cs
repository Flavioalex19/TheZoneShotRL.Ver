using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameMode{
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
        }

        //TESTING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        leagueTeams[0].CreateEquips();
        leagueTeams[0].ActivatePlayerTeam();//This should be set on the team selection!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // Check if there is a saved file for the team
        #region Loading Teams
        //Loadng teams
        if (IsSaveFileExists(leagueTeams[0].TeamName) && IsSaveFileExists(leagueTeams[1].TeamName))
        {
            // Load the saved team
            //saveSystem.LoadTeam(team);OLD!!!!!!!!!!!!!!!!!!!!!!!!!!
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                saveSystem.LoadTeam(leagueTeams[i]);
            }
            PrintTeamPlayers();
            //print()
            //mode = GameMode.Match;
            //SceneManager.LoadScene("Match");//This should be team management screen
        }
        else
        {
            //if(mode == GameMode.Draft)
            // No save file, create new players
            //GeneratePlayers(5); // Create 5 players
            //AlternateTeamsAndAddPlayers();//THIS WAS ALREADY TESTED!!!!!!
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                if (leagueTeams[i].IsPlayerTeam)
                {
                    leagueTeams[i].Moral = 50;

                }
            }
           
        }
        #endregion
    }

    private void Update()
    {
        // If the ESC key is pressed and there is a save file, clear the save
        if (Input.GetKeyDown(KeyCode.Escape) && IsSaveFileExists(leagueTeams[0].TeamName) && IsSaveFileExists(leagueTeams[1].TeamName))
        {
            saveSystem.ClearSave(leagueTeams[0].TeamName, leagueTeams[0]);
            saveSystem.ClearSave(leagueTeams[1].TeamName, leagueTeams[1]);
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
                        GeneratePlayers(8); // Create 5 players
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
            Debug.Log($"Player: {player.playerFirstName}, OVR: {player.ovr}" +" Has a ssaved File");
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
        newButton.GetComponent<Button>().transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.Shooting.ToString();
        newButton.GetComponent<Button>().transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = player.Inside.ToString();
        newButton.GetComponent<Button>().transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = player.Mid.ToString();
        newButton.GetComponent<Button>().transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = player.Outside.ToString();

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
        
        if (IsSaveFileExists(leagueTeams[0].TeamName) && IsSaveFileExists(leagueTeams[1].TeamName))
        {
            mode = GameMode.TeamManagement;
            SceneManager.LoadScene("MyTeamScreen");//This should be team management screen
        }
        else
        {
            mode = GameMode.Draft;
            SceneManager.LoadScene("Draft");
        }
    }
    public void GoToMatch()
    {
        mode = GameMode.Match;
        SceneManager.LoadScene("Match");
    }
    public void ReturnToTeamManegement()
    {
        mode = GameMode.TeamManagement;
        leagueManager.canGenerateEvents = true;
        leagueTeams[0].Score = 0;
        leagueTeams[1].Score = 0;
        uiManager.hasbeenCreatedTheBtn = false;
        SceneManager.LoadScene("MyTeamScreen");
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
    }

    public int GetCurrentTeamIndex()
    {
        return currentTeamIndex;
    }
}
