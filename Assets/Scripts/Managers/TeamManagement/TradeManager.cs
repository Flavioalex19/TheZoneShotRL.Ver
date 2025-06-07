using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    GameManager _gameManager;
    LeagueManager _leagueManager;
    Team _playerTeam;
    [SerializeField] TeamManagerUI _teamManagerUI;
    public Team TradeTeam;
    [SerializeField]int _playerToTradeIndex;
    public int _playerToReceive;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();

        _gameManager.playerTeam = _playerTeam;
    }

    public void SetPlayerToTrade(int playerIndex)
    {
        if(_leagueManager.canTrade == true)
        {
            _playerToTradeIndex = playerIndex;
            FindTeamToTrade();
            FindPlayerForTrade();
        }
        else
        {
            print("Out of trades");
        }
        
    }
    void FindTeamToTrade()
    {
        int playerTeamIndex = _gameManager.leagueTeams.IndexOf(_gameManager.playerTeam);
        int teamToTradeIndex = Random.Range(0, _gameManager.leagueTeams.Count);
        while(playerTeamIndex == teamToTradeIndex)
        {
            teamToTradeIndex = Random.Range(0, _gameManager.leagueTeams.Count);
        }
        TradeTeam = _gameManager.leagueTeams[teamToTradeIndex];   
    }
    void FindPlayerForTrade()
    {
        // Normalize and map front office points to OVR range (60 to 99)
        float normalized = (_gameManager.playerTeam.FrontOfficePoints - 20f) / 80f;
        int maxOVR = Mathf.RoundToInt(Mathf.Lerp(60f, 99f, normalized));

        for (int i = 0; i < TradeTeam.playersListRoster.Count; i++)
        {
            if (TradeTeam.playersListRoster[i].ovr <= maxOVR)
            {
                _playerToReceive = TradeTeam.playersListRoster.IndexOf(TradeTeam.playersListRoster[i]);
                print(TradeTeam.playersListRoster[i].playerFirstName + TradeTeam.playersListRoster[i].ovr + "This is the player avalible for trade");
            }
        }
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TradeTeam.playersListRoster[_playerToReceive].playerFirstName.ToString();
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = TradeTeam.playersListRoster[_playerToReceive].ovr.ToString();
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = TradeTeam.TeamName.ToString();

    }
    public void SwapPlayersBetweenTeams(List<Player> TeamA, int playerAIndex, List<Player> TeamB, int playerBIndex)
    {
        if (TeamA == null || TeamB == null)
        {
            Debug.LogError("One or both lists are null.");
            return;
        }

        Player PlayerA = TeamA[playerAIndex];
        Player PlayerB = TeamB[playerBIndex];

        // Swap
        TeamA[playerAIndex] = PlayerB;
        TeamB[playerBIndex] = PlayerA;
    }
    public void Trade()
    {
        SwapPlayersBetweenTeams(_gameManager.playerTeam.playersListRoster, _playerToTradeIndex, TradeTeam.playersListRoster, _playerToReceive);
        int _currentTeamIndex = _gameManager.leagueTeams.IndexOf(_gameManager.playerTeam);
        _teamManagerUI.TeamRoster(_currentTeamIndex);
        _teamManagerUI.SetTheTradingBtns();
        _leagueManager.canTrade = false;
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
    }
}
