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

    private void Awake()
    {
        _playerToTradeIndex = -1;
        _playerToReceive = -1;
    }

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
            //bloqueia re-roll se for o mesmo jogador já selecionado ===
            if (_playerToTradeIndex == playerIndex)
            {
                return;
            }
            // ==============================================================

            _playerToTradeIndex = playerIndex;
            FindTeamToTrade();
            FindPlayerForTrade();
            
        }
        else
        {
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

            // Preenche UI só se encontrou jogador válido
            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                TradeTeam.playersListRoster[_playerToReceive].playerFirstName + " " +
                TradeTeam.playersListRoster[_playerToReceive].playerLastName;

            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                TradeTeam.playersListRoster[_playerToReceive].ovr.ToString();

            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
                TradeTeam.TeamName;

            attName0 = "OVR";
            att0 = TradeTeam.playersListRoster[_playerToReceive].ovr;

            attName1 = "Age";
            att1 = TradeTeam.playersListRoster[_playerToReceive].Age;

            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = att0.ToString();
            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = att1.ToString();
            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = attName0;
            _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = attName1;
            
            CalculateTradeCost(TradeTeam.playersListRoster[_playerToReceive]);
            _teamManagerUI.SetTradeGrade();
        }
        else
        {
            // Nenhum jogador encontrado  limpa UI e avisa
            _teamManagerUI.SetTradeResultText("No suitable player available for trade with current Front Office points.");

            // Limpa área de recebimento
            for (int i = 0; i < 7; i++)
            {
                _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "";
            }

            tradeCost = 0;
            _teamManagerUI.SetTradeGrade(); // opcional: reseta grade
        }
        
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
        
        // Proteção extra: se não encontrou jogador válido
        if (_playerToReceive == -1)
        {
            _teamManagerUI.SetTradeResultText("No player selected for trade.");
            return;
        }
        
        //chance de trade gratuita baseada no Front Office Level ===
        int finalCost = tradeCost;
        int frontOfficeLevel = _gameManager.playerTeam.OfficeLvl; // ajuste o nome do campo se for diferente (ex: FrontOfficeLvl, offideLvl, etc.)
        float freeChance = (frontOfficeLevel / 7f) * 0.45f; // 0% (lvl 0) até 45% (lvl 7)

        bool isFreeTrade = UnityEngine.Random.value < freeChance;

        if (isFreeTrade)
        {
            finalCost = 0;
        }
        // ==============================================================

        if (_gameManager.playerTeam.FrontOfficePoints >= finalCost)
        {
            // Executa a troca
            SwapPlayersBetweenTeams(_gameManager.playerTeam.playersListRoster, _playerToTradeIndex, TradeTeam.playersListRoster, _playerToReceive);

            int _currentTeamIndex = _gameManager.leagueTeams.IndexOf(_gameManager.playerTeam);
            _teamManagerUI.TeamRoster();
            _teamManagerUI.SetTheTradingBtns();
            _leagueManager.canTrade = false;

            // Subtrai pontos (0 se for free)
            _gameManager.playerTeam.FrontOfficePoints -= finalCost;
            _teamManagerUI.text_trade_frontoffeicePoints.text = _gameManager.playerTeam.FrontOfficePoints.ToString();

            // Mensagem de sucesso
            if (isFreeTrade)
            {
                _teamManagerUI.SetTradeResultText("Amazing! Front Office pulled strings — this trade was FREE!");
            }
            else
            {
                _teamManagerUI.SetTradeResultText("Good job Boss! A new player arrived.");
            }

            // Limpa área de troca
            for (int i = 0; i < 7; i++)
            {
                _teamManagerUI.TradeReceivePlayerArea.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "";
            }
            _teamManagerUI.UpdateAssistancePortrait(_teamManagerUI.transform_trade_AssistanceResultPortrait, true);
        }
        else
        {
            _teamManagerUI.SetTradeResultText("Not enough Front Office points for this trade.");
            _teamManagerUI.UpdateAssistancePortrait(_teamManagerUI.transform_trade_AssistanceResultPortrait, false);
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
        tradeCost = cost;
        return tradeCost;
    }
}
