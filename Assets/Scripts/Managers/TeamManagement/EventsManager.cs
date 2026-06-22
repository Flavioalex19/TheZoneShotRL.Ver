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

    Player playerSelected;

    void Awake()
    {
        leagueManager = FindFirstObjectByType<LeagueManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        teamManagerUI = FindFirstObjectByType<TeamManagerUI>();
        playerSelected = GetRandomPlayer();
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
            Description = "Boss, ZenithFlux wants to sponsor the team. " +
            "They're offering either a big cash injection into the facility or a heavy marketing campaign.",
            Choice1 = "Structure the project financially - <color=#FFD700>Finances lvl Up +1</color>",
            Choice2 = "Start Marketing Campaing - <color=#FFD700>Office +1</color>",
            btnIndex0 = 3,
            btnIndex1 = 0,
            Modifier = 1,
            eventType = EventType.Sponsor
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 1,
            Description = "Helix-Cell brand wants to partner with us. They can supply state-of-the-art training equipment or offer a specific training plan for the team",
            Choice1 = "Upgrade the gear - <color=#FFD700>Team Equipment Lvl Up +1</color>",
            Choice2 = "Special Trainning Session - <color=#FFD700>Player Match bonus +5% </color>" + playerSelected.playerLastName,
            btnIndex0 = 3,
            btnIndex1 = 1,
            Modifier = 1,
            eventType = EventType.Sponsor
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 2,
            Modifier = 1,
            Description = "X-Zest is open to a bigger package. " +
            "We can lock in a brand new gear or go heavy on marketing hype plus on-court performance bonuses.",
            Choice1 = "Smart Lockers & Charging Stations <color=#FFD700>Team Equipment lvl up</color>",
            Choice2 = "Better marketing is always a good idea <color=#FFD700>Marketing lvl up</color>",
            btnIndex0 = 5,
            btnIndex1 = 1,
            eventType = EventType.Sponsor
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 3,
            Description = "Vox Media: I’d like to know if the temperament of one of your players might be interfering with your planning.",
            Choice1 = "Sometimes a player struggles to make adjustments; nothing beats a conversation to sort things out. - " +
            "<color=#FFD700>Change Personality: </color>" + playerSelected.playerLastName + " " + playerSelected.SetOVR().ToString(),
            Choice2 = "Handle the situation politely and protect the player - <color=#FFD700> Shooting Bonus for next match + 5% </color>",
            Modifier = 1,
            btnIndex0 = 0,
            btnIndex1 = 5,
            eventType = EventType.Interview
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 4,
            Description = "Vox media: You have an important game this week; how are you and the team handling it?",
            Choice1 = "Day-to-day, staying focused on our work <color=#FFD700>Team Buff (Shooting)</color>",
            Choice2 = "Focus on the fundamentals <color=#FFD700>Match buff(Shoot or Pass or Juke)</color>",
            Modifier = 1,
            btnIndex0 = 0,
            btnIndex1 = 0,
            eventType = EventType.Interview
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 5,
            Description = "Weekly team meeting. There's some tension in the locker room. We can address the conflicts or run a focused tactical session.",
            Choice1 = "Deal with problematic player - <color=#FFD700>Change player personality </color>" + playerSelected.playerLastName + " Ovr:" +playerSelected.SetOVR(),
            Choice2 = "Run tactical session <color=#FFD700>Train Player </color>" + playerSelected.playerLastName + " Ovr:" + playerSelected.SetOVR(),
            Modifier = 1,
            btnIndex0 = 5,
            btnIndex1 = 0,
            eventType = EventType.Sponsor
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 6,
            Description = "Internal meeting to improve how the team works. We can focus on better organization or direct player development.",
            Choice1 = "Improve organization <color=#FFD700>Office Level Up</color>",
            Choice2 = "Focus on player development <color=#FFD700>Stats Level Up</color>",
            btnIndex0 = 1,
            btnIndex1 = 0,
            Modifier = 1,
            eventType = EventType.TeamMeeting
        });

        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 7,
            Description = "R&D finished new prototypes. We can implement new training equipment or new training methods.",
            Choice1 = "Implement new equipment <color=#FFD700>Equips Level Up</color>",
            Choice2 = "Adopt new training methods <color=#FFD700>Stats Level Up</color>",
            btnIndex0 = 0,
            btnIndex1 = 1,
            Modifier = 1,
            eventType = EventType.RnD
        });

        // R&D 8
        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 8,
            Description = "Research on in-game performance is ready. We can create performance routines or focus on specific technical improvements.",
            Choice1 = "Create performance routines <color=#FFD700>Player Buff</color>",
            Choice2 = "Improve shooting or joke or passing <color=#FFD700>Match Bonus (Shooting/Joke/Pass)</color>",
            btnIndex0 = 1,
            btnIndex1 = 0,
            Modifier = 1,
            eventType = EventType.RnD
        });
        // Operations 9
        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 9,
            Description = "Daily operations meeting. We can focus on financial efficiency and administration or strengthen medical protocols.",
            Choice1 = "Improve financial efficiency <color=#FFD700>Finances Level Up</color>",
            Choice2 = "Strengthen medical protocols <color=#FFD700>Medicare Level Up</color>",
            btnIndex0 = 1,
            btnIndex1 = 0,
            Modifier = 1,
            eventType = EventType.Operations
        });

        // Operations 10
        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 10,
            Description = "Operational review. We need to decide between improving office organization or focusing on arena maintenance.",
            Choice1 = "Improve office organization <color=#FFD700>Office Level Up</color>",
            Choice2 = "Focus on arena maintenance <color=#FFD700>Arena Level Up</color>",
            btnIndex0 = 0,
            btnIndex1 = 1,
            Modifier = 1,
            eventType = EventType.Operations
        });
        // Networking 11
        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 11,
            Description = "Networking event with other GMs. We can gather some interesting infos about other teams.",
            Choice1 = "Focus on your rival, to seek any breaches <color=#FFD700>vsRival Bonus++</color>",
            Choice2 = "Focus on the meeting to improve the team <color=#FFD700>Team Bonus</color>",
            btnIndex0 = 0,
            btnIndex1 = 1,
            Modifier = 1,
            eventType = EventType.Networking
        });

        // Networking 12
        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 12,
            Description = "Networking with media and influencers. We can work on the team's image or generate extra marketing momentum.",
            Choice1 = "Work on team image <color=#FFD700>Change Personality</color>",
            Choice2 = "Generate marketing momentum <color=#FFD700>Marketing Level Up</color>",
            btnIndex0 = 2,
            btnIndex1 = 3,
            Modifier = 1,
            eventType = EventType.Networking
        });
        // Email 13
        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 13,
            Description = "You received an email from an agent and the board. It can give a quick training or be used to traing new sets.",
            Choice1 = "Intense training <color=#FFD700>Match Buff</color>",
            Choice2 = "Train a new set <color=#FFD700>TeamBuff</color>",
            btnIndex0 = 2,
            btnIndex1 = 1,
            Modifier = 1,
            eventType = EventType.Emails
        });

        // Email 14
        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 14,
            Description = "Email with feedback from fans and media. We can give light personality feedback or a small marketing push.",
            Choice1 = "Light personality feedback <color=#FFD700>Light Personality Change</color>",
            Choice2 = "Small marketing push <color=#FFD700>Small Marketing Level Up</color>",
            btnIndex0 = 26,
            btnIndex1 = 27,
            Modifier = 1,
            eventType = EventType.Emails
        });
        // Structure 15
        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 15,
            Description = "Long-term strategic meeting. We can invest in player development or improve office organization.",
            Choice1 = "Invest in player development <color=#FFD700>Long-term Stats Growth</color>",
            Choice2 = "Improve office organization <color=#FFD700>Office Level Up</color>",
            btnIndex0 = 28,
            btnIndex1 = 29,
            Modifier = 1,
            eventType = EventType.Structure
        });

        // Structure 16
        leagueManager.eventOptions.Add(new EventOption
        {
            Index = 16,
            Description = "Long-term planning. We need to choose between expanding the arena or strengthening financial foundations.",
            Choice1 = "Expand the arena <color=#FFD700>Arena Level Up</color>",
            Choice2 = "Strengthen long-term finances <color=#FFD700>Finances Level Up</color>",
            btnIndex0 = 30,
            btnIndex1 = 31,
            Modifier = 1,
            eventType = EventType.Structure
        });

        //Debug.Log($"[EventsManager] {leagueManager.eventOptions.Count} eventos criados.");
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
                if (indexOfChoice == 0) gameManager.playerTeam.FinancesLvl++;
                else gameManager.playerTeam.OfficeLvl++;
                break;
            case 1:
                if (indexOfChoice == 0) gameManager.playerTeam.TeamEquipmentLvl++;
                else PlayerBuff(playerSelected, 5);
                break;
            case 2:
                if (indexOfChoice == 0) gameManager.playerTeam.TeamEquipmentLvl++;
                else gameManager.playerTeam.MarketingLvl++;
                break;
            case 3:
                if (indexOfChoice == 0) ChangePlayerPersonality(playerSelected);
                else PlayerBuff(playerSelected, 5);
                break;
            case 4:
                if (indexOfChoice == 0) TeamBuff();
                else MatchBuff(5);
                break;
            case 5:
                if (indexOfChoice == 0) ChangePlayerPersonality(playerSelected);
                else PlayerBuff(playerSelected, 5);
                break;
            case 6:
                if (indexOfChoice == 0) gameManager.playerTeam.ArenaLvl++;
                else gameManager.playerTeam.MarketingLvl++;
                break;
            case 7:
                if (indexOfChoice == 0) gameManager.playerTeam.TeamEquipmentLvl++;
                else LevelUpPlayer(playerSelected);
                break;
            case 8:
                if (indexOfChoice == 0) PlayerBuff(playerSelected,5);
                else MatchBuff(5);
                break;
            case 9:
                if (indexOfChoice == 0) gameManager.playerTeam.FinancesLvl++;
                else gameManager.playerTeam.MedicalLvl++;
                break; 
            case 10:
                if (indexOfChoice == 0) gameManager.playerTeam.OfficeLvl++;
                else gameManager.playerTeam.ArenaLvl++;
                break;
            case 11:
                if (indexOfChoice == 0) VsRivals();
                else TeamBuff();
                break;
            case 12:
                if (indexOfChoice == 0) ChangePlayerPersonality(playerSelected);
                else gameManager.playerTeam.MarketingLvl++;
                break; 
            case 13:
                if (indexOfChoice == 0)MatchBuff(5);
                else TeamBuff();
                break;
            case 14:
                if (indexOfChoice == 0) gameManager.playerTeam.OfficeLvl++;
                else gameManager.playerTeam.FinancesLvl++;
                break;
            case 15:
                if (indexOfChoice == 0) gameManager.playerTeam.OfficeLvl++;
                else gameManager.playerTeam.FinancesLvl++;
                break;
            case 16:
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
    public string ChangePlayerPersonality(Player player)
    {
        if (player == null)
        {
            Debug.LogWarning("[GameManager] Não foi possível alterar personalidade: player é nulo.");
            return "";
        }

        // ====================== LÓGICA DE ALTERAÇÃO DE PERSONALIDADE ======================
        if (player.Personality == 1)
        {
            // Personalidade 1 só pode aumentar
            player.Personality = 2;
        }
        else if (player.Personality == 5)
        {
            // Personalidade 5 volta para 1 (ciclo)
            player.Personality = 1;
        }
        else
        {
            // Para personalidades 2, 3 e 4: aumenta ou diminui aleatoriamente
            if (UnityEngine.Random.value < 0.5f)
            {
                player.Personality += 1;
            }
            else
            {
                player.Personality -= 1;
            }
        }
        // ================================================================================

        string fullName = $"{player.playerFirstName} {player.playerLastName}";

        Debug.Log($"[GameManager] Personalidade de {fullName} alterada para {player.Personality}");

        return fullName;
    }
    public Player GetRandomPlayer()
    {
        if (gameManager.playerTeam == null || gameManager.playerTeam.playersListRoster == null || gameManager.playerTeam.playersListRoster.Count == 0)
            return null;

        int randomIndex = UnityEngine.Random.Range(0, gameManager.playerTeam.playersListRoster.Count);
        return gameManager.playerTeam.playersListRoster[randomIndex];
    }
    public void PlayerBuff(Player player, int value)
    {
        player.MatchBuff = value;
    }
    public void TeamBuff()
    {
        //gameManager.playerTeam.
    }
    public void MatchBuff(int value)
    {
        int index = 0;
        index = (int)UnityEngine.Random.Range(0,3);
        switch (index) 
        {
            case 0:
                gameManager.playerTeam.bonus_Shoot = value;
                break; 
            case 1:
                gameManager.playerTeam.bonus_Juke = value;
                break;
            case 2:
                gameManager.playerTeam.bonus_Pass = value;
                break;
            case 3:
                gameManager.playerTeam.bonus_Defense = value;
                break;
                

        }
    }
    public void LevelUpPlayer(Player player)
    {
        if (player == null)
        {
            Debug.LogWarning("LevelUpPlayer: Player é nulo!");
            return;
        }

        // Aumenta todos os atributos em +1 (limitando em 99)
        player.Awareness = Mathf.Min(player.Awareness + 1, 99);
        player.Juking = Mathf.Min(player.Juking + 1, 99);
        player.Control = Mathf.Min(player.Control + 1, 99);
        player.Consistency = Mathf.Min(player.Consistency + 1, 99);

        player.Shooting = Mathf.Min(player.Shooting + 1, 99);
        player.Inside = Mathf.Min(player.Inside + 1, 99);
        player.Mid = Mathf.Min(player.Mid + 1, 99);
        player.Outside = Mathf.Min(player.Outside + 1, 99);

        player.Defending = Mathf.Min(player.Defending + 1, 99);
        player.Stealing = Mathf.Min(player.Stealing + 1, 99);
        player.Guarding = Mathf.Min(player.Guarding + 1, 99);

        // Atualiza o OVR do jogador após o level up
        player.UpdateOVR();

        Debug.Log($"[Level Up] {player.playerFirstName} {player.playerLastName} recebeu +1 em todos os atributos.");
    }
    void VsRivals()
    {

    }
}
