using UnityEngine;

public class DebugFunctions : MonoBehaviour
{
    GameManager gameManager;
    LeagueManager leagueManager;
    public MatchManager matchManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
    }

    public void ZeroMorale()
    {
        gameManager.playerTeam.Moral = -1;
    }
    public void JumpToFinalWeeks()
    {
        leagueManager.Week = gameManager.leagueTeams.Count - 2;
    }
    public void OnePosMatch()
    {
        matchManager.currentGamePossessons = 1;
    }
    public void ZeroContract()
    {
        gameManager.playerTeam.playersListRoster[5].ContractYears = 0;
    }
}
