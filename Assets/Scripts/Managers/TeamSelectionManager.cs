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
    [Header("Legacy")]
    [SerializeField] Transform transform_btnsLegacy;
    [SerializeField] Button btn_legacy0;
    [SerializeField] Button btn_legacy1;
    [SerializeField] Button btn_legacy2;
    [SerializeField] Button btn_legacy3;
    [SerializeField] Button btn_legacy4;
    [SerializeField] Button btn_legacy5;
    [SerializeField] Button btn_legacy6;
    [SerializeField] public TextMeshProUGUI text_legacyCurrentPoints;
    [SerializeField] public int legacy_currentPoints;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        legacy_currentPoints = 0;
        

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
        /*
        if (_gameManager.playerTeam == null)
        {
            Debug.LogError("playerTeam está null! Verifique se o time foi selecionado corretamente.");
            return;
        }

        // FORMA MAIS SEGURA: Procura o time na lista leagueTeams pelo nome e seta IsPlayerTeam = true
        bool teamFound = false;

        foreach (Team team in _gameManager.leagueTeams)
        {
            if (team.TeamName == _gameManager.playerTeam.TeamName)
            {
                team.IsPlayerTeam = true;
                _gameManager.playerTeam = team;     // Atualiza a referência principal
                teamFound = true;
                Debug.Log($"IsPlayerTeam definido como TRUE para: {team.TeamName}");
                break;
            }
        }

        if (!teamFound)
        {
            Debug.LogError("Não foi possível encontrar o time na lista leagueTeams!");
            return;
        }

        // Resto do código
        selectedTeam.CreateEquips();
        _gameManager.mode = GameManager.GameMode.Draft;
        _gameManager.ScheduleCreation(_gameManager.leagueTeams);

        SceneManager.LoadScene("Draft");
        */
        if (_gameManager.playerTeam == null)
        {
            Debug.LogError("playerTeam está null! Verifique se o time foi selecionado corretamente.");
            return;
        }

        // FORMA SEGURA: Define o time do jogador corretamente
        bool teamFound = false;
        foreach (Team team in _gameManager.leagueTeams)
        {
            if (team.TeamName == _gameManager.playerTeam.TeamName)
            {
                team.IsPlayerTeam = true;
                _gameManager.playerTeam = team;        // Atualiza referência principal
                teamFound = true;
                Debug.Log($"IsPlayerTeam definido como TRUE para: {team.TeamName}");
                break;
            }
        }

        if (!teamFound)
        {
            Debug.LogError("Não foi possível encontrar o time na lista leagueTeams!");
            return;
        }

        // === CRIAÇÃO DO SCHEDULE (com proteção) ===
        if (_gameManager.playerTeam._schedule == null || _gameManager.playerTeam._schedule.Count == 0)
        {
            Debug.Log("Gerando Schedule pela primeira vez...");
            _gameManager.ScheduleCreation(_gameManager.leagueTeams);
        }
        else
        {
            Debug.Log("Schedule já existe. Pulando recriação.");
        }

        selectedTeam.CreateEquips();           // Se ainda for necessário

        _gameManager.mode = GameManager.GameMode.Draft;

        SceneManager.LoadScene("Draft");
    }
    public void ClearCurrentSelectedTeamText()
    {
        _debugCurrentTeam.text = " ";
    }

    public void ResetLegacyBtns()
    {
        legacy_currentPoints = 0;
        for (int i = 0; i < transform_btnsLegacy.childCount; i++)
        {
            transform_btnsLegacy.GetChild(i).GetComponent<BtnTeamSelectionLegacy>().Btn_legacy.interactable = true;
        }
        //reset league bonus for draft or season
        _leagueManager.isOnDraftLVL0 = false;
        _leagueManager.isOnDraftLVL1 = false;
        _leagueManager.isOnDraftLVL2 = false;

    }
    //Debug Panel
    public void DebugUnlock()
    {
        _leagueManager.isOnDraftLVL0 = true;
        _leagueManager.isOnDraftLVL1 = true;
        _leagueManager.isOnDraftLVL2 = true;
    }
    public void SetPlayerTeam(string selectedTeamName)
    {
        if (string.IsNullOrEmpty(selectedTeamName))
        {
            Debug.LogError("Nome do time selecionado está vazio!");
            return;
        }

        bool found = false;

        foreach (Team team in _gameManager.leagueTeams)
        {
            if (team.TeamName == selectedTeamName)
            {
                team.IsPlayerTeam = true;
                _gameManager.playerTeam = team;           // Atualiza a referência principal
                found = true;
                Debug.Log($"Time do jogador definido: {team.TeamName}");
                break;
            }
        }

        if (!found)
        {
            Debug.LogError($"Time com o nome '{selectedTeamName}' não encontrado na lista leagueTeams!");
        }
    }
}
