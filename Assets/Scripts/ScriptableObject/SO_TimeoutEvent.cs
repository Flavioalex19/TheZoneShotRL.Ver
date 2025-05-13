using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TimeoutNewEvent")]
public class SO_TimeoutEvent : ScriptableObject
{
    public string eventName;
    //public float index;

    public void EventExc(int index, Team team,ref bool islock, ref bool hasAction)
    {
        if(hasAction == true)
        {
            switch (index)
            {
                case 0:

                    break;
                case 1:
                    GameObject.Find("MatchManager").GetComponent<MatchManager>().currentGamePossessons += 15;
                    islock = true;
                    break;
                case 2:
                    team.isOnDefenseBonus = true;
                    islock = true;
                    break;

                default:
                    break;


            }
            
            
        }
        hasAction = false;
    }
}
