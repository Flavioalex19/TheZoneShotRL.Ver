using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayoffUI : MonoBehaviour
{
    [SerializeField] Transform transform_R8Images;
    [SerializeField] PlayoffManager pfManager;
    GameManager gameManager;
    LeagueManager leagueManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        pfManager.CreatePlayoffBracket();
        UpdateR8Images();
    }

    void UpdateR8Images()
    {
        for (int i = 0; i < transform_R8Images.childCount; i++)
        {
            //Resources.Load<Sprite>("2D/Team Logos/" + gameManager.playerTeam.TeamName);
            transform_R8Images.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("2D/Team Logos/" + leagueManager.List_R8Teams[i].TeamName);
        }
    }
}
