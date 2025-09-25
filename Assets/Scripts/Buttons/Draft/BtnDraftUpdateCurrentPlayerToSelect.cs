using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnDraftUpdateCurrentPlayerToSelect : MonoBehaviour, IPointerEnterHandler
{
    public int index;

    public string playerAge;
    public string playerName;

    Sprite sprite;
    Image playerPortrait;
    TextMeshProUGUI text_playerName;
    TextMeshProUGUI text_PlayerAge;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gameManager.mode == GameManager.GameMode.Draft)
        {
            playerPortrait = GameObject.Find("Image_CurrentPlayerToSelect").GetComponent<Image>();

            text_playerName = GameObject.Find("Text_PlayerName").GetComponent<TextMeshProUGUI>();
            text_PlayerAge = GameObject.Find("Text_PlayerAge").GetComponent<TextMeshProUGUI>();
        }
        

    }
    public void SetSprite()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        sprite = sprites[index];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameManager.mode == GameManager.GameMode.Draft)
        {
            playerPortrait.sprite = sprite;
            text_PlayerAge.text = playerAge;
            text_playerName.text = playerName;
        }
            
    }
}
