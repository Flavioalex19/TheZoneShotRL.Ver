using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    GameManager _gameManager;
    LeagueManager _leagueManager;
    Team _playerTeam;
    Team TradeTeam;
    [SerializeField]Player _playerToTrade;
    [SerializeField]Player _playerToReceive;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();

        _gameManager.playerTeam = _playerTeam;
    }

    public void SetPlayerToTrade(Player player)
    {
        _playerToTrade = player;
    }
}
