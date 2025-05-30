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
    [SerializeField] MatchUI _matchUI;
    MatchStates match;
    public int GamePossesions = 10;
    public int currentGamePossessons;
    public Team HomeTeam;
    public Team AwayTeam;
    [SerializeField] public Team teamWithball;
    [SerializeField] Player playerWithTheBall;

    public float _timeOutTimer = 0;
    float timerTimeOutReset = 7f;
    [SerializeField] float _timeoutReset = 10f;
    [SerializeField] bool _canCallTimeout = false;
    public bool IsOnTimeout = false;

    float _actionTimer;
    [SerializeField] float _actionTimerReset = 2f;

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
            if (manager.leagueTeams[i] == manager.playerTeam._schedule[leagueManager.Week-1])
            {
                AwayTeam = manager.leagueTeams[i];
            }
        }
        //AwayTeam = HomeTeam._schedule[leagueManager.Week - 1];
        _actionTimer = _actionTimerReset;
        _matchUI.SetTheTeamTextForTheMatch();
        HomeTeam.Score = 0;
        AwayTeam.Score = 0;
        HomeTeam.HasPlayed = true;
        AwayTeam.HasPlayed = true;
        HomeTeam.isOnDefenseBonus = false;
        AwayTeam.isOnDefenseBonus = false;
        teamWithball = HomeTeam;//Change Later
        HomeTeam.hasPossession = true;

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
        //StartCoroutine(LeagueWeekSimulation());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (_canCallTimeout)
        {
            _debugTimeoutText.text = "Can Call Timeout";


        }
    }
    IEnumerator GameFlow()
    {
        while (currentGamePossessons > 0)
        {
            // Step 1: Choose the player to carry the ball
            ChoosePlayerToCarryBall();
            match = MatchStates.Possesion;

            yield return new WaitForSeconds(_actionTimer);

            // Step 2: Player decides to pass or not
            //yield return ChooseToPass();
            yield return HandlePossession();

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
            HomeTeam.Wins++;
            AwayTeam.Loses--;
        }
        else if (HomeTeam.Score < AwayTeam.Score)
        {
            HomeTeam.Moral -= 15;
            AwayTeam.Moral += 15;
            HomeTeam.Loses--;
            AwayTeam.Wins++;
        }
        else
        {
            HomeTeam.Moral -= 5;
            AwayTeam.Moral -= 5;
            HomeTeam.Draws++;
            AwayTeam.Draws++;
        }
        //HomeTeam.HasPlayed = false;
        //AwayTeam.HasPlayed = false;

        EndScreenStatsPanel.SetActive(true);
    }
    void ChoosePlayerToCarryBall()
    {
        /*
        Player playerWithHighAwareness = null;
        int highestAwareness = int.MinValue;

        foreach (Player player in teamWithball.playersListRoster)//playersListRoster.GetRange(0, 4)
        {
            if (player.Awareness > highestAwareness)
            {
                highestAwareness = player.Awareness;
                playerWithHighAwareness = player;


            }
        }
        if (playerWithHighAwareness != null)
        {
            playerWithHighAwareness.HasTheBall = true;
            playerWithTheBall = playerWithHighAwareness;
            playerWithTheBall.CurrentZone = 0;

            // Swap to first in the list if not already
            int index = teamWithball.playersListRoster.IndexOf(playerWithHighAwareness);
            if (index != 0)
            {
                var temp = teamWithball.playersListRoster[0];
                teamWithball.playersListRoster[0] = playerWithHighAwareness;
                teamWithball.playersListRoster[index] = temp;
            }

            uiManager.PlaybyPlayText(playerWithHighAwareness.playerFirstName + " has the ball.");
            SelectDefender(); // Select new defender based on ball carrier
        }
        //playerWithHighAwareness.HasTheBall = true;
        uiManager.PlaybyPlayText(playerWithHighAwareness.playerFirstName + " " + " Has the ball" + " " + playerWithHighAwareness.HasTheBall);
        */
        /*
        playerWithTheBall = null;
        int highestAwareness = int.MinValue;
        for (int i = 0; i < 4; i++)
        {
            if (teamWithball.playersListRoster[i].Awareness > highestAwareness)
            {
                highestAwareness = teamWithball.playersListRoster[i].Awareness;
                playerWithTheBall = teamWithball.playersListRoster[i];
            }
            playerWithTheBall.CurrentZone = 0;
            playerWithTheBall.HasTheBall = true;
            uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " has the ball.");
            //cHANGE POSIOTN ON THE LIST TODO
            ChangePosOfPlayerWithTheBall();
            //CHOOSE DEFENDER TODO
            print(playerWithTheBall.playerFirstName + " HAS THE BALL");
        }*/
        playerWithTheBall = null;
        int highestAwareness = int.MinValue;

        for (int i = 0; i < 4; i++)
        {
            Player p = teamWithball.playersListRoster[i];
            if (p.Awareness > highestAwareness)
            {
                highestAwareness = p.Awareness;
                playerWithTheBall = p;
            }
        }

        if (playerWithTheBall != null)
        {
            playerWithTheBall.CurrentZone = 0;
            playerWithTheBall.HasTheBall = true;

            ChangePosOfPlayerWithTheBall();
            uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " has the ball.");
        }
        ChangePosOfPlayerWithTheBall();
        uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " has the ball.");
    }
    IEnumerator ChooseToPass()
    {
        /*
        Player currentPlayer = null;
        for (int i = 0; i < 4; i++)//int i = 0; i < teamWithball.playersListRoster.Count; i++
        {
            if (teamWithball.playersListRoster[i].HasTheBall)
            {
                currentPlayer = teamWithball.playersListRoster[i];
                currentPlayer.CurrentZone = 0;
                print(currentPlayer.playerFirstName + " HERE!!!!!!!!!!!!!!!!!!!!!");
                break; // Stop once the player with the ball is found
            }
        }
        //currentPlayer = teamWithball.playersListRoster[0];
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
            //print((currentPlayer.Shooting - 40f) / (99f - 40f) + " WMP");
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
                    yield return new WaitForSeconds(_actionTimer);
                    yield break;
                }
                
                List<Player> availablePlayers = teamWithball.playersListRoster.GetRange(0,4)
    .Where(player => !player.HasTheBall).ToList();
                if (availablePlayers.Count > 0)
                {
                    nextPlayer = availablePlayers[Random.Range(0, availablePlayers.Count)];
                }

                print(nextPlayer.playerFirstName + " wants to reveice the ball from " + currentPlayer.playerFirstName);
                currentPlayer.HasTheBall = false;
                nextPlayer.HasTheBall = true;
                SelectDefender();
                uiManager.PlaybyPlayText(currentPlayer.playerFirstName + " passes to " + nextPlayer.playerFirstName);
                yield return new WaitForSeconds(_actionTimer);
                print(currentPlayer.playerFirstName + " passes to " + nextPlayer.playerFirstName);
                currentPlayer = nextPlayer;
                currentPlayer.CurrentZone = 0;
                currentGamePossessons--;


            }
            else
            {
                uiManager.PlaybyPlayText(currentPlayer.playerFirstName + " Try to score");
                yield return new WaitForSeconds(_actionTimer);
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


        }*/
        yield return null;
    }
    IEnumerator HandlePossession()
    {
        while (true)
        {
            if (currentGamePossessons <= 1)
            {
                uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " must shoot due to low possessions!");
                yield return new WaitForSeconds(_actionTimer);
                yield return Scoring(playerWithTheBall);
                yield break;
            }

            bool shouldPass = Random.Range(1, 4) < 3;

            if (shouldPass)
            {
                if (TryPassBall())
                {
                    yield return new WaitForSeconds(_actionTimer);
                    uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " prepares for next action.");
                    yield return new WaitForSeconds(_actionTimer);
                    continue; // Keep the loop for multiple passes
                }
                else
                {
                    yield return new WaitForSeconds(_actionTimer);
                    SwitchPossession();
                    uiManager.PlaybyPlayText(teamWithball.TeamName + " has the ball.");
                    yield return new WaitForSeconds(_actionTimer);
                    yield break;
                }
            }
            else
            {
                ChangePosOfPlayerWithTheBall();
                uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " goes for the score!");
                yield return new WaitForSeconds(_actionTimer);
                yield return Scoring(playerWithTheBall);
                yield break;
            }
        }
    }
    bool TryPassBall()
    {
        float passSuccessChance = Mathf.Clamp((playerWithTheBall.Awareness - 30f) / (99f - 30f), 0f, 1f);

        if (Random.Range(0f, 1f) > passSuccessChance)
        {
            uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " made a bad pass! Possession lost.");
            return false;
        }

        var availableReceivers = teamWithball.playersListRoster.GetRange(0, 4)
            .Where(p => !p.HasTheBall).ToList();

        if (availableReceivers.Count == 0) return false;

        Player receiver = availableReceivers[Random.Range(0, availableReceivers.Count)];
        playerWithTheBall.HasTheBall = false;
        receiver.HasTheBall = true;
        playerWithTheBall = receiver;

        ChangePosOfPlayerWithTheBall();
        uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " receives the pass.");
        return true;
    }
    IEnumerator Scoring(Player player)
    {
        while (true)
        {
            bool willShoot = Random.Range(1, 4) < 3; // Adjust logic as needed
            if (willShoot)
            {
                uiManager.PlaybyPlayText(player.playerFirstName + " takes a shot!");
                yield return new WaitForSeconds(_actionTimer);
                currentGamePossessons--;
                //THIS 40 WILL BE REPLACED BY THE DEFENDER STAT/AKA THE STEAL VALUE 
                bool hasScored =/* Random.Range(1, 100) < (player.Inside + player.Mid + player.Outside / 3) - 100*/Random.value <= (player.Shooting - 40f) / (99f - 40f);
                if (hasScored)
                {
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
                yield return new WaitForSeconds(_actionTimer);
                SwitchPossession(); // Switch possession after shot attempt
                yield return new WaitForSeconds(_actionTimer);
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
                    yield return new WaitForSeconds(_actionTimer);
                    currentGamePossessons--;

                }
                else willShoot = true;
            }
        }

    }
    
    void SwitchPossession()
    {
        /*
        teamWithball = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
        HomeTeam.hasPossession = (teamWithball == HomeTeam);
        AwayTeam.hasPossession = (teamWithball == AwayTeam);
        playerWithTheBall = null;
        uiManager.PlaybyPlayText("Possession switches to " + teamWithball.TeamName);
        */
        // Toggle team
        if (teamWithball == HomeTeam)
        {
            HomeTeam.hasPossession = false;
            AwayTeam.hasPossession = true;
            teamWithball = AwayTeam;
        }
        else
        {
            AwayTeam.hasPossession = false;
            HomeTeam.hasPossession = true;
            teamWithball = HomeTeam;
        }

        playerWithTheBall = null;
        uiManager.PlaybyPlayText("Possession switches to " + teamWithball.TeamName);
        ChoosePlayerToCarryBall();

    }
    void SelectDefender()
    {
        Team DefendingTeam;
        if (HomeTeam.hasPossession == false)
            DefendingTeam = HomeTeam;
        else
            DefendingTeam = AwayTeam;

        // Pick a random player (excluding index 0)
        int randomIndex = Random.Range(1, DefendingTeam.playersListRoster.Count); // avoid 0
        Player newDefender = DefendingTeam.playersListRoster[randomIndex];

        // Swap the random player with the one at index 0
        Player temp = DefendingTeam.playersListRoster[0];
        DefendingTeam.playersListRoster[0] = newDefender;
        DefendingTeam.playersListRoster[randomIndex] = temp;

        //newDefender.CurrentZone = 0;

        Debug.Log(newDefender.playerFirstName + " is now defending.");
    }
    //Function to enable call for a Timeout
    IEnumerator WaitForTimeOut()
    {
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
    public bool GetCanCallTimeout()
    {
        return _canCallTimeout;
    }
    public IEnumerator LeagueWeekSimulation()
    {
        currentGamePossessons = GamePossesions;
        while (manager.leagueTeams.Any(t => !t.HasPlayed))
        {
            // Find a team that hasn't played this week
            Team teamA = manager.leagueTeams.FirstOrDefault(t => !t.HasPlayed);

            if (teamA == null)
                break; // All teams have played this week

            Team teamB = teamA._schedule[leagueManager.Week];

            // Skip if opponent is null or already played (safety check)
            if (teamB == null || teamB.HasPlayed)
            {
                teamA.HasPlayed = true; // Prevent infinite loop
                continue;
            }

            // Assign match
            HomeTeam = teamA;
            AwayTeam = teamB;
            teamWithball = HomeTeam;

            _actionTimer = 0f; // Instant simulation

            currentGamePossessons = GamePossesions;

            // Reset scores and flags
            HomeTeam.Score = 0;
            AwayTeam.Score = 0;
            HomeTeam.HasPlayed = true;
            AwayTeam.HasPlayed = true;
            HomeTeam.isOnDefenseBonus = false;
            AwayTeam.isOnDefenseBonus = false;

            foreach (Player p in HomeTeam.playersListRoster)
                p.PointsMatch = 0;

            foreach (Player p in AwayTeam.playersListRoster)
                p.PointsMatch = 0;

            // Simulate match using your full GameFlow coroutine
            //yield return StartCoroutine(GameFlow());
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            while (currentGamePossessons > 0)
            {
                // Step 1: Choose the player to carry the ball
                ChoosePlayerToCarryBall();
                match = MatchStates.Possesion;

                yield return new WaitForSeconds(_actionTimer);

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
                HomeTeam.Wins++;
                AwayTeam.Loses--;
            }
            else if (HomeTeam.Score < AwayTeam.Score)
            {
                HomeTeam.Moral -= 15;
                AwayTeam.Moral += 15;
                HomeTeam.Loses--;
                AwayTeam.Wins++;
            }
            else
            {
                HomeTeam.Moral -= 5;
                AwayTeam.Moral -= 5;
                HomeTeam.Draws++;
                AwayTeam.Draws++;
            }
        }
        for (int i = 0; i < manager.leagueTeams.Count; i++)
        {
            manager.leagueTeams[i].HasPlayed = false;
        }

        yield return null;
    }

    void ChangePosOfPlayerWithTheBall()
    {
        // Clear HasTheBall from all players on both teams
        foreach (Player p in HomeTeam.playersListRoster)
            p.HasTheBall = false;

        foreach (Player p in AwayTeam.playersListRoster)
            p.HasTheBall = false;

        // Set the player who currently has the ball
        playerWithTheBall.HasTheBall = true;

        // Find the index of the first element with IsActive == true
        int index = teamWithball.playersListRoster.FindIndex(element => element.HasTheBall);

        if (index > 0) // found and not already first
        {
            // Swap the element at index with the first element
            Player temp = teamWithball.playersListRoster[0];
            teamWithball.playersListRoster[0] = teamWithball.playersListRoster[index];
            teamWithball.playersListRoster[index] = temp;
        }
        print(playerWithTheBall.playerFirstName + " is hte guy with the ball");
    }
}
