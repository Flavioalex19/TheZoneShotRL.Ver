using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchUI : MonoBehaviour
{
    MatchManager _matchManager;
    Transform _debugText;
    [SerializeField] Transform _activeHomePlayers;
    [SerializeField] Transform _activeAwayPlayers;
    [SerializeField] TextMeshProUGUI _homeTeamName;
    [SerializeField] TextMeshProUGUI _awatTeamName;

    [SerializeField] Animator _homeTeamAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayersActive();
        //Debug Area
        if (GameObject.Find("DebugTextHome"))
        {
            for (int i = 0; i < GameObject.Find("DebugTextHome").transform.childCount; i++)
            {
                GameObject.Find("DebugTextHome").transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString();
            }
        }
        //Substitution Buttons
        if (GameObject.Find("Starters"))
        {
            for (int i = 0; i < GameObject.Find("Starters").transform.childCount; i++)
            {
                GameObject.Find("Starters").transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString();
            }
        }
        if (GameObject.Find("Bench"))
        {
            for (int i = 0; i < GameObject.Find("Bench").transform.childCount; i++)
            {
                GameObject.Find("Bench").transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i + 4].playerFirstName.ToString();
            }
        }
    }
    public void SetTheTeamTextForTheMatch()
    {
        _homeTeamName.text = _matchManager.HomeTeam.TeamName.ToString();
        _awatTeamName.text = _matchManager.AwayTeam.TeamName.ToString();
    }
    void UpdatePlayersActive()
    {
        for (int i = 0; i < 4; i++)
        {
            _activeHomePlayers.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].playerFirstName.ToString();
            _activeHomePlayers.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = _matchManager.HomeTeam.playersListRoster[i].PointsMatch.ToString();
        }
    }

    //Animators
    public void TriggerHomeTeamAnim()
    {
        _homeTeamAnimator.SetTrigger("Go");
    }
}
