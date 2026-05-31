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
    [SerializeField] GameObject _actionDefense;

    
    Substitutions _substitutions;
    [Header("Substitutions")]
    [SerializeField] GameObject _panel_SubsPanel;
    [SerializeField] TextMeshProUGUI text_playerStarter;
    [SerializeField] TextMeshProUGUI text_playerBench;
    [SerializeField] TextMeshProUGUI text_CurrentDefensiveStyle;

    [Header("Action Lines")]
    [SerializeField] List<string> list_ReceiveThePos = new List<string>();
    [SerializeField] List<string> list_Passing = new List<string>();
    [SerializeField] List<string> list_Shooting = new List<string>();
    [SerializeField] List <string> list_LosesBall = new List<string>();
    [SerializeField] List<string> list_Preparation = new List<string>();
    [SerializeField] List<string> list_successShoot = new List<string>();
    [SerializeField] List<string> list_FailShoot = new List<string>();
    [SerializeField] List<string> list_successJuke = new List<string>();
    [SerializeField] List<string> list_failJuke = new List<string>();
    [SerializeField] List<string> list_successPass = new List<string>();
    [SerializeField] List<string> list_failPass = new List<string>();
    [SerializeField] List<string> list_Opp_FailedAttempt = new List<string>();
    [SerializeField] List<string> list_Opp_SucAttempt = new List<string>();
    [SerializeField] List<string> list_ChargeLines = new List<string>();
    [SerializeField] List<string> list_Shove = new List<string>();
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


    [Header("Casrds")]
    [SerializeField] Animator animator_HandCards;

    [Header("OffensivePanel")]
    [SerializeField] GameObject panel_OffensivePanel;
    [SerializeField] Transform transform_statsArea;
    [SerializeField] Transform transform_gameStatsArea;
    [SerializeField] Transform transform_ActiveHomePlayers;
    //[SerializeField] TextMeshProUGUI text_playerWithTheBallName;
    [SerializeField] Transform transform_playersZones;
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
    [SerializeField] TextMeshProUGUI text_currentDMG;
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
        if (_matchManager.CanChooseDefenseAction)
        {
            _actionDefense.SetActive(true);
        }
        else
        {
            _actionDefense.SetActive(false);
        }
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
            _panel_SubsPanel.SetActive(true);
        }
        if (_matchManager.HomeTeam.hasHDefense)
        {
            text_CurrentDefensiveStyle.text = "Aggressive";
        }
        else
        {
            text_CurrentDefensiveStyle.text = "Normal";
        }
        //Substitution Buttons
        if (GameObject.Find("Starters"))
        {
            for (int i = 0; i < GameObject.Find("Starters").transform.childCount; i++)
            {
                GameObject.Find("Starters").transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString() + " "
                    + _matchManager.HomeTeam.playersListRoster[i].playerLastName + " " + _matchManager.HomeTeam.playersListRoster[i].J_Number.ToString();
                GameObject.Find("Starters").transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "OVR " + _matchManager.HomeTeam.playersListRoster[i].ovr.ToString();
                GameObject.Find("Starters").transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Image>().fillAmount =
                    (float)_matchManager.HomeTeam.playersListRoster[i].CurrentStamina / (float)_matchManager.HomeTeam.playersListRoster[i].MaxStamina;
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
                GameObject.Find("Starters").transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = sprite;
                Sprite[] sprites1 = Resources.LoadAll<Sprite>("2D/UI/Archtype");
                Sprite spriteArch = null;
                //spriteArchtype = sprites1[index];

                if (_matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait >= 0 || _matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait <= 20)
                {
                    spriteArch = sprites1[0];
                }
                if (_matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait >= 21 && _matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait <= 40)
                {
                    //print("Imge number 1");
                    spriteArch = sprites1[1];
                }
                if (_matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait >= 41 && _matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait <= 60)
                {
                    //print("Imge number 1");
                    spriteArch = sprites1[2];
                }
                if (_matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait >= 61 && _matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait <= 80)
                {
                    spriteArch = sprites1[3];
                }
                GameObject.Find("Starters").transform.GetChild(i).GetChild(4).GetComponent<Image>().sprite = spriteArch;
                //print(_matchManager.HomeTeam.playersListRoster[i].CurrentStamina + "thisis my stamina");
            }
        }
        if (GameObject.Find("Bench"))
        {
            for (int i = 0; i < GameObject.Find("Bench").transform.childCount; i++)
            {

                //print(GameObject.Find("Bench").transform.GetChild(0).GetChild(0).name + " Number od players");
                //GameObject.Find("Bench").transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].playerFirstName.ToString() + " "+
                //_matchManager.HomeTeam.playersListRoster[i + 4].playerLastName.ToString() + " " + _matchManager.HomeTeam.playersListRoster[i + 4].J_Number.ToString();
                //GameObject.Find("Bench").transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "OVR " +_matchManager.HomeTeam.playersListRoster[i + 4].ovr.ToString();
                //GameObject.Find("Bench").transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Image>().fillAmount =
                //(float)_matchManager.HomeTeam.playersListRoster[i + 4].CurrentStamina / (float)_matchManager.HomeTeam.playersListRoster[i+4].MaxStamina;
            }
        }
        for (int i = 0; i < _timeOutBenchPlayers.childCount; i++)
        {
            _timeOutBenchPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].playerLastName + " " 
                + _matchManager.HomeTeam.playersListRoster[i + 4].J_Number;
            _timeOutBenchPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].ovr.ToString();
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
            //spriteArchtype = sprites1[index];

            if (_matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait >= 0 || _matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait <= 20)
            {
                spriteArch = sprites1[0];
            }
            if (_matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait >= 21 && _matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait <= 40)
            {
                //print("Imge number 1");
                spriteArch = sprites1[1];
            }
            if (_matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait >= 41 && _matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait <= 60)
            {
                //print("Imge number 1");
                spriteArch = sprites1[2];
            }
            if (_matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait >= 61 && _matchManager.HomeTeam.playersListRoster[i + 4].ImageCharacterPortrait <= 80)
            {
                spriteArch = sprites1[3];
            }
            _timeOutBenchPlayers.GetChild(i).GetChild(4).GetComponent<Image>().sprite = spriteArch;
        }
        for (int i = 0; i < 4; i++)
        {
            //HomeTeams
            transform_ActiveHomePlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerLastName.ToString();
            transform_ActiveHomePlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].PointsMatch.ToString();
            transform_ActiveHomePlayers.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].J_Number.ToString();
            //AwayTeams
            transform_ActiveAwayPlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].J_Number.ToString();
            transform_ActiveAwayPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.AwayTeam.playersListRoster[i].playerLastName.ToString();


        }
        image_adrenalineBar.fillAmount = (float)gameManager.playerTeam.AdrenalineBar / (float)gameManager.playerTeam.AdrenalineBarFull;
        image_offensivePanel_awayTeamHpBAR.fillAmount = (float)_matchManager.awayHP / (float)100;

        if (Input.GetKeyDown(KeyCode.I))
        {
            _matchManager.awayHP -= 20;
            debugText_awayHp.text = _matchManager.awayHP.ToString();
        }

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
        //print("UpdateStats");
        //text_playerWithTheBallName.text = _matchManager.playerWithTheBall.playerFirstName + " " + _matchManager.playerWithTheBall.playerLastName;
        //text_PlayerwithTheBallJersey.text = _matchManager.playerWithTheBall.J_Number.ToString();
        //Sprite alteration/update
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[_matchManager.playerWithTheBall.ImageCharacterPortrait];
        image_playerWithTheBallPortrait.sprite = sprite;
        PersonalityUpdate();
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
    public void ResultActionPanel(string triggerName, int actionIndex)
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
        }
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
    /*
    public void ActivateAnimatorOffensivePanel()
    {
        print(off_Animator + "New offAnimator");
        off_Animator.SetBool("On", true);
    }
    */
    /*
    public void UsedPlayerBtns()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_matchManager.HomeTeam.playersListRoster[i] == _matchManager.playerWithTheBall)
            {
                Animator animator = transform_ActiveHomePlayers.GetChild(i).GetChild(7).GetComponent<Animator>();
                animator.SetBool("On", false);
            }
            
            //print(_matchManager.HomeTeam.playersListRoster[i].playerLastName + " this is his zone: " + _matchManager.HomeTeam.playersListRoster[i].CurrentZone);
        }
    }
    */
    public void UpdatePlayerPlacements()
    {
        //print("Check position");
        for (int i = 0; i < 4; i++)
        {
            if (_matchManager.HomeTeam.playersListRoster[i].CurrentZone > 0)
            {
                //print("PLay is zoned(UPDATE) " + _matchManager.HomeTeam.playersListRoster[i].playerLastName);
                /*
                transform_ActiveHomePlayers.GetChild(i).position = 
                    transform_ActiveHomePlayers.GetChild(i).GetChild(6).GetChild(_matchManager.HomeTeam.playersListRoster[i].CurrentZone).position;
                */
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
    void PersonalityUpdate()
    {
        //Personality Icons
        Sprite sprite = null; //= Resources.Load<Sprite>("Assets/Resources/2D/Player Personalities/UI_icon_Personalite_01.png");
        switch (_matchManager.playerWithTheBall.Personality)
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
        //_sprite = sprite;
        //image_playerPersonality.sprite = sprite;
    }
    public void UseSpBtn()
    {
        anim_Sp_Button_Activate.SetTrigger("Go");
    }
    public void StartResultPanel(string textM)
    {
        animator_endgame_Result.SetTrigger("Go");
        text_victoryDefeatResult.text = textM;
        
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
        print(style + " this is the style");
    }
    public void OffensivePanelAwayTeamUpdate(Team awayTeam)
    {
        for (int i = 0; i < 4; i++)
        {
            transform_OffensivePanel_awayTeamPlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = awayTeam.playersListRoster[i].playerLastName;
            transform_OffensivePanel_awayTeamPlayers.GetChild(i).GetChild(3).GetComponent<Image>().fillAmount = awayTeam.playersListRoster[i].CurrentStamina / awayTeam.playersListRoster[i].MaxStamina;
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
}
