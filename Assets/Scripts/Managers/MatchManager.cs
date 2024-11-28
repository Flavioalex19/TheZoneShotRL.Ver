using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MatchManager : MonoBehaviour
{
    public enum MatchStates
    {
        None,
        Pause,
        Start,
        Possesion,
        Decision,
        EndPossession,
        Win,
        Lose
    }
    GameManager manager;
    UiManager uiManager;
    MatchStates match;
    public int GamePossesions = 100;
    public int currentGamePossessons;
    Team HomeTeam;
    Team AwayTeam;
    Team teamWithball;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UiManager>();
        match = MatchStates.Start;
        currentGamePossessons = GamePossesions;
        //TESTING !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        HomeTeam = manager.leagueTeams[0];
        AwayTeam = manager.leagueTeams[1];
        teamWithball = HomeTeam;//Change Later

        StartCoroutine(GameFlow());
    }

    // Update is called once per frame
    void Update()
    {
        //if(match == MatchStates.Start)PreparingMatch();
        //if(match == MatchStates.Possesion)TeamPossession(teamWithball);
    }
    IEnumerator GameFlow()
    {
        //Step 1
        if (match == MatchStates.Start)ChoosePlayerToCarrayBall();
        yield return new WaitForSeconds(2f);
        //Step 2
        ChooseToPass();
        yield return new WaitForSeconds(2f);
        //uiManager.PlaybyPlayText("MatchEnded");

    }
    void ChoosePlayerToCarrayBall()
    {
        Player playerWithHighAwareness = null;
        int highestAwareness = int.MinValue;

        foreach (Player player in teamWithball.playersListRoster)
        {
            if (player.Awareness > highestAwareness)
            {
                highestAwareness = player.Awareness;
                playerWithHighAwareness = player;
                
                
            }
        }
        print(playerWithHighAwareness.playerFirstName);
        /*
        // Set HasTheBall to true for the identified player
        if (playerWithHighAwareness != null)
        {
            foreach (Player player in teamWithball.playersListRoster)
            {
                player.HasTheBall = false; // Reset for all players
            }
            playerWithHighAwareness.HasTheBall = true;
        }
        */
        playerWithHighAwareness.HasTheBall = true;
        uiManager.PlaybyPlayText(playerWithHighAwareness.playerFirstName + " " + " Has the ball" + " " + playerWithHighAwareness.HasTheBall);
        match = MatchStates.Possesion;
    }
    void ChooseToPass()
    {
        Player currentPlayer = null;
        for (int i = 0; i < teamWithball.playersListRoster.Count; i++)
        {
            if (teamWithball.playersListRoster[i].HasTheBall)
            {
                currentPlayer = teamWithball.playersListRoster[i];
                print(currentPlayer.playerFirstName + " HAS THE BALL!!!!!!!");
                break; // Stop once the player with the ball is found
            }
        }
        while (true)
        {
            int willMakeThePass = Random.Range(1, 4);
            //willMakeThePass = 1;
            print(willMakeThePass);
            if(willMakeThePass <= 2)
            {
                print("Make the pass " + currentPlayer.playerFirstName);
                Player nextPlayer = null;
                for (int i = 0; i < teamWithball.playersListRoster.Count; i++)
                {
                    if (teamWithball.playersListRoster[i].HasTheBall == false)
                    {
                        print(teamWithball.playersListRoster[i].playerFirstName + " Wants to receive the pass");
                        nextPlayer = teamWithball.playersListRoster[i];
                        break;
                    }
                }
                print(nextPlayer.playerFirstName + " wants to reveice the ball from " + currentPlayer.playerFirstName);
                currentPlayer.HasTheBall = false;
                nextPlayer.HasTheBall = true;
                uiManager.PlaybyPlayText(currentPlayer.playerFirstName + " passes to " + nextPlayer.playerFirstName);
                print(currentPlayer.playerFirstName + " passes to " + nextPlayer.playerFirstName);
                currentPlayer = nextPlayer;
                currentGamePossessons--;
                
            }
            else
            {
                print("No more passes allowed");
                uiManager.PlaybyPlayText(currentPlayer.playerFirstName + " Try to score");
                print(currentPlayer.playerFirstName + " Try to score");
                currentGamePossessons--;
                break;
            }

        }
        

    }
}
