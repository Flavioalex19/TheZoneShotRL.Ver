using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamSelectionManager : MonoBehaviour
{
    GameManager _gameManager;
    public TextMeshProUGUI TeamInfo;
    public TextMeshProUGUI TeamStyle;
    public TextMeshProUGUI TeamCoach;
    public Image CurrentTeamIcon;
    [SerializeField] TextMeshProUGUI _debugCurrentTeam;

    [SerializeField] Team selectedTeam;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
    }
    public void AdvanceToDraft()
    {
        selectedTeam = _gameManager.playerTeam;
        selectedTeam.ActivatePlayerTeam();
        selectedTeam.CreateEquips();
        _gameManager.mode = GameManager.GameMode.Draft;
        _gameManager.ScheduleCreation(_gameManager.leagueTeams);
        SceneManager.LoadScene("Draft");//no transition

    }
}
