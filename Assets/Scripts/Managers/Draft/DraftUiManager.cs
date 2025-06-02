using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraftUiManager : MonoBehaviour
{

    GameManager _gameManager;
    [SerializeField]Transform _currentOnTheClockTeamArea;
    [SerializeField]Transform _playersFromOnTheClockTeamArea;

    [SerializeField] Transform _playerBtnsAreaContent;
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

        if (_gameManager.playerTeam != _gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()])
        {
            Transform selectedBtn = GetRandomChild();
            InvokeButtonClick(selectedBtn.GetComponent<Button>());
        }
    }
    public Transform GetRandomChild()
    {
        if (_playerBtnsAreaContent.childCount == 0)
        {
            Debug.LogWarning("No children found on this Transform.");
            return null;
        }

        int randomIndex = Random.Range(0, _playerBtnsAreaContent.childCount);
        return _playerBtnsAreaContent.GetChild(randomIndex);
    }
    public void InvokeButtonClick(Button button)
    {
        if (button != null)
        {
            button.onClick.Invoke();
            Debug.Log("Button click invoked through code.");
        }
    }
}
