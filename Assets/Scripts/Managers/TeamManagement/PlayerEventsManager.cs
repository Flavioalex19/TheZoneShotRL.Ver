using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerEventsManager : MonoBehaviour
{
    //public GameObject player;
    public List<PlayerEvent> events = new List<PlayerEvent>();
    public Player playerChoosen = null;
    public Player playerChoosen1 = null;
    Player buffedPlayerd = null;
    public PlayerEvent eventChoosen = null;

    GameManager gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerChoosen = null;
        playerChoosen1 = null ;
        buffedPlayerd = null;
        CreatePlayerEvent();
        ChoosePlayerEvent();
        if (eventChoosen.PlayerEventType == PlayerEventsType.Bonds) ChooseBondPlayers(gameManager.playerTeam.playersListRoster);
    }
    private void Start()
    {
        //CreatePlayerEvent();
        //ChoosePlayerEvent();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreatePlayerEvent()
    {
        /*
        events.Add(new PlayerEvent
        {
            Index = 0,
            PlayerEventType = PlayerEventsType.Bonds,
            Description = "Training Sync: " +
            "The coach's clipboard shows the two players completing each repetition in perfect synchrony: passes, cuts, blocks. " +
            "The rest of the team is half a second behind. The coach blows the whistle. Let's see what we can do.",
            Choice1 = "Bond:Lock-Step Drill - Create a Bond",
            Choice2 = "Buff: Solo Polish - Buff +1 On players"
        });
        */
        events.Add(new PlayerEvent
        {
            Index =1,
            PlayerEventType = PlayerEventsType.Bonds,
            Description = "Athena:Hey Boss, just noticed—two of our guys were college roommates. " +
            "Same playbook, same bad pizza habits. " +
            "Should we lean into that chemistry and build plays around it, or mix them in with the system?",
            Choice1 ="Bond: Old pals - Create Bond",
            Choice2 = "Upgrade: Merge into the system - upgrade player stats"
            
        });
        events.Add(new PlayerEvent
        {
            Index = 2,
            PlayerEventType = PlayerEventsType.Upgrade,
            Description = "Athena : Hey Boss, quick heads-up from today’s session—couple of guys are moving a little stiff, not quite at match sharpness. " +
            "Should we slot in some light recovery drills this week, or push a targeted conditioning block to have them flying by kickoff?",
            Choice1 = "Upgrade: Conditioning - Team +1 Attributes",
            Choice2 = "Buff : Recovery - Buff +1"

        });
        events.Add(new PlayerEvent
        {
            Index = 3,
            PlayerEventType = PlayerEventsType.Bonds,
            Description = "Out on the training pitch, you spot two players off to the side, quietly running a little side-drill that’s not on today’s sheet. Looks sharp." +
            " Do you pull them back into the group plan, or give them space to cook up something new?",
            Choice1 = "Bond: We are cooking! - Create Bond",
            Choice2 = "Upgrade: Gear into the system - Upgrade Players +1 all stats"
        });
        events.Add(new PlayerEvent
        {
            Index=4,
            PlayerEventType = PlayerEventsType.Upgrade,
            Description = "Athena: Boss, the squad’s buzzing—energy’s through the roof in both gym and film room. " +
            "Do we ride the wave with extra tactical sessions, or channel it into more conditioning to lock in peak fitness?",
            Choice1 = "Upgrade : Hard Shell - Upgrade all stats for team",
            Choice2 = "Buff: Playing Chess - Buff all players +1"
        });
        events.Add(new PlayerEvent
        {
            Index = 5,
            PlayerEventType = PlayerEventsType.Upgrade,
            Description = "Athena : Boss, shots and passes are drifting a touch this week. " +
            "Want to run tight-range finishing drills to dial in accuracy, or hit set-piece reps to boost scoring odds?",
            Choice1 = "Upgrade : Happy trigger - Upgrade all Stats +1",
            Choice2 = "Buff : Set pieces - Buff All players"
        });
        events.Add(new PlayerEvent
        {
            Index = 6,
            PlayerEventType = PlayerEventsType.Upgrade,
            Description = "Athena : Boss, today’s slate is classic old-school grind. " +
            "Do we hammer the physical side—laps, ladders, lungs or go full tactical with shape, patterns, and walkthroughs",
            Choice1 = "Upgrade : HEAVY WEIGHT - Upgrade all stats +1",
            Choice2 = "Buff : Arrow and Circles = Buff all player +1"
        });
    }
    public void ChoosePlayerEvent()
    {
        int randomIndex = UnityEngine.Random.Range(0, events.Count);
        eventChoosen = events[randomIndex];
    }
    //Choose Player
    void ChoosePlayerToBuff(List<Player> teamPlayers)
    {
        int randomIndex = UnityEngine.Random.Range(0, teamPlayers.Count);
        buffedPlayerd = teamPlayers[randomIndex];
    }
    //Choose bonds
    void ChooseBondPlayers(List<Player> teamPlayers)
    {
        if (teamPlayers == null || teamPlayers.Count < 2)
            return;

        List<Player> eligiblePlayers = teamPlayers
            .Where(p => p.bondPlayer == null)
            .ToList();

        if (eligiblePlayers.Count == 0)
            return;

        playerChoosen = eligiblePlayers[UnityEngine.Random.Range(0, eligiblePlayers.Count)];

        List<Player> validPlayers = teamPlayers
            .Where(p => p != playerChoosen &&
                        p.bondPlayer == null &&
                        (p.playerFirstName != playerChoosen.playerFirstName ||
                         p.playerLastName != playerChoosen.playerLastName))
            .ToList();

        if (validPlayers.Count > 0)
            playerChoosen1 = validPlayers[UnityEngine.Random.Range(0, validPlayers.Count)];
    }
    public void CreateBond()
    {
        if (playerChoosen == null || playerChoosen1 == null)
            return;

        playerChoosen.bondPlayer = playerChoosen1;
        playerChoosen1.bondPlayer = playerChoosen;

        Debug.Log("Bond created: " + playerChoosen.playerFirstName + " <-> " + playerChoosen1.playerFirstName);
    }
    //Upgade
    public void PlayersUpgrade()
    {
        if(eventChoosen.PlayerEventType != PlayerEventsType.Bonds)
        {
            for (int i = 0; i < gameManager.playerTeam.playersListRoster.Count; i++)
            {
                gameManager.playerTeam.playersListRoster[i].Awareness += 1;
                gameManager.playerTeam.playersListRoster[i].Shooting+= 1;
                gameManager.playerTeam.playersListRoster[i].Consistency += 1;
                gameManager.playerTeam.playersListRoster[i].Control += 1;
                gameManager.playerTeam.playersListRoster[i].Defending += 1;
                gameManager.playerTeam.playersListRoster[i].Juking += 1;
                gameManager.playerTeam.playersListRoster[i].UpdateOVR();
            }
        }
        else
        {
            if(playerChoosen !=null && playerChoosen1 != null)
            {
                playerChoosen.Awareness += 1;
                playerChoosen.Shooting += 1;
                playerChoosen.Consistency += 1;
                playerChoosen.Control += 1;
                playerChoosen.Defending += 1;
                playerChoosen.Juking += 1;
                playerChoosen.UpdateOVR();

                playerChoosen1.Awareness += 1;
                playerChoosen1.Shooting += 1;
                playerChoosen1.Consistency += 1;
                playerChoosen1.Control += 1;
                playerChoosen1.Defending += 1;
                playerChoosen1.Juking += 1;
                playerChoosen1.UpdateOVR();
            }
            
        }
    }
    //buff
    public void BuffPlayers()
    {
        if (eventChoosen.PlayerEventType != PlayerEventsType.Bonds)
        {
            for (int i = 0; i < gameManager.playerTeam.playersListRoster.Count; i++)
            {
                gameManager.playerTeam.playersListRoster[i].buff += 1;
                
            }
        }
        else
        {
            if (playerChoosen != null && playerChoosen1 != null)
            {
                playerChoosen.buff += 1;
                playerChoosen1.buff += 1;
            }
        }
    }
}
public class PlayerEvent
{
    public int Index;//Number of the event
    public string Description; // The text for the event option
    public string Choice1;
    public string Choice2;
    public int btnIndex0;
    public int btnIndex1;
    public int Modifier; // Example effect: morale boost or drop
    //public EventType eventType;// Type of the event
    public PlayerEventsType PlayerEventType;

}
public enum PlayerEventsType 
{
    Bonds,
    Buffs,
    Upgrade
}
