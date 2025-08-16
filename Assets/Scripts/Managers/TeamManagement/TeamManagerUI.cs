using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamManagerUI : MonoBehaviour
{
    GameManager gameManager;
    LeagueManager leagueManager;

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
    [SerializeField] public GameObject _trainengCompletePanel;
    [SerializeField] Transform _training_btns;
    [SerializeField] TextMeshProUGUI _textPlayerSelected;
    [SerializeField] TextMeshProUGUI _textDrillSelected;

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

    [Header("UI")]
    [SerializeField]TextMeshProUGUI WeekText;
    [SerializeField] Image image_teamIcon;
    GameObject _advBtn;//to Advance Button Elements


    [SerializeField] Button _closeGameForTestersBtn;
    //Testing
    [SerializeField]Transform teamStatsTextsArea;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        //_scheduleArea = GameObject.Find("ScheduleTeamArea");
        //_schedulePanelTextsArea = GameObject.Find("ScheduleSeasonTexts").transform;
        ScheduleUpdated();
        _advBtn = GameObject.Find("Advance Button");
        //WeekText = GameObject.Find("Week Text").GetComponent<TextMeshProUGUI>();

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
        TeamRoster(_currentTeamIndex);
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
        string newsLine = List_VoxEdgeNewsLines[Random.Range(0, List_VoxEdgeNewsLines.Count)];
        text_newsInfo.text = newsLine;
    }
    void SetTeamIcon()
    {
        Sprite sprite;
        sprite = Resources.Load<Sprite>("2D/Team Logos/" + gameManager.playerTeam.TeamName);
        image_teamIcon.sprite = sprite;
    }
    public void UpdateTeamRoster()
    {
        TeamRoster(_currentTeamIndex);
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
    public void TeamRoster(int index)
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

        //Sprite alteration/update
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait];
        _image_playerPortrait.sprite = sprite;

        //Contract Info
        _text_ContractInfo.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].ContractYears.ToString();
        _text_ContractInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[index].Salary.ToString();

    }
    //Trading
    public void SetTradePanel()
    {
        if(leagueManager.canTrade == true)
        {
            _tradePanel.SetActive(true);
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
    void SetTrainingBtns()
    {
        for (int i = 0; i < _training_btns.childCount; i++)
        {
            _training_btns.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].playerFirstName.ToString();
            _training_btns.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam.playersListRoster[i].ovr.ToString();
            int index = gameManager.playerTeam.playersListRoster.IndexOf(gameManager.playerTeam.playersListRoster[i]);
            _training_btns.GetChild(i).GetComponent<Button>().onClick.AddListener(() => trainingManager.SetPlayerToTrainIndex(index, _textPlayerSelected, _textDrillSelected));
        }
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
}
