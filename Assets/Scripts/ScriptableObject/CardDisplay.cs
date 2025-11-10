using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameCard card;
    MatchManager matchManager;
    GameManager manager;
    MatchUI matchUI;
    Button btn_cardBtn;
    Transform usedCradsFolder;
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI text_cardName;
    [SerializeField] TextMeshProUGUI text_cardType;
    [SerializeField] TextMeshProUGUI text_cardDescription;
    [SerializeField] Image image_cardImage;
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        matchUI = GameObject.Find("MatchUIManager").GetComponent<MatchUI>();
        usedCradsFolder = GameObject.Find("CardsUsedFolder").transform;
        btn_cardBtn = gameObject.GetComponent<Button>();
        //CardEffect();
        SetCardsValues();
        btn_cardBtn.onClick.AddListener(() =>CardEffect());
        
    }
    void SetCardsValues()
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
        text_cardName.text = card.name;
        text_cardType.text = card.cardType;
        text_cardDescription.text = card.cardDescription;
        image_cardImage.sprite = card.cardImage;
    }
    void CardEffect()
    {
        
        switch (card.cardStyle)
        {
            case CardStyle.Defense:
                if(card.cardType == "Common")
                {
                    matchManager.playerWithTheBall.statDefBuff = card.modifyValue;
                }
                else if(card.cardType == "Bronze")
                {
                    matchManager.playerWithTheBall.statDefBuff = card.modifyValue;
                }
                else if (card.cardType == "Silver")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        matchManager.HomeTeam.playersListRoster[i].statDefBuff = card.modifyValue;
                    }
                }
                else if (card.cardType == "Gold")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        matchManager.HomeTeam.playersListRoster[i].statDefBuff = card.modifyValue;
                    }
                }
                break;
            case CardStyle.Attack:
                if (card.cardType == "Common")
                {
                    matchManager.playerWithTheBall.statBuff = card.modifyValue;

                }
                else if (card.cardType == "Bronze")
                {
                    matchManager.playerWithTheBall.statBuff = card.modifyValue;
                }
                else if (card.cardType == "Silver")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        matchManager.HomeTeam.playersListRoster[i].statBuff = card.modifyValue;
                    }
                }
                else if (card.cardType == "Gold")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        matchManager.HomeTeam.playersListRoster[i].statBuff = card.modifyValue;
                    }
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
        matchManager.canUseCards = false;
        this.transform.SetParent(usedCradsFolder);
        this.transform.localScale = Vector3.one;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        matchUI.UpdateCardsHand();
    }
}
