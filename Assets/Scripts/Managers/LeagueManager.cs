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
        eventOptions.Add(new EventOption { Description = "We have to win at any cost!", Modifier = Random.Range(1,5)});
        eventOptions.Add(new EventOption { Description = "We must manage expectations.", Modifier = Random.Range(1, 5) });
        eventOptions.Add(new EventOption {Description = "Discipline and hard work are the focus.", Modifier = Random.Range(1, 5) });
        eventOptions.Add(new EventOption { Description = "Power of friendship is the key guys", Modifier = Random.Range(1, 5) });
        eventOptions.Add(new EventOption { Description = "This league will be yours!", Modifier = Random.Range(1, 5) });

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
            

            Transform ChoiceButtonsTransform = GameObject.Find("ChoiceButtons").transform;
            List<EventOption> tempOptions = new List<EventOption>(eventOptions); // Clone the list

            int buttonCount = Mathf.Min(ChoiceButtonsTransform.childCount, tempOptions.Count); // Ensure no out-of-range

            for (int i = 0; i < buttonCount; i++)
            {
                EventOption eo = tempOptions[Random.Range(0, tempOptions.Count)];
                ChoiceButtonsTransform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => AddOnClick(eo));
                ChoiceButtonsTransform.GetChild(i).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = eo.Description;
                tempOptions.Remove(eo); // Remove from temp list to avoid duplicates
            }

            eventOptions.Clear(); // Clear only after usage
           
        }
        
        canGenerateEvents = false;
        
    }
    public void AddOnClick(EventOption eventOption)
    {
        activeOption = eventOption;
    }
}
[System.Serializable]
public class EventOption
{
    public string Description; // The text for the event option
    public int Modifier; // Example effect: morale boost or drop
    
}

