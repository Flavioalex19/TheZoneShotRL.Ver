using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class btnTeamSelection : MonoBehaviour, IPointerEnterHandler
{

    TeamSelectionManager _teamSelectManager;
    [SerializeField] Team team;


    private void Start()
    {
       _teamSelectManager = GameObject.Find("TeamSelectionManager").GetComponent<TeamSelectionManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _teamSelectManager.TeamInfo.text = team.Description.ToString();
        _teamSelectManager.TeamCoach.text = team.CoachName.ToString();
        _teamSelectManager.TeamStyle.text = team._teamStyle.ToString();
    }
}
