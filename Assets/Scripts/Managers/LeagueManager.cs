using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeagueManager : MonoBehaviour
{

    public int Week = 0;

    public List<EventOption> eventOptions = new List<EventOption>();
    public EventOption activeOption;
    public bool canGenerateEvents = true;
    public bool canStartANewWeek = true;

    [SerializeField]Team _playerTeam;

    [SerializeField]public List<Team> Standings = new List<Team>();

    #region Events Variables
    EventType _choseEventType;
    #endregion

    public bool canTrade = true;

    public bool canTrain = true;

    public bool canNegociateContract;
    //Tutorial
    public bool CanStartTutorial = true;


    //Playoffs
    public bool isOnR8 = false;
    public bool isOnR4 = false;
    public bool isOnFinals = false;
    public List<Team> List_R8Teams = new List<Team>();
    public List<string> List_R8Names = new List<string>();
    public List<Team> List_R4Teams = new List<Team>();
    public List<Team> List_R4Names = new List<Team>();
    public List<Team> List_Finalist = new List<Team>();
    public List<Team> List_FinalistName = new List<Team>();

    GameManager gameManager;
    UiManager uiManager;

    public static LeagueManager instance;
    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents this GameObject from being destroyed when changing scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UiManager>();
        //check to verify if thre is no save file here if thre is none satart the new week-week 1
        _playerTeam = gameManager.playerTeam;//TODO: Search the team that is controlled by the player!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            if (gameManager.leagueTeams[i].IsPlayerTeam) _playerTeam = gameManager.leagueTeams[i];
        }
        if (canGenerateEvents == true && gameManager.mode == GameManager.GameMode.TeamManagement)
        {
            NewWeek();
        }
        Standings = gameManager.leagueTeams;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if(canGenerateEvents == true && gameManager.mode == GameManager.GameMode.TeamManagement)
        {
            print("NEW WEEK");
            NewWeek();
        }
        *///Else will be about the play off
        if (!canGenerateEvents || gameManager == null) return;

        if (gameManager.mode == GameManager.GameMode.TeamManagement)
        {
            NewWeek();
        }
    }
    public void IncreaseWeek()
    {
        Week++;
        
    }
    void NewWeek()
    {
        //Week++;
        //Create a IF to very the week
        
        if (/*GameObject.Find("ChoicesForTheWeek") */gameManager.mode == GameManager.GameMode.TeamManagement/*&& canStartANewWeek == true*/)
        {
            if(canStartANewWeek == true)
            {
                canTrain = true;
                canTrade = true;
                canNegociateContract = true;
                CreateEventsForWeek();
                Transform ChoiceButtonsTransform = GameObject.Find("ChoiceButtons").transform;

                int buttonCount = Mathf.Min(ChoiceButtonsTransform.childCount, 2); // Ensure no out-of-range

                EventType randomEvent0 = (EventType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
                EventType randomEvent1;
                do
                {
                    randomEvent1 = (EventType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
                }
                while (randomEvent1 == randomEvent0);
                //Create the onclick event of the button to choose the event type
                ChoiceButtonsTransform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ChooseEventTypeOnClick(randomEvent0));
                ChoiceButtonsTransform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ChooseEventTypeOnClick(randomEvent1));
                ChoiceButtonsTransform.GetChild(0).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = randomEvent0.ToString();
                ChoiceButtonsTransform.GetChild(1).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = randomEvent1.ToString();
            }
            else
            {
                print("not ativate");
            }
        }
        canGenerateEvents = false;
        
    }
    public void CreateEventsForWeek()
    {
        eventOptions.Add(new EventOption 
        { 
            Index = 0, Description = "Boss, local TV wants a quick hit before the match. Respond in a more humorous way or straight tactics?",
            Choice1 = "Fun & Relatable <color=#FFD700>Marketing +1</color>",
            Choice2 = "Pro & Tactical <color=#FFD700>Office +1</color>",
            btnIndex0 = 0,
            btnIndex1 = 6,
            Modifier = 1,
            eventType = EventType.Interview 
        });
        eventOptions.Add(new EventOption
        {
            Index = 1,
            Description = "City council just emailed a one-time youth grant. Take it and shout it out, or funnel it to the stadium?",
            Choice1 = "Accept & Promote Locally <color=#FFD700>Marketing +1</color>",
            Choice2 = "Redirect to Facility Upgrade <color=#FFD700>Arena +1</color>",
            btnIndex0 = 4,
            btnIndex1 = 0,
            Modifier = 1,
            eventType = EventType.ReplyToEmails
        });
        eventOptions.Add(new EventOption
        {
            Index = 2,
            Modifier = 1,
            Description = "Players are voting on locker room upgrades. Smart tech or recovery pods?",
            Choice1 = "Smart Lockers & Charging Stations <color=#FFD700>Team Equipment +1</color>",
            Choice2 = "Better Recovery Pods <color=#FFD700>MedCare +1</color>",
            eventType = EventType.TeamMeeting


        }) ;
        eventOptions.Add(new EventOption 
        {

            Index = 3,
            Description = "Energy drink brand on the line—standard cash deal or premium with player product?",
            Choice1 = "Standard Deal + Cash <color=#FFD700>Finances +1</color>",
            Choice2= "Premium Deal + Product for Players <color=#FFD700>Team Equipment +1</color>",
            Modifier = 1,
            eventType = EventType.Sponsor
        
        });
        eventOptions.Add(new EventOption
        {

            Index = 4,
            Description = "Vox media in a podcast is asking what the team needs most. Med staff or fan support?",
            Choice1 = "We need better medical support <color=#FFD700>MedCare +1</color>",
            Choice2 = "Fan experience comes first <color=#FFD700>Arena +1</color>",
            Modifier = 1,
            eventType = EventType.Interview

        });
        eventOptions.Add(new EventOption
        {

            Index = 5,
            Description = "Helix cell is offering bulk gear discount—expires today. Stock up or lock in future credit?",
            Choice1 = "Stock Up Now <color=#FFD700>Team Equipment +1</color>",
            Choice2 = "Negotiate for Future Credit <color=#FFD700>Finances +1</color>",
            Modifier = 1,
            eventType = EventType.Sponsor

        });
        eventOptions.Add(new EventOption
        {

            Index = 6,
            Description = "Bank wants to fund a new VIP area. Luxury skybox or family zone?",
            Choice1 = "Luxury Skybox <color=#FFD700>Arena +1</color>",
            Choice2 = "Family Zone with Kids Area <color=#FFD700>Marketing +1</color>",
            Modifier = 1,
            eventType = EventType.TeamMeeting

        });
        eventOptions.Add(new EventOption
        {

            Index = 7,
            Description = "Maintenance found extra space under the stands. Physio suite or merch shop?",
            Choice1 = "Convert to Physio Suite <color=#FFD700>MedCare +1</color>",
            Choice2 = "Add Merch Pop-Up Shop <color=#FFD700>Marketing +1</color>",
            Modifier = 1,
            eventType = EventType.ReplyToEmails

        });
        eventOptions.Add(new EventOption
        {

            Index = 8,
            Description = "HyperMetrics XY is offering free analytics—if we upgrade servers. Take it or sell access?",
            Choice1 = "Take the Suite + Upgrade Servers <color=#FFD700>Office +1</color>",
            Choice2 = "Sell the Suite Access to League <color=#FFD700>Finances +1</color>",
            Modifier = 1,
            eventType = EventType.Sponsor

        });

    }
    void SetNewChoiceForEvent(EventType type)
    {
        //print(type + "This is the eventType!!!!");
        List<EventOption> filteredEvents = new List<EventOption>();
        EventOption event1;
        EventOption event2;
        foreach (var e in eventOptions)
        {
            if (e.eventType == type)
            {
                filteredEvents.Add(e);
                print(e.Index + " Event added");
            }
                
        }
        int firstIndex = UnityEngine.Random.Range(0, filteredEvents.Count);
        int secondIndex;
        
        event1 = filteredEvents[UnityEngine.Random.Range(0, filteredEvents.Count)];
        //print(event1.Index + " This is the index of the event chosen");
        //CREATE THE BUTTONS
        Transform EventsButtonsTransform = GameObject.Find("ButtonsEvent").transform;
        //TextMeshProUGUI eventTypeText = GameObject.Find("EventTypeText").GetComponent<TextMeshProUGUI>();
        //eventTypeText.text = type.ToString();
        EventsButtonsTransform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => AddOnClick(event1, 0));
        EventsButtonsTransform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => AddOnClick(event1, 1));
        EventsButtonsTransform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = event1.Choice1;
        EventsButtonsTransform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = event1.Choice2;
        List<Sprite> spriteList = new List<Sprite>(Resources.LoadAll<Sprite>("2D/UI/Team Management/WeekEventsButtons/Official"));
        EventsButtonsTransform.GetChild(0).GetComponent<Image>().sprite = spriteList[0];
        EventsButtonsTransform.GetChild(1).GetComponent<Image>().sprite = spriteList[1];

        GameObject.Find("EventText").GetComponent<TextMeshProUGUI>().text = event1.Description.ToString();

    }

    //Buttons
    public void AddOnClick(EventOption eventOption, int indexOfChoice)
    {
        activeOption = eventOption;
        canStartANewWeek = false;
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            if (gameManager.leagueTeams[i].IsPlayerTeam)
            {
                _playerTeam = gameManager.leagueTeams[i];
            }
            
        }
        if (_playerTeam == null)
        {
            _playerTeam = gameManager.playerTeam;
            if (_playerTeam == null)
            {
                Debug.LogError("_playerTeam is NULL in AddOnClick");
                return;
            }
        }
        switch (eventOption.Index)
        {
            case 0:
                //print(eventOption.Index + " " + eventOption.Description);
                if(indexOfChoice == 0)
                {
                    gameManager.playerTeam.MarketingLvl++;
                }
                else
                {
                    gameManager.playerTeam.OfficeLvl++;
                }
                break;
            case 1:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    gameManager.playerTeam.MarketingLvl++;
                }
                else
                {
                    gameManager.playerTeam.ArenaLvl++;
                }
                break; 
            case 2:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    gameManager.playerTeam.TeamEquipmentLvl++;
                }
                else
                {
                    gameManager.playerTeam.MedicalLvl++;
                }
                break;
            case 3:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    gameManager.playerTeam.FinancesLvl++;
                }
                else
                {
                    gameManager.playerTeam.TeamEquipmentLvl++;
                }
                break;
            case 4:
                if (indexOfChoice == 0)
                {
                    gameManager.playerTeam.MedicalLvl++;
                }
                else
                {
                    gameManager.playerTeam.ArenaLvl++;
                }
                break;
            case 5:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    gameManager.playerTeam.TeamEquipmentLvl++;
                }
                else
                {
                    gameManager.playerTeam.FinancesLvl++;
                }
                break;
            case 6:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    gameManager.playerTeam.ArenaLvl++;
                }
                else
                {
                    gameManager.playerTeam.MarketingLvl++;
                }
                break;
            case 7:
                if (indexOfChoice == 0)
                {
                    gameManager.playerTeam.MedicalLvl++;
                }
                else
                {
                    gameManager.playerTeam.MarketingLvl++;
                }
                break;
            case 8:
                if (indexOfChoice == 0)
                {
                    gameManager.playerTeam.OfficeLvl++;
                }
                else
                {
                    gameManager.playerTeam.FinancesLvl++;
                }
                break;
            default:
                break;
        }

        //gameManager.saveSystem.SaveTeam(_playerTeam);//This could change!
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            gameManager.saveSystem.SaveTeam(gameManager.leagueTeams[i]);
        }
        eventOptions.Clear(); // Clear only after usage
        

    }
    public void ChooseEventTypeOnClick(EventType eventType)
    {
        _choseEventType = eventType;
        SetNewChoiceForEvent(eventType);
        
    }
    //Standings
    public void CreateStandings()
    {
        if (Week > 1)
        {
            Standings = Standings
                .OrderByDescending(team => team.Wins)
                .ThenByDescending(team => team.Draws)
                .ThenBy(team => team.Loses) // less losses is better
                .ToList();
        }
        for (int i = 0; i < Standings.Count; i++)
        {
            print(Standings[i].TeamName + " " + Standings[i].Wins);
        }
    }
    //SalaryCap
    public void CreateTeamSalary()
    {
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            for(int j = 0; j < gameManager.leagueTeams[i].playersListRoster.Count; j++)
            {
                gameManager.leagueTeams[i].CurrentSalary += gameManager.leagueTeams[i].playersListRoster[j].Salary;
            }
        }

    }
    
}
[System.Serializable]
public class EventOption
{
    public int Index;//Number of the event
    public string Description; // The text for the event option
    public string Choice1;
    public string Choice2;
    public int btnIndex0;
    public int btnIndex1;
    public int Modifier; // Example effect: morale boost or drop
    public EventType eventType;// Type of the event
    
}
public enum EventType
{
    Interview,
    ReplyToEmails,
    TeamMeeting,
    Sponsor
}

