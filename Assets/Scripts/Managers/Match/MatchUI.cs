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
    Transform _debugText;
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

    [Header("Post game")]
    public GameObject EndScreenStatsPanel;
    [SerializeField] Transform teamANames;
    [SerializeField] Transform teamBNames;
    [SerializeField] Transform teamAScore;
    [SerializeField] Transform teamBScore;
    public Button btn_ReturnToTeamManagement;
    [SerializeField] Transform GamesResults;
    [SerializeField] TextMeshProUGUI text_Victory_Defeat;
    [SerializeField] GameObject panel_victory_defeat;
    [SerializeField] Button advbtn;

    [Header("Animators")]
    [SerializeField] Animator _homeTeamAnimator;
    [SerializeField] Animator _awayTeamAnimator;
    [SerializeField] Animator _upgradePanel;
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

        }
        for (int i = 0; i < 4; i++)
        {
            _benchPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i+4].J_Number.ToString();
            _benchPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].PointsMatch.ToString();
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
        }
        for (int i = 0; i < teamBNames.childCount; i++)
        {
            teamBNames.GetChild(i).GetComponent<TextMeshProUGUI>().text = B.playersListRoster[i].playerFirstName.ToString() + " " +
                B.playersListRoster[i].playerLastName.ToString();
            teamBScore.GetChild(i).GetComponent<TextMeshProUGUI>().text = B.playersListRoster[i].PointsMatch.ToString();
        }
    }
    public void WeekResults(int index, Team A, Team B)
    {
        GamesResults.GetChild(index).GetComponent<TextMeshProUGUI>().text = A.TeamName + " " + A.Score.ToString() + " " + B.TeamName + " " + B.Score.ToString();
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
}
