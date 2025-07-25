using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("Team Names Text")]
    [SerializeField] TextMeshProUGUI _homeTeamName;
    [SerializeField] TextMeshProUGUI _awatTeamName;

    [Header("Scorebord text")]
    [SerializeField] TextMeshProUGUI _text_ScoreBoardHomeTeam;
    [SerializeField] TextMeshProUGUI _text_ScoreBoardAwayTeam;

    [Header("Action Button Area")]
    [SerializeField] GameObject _actionArea;

    [Header("Post game")]
    public GameObject EndScreenStatsPanel;
    [SerializeField] Transform teamANames;
    [SerializeField] Transform teamBNames;
    [SerializeField] Transform teamAScore;
    [SerializeField] Transform teamBScore;
    Button btn_ReturnToTeamManagement;
    [SerializeField] Transform GamesResults;

    [Header("Animators")]
    [SerializeField] Animator _homeTeamAnimator;
    [SerializeField] Animator _awayTeamAnimator;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();

        btn_ReturnToTeamManagement = GameObject.Find("Advance to Team Management Screen Button").GetComponent<Button>();
        btn_ReturnToTeamManagement.onClick.AddListener(() => gameManager.ReturnToTeamManegement());
        EndScreenStatsPanel = GameObject.Find("End Game Stats");
        EndScreenStatsPanel.SetActive(false);

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
                GameObject.Find("DebugTextHome").transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString();
            }
        }
        //Substitution Buttons
        if (GameObject.Find("Starters"))
        {
            for (int i = 0; i < GameObject.Find("Starters").transform.childCount; i++)
            {
                GameObject.Find("Starters").transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString();
            }
        }
        if (GameObject.Find("Bench"))
        {
            for (int i = 0; i < GameObject.Find("Bench").transform.childCount; i++)
            {
                GameObject.Find("Bench").transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].playerFirstName.ToString();
            }
        }
    }
    public void SetTheTeamTextForTheMatch()
    {
        _homeTeamName.text = _matchManager.HomeTeam.TeamName.ToString();
        _awatTeamName.text = _matchManager.AwayTeam.TeamName.ToString();
    }
    void UpdatePlayersActive()
    {
        for (int i = 0; i < 4; i++)
        {
            _activeHomePlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString();
            _activeHomePlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].PointsMatch.ToString();
        }
        for (int i = 0; i < 4; i++)
        {
            _activeAwayPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].playerFirstName.ToString();
            _activeAwayPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].PointsMatch.ToString();
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
            teamANames.GetChild(i).GetComponent<TextMeshProUGUI>().text = A.playersListRoster[i].playerFirstName.ToString() + 
                A.playersListRoster[i].playerLastName.ToString();
            teamAScore.GetChild(i).GetComponent<TextMeshProUGUI>().text = A.playersListRoster[i].PointsMatch.ToString();
        }
        for (int i = 0; i < teamBNames.childCount; i++)
        {
            teamBNames.GetChild(i).GetComponent<TextMeshProUGUI>().text = B.playersListRoster[i].playerFirstName.ToString() + 
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
}
