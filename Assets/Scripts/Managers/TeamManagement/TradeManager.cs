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
    [SerializeField] TextMeshProUGUI _trade_currentPlayerToTrade;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        

        _gameManager.playerTeam = _playerTeam;
    }

    public void SetPlayerToTrade(int playerIndex)
    {
        _trade_currentPlayerToTrade = GameObject.Find("TPT").GetComponent<TextMeshProUGUI>();
        if (_leagueManager.canTrade == true)
        {
            _playerToTradeIndex = playerIndex;
            //_trade_currentPlayerToTrade.text = _playerTeam.playersListRoster[_playerToTradeIndex].playerLastName.ToString();
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
        _trade_currentPlayerToTrade.GetComponent<TextMeshProUGUI>().text = _gameManager.leagueTeams[playerTeamIndex].playersListRoster[_playerToTradeIndex].playerLastName.ToString();
    }
    void FindPlayerForTrade()
    {
        int att0;
        int att1;
        string attName0;
        string attName1;
        // Normalize and map front office points to OVR range (60 to 99)
        float normalized = (_gameManager.playerTeam.FrontOfficePoints - 20f) / 80f;
        int maxOVR = Mathf.RoundToInt(Mathf.Lerp(60f, 99f, normalized));

        for (int i = 0; i < TradeTeam.playersListRoster.Count; i++)
        {
            if (TradeTeam.playersListRoster[i].ovr <= maxOVR)
            {
                _playerToReceive = TradeTeam.playersListRoster.IndexOf(TradeTeam.playersListRoster[i]);
                print(TradeTeam.playersListRoster[i].playerLastName + TradeTeam.playersListRoster[i].ovr + "This is the player avalible for trade");
            }
        }
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = TradeTeam.playersListRoster[_playerToReceive].playerFirstName.ToString() +
            " " + TradeTeam.playersListRoster[_playerToReceive].playerLastName.ToString();
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = TradeTeam.playersListRoster[_playerToReceive].ovr.ToString();
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = TradeTeam.TeamName.ToString();
        (attName0, att0, attName1, att1) = GetAttributeValuesForStyle(_gameManager.playerTeam._teamStyle, TradeTeam.playersListRoster[_playerToReceive]);
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = att0.ToString();
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = att1.ToString();
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = attName0;
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = attName1;


    }
    public (string attr1Name, int attr1Value, string attr2Name, int attr2Value) GetAttributeValuesForStyle(TeamStyle style, Player player)
    {
        return style switch
        {
            TeamStyle.Normal => ("Consistency", player.Consistency, "Awareness", player.Awareness),
            TeamStyle.Brawler => ("Shooting", player.Shooting, "Awareness", player.Awareness),
            TeamStyle.HyperDribbler => ("Juking", player.Juking, "Control", player.Control),
            TeamStyle.PhaseDash => ("Control", player.Control, "Positioning", player.Positioning),
            TeamStyle.RailShot => ("Shooting", player.Shooting, "Outside", player.Outside),
            _ => ("Consistency", player.Consistency, "Awareness", player.Awareness)
        };
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
        _teamManagerUI.TeamRoster();
        _teamManagerUI.SetTheTradingBtns();
        _leagueManager.canTrade = false;
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
    }
}
