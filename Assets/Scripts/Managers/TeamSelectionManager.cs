using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamSelectionManager : MonoBehaviour
{
    GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SetPlayerTeam(Team team)
    {
        team.ActivatePlayerTeam();
        _gameManager.playerTeam = team;
        team.CreateEquips();
        _gameManager.mode = GameManager.GameMode.Draft;
        _gameManager.ScheduleCreation(_gameManager.leagueTeams);
        SceneManager.LoadScene("Draft");//no transition
    }
}
