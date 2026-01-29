using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardStyle
{
    Defense,
    Attack,
    Player,
    Stamina,
    Sp,
    Juke,
    Pass
}
[CreateAssetMenu(fileName = "NewGameCard", menuName = "ScriptableObjects/GameCard", order = 1)]
public class GameCard : ScriptableObject
{

    [Header("Card Visuals")]
    public Sprite cardImage;

    [Header("Card Type")]
    public string cardType;

    [Header("Card Name")]
    public string cardName;

    [Header("Card Description")]
    [TextArea(2, 5)]
    public string cardDescription;

    [Header("Card effect")]
    public CardStyle cardStyle;

    [Header("Card Effect Value")]
    public int modifyValue;
}
