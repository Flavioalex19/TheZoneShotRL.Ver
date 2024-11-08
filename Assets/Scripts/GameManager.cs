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
        Draft,
        Match
    }
    public GameMode mode;
    public List<Team> leagueTeams = new List<Team>();
    //public Team team; // Reference to the Team object
    public GameObject playerPrefab; // Prefab to instantiate new players
    public SaveSystem saveSystem; // Reference to the SaveSystem
    public static GameManager Instance { get; private set; }

    GridLayoutGroup glg_draftNames;
    int currentTeamIndex = 0;
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
        
        mode = GameMode.Draft;
        if (mode == GameMode.Draft)
        {
            glg_draftNames = GameObject.Find("DraftContent").GetComponent<GridLayoutGroup>();
        }
        // Check if there is a saved file for the team

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
            mode = GameMode.Match;
            SceneManager.LoadScene(1);
        }
        else
        {
            // No save file, create new players
            GeneratePlayers(5); // Create 5 players
            //AlternateTeamsAndAddPlayers();//THIS WAS ALREADY TESTED!!!!!!
            
            /*
            //NOT NEED IT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                saveSystem.SaveTeam(leagueTeams[i]);
            }
            */
            
        }
    }

    private void Update()
    {
        // If the ESC key is pressed and there is a save file, clear the save
        if (Input.GetKeyDown(KeyCode.Escape) && IsSaveFileExists(leagueTeams[0].TeamName) && IsSaveFileExists(leagueTeams[1].TeamName))
        {
            saveSystem.ClearSave(leagueTeams[0].TeamName, leagueTeams[0]);
            saveSystem.ClearSave(leagueTeams[1].TeamName, leagueTeams[1]);
            
            
        }
        
        

        if (mode == GameMode.Match)
        {
            print("Match State");
            //PrintTeamPlayers();
            if(GameObject.Find("Text Area"))
            {
                //leagueTeams[0].ClearAllPlayers();
                Transform content = GameObject.Find("Text Area").transform;
                for (int i = 0; i < leagueTeams[0].playersListRoster.Count; i++)
                {
                    content.GetChild(i).GetComponent<TextMeshProUGUI>().text = leagueTeams[0].playersListRoster[i].playerFirstName.ToString() + leagueTeams[0].playersListRoster[i].ovr.ToString();
                }
                Transform awayContent = GameObject.Find("Text Area 2").transform;
                
                for (int i = 0; i < leagueTeams[1].playersListRoster.Count; i++)
                {
                    awayContent.GetChild(i).GetComponent<TextMeshProUGUI>().text = leagueTeams[1].playersListRoster[i].playerFirstName.ToString();
                }
            }
        }
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
        newButton.GetComponent<Button>().transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.ovr.ToString();

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
            for (int i = 0; i < leagueTeams.Count; i++)
            {
                saveSystem.SaveTeam(leagueTeams[i]);
            }

            // Change mode and scene
            mode = GameMode.Match;
            SceneManager.LoadScene(1); // Change to your Match scene name
        }
    }
}
