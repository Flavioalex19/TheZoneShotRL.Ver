using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    
    GameManager gameManager;
    MatchManager matchManager;
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
        if(gameManager.mode == GameManager.GameMode.Match)
        {
            #region Player Info Match
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
    }
        
    public void PlaybyPlayText(string textContent)
    {
        Transform playByPlayText = GameObject.Find("play-by-playText").transform;
        playByPlayText.GetComponent<TextMeshProUGUI>().text=textContent;
    }
}

