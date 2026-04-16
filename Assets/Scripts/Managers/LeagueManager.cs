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
    public EventType _choseEventType;
    #endregion

    public bool canTrade = true;

    public bool canTrain = true;

    public bool canNegociateContract = true;
    //Tutorial
    public bool CanStartTutorial = true;

    public bool isGameOver = false;
    public bool CanStartANewRun = true;

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

    //leagueProgession
    [Header("LeagueProgression")]
    public bool CanDraftlvl1 = false;
    public bool CanDraftlvl2 = false;
    public bool CanDraftlvl3 = false;
    public bool CanDrafSpPlayer0 = false;
    public bool CanDraftSpPlayer1 = false;
    public bool CanDraftSpPlayer2 = false;
    public bool CanDraftSpPlayer3 = false;
    public bool CanDraftSpPlayer4 = false;
    public bool FacilitieBonus0 = false;
    public bool FacilitieBonus1 = false;
    //legacyOptions
    public bool isOnDraftLVL0 = false;
    public bool isOnDraftLVL1 = false;
    public bool isOnDraftLVL2 = false;


    public bool TriggerWeek = false;
    public bool WeekFullyInitialized = false;


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
        ///Else will be about the play off
        /*
        if (!canGenerateEvents || gameManager == null) return;

        if (gameManager.mode == GameManager.GameMode.TeamManagement)
        {
            NewWeek();
        }
        */
        /*
        if(TriggerWeek == true)
        {
            
            if (!canGenerateEvents || gameManager == null)
                return;

            // Evita chamar NewWeek() logo após um reset completo
            if (CanStartANewRun == true)
                return;

            if (gameManager.mode == GameManager.GameMode.TeamManagement)
            {
                NewWeek();
                //HandleFreeAgents();
            }
        }
        else
        {
            if (gameManager.mode == GameManager.GameMode.TeamManagement)WeekFullyInitialized = true;

        }
        
        */
    }
    public void IncreaseWeek()
    {
        Week++;
        
    }
    public void NewWeek()
    {
        /*
        TeamManagerUI teamManagerUI = GameObject.Find("TeamManagerUI")?.GetComponent<TeamManagerUI>();
        //Week++;
        //Create a IF to very the week

        if (gameManager.mode == GameManager.GameMode.TeamManagement)
        {
            if(canStartANewWeek == true && CanStartANewRun == false)
            {
                canTrain = true;
                canTrade = true;
                canNegociateContract = true;
                CreateEventsForWeek();
                //HandleFreeAgents();
                WeekFullyInitialized = true;
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
                //ImageIcon1
                Image eventImage0 = GameObject.Find("ImageIcon1").GetComponent<Image>();
                Image eventImage1 = GameObject.Find("ImageIcon0").GetComponent<Image>();
                SetEventImage(randomEvent0.ToString(), eventImage0);
                SetEventImage(randomEvent1.ToString(), eventImage1);

                

            }
            else
            {
                print("not ativate");
            }
        }

        //TeamManagerUI teamManagerUI = GameObject.Find("TeamManagerUI")?.GetComponent<TeamManagerUI>();
        if (teamManagerUI != null)
        {
            StartCoroutine(teamManagerUI.EventTypePanel());
        }
        else
        {
            Debug.LogWarning("TeamManagerUI não encontrado. Provavelmente após reset da run.");
        }
        canGenerateEvents = false;
        WeekFullyInitialized = true;
        */
        Debug.Log("[NewWeek] === INÍCIO DA FUNÇÃO ===");

        if (gameManager.mode != GameManager.GameMode.TeamManagement)
        {
            Debug.LogWarning("[NewWeek] Modo não é TeamManagement.");
            return;
        }

        if (canStartANewWeek == false || CanStartANewRun == true)
        {
            Debug.LogWarning("[NewWeek] Condições impediram execução.");
            return;
        }

        Debug.Log("[NewWeek] Condições OK - Criando eventos...");

        canTrain = true;
        canTrade = true;
        canNegociateContract = true;

        CreateEventsForWeek();
        Debug.Log($"[NewWeek] CreateEventsForWeek() concluído. {eventOptions.Count} eventos.");

        // Configuração dos botões
        Transform ChoiceButtonsTransform = GameObject.Find("ChoiceButtons")?.transform;
        if (ChoiceButtonsTransform != null)
        {
            EventType randomEvent0 = (EventType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
            EventType randomEvent1;
            do
            {
                randomEvent1 = (EventType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
            }
            while (randomEvent1 == randomEvent0);

            ChoiceButtonsTransform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ChooseEventTypeOnClick(randomEvent0));
            ChoiceButtonsTransform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ChooseEventTypeOnClick(randomEvent1));

            ChoiceButtonsTransform.GetChild(0).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = randomEvent0.ToString();
            ChoiceButtonsTransform.GetChild(1).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = randomEvent1.ToString();

            Image eventImage0 = GameObject.Find("ImageIcon1")?.GetComponent<Image>();
            Image eventImage1 = GameObject.Find("ImageIcon0")?.GetComponent<Image>();

            if (eventImage0 != null) SetEventImage(randomEvent0.ToString(), eventImage0);
            if (eventImage1 != null) SetEventImage(randomEvent1.ToString(), eventImage1);

            Debug.Log("[NewWeek] Botões de eventos configurados com sucesso.");
        }
        else
        {
            Debug.LogError("[NewWeek] ERRO: ChoiceButtonsTransform é NULL!");
        }

        WeekFullyInitialized = true;
        canGenerateEvents = false;

        Debug.Log("[NewWeek] Setup concluído. Chamando animação...");

        // === ANIMAÇÃO DO PAINEL (parte que estava falhando) ===
        TeamManagerUI teamManagerUI = GameObject.Find("TeamManagerUI")?.GetComponent<TeamManagerUI>();
        if (teamManagerUI != null)
        {
            Debug.Log("[NewWeek] TeamManagerUI encontrado. Iniciando EventTypePanel...");
            StartCoroutine(teamManagerUI.EventTypePanel());
        }
        else
        {
            Debug.LogError("[NewWeek] ERRO CRÍTICO: TeamManagerUI não encontrado! Animação não será chamada.");
        }

    }
    public void CreateEventsForWeek()
    {
        eventOptions.Add(new EventOption 
        { 
            Index = 0, Description = "Boss, local TV wants a quick hit before the match. Respond in a more humorous way or straight tactics?",
            Choice1 = "Fun & Relatable <color=#FFD700>Marketing +1</color>",
            Choice2 = "Pro & Tactical <color=#FFD700>Office +1</color>",
            btnIndex0 = 3,
            btnIndex1 = 0,
            Modifier = 1,
            eventType = EventType.Interview 
        });
        eventOptions.Add(new EventOption
        {
            Index = 1,
            Description = "City council just emailed a one-time youth grant. Take it and shout it out, or funnel it to the stadium?",
            Choice1 = "Accept & Promote Locally <color=#FFD700>Marketing +1</color>",
            Choice2 = "Redirect to Facility Upgrade <color=#FFD700>Arena +1</color>",
            btnIndex0 = 3,
            btnIndex1 = 1,
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
            btnIndex0 = 5,
            btnIndex1 = 1,//MUDAR QUNADO RECEBER O MEDCARA BTN
            eventType = EventType.TeamMeeting


        }) ;
        eventOptions.Add(new EventOption 
        {

            Index = 3,
            Description = "Energy drink brand on the line—standard cash deal or premium with player product?",
            Choice1 = "Standard Deal + Cash <color=#FFD700>Finances +1</color>",
            Choice2= "Premium Deal + Product for Players <color=#FFD700>Team Equipment +1</color>",
            Modifier = 1,
            btnIndex0 = 0,//MUDA PARA BOTÃO DE FINANÇAS
            btnIndex1 = 5,
            eventType = EventType.Sponsor
        
        });
        eventOptions.Add(new EventOption
        {

            Index = 4,
            Description = "Vox media in a podcast is asking what the team needs most. Med staff or fan support?",
            Choice1 = "We need better medical support <color=#FFD700>MedCare +1</color>",
            Choice2 = "Fan experience comes first <color=#FFD700>Arena +1</color>",
            Modifier = 1,
            btnIndex0 = 0,// MED CARA BTN
            btnIndex1 = 0,//ARENA 
            eventType = EventType.Interview

        });
        eventOptions.Add(new EventOption
        {

            Index = 5,
            Description = "Helix cell is offering bulk gear discount—expires today. Stock up or lock in future credit?",
            Choice1 = "Stock Up Now <color=#FFD700>Team Equipment +1</color>",
            Choice2 = "Negotiate for Future Credit <color=#FFD700>Finances +1</color>",
            Modifier = 1,
            btnIndex0 = 5,
            btnIndex1 = 0,// MUDAR BTN FINANCES
            eventType = EventType.Sponsor

        });
        eventOptions.Add(new EventOption
        {

            Index = 6,
            Description = "Bank wants to fund a new VIP area. Luxury skybox or family zone?",
            Choice1 = "Luxury Skybox <color=#FFD700>Arena +1</color>",
            Choice2 = "Family Zone with Kids Area <color=#FFD700>Marketing +1</color>",
            Modifier = 1,
            btnIndex0 = 1,// MUDA PARA BTN ARENA
            btnIndex1 = 3,
            eventType = EventType.TeamMeeting

        });
        eventOptions.Add(new EventOption
        {

            Index = 7,
            Description = "Maintenance found extra space under the stands. Physio suite or merch shop?",
            Choice1 = "Convert to Physio Suite <color=#FFD700>MedCare +1</color>",
            Choice2 = "Add Merch Pop-Up Shop <color=#FFD700>Marketing +1</color>",
            Modifier = 1,
            btnIndex0 = 1,// MUDAR BTN MEDCARE
            btnIndex1 = 3,
            eventType = EventType.ReplyToEmails

        });
        eventOptions.Add(new EventOption
        {

            Index = 8,
            Description = "HyperMetrics XY is offering free analytics—if we upgrade servers. Take it or sell access?",
            Choice1 = "Take the Suite + Upgrade Servers <color=#FFD700>Office +1</color>",
            Choice2 = "Sell the Suite Access to League <color=#FFD700>Finances +1</color>",
            Modifier = 1,
            btnIndex0 = 0,
            btnIndex1 = 3,// MUDAR BTN FINANCIAS
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
    public void SetEventImage(string eventName, Image eventImage)
    {
        if (eventImage == null)
        {
            Debug.LogWarning("eventImage é null! Não é possível aplicar a sprite.");
            return;
        }

        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("EventName está vazio!");
            eventImage.sprite = null;
            return;
        }

        // Caminho dentro da pasta Resources
        string path = "2D/Events/Events/" + eventName;

        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite != null)
        {
            eventImage.sprite = sprite;
            Debug.Log($"Imagem aplicada com sucesso: {eventName}");
        }
        else
        {
            Debug.LogWarning($"Imagem não encontrada para o evento: {eventName} | Caminho: {path}");
            eventImage.sprite = null;   // ou pode colocar uma imagem padrão aqui
        }
    }
    //Buttons
    public void AddOnClick(EventOption eventOption, int indexOfChoice)
    {
        EnsurePlayerTeamIsSet();
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
        gameManager.saveSystem.SaveLeague();
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            //gameManager.saveSystem.SaveTeam(gameManager.leagueTeams[i]);
            gameManager.saveSystem.SaveTeamInfo(gameManager.leagueTeams[i]);
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
    //Reset
    public void ResetLeagueHistoryMode()
    {
        if (gameManager.leagueTeams.Count > 0)
        {
            for (int i = 0; i < gameManager.leagueTeams.Count; i++)
            {
                gameManager.leagueTeams[i].isChampion = false;
            }
        }
        Week = 0;
        canTrade = true;
        canTrain = true;
        

    }
    private void EnsurePlayerTeamIsSet()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager == null) return;
        }

        if (gameManager.playerTeam != null && gameManager.playerTeam.IsPlayerTeam)
            return; // Já está correto

        // Tenta encontrar o time do jogador
        foreach (Team team in gameManager.leagueTeams)
        {
            if (team.IsPlayerTeam)
            {
                gameManager.playerTeam = team;
                _playerTeam = team;
                Debug.Log($"[LeagueManager] playerTeam definido corretamente: {team.TeamName}");
                return;
            }
        }

        Debug.LogError("[LeagueManager] Não foi possível encontrar o time do jogador!");
    }
    public void HandleFreeAgents()
    {
        bool canGenerate = false;
        TeamManagerUI teamManagerUI = GameObject.Find("TeamManagerUI")?.GetComponent<TeamManagerUI>();
        if (teamManagerUI == null)
        {
            Debug.LogWarning("[LeagueManager] TeamManagerUI não encontrado para HandleFreeAgents.");
            return;
        }

        teamManagerUI._freeAgents_panel.SetActive(false);   // Note: você vai precisar declarar essa variável no LeagueManager ou acessar via teamManagerUI

        if (gameManager.playerTeam != null)
        {
            // 1. Remove contratos expirados e destrói os objetos
            teamManagerUI.freeAgentManager.RemoveExpiredContracts(gameManager.playerTeam);

            // 2. Se o roster ficou abaixo de 8, força geração de EXATAMENTE 8 novos jogadores
            if (gameManager.playerTeam.playersListRoster.Count < 8)
            {
                canGenerate = true;
                teamManagerUI.canProgressWithWeek = false;
                teamManagerUI._freeAgents_panel.SetActive(true);

                if (canGenerate)
                {
                    // SEMPRE gera 8 jogadores novos, independentemente de quantos expiraram
                    teamManagerUI.freeAgentManager.GeneratePlayers(8);
                    teamManagerUI._freeAgents_panel.SetActive(true);
                }
                canGenerate = false;

                Debug.Log($"Free Agents: Gerados 8 jogadores novos. Roster atual: {gameManager.playerTeam.playersListRoster.Count}");

                teamManagerUI.StartCoroutine(teamManagerUI.ProgressWithWeek());
                //teamManagerUI.UpdateTeamSalary();
            }
            else
            {
                teamManagerUI.canProgressWithWeek = true;
                teamManagerUI.UpdateTeamSalary();
            }

            //teamManagerUI.UpdateTeamSalary();
            teamManagerUI.CallWarning();
        }
        else
        {
            Debug.LogWarning("playerTeam é null durante HandleFreeAgents no LeagueManager.");
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

