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
    public int GamePossesions = 10;
    public int currentGamePossessons;
    Team HomeTeam;
    Team AwayTeam;
    Team teamWithball;
    Player playerWithTheBall;
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
        
    }
    IEnumerator GameFlow()
    {
        while (currentGamePossessons > 0)
        {
            //Step 1
            if (match == MatchStates.Start) ChoosePlayerToCarrayBall();
            else ChoosePlayerToCarrayBall();
            match = MatchStates.Possesion;
            yield return new WaitForSeconds(2f);
            /*
            //Step 2
            ChooseToPass();
            yield return new WaitForSeconds(2f);
            */
            //Step 2
            yield return ChooseToPass();
            //Step 3
            yield return Scoring(playerWithTheBall);
        }
        
        uiManager.PlaybyPlayText("MatchEnded");//This while be after/out of the while!!!!!

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
        //match = MatchStates.Possesion;
    }
    IEnumerator ChooseToPass()
    {
        Player currentPlayer = null;
        for (int i = 0; i < teamWithball.playersListRoster.Count; i++)
        {
            if (teamWithball.playersListRoster[i].HasTheBall)
            {
                currentPlayer = teamWithball.playersListRoster[i];
                print(currentPlayer.playerFirstName + " HAS THE BALL!!!!!!!");
                currentPlayer.CurrentZone = 0;
                break; // Stop once the player with the ball is found
            }
        }
        while (true)
        {
            if (currentGamePossessons <= 1)
            {
                print("Only one possession left, player must shoot.");
                uiManager.PlaybyPlayText(currentPlayer.playerFirstName + " must shoot due to low possessions!");
                playerWithTheBall = currentPlayer;
                yield break; // Exit the passing loop and proceed to scoring
            }
            int willMakeThePass = Random.Range(1, 4);
            //willMakeThePass = 1;
            print(willMakeThePass +" WMP");
            if(willMakeThePass < 3)
            {
                print("Make the pass " + currentPlayer.playerFirstName);
                Player nextPlayer = null;
                /*
                for (int i = 0; i < teamWithball.playersListRoster.Count; i++)
                {
                    if (teamWithball.playersListRoster[i].HasTheBall == false)
                    {
                        print(teamWithball.playersListRoster[i].playerFirstName + " Wants to receive the pass");
                        nextPlayer = teamWithball.playersListRoster[i];
                        break;
                    }
                }
                */
                List<Player> availablePlayers = teamWithball.playersListRoster
    .Where(player => !player.HasTheBall).ToList();
                if (availablePlayers.Count > 0)
                {
                    nextPlayer = availablePlayers[Random.Range(0, availablePlayers.Count)];
                }

                print(nextPlayer.playerFirstName + " wants to reveice the ball from " + currentPlayer.playerFirstName);
                currentPlayer.HasTheBall = false;
                nextPlayer.HasTheBall = true;
                uiManager.PlaybyPlayText(currentPlayer.playerFirstName + " passes to " + nextPlayer.playerFirstName);
                yield return new WaitForSeconds(2f);
                print(currentPlayer.playerFirstName + " passes to " + nextPlayer.playerFirstName);
                currentPlayer = nextPlayer;
                currentPlayer.CurrentZone = 0;
                currentGamePossessons--;
                
                
            }
            else
            {
                print("No more passes allowed");
                uiManager.PlaybyPlayText(currentPlayer.playerFirstName + " Try to score");
                print(currentPlayer.playerFirstName + " Will Try to score");
                yield return new WaitForSeconds(2f);
                //Scoring(currentPlayer);
                playerWithTheBall = currentPlayer;
                //currentGamePossessons--;
                break;
            }

        }
        
    }
    
    
    IEnumerator Scoring(Player player)
    {
        while (true)
        {
            bool willShoot = Random.Range(1,4) < 3; // Adjust logic as needed
            if (willShoot)
            {
                uiManager.PlaybyPlayText(player.playerFirstName + " takes a shot!");
                yield return new WaitForSeconds(2f);
                currentGamePossessons--;
                bool hasScored= /*Random.Range(1, 4) < 3*/Random.Range(1, 100) < (player.Inside + player.Mid + player.Outside / 3) - 100;
                if (hasScored)
                {
                    //player.PointsMatch += 2;
                    //teamWithball.Score += 2;
                    if(player.CurrentZone == 0)
                    {
                        player.PointsMatch += 4;
                        teamWithball.Score += 4;
                    }
                    else if (player.CurrentZone == 1)
                    {
                        player.PointsMatch += 3;
                        teamWithball.Score += 3;
                    }
                    else
                    {
                        player.PointsMatch += 2;
                        teamWithball.Score += 2;
                    }
                    uiManager.PlaybyPlayText(player.playerFirstName + " Has Scored" + " " + player.PointsMatch);
                    
                }
                else
                {
                    uiManager.PlaybyPlayText(player.playerFirstName + " Missed");
                }
                yield return new WaitForSeconds(2f);
                SwitchPossession(); // Switch possession after shot attempt
                break; // Exit loop if shooting
            }
            else
            {
                if (currentGamePossessons <= 1)
                {
                    print("Only one possession left, player must shoot!");
                    uiManager.PlaybyPlayText(player.playerFirstName + " must shoot due to low possessions!");
                    continue; // Skip movement logic, forcing the player to attempt a shot
                }
                int newZone = Random.Range(player.CurrentZone, 3); // Restrict movement to current or forward zones
                if (newZone > player.CurrentZone) // Only update if moving forward
                {
                    player.CurrentZone = newZone;
                    uiManager.PlaybyPlayText(player.playerFirstName + " moves to zone " + player.CurrentZone);
                    yield return new WaitForSeconds(2f);
                    currentGamePossessons--;
                    
                }
                else willShoot = true;
            }

            // Commented section for ball stolen logic
            // if (ballStolen) break;
        }
        
    }
    void SwitchPossession()
    {
        teamWithball = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
        playerWithTheBall = null;
        uiManager.PlaybyPlayText("Possession switches to " + teamWithball.TeamName);
    }

}
