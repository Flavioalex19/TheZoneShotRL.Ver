using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class MatchUI : MonoBehaviour
{
    GameManager gameManager;
    MatchManager _matchManager;
    LeagueManager leagueManager;
    Transform _debugText;
    [Header("Tutorial")]
    [SerializeField] GameObject panel_tutorialScreen;

    [Header("Starters")]
    [SerializeField] Transform HomeTeamActive_Starters;
    [SerializeField] Transform AwayTeamActive_Starters;
    [SerializeField] Transform _activeHomePlayers;
    [SerializeField] Transform _activeAwayPlayers;

    [Header("BenchPlayers")]
    [SerializeField] Transform _benchPlayers;
    [SerializeField] Transform _timeOutBenchPlayers;

    [Header("Team Names Text")]
    [SerializeField] TextMeshProUGUI _homeTeamName;
    [SerializeField] TextMeshProUGUI _awatTeamName;

    [Header("Scorebord")]
    [SerializeField] TextMeshProUGUI _text_ScoreBoardHomeTeam;
    [SerializeField] TextMeshProUGUI _text_ScoreBoardAwayTeam;
    [SerializeField] Image _homeTeamImage;
    [SerializeField] Image _awayTeamImage;

    [Header("Action Button Area")]
    [SerializeField] GameObject _actionArea;
    [SerializeField]public GameObject _actionDefense;
    [SerializeField] Animator animator_defensivePanel;

    
    Substitutions _substitutions;
    [Header("Substitutions")]
    [SerializeField] public GameObject _panel_SubsPanel;
    [SerializeField] TextMeshProUGUI text_playerStarter;
    [SerializeField] TextMeshProUGUI text_playerBench;
    [SerializeField] TextMeshProUGUI text_CurrentDefensiveStyle;

    [Header("Action Lines")]
    [SerializeField] List<string> list_ReceiveThePos = new List<string>();
    [SerializeField] List<string> list_Shooting = new List<string>();
    [SerializeField] List <string> list_LosesBall = new List<string>();
    [SerializeField] List<string> list_successShoot = new List<string>();
    [SerializeField] List<string> list_FailShoot = new List<string>();
    [SerializeField] List<string> list_successJuke = new List<string>();
    [SerializeField] List<string> list_failJuke = new List<string>();
    [SerializeField] List<string> list_successPass = new List<string>();
    [SerializeField] List<string> list_failPass = new List<string>();
    [SerializeField] List<string> list_Opp_FailedAttempt = new List<string>();
    [SerializeField] List<string> list_Opp_SucAttempt = new List<string>();
    [SerializeField] List<string> list_Personality = new List<string>();
    [SerializeField] List<string> list_charge = new List<string>();
    [SerializeField] List<string> list_chargeFail = new List<string>();
    [SerializeField] List<string> list_shoveSucesses = new List<string>();
    [SerializeField] List<string> list_shoveFail = new List<string>();
    [SerializeField] List<string> list_genericFail = new List<string>();
    public string gameAction = " ";
    public string ResultplayerAction = " ";

    [Header("Action Panel")]
    [SerializeField] Image image_actionPanel;
    [SerializeField] TextMeshProUGUI text_actionNameText;
    [SerializeField] Animator _animator_ActionPanel;

    [Header("Special Skill Area")]
    [SerializeField] Transform SpArea;

    [Header("Percentages Area")]
    public GameObject percentagePanel;
    [SerializeField] TextMeshProUGUI text_shootPercentage;
    [SerializeField] TextMeshProUGUI text_passPercentage;
    [SerializeField] TextMeshProUGUI text_offensiveScorePercentage;
    [SerializeField] TextMeshProUGUI text_offensivePassPercentage;
    [SerializeField] TextMeshProUGUI text_offensiveJukePercentage;
    [SerializeField] TextMeshProUGUI text_offensiveSpPercentage;

    [Header("Events")]
    [SerializeField] Animator animator_EventPanel;
    [SerializeField] TextMeshProUGUI text_eventPanelEventDescrption;
    [SerializeField] Image image_eventPanelIcon;


    [Header("Bonus Area Elements")]
    [SerializeField] TextMeshProUGUI text_curretnBonusActive;
    [SerializeField] TextMeshProUGUI text_bonusAtk;
    [SerializeField] TextMeshProUGUI text_bonusDef;
    [SerializeField] TextMeshProUGUI text_bonusJuke;
    [SerializeField] TextMeshProUGUI text_bonusSp;


    [Header("Cards")]
    [SerializeField] Animator animator_HandCards;

    [Header("OffensivePanel")]
    [SerializeField] GameObject env_offensive;
    [SerializeField] GameObject env_defensive;
    [SerializeField] GameObject panel_OffensivePanel;
    [SerializeField] Transform transform_statsArea;
    [SerializeField] Transform transform_gameStatsArea;
    [SerializeField] public Transform transform_ActiveHomePlayers;
    //[SerializeField] TextMeshProUGUI text_playerWithTheBallName;
    [SerializeField] public Transform transform_playersZones;
    [SerializeField] Transform transform_ActiveAwayPlayers;
    //[SerializeField] TextMeshProUGUI text_PlayerwithTheBallJersey;
    [SerializeField] TextMeshProUGUI text_PlayerArchtype;
    [SerializeField] Image image_playerWithTheBallPortrait;
    //[SerializeField] Image image_playerPersonality;
    [SerializeField] TextMeshProUGUI text_DefenderFirstName;
    [SerializeField] TextMeshProUGUI text_DefenderLastName;
    [SerializeField] public TextMeshProUGUI text_midChance;
    [SerializeField] public TextMeshProUGUI text_insChance;
    [SerializeField] public TextMeshProUGUI text_remainingCards;
    [SerializeField] Animator anim_Sp_Button_Activate;
    [SerializeField] Image image_adrenalineBar;
    [SerializeField] Image image_offensivePanel_awayTeamHpBAR;
    [SerializeField] Image image_offensivePanel_awayTeamHpBARSecondary;
    [SerializeField] TextMeshProUGUI text_currentDMG;
    private Coroutine awayHpDrainCoroutine;
    private float currentAwayHpVisual = 100f;
    //Away Team
    [SerializeField] Transform transform_OffensivePanel_awayTeamPlayers;
    //Scoreboard
    [SerializeField] Image image_off_HomeTeam;
    [SerializeField] TextMeshProUGUI text_off_score_homeTeam;
    [SerializeField] Image image_off_AwayTeam;
    [SerializeField] TextMeshProUGUI text_off_score_awayTeam;
    [SerializeField] TextMeshProUGUI text_off_currentPos;
    //team styles
    [SerializeField] Image image_CurrentTeamStyle;

    Animator off_Animator;
    //debub
    [SerializeField] TextMeshProUGUI debugText_awayHp;
    public string playernameWithBall;

    [Header("Post game")]
    public GameObject EndScreenStatsPanel;
    [SerializeField] Transform teamANames;
    [SerializeField] Transform teamBNames;
    [SerializeField] Transform teamAScore;
    [SerializeField] Transform teamBScore;
    [SerializeField] Transform teamASteals;
    [SerializeField] Transform teamBSteals;
    public Button btn_ReturnToTeamManagement;
    [SerializeField] Transform GamesResults;
    [SerializeField] TextMeshProUGUI text_Victory_Defeat;
    [SerializeField] GameObject panel_victory_defeat;
    [SerializeField] public Animator anim_victoryCircle;
    [SerializeField] Animator animator_endgame_Result;
    [SerializeField] TextMeshProUGUI text_victoryDefeatResult;
    [SerializeField] TextMeshProUGUI text_currentStreakVALUE;
    [SerializeField] TextMeshProUGUI text_vicotryDefeatLine;
    [SerializeField] Image image_victoryDefeatResult;
    [SerializeField] Sprite sprite_victory;
    [SerializeField] Sprite sprite_defeat;
    [SerializeField] Button advbtn;

    [Header("Skip Button")]
    [SerializeField] TextMeshProUGUI text_skipBtnText;

    [Header("Animators")]
    [SerializeField] Animator _homeTeamAnimator;
    [SerializeField] Animator _awayTeamAnimator;
    [SerializeField] Animator _upgradePanel;

    [Header("TeamStyle")]
    [SerializeField] TextMeshProUGUI text_teamStyle_StyleName;
    [SerializeField] Image image_teamStyle;

    [Header("ResultActionPanel")]
    [SerializeField] Animator _animatorResultPanel;
    [SerializeField] TextMeshProUGUI text_actionResultS;
    [SerializeField] TextMeshProUGUI text_actionResultF;
    [SerializeField] TextMeshProUGUI text_resultAction_SAction;
    [SerializeField] TextMeshProUGUI text_resultAction_FAction;

    [Header("Defensive State")]
    [SerializeField] Image image_defensive_AwayTeamAdrenalineBar;
    [SerializeField] Image image_defensive_HomeTeamHpBar;

    [Header("Defensive Panel")]
    [SerializeField] TextMeshProUGUI text_defensivePanel_ShootPerc;
    [SerializeField] TextMeshProUGUI text_defensivePanel_PassPerc;
    [SerializeField] TextMeshProUGUI text_defensivePanel_JukePerc;

    [Header("Timeout Panel")]
    [SerializeField] Image image_timeout_style;
    

    [Header("Crowd")]
    [SerializeField] Image material_crowd;

    [Header("Zball")]
    [SerializeField] GameObject go_ZBall;
    [SerializeField] Transform transform_zBallPostionOff;
    //debug 
    [SerializeField] Transform debugTest;
    private void Awake()
    {
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        if (leagueManager.CanStartTutorial == false) panel_tutorialScreen.SetActive(false);
        leagueManager.CanStartTutorial = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        _substitutions = GameObject.Find("Subistitution").GetComponent<Substitutions>();
        

        

        btn_ReturnToTeamManagement = GameObject.Find("Advance to Team Management Screen Button").GetComponent<Button>();
        btn_ReturnToTeamManagement.onClick.AddListener(() => gameManager.ReturnToTeamManegement());
        btn_ReturnToTeamManagement.gameObject.SetActive(false);
        EndScreenStatsPanel = GameObject.Find("End Game Stats");
        EndScreenStatsPanel.SetActive(false);
        panel_victory_defeat.SetActive(false);
        

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayersActive();
        UpdateScore();

        //Update textBuffs
        text_bonusAtk.text = _matchManager.buff_Atk.ToString() + "%";
        text_bonusDef.text = _matchManager.buff_Defense.ToString() + "%";
        text_bonusJuke.text = _matchManager.buff_Juke.ToString() + "%";
        text_bonusSp.text = _matchManager.buff_SP.ToString() + "%";

        if(_matchManager.CanChooseAction)
        {
            _actionArea.SetActive(true);
        }
        else
        {
            _actionArea.SetActive(false);
        }
        HomeTeamHp();
        //Debug Area
        if (GameObject.Find("DebugTextHome"))
        {
            for (int i = 0; i < GameObject.Find("DebugTextHome").transform.childCount; i++)
            {
                GameObject.Find("DebugTextHome").transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerLastName.ToString();
            }
        }
        //Sub panel
        if(_matchManager.IsOnTimeout == true)
        {
            //print("Here");
            //_panel_SubsPanel.SetActive(true);
        }
      

        for (int i = 0; i < debugTest.transform.childCount; i++)
        {
            debugTest.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Image>().fillAmount =
                    (float)_matchManager.HomeTeam.playersListRoster[i].CurrentStamina / (float)_matchManager.HomeTeam.playersListRoster[i].MaxStamina;
        }
        

        for (int i = 0; i < 4; i++)
        {
            //HomeTeams
            transform_ActiveHomePlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerLastName.ToString();
            transform_ActiveHomePlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].PointsMatch.ToString();
            transform_ActiveHomePlayers.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].J_Number.ToString();
            //AwayTeams
            //transform_ActiveAwayPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].SetOVR().ToString();
            //transform_ActiveAwayPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].playerLastName.ToString();
            
                

        }
        image_adrenalineBar.fillAmount = (float)gameManager.playerTeam.AdrenalineBar / (float)gameManager.playerTeam.AdrenalineBarFull;
        //image_offensivePanel_awayTeamHpBAR.fillAmount = (float)_matchManager.awayHP / (float)100;

        

        //UpdateOffensiveStats();
    }
    //texts for plays functions
    public string ReceiveBallText()
    {
        gameAction = list_ReceiveThePos[Random.Range(0, list_ReceiveThePos.Count)];
        return gameAction;
    }
    public string ShootingText()
    {
        gameAction = list_Shooting[Random.Range(0, list_Shooting.Count)];
        return gameAction;
    }
    public string LosesPos()
    {
        gameAction = list_LosesBall[Random.Range(0, list_LosesBall.Count)];
        return gameAction;
    }
    public void PlayerWithTheBallOff()
    {
        //text_playerWithTheBallName.text = _matchManager.playerWithTheBall.playerFirstName + " " + _matchManager.playerWithTheBall.playerLastName;
    }
    //Scoreboards
    public void SetTheTeamTextForTheMatch()
    {
        _homeTeamName.text = _matchManager.HomeTeam.TeamName.ToString();
        _awatTeamName.text = _matchManager.AwayTeam.TeamName.ToString();
    }
    public void UpdatePlayersActive()
    {
        for (int i = 0; i < 4; i++)
        {
            _activeHomePlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerLastName.ToString();
            _activeHomePlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].PointsMatch.ToString();
            _activeHomePlayers.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].J_Number.ToString();
            if (_matchManager.HomeTeam.playersListRoster[i].HasTheBall)
            {
                _activeHomePlayers.GetChild(i).GetChild(3).gameObject.SetActive(true);
                //print(_matchManager.HomeTeam.playersListRoster[i] + "Here DUDE");
            }
            else
            {
                _activeHomePlayers.GetChild(i).GetChild(3).gameObject.SetActive(false);
            }
            _activeHomePlayers.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].CurrentStamina.ToString();
            /*
            //Activate injury icon
            if(_matchManager.HomeTeam.playersListRoster[i].isInjured) _activeHomePlayers.GetChild(i).GetChild(6).gameObject.SetActive(true);
            else _activeHomePlayers.GetChild(i).GetChild(6).gameObject.SetActive(false);
            */
            _activeHomePlayers.GetChild(i).GetChild(7).GetChild(0).GetComponent<Image>().fillAmount =
                (float)_matchManager.HomeTeam.playersListRoster[i].CurrentStamina / _matchManager.HomeTeam.playersListRoster[i].MaxStamina;
        }
        for (int i = 0; i < 4; i++)
        {
            if(_matchManager.HomeTeam== gameManager.playerTeam)
            {
                _activeAwayPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].playerLastName.ToString();
                _activeAwayPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].PointsMatch.ToString();
                _activeAwayPlayers.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].J_Number.ToString();
                if (_matchManager.AwayTeam.playersListRoster[i].HasTheBall)
                {
                    _activeAwayPlayers.GetChild(i).GetChild(3).gameObject.SetActive(true);
                    //print(_matchManager.HomeTeam.playersListRoster[i] + "Here DUDE");
                }
                else
                {
                    _activeAwayPlayers.GetChild(i).GetChild(3).gameObject.SetActive(false);
                }
                _activeAwayPlayers.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].CurrentStamina.ToString();
                _activeAwayPlayers.GetChild(i).GetChild(6).GetChild(0).GetComponent<Image>().fillAmount =
                    (float)_matchManager.AwayTeam.playersListRoster[i].CurrentStamina / _matchManager.AwayTeam.playersListRoster[i].MaxStamina;
            }
            

        }
        for (int i = 0; i < 4; i++)
        {
            _benchPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i+4].J_Number.ToString();
            _benchPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].PointsMatch.ToString();
            _matchManager.HomeTeam.playersListRoster[i + 4].HasTheBall = false;
        }
        //Offensive Panel
        if (_matchManager.playerWithTheBall != null)
        {
            
            for (int i = 0; i < 4; i++)
            {
                transform_ActiveHomePlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerLastName.ToString();
                
            }
        }
        

    }
    public void UpdateOffensiveStats()
    {

        //PersonalityUpdate();
        //Current game stats
        transform_gameStatsArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.PointsMatch.ToString();
        transform_gameStatsArea.GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.StealsMatch.ToString();
        transform_gameStatsArea.GetChild(2).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.CurrentStamina.ToString();
        //player with the ball stats
        transform_statsArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.Shooting.ToString();
        transform_statsArea.GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.Inside.ToString();
        transform_statsArea.GetChild(2).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.Mid.ToString();
        transform_statsArea.GetChild(3).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.Outside.ToString();
        transform_statsArea.GetChild(4).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.Awareness.ToString();
        transform_statsArea.GetChild(5).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.Juking.ToString();
        transform_statsArea.GetChild(6).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.Control.ToString();
        transform_statsArea.GetChild(7).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.Consistency.ToString();
        transform_statsArea.GetChild(8).GetComponent<TextMeshProUGUI>().text = _matchManager.playerWithTheBall.Positioning.ToString();
    }
    void UpdateScore()
    {
        _text_ScoreBoardHomeTeam.text = _matchManager.HomeTeam.Score.ToString();
        _text_ScoreBoardAwayTeam.text = _matchManager.AwayTeam.Score.ToString();
    }
    public void UpdateDefenderInfo(Player defender)
    {
        text_DefenderFirstName.text = defender.playerFirstName.ToString();
        text_DefenderLastName.text = defender.playerLastName.ToString();
    }
    //Post match stats
    public void PostGameStats(Team A, Team B)
    {
        //print(B.playersListRoster.Count + "Number of players");
        for (int i = 0; i < teamANames.childCount; i++)
        {
            teamANames.GetChild(i).GetComponent<TextMeshProUGUI>().text = A.playersListRoster[i].playerFirstName.ToString() + " " +
                A.playersListRoster[i].playerLastName.ToString();
            teamAScore.GetChild(i).GetComponent<TextMeshProUGUI>().text = A.playersListRoster[i].PointsMatch.ToString();
            teamASteals.GetChild(i).GetComponent<TextMeshProUGUI>().text = A.playersListRoster[i].StealsMatch.ToString();
        }
        for (int i = 0; i < teamBNames.childCount; i++)
        {
            teamBNames.GetChild(i).GetComponent<TextMeshProUGUI>().text = B.playersListRoster[i].playerFirstName.ToString() + " " +
                B.playersListRoster[i].playerLastName.ToString();
            teamBScore.GetChild(i).GetComponent<TextMeshProUGUI>().text = B.playersListRoster[i].PointsMatch.ToString();
            teamBSteals.GetChild(i).GetComponent<TextMeshProUGUI>().text = B.playersListRoster[i].StealsMatch.ToString();
        }
    }
    public void WeekResults(int index, Team A, Team B)
    {
        GamesResults.GetChild(index).GetComponent<TextMeshProUGUI>().text = A.TeamName + " " + A.Score.ToString() + " "+ " X " + B.Score.ToString() + " " + B.TeamName;
    }
    //Animatons calls
    public void MatchStartAnim()
    {
        _homeTeamAnimator.SetTrigger("Go");
        _awayTeamAnimator.SetTrigger("Go");
    }
    public void ChangePos(Team playerTeam)
    {
        if(playerTeam.hasPossession == true)
        {
            _homeTeamAnimator.SetTrigger("ToAttack");
            _awayTeamAnimator.SetTrigger("ToDefense");
        }
        else
        {
            _homeTeamAnimator.SetTrigger("ToDefense");
            _awayTeamAnimator.SetTrigger("ToAttack");
        }
    }
    public void UpgradeTeamAnim()
    {
        //Add sfx
        _upgradePanel.SetTrigger("On");
    }

    //Action panel call
    public void ActionPanelAnim(int index, string actionName)
    {
        _animator_ActionPanel.SetTrigger("On");
        Sprite sprite;
        //Sprite alteration/update
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/MatchActionsPanels");
        //print(sprites.Length + "Number!!!");
        sprite = sprites[index];
        image_actionPanel.sprite = sprite;
        text_actionNameText.text = actionName;
    }
    public void ResultActionPanel(string triggerName, int actionIndex, string playerAction)
    {
        _animatorResultPanel.SetTrigger(triggerName);
        switch (actionIndex)
        {
            case 0:
                text_actionResultS.text = list_successShoot[Random.Range(0, list_successShoot.Count)];
                text_actionResultF.text = list_FailShoot[Random.Range(0, list_FailShoot.Count)];
                break; 
            case 1:
                text_actionResultS.text = list_successPass[Random.Range(0, list_successPass.Count)];
                text_actionResultF.text = list_failPass[Random.Range(0, list_failPass.Count)];
                break;
            case 2:
                text_actionResultS.text = list_successJuke[Random.Range(0, list_successJuke.Count)];
                text_actionResultF.text = list_failJuke[Random.Range(0, list_failJuke.Count)];
                break;
            case 3:
                text_actionResultS.text = list_Opp_FailedAttempt[Random.Range(0, list_Opp_FailedAttempt.Count)];
                text_actionResultF.text = list_Opp_SucAttempt[Random.Range(0, list_Opp_SucAttempt.Count)];
                break;
            case 4:
                text_actionResultS.text = list_Personality[Random.Range(0, list_Personality.Count)];
                text_actionResultF.text = list_Personality[Random.Range(0, list_Personality.Count)];
                break;
            case 5:
                text_actionResultS.text = list_shoveSucesses[Random.Range(0, list_shoveSucesses.Count)];
                text_actionResultF.text = list_shoveFail[Random.Range(0, list_shoveFail.Count)];
                break;
            case 6:
                text_actionResultS.text = list_charge[Random.Range(0, list_charge.Count)];
                text_actionResultF.text = list_chargeFail[Random.Range(0, list_chargeFail.Count)];
                break;
            case 7:
                text_actionResultS.text = list_genericFail[Random.Range(0, list_genericFail.Count)];
                text_actionResultF.text = list_genericFail[Random.Range(0, list_genericFail.Count)];
                break;

        }
        text_resultAction_SAction.text = playerAction;
        text_resultAction_FAction.text = playerAction;
    }
    public void ActivateVictoryDefeat(string endText)
    {
        panel_victory_defeat.SetActive(true);
        text_Victory_Defeat.text = endText;
        
    }
    public void SetSkillPints()
    {
        //Set SP Points
        for (int i = 0; i < SpArea.childCount; i++)
        {
            if (i < _matchManager._sp_numberOfSPActions) SpArea.GetChild(i).GetChild(0).gameObject.SetActive(true);
            else SpArea.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
    //Update Percentages
    public void SetScoringPercentage(float value)
    {
        float chance = value;
        int roundNumber = Mathf.RoundToInt(chance * 100f);
        text_shootPercentage.text = roundNumber.ToString() + "%";
        text_offensiveScorePercentage.text = roundNumber.ToString() + "%";
    }
    public void SetPassPercentage(float value)
    {
        float chance = value;
        int roundNumber = Mathf.RoundToInt(chance * 100f);
        text_passPercentage.text = roundNumber.ToString() + "%";
        text_offensivePassPercentage.text = roundNumber.ToString() + "%"; 
    }
    public void SetJukePercentage(float value)
    {
        float chance = value;
        int roundNumber = Mathf.RoundToInt(chance);
        text_offensiveJukePercentage.text = roundNumber.ToString() + "%";
    }
    public void SetSpPercentage(float value)
    {
        float chance = value;
        int roundNumber = Mathf.RoundToInt(chance * 100);
        text_offensiveSpPercentage.text = roundNumber.ToString() + "%";
    }
    public void SetDMGText(int value)
    {
        text_currentDMG.text = value.ToString();
    }
    //Events
    public void CallEventPanel(string content, Sprite icon)
    {
        image_eventPanelIcon.sprite = icon;
        text_eventPanelEventDescrption.text = content;
        animator_EventPanel.SetTrigger("On");
    }
    //Cards
    public void UpdateCardsHand()
    {
        animator_HandCards.SetBool("On", _matchManager.canUseCards);
    }
    //OffensivePanel
    public void OffesnivePanelOnOff(bool isOn)
    {
        //print("Here");
        panel_OffensivePanel.SetActive(isOn);
        UpdateOffensiveStats();
        Sprite sprite = null;
        if (isOn)
        {
            env_offensive.SetActive(true);
            env_defensive.SetActive(false);
            for (int i = 0; i < transform_ActiveHomePlayers.childCount; i++)
            {
                
                //PersonalityUpdate(gameManager.playerTeam.playersListRoster[i], transform_ActiveHomePlayers.GetChild(i).GetChild(8).GetComponent<Image>());
                //Sprite sprite = null; //= Resources.Load<Sprite>("Assets/Resources/2D/Player Personalities/UI_icon_Personalite_01.png");
                switch (_matchManager.HomeTeam.playersListRoster[i].Personality)
                {
                    case 1:
                        sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                        break;
                    case 2:
                        sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                        break;
                    case 3:
                        sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                        break;

                    case 4:
                        sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                        break;

                    case 5:
                        sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05");
                        break;

                    default:
                        break;
                }
                transform_ActiveHomePlayers.GetChild(i).GetChild(8).GetComponent<Image>().sprite = sprite;
                //print(transform_ActiveHomePlayers.GetChild(i).GetChild(8).name);
                
            }
            
        }
        else
        {
            env_offensive.SetActive(false);
            env_defensive.SetActive(true);
        }
    }
    public void CallAnim()
    {
        //UpdateCardsHand();

    }
    public void PlayerWithBallButtonsOnOff()
    {
        //print("Check btns");
        //print("playerWithTheBall = " + _matchManager.playerWithTheBall.playerLastName);
        for (int i = 0; i < 4; i++)
        {
            if (_matchManager.HomeTeam.playersListRoster[i] == _matchManager.playerWithTheBall)
            {
                //off_Animator = transform_ActiveHomePlayers.GetChild(i).GetChild(7).GetComponent<Animator>();
                //print(animator + "New");
                transform_ActiveHomePlayers.GetChild(i).GetChild(7).gameObject.SetActive(true);
                transform_ActiveHomePlayers.GetChild(i).GetChild(5).gameObject.SetActive(true);
                transform_ActiveHomePlayers.GetChild(i).GetChild(3).gameObject.SetActive(true);
                //off_Animator.enabled = true;
                //off_Animator.SetBool("On", true);
                
                
            }
            else
            {
                //Animator animator = transform_ActiveHomePlayers.GetChild(i).GetChild(7).GetComponent<Animator>();
                transform_ActiveHomePlayers.GetChild(i).GetChild(7).gameObject.SetActive(false);
                transform_ActiveHomePlayers.GetChild(i).GetChild(5).gameObject.SetActive(false);
                transform_ActiveHomePlayers.GetChild(i).GetChild(3).gameObject.SetActive(false);
                //animator.SetBool("On", false);
                //animator.Update(0f);  // Força update imediato do state machine
                
            }
            //print(_matchManager.HomeTeam.playersListRoster[i].playerLastName + " this is his zone: " + _matchManager.HomeTeam.playersListRoster[i].CurrentZone);
        }

    }
    
    public void UpdatePlayerPlacements()
    {
        //print("Check position");
        for (int i = 0; i < 4; i++)
        {
            if (_matchManager.HomeTeam.playersListRoster[i].CurrentZone > 0)
            {
                MoveUIObject(transform_ActiveHomePlayers.GetChild(i).transform, 
                    transform_playersZones.GetChild(i).GetChild(0).GetChild(_matchManager.HomeTeam.playersListRoster[i].CurrentZone).transform,1f);
                transform_ActiveHomePlayers.GetChild(i).position =
                    transform_playersZones.GetChild(i).GetChild(0).GetChild(_matchManager.HomeTeam.playersListRoster[i].CurrentZone).position;
            }
            else
            {
                transform_ActiveHomePlayers.GetChild(i).position =
                   /*transform_ActiveHomePlayers.GetChild(i).GetChild(6).GetChild(0).position;*/
                   transform_playersZones.GetChild(i).GetChild(0).GetChild(_matchManager.HomeTeam.playersListRoster[i].CurrentZone).position;
            }
            if(_matchManager.HomeTeam.playersListRoster[i].CurrentZone > 1)
            {
                transform_ActiveHomePlayers.GetChild(i).GetChild(7).GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                transform_ActiveHomePlayers.GetChild(i).GetChild(7).GetChild(3).gameObject.SetActive(true);
            }
        }
    }
    public void TurnOffPlayerButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            transform_ActiveHomePlayers.GetChild(i).GetChild(7).gameObject.SetActive(false);
            transform_ActiveHomePlayers.GetChild(i).GetChild(5).gameObject.SetActive(false);
            transform_ActiveHomePlayers.GetChild(i).GetChild(3).gameObject.SetActive(false);
        }
    }
    public void TeamImagesUpdate()
    {
        Sprite sprite = null;
        string teamName;
        teamName = _matchManager.HomeTeam.TeamName;
        sprite = Resources.Load<Sprite>("2D/Team Logos/" + teamName);
        _homeTeamImage.sprite = sprite;
        Sprite sprite1;
        string teamName1;
        teamName1 = _matchManager.AwayTeam.TeamName;
        sprite1 = Resources.Load<Sprite>("2D/Team Logos/" + teamName1);
        _awayTeamImage.sprite = sprite1;
    }
    void PersonalityUpdate(Player player, Image image)
    {
        //Personality Icons
        Sprite sprite = null; //= Resources.Load<Sprite>("Assets/Resources/2D/Player Personalities/UI_icon_Personalite_01.png");
        switch (/*_matchManager.playerWithTheBall.Personality*/player.Personality)
        {
            case 1:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                break;
            case 2:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                break;
            case 3:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                break;

            case 4:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                break;

            case 5:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05");
                break;

            default:
                break;
        }
        image.sprite = sprite;
        //_sprite = sprite;
        //image_playerPersonality.sprite = sprite;
    }
    public void UseSpBtn()
    {
        anim_Sp_Button_Activate.SetTrigger("Go");
    }
    public void StartResultPanel(string textM, string result)
    {
        //animator_endgame_Result.SetTrigger("Go");
        text_victoryDefeatResult.text = textM;
        if(textM == "Victory")
        {
            text_vicotryDefeatLine.text = "Great Result!!!";
            image_victoryDefeatResult.sprite = sprite_victory;
        }
        else
        {
            text_vicotryDefeatLine.text = "We need to improve!";
            image_victoryDefeatResult.sprite = sprite_defeat;
        }
        if(result == "V") animator_endgame_Result.SetTrigger("Go");
        else if( result == "D") animator_endgame_Result.SetTrigger("Defeat");
        else animator_endgame_Result.SetTrigger("Draw");

    }
    public void UpdateStreakValue(int value)
    {
        text_currentStreakVALUE.text = value.ToString();
    }
    public void TeamStyleUpdate(string teamStyle)
    {
        text_teamStyle_StyleName.text = teamStyle;
        //imagem
        Sprite sprite = null;
        string style;
        style = _matchManager.currentFormation;
        sprite = Resources.Load<Sprite>("2D/Styles/" + style);
        image_teamStyle.sprite = sprite;
        image_timeout_style.sprite = sprite;
        //print(style + " this is the style");
    }
    public void OffensivePanelAwayTeamUpdate(Team awayTeam)
    {
        for (int i = 0; i < 4; i++)
        {
            transform_OffensivePanel_awayTeamPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = awayTeam.playersListRoster[i].playerLastName;
            transform_OffensivePanel_awayTeamPlayers.GetChild(i).GetChild(3).GetComponent<Image>().fillAmount =Mathf.Clamp01((float)awayTeam.playersListRoster[i].CurrentStamina / awayTeam.playersListRoster[i].MaxStamina) ;
        }
    }
    public void UpdateOffensiveScoreBoard()
    {
        text_off_score_homeTeam.text = _matchManager.HomeTeam.Score.ToString();
        text_off_score_awayTeam.text = _matchManager.AwayTeam.Score.ToString();
        text_off_currentPos.text =  _matchManager.currentGamePossessons.ToString();

        Sprite sprite = null;
        string teamName;
        teamName = _matchManager.HomeTeam.TeamName;
        sprite = Resources.Load<Sprite>("2D/Team Logos/" + teamName);
        image_off_HomeTeam.sprite = sprite;
        Sprite sprite1;
        string teamName1;
        teamName1 = _matchManager.AwayTeam.TeamName;
        sprite1 = Resources.Load<Sprite>("2D/Team Logos/" + teamName1);
        image_off_AwayTeam.sprite = sprite1;
    }
    
    //SkipBtn
    public void SkipActionFeedback()
    {
        _matchManager.IsFastforward = !_matchManager.IsFastforward;
        if (_matchManager.IsFastforward)
        {
            text_skipBtnText.text = "On";
        }
        else
        {
            text_skipBtnText.text = "Off";
        }
    }
    //home hp bar
    public void HomeTeamHp()
    {
        image_defensive_HomeTeamHpBar.fillAmount =Mathf.Clamp01((float)_matchManager.HomeTeam.match_hp / _matchManager.HomeTeam.match_hpMax);
    }
    //away team adrenaline bar update
    public void AwayTeamAdrenalineBar()
    {
        image_defensive_AwayTeamAdrenalineBar.fillAmount = Mathf.Clamp01((float)_matchManager.AwayTeam.AdrenalineBar / _matchManager.AwayTeam.AdrenalineBarFull) ;


    }
    //Defensive Panel
    public void ActivateDefensivePanel()
    {
        
    }
    public void OpenDefensivePanel()
    {
        animator_defensivePanel.SetTrigger("Open");
    }
    public void CloseDefensivePanel()
    {
        animator_defensivePanel.SetTrigger("Close");
        StartCoroutine(WaitDisplay());
    }
    public void DefensivePerc(float shootP, float passP, int jukeP)
    {
        text_defensivePanel_ShootPerc.text = Math.Round(shootP*100).ToString() + "%";
        text_defensivePanel_PassPerc.text = Math.Round(passP * 100).ToString() + "%";
        text_defensivePanel_JukePerc.text = jukeP.ToString() + "%";
    }
    //set crowd
    public void SetCrowdColors()
    {
        material_crowd.material.SetColor("_ColorA",_matchManager.HomeTeam.TeamColor);
        material_crowd.material.SetColor("_ColorB", _matchManager.AwayTeam.TeamColor);
    }
    IEnumerator  WaitDisplay()
    {
        yield return new WaitForSeconds(1f);
    }
    public void TimeoutStartsUpdateBtns()
    {
        for (int i = 0; i < debugTest.transform.childCount; i++)
        {
            debugTest.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString() + " "
                + _matchManager.HomeTeam.playersListRoster[i].playerLastName + " " + _matchManager.HomeTeam.playersListRoster[i].J_Number.ToString();
            debugTest.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "OVR " + _matchManager.HomeTeam.playersListRoster[i].ovr.ToString();
            Sprite sprite = null; //= Resources.Load<Sprite>("Assets/Resources/2D/Player Personalities/UI_icon_Personalite_01.png");
            switch (_matchManager.HomeTeam.playersListRoster[i].Personality)
            {
                case 1:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                    break;
                case 2:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                    break;
                case 3:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                    break;

                case 4:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                    break;

                case 5:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05");
                    break;

                default:
                    break;
            }
            debugTest.transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = sprite;
            Sprite[] sprites1 = Resources.LoadAll<Sprite>("2D/UI/Archtype");
            Sprite spriteArch = null;
            //spriteArchtype = sprites1[index];
            leagueManager.FindTraitSprite(spriteArch, _matchManager.HomeTeam.playersListRoster[i]);
            Sprite archetypeSprite = leagueManager.FindTraitSprite(spriteArch,_matchManager.HomeTeam.playersListRoster[i]);
            debugTest.transform.GetChild(i).GetChild(4).GetComponent<Image>().sprite = archetypeSprite;
        }
        for (int i = 0; i < _timeOutBenchPlayers.childCount; i++)
        {
            _timeOutBenchPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].playerLastName + " "
                + _matchManager.HomeTeam.playersListRoster[i + 4].J_Number;
            _timeOutBenchPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].ovr.ToString();
            //_matchManager.HomeTeam.playersListRoster[i + 4].CurrentStamina = _matchManager.HomeTeam.playersListRoster[i + 4].MaxStamina;
            print(_matchManager.HomeTeam.playersListRoster[i + 4].CurrentStamina + " Stasmina pf the bench player");
            _timeOutBenchPlayers.GetChild(i).GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = (float)_matchManager.HomeTeam.playersListRoster[i + 4].CurrentStamina / (float)_matchManager.HomeTeam.playersListRoster[i + 4].MaxStamina;
            Sprite sprite = null; //= Resources.Load<Sprite>("Assets/Resources/2D/Player Personalities/UI_icon_Personalite_01.png");
            switch (_matchManager.HomeTeam.playersListRoster[i + 4].Personality)
            {
                case 1:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                    break;
                case 2:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                    break;
                case 3:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                    break;

                case 4:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                    break;

                case 5:
                    sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05");
                    break;

                default:
                    break;
            }
            _timeOutBenchPlayers.GetChild(i).GetChild(3).GetComponent<Image>().sprite = sprite;
            Sprite[] sprites1 = Resources.LoadAll<Sprite>("2D/UI/Archtype");
            Sprite spriteArch = null;
            Sprite archetypeSprite = leagueManager.FindTraitSprite(spriteArch, _matchManager.HomeTeam.playersListRoster[i + 4]);
            _timeOutBenchPlayers.GetChild(i).GetChild(4).GetComponent<Image>().sprite = archetypeSprite;

        }
    }
    public void MoveUIObject(Transform objectToMove, Transform targetPosition, float duration = 0.5f)
    {
        if (objectToMove == null || targetPosition == null)
        {
            Debug.LogWarning("MoveUIObject: Um dos Transforms está nulo!");
            return;
        }

        StartCoroutine(MoveCoroutine(objectToMove, targetPosition.position, duration));
    }

    private IEnumerator MoveCoroutine(Transform obj, Vector3 targetWorldPosition, float duration)
    {
        float elapsed = 0f;
        Vector3 startPosition = obj.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // ====================== SMOOTHSTEP (Ease In Out) ======================
            // Essa é a função da imagem (Smoothstep)
            float smoothT = t * t * (3f - 2f * t);
            // =====================================================================

            obj.position = Vector3.Lerp(startPosition, targetWorldPosition, smoothT);
            yield return null;
        }

        // Garante que chegue exatamente no destino
        obj.position = targetWorldPosition;
    }
    //away Hpbar
    public void UpdateAwayHpBar(float newHpValue)
    {
        // Barra principal - atualiza imediatamente
        image_offensivePanel_awayTeamHpBAR.fillAmount = newHpValue / _matchManager.AwayTeam.match_hpMax;

        // Barra secundária - diminui gradualmente
        if (awayHpDrainCoroutine != null)
            StopCoroutine(awayHpDrainCoroutine);

        awayHpDrainCoroutine = StartCoroutine(DrainAwayHpBar(newHpValue));
    }

    private IEnumerator DrainAwayHpBar(float targetHp)
    {
        float duration = 1.5f; // tempo para drenar (ajuste se quiser)
        float elapsed = 0f;
        float startVisual = currentAwayHpVisual;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Suavização (smoothstep)
            float smoothT = t * t * (3f - 2f * t);

            currentAwayHpVisual = Mathf.Lerp(startVisual, targetHp, smoothT);

            // Atualiza a barra secundária
            image_offensivePanel_awayTeamHpBARSecondary.fillAmount = currentAwayHpVisual / 100f;

            yield return null;
        }

        // Garante que chegue exatamente no valor final
        currentAwayHpVisual = targetHp;
        image_offensivePanel_awayTeamHpBARSecondary.fillAmount = targetHp / 100f;
    }
    //mpve zBall
    public void MoveZball( int targetIndex, Player parentTransform,  float duration = 0.5f)
    {
        Transform startPos = null;
        Transform endPos = transform_ActiveHomePlayers.GetChild(targetIndex);
        for (int i = 0; i < _matchManager.HomeTeam.playersListRoster.Count; i++)
        {
            if (_matchManager.HomeTeam.playersListRoster[i] == parentTransform)
            {
                startPos = transform_ActiveHomePlayers.transform.GetChild(i);
            }
        }

        go_ZBall.transform.position = startPos.position;
        if (go_ZBall == null || endPos == null)
        {
            Debug.LogWarning("MoveUIObject: Um dos Transforms está nulo!");
            return;
        }

        StartCoroutine(MoveCoroutine(go_ZBall.transform, endPos.position, duration));
        go_ZBall.transform.position = transform_zBallPostionOff.position;
        
    }
}
