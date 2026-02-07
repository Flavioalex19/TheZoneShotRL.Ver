using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    [SerializeField] MatchSfxManager sfxManager;
    [SerializeField] MatchUI _matchUI;
    MatchStates match;
    public int GamePossesions = 10;
    public int currentGamePossessons;
    public Team HomeTeam;
    public Team AwayTeam;
    [SerializeField] public Team teamWithball;
    [SerializeField] public Player playerWithTheBall;
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
    [SerializeField] public bool _ChooseBeatDefender;
    public int passPlayerIndex;
    [SerializeField] Button btn_spAttck;
    public bool CanChooseAction = true;
    public bool CanChooseDefenseAction = true;
    public bool chooseDefenseTackle = false;
    public bool chooseBlock = false;
    public bool chooseInterception = false;
    public int _sp_numberOfSPActions;
    #endregion
    //Ai variables
    int ai_maxNumberOfPasses = 5;
    int ai_currentNumberOfPasses =0;
    float ai_difficulty = 1.75f;

    //cards buff
    public float buff_Atk;
    public float buff_Defense;
    public float buff_Pass;
    public float buff_SP;
    public float buff_Stamina;
    public float buff_Juke;

    float jukePercentage = 0;
    bool emptyBool = false;

    private List<string> eventsList = new List<string>()
    {
        "None",
        "Player Injury",
        "Cheerleader Event",
        "Crowd Event",
        "Mascot Event",
        "Skill Power Event",
        "Fans booing"
    };
    [Header("Events Variables")]
    
    

    [Header("Cards Variables")]
    [SerializeField] Transform cardsFolder;
    public Transform cards_cardsUsedFolder;
    public Transform cards_handCards;
    public int mod_Attk = 0;
    public int mod_Def = 0;
    public int momentum = 0;
    public bool canUseCards = true;
    [SerializeField] GameObject cardsPanel;
    //UI Elemens test

    [Header("Debugs")]
    [SerializeField] TextMeshProUGUI _debugTimeoutText;
    [SerializeField] string DefenderName;
    public bool isSimulation = false;

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
        CanChooseDefenseAction = false;
        momentum = 0;
        buff_Atk = 0;
        buff_Defense = 0;
        buff_Pass = 0;
        buff_SP = 0;
        buff_Stamina = 0;
        buff_Juke = 0;
        //Reset the teams to play
        for (int i = 0; i < manager.leagueTeams.Count; i++)
        {
            manager.leagueTeams[i].HasPlayed = false;
        }
        
        isSimulation = false;

        match = MatchStates.Start;
        currentGamePossessons = GamePossesions;

        HomeTeam = manager.playerTeam;
        if (leagueManager.isOnR8 && leagueManager.isOnR4 == false)
        {
            AwayTeam = leagueManager.List_R8Teams[1];
        }
        else if (leagueManager.isOnR4 && leagueManager.isOnR8 == true && leagueManager.isOnFinals == false)
        {
            AwayTeam = leagueManager.List_R4Teams[1];
        }
        else if (leagueManager.isOnFinals && leagueManager.isOnR4 && leagueManager.isOnR8 == true)
        {
            AwayTeam = leagueManager.List_Finalist[1];
        }
        else
        {
            for (int i = 0; i < manager.leagueTeams.Count; i++)
            {
                if (manager.leagueTeams[i] == manager.playerTeam._schedule[leagueManager.Week - 1])
                {
                    AwayTeam = manager.leagueTeams[i];
                }
            }
        }
        _matchUI.TeamImagesUpdate();
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
            HomeTeam.playersListRoster[i].isInjured = false;
        }
        for (int i = 0; i < AwayTeam.playersListRoster.Count; i++)
        {
            AwayTeam.playersListRoster[i].PointsMatch = 0;
            AwayTeam.playersListRoster[i].StealsMatch= 0;
            AwayTeam.playersListRoster[i].isInjured= false;
        }
        CanChooseAction = false;
        _matchUI.SetSkillPints();
        //StartCoroutine(GameFlow());
        //Testing
        print(AwayTeam.playersListRoster.Count + "Number of player i the roster of the away team");
        StartCoroutine(RunMatchThenSimulate());

        if (leagueManager.isOnR8)
        {
            if(leagueManager.isOnR4 == false)
            {
                //leagueManager.isOnR4 = true;
            }
                    }
        //_matchUI.PostGameStats(HomeTeam, AwayTeam);
    }
    private void Update()
    {
        if (_sp_numberOfSPActions <= 0)
        {
            btn_spAttck.interactable = false;
        }
        if(playerWithTheBall!=null)
        print(playerWithTheBall.CurrentStamina + " Mine stamina");
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
        HomeTeam.hasHDefense = false;
        momentum = 0;
        //Stamina set fro the game
        for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
        {
            HomeTeam.playersListRoster[i].CurrentStamina = 100;
            HomeTeam.playersListRoster[i].PointsMatch = 0;
            HomeTeam.playersListRoster[i].StealsMatch = 0;
            HomeTeam.playersListRoster[i].isInjured = false;
            HomeTeam.playersListRoster[i].HasTheBall = false;
            HomeTeam.playersListRoster[i].statBuff = 0;
            HomeTeam.playersListRoster[i].statDefBuff = 0;
        }
        for (int i = 0;i < AwayTeam.playersListRoster.Count; i++) 
        {
            AwayTeam.playersListRoster[i].CurrentStamina = 100;
            AwayTeam.playersListRoster[i].PointsMatch = 0;
            AwayTeam.playersListRoster[i].StealsMatch = 0;
            AwayTeam.playersListRoster[i].isInjured = false;
            AwayTeam.playersListRoster[i].HasTheBall = false;
            AwayTeam.playersListRoster[i].statBuff= 0;
            AwayTeam.playersListRoster[i].statDefBuff = 0;
        }
        _matchUI.MatchStartAnim();
        while (currentGamePossessons > 0)
        {
            // Step 1: Choose the player to carry the ball
            ChoosePlayerToCarryBall();
            match = MatchStates.Possesion;

            if (!isSimulation) yield return new WaitForSeconds(_actionTimer);

            // Step 2: Player decides to pass or not
            //yield return ChooseToPass();
            yield return HandlePossession();

            // Step 3: Wait for final actions (e.g., scoring)
            if (!isSimulation) yield return StartCoroutine(WaitForTimeOut());

            //Restore stamina on the bench
            if(currentGamePossessons>1) RestoreStaminaFromBench();
            //currentGamePossessons--;

            if ((leagueManager.isOnR8 == true || leagueManager.isOnR4 == true || leagueManager.isOnFinals == true) && currentGamePossessons <= 1)
            {
                if (HomeTeam.Score == AwayTeam.Score)
                {
                    currentGamePossessons++;
                }
            }
        }

        // End of match logic
        uiManager.PlaybyPlayText("MatchEnded");

        if (HomeTeam.Score > AwayTeam.Score)
        {
            AwayTeam.Moral -= 15;
            HomeTeam.Moral += 15;
            HomeTeam.Wins++;
            AwayTeam.Loses++;
            if (HomeTeam.IsPlayerTeam) 
            { 
                _matchUI.ActivateVictoryDefeat("Victory");
                if (leagueManager.isOnR8 && leagueManager.isOnR4 == false && leagueManager.isOnFinals == false)
                {
                    //leagueManager.isOnR4 = true;
                    print("Pass to R4!!!!!!!!!!!!!!!!!!!");
                    leagueManager.List_R4Teams.Add(HomeTeam);
                    //leagueManager.isOnR8 = false;
                }
                else if (leagueManager.isOnR4 && leagueManager.isOnR8 == true && leagueManager.isOnFinals == false)
                {
                    //leagueManager.isOnFinals = true;
                    leagueManager.List_Finalist.Add(HomeTeam);
                }
                else if (leagueManager.isOnR4 && leagueManager.isOnR8 == true && leagueManager.isOnFinals)
                {
                    //player team tema a varaivel win
                    HomeTeam.isChampion = true;
                    leagueManager.CanStartANewRun = true;
                    //leagueManager.isGameOver = true;
                }
            }
            
        }
        else if (HomeTeam.Score < AwayTeam.Score)
        {
            HomeTeam.Moral -= 15;
            AwayTeam.Moral += 15;
            HomeTeam.Loses++;
            AwayTeam.Wins++;
            if (HomeTeam.IsPlayerTeam) 
            {
                if (!isSimulation) _matchUI.ActivateVictoryDefeat("Defeat");
                if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
                {
                    leagueManager.isGameOver = true;
                    //leagueManager.isOnR8 = false;
                }
            }
            
        }
        else
        {
            HomeTeam.Moral -= 5;
            AwayTeam.Moral -= 5;
            HomeTeam.Draws++;
            AwayTeam.Draws++;
            if (HomeTeam.IsPlayerTeam) { _matchUI.ActivateVictoryDefeat("Draw"); }
        }
        
        yield return new WaitForSeconds(2f);
        _matchUI.anim_victoryCircle.SetTrigger("Go");
        //stats increment
        if (HomeTeam.IsPlayerTeam)
        {
            // OFFICE  Front Office Points
            if (HomeTeam.OfficeLvl >= 0)
            {
                float bonus = 2f * (1f + HomeTeam.OfficeLvl * 0.5f);
                HomeTeam.FrontOfficePoints += Mathf.RoundToInt(bonus);
            }

            // FINANCES  Salary Cap
            if (HomeTeam.FinancesLvl >= 0)
            {
                float bonus = 2f * (1f + HomeTeam.FinancesLvl * 0.5f);
                HomeTeam.SalaryCap += Mathf.RoundToInt(bonus);
            }

            // MARKETING  Fan Support
            if (HomeTeam.MarketingLvl >= 0)
            {
                float bonus = 3f * (1f + HomeTeam.MarketingLvl * 0.5f);
                HomeTeam.FansSupportPoints += Mathf.RoundToInt(bonus);
            }

            // ARENA  Morale
            if (HomeTeam.ArenaLvl >= 0)
            {
                float bonus = 3f * (1f + HomeTeam.ArenaLvl * 0.5f);
                HomeTeam.Moral += Mathf.RoundToInt(bonus);
                HomeTeam.Moral = Mathf.Clamp(HomeTeam.Moral, 0, 100);
            }

            // MEDICAL  Effort / Stamina / Recovery bonus
            if (HomeTeam.MedicalLvl >= 0)
            {
                float bonus = 3f * (1f + HomeTeam.MedicalLvl * 0.5f);
                HomeTeam.EffortPoints += Mathf.RoundToInt(bonus);
            }

        }
        HomeTeam.HasPlayed = true;
        AwayTeam.HasPlayed = true;
        //Set career stats
        for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
        {
            HomeTeam.playersListRoster[i].CareerPoints += HomeTeam.playersListRoster[i].PointsMatch;
            HomeTeam.playersListRoster[i].CareerSteals += HomeTeam.playersListRoster[i].StealsMatch;
            HomeTeam.playersListRoster[i].CareerGamesPlayed++;
            HomeTeam.playersListRoster[i].buff = 0;
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
            //print("Count Team");
            for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
            {
                HomeTeam.playersListRoster[i].ContractYears--;
            }
        }
        yield return new WaitForSeconds(5f);
        if (!isSimulation) _matchUI.EndScreenStatsPanel.SetActive(true);
        //_matchUI.PostGameStats(HomeTeam, AwayTeam);///////////////////////////////////////////////
        //yield return StartCoroutine(LeagueWeekSimulation());
    }
    void ChoosePlayerToCarryBall()
    {
        if(playerWithTheBall!=null) playerWithTheBall.HasTheBall = false;

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
        playerWithTheBall.HasTheBall = true;
        if (playerWithTheBall != null)
        {
            playerWithTheBall.CurrentZone = 0;
            playerWithTheBall.HasTheBall = true;
            //ChangePosOfPlayerWithTheBall();!!!!!!!!!!!!!!!!!!!!!
            //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " has the ball.");
            if(!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
        }
        //ChangePosOfPlayerWithTheBall();!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        SelectDefender();
        //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " has the ball.");
        if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
        
    }
    IEnumerator ChooseToPass()
    {
        yield return null;
    }
    IEnumerator HandlePossession()
    {
        if(/*teamWithball == AwayTeam*/ teamWithball.IsPlayerTeam == false)
        {
            if (!isSimulation) _matchUI.OffesnivePanelOnOff(false);
            CanChooseAction = false;
            
            ai_currentNumberOfPasses = ai_maxNumberOfPasses;

            for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
            {
                HomeTeam.playersListRoster[i].CurrentZone = 0;
            }
            if (!isSimulation) _matchUI.UpdatePlayerPlacements();

            while (true)
            {
                if (!isSimulation) _matchUI.percentagePanel.SetActive(false);
                CanChooseAction = false;
                if ((leagueManager.isOnR8 == true || leagueManager.isOnR4 == true || leagueManager.isOnFinals == true)&& currentGamePossessons <=1)
                {
                    if (HomeTeam.Score == AwayTeam.Score)
                    {
                        currentGamePossessons++;
                    }
                }

                if (currentGamePossessons <= 1)
                {
                    if(!isSimulation)uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " must shoot due to low possessions!");
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    yield return Scoring(playerWithTheBall,true);
                    currentGamePossessons--;
                    yield break;
                }
                else if (_canCallTimeout == false)
                {
                    yield return StartCoroutine(WaitForTimeOut());
                    if(!isSimulation)CheckAndSwapLowStaminaPlayers(AwayTeam);
                    for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
                    {
                        HomeTeam.playersListRoster[i].HasTheBall = false;
                        AwayTeam.playersListRoster[i].HasTheBall = false;
                    }
                    ChoosePlayerToCarryBall();
                    yield break;
                }
                if (HomeTeam.IsPlayerTeam)
                {
                    //Defense wait for choice
                    CanChooseDefenseAction = true;
                    chooseDefenseTackle = false;
                    chooseBlock = false;
                    //Timeout
                    if (_canCallTimeout == false)
                    {
                        if (!isSimulation) yield return StartCoroutine(WaitForTimeOut());
                        ChoosePlayerToCarryBall();
                        yield break;
                    }
                    if (!isSimulation) yield return WaitForDefenseAction();
                    CanChooseDefenseAction = false;
                    //chooseDefenseTackle = false;
                }


                //bool shouldPass = Random.Range(1, 4) < 3;//Change Later
                Team teamWithoutTheBall = null;
                if(HomeTeam.IsPlayerTeam == false)
                {
                    if (HomeTeam.hasPossession) teamWithoutTheBall = AwayTeam;
                    else teamWithoutTheBall = HomeTeam;
                }
                else
                {
                    teamWithoutTheBall = HomeTeam;
                }
                bool shoudScore = AI_ShouldTryToScore(teamWithball.Score, teamWithoutTheBall.Score, currentGamePossessons);

                if (shoudScore==false && ai_currentNumberOfPasses > 0)
                {
                    ai_currentNumberOfPasses--;
                    if (TryPassBall())
                    {

                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " prepares for next action.");
                        if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        SelectDefender();
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        ResetDefensiveOptions();//Defensife oprions for the player
                        continue; // Keep the loop for multiple passes
                    }
                    else
                    {
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        playerWithTheBall.HasTheBall = false;
                        SwitchPossession();
                        //uiManager.PlaybyPlayText(teamWithball.TeamName + " has the ball.");
                        if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        ResetDefensiveOptions();//Defensife oprions for the player
                        yield break;
                    }
                }
                else
                {
                    //ChangePosOfPlayerWithTheBall();
                    //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " goes for the score!");
                    if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ShootingText());
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    //yield return Scoring(playerWithTheBall);
                    ///
                    if (TryBeatDefender(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone))
                    {
                        yield return Scoring(playerWithTheBall, true);
                        //ResetChoices();
                        ResetDefensiveOptions();//Defensife oprions for the player
                        yield break;
                    }
                    else
                    {
                        playerDefending.StealsMatch++;
                        currentGamePossessons--;
                        print(playerDefending.playerLastName + "Has " + playerDefending.StealsMatch + " Steals");
                        //StaminaLossByDefender(playerWithTheBall);
                        if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.LosesPos() + " Loses the ball to " + playerDefending.playerLastName);
                        playerWithTheBall.HasTheBall = false;
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        ResetDefensiveOptions();//Defensife oprions for the player
                        SwitchPossession();
                        yield break;
                    }
                    //yield break;
                }
            }
        }
        else if (/*teamWithball == HomeTeam*/ teamWithball.IsPlayerTeam)
        {
            //_matchUI.OffesnivePanelOnOff(true);
            CanChooseDefenseAction = false;

            
            
            ResetPostions();
            _matchUI.PlayerWithBallButtonsOnOff();
            

            if (currentGamePossessons > 1)
            {
                ResetChoices();
                if (cardsFolder.childCount > 3)
                {
                    CreateHand();
                    if (!isSimulation) _matchUI.UpdateCardsHand();
                }
            }   
            while (true)
            {
                if (!isSimulation) _matchUI.percentagePanel.SetActive(true);
                if (!isSimulation) _matchUI.PlayerWithTheBallOff();
                if (!isSimulation) _matchUI.PlayerWithBallButtonsOnOff();
                if (!isSimulation) _matchUI.UpdatePlayerPlacements();
                //MatchEvents();
                //CanChooseAction = true;
                if ((leagueManager.isOnR8 == true || leagueManager.isOnR4 == true || leagueManager.isOnFinals == true) && currentGamePossessons <= 1)
                {
                    if (HomeTeam.Score == AwayTeam.Score)
                    {
                        currentGamePossessons++;
                    }
                }
                /*
                if (currentGamePossessons <= 1)
                {
                    if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " must shoot due to low possessions!");
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    if (!isSimulation) yield return Scoring(playerWithTheBall, false);
                    currentGamePossessons--;
                    yield break;
                }
                */
                //old cards creations!!!!!!!!!!!!!!!!!!!!!
                _matchUI.OffesnivePanelOnOff(true);
                //print(GetScoringChance(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone, false) + " Is the cahnce of success");
                _matchUI.SetScoringPercentage(ScoringEquation(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone,0));
                _matchUI.SetPassPercentage(PassEquation());
                _matchUI.SetJukePercentage(/*TryBeatDefenderAdvanceZone(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone, 0)*/GetJukePercentage(playerWithTheBall,playerDefending,playerWithTheBall.CurrentZone));
                _matchUI.SetSpPercentage(GetSpAttackPercentage().ToString() + "%");
                //_matchUI.SetJukePercentage();
                uiManager.PlaybyPlayText("Wait for Player Action");
                //Timeout call
                //yield return StartCoroutine(WaitForTimeOut());
                // Wait until player makes a choice
                yield return new WaitUntil(() => _ChoosePass || _ChooseScoring || _ChooseToSpecialAtt || _ChooseBeatDefender ||_canCallTimeout == false);

                if (_ChooseScoring)
                {
                    _ChooseScoring = false;
                    _matchUI.ActionPanelAnim(2, "Shoot");
                    playerWithTheBall.HasTheBall = false;
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    //ChangePosOfPlayerWithTheBall();
                    //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " goes for the score!");
                    uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ShootingText());
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    //yield return Scoring(playerWithTheBall, false);!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    if (!isSimulation) yield return ToScore(playerWithTheBall,playerDefending, HomeTeam);
                    //ResetChoices();
                    if (!isSimulation) yield break;
                    
                }
                else if (_ChoosePass)
                {
                    _ChoosePass = false;
                    _matchUI.ActionPanelAnim(0, "Passing");
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    if (/*TryPassBall()*/ MakePassToTeammate(passPlayerIndex))
                    {
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " prepares for next action.");
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        SelectDefender();
                        _matchUI.ResultActionPanel("S",1);
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        ResetChoices();
                        continue;
                    }
                    else
                    {
                        playerDefending.StealsMatch++;
                        playerWithTheBall.HasTheBall = false;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        SwitchPossession();
                        _matchUI.OffesnivePanelOnOff(false);
                        _matchUI.ResultActionPanel("F", 1);
                        //uiManager.PlaybyPlayText(teamWithball.TeamName + " has the ball.");
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
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
                    playerWithTheBall.HasTheBall = false;
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    if (Random.Range(0f, 1f) < ActivateSpecialAttk())
                    {
                        int spPoints = 8;
                        //Add later the list of string for a success use o team abillity
                        uiManager.PlaybyPlayText("The team will use their special abillity");
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
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
                        _matchUI.ResultActionPanel("S",0);
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        SwitchPossession();
                        yield break;
                    }
                    else
                    {
                        //Change this later for a list of string for a fail event
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + "Fail to use special team trait");
                        _matchUI.ResultActionPanel("F",0);
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        SwitchPossession();
                        yield break;
                    }
                    

                }
                else if (_ChooseBeatDefender)
                {
                    if (TryBeatDefenderAdvanceZone(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone))
                    {
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + "Pass the defender");
                        SelectDefender();
                        _matchUI.ResultActionPanel("S",2);
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        ResetChoices();
                        continue;
                    }
                    else
                    {
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.LosesPos());
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        playerWithTheBall.HasTheBall = false;
                        _matchUI.ResultActionPanel("F",2);
                        SwitchPossession();
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        yield break;
                    }
                }
                else if(_canCallTimeout == false)
                {
                    if (!isSimulation) yield return StartCoroutine(WaitForTimeOut());
                    CheckAndSwapLowStaminaPlayers(AwayTeam);
                    for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
                    {
                        HomeTeam.playersListRoster[i].HasTheBall = false;
                        AwayTeam.playersListRoster[i].HasTheBall = false;
                    }
                    ChoosePlayerToCarryBall();
                    yield break;
                }
            }
            
        }
        
    }
    void ResetChoices()
    {
        _ChoosePass = false;
        _ChooseScoring = false;
        _ChooseToSpecialAtt = false;
        _ChooseBeatDefender = false;
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
    public void GetChooseBeatDefender()
    {
        _ChooseBeatDefender = true;
        CanChooseAction= false;
    }
    public void UseSpecialAttk()
    {
        _ChooseToSpecialAtt = true;
        CanChooseAction = false;
    }
    //Defense
    IEnumerator WaitForDefenseAction()
    {
        yield return new WaitUntil(() => { return chooseDefenseTackle|| chooseBlock; });
    }
    public void ChooseTackle()
    {
        CanChooseDefenseAction = false;
        chooseDefenseTackle = true;
    }
    public void ChooseBlock()
    {
        CanChooseDefenseAction = false;
        chooseBlock = true;
    }
    public void ChooseInterception()
    {
        CanChooseDefenseAction = false;
        chooseInterception = true;
    }
    public void ResetDefensiveOptions()
    {
        CanChooseDefenseAction = false;
        chooseDefenseTackle = false;
        chooseBlock = false;
        chooseInterception = false;
    }
    bool TryPassBall()
    {
        //float passSuccessChance = Mathf.Clamp((playerWithTheBall.Awareness - 30f) / (99f - 30f), 0f, 1f);

        if (Random.Range(0f, 1f) >/*> passSuccessChance*/ PassEquation())
        {
            //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " made a bad pass! Possession lost.");
            if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.LosesPos());
            playerWithTheBall.HasTheBall = false;
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

        //ChangePosOfPlayerWithTheBall();
        if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " receives the pass.");
        return true;
    }
    IEnumerator Scoring(Player player, bool isAI)
    {
       
        // Jogador escolhe a zona (considerando a preferida)
        player.CurrentZone = ChooseZone(player);

        if (!isSimulation) uiManager.PlaybyPlayText(player.playerLastName + " takes a shot from zone " + player.CurrentZone);
        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);

        //if is AI the momentum is 0
        int teamMomentum;
        if (teamWithball == HomeTeam) teamMomentum = momentum;
        else teamMomentum = 0;

        //print(teamMomentum + " is the teamMomentum" + teamWithball.TeamName);

        bool hasScored = Random.Range(0f, 1f) < ScoringEquation(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone, teamMomentum);
        //print(hasScored + " is the result of Shooting");

        if (hasScored)
        {
            if (player.CurrentZone == 0)
            {
                player.PointsMatch += 4;
                teamWithball.Score += 4;
            }
            else if (player.CurrentZone == 1)
            {
                player.PointsMatch += 5;
                teamWithball.Score += 5;
            }
            else
            {
                player.PointsMatch += 6;
                teamWithball.Score += 6;
            }
            if (!isSimulation) uiManager.PlaybyPlayText(player.playerLastName + " Has Scored " + " " + player.PointsMatch);
            
        }
        else
        {
            if (!isSimulation) uiManager.PlaybyPlayText(player.playerLastName + " Missed");
        }
        //currentGamePossessons--;
        playerWithTheBall.HasTheBall = false;
        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
        player.CurrentZone = 0;
        SwitchPossession();
        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
    }
    int ChooseZone(Player player)
    {
        // 60% de chance do jogador ir para a zona preferida
        if (Random.Range(0f, 1f) < 0.6f)
            return player.Zone;

        // Caso contrário, escolhe aleatoriamente entre 0 e 2
        return 0;
    }
    IEnumerator StunPlayer()
    {
        yield return new WaitForSeconds(_actionTimer);
    }
    void SwitchPossession()
    {
        if (!isSimulation) _matchUI.OffesnivePanelOnOff(false);
        ControlStamina(HomeTeam);
        ControlStamina(AwayTeam);
        playerWithTheBall = null;
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
        if (!isSimulation) if (HomeTeam.IsPlayerTeam)_matchUI.ChangePos(HomeTeam);
        
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
        /*
        //currentGamePossessons--;
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
        */
       
        // === MÉDIA DOS 3 ATRIBUTOS PRINCIPAIS ===
        float offenseAvg = (offense.Consistency + offense.Control + offense.Juking) / 3f;
        float defenseAvg = (defense.Consistency + defense.Guarding + defense.Stealing) / 3f;

        // === ZONA: quanto menor o valor, mais fácil pro atacante ===
        int zoneValue = GetZoneValue(offense, zone);
        // Inverte: zona 0 = +30 bônus, zona alta (ex: 5) = pouco ou nenhum bônus
        float zoneBonus = 30f - (zoneValue * 5f); // ajusta o multiplicador se quiser mais/menos impacto
        zoneBonus = Mathf.Max(zoneBonus, 0f); // nunca negativo

        // Total final do atacante
        float finalOffense = offenseAvg + zoneBonus + bonus;

        // === COMPARATIVO SIMPLES ===
        float difference = finalOffense - defenseAvg;

        // Thresholds equilibrados
        if (difference > 15f)
            return true;                    // Ataque claramente superior  juke sucesso
        else if (difference < -15f)
            return false;                   // Defesa claramente superior  steal/denied
        else
        {
            // Neutro: 50/50 + leve bias pela diferença (favorece quem tá na frente)
            float neutralChance = 0.5f + (difference / 60f); // difference +15  ~75%, -15  ~25%
            return UnityEngine.Random.value < Mathf.Clamp01(neutralChance);
        }
        
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
            //ChoosePlayerToCarryBall();

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
            isSimulation = true;
            //yield return StartCoroutine(GameFlow());
            SimulateMatchInstant(teamA, teamB);
            isSimulation = false;

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
    public IEnumerator SimulatePlayoffRound()
    {
        List<Team> sourceList = null;
        List<Team> targetList = null;

        // Define round atual
        if (leagueManager.isOnR8 && !leagueManager.isOnR4)
        {
            print("Quarters!!!!!!!!!!!!!!!!!");
            sourceList = leagueManager.List_R8Teams;
            targetList = leagueManager.List_R4Teams;
        }
        else if (leagueManager.isOnR4 && !leagueManager.isOnFinals)
        {
            sourceList = leagueManager.List_R4Teams;
            targetList = leagueManager.List_Finalist;
        }
        else
        {
            yield break;
        }

        //targetList.Clear();

        int gameIndex = 0;

        // Enquanto houver times que ainda não jogaram neste round
        while (sourceList.Any(t => !t.HasPlayed))
        {
            // Pega o próximo par (ordem da lista)
            var unplayed = sourceList.Where(t => !t.HasPlayed).ToList();
            Team teamA = unplayed[0];
            Team teamB = unplayed[1];

            Debug.Log($"[PLAYOFF] Match {gameIndex}: {teamA.TeamName} vs {teamB.TeamName}");

            // ===== SETUP COMPLETO (igual temporada regular) =====
            HomeTeam = teamA;
            AwayTeam = teamB;
            teamWithball = HomeTeam;
            _actionTimer = 0f;
            currentGamePossessons = GamePossesions;

            teamA.Score = teamB.Score = 0;
            teamA.HasPlayed = teamB.HasPlayed = true;
            teamA.isOnDefenseBonus = teamB.isOnDefenseBonus = false;

            foreach (Player p in teamA.playersListRoster)
            {
                p.PointsMatch = 0;
                p.StealsMatch = 0;
            }

            foreach (Player p in teamB.playersListRoster)
            {
                p.PointsMatch = 0;
                p.StealsMatch = 0;
            }
            // ================================================

            // Simula o jogo
            yield return StartCoroutine(GameFlow());

            // UI (mesmo método da temporada regular)
            _matchUI.WeekResults(gameIndex, teamA, teamB);

            // Define vencedor e avança
            Team winner = teamA.Score >= teamB.Score ? teamA : teamB;
            targetList.Add(winner);

            gameIndex++;
        }

        // Reset para o próximo round
        foreach (Team t in sourceList)
            t.HasPlayed = false;

        if(leagueManager.isOnR4== false)leagueManager.isOnR4 = true;
        else leagueManager.isOnFinals = true;
        manager.saveSystem.SaveLeague();
    }

    IEnumerator RunMatchThenSimulate()
    {
        
        yield return StartCoroutine(SetupGameplan());
        yield return StartCoroutine(GameFlow());
        _matchUI.PostGameStats(HomeTeam, AwayTeam);

        // run full week simulation FIRST (other teams)
        //yield return StartCoroutine(LeagueWeekSimulation());
        if (leagueManager.isOnR8 || leagueManager.isOnR4)
        {
            print("We are in the playoffs");
            // Estamos nos playoffs  simula APENAS o round atual
            yield return StartCoroutine(SimulatePlayoffRound());
        }
        else
        {
            // Temporada regular  simulação normal da semana
            yield return StartCoroutine(LeagueWeekSimulation());
        }

        // Now do the 3-week upgrade/animation (after simulations finished)
        if (leagueManager.Week % 4 == 0)
        {
            ApplyFiveWeekTraining(manager.playerTeam, manager.playerTeam.Wins, manager.playerTeam.Draws, manager.playerTeam.Loses);
            _matchUI.UpgradeTeamAnim();
            // Wait properly for the animation/coroutine to finish
            yield return StartCoroutine(waitSecondsForAction());
        }
        for (int i = 0; i < manager.leagueTeams.Count; i++)
        {
            if (manager.leagueTeams[i]!= manager.playerTeam)
            {
                //ApplyFiveWeekTraining(manager.leagueTeams[i], manager.leagueTeams[i].Wins, manager.leagueTeams[i].Draws, manager.leagueTeams[i].Loses);
            }
        }
        sfxManager.ResetVolume();
        //Enable progress button
        _matchUI.btn_ReturnToTeamManagement.gameObject.SetActive(true);
    }
    //TESTING NEW SIMULATIONS
    private void SimulateMatchInstant(Team teamA, Team teamB)
    {
        // Setup básico (igual ao GameFlow)
        HomeTeam = teamA;
        AwayTeam = teamB;
        teamWithball = HomeTeam;
        HomeTeam.hasPossession = true;
        AwayTeam.hasPossession = false;
        currentGamePossessons = GamePossesions;
        teamA.Score = teamB.Score = 0;
        momentum = 0;

        // Zera stats dos players
        foreach (Player p in teamA.playersListRoster)
        {
            p.PointsMatch = 0;
            p.StealsMatch = 0;
            p.CurrentStamina = 100; // opcional, se quiser simular stamina
            p.HasTheBall = false;
        }
        foreach (Player p in teamB.playersListRoster)
        {
            p.PointsMatch = 0;
            p.StealsMatch = 0;
            p.CurrentStamina = 100;
            p.HasTheBall = false;
        }

        // Loop principal de posses (igual ao while em GameFlow)
        while (currentGamePossessons > 0)
        {
            // Escolhe jogador com a bola
            ChoosePlayerToCarryBall();
            SelectDefender(); // mantém defensor atualizado

            // Simula posse (AI simples: mistura passes e tentativas de score)
            int passesThisPossession = Random.Range(0, ai_maxNumberOfPasses + 1); // 0 a max passes
            bool possessionEnded = false;

            // Passes múltiplos (se decidir passar)
            for (int passAttempt = 0; passAttempt < passesThisPossession && !possessionEnded; passAttempt++)
            {
                if (TryPassBall())
                {
                    // Pass success  continua posse, escolhe novo portador
                    ChoosePlayerToCarryBall();
                    SelectDefender();
                }
                else
                {
                    // Pass fail steal  switch
                    playerDefending.StealsMatch++;
                    playerWithTheBall.HasTheBall = false;
                    SwitchPossession();
                    possessionEnded = true;
                }
            }

            // Se não acabou com passes, tenta score
            if (!possessionEnded)
            {
                if (TryBeatDefender(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone))
                {
                    // Juke success  tenta score
                    playerWithTheBall.CurrentZone = ChooseZone(playerWithTheBall); // zona preferida
                    int teamMomentum = (teamWithball == HomeTeam) ? momentum : 0;

                    if (Random.value < ScoringEquation(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone, teamMomentum))
                    {
                        int points = (playerWithTheBall.CurrentZone == 0) ? 4 : (playerWithTheBall.CurrentZone == 1 ? 5 : 6);
                        playerWithTheBall.PointsMatch += points;
                        teamWithball.Score += points;
                    }
                    // Miss  nada, mas posse continua ou switch (pra simplificar, switch após tentativa)
                }
                else
                {
                    // Juke fail  steal
                    playerDefending.StealsMatch++;
                }

                playerWithTheBall.HasTheBall = false;
                SwitchPossession();
            }

            // Consome posse (igual ao teu código)
            currentGamePossessons--;
            RestoreStaminaFromBench(); // mantém se quiser

            // Overtime rule (playoffs)
            if ((leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals) && currentGamePossessons <= 1)
            {
                if (teamA.Score == teamB.Score) currentGamePossessons++;
            }
        }

        // Fim do jogo (exatamente igual ao teu GameFlow final)
        if (teamA.Score > teamB.Score)
        {
            teamB.Moral -= 15;
            teamA.Moral += 15;
            teamA.Wins++;
            teamB.Loses++;
            // Playoffs progress (copia teu código)
            // ...
        }
        else if (teamA.Score < teamB.Score)
        {
            teamA.Moral -= 15;
            teamB.Moral += 15;
            teamA.Loses++;
            teamB.Wins++;
            // ...
        }
        else
        {
            teamA.Moral -= 5;
            teamB.Moral -= 5;
            teamA.Draws++;
            teamB.Draws++;
        }

        // Career stats + contract reduce (copia teu código)
        foreach (Player p in teamA.playersListRoster)
        {
            p.CareerPoints += p.PointsMatch;
            p.CareerSteals += p.StealsMatch;
            p.CareerGamesPlayed++;
            p.buff = 0;
            if (teamA.IsPlayerTeam) p.ContractYears--; // só player team
        }
        foreach (Player p in teamB.playersListRoster)
        {
            p.CareerPoints += p.PointsMatch;
            p.CareerSteals += p.StealsMatch;
            p.CareerGamesPlayed++;
        }

        // Front Office / Finances / etc. bonuses (só se um dos times for playerTeam)
        if (teamA.IsPlayerTeam || teamB.IsPlayerTeam)
        {
            Team playerTeamInMatch = teamA.IsPlayerTeam ? teamA : teamB;
            // Copia teu bloco de bonuses (OfficeLvl, etc.)
            // ...
        }

        // HasPlayed flags
        teamA.HasPlayed = true;
        teamB.HasPlayed = true;
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
    float PassEquation()
    {
        /*
        float offenseScore = (playerWithTheBall.Awareness + playerWithTheBall.Consistency) / 2f;
        float defenseScore = (playerDefending.Stealing + playerDefending.Guarding) / 2f;

        // --- NOVO ---
        if (teamWithball.hasHDefense)
            defenseScore *= 1.1f; // +10% eficácia

        float offenseNormalized = Mathf.Clamp((offenseScore - 30f) / (99f - 30f), 0f, 1f);
        float defenseNormalized = Mathf.Clamp((defenseScore - 30f) / (99f - 30f), 0f, 1f);

        float passSuccessChance = offenseNormalized / (offenseNormalized + defenseNormalized);

        //Apply Buff and Bond
        if (playerWithTheBall.buff > 0)
            passSuccessChance *= 1.10f; // +10%

        if (playerWithTheBall.bondPlayer != null)
        {
            int bondIndex = teamWithball.playersListRoster.IndexOf(playerWithTheBall.bondPlayer);
            if (bondIndex >= 0 && bondIndex < 4) // bond está entre os 4 primeiros
                passSuccessChance *= 1.07f; // +7%
        }
        
        //return passSuccessChance;
        // Se a defesa escolheu "tackle", fica 20% mais difícil de o passe dar certo
        if (chooseDefenseTackle)
            passSuccessChance *= 0.8f; // reduz 20%
                                       // Apply AI coefficient only if AI
        if (!teamWithball.IsPlayerTeam)
            passSuccessChance *= ai_difficulty;

        //Buff de passe (só pro playerTeam, aplicado ao final) ===
        if (teamWithball.IsPlayerTeam && buff_Pass > 0)
        {
            float buffMultiplier = 1f + (buff_Pass / 100f); // ex: 2 - *1.02 (+2%)
            passSuccessChance *= buffMultiplier;
        }

        return passSuccessChance;
        */
        // === 1. Cálculo de OVR na hora (média dos 12 attrs) ===
        int offenseOVR = Mathf.RoundToInt(
            (playerWithTheBall.Shooting + playerWithTheBall.Inside + playerWithTheBall.Mid + playerWithTheBall.Outside +
             playerWithTheBall.Awareness + playerWithTheBall.Defending + playerWithTheBall.Guarding + playerWithTheBall.Stealing +
             playerWithTheBall.Juking + playerWithTheBall.Consistency + playerWithTheBall.Control + playerWithTheBall.Positioning) / 12f);

        int defenseOVR = Mathf.RoundToInt(
            (playerDefending.Shooting + playerDefending.Inside + playerDefending.Mid + playerDefending.Outside +
             playerDefending.Awareness + playerDefending.Defending + playerDefending.Guarding + playerDefending.Stealing +
             playerDefending.Juking + playerDefending.Consistency + playerDefending.Control + playerDefending.Positioning) / 12f);

        // === 2. Offense score (attrs relevantes + OVR bonus + random) ===
        float offenseAvg = (playerWithTheBall.Awareness + playerWithTheBall.Consistency + playerWithTheBall.Control +
                            offenseOVR / 5f) / 4f; // OVR dá bônus indireto

        float offenseScore = offenseAvg + UnityEngine.Random.Range(-10f, 15f);

        // === 3. Defense score (attrs defensivos + média extra forte + BONUS AI) ===
        float defenseAvg = (playerDefending.Stealing + playerDefending.Guarding + playerDefending.Awareness +
                            playerDefending.Consistency + defenseOVR / 5f) / 5f;

        float defenseMedianBonus = defenseAvg * 0.3f; // +30% da média defensiva (defesa forte)

        float defenseScore = defenseAvg + defenseMedianBonus;

        // REMOVIDO: bloco do hasHDefense

        // AI defende melhor (bônus extra quando AI é o defensor)
        Team defendingTeam = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
        if (!defendingTeam.IsPlayerTeam) // AI defendendo
            defenseScore *= 1.25f; // +25% defesa AI

        // === 4. Chance base (offense vs defense) ===
        float rawChance = offenseScore / (offenseScore + defenseScore + 40f); // +40 evita extremos

        // === 5. Modificadores (tackle) ===
        if (chooseDefenseTackle)
            rawChance *= 0.80f; // -20%

        // === 6. Bias principal: playerTeam mais difícil, AI mais fácil ===
        float finalChance = rawChance;

        if (teamWithball.IsPlayerTeam)
        {
            // PLAYER TEAM: mais dificuldade (precisa stats/buffs altos)
            finalChance *= 0.85f; // -15% base

            // Buff_Pass compensa (porcentagem)
            if (buff_Pass > 0)
                finalChance *= 1f + (buff_Pass / 100f);

            // ESPAÇO PRA MAIS BUFFS FUTUROS
            // ex: if (buff_Stamina > 0) finalChance *= 1f + (buff_Stamina / 100f);
        }
        else
        {
            // AI: mais fácil no passe
            finalChance *= 1.05f; // +5% base

            // Playoffs: AI ainda mais fácil
            if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
                finalChance *= 1.10f; // +10% extra AI playoffs
        }

        // === 7. Clamp final (tensão) ===
        return Mathf.Clamp(finalChance, 0.15f, 0.95f);
    }
    float ScoringEquation(Player offense, Player defense, int zone, int momentumModifier)
    {
        /*
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

        if (teamWithball.hasHDefense)
            defenseValue = (int)(defenseValue * 1.1f); // +10% eficácia

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

        // 5. Stamina factor (0 = sempre erra, 100 = bônus)
        float staminaFactor = offense.CurrentStamina / 100f;
        float staminaBonus = 1f + (offense.CurrentStamina / 500f); // até +20% no máximo
        baseAccuracy *= staminaFactor * staminaBonus;
        // 5. Clamp result between 0%–100%
        //return Mathf.Clamp01(baseAccuracy);
        // Apply AI coefficient only if AI
        //return isAI ? Mathf.Clamp01(baseAccuracy * ai_difficulty) : Mathf.Clamp01(baseAccuracy);
        // 6. Apply subtle momentum modifier
        switch (momentumModifier)
        {
            case 0: break; // neutro, sem impacto
            case 1: baseAccuracy *= 0.95f; break; // mais difícil (~-5%)
            case 2: baseAccuracy *= 1.05f; break; // mais fácil (~+5%)
            //case 3: baseAccuracy *= 0.60f; break;
        }
        if(playerWithTheBall.isInjured) baseAccuracy *= 0.60f;

        // --- ADIÇÃO: aplicar efeitos de buff e bond ---
        if (offense.buff > 0)
            baseAccuracy *= 1.10f; // +10%

        if (offense.bondPlayer != null)
        {
            int bondIndex = teamWithball.playersListRoster.IndexOf(offense.bondPlayer);
            if (bondIndex >= 0 && bondIndex < 4) // bond está entre os 4 primeiros
                baseAccuracy *= 1.07f; // +7%
        }
        
        if (chooseBlock)
            baseAccuracy *= 0.8f; // reduz 20%
        //return isAI ? Mathf.Clamp01(baseAccuracy * (1f / ai_difficulty)) : Mathf.Clamp01(baseAccuracy);
        if (!teamWithball.IsPlayerTeam)
        {
            baseAccuracy *= 1.45f; // IA arremessa 45% melhor
        }
        // === NOVO: Buff de passe (só pro playerTeam, aplicado ao final) ===
        if (teamWithball.IsPlayerTeam && buff_Atk > 0)
        {
            float buffMultiplier = 1f + (buff_Atk / 100f); // ex: 2  *1.02 (+2%)
            baseAccuracy *= buffMultiplier;
        }

        return Mathf.Clamp01(baseAccuracy);
        */
        // === 1. Cálculo de OVR na hora (média dos 12 attrs) ===
        int ovr = Mathf.RoundToInt(
            (offense.Shooting + offense.Inside + offense.Mid + offense.Outside +
             offense.Awareness + offense.Defending + offense.Guarding + offense.Stealing +
             offense.Juking + offense.Consistency + offense.Control + offense.Positioning) / 12f);

        // === 2. Base accuracy por zona (inside mais fácil) ===
        float baseAccuracy = zone switch
        {
            0 => 0.65f, // Outside
            1 => 0.72f, // Mid
            2 => 0.80f, // Inside
            _ => 0.70f
        };

        // === 3. Offense score (attrs ofensivos + zona + OVR bonus + random) ===
        float offenseAvg = (offense.Shooting + offense.Consistency + offense.Control +
                            GetZoneValue(offense, zone) + ovr / 5f) / 4f;

        float offenseScore = offenseAvg + UnityEngine.Random.Range(-10f, 20f);

        // === 4. Defense score (attrs defensivos + média extra + HDefense + BONUS AI) ===
        float defenseAvg = (defense.Guarding + defense.Stealing + defense.Awareness +
                            defense.Consistency + GetZoneValue(defense, zone)) / 5f;

        float defenseMedianBonus = defenseAvg * 0.3f; // +30% média defensiva (defesa forte geral)

        float defenseScore = defenseAvg + defenseMedianBonus;

        if (teamWithball != manager.playerTeam) // time sem bola (defendendo)
            defenseScore *= 1.20f; // +20% HDefense

        // NOVO: AI defende melhor (bônus extra quando AI é o defensor)
        Team defendingTeam = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
        if (!defendingTeam.IsPlayerTeam) // AI defendendo
            defenseScore *= 1.25f; // +25% defesa AI (ajusta se quiser mais/menos)

        // === 5. Chance base (offense vs defense) ===
        float rawChance = offenseScore / (offenseScore + defenseScore + 50f);

        // === 6. Modificadores gerais (stamina + block) ===
        float staminaFactor = offense.CurrentStamina / 100f;
        rawChance *= staminaFactor * (1f + offense.CurrentStamina / 400f); // até +25%

        if (chooseBlock) rawChance *= 0.80f;

        // === 7. Bias principal: playerTeam mais difícil no ataque, AI mais fácil ===
        float finalChance = rawChance;

        if (teamWithball.IsPlayerTeam)
        {
            // PLAYER TEAM atacando: mais dificuldade (precisa stats/buffs altos)
            finalChance *= 0.85f; // -15% base

            // Buff_Atk compensa fortemente (player precisa disso pra equilibrar)
            if (buff_Atk > 0)
                finalChance *= 1f + (buff_Atk / 100f); // ex: 10 = +10%

            // ESPAÇO PRA MAIS BUFFS (adicione aqui quando voltar)
            // ex: if (buff_Stamina > 0) finalChance *= 1f + (buff_Stamina / 100f);
        }
        else
        {
            // AI atacando: mais fácil
            finalChance *= 1.05f; // +5% base AI ataque

            // Playoffs: AI ainda mais forte no ataque/defesa
            if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
                finalChance *= 1.10f; // +10% extra AI playoffs
        }

        // === 8. Clamp final (tensão) ===
        return Mathf.Clamp(finalChance, 0.15f, 0.95f);
    }
    float ActivateSpecialAttk()
    {
        /*
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
        specialAttkSuccess *= Mathf.Clamp01((offenseScore - defenseScore + 20f) / 120f);
        specialAttkSuccess = Mathf.Clamp(specialAttkSuccess, 0.1f, 0.85f);
        currentGamePossessons--;
        return specialAttkSuccess;
        */
        float specialAttkSuccess = 0f;
        float offenseScore = 0f;
        float defenseScore = 0f;
        float offenseNormalized;
        float defenseNormalized;

        // Normalize fanSupport (0 to 100) to a 0–1 range
        float fanSupportNormalized = Mathf.Clamp(teamWithball.FansSupportPoints / 100f, 0f, 1f);

        // Dynamic risk penalty: ranges from 0.4 (fanSupport=0) to 0.1 (fanSupport=100)
        float riskPenalty = 0.4f - (0.3f * fanSupportNormalized);

        // === OFENSIVE SCORE: média dos 2 atributos principais do TeamStyle ===
        // === DEFENSIVE SCORE: usa atributos defensivos genéricos (Defending, Guarding, Stealing, Awareness) 
        //     – média de 4 pra ser consistente em todos os styles (defesa não varia por style no teu pedido) ===
        defenseScore = (playerDefending.Defending
                      + playerDefending.Guarding
                      + playerDefending.Stealing
                      + playerDefending.Awareness) / 4f;

        switch (teamWithball._teamStyle)
        {
            case TeamStyle.Normal:
                // Normal: sem bônus especial – usa atributos equilibrados (média geral de ataque)
                offenseScore = (playerWithTheBall.Shooting
                              + playerWithTheBall.Juking
                              + playerWithTheBall.Control
                              + playerWithTheBall.Positioning) / 4f;
                break;

            case TeamStyle.Brawler:
                offenseScore = (playerWithTheBall.Guarding + playerWithTheBall.Defending) / 2f;
                break;

            case TeamStyle.HyperDribbler:
                offenseScore = (playerWithTheBall.Control + playerWithTheBall.Juking) / 2f;
                break;

            case TeamStyle.PhaseDash:
                offenseScore = (playerWithTheBall.Juking + playerWithTheBall.Awareness) / 2f;
                break;

            case TeamStyle.RailShot:
                offenseScore = (playerWithTheBall.Shooting + playerWithTheBall.Positioning) / 2f;
                break;

            case TeamStyle.FutureSight:
                offenseScore = (playerWithTheBall.Awareness + playerWithTheBall.Positioning) / 2f;
                break;

            case TeamStyle.PerfectPlay:
                offenseScore = (playerWithTheBall.Positioning + playerWithTheBall.Control) / 2f;
                break;

            case TeamStyle.ShowTime:
                offenseScore = (playerWithTheBall.Consistency + playerWithTheBall.Positioning) / 2f;
                break;

            case TeamStyle.CloneAttack:
                offenseScore = (playerWithTheBall.Shooting + playerWithTheBall.Juking) / 2f;
                break;

            default:
                return 0f; // Falha total se style indefinido ou não suportado
        }

        // Normalize scores (atributos vão até 99, média máxima ~99)
        offenseNormalized = Mathf.Clamp(offenseScore / 99f, 0f, 1f);
        defenseNormalized = Mathf.Clamp(defenseScore / 99f, 0f, 1f);

        // Sigmoid-based formula for volatility
        float scoreDifference = offenseNormalized - defenseNormalized;
        specialAttkSuccess = 1f / (1f + Mathf.Exp(-6f * scoreDifference));

        // Aplica penalty de risco (fan support ajuda a reduzir)
        specialAttkSuccess = Mathf.Clamp(specialAttkSuccess - riskPenalty, 0.05f, 0.95f);

        // Multiplicador final baseado na diferença bruta (mantido do original, ajustado levemente pra escala)
        specialAttkSuccess *= Mathf.Clamp01((offenseScore - defenseScore + 20f) / 120f);

        // Clamp final pra manter tensão (nunca 0% ou 100%)
        specialAttkSuccess = Mathf.Clamp(specialAttkSuccess, 0.1f, 0.85f);

        currentGamePossessons--;
        return specialAttkSuccess;
    }
    //Chance succsefull defense choice
    //Percentages
    public float GetScoringChance(Player offense, Player defense, int zone, bool isAI = false)
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

        if (teamWithball.hasHDefense)
            defenseValue = (int)(defenseValue * 1.1f); // +10% eficácia

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

        // 5. Stamina factor (0 = sempre erra, 100 = bônus)
        float staminaFactor = offense.CurrentStamina / 100f;
        float staminaBonus = 1f + (offense.CurrentStamina / 500f); // até +20% no máximo
        baseAccuracy *= staminaFactor * staminaBonus;
        // 5. Clamp result between 0%–100%
        //return Mathf.Clamp01(baseAccuracy);
        // Apply AI coefficient only if AI
        //return isAI ? Mathf.Clamp01(baseAccuracy * ai_difficulty) : Mathf.Clamp01(baseAccuracy);
        // 6. Apply subtle momentum modifier
        
        if (playerWithTheBall.isInjured) baseAccuracy *= 0.60f;

        // --- ADIÇÃO: aplicar efeitos de buff e bond ---
        if (offense.buff > 0)
            baseAccuracy *= 1.10f; // +10%

        if (offense.bondPlayer != null)
        {
            int bondIndex = teamWithball.playersListRoster.IndexOf(offense.bondPlayer);
            if (bondIndex >= 0 && bondIndex < 4) // bond está entre os 4 primeiros
                baseAccuracy *= 1.07f; // +7%
        }

        if (chooseBlock)
            baseAccuracy *= 0.8f; // reduz 20%
        //return isAI ? Mathf.Clamp01(baseAccuracy * (1f / ai_difficulty)) : Mathf.Clamp01(baseAccuracy);
        if (!teamWithball.IsPlayerTeam)
        {
            baseAccuracy *= 1.45f; // IA arremessa 45% melhor
        }

        return Mathf.Round(Mathf.Clamp01(baseAccuracy) * 100f); // retorna em %
    }
    public float GetPassingChance(bool isAI = false)
    {
        float offenseScore = (playerWithTheBall.Awareness + playerWithTheBall.Consistency) / 2f;
        float defenseScore = (playerDefending.Stealing + playerDefending.Guarding) / 2f;

        // --- NOVO ---
        if (teamWithball.hasHDefense)
            defenseScore *= 1.1f; // +10% eficácia

        float offenseNormalized = Mathf.Clamp((offenseScore - 30f) / (99f - 30f), 0f, 1f);
        float defenseNormalized = Mathf.Clamp((defenseScore - 30f) / (99f - 30f), 0f, 1f);

        float passSuccessChance = offenseNormalized / (offenseNormalized + defenseNormalized);

        //Apply Buff and Bond
        if (playerWithTheBall.buff > 0)
            passSuccessChance *= 1.10f; // +10%

        if (playerWithTheBall.bondPlayer != null)
        {
            int bondIndex = teamWithball.playersListRoster.IndexOf(playerWithTheBall.bondPlayer);
            if (bondIndex >= 0 && bondIndex < 4) // bond está entre os 4 primeiros
                passSuccessChance *= 1.07f; // +7%
        }

        //return passSuccessChance;
        // Se a defesa escolheu "tackle", fica 20% mais difícil de o passe dar certo
        if (chooseDefenseTackle)
            passSuccessChance *= 0.8f; // reduz 20%
                                       // Apply AI coefficient only if AI
        if (!teamWithball.IsPlayerTeam)
            passSuccessChance *= ai_difficulty;

        //return passSuccessChance;
        // Apply AI coefficient only if AI
        //return isAI ? passSuccessChance * ai_difficulty : passSuccessChance;
        return Mathf.Round(Mathf.Clamp01(passSuccessChance) * 100);
    }
    public float GetBeatDefenderSuccessProbability(Player offense, Player defense, int zone, int bonus = 0)
    {
        // Calcula partes fixas (sem randoms)
        float fixedOff = offense.Consistency
                         + offense.Control
                         + offense.Juking
                         + GetZoneValue(offense, zone)
                         + bonus;

        float fixedDef = defense.Consistency
                         + defense.Guarding
                         + defense.Stealing
                         + GetZoneValue(defense, zone);

        // Médias dos randoms:
        // Roll base: +50.5 nos dois  cancela
        // Bonus offense (-5 a 14): média +4.5
        float avgDiff = fixedOff + 4.5f - fixedDef;

        // Estimativa da probabilidade baseada na lógica original
        float probability;
        if (avgDiff > 20)
        {
            probability = 1f; // quase garantido
        }
        else if (avgDiff < -20)
        {
            probability = 0f; // quase impossível
        }
        else
        {
            // No range -20 a 20: centra em 60%, inclina linear pra 20%-100%
            probability = 0.6f + (avgDiff / 40f) * 0.4f;
        }

        // Clamp final (nunca abaixo de 5% ou acima de 95% pra manter tensão)
        probability = Mathf.Clamp(probability, 0.05f, 0.95f);
        probability = Mathf.RoundToInt(probability * 100f);

        return probability; // ex: 0.75 = 75% chance
    }
    public float GetSpAttackPercentage()
    {
        float specialAttkSuccess = 0f;
        float offenseScore = 0f;
        float defenseScore = 0f;
        float offenseNormalized;
        float defenseNormalized;

        // Normalize fanSupport (0 to 100) to a 0–1 range
        float fanSupportNormalized = Mathf.Clamp(teamWithball.FansSupportPoints / 100f, 0f, 1f);

        // === OFENSIVE SCORE: média dos 2 atributos principais do TeamStyle ===
        // === DEFENSIVE SCORE: usa atributos defensivos genéricos (Defending, Guarding, Stealing, Awareness) 
        //     – média de 4 pra ser consistente em todos os styles (defesa não varia por style no teu pedido) ===
        defenseScore = (playerDefending.Defending
                      + playerDefending.Guarding
                      + playerDefending.Stealing
                      + playerDefending.Awareness) / 4f;

        switch (teamWithball._teamStyle)
        {
            case TeamStyle.Normal:
                // Normal: sem bônus especial – usa atributos equilibrados (média geral de ataque)
                offenseScore = (playerWithTheBall.Shooting
                              + playerWithTheBall.Juking
                              + playerWithTheBall.Control
                              + playerWithTheBall.Positioning) / 4f;
                break;

            case TeamStyle.Brawler:
                offenseScore = (playerWithTheBall.Guarding + playerWithTheBall.Defending) / 2f;
                break;

            case TeamStyle.HyperDribbler:
                offenseScore = (playerWithTheBall.Control + playerWithTheBall.Juking) / 2f;
                break;

            case TeamStyle.PhaseDash:
                offenseScore = (playerWithTheBall.Juking + playerWithTheBall.Awareness) / 2f;
                break;

            case TeamStyle.RailShot:
                offenseScore = (playerWithTheBall.Shooting + playerWithTheBall.Positioning) / 2f;
                break;

            case TeamStyle.FutureSight:
                offenseScore = (playerWithTheBall.Awareness + playerWithTheBall.Positioning) / 2f;
                break;

            case TeamStyle.PerfectPlay:
                offenseScore = (playerWithTheBall.Positioning + playerWithTheBall.Control) / 2f;
                break;

            case TeamStyle.ShowTime:
                offenseScore = (playerWithTheBall.Consistency + playerWithTheBall.Positioning) / 2f;
                break;

            case TeamStyle.CloneAttack:
                offenseScore = (playerWithTheBall.Shooting + playerWithTheBall.Juking) / 2f;
                break;

            default:
                return 0f; // Falha total se style indefinido ou não suportado
        }

        // Normalize scores (atributos vão até 99, média máxima ~99)
        offenseNormalized = Mathf.Clamp(offenseScore / 99f, 0f, 1f);
        defenseNormalized = Mathf.Clamp(defenseScore / 99f, 0f, 1f);

        // Sigmoid-based formula for volatility
        float scoreDifference = offenseNormalized - defenseNormalized;
        specialAttkSuccess = 1f / (1f + Mathf.Exp(-6f * scoreDifference));

        // Aplica penalty de risco (fan support ajuda a reduzir)
        specialAttkSuccess = Mathf.Clamp(specialAttkSuccess, 0.05f, 0.95f);

        // Multiplicador final baseado na diferença bruta (mantido do original, ajustado levemente pra escala)
        specialAttkSuccess *= Mathf.Clamp01((offenseScore - defenseScore + 20f) / 120f);

        // Clamp final pra manter tensão (nunca 0% ou 100%)
        specialAttkSuccess = Mathf.Clamp(specialAttkSuccess, 0.1f, 0.85f);

        // Converte para porcentagem inteira (10 a 85) e arredonda pro inteiro mais próximo
        int successPercentage = Mathf.RoundToInt(specialAttkSuccess * 100f);

        //currentGamePossessons--;
        return specialAttkSuccess;
    }
    //Stamina managers
    void ControlStamina(Team team)
    {
        int staminaLoss;
        if (team.hasHDefense == true && team != teamWithball)
        {
            staminaLoss = 10;
        }
        else
        {
            staminaLoss = 5;
        }
        
        for (int i = 0; i < 4; i++)
        {
            if (team.playersListRoster[i].Age < 25)
            {
                team.playersListRoster[i].CurrentStamina -= staminaLoss;
                
            }
            else if(team.playersListRoster[i].Age >= 25 && team.playersListRoster[i].Age < 30)
            {
                team.playersListRoster[i].CurrentStamina -= (staminaLoss + 5);
            }
            else
            {
                team.playersListRoster[i].CurrentStamina -= (staminaLoss + 10);
            }
            if (team.playersListRoster[i].CurrentStamina < 10) team.playersListRoster[i].CurrentStamina = 10;
           
        }
        //_matchUI.UpdatePlayersActive();
    }
    void StaminaLossByDefender(Player player)
    {
        int staminaLoss = 15;
        player.CurrentStamina-=staminaLoss;

    }
    void RestoreStaminaFromBench()
    {
        for (int i = 4;i< HomeTeam.playersListRoster.Count; i++)
        {
            if(HomeTeam.playersListRoster[i].CurrentStamina < 90 )
            HomeTeam.playersListRoster[i].CurrentStamina += 5;
            
        }
        for (int i = 0; i < AwayTeam.playersListRoster.Count; i++)
        {
            if (AwayTeam.playersListRoster[i].CurrentStamina < 90)
                AwayTeam.playersListRoster[i].CurrentStamina += 5;
        }
    }
    //UpdateStatsAfter5Weeks
    public void ApplyFiveWeekTraining(Team team, int wins, int draws, int losses)
    {
        // Performance factor: team success influences growth
        float performanceFactor = (wins * 1.5f + draws * 0.5f - losses * 0.5f);

        

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
            growthValue = Mathf.RoundToInt(growthValue * performanceFactor);

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

            player.UpdateOVR();
        }
    }
    public void ChangeTeamDefenseStyle(bool style)
    {
        HomeTeam.hasHDefense = style;
    }
    IEnumerator waitSecondsForAction()
    {
        yield return new WaitForSeconds(3f);
    }
    //Game EVENTS
    
    //Injury
    bool ChooseInjuryPlayer()
    {
        // Conta quantos já estão lesionados
        int injuredCount = HomeTeam.playersListRoster.Count(p => p.isInjured);

        // Se já houver 2 ou mais, cancela
        if (injuredCount >= 2)
            return false;

        // Escolhe aleatoriamente um jogador não lesionado (apenas entre os 4 primeiros)
        List<Player> healthyPlayers = HomeTeam.playersListRoster
            .Take(4)
            .Where(p => !p.isInjured)
            .ToList();

        // Se não houver jogadores saudáveis, cancela
        if (healthyPlayers.Count == 0)
            return false;

        // Escolhe aleatoriamente e aplica a lesão
        Player chosenPlayer = healthyPlayers[Random.Range(0, healthyPlayers.Count)];
        chosenPlayer.isInjured = true;

        return true;
    }
    int TryTriggerGameEvent(int currentGamePossessions, int totalGamePossessions, List<string> eventsList, Team homeTeam, Team awayTeam)
    {
        if (currentGamePossessions > totalGamePossessions * (2f / 3f))
            return -1;

        // Progresso da partida (0 = início, 1 = fim)
        float progress = 1f - ((float)currentGamePossessions / totalGamePossessions);

        // Chance de evento aumenta conforme o jogo avança
        float eventChance = Mathf.Lerp(0.2f, 1f, progress);

        // Sorteio para decidir se haverá evento nesta posse
        if (Random.value > eventChance)
            return -1;

        List<int> validEvents = new List<int>();

        // Verifica se há jogadores cansados (stamina < 60)
        bool lowStaminaExists = homeTeam.playersListRoster.Any(p => p.CurrentStamina < 60) ||
                                awayTeam.playersListRoster.Any(p => p.CurrentStamina < 60);
        if (lowStaminaExists)
            validEvents.AddRange(new int[] { 0, 1 });

        // Verifica diferença de pontos (8 ou mais)
        int scoreDiff = Mathf.Abs(homeTeam.Score - awayTeam.Score);
        if (scoreDiff >= 8)
            validEvents.AddRange(new int[] { 2, 3, 4 });

        // Se nenhuma condição foi satisfeita, adiciona os eventos restantes
        if (validEvents.Count == 0)
        {
            for (int i = 5; i < eventsList.Count; i++)
                validEvents.Add(i);
        }

        // Proteção extra: se a lista estiver vazia, não dispara evento
        if (validEvents.Count == 0)
            return -1;

        // Escolhe aleatoriamente um evento válido
        int randomEventIndex = validEvents[UnityEngine.Random.Range(0, validEvents.Count)];

        // Log de depuração opcional
        // Debug.Log($"[EVENT DEBUG] Possession: {currentGamePossessions}, Chosen Event Index: {randomEventIndex}");

        return randomEventIndex;
    }
    //Ai Eq to decision
    bool AI_ShouldTryToScore(int aiScore, int opponentScore, int currentPossessions)
    {
        // Base neutra: 50% de chance de tentar pontuar
        float chanceToScore = 0.5f;

        // Diferença de placar (positivo = IA vencendo)
        int scoreDiff = aiScore - opponentScore;

        // Se estiver perdendo -> aumenta a vontade de pontuar
        if (scoreDiff < 0)
        {
            // Cada ponto atrás aumenta 3% até um máximo de +30%
            chanceToScore += Mathf.Clamp(Mathf.Abs(scoreDiff) * 0.03f, 0f, 0.3f);
        }
        // Se estiver ganhando -> tende a jogar mais seguro
        else if (scoreDiff > 0)
        {
            // Cada ponto à frente reduz 2%, até um máximo de -20%
            chanceToScore -= Mathf.Clamp(scoreDiff * 0.02f, 0f, 0.2f);
        }

        // Ajuste pela quantidade de posses restantes
        if (currentPossessions <= GamePossesions/2)
            chanceToScore += 0.2f; // urgência
        else if (currentPossessions <= GamePossesions)
            chanceToScore += 0.1f;

        // Garante o limite entre 0 e 1
        chanceToScore = Mathf.Clamp01(chanceToScore);

        // Decide o resultado final
        return UnityEngine.Random.value < chanceToScore;
    }
    private void CheckAndSwapLowStaminaPlayers(Team aiTeam)
    {
        // Garante que há pelo menos 5 jogadores
        if (aiTeam.playersListRoster == null || aiTeam.playersListRoster.Count < 5)
            return;

        for (int i = 0; i < 4; i++) // verifica os titulares (0–3)
        {
            Player starter = aiTeam.playersListRoster[i];
            if (starter.CurrentStamina < 50f)
            {
                // Procura reserva com stamina > 50
                for (int j = 4; j < aiTeam.playersListRoster.Count; j++)
                {
                    Player bench = aiTeam.playersListRoster[j];
                    if (bench.CurrentStamina > 50f)
                    {
                        // Faz a troca de posição
                        aiTeam.playersListRoster[i] = bench;
                        aiTeam.playersListRoster[j] = starter;

                        //Debug.Log($"[AI Substitution] {starter.p_name} substituted by {bench.p_name} due to low stamina.");
                        break; // sai do loop interno (troca feita)
                    }
                }
            }
        }
    }
    //Cards
    public void CreateHand()
    {
        canUseCards = true;
        // no avaliable cards do draw
        if (cardsFolder.childCount < 3)
        {
            Debug.LogWarning("Não há cards suficientes no cardsFolder para criar a mão.");
            return;
        }

        // clear hand
        while (cards_handCards.childCount > 0)
        {
            Transform card = cards_handCards.GetChild(0);
            card.SetParent(cards_cardsUsedFolder);
            card.localScale = Vector3.one;
            card.localPosition = Vector3.zero;
            card.localRotation = Quaternion.identity;
        }

        // choose cards
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, cardsFolder.childCount);
            Transform chosenCard = cardsFolder.GetChild(randomIndex);

            chosenCard.SetParent(cards_handCards);
            chosenCard.localScale = Vector3.one;
            chosenCard.localPosition = Vector3.zero;
            chosenCard.localRotation = Quaternion.identity;
        }

        //Debug.Log("Nova mão criada com 3 cards!");
    }
    #region Offense and Otpions
    void ResetPostions()
    {
        for (int i = 0; i < HomeTeam.playersListRoster.Count; i++)
        {
            HomeTeam.playersListRoster[i].CurrentZone = 0;
        }
    }
    //Offensive Panel And Options
    bool MakePassToTeammate(int receiverIndex)
    {
        // Falha no passe?
        if (Random.Range(0f, 1f) > PassEquation())
        {
            uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.LosesPos());
            playerWithTheBall.HasTheBall = false;
            playerWithTheBall.CurrentZone = 0;
            return false;
        }

        // Verifica se o index é válido
        if (receiverIndex < 0 || receiverIndex >= teamWithball.playersListRoster.Count)
        {
            Debug.LogWarning("Receiver index out of range!");
            return false;
        }

        Player receiver = teamWithball.playersListRoster[receiverIndex];

        // Verifica se não está passando para ele mesmo ou para alguém que já está com a bola
        if (receiver == playerWithTheBall || receiver.HasTheBall)
        {
            Debug.LogWarning("Invalid receiver for the pass.");
            return false;
        }

        // Transfere a bola
        playerWithTheBall.CurrentZone = 0;
        receiver.CurrentZone = 0;
        playerWithTheBall.HasTheBall = false;
        receiver.HasTheBall = true;
        playerWithTheBall = receiver;
        ResetPostions();
        _matchUI.UpdatePlayerPlacements();
        _matchUI.PlayerWithBallButtonsOnOff();
        _matchUI.TurnOffPlayerButtons();
        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " receives the pass.");
        return true;
    }
    public void ChooseIndexToPass(int index)
    {
        passPlayerIndex = index;
    }
    bool TryBeatDefenderAdvanceZone(Player offense, Player defense, int zone, int bonus = 0)
    {
        // === 1. Cálculo de OVR na hora (média dos 12 attrs) ===
        int offenseOVR = Mathf.RoundToInt(
            (offense.Shooting + offense.Inside + offense.Mid + offense.Outside +
             offense.Awareness + offense.Defending + offense.Guarding + offense.Stealing +
             offense.Juking + offense.Consistency + offense.Control + offense.Positioning) / 12f);

        int defenseOVR = Mathf.RoundToInt(
            (defense.Shooting + defense.Inside + defense.Mid + defense.Outside +
             defense.Awareness + defense.Defending + defense.Guarding + defense.Stealing +
             defense.Juking + defense.Consistency + defense.Control + defense.Positioning) / 12f);

        // === 2. Offense score (attrs juke + zona + OVR bonus + random + bonus) ===
        float offenseAvg = (offense.Consistency + offense.Control + offense.Juking +
                            GetZoneValue(offense, zone) + offenseOVR / 5f) / 4f;

        float offenseScore = offenseAvg + UnityEngine.Random.Range(-10f, 20f) + bonus;

        // === 3. Defense score (attrs defensivos + média extra forte + BONUS AI) ===
        float defenseAvg = (defense.Consistency + defense.Guarding + defense.Stealing +
                            GetZoneValue(defense, zone) + defenseOVR / 5f) / 5f;

        float defenseMedianBonus = defenseAvg * 0.3f; // +30% média defensiva

        float defenseScore = defenseAvg + defenseMedianBonus;

        // AI defende melhor (bônus extra quando AI é o defensor)
        Team defendingTeam = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
        if (!defendingTeam.IsPlayerTeam) // AI defendendo
            defenseScore *= 1.25f; // +25% defesa AI

        // === 4. Chance base (offense vs defense) ===
        float rawChance = offenseScore / (offenseScore + defenseScore + 50f);

        // === 5. Bias principal: playerTeam mais difícil, AI mais fácil ===
        float finalChance = rawChance;

        if (teamWithball.IsPlayerTeam)
        {
            // PLAYER TEAM: mais dificuldade (precisa stats/buffs altos)
            finalChance *= 0.85f; // -15% base

            // Buff_Juke compensa (porcentagem)
            if (buff_Juke > 0)
                finalChance *= 1f + (buff_Juke / 100f);

            // ESPAÇO PRA MAIS BUFFS FUTUROS
            // ex: if (buff_Stamina > 0) finalChance *= 1f + (buff_Stamina / 100f);
        }
        else
        {
            // AI: mais fácil no juke
            finalChance *= 1.05f; // +5% base

            // Playoffs: AI ainda mais fácil
            if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
                finalChance *= 1.10f; // +10% extra AI playoffs
        }

        finalChance = Mathf.Clamp(finalChance, 0.15f, 0.95f);
        jukePercentage = finalChance;
        print(finalChance + " This is the juke percentage");
        // === 6. Decisão final ===
        bool success = UnityEngine.Random.value < finalChance;

        if (!success)
        {
            offense.CurrentZone = 0;
            return false;
        }
        /*
        int offenseRoll = UnityEngine.Random.Range(1, 101)
                         + offense.Consistency
                         + offense.Control
                         + offense.Juking
                         + GetZoneValue(offense, zone)
                         + UnityEngine.Random.Range(-5, 15)
                         + bonus;

        int defenseRoll = UnityEngine.Random.Range(1, 101)
                         + defense.Consistency
                         + defense.Guarding
                         + defense.Stealing
                         + GetZoneValue(defense, zone);

        int difference = offenseRoll - defenseRoll;

        bool success;

        if (difference > 20)
            success = true;
        else if (difference < -20)
            success = false;
        else
            success = UnityEngine.Random.value < 0.6f;

        if (!success)
        {
            //print("Failed juke");
            offense.CurrentZone = 0;
            return false;
        }
        */

        // ------------------------------------
        // AVANÇAR UMA ZONA
        // ------------------------------------
        if (zone < 2)
            zone++;     // avança 1 casa
        else
            zone = 2;   // mantém no limite máximo

        offense.CurrentZone = zone;
        _matchUI.UpdatePlayerPlacements();
        _matchUI.TurnOffPlayerButtons();
        //print(offense.playerLastName + " and the zone is " + offense.CurrentZone);
        return true;
    }
    public int GetJukePercentage(Player offense, Player defense, int zone)
    {
        // === CÁLCULO IDÊNTICO AO DA TryBeatDefenderAdvanceZone (sem o Random da decisão) ===
        // 1. OVR na hora
        int offenseOVR = Mathf.RoundToInt(
            (offense.Shooting + offense.Inside + offense.Mid + offense.Outside +
             offense.Awareness + offense.Defending + offense.Guarding + offense.Stealing +
             offense.Juking + offense.Consistency + offense.Control + offense.Positioning) / 12f);

        int defenseOVR = Mathf.RoundToInt(
            (defense.Shooting + defense.Inside + defense.Mid + defense.Outside +
             defense.Awareness + defense.Defending + defense.Guarding + defense.Stealing +
             defense.Juking + defense.Consistency + defense.Control + defense.Positioning) / 12f);

        // 2. Offense score
        float offenseAvg = (offense.Consistency + offense.Control + offense.Juking +
                            GetZoneValue(offense, zone) + offenseOVR / 5f) / 4f;

        float offenseScore = offenseAvg + UnityEngine.Random.Range(-10f, 20f); // mantém variabilidade leve pra UI variar um pouco

        // 3. Defense score
        float defenseAvg = (defense.Consistency + defense.Guarding + defense.Stealing +
                            GetZoneValue(defense, zone) + defenseOVR / 5f) / 5f;

        float defenseMedianBonus = defenseAvg * 0.3f;
        float defenseScore = defenseAvg + defenseMedianBonus;

        // AI defende melhor
        Team defendingTeam = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
        if (!defendingTeam.IsPlayerTeam)
            defenseScore *= 1.25f;

        // 4. Chance base
        float rawChance = offenseScore / (offenseScore + defenseScore + 50f);

        // 5. Bias
        float finalChance = rawChance;

        if (teamWithball.IsPlayerTeam)
        {
            finalChance *= 0.85f;

            if (buff_Juke > 0)
                finalChance *= 1f + (buff_Juke / 100f);
        }
        else
        {
            finalChance *= 1.05f;

            if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
                finalChance *= 1.10f;
        }

        finalChance = Mathf.Clamp(finalChance, 0.15f, 0.95f);

        // Retorna como % inteiro arredondado
        return Mathf.RoundToInt(finalChance * 100f);
    }
    public IEnumerator ToScore(Player playerWithTheBall, Player playerDefending, Team teamWithBall)
    {
        int zone = playerWithTheBall.CurrentZone;

        uiManager.PlaybyPlayText(
            playerWithTheBall.playerLastName + " takes a shot from zone " + zone
        );

        yield return new WaitForSeconds(_actionTimer);

        // Momentum apenas para o time da casa
        int teamMomentum = (teamWithBall == HomeTeam) ? momentum : 0;

        bool hasScored = Random.Range(0f, 1f) <
            ScoringEquation(playerWithTheBall, playerDefending, zone, teamMomentum);

        if (hasScored)
        {
            if (zone == 0)
            {
                playerWithTheBall.PointsMatch += 4;
                teamWithBall.Score += 4;
            }
            else if (zone == 1)
            {
                playerWithTheBall.PointsMatch += 5;
                teamWithBall.Score += 5;
            }
            else
            {
                playerWithTheBall.PointsMatch += 6;
                teamWithBall.Score += 6;
            }
            currentGamePossessons--;
            uiManager.PlaybyPlayText(
                playerWithTheBall.playerLastName + " scored! Total: " + playerWithTheBall.PointsMatch
            );
            _matchUI.ResultActionPanel("S",0);
        }
        else
        {
            uiManager.PlaybyPlayText(
                playerWithTheBall.playerLastName + " missed!"
            );
            _matchUI.ResultActionPanel("F",0);
        }

        yield return new WaitForSeconds(_actionTimer);

        // Se quiser resetar a zona após o chute, deixe isso ativado
        playerWithTheBall.CurrentZone = 0;
        _matchUI.TurnOffPlayerButtons();
        SwitchPossession();
        // NÃO troca posse — apenas termina
    }
    #endregion
    public void Matchpoint()
    {
        HomeTeam.Score += 50;
    }
}
