using DG.Tweening.Core.Easing;
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
    public enum AIAction
    {
        Pass,
        Juke,
        Shoot,
        Special // vazio por enquanto
    }
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
    [SerializeField] public Player playerDefending;
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
    public bool CanChooseShove = false;
    public bool CanChooseCharge = false;
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

    float jukePercentage;
    public int consecutiveSuccesses = 0;

    [SerializeField]public string currentFormation;

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
    bool canDraw = true;
    [SerializeField] GameObject cardsPanel;

    //Adranline
    int adrenaline_addUp;
    [Header("Hp-RogueliteElements")]
    [SerializeField] public int homeHP;
    [SerializeField] public int awayHP;
    //UI Elemens test
    [Header("Debugs")]
    [SerializeField] TextMeshProUGUI _debugTimeoutText;
    [SerializeField] string DefenderName;
    public bool isSimulation = false;

    //Skip/FastFoward
    public bool isFastforward = false;

    bool _isOnSetupStage;//
    private void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HomeTeam = manager.playerTeam;
        _sp_numberOfSPActions = manager.playerTeam.ArenaLvl;
        if (manager.playerTeam.ArenaLvl == 0) _sp_numberOfSPActions = 1;
        if (manager.playerTeam.ArenaLvl > 5) _sp_numberOfSPActions = 5;
        //_sp_numberOfSPActions = manager.playerTeam.FansSupportPoints / 20;
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
        homeHP = 100;
        awayHP = 100;
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
        AwayTeam.match_hp = AwayTeam.match_hpMax;
        HomeTeam.isOnDefenseBonus = false;
        AwayTeam.isOnDefenseBonus = false;
        teamWithball = HomeTeam;//Change Later
        HomeTeam.hasPossession = true;

        _canCallTimeout = true;
        HasActionOnTimeout = true;

        //AdrenalineBar
        HomeTeam.AdrenalineBar = 0;
        AwayTeam.AdrenalineBar = 0;


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
        //print(AwayTeam.playersListRoster.Count + "Number of player i the roster of the away team");
        StartCoroutine(RunMatchThenSimulate());

        
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
            currentFormation = teamWithball._teamStyle.ToString();
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
            if (currentGamePossessons>1) RestoreStaminaFromBench();
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
                //_matchUI.ActivateVictoryDefeat("Victory");
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
                if (leagueManager.isOnFinals)
                {
                    manager.playerTeam.isChampion = true;
                }
                //_matchUI.ActivateVictoryDefeat("Victory");
                _matchUI.StartResultPanel("Victory");
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
                //if (!isSimulation) _matchUI.ActivateVictoryDefeat("Defeat");
                if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
                {
                    leagueManager.isGameOver = true;
                    //leagueManager.isOnR8 = false;
                }
                if (!isSimulation) _matchUI.StartResultPanel("Defeat"); //_matchUI.ActivateVictoryDefeat("Defeat");
            }
            
        }
        else
        {
            HomeTeam.Moral -= 5;
            AwayTeam.Moral -= 5;
            HomeTeam.Draws++;
            AwayTeam.Draws++;
            if (HomeTeam.IsPlayerTeam) { _matchUI.StartResultPanel("Draw");/*_matchUI.ActivateVictoryDefeat("Draw"); */}
            
        }
        
        yield return new WaitForSeconds(2f);
        _matchUI.anim_victoryCircle.SetTrigger("Go");
        //stats increment
        if (HomeTeam.IsPlayerTeam)
        {
            // OFFICE - Front Office Points
            if (HomeTeam.OfficeLvl >= 0)
            {
                int bonus = GetFacilityBonus(HomeTeam.MarketingLvl);
                HomeTeam.FrontOfficePoints += bonus;
            }

            // FINANCES - Salary Cap
            if (HomeTeam.FinancesLvl >= 0)
            {
                int bonus = GetFacilityBonus(HomeTeam.MarketingLvl);
                //HomeTeam.SalaryCap += bonus;
            }

            // MARKETING - Fan Support
            if (HomeTeam.MarketingLvl >= 0)
            {
                int bonus = GetFacilityBonus(HomeTeam.MarketingLvl);
                HomeTeam.FansSupportPoints += bonus;
            }

            // ARENA - Morale
            if (HomeTeam.ArenaLvl >= 0)
            {
                int bonus = GetFacilityBonus(HomeTeam.MarketingLvl);
                HomeTeam.Moral += bonus;
                HomeTeam.Moral = Mathf.Clamp(HomeTeam.Moral, 0, 100);
            }

            // MEDICAL - Effort Points
            if (HomeTeam.MedicalLvl >= 0)
            {
                int bonus = GetFacilityBonus(HomeTeam.MarketingLvl);
                HomeTeam.EffortPoints += bonus;
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
        /*
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
        */
        int randomIndex = UnityEngine.Random.Range(0, 4);
        playerWithTheBall = teamWithball.playersListRoster[randomIndex];

        playerWithTheBall.HasTheBall = true;
        if (playerWithTheBall != null)
        {
            playerWithTheBall.CurrentZone = 0;
            playerWithTheBall.HasTheBall = true;
            //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " has the ball.");
            if(!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
        }
        SelectDefender();
        //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " has the ball.");
        if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
        
    }
    IEnumerator HandlePossession()
    {
        if (isSimulation) AISub();
        if( teamWithball.IsPlayerTeam == false)
        {
            adrenaline_addUp = 20;
            AwayTeam.AdrenalineBar = 50;
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
                /*
                if (currentGamePossessons <= 1)
                {
                    if(!isSimulation)uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " must shoot due to low possessions!");
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    yield return Scoring(playerWithTheBall,true);
                    currentGamePossessons--;
                    
                    yield break;
                }
                */
                else if (_canCallTimeout == false)
                {
                    yield return StartCoroutine(WaitForTimeOut());
                    //if(!isSimulation)CheckAndSwapLowStaminaPlayers(AwayTeam);
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
                
                AIAction chosenAction = AI_Tendency(); //  NOVA DECISÃO

                switch (chosenAction)
                {
                    case AIAction.Pass:
                        if (TryPassBall())
                        {
                            if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                            if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                            SelectDefender();
                            if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                            if (teamWithball.AdrenalineBar < teamWithball.AdrenalineBarFull) teamWithball.AdrenalineBar += adrenaline_addUp;
                            ResetDefensiveOptions();
                            continue; // continua loop pra próxima ação
                        }
                        else
                        {
                            // pass fail  steal + switch
                            if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                            playerWithTheBall.HasTheBall = false;
                            if (!isSimulation) _matchUI.ResultActionPanel("S",3);
                            SwitchPossession();
                            if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                            if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                            ResetDefensiveOptions();
                            yield break; 
                        }
                    // break; removido (não precisa, já saiu com continue ou yield break)

                    case AIAction.Juke:
                        if (TryBeatDefenderAdvanceZone(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone))
                        {
                            // juke success  continua posse (loop)
                            if (teamWithball.AdrenalineBar < teamWithball.AdrenalineBarFull) teamWithball.AdrenalineBar += adrenaline_addUp;
                            continue;
                        }
                        else
                        {
                            // juke fail  steal  switch
                            playerDefending.StealsMatch++;
                            if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.LosesPos() + " Loses the ball to " + playerDefending.playerLastName);
                            playerWithTheBall.HasTheBall = false;
                            if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                            ResetDefensiveOptions();
                            if (!isSimulation) _matchUI.ResultActionPanel("S", 3);
                            SwitchPossession();
                            yield break; 
                        }
                    // break; removido

                    case AIAction.Shoot:
                        if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ShootingText());
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);

                        yield return Scoring(playerWithTheBall, true);
                        ResetDefensiveOptions();
                        yield break; 

                    case AIAction.Special:
                        // Vazio por enquanto  fallback pra shoot
                        if (!isSimulation) uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ShootingText());
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);

                        yield return Scoring(playerWithTheBall, true);
                        ResetDefensiveOptions();
                        yield break; 
                }
            }
        }
        else if (teamWithball.IsPlayerTeam)
        {
            //_matchUI.OffesnivePanelOnOff(true);
            CanChooseDefenseAction = false;

           
            if (!isSimulation)
            {
               adrenaline_addUp = GetAdrenalineAddUp(HomeTeam);
            }
            HomeTeam.AdrenalineBar += adrenaline_addUp;
            ResetPostions();
            _matchUI.PlayerWithBallButtonsOnOff();
            ResetStreak();

            if (currentGamePossessons > 1)
            {
                ResetChoices();
                if(canDraw == true)
                {
                    if (cardsFolder.childCount > 3)
                    {
                        CreateHand();
                        if (!isSimulation) _matchUI.UpdateCardsHand();
                        canDraw = false;
                    }
                }
                
            }   
            _matchUI.text_remainingCards.text = cardsFolder.childCount.ToString();
            while (true)
            {
                if (!isSimulation) _matchUI.percentagePanel.SetActive(true);
                if (!isSimulation) _matchUI.PlayerWithTheBallOff();
                if (!isSimulation) _matchUI.PlayerWithBallButtonsOnOff();
                if (!isSimulation) _matchUI.UpdatePlayerPlacements();
                if (!isSimulation) _matchUI.UpdateStreakValue(consecutiveSuccesses);
                //if(!isSimulation) _matchUI.ActivateAnimatorOffensivePanel();
                //MatchEvents();
                //CanChooseAction = true;
                if ((leagueManager.isOnR8 == true || leagueManager.isOnR4 == true || leagueManager.isOnFinals == true) && currentGamePossessons <= 1)
                {
                    if (HomeTeam.Score == AwayTeam.Score)
                    {
                        currentGamePossessons++;
                    }
                }
                
                _matchUI.OffesnivePanelOnOff(true);
                _matchUI.SetScoringPercentage(ScoringEquation(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone,0));
                _matchUI.SetPassPercentage(PercentageMakePassToTeammate(Random.Range(0,4)));
                _matchUI.SetJukePercentage(/*TryBeatDefenderAdvanceZone(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone, 0)*/GetJukePercentage(playerWithTheBall,playerDefending,playerWithTheBall.CurrentZone));
                _matchUI.SetSpPercentage(ActivateSpecialAttk(true));
                _matchUI.text_midChance.text = "Mid: " + Mathf.RoundToInt(ScoringEquation(playerWithTheBall, playerDefending, 1, 0) *100).ToString() + "%";
                _matchUI.text_insChance.text = "Inside: " + Mathf.RoundToInt(ScoringEquation(playerWithTheBall, playerDefending, 2, 0) * 100).ToString() + "%";
                //_matchUI.SetJukePercentage();
                _matchUI.SetDMGText(GetDamageValue(playerWithTheBall.CurrentZone));
                uiManager.PlaybyPlayText("Wait for Player Action");

                _matchUI.TeamStyleUpdate(currentFormation);//update team style
                _matchUI.OffensivePanelAwayTeamUpdate(AwayTeam);//update away team player stamna and name
                // Wait until player makes a choice
                yield return new WaitUntil(() => _ChoosePass || _ChooseScoring || _ChooseToSpecialAtt || _ChooseBeatDefender || CanChooseCharge || CanChooseShove||_canCallTimeout == false);

                if (_ChooseScoring)
                {
                    //_matchUI.UsedPlayerBtns();
                    _ChooseScoring = false;
                    _matchUI.ActionPanelAnim(5, "Shoot");
                    //Lose Stamina
                    StaminaLossByAction(playerWithTheBall);
                    yield return new WaitForSeconds(1f);
                    playerWithTheBall.HasTheBall = false;
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
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
                    //_matchUI.UsedPlayerBtns();
                    _ChoosePass = false;
                    _matchUI.ActionPanelAnim(0, "Passing");
                    //Lose Stamina
                    StaminaLossByAction(playerWithTheBall);
                    yield return new WaitForSeconds(1f);
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    if (MakePassToTeammate(passPlayerIndex))
                    {
                        RegisterSuccess();//register +streak
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        //uiManager.PlaybyPlayText(playerWithTheBall.playerFirstName + " prepares for next action.");
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        SelectDefender();
                        if (!isSimulation) _matchUI.ResultActionPanel("S",1);
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        if(HomeTeam.AdrenalineBar<HomeTeam.AdrenalineBarFull)HomeTeam.AdrenalineBar += adrenaline_addUp;
                        //staminaLoss

                        ResetChoices();
                        continue;
                    }
                    else
                    {
                        ResetStreak();
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
                    //_matchUI.UsedPlayerBtns();
                    _ChooseToSpecialAtt = false;
                    _sp_numberOfSPActions--;
                    _matchUI.SetSkillPints();
                    _matchUI.ActionPanelAnim(1, "Special");
                    //Lose Stamina
                    StaminaLossByAction(playerWithTheBall);
                    yield return new WaitForSeconds(1f);
                    playerWithTheBall.HasTheBall = false;
                    if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                    if (Random.Range(0f, 1f) < ActivateSpecialAttk(false))
                    {
                        int spPoints = 8;
                        //Add later the list of string for a success use o team abillity
                        uiManager.PlaybyPlayText("The team will use their special abillity");
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);

                        teamWithball.Score += spPoints;

                        if (!isSimulation && teamWithball.IsPlayerTeam)
                        {
                            Team defendingTeam = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
                            CalculateDamageAndReduceHP(defendingTeam, playerWithTheBall.CurrentZone);
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
                    //_matchUI.UsedPlayerBtns();
                    _matchUI.ActionPanelAnim(7, "Juke");
                    //Lose Stamina
                    StaminaLossByAction(playerWithTheBall);
                    yield return new WaitForSeconds(1f);
                    if (TryBeatDefenderAdvanceZone(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone))
                    {
                        RegisterSuccess();//register +streak
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + "Pass the defender");
                        SelectDefender();
                        _matchUI.ResultActionPanel("S",2);
                        if (HomeTeam.AdrenalineBar < HomeTeam.AdrenalineBarFull) HomeTeam.AdrenalineBar += adrenaline_addUp;
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        ResetChoices();
                        continue;
                    }
                    else
                    {
                        ResetStreak();
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
                else if (CanChooseShove)
                {
                    //_matchUI.UsedPlayerBtns();
                    _matchUI.ActionPanelAnim(9, "Shove");
                    //action panel
                    //Lose Stamina
                    StaminaLossByAction(playerWithTheBall);
                    yield return new WaitForSeconds(_actionTimer);
                    if (TryToShoveDefender(playerWithTheBall,playerDefending))
                    {
                        RegisterSuccess();
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + "Shoved " + playerDefending.playerLastName);
                        _matchUI.ResultActionPanel("S", 2);
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        ResetChoices();
                        continue;

                    }
                    else
                    {

                        ResetStreak();
                        uiManager.PlaybyPlayText(_matchUI.LosesPos());
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        playerWithTheBall.HasTheBall = false;
                        _matchUI.ResultActionPanel("F", 2);
                        SwitchPossession();
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        yield break;

                    }
                    

                }
                else if (CanChooseCharge)
                {
                    _matchUI.ActionPanelAnim(8, "Charge");
                    yield return new WaitForSeconds(_actionTimer);
                    //_matchUI.UsedPlayerBtns();
                    uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " is Charging");
                    if (TryToChargeAdrenaline(playerWithTheBall, HomeTeam))
                    {
                        ResetChoices();
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " Charged the field!");
                        _matchUI.ResultActionPanel("S", 2);//mudar
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        ResetChoices();
                        continue;
                    }
                    else
                    {
                        ResetStreak();
                        uiManager.PlaybyPlayText(_matchUI.LosesPos());//fazer uma lista propria disso
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        playerWithTheBall.HasTheBall = false;
                        _matchUI.ResultActionPanel("F", 2);
                        SwitchPossession();
                        uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.ReceiveBallText());
                        if (!isSimulation) yield return new WaitForSeconds(_actionTimer);
                        yield break;
                    }
                    

                }
                else if(_canCallTimeout == false)
                {
                    if (!isSimulation) yield return StartCoroutine(WaitForTimeOut());
                    //CheckAndSwapLowStaminaPlayers(AwayTeam);
                    consecutiveSuccesses = 0;
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
        currentGamePossessons--;
    }
    void ResetChoices()
    {
        _ChoosePass = false;
        _ChooseScoring = false;
        _ChooseToSpecialAtt = false;
        _ChooseBeatDefender = false;
        CanChooseShove = false;
        CanChooseCharge = false;
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
    public void ChooseShove()
    {
        CanChooseShove = true;
        CanChooseAction = false;
    }
    public void ChooseCharge()
    {
        print("I CHOOSE CHARGE!!!!");
        CanChooseCharge = true;
        CanChooseAction = false;
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

        if (Random.Range(0f, 1f) > PassEquation())
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
            if (!isSimulation)
            {
                if (teamWithball!= manager.playerTeam) _matchUI.ResultActionPanel("F", 3);
            }
            if (teamWithball.AdrenalineBar < teamWithball.AdrenalineBarFull) teamWithball.AdrenalineBar += adrenaline_addUp;
            //Damage Deal
            if (!isSimulation && teamWithball.IsPlayerTeam)
            {
                Team defendingTeam = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
                CalculateDamageAndReduceHP(defendingTeam, player.CurrentZone);
            }


        }
        else
        {
            if (!isSimulation) uiManager.PlaybyPlayText(player.playerLastName + " Missed");
            if (!isSimulation)
            {
                if (teamWithball != manager.playerTeam) _matchUI.ResultActionPanel("S", 3);
            }
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
    void SwitchPossession()
    {
        if (!isSimulation) _matchUI.OffesnivePanelOnOff(false);
        ControlStamina(HomeTeam);
        ControlStamina(AwayTeam);
        playerWithTheBall = null;
        canDraw = true;//possibilida ao inicio de turno scar cartas 
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
        currentFormation = teamWithball._teamStyle.ToString();
        playerWithTheBall = null;
        uiManager.PlaybyPlayText("Possession switches to " + teamWithball.TeamName);
        ChoosePlayerToCarryBall();
        SelectDefender();
        if (!isSimulation) if (HomeTeam.IsPlayerTeam)_matchUI.ChangePos(HomeTeam);
        currentGamePossessons--;
        
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
        int randomIndex = Random.Range(0, 4); // avoid 0
        Player newDefender = DefendingTeam.playersListRoster[randomIndex];
        //newDefender.CurrentZone = 0;
        DefenderName = newDefender.playerFirstName + " " + DefendingTeam.TeamName;
        playerDefending = newDefender;
        if(HomeTeam.IsPlayerTeam && HomeTeam.hasPossession)
        {
            _matchUI.UpdateDefenderInfo(playerDefending);
        }
        //Debug.Log(newDefender.playerFirstName + " is now defending.");
    }
    //Beat the defender and change zones
    bool TryBeatDefender(Player offense, Player defense, int zone, int bonus = 0)
    {
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

            //Debug.Log($"[RESULT] {teamA.TeamName} {teamA.Score} - {teamB.Score} {teamB.TeamName}");
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
            //print("Quarters!!!!!!!!!!!!!!!!!");
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
            //yield return StartCoroutine(GameFlow());
            SimulateMatchInstant(teamA,teamB);

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
            p.CurrentStamina = 100;
            p.HasTheBall = false;
        }
        foreach (Player p in teamB.playersListRoster)
        {
            p.PointsMatch = 0;
            p.StealsMatch = 0;
            p.CurrentStamina = 100;
            p.HasTheBall = false;
        }

        // Loop principal de posses
        while (currentGamePossessons > 0)
        {
            ChoosePlayerToCarryBall();
            SelectDefender();

            // Simulação mais agressiva para ter mais pontuação
            int actionsThisPossession = Random.Range(1, 4); // 1 a 3 ações por posse

            bool possessionEnded = false;

            for (int i = 0; i < actionsThisPossession && !possessionEnded; i++)
            {
                float rand = Random.value;

                if (rand < 0.35f) // 35% chance de Pass
                {
                    if (!SimulatePass())
                        possessionEnded = true;
                }
                else if (rand < 0.65f) // 30% chance de Juke
                {
                    if (!SimulateJuke())
                        possessionEnded = true;
                }
                else if (rand < 0.90f) // 25% chance de Shoot
                {
                    SimulateShoot();
                    possessionEnded = true;
                }
                else // 10% chance de Special
                {
                    SimulateSpecial();
                    possessionEnded = true;
                }
            }

            // Proteção contra loop infinito em playoffs
            currentGamePossessons--;

            if ((leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals) && currentGamePossessons <= 1)
            {
                if (teamA.Score == teamB.Score)
                {
                    currentGamePossessons = Mathf.Min(currentGamePossessons + 1, GamePossesions + 2); // nunca passa de +2 posses
                }
                else
                {
                    currentGamePossessons = 0; // força fim se já tiver vencedor
                }
            }

            RestoreStaminaFromBench();
        }
        if ((leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals) && teamA.Score == teamB.Score)
        {
            // Se ainda estiver empatado, força um vencedor dando 2 pontos aleatórios
            if (Random.value < 0.5f)
                teamA.Score += 2;
            else
                teamB.Score += 2;

            //Debug.Log("[SimulateMatchInstant] Empate resolvido em playoffs com +2 pontos aleatórios");
        }
        // Fim do jogo - mantido exatamente como estava
        if (teamA.Score > teamB.Score)
        {
            teamB.Moral -= 15;
            teamA.Moral += 15;
            teamA.Wins++;
            teamB.Loses++;
        }
        else if (teamA.Score < teamB.Score)
        {
            teamA.Moral -= 15;
            teamB.Moral += 15;
            teamA.Loses++;
            teamB.Wins++;
        }
        else
        {
            teamA.Moral -= 5;
            teamB.Moral -= 5;
            teamA.Draws++;
            teamB.Draws++;
        }

        // Career stats + contract
        foreach (Player p in teamA.playersListRoster)
        {
            p.CareerPoints += p.PointsMatch;
            p.CareerSteals += p.StealsMatch;
            p.CareerGamesPlayed++;
            p.buff = 0;
            if (teamA.IsPlayerTeam) p.ContractYears--;
        }
        foreach (Player p in teamB.playersListRoster)
        {
            p.CareerPoints += p.PointsMatch;
            p.CareerSteals += p.StealsMatch;
            p.CareerGamesPlayed++;
        }

        teamA.HasPlayed = true;
        teamB.HasPlayed = true;
    }
    
    //Equations
    float PassEquation()
    {

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
            rawChance *= 0.90f; // -20%

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
            finalChance *= 1.25f; // +5% base

            // Playoffs: AI ainda mais fácil
            if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
                finalChance *= 1.30f; // +10% extra AI playoffs
                                      // === NOVO: BOOST AI SE OVR BAIXO ===
            int playerOVR = Mathf.RoundToInt(
                (playerWithTheBall.Shooting + playerWithTheBall.Inside + playerWithTheBall.Mid + playerWithTheBall.Outside +
                 playerWithTheBall.Awareness + playerWithTheBall.Defending + playerWithTheBall.Guarding + playerWithTheBall.Stealing +
                 playerWithTheBall.Juking + playerWithTheBall.Consistency + playerWithTheBall.Control + playerWithTheBall.Positioning) / 12f);

            if (playerOVR <= 70)
                finalChance *= 1.30f; // maior boost
            else if (playerOVR <= 85)
                finalChance *= 1.15f; // médio boost
        }
        //if (consecutiveSuccesses >= 2) finalChance *= 1.15f;
        float streakMultiplier = 1f;

        if (teamWithball.IsPlayerTeam)
        {
            // Streak real do Player Team (0 a 10)
            int streak = Mathf.Clamp(consecutiveSuccesses, 0, 10);
            streakMultiplier = 1f + (streak * 0.04f);   // Máximo 25% de bônus no streak 10
        }
        else
        {
            // AI sempre tem streak fixo em 5
            streakMultiplier = 1f + (5 * 0.025f);        // 12.5% fixo para AI
        }

        finalChance *= streakMultiplier;
        // === 7. Clamp final (tensão) ===
        return Mathf.Clamp(finalChance, 0.15f, 0.95f);
    }
    float ScoringEquation(Player offense, Player defense, int zone, int momentumModifier)
    {
        
        int ovr = offense.SetOVR();

        // X = Offense
        float X = 100f
                  + (offense.Consistency * 0.1f)   // 10 a 99
                  + (zone * 10f);                  // Zone bonus

        // Y = Defense
        float Y = (defense.Consistency * 0.1f)
                  + (defense.Positioning * 0.1f)
                  + (zone * 10f);

        float Z = X - Y;   // Positional Bonus Result

        // Offense final
        float offenseShot = (offense.Consistency * 0.1f)
                            + (offense.Control * 0.1f)
                            + (offense.Shooting * 0.1f)
                            + (zone * 10f)
                            + Z;   // + Positional Bonus Result

        // Defense final
        float defenseShot = (defense.Consistency * 0.1f)
                            + (defense.Guarding * 0.1f)
                            + (defense.Awareness * 0.1f)   // ou Blocking se tiver
                            + (zone * 10f);

        float shotResult = offenseShot - defenseShot;


        float defenderQualityMultiplier = 1f;

        int defenseOVR = defense.SetOVR();

        if (defenseOVR < 60)        // Bad Defender
        {
            defenderQualityMultiplier = zone switch
            {
                0 => 0.85f,   // Outside
                1 => 0.75f,   // Mid
                2 => 0.80f,   // Inside
                _ => 0.80f
            };
        }
        else if (defenseOVR <= 75)  // Average Defender
        {
            defenderQualityMultiplier = zone switch
            {
                0 => 0.70f,
                1 => 0.65f,
                2 => 0.75f,
                _ => 0.70f
            };
        }
        else                        // Good / Great / Star Defender (75+)
        {
            defenderQualityMultiplier = zone switch
            {
                0 => 0.55f,
                1 => 0.50f,
                2 => 0.60f,
                _ => 0.55f
            };
        }

        float baseAccuracy = shotResult * defenderQualityMultiplier * 0.0028f;

        //float baseAccuracy = 0.68f; // valor padrão

        if (shotResult >= 200f) baseAccuracy = 0.93f;
        else if (shotResult >= 150f) baseAccuracy = 0.88f;
        else if (shotResult >= 100f) baseAccuracy = 0.82f;
        else if (shotResult >= 50f) baseAccuracy = 0.76f;
        else if (shotResult >= 0f) baseAccuracy = 0.71f;
        else if (shotResult >= -50f) baseAccuracy = 0.65f;
        else if (shotResult >= -100f) baseAccuracy = 0.58f;
        else baseAccuracy = 0.50f; // valores negativos fortes

        float adrenaline = teamWithball.IsPlayerTeam ? teamWithball.AdrenalineBar : 75f;
        float adrenalineFactor = adrenaline / 100f;

        float difficulty = teamWithball.IsPlayerTeam
            ? 1f - (adrenalineFactor * 0.32f)
            : 1f - (adrenalineFactor * 0.14f);

        float rawChance = baseAccuracy * difficulty;

        float shootingBuffMultiplier = 1f + (buff_Atk / 100f);

        float finalChance = rawChance * shootingBuffMultiplier;

        float staminaFactor = GetStaminaMultiplier(offense.CurrentStamina);
        finalChance *= staminaFactor * 0.70f;//coeficente 

        float streakMultiplier = 1f;
        if (teamWithball.IsPlayerTeam)
        {
            int streak = Mathf.Clamp(consecutiveSuccesses, 0, 10);
            streakMultiplier = 1f + (streak * 0.04f);
        }
        else
        {
            if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
            {
                finalChance *= 1.35f;
            }
            else
            {
                finalChance *= 1.20f;
            }
            streakMultiplier = 1f + (5 * 0.065f);
        }
        finalChance *= streakMultiplier;

        //Bonus de formação - ataque
        string bonusType = GetFormationBonus(teamWithball, offense);

        // Aplica bônus se a formação favorece Shooting ou Juke
        if (bonusType.Contains("Shooting") || bonusType.Contains("Shooting & Juke"))
        {
            finalChance *= 1.18f;   // +18% de eficiência
        }

        return Mathf.Clamp(finalChance, 0.20f, 0.93f);
    }
    float ActivateSpecialAttk(bool isPercentage)
    {
        float fillPercent = (teamWithball.AdrenalineBar / teamWithball.AdrenalineBarFull) * 100f;

        float specialAttkSuccess = 0f;

        // === REGRA ESPECIAL: 100% de chance quando barra cheia ===
        if (fillPercent >= 100f)
        {
            specialAttkSuccess = 1f;           // Garantido
            if (!isPercentage)
            {
                teamWithball.AdrenalineBar = 0; // Zera a barra ao usar
                //currentGamePossessons--;
            }
            return 1f;
        }

        // === LÓGICA NORMAL (abaixo de 100%) ===
        if (fillPercent < 50f)
            return 0f; // Não pode usar abaixo de 50%

        // Cálculo base (mantido da sua versão anterior)
        int offenseOVR = Mathf.RoundToInt(
            (playerWithTheBall.Shooting + playerWithTheBall.Inside + playerWithTheBall.Mid + playerWithTheBall.Outside +
             playerWithTheBall.Awareness + playerWithTheBall.Defending + playerWithTheBall.Guarding + playerWithTheBall.Stealing +
             playerWithTheBall.Juking + playerWithTheBall.Consistency + playerWithTheBall.Control + playerWithTheBall.Positioning) / 12f);

        int defenseOVR = Mathf.RoundToInt(
            (playerDefending.Shooting + playerDefending.Inside + playerDefending.Mid + playerDefending.Outside +
             playerDefending.Awareness + playerDefending.Defending + playerDefending.Guarding + playerDefending.Stealing +
             playerDefending.Juking + playerDefending.Consistency + playerDefending.Control + playerDefending.Positioning) / 12f);

        float offenseAvg = (playerWithTheBall.Shooting + playerWithTheBall.Juking + playerWithTheBall.Control +
                            offenseOVR / 5f) / 4f;
        float offenseScore = offenseAvg + UnityEngine.Random.Range(-10f, 20f);

        float defenseAvg = (playerDefending.Defending + playerDefending.Guarding + playerDefending.Stealing +
                            defenseOVR / 5f) / 4f;
        float defenseMedianBonus = defenseAvg * 0.3f;
        float defenseScore = defenseAvg + defenseMedianBonus;

        Team defendingTeam = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
        if (!defendingTeam.IsPlayerTeam)
            defenseScore *= 1.25f;

        float rawChance = offenseScore / (offenseScore + defenseScore + 50f);

        // Chance baseada na barra (50% ~ 100%)
        if (fillPercent >= 75f)
            specialAttkSuccess = 0.70f + (fillPercent - 75f) / 25f * 0.30f;   // 70% a 100%
        else
            specialAttkSuccess = 0.50f + (fillPercent - 50f) / 25f * 0.20f;   // 50% a 70%

        float finalChance = specialAttkSuccess * rawChance;

        if (teamWithball.IsPlayerTeam)
        {
            finalChance *= 0.85f;                     // Player sofre mais dificuldade
            if (buff_SP > 0)
                finalChance *= 1f + (buff_SP / 100f); // Buff_SP ajuda
        }
        else
        {
            finalChance *= 1.05f;
            if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
                finalChance *= 1.10f;
        }

        finalChance = Mathf.Clamp(finalChance, 0f, 1f);

        // Se não for apenas para mostrar porcentagem, zera a barra ao usar
        if (!isPercentage)
        {
            teamWithball.AdrenalineBar = 0;
            //currentGamePossessons--;
        }
        teamWithball.AdrenalineBar = 0; // Zera a barra ao usar
        currentGamePossessons--;
        return finalChance;
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

        float medicalReduction = 0f;

        if (team.IsPlayerTeam && manager != null && manager.playerTeam != null)
        {
            // Level 0 = 0%, Level 1 = 3%, ... Level 7 = 15%
            medicalReduction = Mathf.Clamp(team.MedicalLvl * 0.0214f, 0f, 0.15f);
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

            if (team.IsPlayerTeam && medicalReduction > 0f)
            {
                team.playersListRoster[i].CurrentStamina += Mathf.RoundToInt((staminaLoss + 5) * medicalReduction);
            }

            if (team.playersListRoster[i].CurrentStamina < 10)
                team.playersListRoster[i].CurrentStamina = 10;

        }
        //_matchUI.UpdatePlayersActive();
    }
    void StaminaLossByAction(Player player)
    {
        int loss;
        if (player.Age <= 23)
            loss = 10; // baixa
        else if (player.Age <= 27)
            loss = 15; // média baixa
        else if (player.Age <= 31)
            loss = 30; // média alta
        else
            loss = 35; // alta

        //check medcare lvl bonus
        float medicalReduction = 0f;
        if (manager != null && manager.playerTeam != null &&
            player != null && player.transform.parent != null)
        {
            if (player.transform.parent.GetComponent<Team>() == manager.playerTeam ||
                manager.playerTeam.playersListRoster.Contains(player))
            {
                // Novo scaling: 0% no lvl 0 - 24% no lvl 4 - 38% no lvl 7
                medicalReduction = Mathf.Clamp(manager.playerTeam.MedicalLvl * 0.055f, 0f, 0.38f);
            }
        }

        player.CurrentStamina -= loss;

        //apply bonus
        if (medicalReduction > 0f)
        {
            player.CurrentStamina += Mathf.RoundToInt(loss * medicalReduction);
        }

        player.CurrentStamina = Mathf.Max(player.CurrentStamina, 10); // clamp min

       

        
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
    IEnumerator waitSecondsForAction()
    {
        yield return new WaitForSeconds(3f);
    }
    public void AISub()
    {
        foreach (Team team in new[] { HomeTeam, AwayTeam })
        {
            if (!team.IsPlayerTeam)
            {
                for (int i = 0; i < 4; i++) // starters (0-3)
                {
                    Player starter = team.playersListRoster[i];
                    if (starter.CurrentStamina < 75)
                    {
                        for (int j = 4; j < team.playersListRoster.Count; j++) // subs (4+)
                        {
                            Player sub = team.playersListRoster[j];
                            if (sub.CurrentStamina > starter.CurrentStamina)
                            {
                                // Troca
                                team.playersListRoster[i] = sub;
                                team.playersListRoster[j] = starter;
                                break;
                            }
                        }
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
            //Debug.LogWarning("Não há cards suficientes no cardsFolder para criar a mão.");
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
        
        if (receiverIndex < 0 || receiverIndex >= teamWithball.playersListRoster.Count)
            return false;

        Player receiver = teamWithball.playersListRoster[receiverIndex];
        if (receiver == playerWithTheBall || receiver.HasTheBall)
            return false;

        // Adrenalina (IA sempre usa 75)
        float adrenaline = teamWithball.IsPlayerTeam ? teamWithball.AdrenalineBar : 75f;
        float adrenalineFactor = adrenaline / 100f;

        float baseChance = 0.80f;                                   // Base boa
        float difficulty = teamWithball.IsPlayerTeam
            ? 1f - (adrenalineFactor * 0.32f)                      // Penalidade adrenalina
            : 1f - (adrenalineFactor * 0.12f);

        // BUFF DO PASSE (facilitador forte)
        float passBuffMultiplier = 1f + (buff_Pass / 100f);        // ex: buff_Pass = 20  +20%

        float finalChance = baseChance * difficulty * passBuffMultiplier;

        float staminaFactor = GetStaminaMultiplier(playerWithTheBall.CurrentStamina);
        finalChance *= staminaFactor;
        finalChance = Mathf.Clamp(finalChance, 0.38f, 0.94f);

        if (Random.value > finalChance)
        {
            uiManager.PlaybyPlayText(playerWithTheBall.playerLastName + " " + _matchUI.LosesPos());
            playerWithTheBall.HasTheBall = false;
            playerWithTheBall.CurrentZone = 0;
            return false;
        }

        // Sucesso
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
        
        // ====================== JUKE CHECK (da imagem) ======================
        // Offense
        float X = 100f
                  + (offense.Consistency * 0.1f)
                  + (offense.Control * 0.1f)
                  + (offense.Juking * 0.1f)
                  + (zone * 10f)
                  + 0f; // Positional Bonus Result (pode ser integrado depois se quiser)

        // Defense (sem zone, conforme você pediu)
        float Y = (defense.Consistency * 0.1f)
                  + (defense.Guarding * 0.1f)
                  + (defense.Stealing * 0.1f);

        float jukeResult = X - Y;

        // ====================== CONVERSÃO DO RESULTADO EM CHANCE ======================
        float baseJukeChance;

        if (jukeResult >= 100f)
            baseJukeChance = 0.85f;     // Juke bem sucedido
        else if (jukeResult >= -99f)
            baseJukeChance = 0.50f;     // Meio a meio (removido o "Nothing")
        else
            baseJukeChance = 0.25f;     // Steal (defesa vence)

        // ====================== FATORES EXISTENTES (mantidos) ======================
        float adrenaline = teamWithball.IsPlayerTeam ? teamWithball.AdrenalineBar : 75f;
        float adrenalineFactor = adrenaline / 100f;

        float difficulty = teamWithball.IsPlayerTeam
            ? 1f - (adrenalineFactor * 0.38f)
            : 1f - (adrenalineFactor * 0.18f);

        float rawChance = baseJukeChance * difficulty;

        float jukeBuffMultiplier = 1f + (buff_Juke / 100f);

        float finalChance = rawChance * jukeBuffMultiplier * (1f + bonus / 100f);

        float staminaFactor = GetStaminaMultiplier(offense.CurrentStamina);
        finalChance *= staminaFactor;

        float streakMultiplier = 1f;
        if (teamWithball.IsPlayerTeam)
        {
            int streak = Mathf.Clamp(consecutiveSuccesses, 0, 10);
            streakMultiplier = 1f + (streak * 0.055f);
        }
        else
        {
            streakMultiplier = 1f + (5 * 0.025f);
        }
        finalChance *= streakMultiplier;

        //Bonus de formação - ataque
        string bonusType = GetFormationBonus(teamWithball, offense);

        // Aplica bônus de Juke
        if (bonusType.Contains("Juke"))
        {
            finalChance *= 1.18f;   // +18% de eficiência
        }

        finalChance = Mathf.Clamp(finalChance, 0.25f, 0.90f);

        jukePercentage = finalChance;

        bool success = Random.value < finalChance;

        if (!success)
        {
            offense.CurrentZone = 0;
            return false;   // Steal
        }

        offense.CurrentZone = Mathf.Min(zone + 1, 2);
        _matchUI.UpdatePlayerPlacements();
        _matchUI.TurnOffPlayerButtons();
        return true;   // Juke bem sucedido
    }
    bool TryToShoveDefender(Player attacker, Player defender)
    {
        if (attacker.CurrentStamina < 10)
        {
            return false; // Não tem stamina suficiente para tentar
        }

        attacker.CurrentStamina -= 10; // Gasta 10 sempre para tentar

        // Calcula score do attacker: média de Consistency, Control, Shooting + bônus por stamina atual (escala de 0-10)
        float attackerScore = (attacker.Consistency + attacker.Control + attacker.Shooting) / 3f;
        attackerScore += attacker.CurrentStamina / 10f; // Mais stamina ajuda (ex: 100 stamina = +10)

        // Calcula score do defender: média de Defending, Awareness, Positioning
        float defenderScore = (defender.Defending + defender.Awareness + defender.Positioning) / 3f;

        // Adiciona elemento aleatório para chance (ex: -10 a +10)
        float randomFactor = Random.Range(-10f, 10f);

        // Calcula valor final: positivo = sucesso, negativo = falha
        float shoveValue = attackerScore - defenderScore + randomFactor;

        if (shoveValue > 0)
        {
            defender.CurrentStamina -= 20; // Sucesso: defender perde 20
            defender.CurrentStamina = Mathf.Max(0, defender.CurrentStamina); // Evita negativo
            return true;
        }
        else
        {
            // Falha: torne o valor negativo entendido - talvez attacker perca extra baseado no |shoveValue|
            int extraLoss = Mathf.RoundToInt(Mathf.Abs(shoveValue)); // Ex: quanto pior, mais perde
            attacker.CurrentStamina -= extraLoss;
            attacker.CurrentStamina = Mathf.Max(0, attacker.CurrentStamina); // Evita negativo
            return false;
        }
    }
    bool TryToChargeAdrenaline(Player playerWithBall, Team team)
    {
        if (playerWithBall.CurrentStamina <= 25)
        {
            return false; // Não tem stamina suficiente, falha
        }

        playerWithBall.CurrentStamina -= 25; // Reduz 25 de stamina
        playerWithBall.CurrentStamina = Mathf.Max(0, playerWithBall.CurrentStamina); // Clamp para evitar negativo (embora >25 garanta)

        int increaseValue = Random.Range(25, 35);

        team.AdrenalineBar += increaseValue; // Aumenta a barra em 25
                                  // Assuma que adrenalinebar tem um max, se necessário: team.adrenalinebar = Mathf.Min(team.adrenalinebar, maxAdrenaline);

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
        ResetStreak();
        int zone = playerWithTheBall.CurrentZone;

        if(zone == 0)
        uiManager.PlaybyPlayText(
            playerWithTheBall.playerLastName + " takes a shot from Outside "
        );
        if(zone == 1) uiManager.PlaybyPlayText(
            playerWithTheBall.playerLastName + " takes a shot from Mid range "
        );
        if(zone == 2) uiManager.PlaybyPlayText(
            playerWithTheBall.playerLastName + " takes a shot from Inside "
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
            //Soemnte perder posse se arremeçar
            //currentGamePossessons--;
            uiManager.PlaybyPlayText(
                playerWithTheBall.playerLastName + " scored! Total: " + playerWithTheBall.PointsMatch
            );
            _matchUI.ResultActionPanel("S",0);
            if (HomeTeam.AdrenalineBar < HomeTeam.AdrenalineBarFull) HomeTeam.AdrenalineBar += adrenaline_addUp;
            //Damage Deal
            if (!isSimulation && teamWithball.IsPlayerTeam)
            {
                Team defendingTeam = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
                CalculateDamageAndReduceHP(defendingTeam, playerWithTheBall.CurrentZone);
            }
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
    //AI
    private AIAction AI_Tendency()
    {
        
        float awareness = playerWithTheBall.Awareness;
        float shooting = playerWithTheBall.Shooting;

        float passChance = PassEquation() * (1f + (awareness / 100f) * 0.4f);     // Awareness aumenta chance de passe
        float jukeChance = CalculateJukeProbability(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone);

        // Se o jogador está fora da sua zona preferida, aumenta chance de Juke
        int preferredZone = GetPreferredZone(playerWithTheBall); // função auxiliar simples
        if (playerWithTheBall.CurrentZone < preferredZone && playerWithTheBall.CurrentZone < 2)
        {
            jukeChance *= 1.35f;   // Aumenta chance de driblar para avançar
        }

        float shootChance = ScoringEquation(playerWithTheBall, playerDefending, playerWithTheBall.CurrentZone, 0)
                            * (1f + (shooting / 100f) * 0.35f);   // Shooting aumenta chance de arremesso

        float spChance = 0f;
        if (teamWithball.AdrenalineBar >= 50f)
        {
            // Quanto mais perto de 100, maior a chance de Special
            float adrenalineFactor = (teamWithball.AdrenalineBar - 50f) / 50f;
            spChance = ActivateSpecialAttk(true) * (0.6f + adrenalineFactor * 0.4f);
        }
        ////////////////////////////////////////////
        string bonusType = GetFormationBonus(teamWithball, playerWithTheBall);

        // Aplica o bônus conforme o tipo retornado pela formação
        if (bonusType.Contains("Shooting"))
        {
            shootChance *= 1.35f;   // Bônus forte para arremesso
        }
        else if (bonusType.Contains("Juke"))
        {
            jukeChance *= 1.40f;    // Bônus forte para juke/drible
        }
        else if (bonusType.Contains("Pass"))
        {
            passChance *= 1.30f;    // Bônus para passe
        }
        

        ///////////////////////////////////////

        if (currentGamePossessons == 1 && !teamWithball.IsPlayerTeam)
        {
            int scoreDifference = HomeTeam.Score - AwayTeam.Score;

            if (scoreDifference < 0)
            {
                if (playerWithTheBall.CurrentZone < 2 && jukeChance > 0.25f)
                {
                    Debug.Log("AI Last Possession - Losing and zone < 2 - Forcing Juke");
                    return AIAction.Juke;
                }
                else
                {
                    Debug.Log("AI Last Possession - Losing but zone = 2 - Trying Shoot");
                    return AIAction.Shoot;
                }
            }
            else if (scoreDifference == 0)
            {
                if (shootChance > 0.50f)
                    return AIAction.Shoot;
                else if (playerWithTheBall.CurrentZone < 2)
                    return AIAction.Juke;
                else
                    return AIAction.Shoot;
            }
        }

        float totalWeight = passChance + jukeChance + shootChance + spChance;

        if (totalWeight <= 0f)
            return AIAction.Shoot;

        float randomValue = UnityEngine.Random.value * totalWeight;

        if (randomValue < passChance)
            return AIAction.Pass;
        else if (randomValue < passChance + jukeChance)
            return AIAction.Juke;
        else if (randomValue < passChance + jukeChance + shootChance)
            return AIAction.Shoot;
        else
            return AIAction.Special;
    }
    private int GetPreferredZone(Player p)
    {
        if (p == null)
            return 1; // fallback: Mid

        // Compara os atributos principais de cada zona
        int insideValue = p.Inside;
        int midValue = p.Mid;
        int outsideValue = p.Outside;   // ou p.Shooting, se você preferir usar Shooting

        // Retorna a zona com o maior valor
        if (insideValue >= midValue && insideValue >= outsideValue)
            return 2; // Inside

        if (midValue >= outsideValue)
            return 1; // Mid

        return 0;     // Outside
    }
    // Função auxiliar pra juke chance (idêntica à lógica do TryBeatDefender)
    private float CalculateJukeProbability(Player offense, Player defense, int zone)
    {
        
        int offenseOVR = /* cálculo OVR */85;
        int defenseOVR = /* cálculo OVR */85;

        float offenseAvg = (offense.Consistency + offense.Control + offense.Juking +
                            GetZoneValue(offense, zone) + offenseOVR / 5f) / 4f;

        float offenseScore = offenseAvg + UnityEngine.Random.Range(-10f, 20f);

        float defenseAvg = (defense.Consistency + defense.Guarding + defense.Stealing +
                            GetZoneValue(defense, zone) + defenseOVR / 5f) / 5f;

        float defenseMedianBonus = defenseAvg * 0.3f;
        float defenseScore = defenseAvg + defenseMedianBonus;

        Team defendingTeam = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
        if (!defendingTeam.IsPlayerTeam)
            defenseScore *= 1.25f;

        float rawChance = offenseScore / (offenseScore + defenseScore + 50f);

        float finalChance = rawChance;

        if (teamWithball.IsPlayerTeam)
        {
            finalChance *= 0.85f;
            if (buff_Juke > 0)
                finalChance *= 1f + (buff_Juke / 100f);
        }
        else
        {
            finalChance *= 1.25f;
            if (leagueManager.isOnR8 || leagueManager.isOnR4 || leagueManager.isOnFinals)
                finalChance *= 1.10f;
        }

        return Mathf.Clamp(finalChance, 0.15f, 0.95f);
    }
    float PercentageMakePassToTeammate(int receiverIndex)
    {
        if (receiverIndex < 0 || receiverIndex >= teamWithball.playersListRoster.Count)
            return 0f;

        Player receiver = teamWithball.playersListRoster[receiverIndex];

        if (receiver == playerWithTheBall || receiver.HasTheBall)
            return 0f;

        // === CÁLCULO EXATO DA PROBABILIDADE (idêntico ao original) ===
        float adrenaline = teamWithball.IsPlayerTeam ? teamWithball.AdrenalineBar : 75f;
        float adrenalineFactor = adrenaline / 100f;

        float baseChance = 0.80f;
        float difficulty = teamWithball.IsPlayerTeam
            ? 1f - (adrenalineFactor * 0.32f)
            : 1f - (adrenalineFactor * 0.12f);

        float passBuffMultiplier = 1f + (buff_Pass / 100f);

        float finalChance = baseChance * difficulty * passBuffMultiplier;

        float staminaFactor = GetStaminaMultiplier(playerWithTheBall.CurrentStamina);
        finalChance *= staminaFactor;

        finalChance = Mathf.Clamp(finalChance, 0.38f, 0.94f);

        return finalChance;
    }
    //Damage
    private void CalculateDamageAndReduceHP(Team defendingTeam, int zone, bool isSP = false)
    {
        
        // HP atual do time defensor
        int currentHP = defendingTeam == HomeTeam ? homeHP : awayHP;

        // === DANO BASE POR ZONA ===
        int baseDamage = zone switch
        {
            0 => 12,   // Outside
            1 => 16,   // Mid
            2 => 22,   // Inside (mais perigoso)
            _ => 12
        };

        // === MULTIPLICADOR POR ADRENALINA ===
        float adrenaline = teamWithball.AdrenalineBar;           // 0 a 100
        float adrenalineMultiplier = 1f + (adrenaline / 100f) * 0.70f;   // Máximo +70% quando barra cheia

        // Dano final antes de SP
        float finalDamage = baseDamage * adrenalineMultiplier;

        // Se for Special Attack (isSP), aumenta significativamente
        if (isSP)
        {
            finalDamage *= 1.8f;        // Special Attack causa muito mais dano
        }

        // Reduz um pouco o dano quando o time defensor está com HP muito baixo (evita matar muito rápido)
        if (currentHP < 40)
            finalDamage *= 0.85f;

        int damageToApply = Mathf.RoundToInt(finalDamage);

        // Aplica o dano
        if (defendingTeam == HomeTeam)
            homeHP = Mathf.Max(0, homeHP - damageToApply);
        else
            awayHP = Mathf.Max(0, awayHP - damageToApply);

        Debug.Log($"Dano causado: {damageToApply} | Zona: {zone} | Adrenalina: {adrenaline} | HP restante: {(defendingTeam == HomeTeam ? homeHP : awayHP)}");
    }
    private float GetStaminaMultiplier(int stamina)
    {
        if (stamina >= 80) return 1.00f;   // Sem penalidade
        else if (stamina >= 60) return 0.92f;   // Leve
        else if (stamina >= 40) return 0.80f;   // Médio
        else return 0.60f;   // Forte (abaixo de 40)
    }
    private int GetDamageValue(int zone, bool isSP = false)
    {
        // Dano base por zona
        int baseDamage = zone switch
        {
            0 => 12,   // Outside
            1 => 16,   // Mid
            2 => 22,   // Inside
            _ => 12
        };

        // Multiplicador pela adrenalina (0 a 100)
        float adrenalineFactor = teamWithball.AdrenalineBar / 100f;
        float adrenalineMultiplier = 1f + (adrenalineFactor * 0.70f);   // até +70% quando barra cheia

        float finalDamage = baseDamage * adrenalineMultiplier;

        if (isSP)
        {
            finalDamage *= 1.85f;     // Special Attack causa muito mais dano
        }

        // Redução leve se o inimigo estiver com HP baixo
        int enemyHP = (teamWithball == HomeTeam) ? awayHP : homeHP;
        if (enemyHP < 40)
            finalDamage *= 0.85f;

        return Mathf.RoundToInt(finalDamage);
    }
    private int GetFacilityBonus(int level)
    {
        if (level >= 6)
            return 40;           // Nivel 6 ou superior = 20 pontos fixos

        // Niveis 0 a 5 - Progressão racional e crescente
        switch (level)
        {
            case 0: return 5;
            case 1: return 10;
            case 2: return 15;
            case 3: return 20;
            case 4: return 25;
            case 5: return 30;
            default: return 5;   // segurança
        }
    }
    //streak functions
    private void RegisterSuccess()
    {
        consecutiveSuccesses++;
        if (consecutiveSuccesses >= 5)
        {
            uiManager.PlaybyPlayText("Player is on fire!");
        }
    }

    private void ResetStreak()
    {
        consecutiveSuccesses = 0;
    }
    public void Matchpoint()
    {
        HomeTeam.Score += 50;
    }

    #region Simulation Specific Functions

    // Funções otimizadas para simulação - MAIS PONTUAÇÃO (menos 0x0)

    private bool SimulatePass()
    {
        // Na simulação o passe é bem mais fácil e quase sempre dá certo
        if (Random.value > 0.78f) // 78% de chance de sucesso no passe
        {
            playerDefending.StealsMatch++;
            playerWithTheBall.HasTheBall = false;
            SwitchPossession();
            return false;
        }

        if (TryPassBall())
        {
            if (teamWithball.AdrenalineBar < teamWithball.AdrenalineBarFull)
                teamWithball.AdrenalineBar += adrenaline_addUp;
            SelectDefender();
            return true;
        }
        return false;
    }

    private bool SimulateJuke()
    {
        // Juke mais fácil na simulação para avançar de zona
        if (Random.value < 0.72f) // 72% de chance de sucesso no juke
        {
            if (teamWithball.AdrenalineBar < teamWithball.AdrenalineBarFull)
                teamWithball.AdrenalineBar += adrenaline_addUp;
            SelectDefender();
            return true;
        }
        else
        {
            playerDefending.StealsMatch++;
            playerWithTheBall.HasTheBall = false;
            SwitchPossession();
            return false;
        }
    }

    private void SimulateShoot()
    {
        playerWithTheBall.CurrentZone = ChooseZone(playerWithTheBall);

        // Chance de acerto bem maior na simulação
        float shootChance = 0.68f; // base

        if (playerWithTheBall.CurrentZone == 1) shootChance = 0.78f;
        if (playerWithTheBall.CurrentZone == 2) shootChance = 0.86f;

        // Boost extra se estiver na última posse e perdendo
        if (currentGamePossessons <= 2 && !teamWithball.IsPlayerTeam)
        {
            int diff = HomeTeam.Score - AwayTeam.Score;
            if (diff < 0) shootChance += 0.18f; // força virada
        }

        if (Random.value < shootChance)
        {
            int points = playerWithTheBall.CurrentZone == 0 ? 4 : (playerWithTheBall.CurrentZone == 1 ? 5 : 6);
            playerWithTheBall.PointsMatch += points;
            teamWithball.Score += points;

            Team defending = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
            CalculateDamageAndReduceHP(defending, playerWithTheBall.CurrentZone);
        }

        playerWithTheBall.HasTheBall = false;
        SwitchPossession();
    }

    private void SimulateSpecial()
    {
        // Special bem mais fácil na simulação
        float specialChance = 0.75f;

        if (Random.value < specialChance)
        {
            int spPoints = 8;
            playerWithTheBall.PointsMatch += spPoints;
            teamWithball.Score += spPoints;

            if (teamWithball.AdrenalineBar >= teamWithball.AdrenalineBarFull)
                teamWithball.AdrenalineBar = 0;

            Team defending = teamWithball == HomeTeam ? AwayTeam : HomeTeam;
            CalculateDamageAndReduceHP(defending, playerWithTheBall.CurrentZone, true);
        }

        playerWithTheBall.HasTheBall = false;
        SwitchPossession();
    }

    #endregion
    public int GetAdrenalineAddUp(Team team)
    {
        if (team == null)
            return 5; // valor padrão de segurança

        int lvl = team.ArenaLvl;

        // Nível 0 = 5
        // Aumenta progressivamente até o máximo de 25 (a partir do level 6/7)
        if (lvl >= 7)
            return 25;

        // Graduação suave: level 0 = 5, level 6 = 23, level 7+ = 25
        return 5 + (lvl * 3);
    }
    public string GetFormationBonus(Team teamWithBall, Player playerWithTheBall)
    {
        if (teamWithBall == null || playerWithTheBall == null)
            return "Neutral";

        // Encontra a posição do jogador no elenco (0 a 3)
        int positionIndex = teamWithBall.playersListRoster.IndexOf(playerWithTheBall);

        if (positionIndex < 0 || positionIndex > 3)
            return "Neutral";

        // Pega o TeamStyle do time que tem a bola
        string formation = /*teamWithBall._teamStyle.ToString()*/currentFormation;

        switch (formation)
        {
            case "Neutral":
                return "Neutral";

            case "Wings":
                string[] wings = { "Defense", "Shooting", "Defense", "Shooting" };
                return wings[positionIndex];

            case "Marshall":
                string[] marshall = { "Shooting & Juke", "Neutral", "Neutral", "Neutral" };
                return marshall[positionIndex];

            case "Snake":
                string[] snake = { "Shooting", "Shooting", "Neutral", "Neutral" };
                return snake[positionIndex];

            case "Forward":
                string[] forward = { "Shooting", "Neutral", "Shooting", "Neutral" };
                return forward[positionIndex];

            case "Combo":
                string[] combo = { "Juke", "Weak", "Shooting", "Weak" };
                return combo[positionIndex];

            case "Horns":
                string[] horns = { "Weak", "Juke", "Weak", "Juke" };
                return horns[positionIndex];

            case "Artillery":
                string[] artillery = { "Shooting", "Weak", "Shooting", "Weak" };
                return artillery[positionIndex];

            case "Wall":
                string[] wall = { "Defense", "Defense", "Defense", "Defense" };
                return wall[positionIndex];

            case "Circle":
                string[] circle = { "Pass", "Pass", "Pass", "Pass" };
                return circle[positionIndex];

            case "GiveAndGo":
                string[] giveAndGo = { "Pass", "Juke", "Pass", "Juke"};
                return giveAndGo[positionIndex];

            case "GiveAndShoot":
                string[] giveAndShoot = { "Shooting", "Pass", "Shooting", "Pass" };
                return giveAndShoot[positionIndex];

            case "Dynamic":
                string[] dynamic = { "Pass", "Shooting", "Defense", "Juke"};
                return dynamic[positionIndex];

            default:
                return "Neutral";
        }
    }
}
