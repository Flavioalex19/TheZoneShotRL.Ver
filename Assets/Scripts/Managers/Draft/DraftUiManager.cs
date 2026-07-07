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
    [SerializeField] Animator animator_transition;
    [SerializeField] BtnSelectionHandler btn_selectionHandler;
    [SerializeField] Image image_playerTeam;
    private int currentTeamIndex = 0;
    private int totalPicksPerTeam = 8;
    private int extraPlayersForPlayer = 15;


    private void Start()
    {
        if (_gameManager == null)
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (leagueManager == null)
            leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();

        Sprite sprite;
        sprite = Resources.Load<Sprite>("2D/Team Logos/" + _gameManager.playerTeam.TeamName);
        image_playerTeam.sprite = sprite;
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
                Player newPlayer = CreateRandomPlayer(false);
                team.playersListRoster.Add(newPlayer);
            }
        }

        // 2. Gera os 15 jogadores que vão aparecer como botões para o player escolher
        for (int i = 0; i < extraPlayersForPlayer; i++)
        {
            Player newPlayer = CreateRandomPlayer(true);
            GeneratePlayerDraftButton(newPlayer);
        }
        //LayoutRebuilder.ForceRebuildLayoutImmediate(glg_draftNames.GetComponent<RectTransform>());
        //glg_draftNames.enabled = false;
        //3. Adiciona os legends ao time do player
        if (leagueManager.CanCreateLegend0)
        {
            GameObject playerObject = Instantiate(_gameManager.player_legend0);
            DontDestroyOnLoad(playerObject);
            if (_gameManager != null)
            {
                playerObject.transform.SetParent(_gameManager.transform);
            }
            _gameManager.playerTeam.playersListRoster.Add(playerObject.GetComponent<Player>());
        }
        if (leagueManager.CanCreateLegend1)
        {
            GameObject playerObject = Instantiate(_gameManager.player_legend1);
            DontDestroyOnLoad(playerObject);
            if (_gameManager != null)
            {
                playerObject.transform.SetParent(_gameManager.transform);
            }
            _gameManager.playerTeam.playersListRoster.Add(playerObject.GetComponent<Player>());
        }
        if (leagueManager.CanCreateLegend4)
        {
            GameObject playerObject = Instantiate(_gameManager.player_legend4);
            DontDestroyOnLoad(playerObject);
            if (_gameManager != null)
            {
                playerObject.transform.SetParent(_gameManager.transform);
            }
            _gameManager.playerTeam.playersListRoster.Add(playerObject.GetComponent<Player>());
        }
        // Atualiza UI inicial (mostra o time do player)
        UpdateCurrentTeamUI(_gameManager.playerTeam);
    }

    // Cria um jogador aleatório com tier
    private Player CreateRandomPlayer(bool isPlayerteam)
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
        
        // === LÓGICA DE GERAÇÃO BASEADA NOS NÍVEIS DESBLOQUEADOS ===
        float rand = Random.value;
        if (isPlayerteam)
        {
            if (!leagueManager.isOnDraftLVL0 && !leagueManager.isOnDraftLVL1 && !leagueManager.isOnDraftLVL2)
            {
                // Todas falsas  Piores jogadores (muitos End, poucos Early)
                /*
                if (rand < 0.20f)
                    newPlayer.GenerateEarlyPlayer();      // 20% Early
                else if (rand < 0.80f)
                    newPlayer.GenerateMidPlayer();        // 60% Mid
                else
                    newPlayer.GenerateEarlyPlayer();        // 20% End
                */
                if (rand < 50) newPlayer.GenerateStarters();
                else newPlayer.GenerateEarlyPlayer();
            }
            else if (leagueManager.isOnDraftLVL0 && !leagueManager.isOnDraftLVL1 && !leagueManager.isOnDraftLVL2)
            {
                print("LVL1");
                // Apenas LVL 0  Equilíbrio Early / Mid
                if (rand < 0.50f)
                    newPlayer.GenerateStarters();      // 50% Early
                else
                    newPlayer.GenerateEarlyPlayer();        // 50% Mid
            }
            else if (leagueManager.isOnDraftLVL1)
            {
                print("LVL2");
                // LVL 1 desbloqueado  Mais Mid, menos Early, pouco End
                if (rand < 0.20f)
                    newPlayer.GenerateEarlyPlayer();      // 20% Early
                else if (rand < 0.90f)
                    newPlayer.GenerateMidPlayer();        // 70% Mid
                else
                    newPlayer.GenerateEarlyPlayer();        // 10% End
            }
            else if (leagueManager.isOnDraftLVL2)
            {
                print("LVL3");
                // LVL 2 desbloqueado  Foco em Mid + bom End
                if (rand < 0.10f)
                    newPlayer.GenerateEarlyPlayer();      // 10% Early
                else if (rand < 0.70f)
                    newPlayer.GenerateMidPlayer();        // 80% Mid
                else
                    newPlayer.GenerateEndPlayer();        // 10% End
            }
            else
            {
                print("Strange");
                // Fallback (caso estranho)
                newPlayer.GenerateStarters();
            }
        }
        else
        {
            print("Isnot player team");
            if (rand < 0.10f)
                newPlayer.GenerateEarlyPlayer();      // 10% Early
            else if (rand < 0.70f)
                newPlayer.GenerateMidPlayer();        // 80% Mid
            else
                newPlayer.GenerateEndPlayer();
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
        btnScript.btn_draft_playerInfo = player;
        btnScript.index = player.ImageCharacterPortrait;
        btnScript.playerAge = player.Age.ToString();
        //btnScript.playerSalary = player.Salary.ToString();
        btnScript.PlayerOvr = player.ovr;
        btnScript.archtypeIndex = player.TraitIndex;
        //btnScript.btn_draft_playerInfo = player;
        btnScript.personalityIndex = player.Personality;
        btnScript.personalitySprite = spriteP;
        btnScript.SetSprite();
        btnScript.playerName = player.playerFirstName + " " + player.playerLastName;
        btnScript.PlayerOvr = player.SetOVR();

        btn_selectionHandler.GetSelectabes().Add(newButton.GetComponent<Button>());
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
                    p.playerFirstName + " " + p.playerLastName + " OVR:" + p.SetOVR();
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
        //SceneManager.LoadScene("MyTeamScreen");
        StartCoroutine(WaitCutSceneToEnd());
    }
    IEnumerator WaitCutSceneToEnd()
    {
        animator_transition.SetTrigger("Go");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MyTeamScreen");
    }
}
