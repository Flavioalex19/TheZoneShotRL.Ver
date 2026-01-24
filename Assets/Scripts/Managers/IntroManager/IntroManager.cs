using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField] bool _canProgressToMainMenu = false;
    [SerializeField] bool _canPressEnter = false;
    [SerializeField] Animator _introAnimator;
    [SerializeField] TextMeshProUGUI _teamText;
    [SerializeField] GameManager _gameManager;
    LeagueManager leagueManager;
    [SerializeField] Button btn_career;
    [SerializeField] Button btn_resetLeague;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        _canProgressToMainMenu = false;
        _canPressEnter = false;
        btn_career.onClick.AddListener(() => _gameManager.AdvanceToDraft());
        btn_resetLeague.onClick.AddListener(() => leagueManager.ResetLeagueHistoryMode());
        StartCoroutine(ProgressToMainMenu());
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) )&& _canPressEnter == true)
        {
            _canProgressToMainMenu=true;
        }
        
    }
    IEnumerator ProgressToMainMenu()
    {
        yield return new WaitForSeconds(3.5f);
        _introAnimator.SetTrigger("ToStart");
        _canPressEnter = true;
        yield return new WaitUntil(() => _canProgressToMainMenu);
        _introAnimator.SetTrigger("ToMain");
    }
    public void ChangeStageTransitionTextIntro()
    {
        if (_gameManager.playerTeam != null)
        {
            _teamText.text = "Welcome back coach! " + _gameManager.playerTeam.TeamName + " Career";
        }
        else
        {
            _teamText.text = "A new journey towards the trophy awaits you.";
        }
    }
}
