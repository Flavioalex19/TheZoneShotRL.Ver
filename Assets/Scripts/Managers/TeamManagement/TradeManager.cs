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
    public int tradeCost = 0;
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
            //print("Out of trades");
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
        
        int bestIndex = -1;
        int bestOVR = -1;

        for (int i = 0; i < TradeTeam.playersListRoster.Count; i++)
        {
            int currentOVR = TradeTeam.playersListRoster[i].ovr;

            // busca o jogador com o maior OVR possível, sem ultrapassar o limite
            if (currentOVR <= maxOVR && currentOVR > bestOVR)
            {
                bestOVR = currentOVR;
                bestIndex = i;
            }
        }

        if (bestIndex != -1)
        {
            _playerToReceive = bestIndex;
            //print(TradeTeam.playersListRoster[_playerToReceive].playerLastName + " " +
                  //TradeTeam.playersListRoster[_playerToReceive].ovr + " - This is the player available for trade");
        }
        else
        {
            //print("No valid player found for trade.");
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

        CalculateTradeCost(TradeTeam.playersListRoster[_playerToReceive]);
        _teamManagerUI.SetTradeGrade();


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

        
        //_gameManager.playerTeam.FrontOfficePoints -= CalculateTradeCost(PlayerB);
    }
    public void Trade()
    {
        if (_gameManager.playerTeam.FrontOfficePoints >= tradeCost && _leagueManager.canTrade == true)
        {
            // allow trade
            SwapPlayersBetweenTeams(_gameManager.playerTeam.playersListRoster, _playerToTradeIndex, TradeTeam.playersListRoster, _playerToReceive);
            int _currentTeamIndex = _gameManager.leagueTeams.IndexOf(_gameManager.playerTeam);
            _teamManagerUI.TeamRoster();
            _teamManagerUI.SetTheTradingBtns();
            _leagueManager.canTrade = false;
            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";

            _teamManagerUI.SetTradeResultText("Good job Boss! A new player arrived");
            _gameManager.playerTeam.FrontOfficePoints-=tradeCost;
        }
        else
        {
            // vtol trade
            _teamManagerUI.SetTradeResultText("No enough points boss! E cannot trade for this player");
        }
        
        
    }
    public int CalculateTradeCost(Player p)
    {
        int cost = 0;

        // Lista dos atributos do jogador
        int[] stats = new int[]
        {
        p.Shooting, p.Inside, p.Mid, p.Outside, p.Awareness,
        p.Defending, p.Guarding, p.Stealing, p.Juking,
        p.Consistency, p.Control, p.Positioning
        };

        // Conta quantos atributos são >= 80
        int highStats = 0;
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] >= 80)
                highStats++;
        }

        // Custo base por atributo acima de 80
        // (Ajuste se quiser mais/menos impacto)
        int costPerStat = 5;

        cost = highStats * costPerStat;

        // Nunca ultrapassar 80
        cost = Mathf.Min(cost, 80);
        //print(cost + " This is the vos of the trade!!!!!!!!!!!!!!!!!!!!");
        tradeCost = cost;
        return tradeCost;
    }
}
