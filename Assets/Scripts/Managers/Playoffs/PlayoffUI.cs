using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayoffUI : MonoBehaviour
{
    [SerializeField] Transform transform_R8Images;
    [SerializeField] Transform transform_R4Images;
    [SerializeField] Transform transform_FinalsImages;
    [SerializeField] PlayoffManager pfManager;
    [SerializeField] Button advButttton;
    GameManager gameManager;
    LeagueManager leagueManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        pfManager.CreatePlayoffBracket();
        UpdateR8Images();
        if (leagueManager.isOnR4)
        {
            UpdateSemiFinalsLogos();
        }
        if (leagueManager.isOnFinals)
        {
            UpdateFinalistsLogos();
        }
        advButttton.onClick.AddListener(() => gameManager.GoToMatch());
    }

    void UpdateR8Images()
    {
        for (int i = 0; i < transform_R8Images.childCount; i++)
        {
            //Resources.Load<Sprite>("2D/Team Logos/" + gameManager.playerTeam.TeamName);
            transform_R8Images.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("2D/Team Logos/" + leagueManager.List_R8Teams[i].TeamName);
        }
    }
    void UpdateSemiFinalsLogos()
    {
        for (int i = 0; i < transform_R4Images.childCount; i++)
        {
            //Resources.Load<Sprite>("2D/Team Logos/" + gameManager.playerTeam.TeamName);
            transform_R4Images.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("2D/Team Logos/" + leagueManager.List_R4Teams[i].TeamName);
        }
    }
    void UpdateFinalistsLogos()
    {
        for (int i = 0; i < transform_FinalsImages.childCount; i++)
        {
            //Resources.Load<Sprite>("2D/Team Logos/" + gameManager.playerTeam.TeamName);
            transform_FinalsImages.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("2D/Team Logos/" + leagueManager.List_Finalist[i].TeamName);
        }
    }
}
