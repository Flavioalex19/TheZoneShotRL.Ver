using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchUI : MonoBehaviour
{
    MatchManager _matchManager;
    Transform _debugText;
    // Start is called before the first frame update
    void Start()
    {
        _matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
