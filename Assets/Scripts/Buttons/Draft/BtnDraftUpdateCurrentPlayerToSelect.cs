using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public string playerSalary;
    public int PlayerOvr;
    public int archtypeIndex; 

    Sprite sprite;
    Sprite spriteArchtype;
    Image playerPortrait;
    Image playerArchtypeImage;
    TextMeshProUGUI text_playerName;
    TextMeshProUGUI text_PlayerAge;
    TextMeshProUGUI text_PlayerSalary;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gameManager.mode == GameManager.GameMode.Draft)
        {
            playerPortrait = GameObject.Find("Image_CurrentPlayerToSelect").GetComponent<Image>();

            //text_playerName = GameObject.Find("Text_PlayerName").GetComponent<TextMeshProUGUI>();
            text_PlayerAge = GameObject.Find("Text_PlayerAge").GetComponent<TextMeshProUGUI>();
            text_PlayerSalary = GameObject.Find("TextSalary").GetComponent<TextMeshProUGUI>();
            playerArchtypeImage = GameObject.Find("ArchtypeImage").GetComponent<Image>();
        }
        

    }
    public void SetSprite()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        sprite = sprites[index];
        Sprite[] sprites1 = Resources.LoadAll<Sprite>("2D/UI/Archtype");
        //spriteArchtype = sprites1[index];
        
        if(index >=0 || index < 11)
        {
            spriteArchtype = sprites1[0];
        }
        if(index >= 12 && index <= 20)
        {
            print("Imge number 1");
            spriteArchtype = sprites1[1];
        }
        if (index >= 21 && index <= 32)
        {
            print("Imge number 1");
            spriteArchtype = sprites1[2];
        }
        if (index >= 33 && index <= 35)
        {
            spriteArchtype = sprites1[3];
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameManager.mode == GameManager.GameMode.Draft)
        {
            playerPortrait.sprite = sprite;
            text_PlayerAge.text = playerAge;
            //text_playerName.text = playerName;
            text_PlayerSalary.text = playerSalary;
            playerArchtypeImage.sprite = spriteArchtype;
        }
            
    }
}
