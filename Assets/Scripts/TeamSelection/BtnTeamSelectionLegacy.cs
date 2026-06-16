using UnityEngine;
using UnityEngine.UI;

public class BtnTeamSelectionLegacy : MonoBehaviour
{
    public Button Btn_legacy;
    [SerializeField] int cost;
    [SerializeField] int legacyIndex;

    LeagueManager leagueManager;
    [SerializeField]TeamSelectionManager teamSelectionManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        teamSelectionManager = GameObject.Find("TeamSelectionManager").GetComponent<TeamSelectionManager>();
        Btn_legacy = GetComponent<Button>();
        Btn_legacy.onClick.AddListener(() => LegacyBtnCost());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LegacyBtnCost()
    {
        if(teamSelectionManager.legacy_currentPoints + cost < 10/*max points*/)
        {
            Btn_legacy.interactable = false;
            teamSelectionManager.legacy_currentPoints += cost;
            teamSelectionManager.text_legacyCurrentPoints.text = teamSelectionManager.legacy_currentPoints.ToString();
            switch (legacyIndex)
            {
                case 1:
                    if (leagueManager.CanDraftlvl1) leagueManager.isOnDraftLVL0 = true;
                    else print("NO DRAFT 1");
                    break;
                case 2:
                    if (leagueManager.CanDraftlvl2) leagueManager.isOnDraftLVL1 = true;
                    break;
                case 3:
                    if (leagueManager.CanDraftlvl3)leagueManager.isOnDraftLVL2 = true;
                    break;
                case 4:
                    if(leagueManager.CanDrafSpPlayer0)leagueManager.CanCreateLegend0 = true;
                    break;
                case 5:
                    if(leagueManager.CanDraftSpPlayer1)leagueManager.CanCreateLegend1 = true;
                    break;
                case 6:
                    if (leagueManager.CanDraftSpPlayer4) leagueManager.CanCreateLegend4 = true;
                    break;

            }
        }
        

    }

    
}
