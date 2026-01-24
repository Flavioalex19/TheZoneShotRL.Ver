using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamSelectionManager : MonoBehaviour
{
    GameManager _gameManager;
    LeagueManager _leagueManager;
    public TextMeshProUGUI TeamInfo;
    public TextMeshProUGUI TeamStyle;
    public TextMeshProUGUI TeamCoach;
    public Image CurrentTeamIcon;
    public Transform _teamStatsArea;
    [SerializeField] TextMeshProUGUI _debugCurrentTeam;
    [SerializeField] TextMeshProUGUI _text_selectedTeamOnConfirmation;

    [SerializeField] Team selectedTeam;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
    }

    public void SetPlayerTeam(Team team)
    {
        
        _gameManager.playerTeam = team;
        selectedTeam = _gameManager.playerTeam;
        //team.CreateEquips();
        //_gameManager.mode = GameManager.GameMode.Draft;
        //_gameManager.ScheduleCreation(_gameManager.leagueTeams);
        //SceneManager.LoadScene("Draft");//no transition

        _debugCurrentTeam.text = _gameManager.playerTeam.TeamName;
        _text_selectedTeamOnConfirmation.text = _gameManager.playerTeam.TeamName;
    }
    public void AdvanceToDraft()
    {
        //do a check if the bonus is valid

        selectedTeam = _gameManager.playerTeam;
        selectedTeam.ActivatePlayerTeam();
        selectedTeam.CreateEquips();
        _gameManager.mode = GameManager.GameMode.Draft;
        _gameManager.ScheduleCreation(_gameManager.leagueTeams);
        _gameManager.mode = GameManager.GameMode.Draft;
        SceneManager.LoadScene("Draft");//no transition

    }
    public void ClearCurrentSelectedTeamText()
    {
        _debugCurrentTeam.text = " ";
    }
}
