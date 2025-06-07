using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_TradeBtn : MonoBehaviour
{
    public Player player;

    [SerializeField]TradeManager tradeManager;

    private void Start()
    {
        //gameObject.GetComponent<Button>().onClick.AddListener(() =>tradeManager.SetPlayerToTrade(player));
    }
    public void SetOnClickOnTheButton()
    {
        //gameObject.GetComponent<Button>().onClick.AddListener(() => tradeManager.SetPlayerToTrade(player));
    }
}
