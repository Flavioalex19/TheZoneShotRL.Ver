using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamManagerUI : MonoBehaviour
{
    GameManager gameManager;
    LeagueManager leagueManager;

    GameObject _scheduleArea;
    Transform _schedulePanelTextsArea;
    [SerializeField] TradeManager tradeManager;

    [SerializeField] GameObject _EndBuildScreen;
    //Team Roster panel
    [SerializeField]GameObject _teamRoster;
    [SerializeField]Transform _teamRosterStartersPlayersText;
    [SerializeField] Transform _teamRosterBenchPlayerText;
    [SerializeField]int _currentTeamIndex;
    [SerializeField] TextMeshProUGUI _currentTeamNameTeamRosterText;
    [SerializeField] Image _currentTeamImage;
    [SerializeField] Image _awayteamImage;

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

    GameObject _advBtn;//to Advance Button Elements
    TextMeshProUGUI WeekText;


    [SerializeField] Button _closeGameForTestersBtn;
    //Testing
    Transform teamStatsTextsArea;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        _scheduleArea = GameObject.Find("ScheduleTeamArea");
        _schedulePanelTextsArea = GameObject.Find("ScheduleSeasonTexts").transform;
        ScheduleUpdated();
        _advBtn = GameObject.Find("Advance Button");
        WeekText = GameObject.Find("Week Text").GetComponent<TextMeshProUGUI>();

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
        //End tESTING Screen
        _closeGameForTestersBtn.onClick.AddListener(() => gameManager.QuitAndClear());
        _EndBuildScreen.SetActive(false);


        //Testing
        teamStatsTextsArea = GameObject.Find("TeamPoints").transform;
        

    }

    // Update is called once per frame
    void Update()
    {
        //Update the current Week text
        WeekText.text = leagueManager.Week.ToString();
        if(leagueManager.Week> gameManager.leagueTeams.Count - 1)
        {
            _EndBuildScreen.SetActive(true);
        }
        if(leagueManager.canGenerateEvents == false)
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
    }

    //Schedule Updated
    void ScheduleUpdated()
    {
        if (_schedulePanelTextsArea != null)
        {
            for (int i = 0; i < gameManager.playerTeam._schedule.Count; i++)
            {
                _schedulePanelTextsArea.GetChild(i).GetComponent<TextMeshProUGUI>().text = gameManager.playerTeam._schedule[i].TeamName.ToString();
            }
        }
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

    //Advance Button Update the elements
    void AdvButtonUpdate()
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
    public void TeamRoster(int index)
    {
        _currentTeamNameTeamRosterText.text = gameManager.leagueTeams[_currentTeamIndex].TeamName.ToString();

        for (int i = 0; i < 4; i++)
        {
            _teamRosterStartersPlayersText.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i].playerFirstName.ToString();
            _teamRosterStartersPlayersText.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i].ovr.ToString();
        }
        for (int i = 0; i < 4; i++)
        {
            _teamRosterBenchPlayerText.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i + 4].playerFirstName.ToString();
            _teamRosterBenchPlayerText.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManager.leagueTeams[_currentTeamIndex].playersListRoster[i + 4].ovr.ToString();
        }
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
            print(gameManager.playerTeam.playersListRoster[i] + "player on the trade btn" + gameManager.playerTeam.playersListRoster[i].playerFirstName);
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
}
