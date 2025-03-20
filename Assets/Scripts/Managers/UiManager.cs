using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    
    GameManager gameManager;
    MatchManager matchManager;

    //Team Management UI
    public GameObject btn_AdvanceToMatch;
    public bool hasbeenCreatedTheBtn = false;

    //Match elements UI
    Transform playerInfoInMach;
    TextMeshProUGUI homeScoreText;
    TextMeshProUGUI awayScoreText;
    List<Transform> homePlayersStartersUI = new List<Transform>();
    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents this GameObject from being destroyed when changing scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //ADD THIS AS A FUNCTION ALTER ON THE MATCH MANAGER!!!!!
        #region Match
        if (gameManager.mode == GameManager.GameMode.Match)
        {
            #region Player Info Match
            if (GameObject.Find("Team A Players Area"))
            {
                playerInfoInMach = GameObject.Find("Team A Players Area").transform;
                //Change for the leagueManger Home Team
                for (int i = 0; i < gameManager.leagueTeams[0].playersListRoster.Count; i++)
                {
                    playerInfoInMach.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[0].playersListRoster[i].playerFirstName.ToString();
                    playerInfoInMach.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[0].playersListRoster[i].PointsMatch.ToString();
                    homePlayersStartersUI.Add(playerInfoInMach.GetChild(i));

                }
            }
            if (GameObject.Find("Team B Players Area"))
            {
                playerInfoInMach = GameObject.Find("Team B Players Area").transform;
                for (int i = 0; i < gameManager.leagueTeams[1].playersListRoster.Count; i++)
                {
                    playerInfoInMach.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[1].playersListRoster[i].playerFirstName.ToString();
                    playerInfoInMach.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[1].playersListRoster[i].PointsMatch.ToString();
                }
            }
            //Scoreboard
            if(GameObject.Find("Away Team Score Text") && GameObject.Find("Home Team Score Text"))
            {
                homeScoreText = GameObject.Find("Home Team Score Text").GetComponent<TextMeshProUGUI>();
                awayScoreText = GameObject.Find("Away Team Score Text").GetComponent<TextMeshProUGUI>();

                homeScoreText.text = gameManager.leagueTeams[0].Score.ToString();
                awayScoreText.text = gameManager.leagueTeams[1].Score.ToString();
            }
            ///REMOVE THIS LATER    
            if (GameObject.Find("Text Area"))
            {
                //leagueTeams[0].ClearAllPlayers();
                Transform content = GameObject.Find("Text Area").transform;
                Transform insideVariable = GameObject.Find("Inside Stat Text Area").transform;
                Transform midVariable = GameObject.Find("Mid Stat Text Area").transform;
                Transform outVariable = GameObject.Find("Out Stat Text Area").transform;
                for (int i = 0; i < gameManager.leagueTeams[0].playersListRoster.Count; i++)
                {

                    content.GetChild(i).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[0].playersListRoster[i].playerFirstName.ToString() + " " + gameManager.leagueTeams[0].playersListRoster[i].ovr.ToString() + " AWA" + gameManager.leagueTeams[0].playersListRoster[i].Awareness.ToString();
                    insideVariable.GetChild(i).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[0].playersListRoster[i].Inside.ToString();
                    midVariable.GetChild(i).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[0].playersListRoster[i].Mid.ToString();
                    outVariable.GetChild(i).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[0].playersListRoster[i].Outside.ToString();
                }
                Transform awayContent = GameObject.Find("Text Area 2").transform;
                for (int i = 0; i < gameManager.leagueTeams[1].playersListRoster.Count; i++)
                {
                    awayContent.GetChild(i).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[1].playersListRoster[i].playerFirstName.ToString();
                }
            }
            //testing!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (GameObject.Find("Away Team Name Text"))
            {
                Transform aContentText = GameObject.Find("Away Team Name Text").transform;
                for (int i = 0; i < gameManager.leagueTeams.Count; i++)
                {
                    if (gameManager.leagueTeams[i].IsPlayerTeam)
                    {
                        aContentText.GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[i].TeamName.ToString();
                    }
                }

            }
            //Possessions count text
            if (GameObject.Find("NumberOfPossessionsText"))
            {
                Transform contentText = GameObject.Find("NumberOfPossessionsText").transform;
                if (GameObject.Find("MatchManager"))
                {
                    matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
                    contentText.GetComponent<TextMeshProUGUI>().text = matchManager.currentGamePossessons.ToString();
                }
                
            }
            #endregion
            

        }
        #endregion
        #region Team Managemet
        //Change later!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (GameObject.Find("Equips"))
        {
            //Later create a check to find the team that is controlled by the player

            //find equipment text
            Transform equipAreaText = GameObject.Find("Equips").transform;
            for (int i = 0; i < equipAreaText.childCount; i++)
            {
                equipAreaText.GetChild(i).GetComponent<TextMeshProUGUI>().text = 
                    gameManager.leagueTeams[0].GetEquipment()[i].Name.ToString() + " " +gameManager.leagueTeams[0].GetEquipment()[i].Level.ToString();
            }
        }
        //Team Stats and atrributes -DEBUG for now
        if (GameObject.Find("TeamAttValues"))
        {
            Transform teamStatsTextsArea = GameObject.Find("TeamAttValues").transform;
            teamStatsTextsArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Moral: " + gameManager.leagueTeams[0].Moral.ToString();
            /*
            for (int i = 0; i < teamStatsTextsArea.childCount; i++)
            {
                
            }
            */
        }
        #endregion

        /*
        if (GameObject.Find("MatchManager"))
        {
           
            if (matchManager.currentGamePossessons == 0)
            {
                matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
                GameObject teamAStatsName = GameObject.Find("TeamANames");
                GameObject teamAStatsPts = GameObject.Find("TeamAPoints");
                for (int i = 0; i < matchManager.HomeTeam.playersListRoster.Count; i++)
                {
                    teamAStatsName.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString();
                }
            }
        }
        */
        //Match Area!!!!!!!
        if (GameObject.Find("MatchManager"))
        {
            if (matchManager.currentGamePossessons == 0)
            {
                matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
                GameObject teamAStatsName = GameObject.Find("TeamANames");
                GameObject teamAStatsPts = GameObject.Find("TeamAPoints");
                GameObject teamBStatsName = GameObject.Find("TeamBNames");
                GameObject teamBStatsPts = GameObject.Find("TeamBPoints");

                if (teamAStatsName != null) // Check if the object exists
                {
                    for (int i = 0; i < matchManager.HomeTeam.playersListRoster.Count; i++)
                    {
                        if (i < teamAStatsName.transform.childCount) // Prevent out-of-bounds errors
                        {
                            teamAStatsName.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString();
                            teamAStatsPts.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = matchManager.HomeTeam.playersListRoster[i].PointsMatch.ToString();
                        }
                    }
                }
                if (teamAStatsName != null) // Check if the object exists
                {
                    for (int i = 0; i < matchManager.AwayTeam.playersListRoster.Count; i++)
                    {
                        if (i < teamAStatsName.transform.childCount) // Prevent out-of-bounds errors
                        {
                            teamBStatsName.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = matchManager.AwayTeam.playersListRoster[i].playerFirstName.ToString();
                            teamBStatsPts.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = matchManager.AwayTeam.playersListRoster[i].PointsMatch.ToString();
                        }
                    }
                }
            }
        }

    }
    private void FixedUpdate()
    {
        if (gameManager.mode == GameManager.GameMode.TeamManagement && hasbeenCreatedTheBtn == false)
        {
            print("TM Screen -----------------------------");
            GameObject matchButton = GameObject.Find("Match Button");
            if (matchButton != null && matchButton.activeInHierarchy)
            {
                btn_AdvanceToMatch = matchButton;
                btn_AdvanceToMatch.GetComponent<Button>().onClick.AddListener(() => gameManager.GoToMatch());
                hasbeenCreatedTheBtn = true;
            }
            else
            {
                print("Match Button not found or inactive.");
            }
        }
        
    }
    #region Team Management
    public void SetChoiceText(string Message)
    {
        if (GameObject.Find("ChoiceText"))
        {
            GameObject.Find("ChoiceText").GetComponent<TextMeshProUGUI>().text = Message;
        }
    }
    #endregion
    #region Match 
    public void PlaybyPlayText(string textContent)
    {
        Transform playByPlayText = GameObject.Find("play-by-playText").transform;
        playByPlayText.GetComponent<TextMeshProUGUI>().text=textContent;
    }
    //After Game

    #endregion

    public void UpdateCurrentSongAlert(string songName)
    {
        print("New Song");
        if(GameObject.Find("Current Song Panel"))
        {
            Animator songAnim = GameObject.Find("Current Song Panel").GetComponent<Animator>();
            TextMeshProUGUI songText = GameObject.Find("Current Song Panel").GetComponentInChildren<TextMeshProUGUI>();
            songText.text = songName.ToString();
            songAnim.SetTrigger("Play");

        }
    }
    public void ReturnAnimTest()
    {
        if (GameObject.Find("Current Song Panel"))
        {
            Animator songAnim = GameObject.Find("Current Song Panel").GetComponent<Animator>();
            songAnim.SetTrigger("Return");

        }
    }
}

