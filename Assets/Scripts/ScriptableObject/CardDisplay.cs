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
        /*
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
                card.modifyValue = 10;
                break;
        }
        */
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
                matchManager.buff_Defense = card.modifyValue;
                break;
            case CardStyle.Attack:
                matchManager.buff_Atk = card.modifyValue;
                
                break;
            case CardStyle.Player:
               
                break;
            case CardStyle.Stamina:
                for (int i = 0; i < matchManager.teamWithball.playersListRoster.Count; i++)
                {
                    matchManager.teamWithball.playersListRoster[i].CurrentStamina += card.modifyValue * 2;
                }
                break;
            case CardStyle.Sp:
                matchManager.buff_SP = card.modifyValue;
                break;
            case CardStyle.Juke:
                matchManager.buff_Juke = card.modifyValue;
                break;
            case CardStyle.Pass:
                matchManager.buff_Pass = card.modifyValue;
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
