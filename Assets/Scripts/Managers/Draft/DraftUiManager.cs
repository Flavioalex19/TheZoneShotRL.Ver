using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DraftUiManager : MonoBehaviour
{

    GameManager _gameManager;
    LeagueManager leagueManager;
    [SerializeField]Transform _currentOnTheClockTeamArea;
    [SerializeField]Transform _playersFromOnTheClockTeamArea;
    [SerializeField] TextMeshProUGUI text_currentPlayersOnTeam;
    [SerializeField] Transform _playerBtnsAreaContent;
    [SerializeField] TextMeshProUGUI text_currentTeamSalary;

    [SerializeField] GridLayoutGroup glg_draftNames;
    /*
    [SerializeField] Team testTeam;
    Team currentTeam;

    // Start is called before the first frame update
    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gameManager.glg_draftNames = glg_draftNames;
        glg_draftNames = GameObject.Find("DraftContent").GetComponent<GridLayoutGroup>();

        int count = 0;
        if (count < 1)
        {
            _gameManager.GeneratePlayers(_gameManager.leagueTeams.Count * 8);
            count++;
        }
        //_gameManager.SortDraftButtonsByOVRCrescente();
        
        _playersFromOnTheClockTeamArea = GameObject.Find("Info_Team_Players").transform;


    }

    // Update is called once per frame
    void Update()
    {

        //_currentOnTheClockTeamArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = _gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()].TeamName.ToString();

        //print(_gameManager.leagueTeams[0].playersListRoster.Count + " Team B" + _gameManager.leagueTeams[1].playersListRoster.Count);
        
        for (int i = 0; i < _gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()].playersListRoster.Count; i++)
        {
            if (_gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()].playersListRoster.Count > 0)
            {
                _playersFromOnTheClockTeamArea.GetChild(i).GetComponent<TextMeshProUGUI>().text = 
                    _gameManager.playerTeam.playersListRoster[i].playerFirstName.ToString() +
                    " " +
                    _gameManager.playerTeam.playersListRoster[i].playerLastName.ToString(); 
                text_currentPlayersOnTeam.text = _gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()].playersListRoster.Count.ToString();
            }
        }
        
        

        if (_gameManager.playerTeam != _gameManager.leagueTeams[_gameManager.GetCurrentTeamIndex()])
        {
            Transform selectedBtn = GetRandomChild();
            InvokeButtonClick(selectedBtn.GetComponent<Button>());
        }

        
        
    }
    public Transform GetRandomChild()
    {
        if (_playerBtnsAreaContent.childCount == 0)
        {
            Debug.LogWarning("No children found on this Transform.");
            return null;
        }

        int randomIndex = Random.Range(0, _playerBtnsAreaContent.childCount);
        return _playerBtnsAreaContent.GetChild(randomIndex);
    }
    public void InvokeButtonClick(Button button)
    {
        if (button != null)
        {
            button.onClick.Invoke();
            Debug.Log("Button click invoked through code.");
        }
    }
    */
    private int currentTeamIndex = 0;
    private int totalPicksPerTeam = 8;
    private int extraPlayersForPlayer = 15;


    private void Start()
    {
        if (_gameManager == null)
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (leagueManager == null)
            leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();

        GenerateDraftPlayers();
    }

    // Gera os jogadores conforme a nova lógica
    private void GenerateDraftPlayers()
    {
        // Limpa qualquer botão antigo
        foreach (Transform child in glg_draftNames.transform)
            Destroy(child.gameObject);

        int numTeams = _gameManager.leagueTeams.Count;

        // 1. Distribui 8 jogadores automaticamente APENAS para os times que NÃO são do player
        for (int t = 0; t < numTeams; t++)
        {
            Team team = _gameManager.leagueTeams[t];

            // PROTEÇÃO IMPORTANTE: Só preenche times que NÃO são do jogador
            if (team.IsPlayerTeam)
                continue;   // Pula o time do player

            // Limpa roster antes de preencher (segurança)
            team.playersListRoster.Clear();

            for (int i = 0; i < totalPicksPerTeam; i++)   // totalPicksPerTeam = 8
            {
                Player newPlayer = CreateRandomPlayer();
                team.playersListRoster.Add(newPlayer);
            }
        }

        // 2. Gera os 15 jogadores que vão aparecer como botões para o player escolher
        for (int i = 0; i < extraPlayersForPlayer; i++)
        {
            Player newPlayer = CreateRandomPlayer();
            GeneratePlayerDraftButton(newPlayer);
        }

        // Atualiza UI inicial (mostra o time do player)
        UpdateCurrentTeamUI(_gameManager.playerTeam);
    }

    // Cria um jogador aleatório com tier
    private Player CreateRandomPlayer()
    {
        
        GameObject playerObject = Instantiate(_gameManager.playerPrefab);
        Player newPlayer = playerObject.GetComponent<Player>();

        // === ALTERAÇÃO IMPORTANTE: Torna o jogador persistente entre cenas ===
        DontDestroyOnLoad(playerObject);

        // (Opcional) Coloca como filho do GameManager para organizar na Hierarchy
        if (_gameManager != null)
        {
            playerObject.transform.SetParent(_gameManager.transform);
        }
        /*
        float rand = Random.value;
        if (rand < 0.40f)
            newPlayer.GenerateEarlyPlayer();
        else if (rand < 0.75f)
            newPlayer.GenerateMidPlayer();
        else
            newPlayer.GenerateEndPlayer();
        */
        // === LÓGICA DE GERAÇÃO BASEADA NOS NÍVEIS DESBLOQUEADOS ===
        float rand = Random.value;

        if (!leagueManager.isOnDraftLVL0 && !leagueManager.isOnDraftLVL1 && !leagueManager.isOnDraftLVL2)
        {
            // Todas falsas  Piores jogadores (muitos End, poucos Early)
            if (rand < 0.20f)
                newPlayer.GenerateEarlyPlayer();      // 20% Early
            else if (rand < 0.80f)
                newPlayer.GenerateMidPlayer();        // 60% Mid
            else
                newPlayer.GenerateEndPlayer();        // 20% End
        }
        else if (leagueManager.isOnDraftLVL0 && !leagueManager.isOnDraftLVL1 && !leagueManager.isOnDraftLVL2)
        {
            // Apenas LVL 0  Equilíbrio Early / Mid
            if (rand < 0.50f)
                newPlayer.GenerateEarlyPlayer();      // 50% Early
            else
                newPlayer.GenerateMidPlayer();        // 50% Mid
        }
        else if (leagueManager.isOnDraftLVL1)
        {
            // LVL 1 desbloqueado  Mais Mid, menos Early, pouco End
            if (rand < 0.20f)
                newPlayer.GenerateEarlyPlayer();      // 20% Early
            else if (rand < 0.90f)
                newPlayer.GenerateMidPlayer();        // 70% Mid
            else
                newPlayer.GenerateEndPlayer();        // 10% End
        }
        else if (leagueManager.isOnDraftLVL2)
        {
            // LVL 2 desbloqueado  Foco em Mid + bom End
            if (rand < 0.10f)
                newPlayer.GenerateEarlyPlayer();      // 10% Early
            else if (rand < 0.70f)
                newPlayer.GenerateMidPlayer();        // 60% Mid
            else
                newPlayer.GenerateEndPlayer();        // 30% End
        }
        else
        {
            // Fallback (caso estranho)
            newPlayer.GenerateMidPlayer();
        }
        return newPlayer;
    }

    public void GeneratePlayerDraftButton(Player player)
    {
        if (player.bt_DraftInfo == null || glg_draftNames == null) return;

        GameObject newButton = Instantiate(player.bt_DraftInfo, glg_draftNames.transform, false);
        Button btn = newButton.GetComponent<Button>();
        btn.onClick.AddListener(() => AddPlayerToTeam(player, btn));

        // Preenche informações no botão
        newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.playerFirstName;
        newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.playerLastName;
        newButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = player.ovr.ToString();

        // Personality Sprite
        Sprite spriteP = null;
        switch (player.Personality)
        {
            case 1: spriteP = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01"); break;
            case 2: spriteP = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02"); break;
            case 3: spriteP = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03"); break;
            case 4: spriteP = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04"); break;
            case 5: spriteP = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05"); break;
        }

        if (newButton.transform.childCount > 3)
        {
            Image img = newButton.transform.GetChild(3).GetComponent<Image>();
            if (img != null && spriteP != null)
                img.sprite = spriteP;
        }

        var btnScript = newButton.GetComponent<BtnDraftUpdateCurrentPlayerToSelect>();
        btnScript.index = player.ImageCharacterPortrait;
        btnScript.playerAge = player.Age.ToString();
        btnScript.playerSalary = player.Salary.ToString();
        btnScript.PlayerOvr = player.ovr;
        btnScript.archtypeIndex = player.ImageCharacterPortrait;
        btnScript.personalityIndex = player.Personality;
        btnScript.personalitySprite = spriteP;
        btnScript.SetSprite();
    }

    private void AddPlayerToTeam(Player player, Button btn)
    {
        // Adiciona o jogador ao time do player
        _gameManager.playerTeam.playersListRoster.Add(player);
        player.J_Number = _gameManager.playerTeam.GenerateUniqueShirtNumber();

        Destroy(btn.gameObject);

        // Atualiza a UI
        UpdateCurrentTeamUI(_gameManager.playerTeam);

        // Verifica se o player já completou seus 8 jogadores
        if (_gameManager.playerTeam.playersListRoster.Count >= totalPicksPerTeam)
        {
            EndDraft();
        }
    }

    private void UpdateCurrentTeamUI(Team currentTeam)
    {
        if (_currentOnTheClockTeamArea.childCount > 0)
            _currentOnTheClockTeamArea.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentTeam.TeamName;

        text_currentPlayersOnTeam.text = currentTeam.playersListRoster.Count.ToString();

        UpdateTeamPlayersUI();
    }

    private void UpdateTeamPlayersUI()
    {
        int uiSlots = _playersFromOnTheClockTeamArea.childCount;
        int playerCount = _gameManager.playerTeam.playersListRoster.Count;

        for (int i = 0; i < uiSlots; i++)
        {
            if (i < playerCount)
            {
                Player p = _gameManager.playerTeam.playersListRoster[i];
                _playersFromOnTheClockTeamArea.GetChild(i).GetComponent<TextMeshProUGUI>().text =
                    p.playerFirstName + " " + p.playerLastName;
            }
            else
            {
                _playersFromOnTheClockTeamArea.GetChild(i).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    private void EndDraft()
    {
        for (int i = 0; i < _gameManager.leagueTeams.Count; i++)
        {
            _gameManager.saveSystem.SaveTeamInfo(_gameManager.leagueTeams[i]);
        }
        
        _gameManager.mode = GameManager.GameMode.TeamManagement;
        leagueManager.CanStartANewRun = false;
        leagueManager.canGenerateEvents = true;
        leagueManager.canStartANewWeek = true;
        SceneManager.LoadScene("MyTeamScreen");
    }
}
