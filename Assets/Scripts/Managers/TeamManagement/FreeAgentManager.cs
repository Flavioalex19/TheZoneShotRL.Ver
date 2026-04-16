using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FreeAgentManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] TeamManagerUI teamManagerUI;
    [SerializeField] GridLayoutGroup gdl_FreeAgents;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    //Generate Players
    public void GeneratePlayers(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            // Instantiate a new player object
            GameObject playerObject = Instantiate(gameManager.playerPrefab);

            // Access the Player component and set the attributes
            Player newPlayer = playerObject.GetComponent<Player>();
            newPlayer.GenerateEarlyPlayer(); // Randomize the player's name and overall rating
            DontDestroyOnLoad(playerObject);                    // Mantém o jogador entre cenas
            playerObject.transform.SetParent(gameManager.transform); // Organiza como filho do GameManager
            GeneratePlayerDraftButton(newPlayer);
            

        }
    }
    void GeneratePlayerDraftButton(Player player)
    {
        GameObject newButton = Instantiate(player.bt_DraftInfo, gdl_FreeAgents.transform, false);
        newButton.GetComponent<Button>().onClick.AddListener(() => AddPlayerToTeam(player, newButton.GetComponent<Button>()));
        newButton.GetComponent<Button>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.playerFirstName.ToString();
        newButton.GetComponent<Button>().transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.playerLastName.ToString();
        newButton.GetComponent<Button>().transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = player.ovr.ToString();
        newButton.GetComponent<BtnDraftUpdateCurrentPlayerToSelect>().index = player.ImageCharacterPortrait;
        newButton.GetComponent<BtnDraftUpdateCurrentPlayerToSelect>().SetSprite();
        newButton.GetComponent<BtnDraftUpdateCurrentPlayerToSelect>().playerName = player.playerLastName;
        newButton.GetComponent<BtnDraftUpdateCurrentPlayerToSelect>().playerAge = player.Age.ToString();

        Sprite sprite = null; //= Resources.Load<Sprite>("Assets/Resources/2D/Player Personalities/UI_icon_Personalite_01.png");
        switch (player.Personality)
        {
            case 1:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_01");
                break;
            case 2:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_02");
                break;
            case 3:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_03");
                break;

            case 4:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_04");
                break;

            case 5:
                sprite = Resources.Load<Sprite>("2D/Player Personalities/UI_icon_Personalite_05");
                break;

            default:
                break;
        }
        Image myImageComponent = newButton.GetComponent<Button>().transform.GetChild(3).GetComponent<Image>();
        myImageComponent.sprite = sprite;


    }
    void AddPlayerToTeam(Player player, Button btn)
    {
        //Fazer if para ver se pode adicionar

        player.J_Number = gameManager.playerTeam.GenerateUniqueShirtNumber();
        print("This player number and team is: " + player.J_Number + " " + gameManager.playerTeam.TeamName);
        // Add the player to the current team
        gameManager.playerTeam.playersListRoster.Add(player);
        // Print the player's name and the team they were added to
        //Debug.Log($"Player {player.playerFirstName} added to Team {leagueTeams[currentTeamIndex].TeamName}");
        Destroy(btn.gameObject);//NEW!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // Check child count in the next frame
        StartCoroutine(CheckAndProceedAfterDestroy());
    }
    IEnumerator CheckAndProceedAfterDestroy()
    {
        // Wait for the end of the frame to ensure Destroy() has taken effect
        yield return new WaitForEndOfFrame();

        // Check if all buttons are removed from GridLayoutGroup
        if (gameManager.playerTeam.playersListRoster.Count >= 8)
        {

            //Fechar o painel de FA!!!!!!!!!!!!!!!!!!!!!!!!!
            //teamManagerUI.ValidateProgressWeek();//can change this for a button
            teamManagerUI.canProgressWithWeek = true;
        }


    }
    //Check if contracts expires
    public void RemoveExpiredContracts(Team team)
    {
        /*
        // Remove all players whose contract years are less than 1
        //team.playersListRoster.RemoveAll(player => player.ContractYears < 1);
        int beforeCount = team.playersListRoster.Count;

        // Remove all players whose contract years are less than 1
        team.playersListRoster.RemoveAll(player => player.ContractYears < 1);

        int afterCount = team.playersListRoster.Count;

        if (beforeCount == afterCount)
        {
            // No players were removed
            Debug.Log("No players with expiring contracts on team: " + team.TeamName);
        }
        */
        if (team == null || team.playersListRoster == null)
            return;

        int beforeCount = team.playersListRoster.Count;

        // Lista temporária para guardar os jogadores que vamos destruir
        List<Player> playersToDestroy = new List<Player>();

        // 1. Procura e coleta os jogadores com contrato expirado
        for (int i = team.playersListRoster.Count - 1; i >= 0; i--)
        {
            Player p = team.playersListRoster[i];

            if (p != null && p.ContractYears < 1)
            {
                playersToDestroy.Add(p);           // salva o jogador
                team.playersListRoster.RemoveAt(i); // remove da lista
                Debug.Log($"[RemoveExpiredContracts] Jogador marcado para remoção: {p.playerFirstName} {p.playerLastName}");
            }
        }

        // 2. Destrói os GameObjects dos jogadores removidos
        foreach (Player p in playersToDestroy)
        {
            if (p != null && p.gameObject != null)
            {
                Destroy(p.gameObject);
                Debug.Log($"[RemoveExpiredContracts] GameObject destruído: {p.playerFirstName} {p.playerLastName}");
            }
        }

        int afterCount = team.playersListRoster.Count;

        Debug.Log($"[RemoveExpiredContracts] Time {team.TeamName}: {beforeCount} - {afterCount} jogadores. Removidos: {beforeCount - afterCount}");

        // 3. Limpeza extra (segurança) - remove qualquer null que possa ter sobrado
        team.playersListRoster.RemoveAll(p => p == null);

    }
}
