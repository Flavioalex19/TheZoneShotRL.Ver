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
    LeagueManager leagueManager;
    MatchStates match;
    public int GamePossesions = 10;
    public int currentGamePossessons;
    public Team HomeTeam;
    public Team AwayTeam;
    Team teamWithball;
    Player playerWithTheBall;

    public float _timeOutTimer = 0;
    float timerTimeOutReset = 7f;
    [SerializeField] float _timeoutReset = 10f;
    [SerializeField] bool _canCallTimeout = false;
    public bool IsOnTimeout = false;
    //[SerializeField]MatchStates _previousMatchState;//save the current state for the timeout


    //Area for swapping players
    int _indexPLayerA;
    int _indexPLayerB;

    public bool HasActionOnTimeout = true;

    //UI Elemens test
    public GameObject EndScreenStatsPanel;
    Button btn_ReturnToTeamManagement;
    [SerializeField] TextMeshProUGUI _debugTimeoutText;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UiManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        btn_ReturnToTeamManagement = GameObject.Find("Advance to Team Management Screen Button").GetComponent<Button>();
        EndScreenStatsPanel = GameObject.Find("End Game Stats");
        btn_ReturnToTeamManagement.onClick.AddListener(() => manager.ReturnToTeamManegement());
        EndScreenStatsPanel.SetActive(false);
        match = MatchStates.Start;
        currentGamePossessons = GamePossesions;
        
        HomeTeam = manager.playerTeam;


        //AwayTeam = manager.leagueTeams[1];
        for (int i = 0; i < manager.leagueTeams.Count; i++)
        {
            if (manager.leagueTeams[i] == manager.playerTeam._schedule[leagueManager.Week])
            {
                AwayTeam = manager.leagueTeams[i];
            }
        }


        HomeTeam.Score = 0;
        AwayTeam.Score = 0;
        HomeTeam.isOnDefenseBonus = false;
        AwayTeam.isOnDefenseBonus = false;
        teamWithball = HomeTeam;//Change Later

        _canCallTimeout = true;
        HasActionOnTimeout = true;

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
    void FixedUpdate()
    {
        
        if (_canCallTimeout)
        {
            _debugTimeoutText.text = "Can Call Timeout";

            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
        }
    }
    IEnumerator GameFlow()
    {
        while (currentGamePossessons > 0)
        {
            // Step 1: Choose the player to carry the ball
            ChoosePlayerToCarryBall();
            match = MatchStates.Possesion;

            yield return new WaitForSeconds(2f);

            // Step 2: Player decides to pass or not
            yield return ChooseToPass();

            // Step 3: Wait for final actions (e.g., scoring)
            yield return StartCoroutine(WaitForTimeOut());

            currentGamePossessons--;
        }
       
        // End of match logic
        uiManager.PlaybyPlayText("MatchEnded");

        if (HomeTeam.Score > AwayTeam.Score)
        {
            AwayTeam.Moral -= 15;
            HomeTeam.Moral += 15;
        }
        else if (HomeTeam.Score < AwayTeam.Score)
        {
            HomeTeam.Moral -= 15;
            AwayTeam.Moral += 15;
        }
        else
        {
            HomeTeam.Moral -= 5;
            AwayTeam.Moral -= 5;
        }

        EndScreenStatsPanel.SetActive(true);
    }
    void ChoosePlayerToCarryBall()
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
        //print(playerWithHighAwareness.playerFirstName);
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
                //print(currentPlayer.playerFirstName + " HAS THE BALL!!!!!!!");
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
            print((currentPlayer.Shooting - 40f) / (99f - 40f) + " WMP");
            if (willMakeThePass < 3)
            {
                print("Make the pass " + currentPlayer.playerFirstName);
                Player nextPlayer = null;

                float passSuccessChance = Mathf.Clamp((currentPlayer.Awareness - 30f) / (99f - 30f), 0f, 1f);
                if (Random.value > passSuccessChance)
                {
                    print(currentPlayer.playerFirstName + " made a bad pass! Turnover.");
                    uiManager.PlaybyPlayText(currentPlayer.playerFirstName + " made a bad pass! Possession lost.");
                    SwitchPossession();
                    yield return new WaitForSeconds(2f);
                    yield break;
                }
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
                //playerWithTheBall = currentPlayer;
                // Ensure playerWithTheBall is assigned correctly
                if (currentPlayer != null)
                {
                    playerWithTheBall = currentPlayer;
                }
                else
                {
                    // Fallback in case of an unexpected error
                    playerWithTheBall = teamWithball.playersListRoster[0]; // Pick first player
                    Debug.LogWarning("playerWithTheBall was null. Assigned default player.");
                }
                yield return Scoring(playerWithTheBall);
                break;
            }

        }

    }


    IEnumerator Scoring(Player player)
    {
        while (true)
        {
            bool willShoot = Random.Range(1, 4) < 3; // Adjust logic as needed
            if (willShoot)
            {
                uiManager.PlaybyPlayText(player.playerFirstName + " takes a shot!");
                yield return new WaitForSeconds(2f);
                currentGamePossessons--;
                //THIS 40 WILL BE REPLACED BY THE DEFENDER STAT/AKA THE STEAL VALUE 
                bool hasScored =/* Random.Range(1, 100) < (player.Inside + player.Mid + player.Outside / 3) - 100*/Random.value <= (player.Shooting - 40f) / (99f - 40f);
                if (hasScored)
                {
                    //player.PointsMatch += 2;
                    //teamWithball.Score += 2;
                    if (player.CurrentZone == 0)
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
        _timeOutTimer = timerTimeOutReset;

        if (_canCallTimeout == false)
        {
            IsOnTimeout = true;
            _canCallTimeout = false;

            while (_timeOutTimer > 0) // Countdown loop
            {
                _debugTimeoutText.text = $"Time Out: {_timeOutTimer:F1}"; // Show countdown with 1 decimal place
                yield return new WaitForSeconds(0.1f); // Update every 0.1 second
                _timeOutTimer -= 0.1f;
            }

            _debugTimeoutText.text = "Return to game";
            yield return new WaitForSeconds(2f);

            _debugTimeoutText.text = "Can Call Timeout";
            _canCallTimeout = true;
            IsOnTimeout = false;
        }
        */
        //create a variable to show the counter later!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /*
        _canCallTimeout = true;
        float elapsed = 0f;

        while (elapsed < _timeoutReset)
        {
            if (IsOnTimeout)
            {
                yield return new WaitForSeconds(timerTimeOutReset); // simulate timeout duration
                IsOnTimeout = false;
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        _canCallTimeout = false;
        */
        //yield return new WaitForSeconds(5);
        if (_canCallTimeout == false)
        {
            IsOnTimeout = true;
            uiManager.PlaybyPlayText("We are on a timeout Champ");
            yield return new WaitForSeconds(timerTimeOutReset);
            _canCallTimeout = true;
            IsOnTimeout = false;
            HasActionOnTimeout = true;
        }
        else
        {
            yield return null;
        }

    }
    public void CallTimeout()
    {
        if (_canCallTimeout)
        {
            match = MatchStates.Timeout;
            //IsOnTimeout = true;
            _canCallTimeout = false;
            _debugTimeoutText.text = "Timeout Called!";
        }

    }
    //Substituitions
    public void SubIndex()
    {

    }
    //Create timeout Events
    public void CreateTimeoutEvents()
    {

    }
    public bool GetCanCallTimeout()
    {
        return _canCallTimeout;
    }
}
