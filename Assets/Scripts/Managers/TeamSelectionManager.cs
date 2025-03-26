using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPlayerTeam(Team team)
    {
        team.IsPlayerTeam = true;
    }
}
