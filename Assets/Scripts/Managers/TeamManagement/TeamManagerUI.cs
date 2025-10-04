using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamManagerUI : MonoBehaviour
{
    GameManager gameManager;
    LeagueManager leagueManager;
    MusicManager musicManager;

    [Header("Intro")]
    [SerializeField] TextMeshProUGUI _text_NameTeam;

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
    [SerializeField] GameObject careerStatsArea;
    [SerializeField] Transform careerStats;
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
    [Header("Training")]
    //Training
    [SerializeField] TrainingManager trainingManager;
    [SerializeField] GameObject _trainingPanel;
    [SerializeField] Transform _training_btns;
    [SerializeField] TextMeshProUGUI _textPlayerSelected;
    [SerializeField] TextMeshProUGUI _textDrillSelected;
    [SerializeField] Image _training_assistancePortrait;
    [SerializeField] Sprite _training_AssistanceSprite;

    [Header("Standings")]
    [SerializeField] GameObject _standingsPanel;
    [SerializeField] Transform _standingsPlacement;

    [Header("SalaryCap")]
    [SerializeField] TextMeshProUGUI _text_CurrentTeamSalary;

    [Header("Options")]
    [SerializeField] GameObject _optionsPanel;
    [SerializeField] Button _optionsQuitBtn;

    [Header("News Info")]
    [SerializeField] List<string> List_VoxEdgeNewsLines = new List<string>();
    [SerializeField] List<string> list_VoxEdgeNewsResults = new List<string>();
    [SerializeField] List<string> list_VoxelEdgePlayersNews = new List<string>();
    [SerializeField] TextMeshProUGUI text_newsInfo;
    [SerializeField]public List<Sprite> sprites_newsSprites = new List<Sprite>();
    [SerializeField] Animator _animator_newsTransition;

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
    int newGamesValue;
    int newSalaryValue;
    int indexForPlayer;
    Player _contractPlayer;

    [Header("GameOverPanel")]
    [SerializeField] Animator gameoverPanel;
    [SerializeField] Button gameover_Btn;

    [Header("FreeAgents")]
    [SerializeField] GameObject _freeAgents_panel;
    [SerializeField] FreeAgentManager freeAgentManager;
    public bool canProgressWithWeek = false;

    [Header("InfoArea")]
    [SerializeField] GameObject panel_infoButtons;
    [SerializeField] TextMeshProUGUI text_expiringContractsWarning;
    [SerializeField] Animator animator_expiringContractBtn;

    [Header("UI")]
    [SerializeField]TextMeshProUGUI WeekText;
    [SerializeField] Image image_teamIcon;
    GameObject _advBtn;//to Advance Button Elements

    [SerializeField] GameObject tutorialPanel;

    [SerializeField] Button _closeGameForTestersBtn;
    //Testing
    [SerializeField]Transform teamStatsTextsArea;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        //_scheduleArea = GameObject.Find("ScheduleTeamArea");
        //_schedulePanelTextsArea = GameObject.Find("ScheduleSeasonTexts").transform;
        ScheduleUpdated();
        _advBtn = GameObject.Find("Advance Button");
        gameover_Btn.onClick.AddListener(() => gameManager.QuitAndClear());//Set Game over button
        //WeekText = GameObject.Find("Week Text").GetComponent<TextMeshProUGUI>();

;       

        
        musicManager.RestoreMutedAudioSources();
        
        //EquipmentUI
        EquipUI();

        #region AdvanceButton
        //AdvanceButton
        AdvButtonUpdate();
        #endregion
        _scheduleArea.SetActive(false);
        

        #region TeamRoster Panel
        //Team Roster panel setup
        _currentTeamIndex = gameManager.leagueTeams.IndexOf(gameManager.playerTeam);
        TeamRoster();
        //careerStatsArea.SetActive(false);
        _teamRoster.SetActive(false);
        #endregion
        //Trading UI Elements
        //Set the btns
        SetTheTradingBtns();
        _tradePanel.SetActive(false);
        //Training
        SetTrainingBtns();
        _trainingPanel.SetActive(false);
        //Schedule
        leagueManager.CreateStandings();
        PopulateStandings();
        _standingsPanel.SetActive(false);
        //Options
        _optionsQuitBtn.onClick.AddListener(() => Application.Quit());
        _optionsPanel.SetActive(false);
        //Contract
        ContractButtonsUpdate();
        contract_asstancePanel.SetActive(false);
        contract_ContractPainel.SetActive(false);
        //News
        NewsUpdate();
        //End tESTING Screen
        _closeGameForTestersBtn.onClick.AddListener(() => gameManager.QuitAndClear());
        _EndBuildScreen.SetActive(false);
        SetTeamIcon();
        if (leagueManager.Week > gameManager.leagueTeams.Count - 1)
        {
            _EndBuildScreen.SetActive(true);
        }

        leagueManager.CreateTeamSalary();
        UpdateTeamSalary();

        if (gameManager.playerTeam.Moral < 0) 
        {
            gameoverPanel.SetTrigger("On");
            gameover_Btn.gameObject.SetActive(true);
        }
        else
        {
            gameover_Btn.gameObject.SetActive(false);
        }


        //FreeAgents if necessary
        _freeAgents_panel.SetActive(false);
        freeAgentManager.RemoveExpiredContracts(gameManager.playerTeam);
        if (gameManager.playerTeam.playersListRoster.Count < 8)
        {
            canProgressWithWeek = false;
            _freeAgents_panel.SetActive(true);
            freeAgentManager.GeneratePlayers(10 - gameManager.playerTeam.playersListRoster.Count);
            StartCoroutine(ProgressWithWeek());

            //_freeAgents_panel.SetActive(false);
            print("Pass");
        }
        //tutorialPanel
        if(leagueManager.canGenerateEvents == false|| leagueManager.Week>1 || leagueManager.CanStartTutorial == false)
        {
            tutorialPanel.SetActive(false);
        }
        //warningBtn
        CallWarning();
        
        StartCoroutine(NewsLoop(10f));
    }

    // Update is called once per frame
    void Update()
    {
        //Update the current Week text
        WeekText.text = leagueManager.Week.ToString();
        
        if(leagueManager.canGenerateEvents == false && leagueManager.canStartANewWeek == false)
        {
            CurrentEventChoiceArea.SetActive(false);
        }
        //testing area
        //TODO- AT THE START CREATE A VARIABLE FOR THE TEAM CONTROLLERD BY THE PLAYER!!!!!
        teamStatsTextsArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.Moral.ToString();
        if (GameObject.Find("Week Text"))
        {
            GameObject.Find("Week Text").GetComponent<TextMeshProUGUI>().text = leagueManager.Week.ToString();
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
        for (int i = 0; i < equipAreaText.childCount; i++)
        {
            equipAreaText.GetChild(i).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam._equipmentList[i].Level.ToString();
        }

        //end build
        if (leagueManager.Week > gameManager.leagueTeams.Count - 1)
        {
            _EndBuildScreen.SetActive(true);
        }
    }
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
        leagueManager.IncreaseWeek();
        leagueManager.CreateStandings();
    }
    //Advance Button Update the elements
    public void AdvButtonUpdate()
    {
        
        
        //Player team
        Sprite sprite = null;
        string teamName;
        teamName = gameManager.playerTeam.TeamName;
        sprite = Resources.Load<Sprite>("2D/Team Logos/" + teamName);
        Image myImageComponent = _advBtn.transform.GetChild(1).GetComponent<Image>();
        myImageComponent.sprite = sprite;
        _currentTeamImage.sprite = sprite;
        //AwayTeam
        Sprite sprite1 = null;
        string awayTeamName;

        int currentWeek = 0;
        if(leagueManager.Week == 0)
        {
            currentWeek = 0;
        }
        else
        {
            currentWeek = leagueManager.Week -1;
        }
        awayTeamName = gameManager.playerTeam._schedule[currentWeek].TeamName;
        print(awayTeamName + "NEXT OPP");
        sprite1 = Resources.Load<Sprite>("2D/Team Logos/" + awayTeamName);
        Image image = _advBtn.transform.GetChild(2).GetComponent<Image>();
        image.sprite = sprite1;
        _awayteamImage.sprite = sprite1;

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
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                break;

            default:
                break;
        }
        _sprite = sprite;
        
        
    }
    //Trading
    public void SetTradePanel()
    {
        if(leagueManager.canTrade == true)
        {
            _tradePanel.SetActive(true);
            SetTheTradingBtns();
        }
    }
    public void SetTheTradingBtns()
    {
        for (int i = 0; i < _trade_btn_PlayersFronControlledTeam.childCount; i++)
        {
            _trade_btn_PlayersFronControlledTeam.GetChild(i).GetComponent<Btn_TradeBtn>().player = gameManager.playerTeam.playersListRoster[i];
            print(gameManager.playerTeam.playersListRoster[i] + "player on the trade btn" + gameManager.playerTeam.playersListRoster[i].playerLastName);
            //_trade_btn_PlayersFronControlledTeam.GetChild(i).GetComponent<Button>().onClick.AddListener(() => tradeManager.SetPlayerToTrade(gameManager.playerTeam.playersListRoster.IndexOf(gameManager.playerTeam.playersListRoster[i])));
            int index = gameManager.playerTeam.playersListRoster.IndexOf(gameManager.playerTeam.playersListRoster[i]);
            _trade_btn_PlayersFronControlledTeam.GetChild(i).GetComponent<Button>().onClick.AddListener(() => tradeManager.SetPlayerToTrade(index));

            _trade_btn_PlayersFronControlledTeam.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                gameManager.playerTeam.playersListRoster[i].playerFirstName.ToString();
            _trade_btn_PlayersFronControlledTeam.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text =
                gameManager.playerTeam.playersListRoster[i].ovr.ToString();
        }
    }
    //Training
    public void SetTrainingBtns()
    {
        for (int i = 0; i < _training_btns.childCount; i++)
        {
            _training_btns.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].playerFirstName.ToString();
            _training_btns.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].ovr.ToString();
            int index = gameManager.playerTeam.playersListRoster.IndexOf(gameManager.playerTeam.playersListRoster[i]);
            _training_btns.GetChild(i).GetComponent<Button>().onClick.AddListener(() => trainingManager.SetPlayerToTrainIndex(index, _textPlayerSelected, _textDrillSelected));
        }
    }
    public void UpdateAssistancePortrait()
    {
        _training_assistancePortrait.sprite = _training_AssistanceSprite;
    }
    //Standings
    void PopulateStandings()
    {
        print(_standingsPlacement.childCount + " Info teams slots");
        for (int i = 0; i < /*_standingsPlacement.childCount*/gameManager.leagueTeams.Count; i++)
        {
            
            _standingsPlacement.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            _standingsPlacement.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = leagueManager.Standings[i].TeamName.ToString();
            _standingsPlacement.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = leagueManager.Standings[i].Wins.ToString();
            _standingsPlacement.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = leagueManager.Standings[i].Draws.ToString();
            _standingsPlacement.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = leagueManager.Standings[i].Loses.ToString();
            
        }
    }
    //Salary
    void UpdateTeamSalary()
    {
        gameManager.playerTeam.CurrentSalary = 0;
        for (int i = 0; i < gameManager.playerTeam.playersListRoster.Count; i++)
        {
            gameManager.playerTeam.CurrentSalary += gameManager.playerTeam.playersListRoster[i].Salary;
        }
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
        }
    }
    public void UpdatePlayerContract(int index)
    {

        contract_playerName.text = gameManager.playerTeam.playersListRoster[index].playerFirstName + " " + gameManager.playerTeam.playersListRoster[index].playerLastName;
        contract_CurrentPlayerGames.text = "Games Remaining " + gameManager.playerTeam.playersListRoster[index].ContractYears.ToString();
        contract_CurrentPlayerSalary.text = "Salary " + gameManager.playerTeam.playersListRoster[index].Salary.ToString();
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait];
        contract_selectePlayer.sprite = sprite;
        indexForPlayer = index;
        newSalaryValue = 2;
        newGamesValue = 2;
        contract_newContractValuesArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = newGamesValue.ToString();
        contract_newContractValuesArea.GetChild(1).GetComponent<TextMeshProUGUI>().text = newSalaryValue.ToString();



    }
    public void ContractDiscussion()
    {
        contract_asstancePanel.SetActive(true);
        if(leagueManager.canNegociateContract == true)
        {
            if (gameManager.playerTeam.playersListRoster[indexForPlayer].ContractYears < 5 ||
            (gameManager.playerTeam.playersListRoster[indexForPlayer].Salary + newSalaryValue) + gameManager.playerTeam.CurrentSalary < gameManager.playerTeam.SalaryCap)
            {
                if (TryExtendContract(gameManager.playerTeam, gameManager.playerTeam.playersListRoster[indexForPlayer], newSalaryValue, newGamesValue))
                {
                    gameManager.playerTeam.playersListRoster[indexForPlayer].ContractYears += newGamesValue;
                    gameManager.playerTeam.playersListRoster[indexForPlayer].Salary = newSalaryValue;
                    contract_resultNegotiationText.text = "Good Job Boss!" + gameManager.playerTeam.playersListRoster[indexForPlayer].playerLastName + " for " + gameManager.playerTeam.playersListRoster[indexForPlayer].ContractYears;
                }
                else
                {
                    contract_resultNegotiationText.text = "Damn! We can't come to an agreement with " + gameManager.playerTeam.playersListRoster[indexForPlayer].playerLastName + ". " +
                        "Maybe he needs some time to think...";
                }
                leagueManager.canNegociateContract = false;
            }
            else
            {
                contract_resultNegotiationText.text = "Boss, we can't extend his contract for now.";
            }
        }
        else
        {
            contract_resultNegotiationText.text = "Boss, we can't negotiate any more contracts this week.";
        }
        
        ContractButtonsUpdate();
        UpdateTeamSalary();

    }
    public bool TryExtendContract(Team team, Player player, int salaryProposed, int gamesProposed)
    {
        // Base demand: higher personality = tougher negotiation
        float baseDemand = player.Salary * (1f + (player.Personality - 1) * 0.1f);

        // Adjust with front office (good management lowers demand)
        baseDemand *= 1f - (team.FrontOfficePoints / 200f);

        // Fan support adds pressure (more fans = higher chance of accept)
        float fanFactor = 1f + (team.FansSupportPoints / 300f);

        // Compare proposed salary vs demand
        float salaryScore = (float)salaryProposed / baseDemand;

        // Game years factor: if offering longer than remaining, good; shorter is worse
        float contractFactor = (gamesProposed >= player.ContractYears) ? 1.1f : 0.9f;

        // Final acceptance chance
        float acceptanceChance = salaryScore * contractFactor * fanFactor;

        // Clamp
        acceptanceChance = Mathf.Clamp01(acceptanceChance);

        return UnityEngine.Random.value < acceptanceChance;
    }
    public void AddOrDecreaseContractGamesGamesValue(bool isAdding)
    {
        if (isAdding)
        {
            if (newGamesValue < 5) newGamesValue++;
            contract_newContractValuesArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = newGamesValue.ToString();
        }
        else
        {
            if (newGamesValue > 0) newGamesValue--;
            contract_newContractValuesArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = newGamesValue.ToString();
        }
    }
    public void AddOrDecreaseSalary(bool isAdding)
    {
        if (isAdding)
        {
            if (newSalaryValue < 12) newSalaryValue++;
            contract_newContractValuesArea.GetChild(1).GetComponent<TextMeshProUGUI>().text = newSalaryValue.ToString();
        }
        else
        {
            if(newSalaryValue>0)newSalaryValue--;
            contract_newContractValuesArea.GetChild(1).GetComponent<TextMeshProUGUI>().text = newSalaryValue.ToString();
        }
    }
    //FreeAgent
    IEnumerator ProgressWithWeek()
    {
        yield return null;
        yield return new WaitUntil(()=>canProgressWithWeek);
        _freeAgents_panel.SetActive(false);
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
                expiringContractPlayers += gameManager.playerTeam.playersListRoster[i].playerFirstName + " " + gameManager.playerTeam.playersListRoster[i].playerLastName + " ";
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
}
