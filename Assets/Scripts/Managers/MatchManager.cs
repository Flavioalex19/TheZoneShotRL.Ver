using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class MatchManager : MonoBehaviour
{
    public enum MatchStates
    {
        None,
        Pause,
        Start,
        Possesion,
        Decision,
        Timeout,
        EndPossession,
        Win,
        Lose
    }
    GameManager manager;
    UiManager uiManager;
    MatchStates match;
    public int GamePossesions = 10;
    public int currentGamePossessons;
    public Team HomeTeam;
    public Team AwayTeam;
    Team teamWithball;
    Player playerWithTheBall;

    float _timeOutTimer = 0;
    [SerializeField] float _timeoutReset = 2f;
    [SerializeField]bool _canCallTimeout = false;
    //[SerializeField]MatchStates _previousMatchState;//save the current state for the timeout

    //UI Elemens test
    public GameObject EndScreenStatsPanel;
    Button btn_ReturnToTeamManagement;
    [SerializeField] TextMeshProUGUI _debugTimeoutText;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UiManager>();
        btn_ReturnToTeamManagement = GameObject.Find("Advance to Team Management Screen Button").GetComponent<Button>();
        EndScreenStatsPanel = GameObject.Find("End Game Stats");
        btn_ReturnToTeamManagement.onClick.AddListener(()=>manager.ReturnToTeamManegement());
        EndScreenStatsPanel.SetActive(false);
        match = MatchStates.Start;
        currentGamePossessons = GamePossesions;
        //TESTING !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        HomeTeam = manager.leagueTeams[0];
        AwayTeam = manager.leagueTeams[1];
        HomeTeam.Score = 0;
        AwayTeam.Score = 0;
        teamWithball = HomeTeam;//Change Later

        _canCallTimeout = false;

        //JUST FOR TESTINg
        for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
        {
            HomeTeam.playersListRoster[i].PointsMatch = 0;
        }
        for (int i = 0; i < AwayTeam.playersListRoster.Count; i++)
        {
            AwayTeam.playersListRoster[i].PointsMatch = 0;
        }
        
        StartCoroutine(GameFlow());
    }

    // Update is called once per frame
    void Update()
    {
        if(_canCallTimeout == true)
        {
            _debugTimeoutText.text = "Can Call Timeout";
            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //_previousMatchState = match; // Save the last state
            //match = MatchStates.Timeout;
            _canCallTimeout = false;
        }
    }
    IEnumerator GameFlow()
    {
        while (currentGamePossessons > 0)
        {
            //Step 1
            if (match == MatchStates.Start) ChoosePlayerToCarrayBall();
            else ChoosePlayerToCarrayBall();
            match = MatchStates.Possesion;
            //yield return StartCoroutine(WaitForTimeOut());
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(WaitForTimeOut());
            //Step 2

            // return StartCoroutine(WaitForTimeOut());
            yield return ChooseToPass();
            yield return StartCoroutine(WaitForTimeOut());

            //Step 3
            //yield return StartCoroutine(WaitForTimeOut());
            yield return Scoring(playerWithTheBall);
            yield return StartCoroutine(WaitForTimeOut());
        }

        uiManager.PlaybyPlayText("MatchEnded");
        if (HomeTeam.Score > AwayTeam.Score) { AwayTeam.Moral -= 15; HomeTeam.Moral += 15; }
        else if (HomeTeam.Score < AwayTeam.Score) { HomeTeam.Moral -= 15; AwayTeam.Moral += 15; } 
        else
        {
            HomeTeam.Moral -= 5;
            AwayTeam.Moral -= 5;
        }
        //WIP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        EndScreenStatsPanel.SetActive(true);

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

    //Function to enable call for a Timeout
    IEnumerator WaitForTimeOut()
    {
        /*
        _canCallTimeout = true;
        _debugTimeoutText.text = "Can call a timeout";
        yield return new WaitForSeconds(5f);
        while (match == MatchStates.Timeout)
        {
            _debugTimeoutText.text = "Timeout!!!";
            uiManager.PlaybyPlayText("Timeout!");
            yield return new WaitForSeconds(5f); // Wait until the game is unpaused
            //returen to last possiion before the 
            match = MatchStates.Possesion; // Restore last state
        }
        _debugTimeoutText.text = "Returning to game";
       
        yield return new WaitForSeconds(2f); // Wait until the game is unpaused
        
        _debugTimeoutText.text = "Game";
        _canCallTimeout = false;
        
        */
        if (_canCallTimeout == false)
        {
            _debugTimeoutText.text = "Time Out!!!";
            yield return new WaitForSeconds(5f); // Wait until the game is unpaused
            _debugTimeoutText.text = "Return to game";
            yield return new WaitForSeconds(2f);
            _debugTimeoutText.text = "Can Call Timeout";
            _canCallTimeout = true;
        }
        //else _debugTimeoutText.text = "No time outs called";
    }

}
