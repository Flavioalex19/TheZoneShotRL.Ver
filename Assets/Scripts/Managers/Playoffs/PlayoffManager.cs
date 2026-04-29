using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayoffManager : MonoBehaviour
{
    // Start is called before the first frame update
    LeagueManager leagueManager;
    GameManager gameManager;

    
    void Start()
    {
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void CreatePlayoffBracket()
    {
        
        if (leagueManager.Standings == null || leagueManager.Standings.Count < 8)
        {
            Debug.LogWarning("Not enough teams in standings to create playoffs!");
            return;
        }

        if (leagueManager.List_R8Teams.Count > 0)
            return;

        leagueManager.List_R8Teams.Clear();
        leagueManager.List_R8Names.Clear();

        Team playerTeam = leagueManager.Standings.FirstOrDefault(t => t.IsPlayerTeam);
        Team playerOpponent = null;

        List<Team> remainingTeams = new List<Team>();

        // Monta os confrontos 1x8, 2x7, 3x6, 4x5
        for (int i = 0; i < 4; i++)
        {
            Team high = leagueManager.Standings[i];
            Team low = leagueManager.Standings[7 - i];

            // Se o confronto envolve o player
            if (high.IsPlayerTeam)
            {
                playerOpponent = low;
            }
            else if (low.IsPlayerTeam)
            {
                playerOpponent = high;
            }
            else
            {
                // Confronto que não envolve o player
                remainingTeams.Add(high);
                remainingTeams.Add(low);
            }
        }

        // Player SEMPRE vem primeiro
        if (playerTeam != null && playerOpponent != null)
        {
            leagueManager.List_R8Teams.Add(playerTeam);
            leagueManager.List_R8Names.Add(playerTeam.TeamName);

            leagueManager.List_R8Teams.Add(playerOpponent);
            leagueManager.List_R8Names.Add(playerOpponent.TeamName);

            Debug.Log($"[PLAYOFF] Player Matchup: {playerTeam.TeamName} vs {playerOpponent.TeamName}");
        }

        // Adiciona os outros confrontos mantendo a lógica 1x8, 2x7...
        foreach (Team t in remainingTeams)
        {
            leagueManager.List_R8Teams.Add(t);
            leagueManager.List_R8Names.Add(t.TeamName);
        }

        gameManager.saveSystem.SaveLeague();
        //Debug.Log("Playoff bracket created successfully!");
    }
}
