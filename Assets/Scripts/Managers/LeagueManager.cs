using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeagueManager : MonoBehaviour
{

    public int Week = 0;

    public List<EventOption> eventOptions = new List<EventOption>();
    public EventOption activeOption;
    public bool canGenerateEvents = true;

    public GameObject Test;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canGenerateEvents == true && gameManager.mode == GameManager.GameMode.TeamManagement)
        {
            NewWeek();
        }
    }
    void NewWeek()
    {
        Week++;
        //Test = GameObject.Find("NewTest");
        
        if (GameObject.Find("Choices"))
        {
            CreateEventsForWeek();
            /*
            //CHANGE TO SWITVH LATER ON!!!!!!!!!!!!!!!!!!!!!!!!
            if(Week == 1)
            {
                uiManager.SetChoiceText("Meet the players");
            }
            else if (Week == 2)
            {
                uiManager.SetChoiceText("After your first game, we should plan our next game and talk to our players");
                eventOptions.Add(new EventOption { Description = "We must continue with our effort", Modifier = Random.Range(1, 5) });
                eventOptions.Add(new EventOption { Description = "Lets pratcice our gameplan again", Modifier = Random.Range(1, 5) });
                eventOptions.Add(new EventOption { Description = "Discipline and hard work are the focus.", Modifier = Random.Range(1, 5) });
                eventOptions.Add(new EventOption { Description = "Power of friendship is the key guys", Modifier = Random.Range(1, 5) });
                eventOptions.Add(new EventOption { Description = "This league will be yours!", Modifier = Random.Range(1, 5) });
            }
            else if (Week == 3)
            {
                eventOptions.Add(new EventOption { Description = "We have to win at any cost!", Modifier = Random.Range(1, 5) });
                eventOptions.Add(new EventOption { Description = "We must manage expectations.", Modifier = Random.Range(1, 5) });
                eventOptions.Add(new EventOption { Description = "Discipline and hard work are the focus.", Modifier = Random.Range(1, 5) });
                eventOptions.Add(new EventOption { Description = "Power of friendship is the key guys", Modifier = Random.Range(1, 5) });
                eventOptions.Add(new EventOption { Description = "This league will be yours!", Modifier = Random.Range(1, 5) });
            }
            
            */
            
            Transform ChoiceButtonsTransform = GameObject.Find("ChoiceButtons").transform;
            
            //List<EventOption> tempOptions = new List<EventOption>(eventOptions); // Clone the list

            int buttonCount = Mathf.Min(ChoiceButtonsTransform.childCount, 2); // Ensure no out-of-range
            /*
            for (int i = 0; i < buttonCount; i++)
            {
                EventOption eo = tempOptions[Random.Range(0, tempOptions.Count)];
                ChoiceButtonsTransform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => AddOnClick(eo));
                ChoiceButtonsTransform.GetChild(i).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = eo.Description;
                tempOptions.Remove(eo); // Remove from temp list to avoid duplicates
            }*/

            EventOption currentEvent = eventOptions[Random.Range(0, eventOptions.Count)];
            //Ajustar os botoões para que cada um tenha uma função---Pensar niisso depois!!!!!!!
            uiManager.SetChoiceText(currentEvent.Description);
            ChoiceButtonsTransform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => AddOnClick(currentEvent));
            ChoiceButtonsTransform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => AddOnClick(currentEvent));
            ChoiceButtonsTransform.GetChild(0).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.Choice1;
            ChoiceButtonsTransform.GetChild(1).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.Choice2;
            
            
            eventOptions.Clear(); // Clear only after usage
           
        }
        

        canGenerateEvents = false;
        
    }
    public void CreateEventsForWeek()
    {
        eventOptions.Add(new EventOption 
        { 
            Index = 0, Description = "Since you are new here i think it's time to set the tone for the season. What kind of team do you want us to be?",
            Choice1 = "Fast Pace and High Scoring", 
            Choice2 = "Defense,Defense, Defense!",
            Modifier = Random.Range(1, 5), 
            eventType = EventType.Interview 
        });
        eventOptions.Add(new EventOption
        {
            Index = 1,
            Description = "Coach, One player from the opposing team seems to have accidentally part of their gameplan against us this week.It could be a trick, how do you approach next game?",
            Choice1 = "Lets ajust accordingly",
            Choice2 = "Stick to out gameplan!",
            Modifier = Random.Range(1, 5),
            eventType = EventType.Interview
        });
    }
    public void AddOnClick(EventOption eventOption)
    {
        activeOption = eventOption;
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

