using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MatchManager : MonoBehaviour
{
    public enum MatchStates
    {
        None,
        Pause,
        Start,
        Possesion,
        Decision,
        EndPossession,
        Win,
        Lose
    }
    GameManager manager;
    MatchStates match;
    public int GamePossesions = 100;
    int currentGamePossessons;
    Team HomeTeam;
    Team AwayTeam;
    Team teamWithball;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        match = MatchStates.Start;
        currentGamePossessons = GamePossesions;
        //TESTING !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        HomeTeam = manager.leagueTeams[0];
        AwayTeam = manager.leagueTeams[1];
    }

    // Update is called once per frame
    void Update()
    {
        if(match == MatchStates.Start)PreparingMatch();
        if(match == MatchStates.Possesion)TeamPossession(teamWithball);
    }
    void PreparingMatch()
    {
        ChooseTeamBall();
        if (ChooseTeamBall() == 0) 
        { 
            HomeTeam.hasPossession = true;
            teamWithball = HomeTeam;
            //ChoosePlayerToStartWithTheBall(HomeTeam); 
            //match = MatchStates.HomeTeamPos; 
        }
        else 
        { 
            AwayTeam.hasPossession = true; 
            teamWithball = AwayTeam;
            //ChoosePlayerToStartWithTheBall(AwayTeam); 
            //match = MatchStates.AwayTeamPos; 
        }
        
        ChoosePlayerToStartWithTheBall(teamWithball);
        match = MatchStates.Possesion;
    }
    int ChooseTeamBall()
    {
        int teamIndex;
        teamIndex = Random.Range(0, 1);
        return teamIndex;
    }
    void ChoosePlayerToStartWithTheBall(Team PossessionTeam)
    {
        int index;
        index = Random.Range(0, PossessionTeam.playersListRoster.Count);
        PossessionTeam.playersListRoster[index].HasTheBall = true;
        print(PossessionTeam.playersListRoster[index].playerFirstName + " " + PossessionTeam.TeamName + " " + "Player chosen");
    }
    public void TeamPossession(Team PossessionTeam)
    {
        // Find the player who currently has the ball
        Player currentPlayer = PossessionTeam.playersListRoster.Find(player => player.HasTheBall);
        print(currentPlayer.playerFirstName + " Has the ball");

        
        StartCoroutine(DelaySeconds());
        
    }
    IEnumerator DelaySeconds()
    {
        yield return new WaitForSeconds(5f);
        print("Test");
    }
}
