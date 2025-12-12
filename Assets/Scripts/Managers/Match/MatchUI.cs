using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MatchUI : MonoBehaviour
{
    GameManager gameManager;
    MatchManager _matchManager;
    LeagueManager leagueManager;
    Transform _debugText;
    [Header("Tutorial")]
    [SerializeField] GameObject panel_tutorialScreen;

    [Header("Starters")]
    [SerializeField] Transform HomeTeamActive_Starters;
    [SerializeField] Transform AwayTeamActive_Starters;
    [SerializeField] Transform _activeHomePlayers;
    [SerializeField] Transform _activeAwayPlayers;

    [Header("BenchPlayers")]
    [SerializeField] Transform _benchPlayers;
    [SerializeField] Transform _timeOutBenchPlayers;

    [Header("Team Names Text")]
    [SerializeField] TextMeshProUGUI _homeTeamName;
    [SerializeField] TextMeshProUGUI _awatTeamName;

    [Header("Scorebord text")]
    [SerializeField] TextMeshProUGUI _text_ScoreBoardHomeTeam;
    [SerializeField] TextMeshProUGUI _text_ScoreBoardAwayTeam;

    [Header("Action Button Area")]
    [SerializeField] GameObject _actionArea;
    [SerializeField] GameObject _actionDefense;

    
    Substitutions _substitutions;
    [Header("Substitutions")]
    [SerializeField] GameObject _panel_SubsPanel;
    [SerializeField] TextMeshProUGUI text_playerStarter;
    [SerializeField] TextMeshProUGUI text_playerBench;
    [SerializeField] TextMeshProUGUI text_CurrentDefensiveStyle;

    [Header("Action Lines")]
    [SerializeField] List<string> list_ReceiveThePos = new List<string>();
    [SerializeField] List<string> list_Passing = new List<string>();
    [SerializeField] List<string> list_Shooting = new List<string>();
    [SerializeField] List <string> list_LosesBall = new List<string>();
    [SerializeField] List<string> list_Preparation = new List<string>();
    public string gameAction = " ";

    [Header("Action Panel")]
    [SerializeField] Image image_actionPanel;
    [SerializeField] TextMeshProUGUI text_actionNameText;
    [SerializeField] Animator _animator_ActionPanel;

    [Header("Special Skill Area")]
    [SerializeField] Transform SpArea;

    [Header("Percentages Area")]
    public GameObject percentagePanel;
    [SerializeField] TextMeshProUGUI text_shootPercentage;
    [SerializeField] TextMeshProUGUI text_passPercentage;

    [Header("Events")]
    [SerializeField] Animator animator_EventPanel;
    [SerializeField] TextMeshProUGUI text_eventPanelEventDescrption;
    [SerializeField] Image image_eventPanelIcon;

    [Header("Casrds")]
    [SerializeField] Animator animator_HandCards;

    [Header("OffensivePanel")]
    [SerializeField] GameObject panel_OffensivePanel;
    [SerializeField] Transform transform_statsArea;
    [SerializeField] Transform transform_gameStatsArea;
    [SerializeField] Transform transform_ActiveHomePlayers;
    [SerializeField] TextMeshProUGUI text_playerWithTheBallName;
    //debub
    public string playernameWithBall;

    [Header("Post game")]
    public GameObject EndScreenStatsPanel;
    [SerializeField] Transform teamANames;
    [SerializeField] Transform teamBNames;
    [SerializeField] Transform teamAScore;
    [SerializeField] Transform teamBScore;
    [SerializeField] Transform teamASteals;
    [SerializeField] Transform teamBSteals;
    public Button btn_ReturnToTeamManagement;
    [SerializeField] Transform GamesResults;
    [SerializeField] TextMeshProUGUI text_Victory_Defeat;
    [SerializeField] GameObject panel_victory_defeat;
    [SerializeField] Button advbtn;

    [Header("Animators")]
    [SerializeField] Animator _homeTeamAnimator;
    [SerializeField] Animator _awayTeamAnimator;
    [SerializeField] Animator _upgradePanel;
    private void Awake()
    {
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        if (leagueManager.CanStartTutorial == false) panel_tutorialScreen.SetActive(false);
        leagueManager.CanStartTutorial = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        _substitutions = GameObject.Find("Subistitution").GetComponent<Substitutions>();
        

        

        btn_ReturnToTeamManagement = GameObject.Find("Advance to Team Management Screen Button").GetComponent<Button>();
        btn_ReturnToTeamManagement.onClick.AddListener(() => gameManager.ReturnToTeamManegement());
        btn_ReturnToTeamManagement.gameObject.SetActive(false);
        EndScreenStatsPanel = GameObject.Find("End Game Stats");
        EndScreenStatsPanel.SetActive(false);
        panel_victory_defeat.SetActive(false);
        

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayersActive();
        UpdateScore();

        if(_matchManager.CanChooseAction)
        {
            _actionArea.SetActive(true);
        }
        else
        {
            _actionArea.SetActive(false);
        }
        if (_matchManager.CanChooseDefenseAction)
        {
            _actionDefense.SetActive(true);
        }
        else
        {
            _actionDefense.SetActive(false);
        }
        //Debug Area
        if (GameObject.Find("DebugTextHome"))
        {
            for (int i = 0; i < GameObject.Find("DebugTextHome").transform.childCount; i++)
            {
                GameObject.Find("DebugTextHome").transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerLastName.ToString();
            }
        }
        //Sub panel
        if(_matchManager.IsOnTimeout == true)
        {
            //print("Here");
            _panel_SubsPanel.SetActive(true);
        }
        if (_matchManager.HomeTeam.hasHDefense)
        {
            text_CurrentDefensiveStyle.text = "Aggressive";
        }
        else
        {
            text_CurrentDefensiveStyle.text = "Normal";
        }
        //Substitution Buttons
        if (GameObject.Find("Starters"))
        {
            for (int i = 0; i < GameObject.Find("Starters").transform.childCount; i++)
            {
                GameObject.Find("Starters").transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString() + " "
                    + _matchManager.HomeTeam.playersListRoster[i].playerLastName + " " + _matchManager.HomeTeam.playersListRoster[i].J_Number.ToString();
                GameObject.Find("Starters").transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "OVR " + _matchManager.HomeTeam.playersListRoster[i].ovr.ToString();
                GameObject.Find("Starters").transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Image>().fillAmount =
                    (float)_matchManager.HomeTeam.playersListRoster[i].CurrentStamina / (float)_matchManager.HomeTeam.playersListRoster[i].MaxStamina;
                //print(_matchManager.HomeTeam.playersListRoster[i].CurrentStamina + "thisis my stamina");
            }
        }
        if (GameObject.Find("Bench"))
        {
            for (int i = 0; i < GameObject.Find("Bench").transform.childCount; i++)
            {

                //print(GameObject.Find("Bench").transform.GetChild(0).GetChild(0).name + " Number od players");
                //GameObject.Find("Bench").transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].playerFirstName.ToString() + " "+
                //_matchManager.HomeTeam.playersListRoster[i + 4].playerLastName.ToString() + " " + _matchManager.HomeTeam.playersListRoster[i + 4].J_Number.ToString();
                //GameObject.Find("Bench").transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "OVR " +_matchManager.HomeTeam.playersListRoster[i + 4].ovr.ToString();
                //GameObject.Find("Bench").transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Image>().fillAmount =
                //(float)_matchManager.HomeTeam.playersListRoster[i + 4].CurrentStamina / (float)_matchManager.HomeTeam.playersListRoster[i+4].MaxStamina;
            }
        }
        for (int i = 0; i < _timeOutBenchPlayers.childCount; i++)
        {
            _timeOutBenchPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].playerLastName;
            _timeOutBenchPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "OVR " + _matchManager.HomeTeam.playersListRoster[i + 4].ovr.ToString();
            _timeOutBenchPlayers.GetChild(i).GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = (float)_matchManager.HomeTeam.playersListRoster[i + 4].CurrentStamina / (float)_matchManager.HomeTeam.playersListRoster[i + 4].MaxStamina;
        }
        for (int i = 0; i < 4; i++)
        {
            transform_ActiveHomePlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerLastName.ToString();
        }
        

    }
    //texts for plays functions
    public string ReceiveBallText()
    {
        gameAction = list_ReceiveThePos[Random.Range(0, list_ReceiveThePos.Count)];
        return gameAction;
    }
    public string ShootingText()
    {
        gameAction = list_Shooting[Random.Range(0, list_Shooting.Count)];
        return gameAction;
    }
    public string LosesPos()
    {
        gameAction = list_LosesBall[Random.Range(0, list_LosesBall.Count)];
        return gameAction;
    }
    
    //Scoreboards
    public void SetTheTeamTextForTheMatch()
    {
        _homeTeamName.text = _matchManager.HomeTeam.TeamName.ToString();
        _awatTeamName.text = _matchManager.AwayTeam.TeamName.ToString();
    }
    public void UpdatePlayersActive()
    {
        for (int i = 0; i < 4; i++)
        {
            _activeHomePlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerLastName.ToString();
            _activeHomePlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].PointsMatch.ToString();
            _activeHomePlayers.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].J_Number.ToString();
            if (_matchManager.HomeTeam.playersListRoster[i].HasTheBall)
            {
                _activeHomePlayers.GetChild(i).GetChild(3).gameObject.SetActive(true);
                //print(_matchManager.HomeTeam.playersListRoster[i] + "Here DUDE");
            }
            else
            {
                _activeHomePlayers.GetChild(i).GetChild(3).gameObject.SetActive(false);
            }
            _activeHomePlayers.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].CurrentStamina.ToString();
            //Activate injury icon
            if(_matchManager.HomeTeam.playersListRoster[i].isInjured) _activeHomePlayers.GetChild(i).GetChild(6).gameObject.SetActive(true);
            else _activeHomePlayers.GetChild(i).GetChild(6).gameObject.SetActive(false);
        }
        for (int i = 0; i < 4; i++)
        {
            _activeAwayPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].playerLastName.ToString();
            _activeAwayPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].PointsMatch.ToString();
            _activeAwayPlayers.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].J_Number.ToString();
            if (_matchManager.AwayTeam.playersListRoster[i].HasTheBall)
            {
                _activeAwayPlayers.GetChild(i).GetChild(3).gameObject.SetActive(true);
                //print(_matchManager.HomeTeam.playersListRoster[i] + "Here DUDE");
            }
            else
            {
                _activeAwayPlayers.GetChild(i).GetChild(3).gameObject.SetActive(false);
            }
            _activeAwayPlayers.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].CurrentStamina.ToString();

        }
        for (int i = 0; i < 4; i++)
        {
            _benchPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i+4].J_Number.ToString();
            _benchPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].PointsMatch.ToString();
            _matchManager.HomeTeam.playersListRoster[i + 4].HasTheBall = false;
        }
        //Offensive Panel
        if (_matchManager.playerWithTheBall != null)
        {
            for (int i = 0; i < 4; i++)
            {
                transform_ActiveHomePlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerLastName.ToString();
            }
        }
        
    }
    void UpdateScore()
    {
        _text_ScoreBoardHomeTeam.text = _matchManager.HomeTeam.Score.ToString();
        _text_ScoreBoardAwayTeam.text = _matchManager.AwayTeam.Score.ToString();
    }
    //Post match stats
    public void PostGameStats(Team A, Team B)
    {
        //print(B.playersListRoster.Count + "Number of players");
        for (int i = 0; i < teamANames.childCount; i++)
        {
            teamANames.GetChild(i).GetComponent<TextMeshProUGUI>().text = A.playersListRoster[i].playerFirstName.ToString() + " " +
                A.playersListRoster[i].playerLastName.ToString();
            teamAScore.GetChild(i).GetComponent<TextMeshProUGUI>().text = A.playersListRoster[i].PointsMatch.ToString();
            teamASteals.GetChild(i).GetComponent<TextMeshProUGUI>().text = A.playersListRoster[i].StealsMatch.ToString();
        }
        for (int i = 0; i < teamBNames.childCount; i++)
        {
            teamBNames.GetChild(i).GetComponent<TextMeshProUGUI>().text = B.playersListRoster[i].playerFirstName.ToString() + " " +
                B.playersListRoster[i].playerLastName.ToString();
            teamBScore.GetChild(i).GetComponent<TextMeshProUGUI>().text = B.playersListRoster[i].PointsMatch.ToString();
            teamBSteals.GetChild(i).GetComponent<TextMeshProUGUI>().text = B.playersListRoster[i].StealsMatch.ToString();
        }
    }
    public void WeekResults(int index, Team A, Team B)
    {
        GamesResults.GetChild(index).GetComponent<TextMeshProUGUI>().text = A.TeamName + " " + A.Score.ToString() + " "+ " X " + B.Score.ToString() + " " + B.TeamName;
    }
    //Animatons calls
    public void MatchStartAnim()
    {
        _homeTeamAnimator.SetTrigger("Go");
        _awayTeamAnimator.SetTrigger("Go");
    }
    public void ChangePos(Team playerTeam)
    {
        if(playerTeam.hasPossession == true)
        {
            _homeTeamAnimator.SetTrigger("ToAttack");
            _awayTeamAnimator.SetTrigger("ToDefense");
        }
        else
        {
            _homeTeamAnimator.SetTrigger("ToDefense");
            _awayTeamAnimator.SetTrigger("ToAttack");
        }
    }
    public void UpgradeTeamAnim()
    {
        //Add sfx
        _upgradePanel.SetTrigger("On");
    }

    //Action panel call
    public void ActionPanelAnim(int index, string actionName)
    {
        _animator_ActionPanel.SetTrigger("On");
        Sprite sprite;
        //Sprite alteration/update
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/MatchActionsPanels");
        //print(sprites.Length + "Number!!!");
        sprite = sprites[index];
        image_actionPanel.sprite = sprite;
        text_actionNameText.text = actionName;
    }
    public void ActivateVictoryDefeat(string endText)
    {
        panel_victory_defeat.SetActive(true);
        text_Victory_Defeat.text = endText;
    }
    public void SetSkillPints()
    {
        //Set SP Points
        for (int i = 0; i < SpArea.childCount; i++)
        {
            if (i < _matchManager._sp_numberOfSPActions) SpArea.GetChild(i).GetChild(0).gameObject.SetActive(true);
            else SpArea.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
    //Update Percentages
    public void SetScoringPercentage(string perc)
    {
        text_shootPercentage.text = perc;
    }
    public void SetPassPercentage(string perc)
    {
        text_passPercentage.text = perc;
    }
    //Events
    public void CallEventPanel(string content, Sprite icon)
    {
        image_eventPanelIcon.sprite = icon;
        text_eventPanelEventDescrption.text = content;
        animator_EventPanel.SetTrigger("On");
    }
    //Cards
    public void UpdateCardsHand()
    {
        animator_HandCards.SetBool("On", _matchManager.canUseCards);
    }
    //OffensivePanel
    public void OffesnivePanelOnOff(bool isOn)
    {
        panel_OffensivePanel.SetActive(isOn);
    }
    public void PlayerWithBallButtonsOnOff()
    {
        print("Check btns");
        print("playerWithTheBall = " + _matchManager.playerWithTheBall.playerLastName);
        for (int i = 0; i < 4; i++)
        {
            if (_matchManager.HomeTeam.playersListRoster[i] == _matchManager.playerWithTheBall)
            {
                transform_ActiveHomePlayers.GetChild(i).GetChild(7).gameObject.SetActive(true);
                transform_ActiveHomePlayers.GetChild(i).GetChild(5).gameObject.SetActive(true);
                transform_ActiveHomePlayers.GetChild(i).GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                transform_ActiveHomePlayers.GetChild(i).GetChild(7).gameObject.SetActive(false);
                transform_ActiveHomePlayers.GetChild(i).GetChild(5).gameObject.SetActive(false);
                transform_ActiveHomePlayers.GetChild(i).GetChild(3).gameObject.SetActive(false);
            }
            print(_matchManager.HomeTeam.playersListRoster[i].playerLastName + " this is his zone: " + _matchManager.HomeTeam.playersListRoster[i].CurrentZone);
        }

    }
    public void UpdatePlayerPlacements()
    {
        print("Check position");
        for (int i = 0; i < 4; i++)
        {
            if (_matchManager.HomeTeam.playersListRoster[i].CurrentZone > 0)
            {
                print("PLay is zoned(UPDATE) " + _matchManager.HomeTeam.playersListRoster[i].playerLastName);
                transform_ActiveHomePlayers.GetChild(i).position = 
                    transform_ActiveHomePlayers.GetChild(i).GetChild(6).GetChild(_matchManager.HomeTeam.playersListRoster[i].CurrentZone).position;
            }
            else
            {
                transform_ActiveHomePlayers.GetChild(i).position =
                   transform_ActiveHomePlayers.GetChild(i).GetChild(6).GetChild(0).position;
            }
        }
    }
}
