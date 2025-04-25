using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;

public class LeagueManager : MonoBehaviour
{

    public int Week = 0;

    public List<EventOption> eventOptions = new List<EventOption>();
    public EventOption activeOption;
    public bool canGenerateEvents = true;
    public bool canStartANewWeek = true;

    Team _playerTeam;
    

    #region Events Variables
    EventType _choseEventType;
    #endregion

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
        /*
        eventOptions.Add(new EventOption { Description = "We have to win at any cost!", Modifier = Random.Range(1,5)});
        eventOptions.Add(new EventOption { Description = "We must manage expectations.", Modifier = Random.Range(1, 5) });
        eventOptions.Add(new EventOption {Description = "Discipline and hard work are the focus.", Modifier = Random.Range(1, 5) });
        eventOptions.Add(new EventOption { Description = "Power of friendship is the key guys", Modifier = Random.Range(1, 5) });
        eventOptions.Add(new EventOption { Description = "This league will be yours!", Modifier = Random.Range(1, 5) });
        */
        //CreateEventsForWeek();
        _playerTeam = gameManager.playerTeam;//TODO: Search the team that is controlled by the player!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            if (gameManager.leagueTeams[i].IsPlayerTeam) _playerTeam = gameManager.leagueTeams[i];
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canGenerateEvents == true && gameManager.mode == GameManager.GameMode.TeamManagement)
        {
            print("NEW WEEK");
            NewWeek();
        }//Else will be about the play off
    }
    void NewWeek()
    {
        Week++;
        //Create a IF to very the week
        
        if (GameObject.Find("ChoicesForTheWeek") /*&& canStartANewWeek == true*/)
        {
            if(canStartANewWeek == true)
            {
                CreateEventsForWeek();


                Transform ChoiceButtonsTransform = GameObject.Find("ChoiceButtons").transform;

                //List<EventOption> tempOptions = new List<EventOption>(eventOptions); // Clone the list

                int buttonCount = Mathf.Min(ChoiceButtonsTransform.childCount, 2); // Ensure no out-of-range

                EventType randomEvent0 = (EventType)Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
                EventType randomEvent1;
                do
                {
                    randomEvent1 = (EventType)Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
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
                //GameObject.Find("Choices").SetActive(false);
            }
            



            //eventOptions.Clear(); // Clear only after usage
           

        }
        
        
        

        canGenerateEvents = false;
        
    }
    public void CreateEventsForWeek()
    {
        eventOptions.Add(new EventOption 
        { 
            Index = 0, Description = "We have a sponsor to improve our helmets or gloves. Which one should we choose for our team?",
            Choice1 = "Helmet, so we can be more aggressive in attack",
            Choice2 = "Gloves, to improve our accuracy in attack",
            Modifier = 1,
            eventType = EventType.Sponsor 
        });
        eventOptions.Add(new EventOption
        {
            Index = 1,
            Description = "A new sponsor has come forward with proposals to improve our Pads or Shoes. Which should we choose?",
            Choice1 = "Pads",
            Choice2 = "Shoes",
            Modifier = 1,
            eventType = EventType.Sponsor
        });
        eventOptions.Add(new EventOption
        {
            Index = 2,
            Description = "Reporter: How do you project the rest of the season for your team?",
            Choice1 = "The season has its ups and downs, but we think about doing our best for whatever comes.",
            Choice2 = "We want to show everyone who we are, and to go mercilessly against our opponents.",
            Modifier = 15,
            eventType = EventType.Interview


        }) ;
        eventOptions.Add(new EventOption 
        {

            Index = 3,
            Description = "An unknown figure shows p at the training facility.I’ve seen how you run this team,they say, voice low. Want to hear something that'll give you the edge?",
            Choice1 = "I’ll listen. What’s the catch?",
            Choice2= "No thanks. I run this clean.",
            Modifier = 25,
            eventType = EventType.TeamMeeting
        
        });
        eventOptions.Add(new EventOption
        {

            Index = 4,
            Description = "New Email: You receive a strange email late at night.It seems that the email shows certain aspects of the opposing teams. Not only the players but their Front Offices.",
            Choice1 = "Read the message and study it",
            Choice2 = "Ignore",
            Modifier = 10,
            eventType = EventType.ReplyToEmails

        });
        eventOptions.Add(new EventOption
        {

            Index = 5,
            Description = "You wake up feeling strange. There's no alert. No crisis. Just a feeling. Something’s off. You can’t shake it",
            Choice1 = "Trust the feeling. Adjust the game plan.",
            Choice2 = "Ignore it. Routine is everything",
            Modifier = 10,
            eventType = EventType.TeamMeeting

        });
        eventOptions.Add(new EventOption
        {

            Index = 6,
            Description = "Reporter: The fans are... passionate this year. How much do their eyes weigh on you?",
            Choice1 = "We feel it. That energy drives us every night.",
            Choice2 = "We stay focused inside the lines. Everything else is just noise.",
            Modifier = 10,
            eventType = EventType.TeamMeeting

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
        /*
        if (filteredEvents.Count < 2)
        {
            event1 = filteredEvents.Count > 0 ? filteredEvents[0] : null;
            event2 = null;
            return;
        }
        */

        int firstIndex = Random.Range(0, filteredEvents.Count);
        int secondIndex;
        /*
        do
        {
            secondIndex = Random.Range(0, filteredEvents.Count);
        } while (secondIndex == firstIndex);
        */
        //event1 = filteredEvents[firstIndex];
        event1 = filteredEvents[Random.Range(0, filteredEvents.Count)];
        print(event1.Index + " This is the index of the event chosen");
        //event2 = filteredEvents[secondIndex];
        //CREATE THE BUTTONS
        Transform EventsButtonsTransform = GameObject.Find("ButtonsEvent").transform;
        TextMeshProUGUI eventTypeText = GameObject.Find("EventTypeText").GetComponent<TextMeshProUGUI>();
        eventTypeText.text = type.ToString();
        EventsButtonsTransform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => AddOnClick(event1, 0));
        EventsButtonsTransform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => AddOnClick(event1, 1));
        EventsButtonsTransform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = event1.Choice1;
        EventsButtonsTransform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = event1.Choice2;
    }

    //Buttons
    public void AddOnClick(EventOption eventOption, int indexOfChoice)
    {
        activeOption = eventOption;
        canStartANewWeek = false;

        //print(_playerTeam._equipmentList[2].Name + " " + _playerTeam._equipmentList[0].Level);
        _playerTeam = gameManager.playerTeam;
        switch (eventOption.Index)
        {
            case 0:
                print(eventOption.Index + " " + eventOption.Description);
                if(indexOfChoice == 0)
                {
                    _playerTeam._equipmentList[0].Level++;
                }
                else
                {
                    _playerTeam._equipmentList[1].Level++;
                }
                break;
            case 1:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    _playerTeam._equipmentList[2].Level++;
                }
                else
                {
                    _playerTeam._equipmentList[3].Level++;
                }
                break; 
            case 2:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    _playerTeam.Moral+= eventOption.Modifier;
                }
                else
                {
                    _playerTeam.FansSupportPoints += eventOption.Modifier;
                }
                break;
            case 3:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    _playerTeam.FrontOfficePoints += eventOption.Modifier;
                }
                else
                {
                    _playerTeam.Moral += eventOption.Modifier;
                }
                break;
            case 4:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    _playerTeam.FrontOfficePoints += eventOption.Modifier;
                }
                else
                {
                    _playerTeam.Moral += eventOption.Modifier;
                }
                break;
            case 5:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    _playerTeam.FrontOfficePoints += eventOption.Modifier;
                }
                else
                {
                    _playerTeam.Moral += eventOption.Modifier;
                }
                break;
            case 6:
                print(eventOption.Index + " " + eventOption.Description);
                if (indexOfChoice == 0)
                {
                    _playerTeam.FansSupportPoints += eventOption.Modifier;
                }
                else
                {
                    _playerTeam.FrontOfficePoints += eventOption.Modifier;
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
}
[System.Serializable]
public class EventOption
{
    public int Index;//Number of the event
    public string Description; // The text for the event option
    public string Choice1;
    public string Choice2;
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

