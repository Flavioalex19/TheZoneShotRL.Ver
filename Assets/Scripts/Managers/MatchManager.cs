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
    [SerializeField] Player playerDefending;
    bool playerTeamWin = false;

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

    [Header("PlayersActions")]
    #region PlayerActions
    [SerializeField]public bool _ChoosePass;
    [SerializeField]public bool _ChooseScoring;
    [SerializeField] public bool _ChooseToSpecialAtt;
    [SerializeField] Button btn_spAttck;
    public bool CanChooseAction = true;
    public int _sp_numberOfSPActions;
    #endregion
    //Ai variables
    int ai_maxNumberOfPasses = 5;
    int ai_currentNumberOfPasses =0;
    float ai_difficulty = 0.75f;

    //UI Elemens test

    [Header("Debugs")]
    [SerializeField] TextMeshProUGUI _debugTimeoutText;
    [SerializeField] string DefenderName;

    bool _isOnSetupStage;//
    private void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HomeTeam = manager.playerTeam;
        _sp_numberOfSPActions = manager.playerTeam.FansSupportPoints / 20;
        //print(_sp_numberOfSPActions + " Here");
    }
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UiManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        _isOnSetupStage = true;
        playerTeamWin = false;
        //Reset the teams to play
        for (int i = 0; i < manager.leagueTeams.Count; i++)
        {
            manager.leagueTeams[i].HasPlayed = false;
        }
        
        match = MatchStates.Start;
        currentGamePossessons = GamePossesions;

        HomeTeam = manager.playerTeam;

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
            HomeTeam.playersListRoster[i].StealsMatch = 0;
        }
        for (int i = 0; i < AwayTeam.playersListRoster.Count; i++)
        {
            AwayTeam.playersListRoster[i].PointsMatch = 0;
            AwayTeam.playersListRoster[i].StealsMatch= 0;
        }
        CanChooseAction = false;
        _matchUI.SetSkillPints();
        //StartCoroutine(GameFlow());
        StartCoroutine(RunMatchThenSimulate());
        //_matchUI.PostGameStats(HomeTeam, AwayTeam);
    }
    private void Update()
    {
        if (_sp_numberOfSPActions <= 0)
        {
            btn_spAttck.interactable = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (_canCallTimeout)
        {
            _debugTimeoutText.text = "Can Call Timeout";


        }

    }
    public void StartTheMatch()
    {
        _isOnSetupStage = false;
    }
    IEnumerator SetupGameplan()
    {
        //Wait for the intro to end
        for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
        {
            HomeTeam.playersListRoster[i].CurrentStamina = 100;
        }
        for (int i = 0; i < AwayTeam.playersListRoster.Count; i++)
        {
            AwayTeam.playersListRoster[i].CurrentStamina = 100;
        }

        yield return new WaitUntil(()=> _isOnSetupStage == false);
    }
    IEnumerator GameFlow()
    {
        HomeTeam.HasPlayed = true;
        AwayTeam.HasPlayed = true;
        //Stamina set fro the game
        for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
        {
            HomeTeam.playersListRoster[i].CurrentStamina = 100;
            HomeTeam.playersListRoster[i].PointsMatch = 0;
            HomeTeam.playersListRoster[i].StealsMatch = 0;
        }
        for (int i = 0;i < AwayTeam.playersListRoster.Count; i++) 
        {
            AwayTeam.playersListRoster[i].CurrentStamina = 100;
            AwayTeam.playersListRoster[i].PointsMatch = 0;
            AwayTeam.playersListRoster[i].StealsMatch = 0;
        }
        _matchUI.MatchStartAnim();
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
            AwayTeam.Loses++;
            if (HomeTeam.IsPlayerTeam) { _matchUI.ActivateVictoryDefeat("Victory"); }
            
        }
        else if (HomeTeam.Score < AwayTeam.Score)
        {
            HomeTeam.Moral -= 15;
            AwayTeam.Moral += 15;
            HomeTeam.Loses++;
            AwayTeam.Wins++;
            if (HomeTeam.IsPlayerTeam) { _matchUI.ActivateVictoryDefeat("Defeat"); }
        }
        else
        {
            HomeTeam.Moral -= 5;
            AwayTeam.Moral -= 5;
            HomeTeam.Draws++;
            AwayTeam.Draws++;
            if (HomeTeam.IsPlayerTeam) { _matchUI.ActivateVictoryDefeat("Draw"); }
        }
        HomeTeam.HasPlayed = true;
        AwayTeam.HasPlayed = true;
        //Set career stats
        for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
        {
            HomeTeam.playersListRoster[i].CareerPoints += HomeTeam.playersListRoster[i].PointsMatch;
            HomeTeam.playersListRoster[i].CareerSteals += HomeTeam.playersListRoster[i].StealsMatch;
            HomeTeam.playersListRoster[i].CareerGamesPlayed++;
        }
        for (int i = 0; i < AwayTeam.playersListRoster.Count; i++)
        {
            AwayTeam.playersListRoster[i].CareerPoints += AwayTeam.playersListRoster[i].PointsMatch;
            AwayTeam.playersListRoster[i].CareerSteals += AwayTeam.playersListRoster[i].StealsMatch;
            AwayTeam.playersListRoster[i].CareerGamesPlayed++;
        }
        //Reduce the contract for the players
        if (HomeTeam.IsPlayerTeam)
        {
            print("Count Team");
            for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
            {
                HomeTeam.playersListRoster[i].ContractYears--;
            }
        }
        yield return new WaitForSeconds(3f);
        _matchUI.EndScreenStatsPanel.SetActive(true);
        
        //_matchUI.PostGameStats(HomeTeam, AwayTeam);///////////////////////////////////////////////
        //yield return StartCoroutine(LeagueWeekSimulation());
    }
    void ChoosePlayerToCarryBall()
    {
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
            //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " has the ball.");
            uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
        }
        ChangePosOfPlayerWithTheBall();
        SelectDefender();
        //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " has the ball.");
        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
    }
    IEnumerator ChooseToPass()
    {
        yield return null;
    }
    IEnumerator HandlePossession()
    {
        if(/*teamWithball == AwayTeam*/ teamWithball.IsPlayerTeam == false)
        {
            CanChooseAction = false;
            ai_currentNumberOfPasses = ai_maxNumberOfPasses;
            
            while (true)
            {
                CanChooseAction = false;
                if (currentGamePossessons <= 1)
                {
                    uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " must shoot due to low possessions!");
                    yield return new WaitForSeconds(_actionTimer);
                    yield return Scoring(playerWithTheBall,true);
                    yield break;
                }

                bool shouldPass = Random.Range(1, 4) < 3;//Change Later

                if (shouldPass && ai_currentNumberOfPasses > 0)
                {
                    ai_currentNumberOfPasses--;
                    if (TryPassBall())
                    {
                        
                        yield return new WaitForSeconds(_actionTimer);
                        //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " prepares for next action.");
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        SelectDefender();
                        yield return new WaitForSeconds(_actionTimer);
                        continue; // Keep the loop for multiple passes
                    }
                    else
                    {
                        yield return new WaitForSeconds(_actionTimer);
                        SwitchPossession();
                        //uiManager.PlaybyPlayText(teamWithball.TeamName + " has the ball.");
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        yield return new WaitForSeconds(_actionTimer);
                        yield break;
                    }
                }
                else
                {
                    ChangePosOfPlayerWithTheBall();
                    //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " goes for the score!");
                    uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ShootingText());
                    yield return new WaitForSeconds(_actionTimer);
                    //yield return Scoring(playerWithTheBall);
                    ///
                    if (TryBeatDefender(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone))
                    {
                        yield return Scoring(playerWithTheBall, true);
                        //ResetChoices();
                        yield break;
                    }
                    else
                    {
                        playerDefending.StealsMatch++;
                        print(playerDefending.playerLastName + "Has " + playerDefending.StealsMatch + " Steals");
                        StaminaLossByDefender(playerWithTheBall);
                        print("Fail to pass by");
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.LosesPos() + " Loses the ball to " + playerDefending.playerLastName);
                        yield return new WaitForSeconds(_actionTimer);
                        SwitchPossession();
                        yield break;
                    }
                    //yield break;
                }
            }
        }
        else if (/*teamWithball == HomeTeam*/ teamWithball.IsPlayerTeam)
        {
            if (currentGamePossessons > 1)
            {
                ResetChoices();
            }
                
            while (true)
            {
                //CanChooseAction = true;
                if (currentGamePossessons <= 1)
                {
                    uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " must shoot due to low possessions!");
                    yield return new WaitForSeconds(_actionTimer);
                    yield return Scoring(playerWithTheBall, false);
                    yield break;
                }
                uiManager.PlaybyPlayText("Wait for Player Action");
                // Wait until player makes a choice
                yield return new WaitUntil(() => _ChoosePass || _ChooseScoring || _ChooseToSpecialAtt);

                if (_ChooseScoring)
                {
                    _ChooseScoring = false;
                    _matchUI.ActionPanelAnim(2, "Shoot");
                    yield return new WaitForSeconds(_actionTimer);
                    ChangePosOfPlayerWithTheBall();
                    //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " goes for the score!");
                    uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ShootingText());
                    yield return new WaitForSeconds(_actionTimer);
                    ///!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    if (TryBeatDefender(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone))
                    {
                        yield return Scoring(playerWithTheBall, false);
                        //ResetChoices();
                        yield break;
                    }
                    else
                    {
                        playerDefending.StealsMatch++;
                        StaminaLossByDefender(playerWithTheBall);
                        print("Fail to pass by defender");
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName +" " + _matchUI.LosesPos()  + " Loses the ball to " + playerDefending.playerLastName);
                        yield return new WaitForSeconds(_actionTimer);
                        SwitchPossession();
                        //ResetChoices();
                        yield break;
                    }
                    /*
                    yield return Scoring(playerWithTheBall);
                    //ResetChoices();
                    yield break;
                    */
                }
                else if (_ChoosePass)
                {
                    _ChoosePass = false;
                    _matchUI.ActionPanelAnim(0, "Passing");
                    yield return new WaitForSeconds(_actionTimer);
                    if (TryPassBall())
                    {
                        yield return new WaitForSeconds(_actionTimer);
                        //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " prepares for next action.");
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        SelectDefender();
                        yield return new WaitForSeconds(_actionTimer);
                        ResetChoices();
                        continue;
                    }
                    else
                    {
                        //print("Fail to pass by");
                        playerDefending.StealsMatch++;
                        print(playerDefending.playerLastName + "Has " + playerDefending.StealsMatch + " Steals");
                        yield return new WaitForSeconds(_actionTimer);
                        SwitchPossession();
                        //uiManager.PlaybyPlayText(teamWithball.TeamName + " has the ball.");
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        yield return new WaitForSeconds(_actionTimer);
                        //ResetChoices();/////////////////
                        yield break;
                    }
                }
                else if (_ChooseToSpecialAtt)
                {
                    _ChooseToSpecialAtt = false;
                    _sp_numberOfSPActions--;
                    _matchUI.SetSkillPints();
                    _matchUI.ActionPanelAnim(1, "Special");
                    yield return new WaitForSeconds(_actionTimer);
                    if (Random.Range(0f, 1f) < ActivateSpecialAttk())
                    {
                        int spPoints = 8;
                        //Add later the list of string for a success use o team abillity
                        uiManager.PlaybyPlayText("The team will use their special abillity");
                        yield return new WaitForSeconds(_actionTimer);
                        switch (teamWithball._teamStyle)
                        {
                            case TeamStyle.Brawler:
                                playerWithTheBall.PointsMatch += spPoints;
                                teamWithball.Score += spPoints;
                                break;
                            case TeamStyle.PhaseDash:
                                playerWithTheBall.PointsMatch += spPoints;
                                teamWithball.Score += spPoints;
                                break;
                            case TeamStyle.RailShot:
                                playerWithTheBall.PointsMatch += spPoints;
                                teamWithball.Score += spPoints;
                                break;
                            case TeamStyle.HyperDribbler:
                                playerWithTheBall.PointsMatch += spPoints;
                                teamWithball.Score += spPoints;
                                break;
                            default: break;
                        }
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + "has scored!");
                        
                        yield return new WaitForSeconds(_actionTimer);
                        SwitchPossession();
                        yield break;
                    }
                    else
                    {
                        //Change this later for a list of string for a fail event
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + "Fail to use special team trait");
                        
                        yield return new WaitForSeconds(_actionTimer);
                        SwitchPossession();
                        yield break;
                    }


                }
            }
        }
    }
    void ResetChoices()
    {
        _ChoosePass = false;
        _ChooseScoring = false;
        _ChooseToSpecialAtt = false;
        CanChooseAction = true;
    }
    public void GetChoosePass()
    {
        _ChoosePass = true;
        CanChooseAction = false;
    }
    public void GetChooseScoring()
    {
        _ChooseScoring = true;
        CanChooseAction = false;
    }
    public void UseSpecialAttk()
    {
        _ChooseToSpecialAtt = true;
        CanChooseAction = false;
    }
    bool TryPassBall()
    {
        //float passSuccessChance = Mathf.Clamp((playerWithTheBall.Awareness - 30f) / (99f - 30f), 0f, 1f);

        if (Random.Range(0f, 1f) >/*> passSuccessChance*/ PassEquation())
        {
            //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " made a bad pass! Possession lost.");
            uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " " + _matchUI.LosesPos());
            //ResetChoices();
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
    IEnumerator Scoring(Player player, bool isAI)
    {
        while (true)
        {
            bool willShoot = Random.Range(1, 4) < 3; // Adjust logic as needed create a function base on the inside, mid and out
            if (willShoot)
            {
                uiManager.PlaybyPlayText(player.playerFirstName + " takes a shot!");
                yield return new WaitForSeconds(_actionTimer);
                //currentGamePossessons--;
                bool hasScored =Random.Range(0f, 1f) < ScoringEquation(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone, isAI);
                print(hasScored + " is  the result of Shooting");
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
                player.CurrentZone = 0;
                SwitchPossession(); // Switch possession after shot attempt
                yield return new WaitForSeconds(_actionTimer);
                break; // Exit loop if shooting
            }
            else
            {
                if (currentGamePossessons <= 1)
                {
                    //print("Only one possession left, player must shoot!");
                    uiManager.PlaybyPlayText(player.playerFirstName + " must shoot due to low possessions!");
                    continue; // Skip movement logic, forcing the player to attempt a shot
                }
                int newZone = Random.Range(player.CurrentZone, 3); // Restrict movement to current or forward zones
                if (newZone > player.CurrentZone) // Only update if moving forward
                {
                    player.CurrentZone = newZone;
                    uiManager.PlaybyPlayText(player.playerFirstName + " moves to zone " + player.CurrentZone);
                    yield return new WaitForSeconds(_actionTimer);
                    //currentGamePossessons--;

                }
                else willShoot = true;
            }
        }

    }
    IEnumerator StunPlayer()
    {
        yield return new WaitForSeconds(_actionTimer);
    }
    void SwitchPossession()
    {
        ControlStamina(teamWithball);
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
        SelectDefender();
        if(HomeTeam.IsPlayerTeam)_matchUI.ChangePos(HomeTeam);

    }
    void SelectDefender()
    {
        //print("CHOOSE DEFENDER!!!!!!!!!!!!!!!!!!!!!!!");
        Team DefendingTeam;
        if (HomeTeam.hasPossession == false)
            DefendingTeam = HomeTeam;
        else
            DefendingTeam = AwayTeam;

        // Pick a random player (excluding index 0)
        int randomIndex = Random.Range(1, 4); // avoid 0
        Player newDefender = DefendingTeam.playersListRoster[randomIndex];

        // Swap the random player with the one at index 0
        Player temp = DefendingTeam.playersListRoster[0];
        DefendingTeam.playersListRoster[0] = newDefender;
        DefendingTeam.playersListRoster[randomIndex] = temp;

        //newDefender.CurrentZone = 0;
        DefenderName = newDefender.playerFirstName + " " + DefendingTeam.TeamName;
        playerDefending = newDefender;
        //Debug.Log(newDefender.playerFirstName + " is now defending.");
    }
    //Beat the defender and change zones
    bool TryBeatDefender(Player offense, Player defense, int zone, int bonus = 0)
    {
        // Offense roll
        int offenseRoll = UnityEngine.Random.Range(1, 101)
                        + offense.Consistency
                        + offense.Control
                        + offense.Juking
                        + GetZoneValue(offense, zone)
                        + UnityEngine.Random.Range(-5, 15) // small variability
                        + bonus;

        // Defense roll
        int defenseRoll = UnityEngine.Random.Range(1, 101)
                        + defense.Consistency
                        + defense.Guarding
                        + defense.Stealing
                        + GetZoneValue(defense, zone);

        int difference = offenseRoll - defenseRoll;

        // --- Adjusted thresholds ---
        if (difference > 20)
            return true;   // Successful juke
        else if (difference < -20)
            return false;  // Defender shuts down (could extend to "steal" here)
        else
            return UnityEngine.Random.value < 0.4f;
        // 40% chance nothing happens, 60% chance juke succeeds
    }
    int GetZoneValue(Player player, int zone)
    {
        switch (zone)
        {
            case 0: return player.Inside;
            case 1: return player.Mid;
            case 2: return player.Outside;
            default: return 0;
        }
    }
    //Function to enable call for a Timeout
    IEnumerator WaitForTimeOut()
    {
        //yield return new WaitForSeconds(5);
        if (_canCallTimeout == false)
        {
            IsOnTimeout = true;
            yield return new WaitUntil(() => _canCallTimeout == true);
            IsOnTimeout = false;
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
            //match = MatchStates.Timeout;
            //IsOnTimeout = true;
            _canCallTimeout = false;
            _debugTimeoutText.text = "Timeout Called!";
        }

    }
    public void ResumeTimeout()
    {
        _canCallTimeout = true;
    }
    public bool GetCanCallTimeout()
    {
        return _canCallTimeout;
    }
    public IEnumerator LeagueWeekSimulation()
    {
        int gameIndex = 0;
        int week = leagueManager.Week;

        Debug.Log($"[SIMULATION] Starting week {week}");

        while (manager.leagueTeams.Any(t => !t.HasPlayed))
        {
            Team teamA = manager.leagueTeams.FirstOrDefault(t => !t.HasPlayed);
            Team teamB = teamA._schedule[leagueManager.Week - 1];
            Debug.Log($"[SIMULATING] Match {gameIndex}: {teamA.TeamName} vs {teamB.TeamName}");

            // Setup
            HomeTeam = teamA;
            AwayTeam = teamB;
            teamWithball = HomeTeam;
            _actionTimer = 0f;
            currentGamePossessons = GamePossesions;

            teamA.Score = teamB.Score = 0;
            teamA.HasPlayed = teamB.HasPlayed = true;
            teamA.isOnDefenseBonus = teamB.isOnDefenseBonus = false;

            foreach (Player p in teamA.playersListRoster) { p.PointsMatch = 0;p.StealsMatch = 0; }

            foreach (Player p in teamB.playersListRoster) { p.PointsMatch = 0; p.StealsMatch = 0; } 

            yield return StartCoroutine(GameFlow());

            Debug.Log($"[RESULT] {teamA.TeamName} {teamA.Score} - {teamB.Score} {teamB.TeamName}");
            _matchUI.WeekResults(gameIndex, teamA, teamB);

            gameIndex++;
        }

        Debug.Log("[SIMULATION COMPLETE]");

        // Reset flags after all matches
        foreach (var team in manager.leagueTeams)
            team.HasPlayed = false;

        yield return null;

    }
    IEnumerator RunMatchThenSimulate()
    {
        yield return StartCoroutine(SetupGameplan());
        yield return StartCoroutine(GameFlow());
        _matchUI.PostGameStats(HomeTeam, AwayTeam);
        //Do only for yur team-cahnge if  the others need after 3 games
        if (leagueManager.Week % 3 == 0) ApplyFiveWeekTraining(manager.playerTeam, manager.playerTeam.Wins, manager.playerTeam.Draws, manager.playerTeam.Loses);
        _matchUI.UpgradeTeamAnim();
        yield return StartCoroutine(LeagueWeekSimulation());
        //Enable progress button
        _matchUI.btn_ReturnToTeamManagement.gameObject.SetActive(true);

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
        //print(playerWithTheBall.playerFirstName + " is hte guy with the ball");
    }
    //Equations
    float PassEquation(bool isAI = false)
    {
        float offenseScore = (playerWithTheBall.Awareness + playerWithTheBall.Consistency) / 2f;
        float defenseScore = (playerDefending.Stealing + playerDefending.Guarding) / 2f;

        float offenseNormalized = Mathf.Clamp((offenseScore - 30f) / (99f - 30f), 0f, 1f);
        float defenseNormalized = Mathf.Clamp((defenseScore - 30f) / (99f - 30f), 0f, 1f);

        float passSuccessChance = offenseNormalized / (offenseNormalized + defenseNormalized);

        //return passSuccessChance;
        // Apply AI coefficient only if AI
        return isAI ? passSuccessChance * ai_difficulty : passSuccessChance;
    }
    float ScoringEquation(Player offense, Player defense, int zone, bool isAI = false)
    {
        // 1. Base Zone Accuracy
        float baseAccuracy = 0.7f; // default mid
        switch (zone)
        {
            case 0: baseAccuracy = 0.64f; break; // Outside
            case 1: baseAccuracy = 0.70f; break; // Mid
            case 2: baseAccuracy = 0.75f; break; // Inside
        }

        // 2. Offense Value (X)
        int offenseValue =
            UnityEngine.Random.Range(1, 101) +  // variance
            offense.Consistency +
            offense.Control +
            offense.Shooting +
            GetZoneValue(offense, zone) +
            UnityEngine.Random.Range(-10, 26); // bonus

        // 3. Defense Value (Y)
        int defenseValue =
            UnityEngine.Random.Range(1, 101) +  // variance
            defense.Consistency +
            defense.Guarding +
            defense.Stealing +
            GetZoneValue(defense, zone);

        int Z = offenseValue - defenseValue;

        // 4. Modify accuracy based on result
        if (Z >= 300 && Z <= 356) baseAccuracy += 0.18f;
        else if (Z >= 200 && Z <= 299) baseAccuracy += 0.15f;
        else if (Z >= 150 && Z <= 199) baseAccuracy += 0.12f;
        else if (Z >= 100 && Z <= 149) baseAccuracy += 0.08f;
        else if (Z >= 50 && Z <= 99) baseAccuracy += 0.05f;
        else if (Z >= 0 && Z <= 49) baseAccuracy += 0f;
        else if (Z >= -50 && Z <= -1) baseAccuracy -= 0.06f;
        else if (Z >= -100 && Z <= -51) baseAccuracy -= 0.12f;
        else if (Z <= -101 && Z >= -356) return 0f; // Block

        // 5. Clamp result between 0%–100%
        //return Mathf.Clamp01(baseAccuracy);
        // Apply AI coefficient only if AI
        return isAI ? Mathf.Clamp01(baseAccuracy * ai_difficulty) : Mathf.Clamp01(baseAccuracy);
    }
    float ActivateSpecialAttk()
    {
        float specialAttkSuccess = 0f;
        float offenseScore;
        float defenseScore;
        float offenseNormalized;
        float defenseNormalized;

        // Normalize fanSupport (0 to 100) to a 0–1 range
        float fanSupportNormalized = Mathf.Clamp(teamWithball.FansSupportPoints / 100f, 0f, 1f);
        // Dynamic risk penalty: ranges from 0.4 (fanSupport=0) to 0.1 (fanSupport=100)
        float riskPenalty = 0.4f - (0.3f * fanSupportNormalized);

        switch (teamWithball._teamStyle)
        {
            case TeamStyle.Brawler:
                offenseScore = (playerWithTheBall.Shooting + playerWithTheBall.Positioning) / 2f;
                defenseScore = (playerDefending.Defending + playerDefending.Guarding) / 2f;
                break;
            case TeamStyle.RailShot:
                offenseScore = (playerWithTheBall.Awareness + playerWithTheBall.Outside) / 2f;
                defenseScore = (playerDefending.Positioning + playerDefending.Guarding) / 2f;
                break;
            case TeamStyle.PhaseDash:
                offenseScore = (playerWithTheBall.Juking + playerWithTheBall.Control) / 2f;
                defenseScore = (playerDefending.Awareness + playerDefending.Stealing) / 2f;
                break;
            case TeamStyle.HyperDribbler:
                offenseScore = (playerWithTheBall.Juking + playerWithTheBall.Consistency) / 2f;
                defenseScore = (playerDefending.Awareness + playerDefending.Guarding) / 2f;
                break;
            default:
                return 0f; // Fail if team style is undefined
        }

        // Normalize scores 
        offenseNormalized = Mathf.Clamp(offenseScore / 100f, 0f, 1f);
        defenseNormalized = Mathf.Clamp(defenseScore / 100f, 0f, 1f);

        // Sigmoid-based formula for volatility, adjusted by dynamic risk penalty
        float scoreDifference = offenseNormalized - defenseNormalized;
        specialAttkSuccess = 1f / (1f + Mathf.Exp(-6f * scoreDifference));
        specialAttkSuccess = Mathf.Clamp(specialAttkSuccess - riskPenalty, 0.05f, 0.95f);
        return specialAttkSuccess;
    }
    //Stamina managers
    void ControlStamina(Team team)
    {
        int staminaLoss = 5;
        for (int i = 0; i < 4; i++)
        {
            if (team.playersListRoster[i].HasTheBall)
            {
                team.playersListRoster[i].CurrentStamina -= staminaLoss*2;
            }
            else
            {
                team.playersListRoster[i].CurrentStamina -= staminaLoss;
            }
        }
    }
    void StaminaLossByDefender(Player player)
    {
        int staminaLoss = 15;
        player.CurrentStamina-=staminaLoss;

    }
    //UpdateStatsAfter5Weeks
    public void ApplyFiveWeekTraining(Team team, int wins, int draws, int losses)
    {
        // Performance factor: team success influences growth
        float performanceFactor = (wins * 1.5f + draws * 0.5f - losses * 0.5f);

        // Equipment factor
        int equipmentLevel = team._equipmentList[4].Level; // Assuming this is an int
        float equipmentBonus = 1f + (equipmentLevel * 0.1f); // +10% per level

        foreach (Player player in team.playersListRoster)
        {
            // Base growth
            int growthMin = 1;
            int growthMax = 5;

            // Adjust based on personality (1–5)
            // Lower values = easier going, less ambitious
            // Higher values = more competitive, more ambitious
            switch (player.Personality)
            {
                case 1: // Cool guy
                    growthMax = 3;
                    break;
                case 2:
                    growthMax = 4;
                    break;
                case 3: // Balanced
                        // no change
                    break;
                case 4:
                    growthMin = 2;
                    break;
                case 5: // Competitive/difficult
                    growthMin = 3;
                    break;
            }

            // Pick random growth
            int growthValue = UnityEngine.Random.Range(growthMin, growthMax + 1);

            // Scale with performance + equipment
            growthValue = Mathf.RoundToInt(growthValue * performanceFactor * equipmentBonus);

            // Keep it realistic
            growthValue = Mathf.Clamp(growthValue, 0, 5);

            // Apply to player stats (example attributes)
            player.Awareness = Mathf.Min(99, player.Awareness + growthValue);
            player.Consistency = Mathf.Min(99, player.Consistency + growthValue);
            player.Shooting = Mathf.Min(99, player.Shooting + growthValue);
            player.Control = Mathf.Min(99, player.Control + growthValue);
            player.Guarding = Mathf.Min(99, player.Guarding + growthValue);
            player.Stealing = Mathf.Min(99, player.Stealing + growthValue);
            player.Defending = Mathf.Min(99, player.Defending + growthValue);
            player.Juking = Mathf.Min(99, player.Juking + growthValue);
            player.Inside = Mathf.Min(99, player.Inside + growthValue);
            player.Mid = Mathf.Min(99, player.Mid + growthValue);
            player.Outside = Mathf.Min(99, player.Outside + growthValue);
            player.Positioning = Mathf.Min(99, player.Positioning + growthValue);


        }
    }
}
