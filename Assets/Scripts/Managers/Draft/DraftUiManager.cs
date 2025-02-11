using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DraftUiManager : MonoBehaviour
{

    GameManager _gameManager;
    [SerializeField]Transform _currentOnTheClockTeamArea;
    [SerializeField]Transform _playersFromOnTheClockTeamArea;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        _currentOnTheClockTeamArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()].TeamName.ToString();

        //print(_gameManager.leagueTeams[0].playersListRoster.Count + " Team B" + _gameManager.leagueTeams[1].playersListRoster.Count);
        for (int i = 0; i < _gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()].playersListRoster.Count; i++)
        {
            if (_gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()].playersListRoster.Count > 0)
            {
                _playersFromOnTheClockTeamArea.GetChild(i).GetComponent<TextMeshProUGUI>().text = 
                    _gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()].playersListRoster[i].playerFirstName.ToString(); 
            }
        }
    }
}
