using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EventsManager : MonoBehaviour
{
    private LeagueManager leagueManager;
    private GameManager gameManager;
    private TeamManagerUI teamManagerUI;

    [Header("Referências UI - Arraste no Inspector")]
    public Transform ChoiceButtonsTransform;        // Painel com os 2 botões de tipo de evento
    public Transform EventChoiceButtonsTransform;   // Painel com os 2 botões de escolha (Choice1 / Choice2)

    public Image eventImage0;   // ImageIcon1
    public Image eventImage1;   // ImageIcon0

    void Awake()
    {
        leagueManager = FindFirstObjectByType<LeagueManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        teamManagerUI = FindFirstObjectByType<TeamManagerUI>();
    }

    // ====================== FUNÇÃO PRINCIPAL (chamada pelo TeamManagerUI) ======================
    public void StartNewWeekEvents()
    {
        if (leagueManager == null || gameManager == null)
        {
            Debug.LogError("[EventsManager] LeagueManager ou GameManager não encontrado!");
            return;
        }

        Debug.Log("[EventsManager] Iniciando Nova Semana de Eventos...");

        leagueManager.canTrain = true;
        leagueManager.canTrade = true;
        leagueManager.canNegociateContract = true;

        CreateEventsForWeek();
        SetupEventTypeButtons();

        leagueManager.WeekFullyInitialized = true;
        leagueManager.canGenerateEvents = false;

        Debug.Log("[EventsManager] Eventos configurados.");
        Debug.Log("[EventsManager] StartNewWeekEvents() finalizado - botões devem estar configurados");
        // ====================== CHAMADA DA ANIMAÇÃO ======================
        if (teamManagerUI != null)
        {
            Debug.Log("[EventsManager] Iniciando animação EventTypePanel...");
            StartCoroutine(teamManagerUI.EventTypePanel());
        }
        else
        {
            Debug.LogWarning("[EventsManager] TeamManagerUI não encontrado! Animação não será chamada.");
        }
    }

    // ====================== CRIA OS EVENTOS (exatamente como estava) ======================
    public void CreateEventsForWeek()
    {
        leagueManager.eventOptions.Clear();

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 0,
            Description = "Boss, local TV wants a quick hit before the match. Respond in a more humorous way or straight tactics?",
            Choice1 = "Fun & Relatable <color=#FFD700>Marketing +1</color>",
            Choice2 = "Pro & Tactical <color=#FFD700>Office +1</color>",
            btnIndex0 = 3,
            btnIndex1 = 0,
            Modifier = 1,
            eventType = EventType.Interview
        });

        leagueManager.eventOptions.Add(new EventOption
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

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 2,
            Modifier = 1,
            Description = "Players are voting on locker room upgrades. Smart tech or recovery pods?",
            Choice1 = "Smart Lockers & Charging Stations <color=#FFD700>Team Equipment +1</color>",
            Choice2 = "Better Recovery Pods <color=#FFD700>MedCare +1</color>",
            btnIndex0 = 5,
            btnIndex1 = 1,
            eventType = EventType.TeamMeeting
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 3,
            Description = "Energy drink brand on the line—standard cash deal or premium with player product?",
            Choice1 = "Standard Deal + Cash <color=#FFD700>Finances +1</color>",
            Choice2 = "Premium Deal + Product for Players <color=#FFD700>Team Equipment +1</color>",
            Modifier = 1,
            btnIndex0 = 0,
            btnIndex1 = 5,
            eventType = EventType.Sponsor
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 4,
            Description = "Vox media in a podcast is asking what the team needs most. Med staff or fan support?",
            Choice1 = "We need better medical support <color=#FFD700>MedCare +1</color>",
            Choice2 = "Fan experience comes first <color=#FFD700>Arena +1</color>",
            Modifier = 1,
            btnIndex0 = 0,
            btnIndex1 = 0,
            eventType = EventType.Interview
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 5,
            Description = "Helix cell is offering bulk gear discount—expires today. Stock up or lock in future credit?",
            Choice1 = "Stock Up Now <color=#FFD700>Team Equipment +1</color>",
            Choice2 = "Negotiate for Future Credit <color=#FFD700>Finances +1</color>",
            Modifier = 1,
            btnIndex0 = 5,
            btnIndex1 = 0,
            eventType = EventType.Sponsor
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 6,
            Description = "Bank wants to fund a new VIP area. Luxury skybox or family zone?",
            Choice1 = "Luxury Skybox <color=#FFD700>Arena +1</color>",
            Choice2 = "Family Zone with Kids Area <color=#FFD700>Marketing +1</color>",
            Modifier = 1,
            btnIndex0 = 1,
            btnIndex1 = 3,
            eventType = EventType.TeamMeeting
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 7,
            Description = "Maintenance found extra space under the stands. Physio suite or merch shop?",
            Choice1 = "Convert to Physio Suite <color=#FFD700>MedCare +1</color>",
            Choice2 = "Add Merch Pop-Up Shop <color=#FFD700>Marketing +1</color>",
            Modifier = 1,
            btnIndex0 = 1,
            btnIndex1 = 3,
            eventType = EventType.ReplyToEmails
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 8,
            Description = "HyperMetrics XY is offering free analytics—if we upgrade servers. Take it or sell access?",
            Choice1 = "Take the Suite + Upgrade Servers <color=#FFD700>Office +1</color>",
            Choice2 = "Sell the Suite Access to League <color=#FFD700>Finances +1</color>",
            Modifier = 1,
            btnIndex0 = 0,
            btnIndex1 = 3,
            eventType = EventType.Sponsor
        });

        Debug.Log($"[EventsManager] {leagueManager.eventOptions.Count} eventos criados.");
    }

    // ====================== ETAPA 1: Botões de Tipo ======================
    private void SetupEventTypeButtons()
    {
        if (ChoiceButtonsTransform == null)
        {
            Debug.LogError("[EventsManager] ChoiceButtonsTransform não atribuído!");
            return;
        }

        EventType randomEvent0 = (EventType)Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
        EventType randomEvent1;
        do
        {
            randomEvent1 = (EventType)Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
        }
        while (randomEvent1 == randomEvent0);

        Button btn0 = ChoiceButtonsTransform.GetChild(0).GetComponent<Button>();
        btn0.onClick.RemoveAllListeners();
        btn0.onClick.AddListener(() => ChooseEventTypeOnClick(randomEvent0));

        Button btn1 = ChoiceButtonsTransform.GetChild(1).GetComponent<Button>();
        btn1.onClick.RemoveAllListeners();
        btn1.onClick.AddListener(() => ChooseEventTypeOnClick(randomEvent1));

        ChoiceButtonsTransform.GetChild(0).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = randomEvent0.ToString();
        ChoiceButtonsTransform.GetChild(1).Find("Description Text").GetComponentInChildren<TextMeshProUGUI>().text = randomEvent1.ToString();

        if (eventImage0 != null) SetEventImage(randomEvent0.ToString(), eventImage0);
        if (eventImage1 != null) SetEventImage(randomEvent1.ToString(), eventImage1);

        Debug.Log("[EventsManager] Botões de tipo configurados.");
    }

    // ====================== CHAMADO AO CLICAR NO TIPO ======================
    public void ChooseEventTypeOnClick(EventType eventType)
    {
        leagueManager._choseEventType = eventType;   // mantendo a variável que você tinha
        SetNewChoiceForEvent(eventType);
    }

    // ====================== ETAPA 2: Botões de Escolha ======================
    void SetNewChoiceForEvent(EventType type)
    {
        List<EventOption> filteredEvents = new List<EventOption>();
        foreach (var e in leagueManager.eventOptions)
        {
            if (e.eventType == type)
                filteredEvents.Add(e);
        }

        if (filteredEvents.Count == 0) return;

        EventOption event1 = filteredEvents[Random.Range(0, filteredEvents.Count)];

        if (EventChoiceButtonsTransform == null) return;

        // Esconde painel de tipos e mostra painel de escolhas
        ChoiceButtonsTransform?.gameObject.SetActive(false);
        EventChoiceButtonsTransform.gameObject.SetActive(true);

        Button btn0 = EventChoiceButtonsTransform.GetChild(0).GetComponent<Button>();
        Button btn1 = EventChoiceButtonsTransform.GetChild(1).GetComponent<Button>();

        btn0.onClick.RemoveAllListeners();
        btn1.onClick.RemoveAllListeners();

        btn0.onClick.AddListener(() => AddOnClick(event1, 0));
        btn1.onClick.AddListener(() => AddOnClick(event1, 1));

        EventChoiceButtonsTransform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = event1.Choice1;
        EventChoiceButtonsTransform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = event1.Choice2;
        
        // Sprites dos botões de escolha
        List<Sprite> spriteList = new List<Sprite>(Resources.LoadAll<Sprite>("2D/UI/Team Management/WeekEventsButtons/Official"));
        if (spriteList.Count >= 2)
        {
            EventChoiceButtonsTransform.GetChild(0).GetComponent<Image>().sprite = spriteList[0];
            EventChoiceButtonsTransform.GetChild(1).GetComponent<Image>().sprite = spriteList[1];
        }

        GameObject.Find("EventText").GetComponent<TextMeshProUGUI>().text = event1.Description.ToString();
    }

    // ====================== APLICA O EFEITO (exatamente o switch que você mandou) ======================
    public void AddOnClick(EventOption eventOption, int indexOfChoice)
    {
        if (eventOption == null) return;

        // Garante que o playerTeam está setado
        if (gameManager.playerTeam == null)
        {
            Debug.LogError("[EventsManager] playerTeam é null em AddOnClick!");
            return;
        }

        switch (eventOption.Index)
        {
            case 0:
                if (indexOfChoice == 0) gameManager.playerTeam.MarketingLvl++;
                else gameManager.playerTeam.OfficeLvl++;
                break;
            case 1:
                if (indexOfChoice == 0) gameManager.playerTeam.MarketingLvl++;
                else gameManager.playerTeam.ArenaLvl++;
                break;
            case 2:
                if (indexOfChoice == 0) gameManager.playerTeam.TeamEquipmentLvl++;
                else gameManager.playerTeam.MedicalLvl++;
                break;
            case 3:
                if (indexOfChoice == 0) gameManager.playerTeam.FinancesLvl++;
                else gameManager.playerTeam.TeamEquipmentLvl++;
                break;
            case 4:
                if (indexOfChoice == 0) gameManager.playerTeam.MedicalLvl++;
                else gameManager.playerTeam.ArenaLvl++;
                break;
            case 5:
                if (indexOfChoice == 0) gameManager.playerTeam.TeamEquipmentLvl++;
                else gameManager.playerTeam.FinancesLvl++;
                break;
            case 6:
                if (indexOfChoice == 0) gameManager.playerTeam.ArenaLvl++;
                else gameManager.playerTeam.MarketingLvl++;
                break;
            case 7:
                if (indexOfChoice == 0) gameManager.playerTeam.MedicalLvl++;
                else gameManager.playerTeam.MarketingLvl++;
                break;
            case 8:
                if (indexOfChoice == 0) gameManager.playerTeam.OfficeLvl++;
                else gameManager.playerTeam.FinancesLvl++;
                break;
            default:
                break;
        }

        

        // Limpa após uso
        leagueManager.eventOptions.Clear();

        // Esconde painel de escolhas
        if (EventChoiceButtonsTransform != null)
            EventChoiceButtonsTransform.gameObject.SetActive(false);

        // Atualiza toda a UI do Headquarters após aplicar o efeito do evento
        if (teamManagerUI != null)
        {
            teamManagerUI.UpdateHeadquartersUI();
            leagueManager.HandleFreeAgents();
        }
        Debug.Log($"[EventsManager] Efeito aplicado - Index {eventOption.Index}, Escolha {indexOfChoice}");
        // Salva
        gameManager.saveSystem.SaveLeague();
        for (int i = 0; i < gameManager.leagueTeams.Count; i++)
        {
            gameManager.saveSystem.SaveTeamInfo(gameManager.leagueTeams[i]);
        }
    }

    public void SetEventImage(string eventName, Image eventImage)
    {
        if (eventImage == null)
        {
            Debug.LogWarning("eventImage é null!");
            return;
        }

        string path = "2D/Events/Events/" + eventName;
        Sprite sprite = Resources.Load<Sprite>(path);

        if (sprite != null)
        {
            eventImage.sprite = sprite;
            Debug.Log($"Imagem aplicada com sucesso: {eventName}");
        }
        else
        {
            Debug.LogWarning($"Imagem não encontrada: {eventName}");
        }
    }
}
