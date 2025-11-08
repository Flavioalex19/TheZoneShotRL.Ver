using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameCard card;
    MatchManager matchManager;
    GameManager manager;
    Button btn_cardBtn;
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        btn_cardBtn = gameObject.GetComponent<Button>();
    }

    void CardEffect()
    {
        //Define the modify value
        switch (card.cardType)
        {
            case "Common":
                card.modifyValue = 2;
                break;
            case "Bronze":
                card.modifyValue = 3;
                break;
            case "Silver":
                card.modifyValue = 5;
                break;
            case "Gold":
                card.modifyValue = 8;
                break;
        }
        switch (card.cardStyle)
        {
            case CardStyle.Defense:
                if(card.cardType == "Common")
                {
                    
                }
                else if(card.cardType == "Bronze")
                {

                }
                else if (card.cardType == "Silver")
                {

                }
                else if (card.cardType == "Gold")
                {

                }
                break;
            case CardStyle.Attack:
                if (card.cardType == "Common")
                {

                }
                else if (card.cardType == "Bronze")
                {

                }
                else if (card.cardType == "Silver")
                {

                }
                else if (card.cardType == "Gold")
                {

                }
                break;
            case CardStyle.Player:
                if (card.cardType == "Common")
                {

                }
                else if (card.cardType == "Bronze")
                {

                }
                else if (card.cardType == "Silver")
                {

                }
                else if (card.cardType == "Gold")
                {

                }
                break;
            case CardStyle.Stamina:
                for (int i = 0; i < matchManager.teamWithball.playersListRoster.Count; i++)
                {
                    matchManager.teamWithball.playersListRoster[i].CurrentStamina += card.modifyValue;
                }
                break;
        }
    }
}
