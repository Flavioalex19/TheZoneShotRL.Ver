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

    [SerializeField] GameObject _EndBuildScreen;
    //Team Roster panel
    [SerializeField]GameObject _teamRoster;
    [SerializeField]Transform _teamRosterStartersPlayersText;
    [SerializeField] Transform _teamRosterBenchPlayerText;
    [SerializeField]int _currentTeamIndex;
    [SerializeField] TextMeshProUGUI _currentTeamNameTeamRosterText;

    GameObject _advBtn;//to Advance Button Elements
    TextMeshProUGUI WeekText;


    [SerializeField] Button _closeGameForTestersBtn;
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
        #region AdvanceButton
        //AdvanceButton
        AdvButtonUpdate();
        #endregion
        _scheduleArea.SetActive(false);
        //Team Roster panel setup
        _currentTeamIndex = gameManager.leagueTeams.IndexOf(gameManager.playerTeam);
        TeamRoster(_currentTeamIndex);
        _teamRoster.SetActive(false);

        //End tESTING Screen
        _closeGameForTestersBtn.onClick.AddListener(() => gameManager.QuitAndClear());
        _EndBuildScreen.SetActive(false);

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

    }
    void TeamRoster(int index)
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
}
