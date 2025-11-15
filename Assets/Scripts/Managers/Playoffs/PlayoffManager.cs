using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayoffManager : MonoBehaviour
{
    // Start is called before the first frame update
    LeagueManager leagueManager;

    
    void Start()
    {
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
    }

    public void CreatePlayoffBracket()
    {
        // Verifica se há times suficientes para formar os playoffs
        if (leagueManager.Standings == null || leagueManager.Standings.Count < 8)
        {
            Debug.LogWarning("Not enough teams in standings to create playoffs!");
            return;
        }

        // Adiciona os confrontos: 1x8, 2x7, 3x6, 4x5
        for (int i = 0; i < 4; i++)
        {
            Team teamHigh = leagueManager.Standings[i];
            Team teamLow = leagueManager.Standings[leagueManager.Standings.Count - 1 - i];

            // Se o time do jogador estiver em um dos dois, ele vem primeiro
            if (teamHigh.IsPlayerTeam)
            {
                leagueManager.List_R8Teams.Add(teamHigh);
                leagueManager.List_R8Names.Add(teamHigh.TeamName);
                leagueManager.List_R8Teams.Add(teamLow);
                leagueManager.List_R8Names.Add(teamLow.TeamName);
                
                Debug.Log($"Playoff Matchup: {teamHigh.TeamName} vs {teamLow.TeamName}");
            }
            else if (teamLow.IsPlayerTeam)
            {
                leagueManager.List_R8Teams.Add(teamLow);
                leagueManager.List_R8Names.Add(teamLow.TeamName);

                leagueManager.List_R8Teams.Add(teamHigh);
                leagueManager.List_R8Names.Add(teamHigh.TeamName);
                Debug.Log($"Playoff Matchup: {teamLow.TeamName} vs {teamHigh.TeamName}");
            }
            else
            {
                // Ordem padrão (1º x 8º, etc.)
                leagueManager.List_R8Teams.Add(teamHigh);
                leagueManager.List_R8Names.Add(teamHigh.TeamName);
                leagueManager.List_R8Teams.Add(teamLow);
                leagueManager.List_R8Names.Add(teamLow.TeamName);

                Debug.Log($"Playoff Matchup: {teamHigh.TeamName} vs {teamLow.TeamName}");
            }
        }

        Debug.Log("Playoff bracket created successfully!");
    }
}
