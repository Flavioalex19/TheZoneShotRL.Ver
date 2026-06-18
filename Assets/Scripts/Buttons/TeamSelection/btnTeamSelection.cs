using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class btnTeamSelection : MonoBehaviour, IPointerEnterHandler
{

    TeamSelectionManager _teamSelectManager;
    [SerializeField]public Team team;
    [SerializeField]TextMeshProUGUI text_budget;


    private void Start()
    {
       _teamSelectManager = GameObject.Find("TeamSelectionManager").GetComponent<TeamSelectionManager>();
       text_budget = GameObject.Find("Text_TeamCurrency").GetComponent<TextMeshProUGUI>();

        if (team.isChampion)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _teamSelectManager.TeamInfo.text = team.Description.ToString();
        _teamSelectManager.TeamCoach.text = team.CoachName.ToString();
        _teamSelectManager.TeamStyle.text = team._teamStyle.ToString();
        _teamSelectManager._teamStatsArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = team.Moral.ToString();
        //_teamSelectManager._teamStatsArea.GetChild(1).GetComponent<TextMeshProUGUI>().text = team.FrontOfficePoints.ToString();
        //_teamSelectManager._teamStatsArea.GetChild(2).GetComponent<TextMeshProUGUI>().text = team.FansSupportPoints.ToString();
        _teamSelectManager._teamStatsArea.GetChild(3).GetComponent<TextMeshProUGUI>().text = team.CurrentBudget.ToString();
        text_budget.text= team.CurrentBudget.ToString();
    }
}
