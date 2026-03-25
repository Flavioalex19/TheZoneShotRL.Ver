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
        //do a check if the bonus is valid

        selectedTeam = _gameManager.playerTeam;
        selectedTeam.ActivatePlayerTeam();
        selectedTeam.CreateEquips();
        _gameManager.mode = GameManager.GameMode.Draft;
        _gameManager.ScheduleCreation(_gameManager.leagueTeams);
        _gameManager.mode = GameManager.GameMode.Draft;
        SceneManager.LoadScene("Draft");//no transition

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
}
