using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class TeamManagerUI : MonoBehaviour
{
    GameManager gameManager;
    LeagueManager leagueManager;
    MusicManager musicManager;
    PlayoffManager playoffManager;

    [Header("Intro")]
    [SerializeField] TextMeshProUGUI _text_NameTeam;

    [Header("Events Panel")]
    [SerializeField] GameObject _panelEventsTypes;
    [SerializeField] GameObject _panelEventsChoices;
    [SerializeField] Animator _animatorEventsType;
    Transform _transformEventTypeBtns;

    [Header("Facility Elelements")]
    [SerializeField] TextMeshProUGUI text_assistanceFacilityDescription;
    [SerializeField] TextMeshProUGUI text_facilityEffects;
    [SerializeField] TextMeshProUGUI text_facilityEffects1;
    [SerializeField] TextMeshProUGUI text_facilityEffects2;
    [SerializeField] TextMeshProUGUI text_facilityName;
    [SerializeField] Image _imageAssistanceFacilityImage;

    [Header("MainButtons")]
    [SerializeField] Animator _animator_trade;
    [SerializeField] Animator _animator_training;
    [SerializeField] Animator _animator_contract;

    [Header("Schedule")]
    [SerializeField]GameObject _scheduleArea;
    [SerializeField] Transform _schedulePanelTextsArea;
    [SerializeField] TradeManager tradeManager;

    [SerializeField] GameObject _EndBuildScreen;
    //Team Roster panel
    [Header("Team Roster")]
    [SerializeField] Image _image_playerPortrait;
    [SerializeField]GameObject _teamRoster;
    [SerializeField]Transform _teamRosterStartersPlayersText;
    [SerializeField] Transform _teamRosterBenchPlayerText;
    [SerializeField]int _currentTeamIndex;
    [SerializeField] TextMeshProUGUI _currentTeamNameTeamRosterText;
    [SerializeField] Image _currentTeamImage;
    [SerializeField] Image _awayteamImage;
    [SerializeField] Transform _text_playerInfoStats;
    [SerializeField] TextMeshProUGUI _text_playerInfoLastName;
    [SerializeField] Transform _text_ContractInfo;
    [SerializeField] Image _image_PersonalityImage;
    [SerializeField] Image _image_playerStyle;
    [SerializeField] TextMeshProUGUI _text_playerNUmber;
    [SerializeField] TextMeshProUGUI text_playerAge;
    [SerializeField] GameObject careerStatsArea;
    [SerializeField] Transform careerStats;
    //new/rework
    [SerializeField] Transform transform_teamBtnsPlayers;
    [SerializeField] TextMeshProUGUI text_TeamPlayerName;
    [SerializeField] TextMeshProUGUI text_TeamPlayerOvr;
    [SerializeField] TextMeshProUGUI text_TeamPlayerContract;
    [SerializeField] TextMeshProUGUI text_text_TeamPlayerSalary;
    [SerializeField] TextMeshProUGUI text_TeamPlayerJersey;
    [SerializeField] Image image_teamPortraitPlayer;
    [SerializeField] Image image_teamPlayerPersonality;
    [SerializeField] Image image_TeamPlayerArchtype;
    [SerializeField] Transform transform_TeamPlayerAllStats;
    Player currentPlayer;
    Sprite _sprite;
    
    [Header("Equips")]
    //Equips
    [SerializeField] Transform equipAreaText;
    [SerializeField]GameObject choicesForTheWeek;
    [SerializeField] GameObject CurrentEventChoiceArea;
    
    [Header("Trade")]
    //Trade
    [SerializeField]GameObject _tradePanel;
    [SerializeField] Transform _trade_btn_PlayersFronControlledTeam;
    public GameObject TradeReceivePlayerArea;
    [SerializeField] TextMeshProUGUI text_teamFrontOfficeGrade;
    [SerializeField] TextMeshProUGUI text_tradeResult;
    [SerializeField] GameObject panel_tradeResult;
    [SerializeField] TextMeshProUGUI text_currentFrontOfficePoints;
    [SerializeField] Image image_playerToTrade;
    [SerializeField] Image image_playerTeam;
    public Transform transform_trade_AssistanceResultPortrait;
    //New/Rework
    [SerializeField] Transform transform_tradeMyPlayersBtn;
    [SerializeField] GameObject go_tradeFinished;
    [SerializeField] GameObject prefab_TradePlayerOptionToTradeFor;
    [SerializeField] Transform transform_tradeBtnOptions;
    [SerializeField] Transform teste;
    [SerializeField] GameObject prefabTrade;
    [SerializeField] Sprite sprite_transparent;
    //playerMyTeamInfo
    [SerializeField] Image image_trade_MyTeamImage;
    [SerializeField] Image image_trade_MyPlayerToTradePortrait;
    [SerializeField] TextMeshProUGUI text_trade_myPlayerName;
    [SerializeField] TextMeshProUGUI text_trade_MyPlayerOVR;
    [SerializeField] TextMeshProUGUI text_trade_MyPlayerAge;
    [SerializeField] Image image_Trade_myPlayerPersonality;
    [SerializeField] Image image_trade_MyPlayerArchtype;
    [SerializeField] Image image_trade_receiveTeamImage;
    [SerializeField] Image image_trade_playerReceivePortrait;
    [SerializeField] TextMeshProUGUI text_trade_receiveName;
    [SerializeField] TextMeshProUGUI text_trade_receiveOVR;
    [SerializeField] TextMeshProUGUI text_trade_receiveAge;
    [SerializeField] Image image_trade_receivePersonality;
    [SerializeField] Image image_trade_receiveArchtype;
    [SerializeField]Player trade_playerToTrade;
    [SerializeField] public TextMeshProUGUI text_tradeCostOfTrade;
    [SerializeField] TextMeshProUGUI text_tradeWarningForNoTrade;
    [SerializeField] TextMeshProUGUI text_trade_frontoffeicePoints;
    int trade_costOfTrade;
    Player trade_PlayerToReceive;
    int trade_teamIndex;


    [Header("Training")]
    //Training
    [SerializeField] TrainingManager trainingManager;
    [SerializeField] GameObject _trainingPanel;
    [SerializeField] Transform _training_btns;
    [SerializeField]public TextMeshProUGUI _textPlayerSelected;
    [SerializeField]public TextMeshProUGUI _textDrillSelected;
    [SerializeField] Image _training_assistancePortrait;
    [SerializeField] Sprite _training_AssistanceSprite;
    [SerializeField] public Image image_currentPlayerPortraitToTrain;
    [SerializeField] public TextMeshProUGUI text_trainingType;
    [SerializeField] public Transform transform_assistance_ResultPortrait;
    [SerializeField] public TextMeshProUGUI training_playerOVR;
    [SerializeField] public TextMeshProUGUI training_playerAge;
    
    //public TextMeshProUGUI text_playerToTrain;
    //public TextMeshProUGUI text_drill;
    [SerializeField] public GameObject _trainingResultPanel;
    [SerializeField] TextMeshProUGUI _text_TrainingGrade;

    [Header("Standings")]
    [SerializeField] GameObject _standingsPanel;
    [SerializeField] Transform _standingsPlacement;

    [Header("SalaryCap")]
    [SerializeField] TextMeshProUGUI _text_CurrentTeamSalary;

    [Header("Options---Old")]
    [SerializeField] GameObject _optionsPanel;
    [SerializeField] Button _optionsQuitBtn;

    [Header("News Info")]
    [SerializeField] List<string> List_VoxEdgeNewsLines = new List<string>();
    [SerializeField] List<string> list_VoxEdgeNewsResults = new List<string>();
    [SerializeField] List<string> list_VoxelEdgePlayersNews = new List<string>();
    [SerializeField] TextMeshProUGUI text_newsInfo;
    [SerializeField]public List<Sprite> sprites_newsSprites = new List<Sprite>();
    [SerializeField] Animator _animator_newsTransition;
    [SerializeField] GameObject panel_newsPanel;

    [Header("LeagueHistory")]
    [SerializeField] GameObject leagueHistoryPanel;
    //TextAreas
    [SerializeField] Transform recordsArea;
    [SerializeField] Transform awardsUpdatesArea;
    [SerializeField] Transform playerTeamRecords;
    Player playerMvp;

    [Header("Contract")]
    [SerializeField] ContractManager contractManager;
    [SerializeField] GameObject contract_ContractPainel;
    [SerializeField] GameObject contract_asstancePanel;
    [SerializeField] Transform contract_newContractValuesArea;
    [SerializeField] Transform contract_PlayerbuttonsArea;
    [SerializeField] Transform contract_changeValuesButtons;
    [SerializeField] TextMeshProUGUI contract_resultNegotiationText;
    [SerializeField] TextMeshProUGUI contract_CurrentPlayerGames;
    [SerializeField] TextMeshProUGUI contract_CurrentPlayerSalary;
    [SerializeField] TextMeshProUGUI contract_playerName;
    [SerializeField] Image contract_selectePlayer;
    [SerializeField] Image image_assistance;
    [SerializeField] Sprite sprite_AssistanceHappy;
    [SerializeField] Sprite sprite_AssistanceFail;
    [SerializeField] Transform transform_contract_AssistancePortrait;
    [SerializeField] TextMeshProUGUI contract_playerOvr;
    [SerializeField] TextMeshProUGUI contract_PlayerAge;
    [SerializeField] TextMeshProUGUI contract_text_contractCostValue;
    [SerializeField] TextMeshProUGUI contract_text_currentFrontOfficePoints;
    int newGamesValue;
    int newSalaryValue;
    int indexForPlayer;
    Player _contractPlayer;

    [Header("GameOverPanel")]
    [SerializeField] Animator gameoverPanel;
    [SerializeField] Button gameover_Btn;

    [Header("FreeAgents")]
    [SerializeField] public GameObject _freeAgents_panel;
    [SerializeField] public FreeAgentManager freeAgentManager;
    public bool canProgressWithWeek = false;

    [Header("InfoArea")]
    [SerializeField] GameObject panel_infoButtons;
    [SerializeField] TextMeshProUGUI text_expiringContractsWarning;
    [SerializeField] Animator animator_expiringContractBtn;

    [Header("Facilities")]
    [SerializeField] TextMeshProUGUI text_facilitiesOfficeLvl;
    [SerializeField] TextMeshProUGUI text_facilitiesFinancesLvl;
    [SerializeField] TextMeshProUGUI text_facilitiesMarketingLvl;
    [SerializeField] TextMeshProUGUI text_facilitiesTeamEquipsLvl;
    [SerializeField] TextMeshProUGUI text_facilitiesArenaLvl;
    [SerializeField] TextMeshProUGUI text_facilitiesMedicalLvl;

    [Header("PlayerEvents")]
    [SerializeField] PlayerEventsManager playerEventsManager;
    [SerializeField] GameObject panel_playerEventPanel;
    [SerializeField] Button btn_playerEventButton0;
    [SerializeField] Button btn_playerEventButton1;
    [SerializeField] TextMeshProUGUI text_playerEventDescription;

    [Header("AssistancePanel")]
    [SerializeField] GameObject assiatncePanel;
    [SerializeField] TextMeshProUGUI text_TaskArea;
    [SerializeField] TextMeshProUGUI text_btn_toggle;
    [SerializeField] GameObject panel_assisyanceWarning;

    [Header("ResultPanel")]
    [SerializeField] GameObject resultPanel;
    [SerializeField] Animator animator_resultPanel;
    [SerializeField] Button btn_returnToMainScreen;
    

    [SerializeField] Image image_resultPanel_teamLogo;
    [SerializeField] TextMeshProUGUI text_resultPanel_teamName;
    [SerializeField] TextMeshProUGUI text_resultPanel_teamPlacement;
    [SerializeField] TextMeshProUGUI text_resultPanel_teamWins;
    [SerializeField] TextMeshProUGUI text_resultPanel_teamDraws;
    [SerializeField] TextMeshProUGUI text_resultPanel_teamDefeats;
    //MVP
    [SerializeField] Player mvp;
    [SerializeField] Image mvpPortrait;
    [SerializeField] TextMeshProUGUI text_resultPanel_mvpName;
    [SerializeField] TextMeshProUGUI text_resultPanel_mvpPtsGame;
    [SerializeField] TextMeshProUGUI text_resultPanel_mvpStealsGames;
    [SerializeField] TextMeshProUGUI text_resultPanel_mvpGamesPlayed;

    [Header("GameOverPanel")]
    [SerializeField] GameObject go_playerGameOverFiredPanel;
    [SerializeField] Button btn_go_ReturnToMainTitle;
    bool isGameOver = false;

    [Header("UI")]
    [SerializeField]TextMeshProUGUI WeekText;
    [SerializeField] Image image_teamIcon;
    [SerializeField] GameObject _playoffsAdvBtn;
    [SerializeField] TextMeshProUGUI text_teamStyle;
    GameObject _advBtn;//to Advance Button Elements

    [Header("Menu Animations")]
    [SerializeField] Animator animator_SchedulePanel;
    [SerializeField] Animator animator_TeamPanel;
    [SerializeField] Animator animator_TradingPanel;
    [SerializeField] Animator animator_TrainingPanel;
    [SerializeField] Animator animator_ContractsPanel;
    [SerializeField] Animator animator_StandingsPanel;
    

    [SerializeField] GameObject tutorialPanel;

    [SerializeField] Button _closeGameForTestersBtn;
    //Testing
    [SerializeField]Transform teamStatsTextsArea;
    // Start is called before the first frame update
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        playoffManager = GameObject.Find("PlayoffManager").GetComponent<PlayoffManager>();
        //Schedule
        ScheduleUpdated();
        leagueManager.CreateStandings();
        PopulateStandings();
    }
    void Start()
    {
        if (leagueManager.isGameOver)
        {
            isGameOver = true;
        }
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            if (gameManager.leagueTeams[i].isChampion == true &&
                gameManager.leagueTeams[i].IsPlayerTeam == true)
            {
                isGameOver = true;
                break;
            }
        }
       
        if (leagueManager.Week >= gameManager.leagueTeams.Count)
        {

            if (IsPlayerTeamInTop8())
            {
                leagueManager.isOnR8 = true;
            }
            else
            {
                isGameOver = true;           
                leagueManager.isOnR8 = false;
            }
        }

        // Verificação extra de playoffs
        if (leagueManager.Week > gameManager.leagueTeams.Count - 1 && leagueManager.isOnR8 == false)
        {
            isGameOver = true;
        }

        if (gameManager.playerTeam.Moral <= 0)
        {
            isGameOver = true;               // Só define true quando necessário
                                             
        }

        if (leagueManager.isOnR8)
        {
            print("Playoffs rackerts made");
            playoffManager.CreatePlayoffBracket();
        }

        // Decisão final - agora funciona corretamente
        if (isGameOver == false)
        {
            _advBtn = GameObject.Find("Advance Button");
            if (gameManager.playerTeam == null)
            {
                gameManager.ReassignPlayerTeam();
            }
            Debug.Log("[TeamManagerUI] Start() iniciado");

            EventsManager eventsManager = FindFirstObjectByType<EventsManager>();
            if (eventsManager != null && leagueManager.canGenerateEvents && leagueManager.canStartANewWeek)
            {
                Debug.Log("[TeamManagerUI] Chamando StartNewWeekEvents()");
                eventsManager.StartNewWeekEvents();
            }
            else
            {
                _panelEventsChoices.SetActive(false);
                _panelEventsTypes.SetActive(false);
            }

            _text_NameTeam.text = gameManager.playerTeam.TeamName + " Headquarters";
            text_teamStyle.text = gameManager.playerTeam._teamStyle.ToString();
            musicManager.RestoreMutedAudioSources();
            _scheduleArea.SetActive(false);
            SetTeamIcon();
            AdvButtonUpdate();
            SetTextOfFacilities();
            WeekText.text = leagueManager.Week.ToString();

            if (leagueManager.Week > gameManager.leagueTeams.Count - 1)
            {
                WeekText.text = "Playoffs";
            }

            if (leagueManager.canGenerateEvents)
            {
                _panelEventsChoices.SetActive(true);
                _panelEventsTypes.SetActive(true);
            }

            Debug.Log("[TeamManagerUI] Start() finalizado - UI básica carregada");
        }
        else
        {
            ResultPanelCreation();
            btn_returnToMainScreen.onClick.RemoveAllListeners();
            btn_returnToMainScreen.onClick.AddListener(() => StartNewLeagueRun());
        }
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if(leagueManager.canGenerateEvents == false && leagueManager.canStartANewWeek == false)
        {
            CurrentEventChoiceArea.SetActive(false);
        }

        if(gameManager.mode == GameManager.GameMode.TeamManagement && leagueManager.canStartANewWeek == false && leagueManager.isGameOver == false)
        {
            if (GameObject.Find("Week Text"))
            {
                //GameObject.Find("Week Text").GetComponent<TextMeshProUGUI>().text = leagueManager.Week.ToString();
            }
            //Team Moral/FrontOffice/FansSupport
            if (GameObject.Find("MoralePointsText"))
            {
                GameObject.Find("MoralePointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.Moral.ToString();
            }
            if (GameObject.Find("FrontOfficePointsText"))
            {
                GameObject.Find("FrontOfficePointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.FrontOfficePoints.ToString();
            }
            if (GameObject.Find("FanSupportPointsText"))
            {
                GameObject.Find("FanSupportPointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.FansSupportPoints.ToString();
            }
            if (GameObject.Find("EffortPointsText"))
            {
                GameObject.Find("EffortPointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.EffortPoints.ToString();
            }
            UpdateFacilities();
        }
         
        
        
       

        
        /*
        // Btns Animations (estas são seguras mesmo se playerTeam for null)
        _animator_trade.SetBool("On", leagueManager.canTrade);
        _animator_training.SetBool("On", leagueManager.canTrain);
        _animator_contract.SetBool("On", leagueManager.canNegociateContract);
        */

    }
    //GameOverReset
    public void ResetRun()
    {
        leagueManager.CanStartANewRun = true;
        //gameManager.ResetRunTeams();
        gameManager.playerTeam = null;
        

        leagueManager.isGameOver = false;
        leagueManager.CanStartANewRun = true;
        gameManager.saveSystem.SaveLeague();
        //loadscene
        //reset teams
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            gameManager.saveSystem.ClearSave(gameManager.leagueTeams[i].TeamName, gameManager.leagueTeams[i]);
        }
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            gameManager.saveSystem.SaveTeamInfo(gameManager.leagueTeams[i]);//mudar apara a lista fixa depois
        }
        gameManager.DestroyAllTeams();
        

    }
    //Facilities
    void UpdateFacilities()
    {
        if (gameManager == null || gameManager.playerTeam == null)
        {
            Debug.LogWarning("[TeamManagerUI] UpdateFacilities: playerTeam ainda é null.");
            return;
        }

        if (text_facilitiesOfficeLvl != null)
            text_facilitiesOfficeLvl.text = "Office LVL: " + gameManager.playerTeam.OfficeLvl.ToString();

        if (text_facilitiesFinancesLvl != null)
            text_facilitiesFinancesLvl.text = "Finances LVL: " + gameManager.playerTeam.FinancesLvl.ToString();

        if (text_facilitiesMarketingLvl != null)
            text_facilitiesMarketingLvl.text = "Marketing LVL: " + gameManager.playerTeam.MarketingLvl.ToString();

        if (text_facilitiesTeamEquipsLvl != null)
            text_facilitiesTeamEquipsLvl.text = "Equipments LVL: " + gameManager.playerTeam.TeamEquipmentLvl.ToString();

        if (text_facilitiesArenaLvl != null)
            text_facilitiesArenaLvl.text = "Arena LVL: " + gameManager.playerTeam.ArenaLvl.ToString();

        if (text_facilitiesMedicalLvl != null)
            text_facilitiesMedicalLvl.text = "Med LVL: " + gameManager.playerTeam.MedicalLvl.ToString();
    }
    public void UpdateMoralAndPointsTexts()
    {
        if (gameManager == null || gameManager.playerTeam == null)
            return;

        // Moral
        if (GameObject.Find("MoralePointsText") != null)
            GameObject.Find("MoralePointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.Moral.ToString();

        // Front Office
        if (GameObject.Find("FrontOfficePointsText") != null)
            GameObject.Find("FrontOfficePointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.FrontOfficePoints.ToString();

        // Fan Support
        if (GameObject.Find("FanSupportPointsText") != null)
            GameObject.Find("FanSupportPointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.FansSupportPoints.ToString();

        // Effort Points
        if (GameObject.Find("EffortPointsText") != null)
            GameObject.Find("EffortPointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.EffortPoints.ToString();
    }
    //Btn Animations

    //News
    void NewsUpdate()
    {
        string newsLine = List_VoxEdgeNewsLines[UnityEngine.Random.Range(0, List_VoxEdgeNewsLines.Count)];
        text_newsInfo.text = newsLine;
    }
    public void PlayNewsTransition()
    {
        _animator_newsTransition.SetTrigger("On");
    }
    IEnumerator NewsLoop(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            PlayNewsTransition();
            NewsUpdate();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void SetTeamIcon()
    {
        Sprite sprite;
        sprite = Resources.Load<Sprite>("2D/Team Logos/" + gameManager.playerTeam.TeamName);
        image_teamIcon.sprite = sprite;
    }
    public void UpdateTeamRoster()
    {
        TeamRoster();
    }
    //Schedule Updated
    public void ScheduleUpdated()
    {
        Sprite sprite;
        string nameForOpponent;
        if (_schedulePanelTextsArea != null)
        {
            for (int i = 0; i < gameManager.playerTeam._schedule.Count; i++)
            {
                print(gameManager.playerTeam._schedule.Count);
                nameForOpponent = gameManager.playerTeam._schedule[i].TeamName;
                sprite = Resources.Load<Sprite>("2D/Team Logos/" + nameForOpponent);
                //_schedulePanelTextsArea.GetChild(i).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam._schedule[i].TeamName.ToString();
                Image oppImage = _schedulePanelTextsArea.GetChild(i).GetChild(0).GetComponent<Image>();
                oppImage.sprite = sprite;
                _schedulePanelTextsArea.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = nameForOpponent;
                _schedulePanelTextsArea.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Week " + (i + 1);
            }
        }
        leagueManager.CreateStandings();
    }

    //EquipUI
    void EquipUI()
    {
        for (int i = 0; i < equipAreaText.childCount; i++)
        {
            equipAreaText.GetChild(i).GetComponent<TextMeshProUGUI>().text =
                gameManager.playerTeam.GetEquipment()[i].Name.ToString() + " " + gameManager.playerTeam.GetEquipment()[i].Level.ToString();

        }
        if (leagueManager.canStartANewWeek == false)
        {
            choicesForTheWeek.SetActive(false);
        }
    }
    public void UpdateWeek()
    {
        print("Hre is increasing teh week");
        leagueManager.IncreaseWeek();
        leagueManager.CreateStandings();
        WeekText.text = leagueManager.Week.ToString();
    }
    
    
    //Advance Button Update the elements
    public void AdvButtonUpdate()
    {
        string tn;
        Sprite sp;
        Sprite sp1;

        // === PROTEÇÃO PRINCIPAL ===
        if (gameManager == null || gameManager.playerTeam == null)
        {
            Debug.LogWarning("AdvButtonUpdate: gameManager.playerTeam ainda é null. Aguardando atribuição...");
            // Limpa as imagens para evitar visual quebrado
            if (_currentTeamImage != null) _currentTeamImage.sprite = null;
            if (_awayteamImage != null) _awayteamImage.sprite = null;
            return;
        }

        // ====================== PLAYOFFS ======================
        if (leagueManager.isOnFinals)
        {
            Debug.Log("[AdvButtonUpdate] Modo FINALS ativado");

            // Player Team Logo
            tn = gameManager.playerTeam.TeamName;
            sp = Resources.Load<Sprite>("2D/Team Logos/" + tn);
            if (sp != null)
            {
                Image myImageComponent = _advBtn.transform.GetChild(1).GetComponent<Image>();
                if (myImageComponent != null) myImageComponent.sprite = sp;
                if (_currentTeamImage != null) _currentTeamImage.sprite = sp;
            }

            // Adversário na Final
            Team opponent = null;
            if (leagueManager.List_Finalist != null)
            {
                foreach (Team t in leagueManager.List_Finalist)
                {
                    if (t != null && !t.IsPlayerTeam)
                    {
                        opponent = t;
                        break;
                    }
                }
            }

            if (opponent != null)
            {
                sp1 = Resources.Load<Sprite>("2D/Team Logos/" + opponent.TeamName);
                if (sp1 != null)
                {
                    Image image = _advBtn.transform.GetChild(2).GetComponent<Image>();
                    if (image != null) image.sprite = sp1;
                    if (_awayteamImage != null) _awayteamImage.sprite = sp1;
                }
            }

            if (WeekText != null) WeekText.text = "Finals";
            return;
        }
        else if (leagueManager.isOnR4)
        {
            Debug.Log("[AdvButtonUpdate] Modo R4 (Semifinal) ativado");

            // Player Team Logo
            tn = gameManager.playerTeam.TeamName;
            sp = Resources.Load<Sprite>("2D/Team Logos/" + tn);
            if (sp != null)
            {
                Image myImageComponent = _advBtn.transform.GetChild(1).GetComponent<Image>();
                if (myImageComponent != null) myImageComponent.sprite = sp;
                if (_currentTeamImage != null) _currentTeamImage.sprite = sp;
            }

            // Adversário no R4
            Team opponent = null;
            if (leagueManager.List_R4Teams != null)
            {
                foreach (Team t in leagueManager.List_R4Teams)
                {
                    if (t != null && !t.IsPlayerTeam)
                    {
                        opponent = t;
                        break;
                    }
                }
            }

            if (opponent != null)
            {
                sp1 = Resources.Load<Sprite>("2D/Team Logos/" + opponent.TeamName);
                if (sp1 != null)
                {
                    Image image = _advBtn.transform.GetChild(2).GetComponent<Image>();
                    if (image != null) image.sprite = sp1;
                    if (_awayteamImage != null) _awayteamImage.sprite = sp1;
                }
            }

            if (WeekText != null) WeekText.text = "Semifinals";
            return;
        }
        else if (leagueManager.isOnR8)
        {
            Debug.Log("[AdvButtonUpdate] Modo R8 (Quartas) ativado");

            // Player Team Logo
            string teamN = gameManager.playerTeam.TeamName;
            Sprite spriteTeamH = Resources.Load<Sprite>("2D/Team Logos/" + teamN);
            if (spriteTeamH != null)
            {
                Image myImageComponent = _advBtn.transform.GetChild(1).GetComponent<Image>();
                if (myImageComponent != null) myImageComponent.sprite = spriteTeamH;
                if (_currentTeamImage != null) _currentTeamImage.sprite = spriteTeamH;
            }

            // === BLOCO ALTERADO PARA R8 ===
            // Busca o adversário de forma mais segura
            Team opponent = null;

            if (leagueManager.List_R8Teams != null && leagueManager.List_R8Teams.Count > 0)
            {
                // Tenta encontrar um time que NÃO seja o do jogador
                foreach (Team t in leagueManager.List_R8Teams)
                {
                    if (t != null && t != gameManager.playerTeam)
                    {
                        opponent = t;
                        break;
                    }
                }
                opponent = leagueManager.List_R8Teams[1];
                // Fallback: se não encontrou por referência, pega o primeiro time diferente
                if (opponent == null)
                {
                    foreach (Team t in leagueManager.List_R8Teams)
                    {
                        if (t != null && t.TeamName != gameManager.playerTeam.TeamName)
                        {
                            opponent = t;
                            break;
                        }
                    }
                }
            }

            // Aplica a imagem do adversário
            if (opponent != null)
            {
                Sprite spriteTeamAway = Resources.Load<Sprite>("2D/Team Logos/" + opponent.TeamName);
                if (spriteTeamAway != null)
                {
                    Image image = _advBtn.transform.GetChild(2).GetComponent<Image>();
                    if (image != null) image.sprite = spriteTeamAway;
                    if (_awayteamImage != null) _awayteamImage.sprite = spriteTeamAway;
                }
            }
            else
            {
                Debug.LogWarning("[AdvButtonUpdate] Não encontrou adversário na List_R8Teams");
            }
            // === FIM DO BLOCO ALTERADO ===

            if (WeekText != null) WeekText.text = "Playoffs";
            return;
        }

        //reguçar season!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Player Team Logo
        string teamName = gameManager.playerTeam.TeamName;
        Sprite sprite = Resources.Load<Sprite>("2D/Team Logos/" + teamName);

        if (sprite != null)
        {
            Image myImageComponent = _advBtn.transform.GetChild(1).GetComponent<Image>();
            if (myImageComponent != null) myImageComponent.sprite = sprite;

            if (_currentTeamImage != null) _currentTeamImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"Logo não encontrado para o time: {teamName}");
        }

        // Away Team Logo
        int currentWeek = Mathf.Max(0, leagueManager.Week - 1);
        Sprite sprite1 = null;

        if (gameManager.playerTeam._schedule != null &&
            currentWeek < gameManager.playerTeam._schedule.Count &&
            gameManager.playerTeam._schedule[currentWeek] != null)
        {
            string awayTeamName = gameManager.playerTeam._schedule[currentWeek].TeamName;
            sprite1 = Resources.Load<Sprite>("2D/Team Logos/" + awayTeamName);

            if (sprite1 != null)
            {
                Image image = _advBtn.transform.GetChild(2).GetComponent<Image>();
                if (image != null) image.sprite = sprite1;
                if (_awayteamImage != null) _awayteamImage.sprite = sprite1;
            }
        }
        else
        {
            Debug.LogWarning($"Não foi possível encontrar o adversário na semana {currentWeek}");
            // Opcional: colocar imagem de "Bye" ou deixar vazio
        }

    }
    //TeamRoster
    public void TeamRoster()
    {
        _currentTeamNameTeamRosterText.text = gameManager.leagueTeams[_currentTeamIndex].TeamName.ToString();
        //Starters
        for (int i = 0; i < 4; i++)
        {
            _teamRosterStartersPlayersText.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i].playerFirstName.ToString();
            _teamRosterStartersPlayersText.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i].playerLastName.ToString();
            _teamRosterStartersPlayersText.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i].ovr.ToString();
            //_teamRosterStartersPlayersText.GetChild(i).GetComponent<Button>().onClick.AddListener(() => PlayerStats(i));
        }
        //Bench
        for (int i = 0; i < 4; i++)
        {
            _teamRosterBenchPlayerText.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i + 4].playerFirstName.ToString();
            _teamRosterBenchPlayerText.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i + 4].playerLastName.ToString();
            _teamRosterBenchPlayerText.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i + 4].ovr.ToString();
            //_teamRosterBenchPlayerText.GetChild(i).GetComponent<Button>().onClick.AddListener(() => PlayerStats(i+4));
        }
        
    }
    public void CurrentPlayerStats(int index)
    {
        _text_playerInfoLastName.text = gameManager.playerTeam.playersListRoster[index].playerLastName.ToString();
        gameManager.playerTeam.playersListRoster[index].UpdateOVR();
        _text_playerInfoStats.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Shooting.ToString();
        _text_playerInfoStats.GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Inside.ToString();
        _text_playerInfoStats.GetChild(2).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Mid.ToString();
        _text_playerInfoStats.GetChild(3).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Outside.ToString();
        _text_playerInfoStats.GetChild(4).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Awareness.ToString();
        _text_playerInfoStats.GetChild(5).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Defending.ToString();
        _text_playerInfoStats.GetChild(6).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Guarding.ToString();
        _text_playerInfoStats.GetChild(7).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Stealing.ToString();
        _text_playerInfoStats.GetChild(8).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Juking.ToString();
        _text_playerInfoStats.GetChild(9).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Consistency.ToString();
        _text_playerInfoStats.GetChild(10).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Control.ToString();
        _text_playerInfoStats.GetChild(11).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Positioning.ToString();
        _text_playerNUmber.text = gameManager.playerTeam.playersListRoster[index].J_Number.ToString();
        text_playerAge.text = gameManager.playerTeam.playersListRoster[index].Age.ToString();

        //Sprite alteration/update
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait];
        _image_playerPortrait.sprite = sprite;

        //Contract Info
        _text_ContractInfo.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].ContractYears.ToString();
        _text_ContractInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Salary.ToString();

        //Icons
        IconsUpdate(gameManager.playerTeam.playersListRoster[index]);
        _image_PersonalityImage.sprite = _sprite;
        currentPlayer = gameManager.playerTeam.playersListRoster[index];

        CareerStatsUpdate(index, gameManager.playerTeam.playersListRoster[index]);
    }
    public void CareerStatsUpdate(int index, Player player)
    {
        
        careerStats.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.CareerPoints.ToString();//testing
        if (leagueManager.Week < 2) careerStats.GetChild(1).GetComponent<TextMeshProUGUI>().text = "none";
        else careerStats.GetChild(1).GetComponent<TextMeshProUGUI>().text = ((float)player.CareerPoints/(float)player.CareerGamesPlayed).ToString();
        careerStats.GetChild(2).GetComponent<TextMeshProUGUI>().text = player.CareerSteals.ToString();
        careerStats.GetChild(3).GetComponent<TextMeshProUGUI>().text = ((float)player.CareerSteals/(float)player.CareerGamesPlayed).ToString();
        careerStats.GetChild(4).GetComponent<TextMeshProUGUI>().text = player.CareerGamesPlayed.ToString();
        playerEventsManager.ValidateBond(player, gameManager.playerTeam.playersListRoster);
        /*
        if (player.bondPlayer != null) careerStats.GetChild(5).GetComponent<TextMeshProUGUI>().text = player.bondPlayer.playerFirstName + player.bondPlayer.playerLastName;
        else careerStats.GetChild(5).GetComponent<TextMeshProUGUI>().text = "None";
        */

    }
    void IconsUpdate(Player player)
    {
        //Personality Icons
        Sprite sprite = null; //= Resources.Load<Sprite>("Assets/Resources/2D/Player Personalities/UI_icon_Personalite_01.png");
        switch (player.Personality)
        {
            case 1:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                break;
            case 2:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                break;
            case 3:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                break;

            case 4:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                break;

            case 5:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05");
                break;

            default:
                break;
        }
        _sprite = sprite;
        
        
    }
    public void TeamSetPlayerBtns()
    {
        for (int i = 0; i < transform_teamBtnsPlayers.childCount; i++)
        {
            transform_teamBtnsPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                gameManager.playerTeam.playersListRoster[i].playerFirstName + " " + gameManager.playerTeam.playersListRoster[i].playerLastName;
            transform_teamBtnsPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].SetOVR().ToString();
        }
    }
    //new team page
    public void TeamSetPlayersInfo(int index)
    {
        text_TeamPlayerName.text = gameManager.playerTeam.playersListRoster[index].playerFirstName[0] + ". " + gameManager.playerTeam.playersListRoster[index].playerLastName;
        text_TeamPlayerOvr.text = gameManager.playerTeam.playersListRoster[index].SetOVR().ToString();
        text_TeamPlayerContract.text = gameManager.playerTeam.playersListRoster[index].ContractYears.ToString() + " Games";
        text_text_TeamPlayerSalary.text = gameManager.playerTeam.playersListRoster[index].Salary.ToString();
        text_TeamPlayerJersey.text = gameManager.playerTeam.playersListRoster[index].J_Number.ToString();
        //Stats
        transform_TeamPlayerAllStats.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Shooting.ToString();
        transform_TeamPlayerAllStats.GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Inside.ToString();
        transform_TeamPlayerAllStats.GetChild(2).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Mid.ToString();
        transform_TeamPlayerAllStats.GetChild(3).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Outside.ToString();
        transform_TeamPlayerAllStats.GetChild(4).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Awareness.ToString();
        transform_TeamPlayerAllStats.GetChild(5).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Defending.ToString();
        transform_TeamPlayerAllStats.GetChild(6).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Guarding.ToString();
        transform_TeamPlayerAllStats.GetChild(7).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Stealing.ToString();
        transform_TeamPlayerAllStats.GetChild(8).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Juking.ToString();
        transform_TeamPlayerAllStats.GetChild(9).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Consistency.ToString();
        transform_TeamPlayerAllStats.GetChild(10).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Control.ToString();
        transform_TeamPlayerAllStats.GetChild(11).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Positioning.ToString();
        //Portrait
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait];
        image_teamPortraitPlayer.sprite = sprite;
        //Archtype
        Sprite[] sprites1 = Resources.LoadAll<Sprite>("2D/UI/Archtype");
        Sprite archtypeSprite = null;
        
        if (gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait >= 0 || gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait < 20)
        {
            archtypeSprite = sprites1[0];
        }
        if (gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait >= 21 && gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait <= 40)
        {
            
            archtypeSprite = sprites1[1];
        }
        if (gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait >= 41 && gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait <= 60)
        {
            
            archtypeSprite = sprites1[2];
        }
        if (gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait >= 61 && gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait <= 80)
        {
            archtypeSprite = sprites1[3];
        }
        image_TeamPlayerArchtype.sprite = archtypeSprite;
        //Personality
        Sprite personalitySprite = null;
        switch (gameManager.playerTeam.playersListRoster[index].Personality)
        {
            case 1:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                break;
            case 2:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                break;
            case 3:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                break;

            case 4:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                break;

            case 5:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05");
                break;

            default:
                break;
        }
        image_teamPlayerPersonality.sprite = personalitySprite;
    }
    //Trading
    public void SetTradePanel()
    {
        /*
        if(leagueManager.canTrade == true)
        {
            _tradePanel.SetActive(true);
            SetTheTradingBtns();
            
        }
        */
        _tradePanel.SetActive(true);
        SetTheTradingBtns();
    }
    public void SetTheTradingBtns()
    {
        for (int i = 0; i < _trade_btn_PlayersFronControlledTeam.childCount; i++)
        {
            
            _trade_btn_PlayersFronControlledTeam.GetChild(i).GetComponent<Btn_TradeBtn>().player = gameManager.playerTeam.playersListRoster[i];
            //print(gameManager.playerTeam.playersListRoster[i] + "player on the trade btn" + gameManager.playerTeam.playersListRoster[i].playerLastName);
            int index = gameManager.playerTeam.playersListRoster.IndexOf(gameManager.playerTeam.playersListRoster[i]);
            _trade_btn_PlayersFronControlledTeam.GetChild(i).GetComponent<Button>().onClick.AddListener(() => tradeManager.SetPlayerToTrade(index));

            _trade_btn_PlayersFronControlledTeam.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                gameManager.playerTeam.playersListRoster[i].playerFirstName.ToString();
            _trade_btn_PlayersFronControlledTeam.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text =
                gameManager.playerTeam.playersListRoster[i].ovr.ToString();
            
            
        }
    }
    public void SetTradeGrade()
    {
        //print("To setTradeValue");
        //if(tradeManager.TradeTeam !=null ) tradeManager.CalculateTradeCost(tradeManager.TradeTeam.playersListRoster[tradeManager._playerToReceive]);
        text_teamFrontOfficeGrade.text = tradeManager.tradeCost.ToString();
        text_currentFrontOfficePoints.text = gameManager.playerTeam.FrontOfficePoints.ToString();


    }
    public void SetTradeResultText(string result)
    {
        panel_tradeResult.SetActive(true);
        text_tradeResult.text = result;
       
    }
    public void SetPlayersTradeImages(Player playerTeam, Player tradePlayer)
    {
        //Sprite alteration/update
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");

        Sprite sprite = sprites[playerTeam.ImageCharacterPortrait];
        image_playerTeam.sprite = sprite;

        sprite = sprites[tradePlayer.ImageCharacterPortrait];
        image_playerToTrade.sprite = sprite;
    }
    //New trade functions
    public void TradeSetMyPlayersBtns()
    {
        for (int i = 0; i < transform_tradeMyPlayersBtn.childCount; i++)
        {
            int index = i;   

            // Adiciona o listener usando a variável local (index)
            transform_tradeMyPlayersBtn.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
                TradePlayerToTradeSelected(gameManager.playerTeam.playersListRoster[index]));

            // Preenche os textos (pode continuar usando i normalmente)
            transform_tradeMyPlayersBtn.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                gameManager.playerTeam.playersListRoster[i].playerLastName;

            transform_tradeMyPlayersBtn.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text =
                gameManager.playerTeam.playersListRoster[i].SetOVR().ToString();
        }
        /*
        if (leagueManager.canTrade == false)
        {
            go_tradeFinished.SetActive(true);
        }
        */
        text_trade_frontoffeicePoints.text = gameManager.playerTeam.FrontOfficePoints.ToString();
    }
    void TradePlayerToTradeSelected(Player player)
    {
        
        trade_playerToTrade = player;
        text_trade_myPlayerName.text = player.playerLastName;
        text_trade_MyPlayerOVR.text = player.SetOVR().ToString();
        text_trade_MyPlayerAge.text = player.Age.ToString();
        //Portrait
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[player.ImageCharacterPortrait];
        image_trade_MyPlayerToTradePortrait.sprite = sprite;
        //archtype
        Sprite[] sprites1 = Resources.LoadAll<Sprite>("2D/UI/Archtype");
        Sprite archtypeSprite = null;

        if (player.ImageCharacterPortrait >= 0 || player.ImageCharacterPortrait < 20)
        {
            archtypeSprite = sprites1[0];
        }
        if (player.ImageCharacterPortrait >= 21 && player.ImageCharacterPortrait <= 40)
        {

            archtypeSprite = sprites1[1];
        }
        if (player.ImageCharacterPortrait >= 41 && player.ImageCharacterPortrait <= 60)
        {

            archtypeSprite = sprites1[2];
        }
        if (player.ImageCharacterPortrait >= 61 && player.ImageCharacterPortrait <= 80)
        {
            archtypeSprite = sprites1[3];
        }
        image_trade_MyPlayerArchtype.sprite = archtypeSprite;
        //personality
        Sprite personalitySprite = null;
        switch (player.Personality)
        {
            case 1:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                break;
            case 2:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                break;
            case 3:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                break;

            case 4:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                break;

            case 5:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05");
                break;

            default:
                break;
        }
        image_Trade_myPlayerPersonality.sprite = personalitySprite;
        //team
        Sprite teamImage;
        teamImage = Resources.Load<Sprite>("2D/Team Logos/" + gameManager.playerTeam.TeamName);
        image_trade_MyTeamImage.sprite = teamImage;

        TradeSearchTradeOptions(player);
    }
    void TradeSearchTradeOptions(Player myPlayer)
    {
        print(myPlayer.playerLastName);
        // Limpa opções antigas
        foreach (Transform child in transform_tradeBtnOptions)
        {
            Destroy(child.gameObject);
        }

        if (myPlayer == null)
        {
            Debug.LogError("trade_playerToTrade é null! Não pode prosseguir.");
            return;
        }

        List<Team> otherTeams = gameManager.leagueTeams.Where(t => t != gameManager.playerTeam).ToList();
        if (otherTeams.Count == 0)
        {
            Debug.LogError("Nenhum outro time disponível na liga!");
            return;
        }

        for (int i = 0; i < 7; i++)
        {
            Team targetTeam = otherTeams[UnityEngine.Random.Range(0, otherTeams.Count)];
            Debug.Log("Time selecionado: " + targetTeam.TeamName);

            float higherOvrChance = gameManager.playerTeam.OfficeLvl * 0.2f;
            List<Player> candidates = targetTeam.playersListRoster;
            if (candidates.Count == 0)
            {
                Debug.Log("Roster vazio para " + targetTeam.TeamName + ". Skip.");
                continue;
            }

            List<Player> betterCandidates = new List<Player>();
            int tradeOvr = myPlayer.SetOVR();
            int tradeAge = myPlayer.Age;
            foreach (Player p in candidates)
            {
                if (p.SetOVR() > tradeOvr || p.Age < tradeAge)
                {
                    betterCandidates.Add(p);
                }
            }
            Debug.Log("Candidatos com OVR maior: " + betterCandidates.Count);

            Player targetPlayer;
            if (betterCandidates.Count > 0 && UnityEngine.Random.value < higherOvrChance)
            {
                targetPlayer = betterCandidates[UnityEngine.Random.Range(0, betterCandidates.Count)];
            }
            else
            {
                targetPlayer = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            }
            Debug.Log("Jogador selecionado: " + targetPlayer.playerLastName);
            int cost = TradeCalculateCost(targetPlayer);
            // Instancia botão
            GameObject buttonObj = Instantiate(prefab_TradePlayerOptionToTradeFor, transform_tradeBtnOptions);
            if (buttonObj == null)
            {
                Debug.LogError("Instantiate falhou! Verifique prefab_TradePlayerOptionToTradeFor.");
                continue;
            }
            Debug.Log("Botão instanciado para " + targetPlayer.playerLastName);

            // Configura textos
            buttonObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = targetPlayer.playerLastName;
            buttonObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = targetPlayer.Salary.ToString();
            

            // Listener
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                Player localPlayer = targetPlayer;
                int localTeamIndex = gameManager.leagueTeams.IndexOf(targetTeam);
                buttonObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[localTeamIndex].TeamName;
                button.onClick.AddListener(() => TradePlayerToReceive(localPlayer, localTeamIndex, cost));
            }
            else
            {
                Debug.LogError("Botão sem componente Button!");
            }
        }
        if (transform_tradeBtnOptions.childCount == 0)
        {
            ClearTradeTargetUI();
            Debug.Log("Nenhuma opção de troca encontrada. UI limpa.");
        }
        //Debug.Log(transform_tradeBtnOptions.childCount + " opções criadas");
    }
    
    void TradePlayerToReceive(Player player, int teamIndex, int cost)
    {
        trade_PlayerToReceive = player;
        trade_teamIndex = teamIndex;
        trade_costOfTrade = cost;
        //trade_playerToTrade = player;
        text_trade_receiveName.text = player.playerLastName;
        text_trade_receiveOVR.text = player.SetOVR().ToString();
        text_trade_receiveAge.text = player.Age.ToString();
        //Portrait
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[player.ImageCharacterPortrait];
        image_trade_playerReceivePortrait.sprite = sprite;
        //archtype
        Sprite[] sprites1 = Resources.LoadAll<Sprite>("2D/UI/Archtype");
        Sprite archtypeSprite = null;

        if (player.ImageCharacterPortrait >= 0 || player.ImageCharacterPortrait < 20)
        {
            archtypeSprite = sprites1[0];
        }
        if (player.ImageCharacterPortrait >= 21 && player.ImageCharacterPortrait <= 40)
        {

            archtypeSprite = sprites1[1];
        }
        if (player.ImageCharacterPortrait >= 41 && player.ImageCharacterPortrait <= 60)
        {

            archtypeSprite = sprites1[2];
        }
        if (player.ImageCharacterPortrait >= 61 && player.ImageCharacterPortrait <= 80)
        {
            archtypeSprite = sprites1[3];
        }
        image_trade_receiveArchtype.sprite = archtypeSprite;
        text_tradeCostOfTrade.text = trade_costOfTrade.ToString();
        //personality
        Sprite personalitySprite = null;
        switch (player.Personality)
        {
            case 1:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                break;
            case 2:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                break;
            case 3:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                break;

            case 4:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                break;

            case 5:
                personalitySprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05");
                break;

            default:
                break;
        }
        image_trade_receivePersonality.sprite = personalitySprite;
        //team
        Sprite teamImage;
        teamImage = Resources.Load<Sprite>("2D/Team Logos/" + gameManager.leagueTeams[teamIndex].TeamName);
        image_trade_receiveTeamImage.sprite = teamImage;

        text_tradeWarningForNoTrade.text = " ";


    }
    public void TradeSwapPlayers()
    {
        if(gameManager.playerTeam.FrontOfficePoints - trade_costOfTrade > 0)
        {
            // Remove playerA de teamA
            gameManager.playerTeam.playersListRoster.Remove(trade_playerToTrade);
            // Remove playerB de teamB
            gameManager.leagueTeams[trade_teamIndex].playersListRoster.Remove(trade_PlayerToReceive);
            // Adiciona playerB a teamA
            gameManager.playerTeam.playersListRoster.Add(trade_PlayerToReceive);
            // Adiciona playerA a teamB
            gameManager.leagueTeams[trade_teamIndex].playersListRoster.Add(trade_playerToTrade);
            //cannot tade this week
            //leagueManager.canTrade = false;
            //turn oon panel
            go_tradeFinished.SetActive(true);
            gameManager.playerTeam.FrontOfficePoints -= trade_costOfTrade;
            text_trade_frontoffeicePoints.text = gameManager.playerTeam.FrontOfficePoints.ToString();
            for (int i = 0; i < gameManager.leagueTeams.Count; i++)
            {
                gameManager.saveSystem.SaveTeamInfo(gameManager.leagueTeams[i]);
            }
        }
        else
        {
            text_tradeWarningForNoTrade.text = "Not enought points";
        }
        


    }
    int TradeCalculateCost(Player player)
    {
        int ovr = player.SetOVR();
        int baseCost;
        if (ovr < 70)
        {
            baseCost = 10;
        }
        else if (ovr <= 80)
        {
            baseCost = 20;
        }
        else if (ovr <= 90)
        {
            baseCost = 30;
        }
        else
        {
            baseCost = 40;
        }
        int financeLvl = gameManager.playerTeam.FinancesLvl;  // Assuma que existe essa propriedade (0-7)
        int discount = financeLvl;  // Simples: reduz por lvl (max -7)
        int cost = Mathf.Max(0, baseCost - discount);  // Evita negativo
        return cost;
    }
    public void ClearTradeTargetUI()
    {
        // Limpa informações do jogador alvo
        text_trade_receiveName.text = "";
        text_trade_receiveOVR.text = "";
        text_trade_receiveAge.text = "";
        text_tradeCostOfTrade.text = "";
        text_tradeWarningForNoTrade.text = "No trade options available";

        // Limpa imagens
        if (image_trade_playerReceivePortrait != null) image_trade_playerReceivePortrait.sprite = sprite_transparent;   // ou seu sprite transparente
        if (image_trade_receiveArchtype != null) image_trade_receiveArchtype.sprite = sprite_transparent;
        if (image_trade_receivePersonality != null) image_trade_receivePersonality.sprite = sprite_transparent;
        if (image_trade_receiveTeamImage != null) image_trade_receiveTeamImage.sprite = sprite_transparent;

        // Opcional: desativa o painel de receive se quiser
        // TradeReceivePlayerArea.SetActive(false);
    }
    //Training
    public void SetTrainingBtns()
    {
        for (int i = 0; i < _training_btns.childCount; i++)
        {
            _training_btns.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].playerFirstName.ToString()+ " " +
                gameManager.playerTeam.playersListRoster[i].playerLastName.ToString();
            _training_btns.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].ovr.ToString();
            _training_btns.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].Age.ToString();
            _training_btns.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].ContractYears.ToString();
            int index = gameManager.playerTeam.playersListRoster.IndexOf(gameManager.playerTeam.playersListRoster[i]);
            _training_btns.GetChild(i).GetComponent<Button>().onClick.AddListener(() => trainingManager.SetPlayerToTrainIndex(index/*, _textPlayerSelected, _textDrillSelected*/));
        }
        
    }
    public void UpdateAssistancePortrait(Transform portaritTransform, bool isOn)
    {
        //_training_assistancePortrait.sprite = _training_AssistanceSprite;
        if (isOn)
        {
            portaritTransform.GetChild(0).gameObject.SetActive(true);
        }
        
    }
    public void SetTrainingGrade()
    {
        _text_TrainingGrade.text = gameManager.playerTeam.EffortPoints.ToString();
        
    }
    //Standings
    void PopulateStandings()
    {
        for (int i = 0; i < leagueManager.Standings.Count; i++)
        {
            
            _standingsPlacement.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            _standingsPlacement.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = leagueManager.Standings[i].TeamName.ToString();
            _standingsPlacement.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = leagueManager.Standings[i].Wins.ToString();
            _standingsPlacement.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = leagueManager.Standings[i].Draws.ToString();
            _standingsPlacement.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = leagueManager.Standings[i].Loses.ToString();
            
        }
    }
    //Salary
    public void UpdateTeamSalary()
    {
        gameManager.playerTeam.CurrentSalary = 0;
        for (int i = 0; i < gameManager.playerTeam.playersListRoster.Count; i++)
        {
            gameManager.playerTeam.CurrentSalary += gameManager.playerTeam.playersListRoster[i].Salary;
        }
        //leagueManager.CreateTeamSalary();
        _text_CurrentTeamSalary.text = gameManager.playerTeam.CurrentSalary.ToString();
    }
    //Contracts 
    public void ContractButtonsUpdate()
    {
        for (int i = 0; i < contract_PlayerbuttonsArea.childCount; i++)
        {
            contract_PlayerbuttonsArea.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].playerFirstName + " " +
                gameManager.playerTeam.playersListRoster[i].playerLastName;
            contract_PlayerbuttonsArea.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].ContractYears.ToString();
            contract_PlayerbuttonsArea.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].SetOVR().ToString();
        }
        contract_text_currentFrontOfficePoints.text = gameManager.playerTeam.FansSupportPoints.ToString();
    }
    public void UpdatePlayerContract(int index)
    {

        contract_playerName.text = gameManager.playerTeam.playersListRoster[index].playerFirstName + " " + gameManager.playerTeam.playersListRoster[index].playerLastName;
        contract_CurrentPlayerGames.text =  gameManager.playerTeam.playersListRoster[index].ContractYears.ToString();
        contract_CurrentPlayerSalary.text =  gameManager.playerTeam.playersListRoster[index].Salary.ToString();
        contract_PlayerAge.text = gameManager.playerTeam.playersListRoster[index].Age.ToString();
        contract_playerOvr.text = gameManager.playerTeam.playersListRoster[index].SetOVR().ToString();
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait];
        contract_selectePlayer.sprite = sprite;
        indexForPlayer = index;
        newSalaryValue = gameManager.playerTeam.playersListRoster[index].ContractYears;
        newGamesValue = 2;
        contract_newContractValuesArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = newGamesValue.ToString();
        contract_newContractValuesArea.GetChild(1).GetComponent<TextMeshProUGUI>().text = newSalaryValue.ToString();
        contract_text_contractCostValue.text = ContractValue(gameManager.playerTeam.playersListRoster[index]).ToString();



    }
    public void ContractDiscussion(int weight)
    {
        
        Player p = gameManager.playerTeam.playersListRoster[indexForPlayer];

        // === NOVA VERIFICAÇÃO: Front Office Points suficientes? ===
        int cost = ContractValue(p);

        if (gameManager.playerTeam.FansSupportPoints < cost)
        {
            contract_resultNegotiationText.text = "We don't have enough Front Office Points for this.";
            UpdateAssistancePortrait(transform_contract_AssistancePortrait, false);
            ContractButtonsUpdate();
            contract_asstancePanel.SetActive(true);
            return;
        }

        // Atualiza CurrentSalary antes de qualquer cálculo
        gameManager.playerTeam.CurrentSalary = 0;
        for (int i = 0; i < gameManager.playerTeam.playersListRoster.Count; i++)
        {
            gameManager.playerTeam.CurrentSalary += gameManager.playerTeam.playersListRoster[i].Salary;
        }
        _text_CurrentTeamSalary.text = gameManager.playerTeam.CurrentSalary.ToString();

        // === CÁLCULO DE AUMENTO DE JOGOS BASEADO NO FRONT OFFICE LEVEL ===
        int salaryIncrease = 0;
        int gamesIncrease = 0;

        switch (weight)
        {
            case 0: salaryIncrease = UnityEngine.Random.Range(2, 6); break;
            case 1: salaryIncrease = UnityEngine.Random.Range(6, 9); break;
            case 2: salaryIncrease = UnityEngine.Random.Range(9, 13); break;
        }

        // Quantidade de jogos de extensão depende do FrontOfficeLvl
        if (gameManager.playerTeam.OfficeLvl >= 6)
            gamesIncrease = 6;
        else if (gameManager.playerTeam.OfficeLvl >= 4)
            gamesIncrease = 4;
        else if (gameManager.playerTeam.OfficeLvl >= 1)
            gamesIncrease = 3;
        else
            gamesIncrease = 2;   // nível 0

        if (p.ContractYears + gamesIncrease > 10)
        {
            gamesIncrease = 10 - p.ContractYears;
        }

        // === PROTEÇÃO DE SALARY CAP ===
        int projectedTotalSalary = gameManager.playerTeam.CurrentSalary + salaryIncrease;
        if (projectedTotalSalary > gameManager.playerTeam.SalaryCap)
        {
            contract_resultNegotiationText.text = "We can't extend this contract because it would exceed our Salary Cap.";
            UpdateAssistancePortrait(transform_contract_AssistancePortrait, false);
            ContractButtonsUpdate();
            contract_asstancePanel.SetActive(true);
            return;
        }

        // Continua com a negociação normal
        if (TryExtendContract(gameManager.playerTeam, p, p.Salary + salaryIncrease, p.ContractYears + gamesIncrease, weight))
        {
            p.ContractYears += gamesIncrease;
            p.Salary += salaryIncrease;

            // Debita o custo do Fansupport Points
            gameManager.playerTeam.FansSupportPoints -= ContractValue(p);

            gameManager.playerTeam.CurrentSalary += salaryIncrease;
            _text_CurrentTeamSalary.text = gameManager.playerTeam.CurrentSalary.ToString();

            contract_resultNegotiationText.text = "Good Job Boss! " + p.playerLastName + " for " + p.ContractYears + " games";
            UpdateAssistancePortrait(transform_contract_AssistancePortrait, true);
        }
        else
        {
            contract_resultNegotiationText.text = "Damn! We can't come to an agreement with " + p.playerLastName + ". Maybe he needs some time to think...";
            UpdateAssistancePortrait(transform_contract_AssistancePortrait, false);
        }
        contract_text_currentFrontOfficePoints.text = gameManager.playerTeam.FansSupportPoints.ToString();
        UpdateMoralAndPointsTexts();
        ContractButtonsUpdate();
        contract_asstancePanel.SetActive(true);
        SaveAfterPlayerEvent();
    }
    public bool TryExtendContract(Team team, Player player, int salaryProposed, int gamesProposed,int weight)
    {
        
        if (salaryProposed < player.Salary)
            return false;

        float baseChance = weight switch
        {
            0 => 0.50f,
            1 => 0.70f,
            2 => 0.85f,
            _ => 0.50f
        };

        float refusalChance = player.Personality switch
        {
            1 => 0.00f,
            2 => 0.05f,
            3 => 0.10f,
            4 => 0.30f,
            5 => 0.40f,
            _ => 0.00f
        };

        float financesMultiplier = 1f + (team.FinancesLvl / 7f) * 0.5f;

        float finalChance = baseChance * (1f - refusalChance) * financesMultiplier;

        finalChance = Mathf.Clamp(finalChance, 0.1f, 0.95f);

        return UnityEngine.Random.value < finalChance;
    }
    public int ContractValue(Player player)
    {
        if (player == null) return 0;

        int baseCost;

        // Base cost baseado no OVR
        if (player.SetOVR() < 60)
            baseCost = 20;
        else if (player.SetOVR() <= 74)
            baseCost = 30;
        else if (player.SetOVR() <= 79)
            baseCost = 35;
        else
            baseCost = 50;   // máximo

        // Desconto baseado no FinancesLvl (0 a 7)
        int discount = gameManager.playerTeam.FinancesLvl * 4;   // máximo 28 de desconto no lvl 7

        int finalCost = Mathf.Max(10, baseCost - discount);

        return finalCost;
    }
    
    //FreeAgent
    public IEnumerator ProgressWithWeek()
    {
        yield return null;
        yield return new WaitUntil(()=>canProgressWithWeek);
        //UpdateTeamSalary();
        _freeAgents_panel.SetActive(false);
        gameManager.saveSystem.SaveLeague();
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            gameManager.saveSystem.SaveTeamInfo(gameManager.leagueTeams[i]);
        }
        UpdateTeamSalary();
    }
    public void ValidateProgressWeek()
    {
        canProgressWithWeek = true;
    }

    //INfoButtons
    public void CallWarning()
    {
        bool hasAExpiringContract = false;
        for (int i = 0; i < gameManager.playerTeam.playersListRoster.Count; i++)
        {
            if (gameManager.playerTeam.playersListRoster[i].ContractYears == 1)
            {
                hasAExpiringContract = true;
            }
        }
        if(hasAExpiringContract == true)
        {
            animator_expiringContractBtn.SetTrigger("On");
        }
    }
    public void CheckFreeAgentsForWarning()
    {
        animator_expiringContractBtn.SetTrigger("Off");
        string expiringContractPlayers = "";
        bool hasExpiringContract = false;
        for (int i = 0; i < gameManager.playerTeam.playersListRoster.Count; i++)
        {
            if (gameManager.playerTeam.playersListRoster[i].ContractYears == 1)
            {
                expiringContractPlayers += gameManager.playerTeam.playersListRoster[i].playerFirstName + " " +
                    gameManager.playerTeam.playersListRoster[i].playerLastName + " " + gameManager.playerTeam.playersListRoster[i].ovr.ToString() + " \n";
                hasExpiringContract = true;
            }
        }
        if (hasExpiringContract)
        {
            text_expiringContractsWarning.text = expiringContractPlayers;
        }
        else
        {
            text_expiringContractsWarning.text = "No expiring contracts";
        }
    }
    //PLayer Events
    void SetPlayerEvetPanel()
    {
        text_playerEventDescription.text = playerEventsManager.eventChoosen.Description;
        if (playerEventsManager.eventChoosen.PlayerEventType == PlayerEventsType.Bonds)
        {
            btn_playerEventButton0.onClick.AddListener(() =>playerEventsManager.CreateBond());
            btn_playerEventButton0.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 20;
            btn_playerEventButton0.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bond:" + playerEventsManager.playerChoosen.playerFirstName +" " +
                playerEventsManager.playerChoosen.playerLastName + " " + playerEventsManager.playerChoosen.ovr.ToString() + " && " + playerEventsManager.playerChoosen1.playerFirstName +
                playerEventsManager.playerChoosen1.playerLastName + " " + playerEventsManager.playerChoosen1.ovr;

            btn_playerEventButton1.onClick.AddListener(() => playerEventsManager.PlayersUpgrade());
            btn_playerEventButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerEventsManager.eventChoosen.Choice2;

            //SAVE


        }
        else if(playerEventsManager.eventChoosen.PlayerEventType == PlayerEventsType.Upgrade)
        {
            btn_playerEventButton0.onClick.AddListener(() => playerEventsManager.PlayersUpgrade());
            btn_playerEventButton0.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerEventsManager.eventChoosen.Choice1;
            btn_playerEventButton1.onClick.AddListener(() => playerEventsManager.BuffPlayers());
            btn_playerEventButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerEventsManager.eventChoosen.Choice2;
        }
    }
    //Assiatance panel
    public void SetAssistancePanel()
    {
        string result = " ";

        if (leagueManager.canTrain) result += "<color=#3AB0FF> - Training is avaliable for this week\n</color>";
        //if (leagueManager.canTrade) result += "\n<color=#FFD700> - We can trade this week, boss!</color>\n";
        if (leagueManager.canNegociateContract) result += "\n<color=#90EE90> - We can choose a player to negotiate a contract extension</color>";

        if (result == " ") result = "No tasks remaining for the week";
        text_TaskArea.text = result;
        
    }
    public void ToogleNewsAndAssistancePanel()
    {
        panel_newsPanel.SetActive(!panel_newsPanel.activeSelf);
        assiatncePanel.SetActive(!assiatncePanel.activeSelf);
        if (assiatncePanel.activeInHierarchy)
        {
            SetAssistancePanel();
            text_btn_toggle.text = "<color=#3AB0FF>News</color>";
        }
        else
        {
            text_btn_toggle.text = "<color=#FFD700>Tasks</color>";
        }
    }
    //LeagueHistory
    public void LeagueHistory()
    {
        //LeagueHistoryy
        //Awards
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            for (int j = 0; j < gameManager.leagueTeams[i].playersListRoster.Count; j++)
            {
                //gameManager.leagueTeams[i].playersListRoster[j];
            }
        }
        //TEAMHisotry
    }
    //Menu Animations
    public void ScheduleMenu(bool On)
    {
        animator_SchedulePanel.SetBool("On",On);
    }
    public void TeamPanelAnim(bool On)
    {
        animator_TeamPanel.SetBool("On", On);
    }
    public void TradePanelAnimation(bool On)
    {
        animator_TradingPanel.SetBool("On", On);
    }
    public void TrainingPanelAnim(bool On)
    {
        animator_TrainingPanel.SetBool("On", On);
    }
    public bool IsPlayerTeamInTop8()
    {
        
        // Se você já tem referência ao leagueManager no script, use ela
        if (leagueManager == null)
            leagueManager = FindFirstObjectByType<LeagueManager>();

        if (leagueManager == null || leagueManager.Standings == null || leagueManager.Standings.Count == 0)
            return false;

        int limit = Mathf.Min(8, leagueManager.Standings.Count);

        for (int i = 0; i < limit; i++)
        {
            if (leagueManager.Standings[i] != null && leagueManager.Standings[i].IsPlayerTeam)
                return true;
        }

        return false;
    }
    //update facility panel
    public void FacilityPanelInfoUpdate(string content, string effect, string effect1, string effect2,Sprite sprite, string name)
    {
        _imageAssistanceFacilityImage.sprite = sprite;
        text_facilityEffects.text = effect;
        text_assistanceFacilityDescription.text = content;
        text_facilityEffects1.text = effect1;
        text_facilityEffects2.text = effect2;
        text_facilityName.text = name;
    }
    //Result team panel
    public void ResultPanelCreation()
    {
        if(isGameOver == false)
        {
            
        }
        resultPanel.SetActive(true);
        animator_resultPanel.SetTrigger("Go");
        btn_returnToMainScreen.onClick.AddListener(() => ResetRun());
        btn_returnToMainScreen.onClick.AddListener(() => gameManager.ResetRunTeams());
        btn_returnToMainScreen.onClick.AddListener(() => gameManager.saveSystem.ResetForNewLeagueRun());

        //Team Results
        //find position
        int index = leagueManager.Standings.IndexOf(gameManager.playerTeam);
        text_resultPanel_teamPlacement.text = index.ToString();

        text_resultPanel_teamWins.text = gameManager.playerTeam.Wins.ToString();
        text_resultPanel_teamDraws.text = gameManager.playerTeam.Draws.ToString();
        text_resultPanel_teamDefeats.text = gameManager.playerTeam.Loses.ToString();
        //MVP
        mvp = FindMVP();
        text_resultPanel_mvpName.text = mvp.playerFirstName + " " + mvp.playerLastName;
        text_resultPanel_mvpPtsGame.text = (mvp.CareerPoints / mvp.CareerGamesPlayed).ToString();
        text_resultPanel_mvpStealsGames.text = (mvp.CareerSteals / mvp.CareerGamesPlayed).ToString();
        text_resultPanel_mvpGamesPlayed.text = mvp.CareerGamesPlayed.ToString();
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[mvp.ImageCharacterPortrait];

    }
    //Find MVP
    public Player FindMVP()
    {
        Player mvpPlayer = null;
        float bestScore = float.MinValue;

        foreach (Team team in gameManager.leagueTeams)
        {
            // Calcula OVR médio do time (proxy para número de vitórias/sucesso)
            float teamTotalOVR = 0;
            foreach (Player p in team.playersListRoster)
            {
                teamTotalOVR += p.ovr;
            }
            float teamAverageOVR = team.playersListRoster.Count > 0 ? teamTotalOVR / team.playersListRoster.Count : 0;
            float teamWinsBonus = teamAverageOVR / 10f; // ex: time 90 OVR  +9 pontos de bônus

            foreach (Player player in team.playersListRoster)
            {
                // Pontos por jogo estimados (baseado em atributos ofensivos)
                float pointsPerGame = (player.Shooting + player.Inside + player.Mid + player.Outside) / 4f * 0.4f;

                // Steals por jogo estimados
                float stealsPerGame = player.Stealing * 0.05f;

                // Score total do jogador
                float mvpScore = (pointsPerGame * 1.5f) +                 // peso alto para scoring
                                 (stealsPerGame * 4.0f) +                 // peso alto para steals (defesa impactante)
                                 teamWinsBonus +                          // bônus por time vencedor
                                 (player.ovr * 0.2f);                     // tie-breaker geral

                // Debug opcional pra testar
                // Debug.Log($"{player.playerFirstName} {player.playerLastName} - PPG: {pointsPerGame:F1}, SPG: {stealsPerGame:F1}, Score: {mvpScore:F1}");

                if (mvpScore > bestScore)
                {
                    bestScore = mvpScore;
                    mvpPlayer = player;
                }
            }
        }

        if (mvpPlayer != null)
        {
            Debug.Log($"MVP da temporada: {mvpPlayer.playerFirstName} {mvpPlayer.playerLastName} (Score: {bestScore:F1})");
        }
        else
        {
            Debug.LogWarning("Nenhum jogador encontrado para MVP.");
        }

        return mvpPlayer;
    }
    void SetTextOfFacilities()
    {
        
        // Proteção para eventos
        if (CurrentEventChoiceArea != null)
        {
            CurrentEventChoiceArea.SetActive(leagueManager.canGenerateEvents == false && leagueManager.canStartANewWeek == false);
        }

        // Só atualiza facilities quando realmente estamos na tela de Team Management
        if (gameManager.mode == GameManager.GameMode.TeamManagement &&
            leagueManager.canStartANewWeek == false &&
            leagueManager.isGameOver == false)
        {
            UpdateFacilities();
        }
        UpdateFacilities();
        UpdateMoralAndPointsTexts();
    }
    private void StartNewLeagueRun()
    {
        Debug.Log("Iniciando nova league run...");

        // Reset completo (apaga saves, destrói objetos antigos, recria times, etc.)
        gameManager.saveSystem.FullResetForNewLeagueRun();

        // Volta para a tela inicial
        gameManager.ReturnToTitleScreen();
    }
    //EventPanelAnim
    public Transform GetEventBtns()
    {
        return _transformEventTypeBtns;
    }
    public IEnumerator EventTypePanel()
    {
        print("CALL ANIM");
        yield return new WaitForSeconds(2f);
        _animatorEventsType.SetBool("isOn",true);
    }
    public void RemoveExpiredContracts(Team team)
    {
        if (team == null || team.playersListRoster == null) return;

        for (int i = team.playersListRoster.Count - 1; i >= 0; i--)
        {
            Player p = team.playersListRoster[i];
            if (p.ContractYears <= 0)
            {
                // Destroi o GameObject do jogador (importante para limpar da cena)
                if (p.gameObject != null)
                    Destroy(p.gameObject);

                team.playersListRoster.RemoveAt(i);
            }
        }
    }
    //SaveFunction
    public void SaveAfterPlayerEvent()
    {
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            //gameManager.saveSystem.SaveTeam(gameManager.leagueTeams[i]);
        }
    }
    //free agents
    
    //testing
    public void UpdateHeadquartersUI()
    {
        Debug.Log("[TeamManagerUI] UpdateHeadquartersUI() chamado - atualizando UI completa");

        // Moral, Front Office, Fan Support, Effort
        if (GameObject.Find("MoralePointsText") != null)
            GameObject.Find("MoralePointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.Moral.ToString();

        if (GameObject.Find("FrontOfficePointsText") != null)
            GameObject.Find("FrontOfficePointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.FrontOfficePoints.ToString();

        if (GameObject.Find("FanSupportPointsText") != null)
            GameObject.Find("FanSupportPointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.FansSupportPoints.ToString();

        if (GameObject.Find("EffortPointsText") != null)
            GameObject.Find("EffortPointsText").GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.EffortPoints.ToString();

        // Facilities
        UpdateFacilities();

        // Outros updates importantes após evento
        //UpdateTeamSalary();
        AdvButtonUpdate();
        //CallWarning();
        SetTextOfFacilities();

        Debug.Log("[TeamManagerUI] UpdateHeadquartersUI() concluído");
    }
}
