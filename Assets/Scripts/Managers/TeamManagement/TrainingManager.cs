using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] int playerIndex;
    [SerializeField] int _trainingIndex;
    [SerializeField] int _trainingDrillAmount = 5;

    GameManager gameManager;
    LeagueManager leagueManager;
    [SerializeField]TeamManagerUI teamManagerUI;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
    }

    
    public void SetPlayerToTrainIndex(int index, TextMeshProUGUI playerChoosed, TextMeshProUGUI drillChoosed)
    {
        playerIndex = index;
        playerChoosed.text = gameManager.playerTeam.playersListRoster[index].playerFirstName.ToString();
        SetTraining(index, drillChoosed);
    }

    void SetTraining(int playerIndex,TextMeshProUGUI drillChoosed)
    {
        /*
        //Change this for probability or use other varible to increase the chance
        _trainingIndex = Random.Range(0, _trainingDrillAmount);
        switch (_trainingIndex)
        {
            case 0:
                drillChoosed.text = gameManager.playerTeam.playersListRoster[playerIndex].playerFirstName + " + Minimum Boost" + " All stats + 1";
                gameManager.playerTeam.playersListRoster[playerIndex].Awareness += 1;
                gameManager.playerTeam.playersListRoster[playerIndex].Shooting += 1;
                gameManager.playerTeam.playersListRoster[playerIndex].Inside += 1;
                gameManager.playerTeam.playersListRoster[playerIndex].Outside += 1;
                gameManager.playerTeam.playersListRoster[playerIndex].Mid += 1;
                break;
            case 1:
                drillChoosed.text = gameManager.playerTeam.playersListRoster[playerIndex].playerFirstName + " + Good Training day" + "All stats + 2 ";
                gameManager.playerTeam.playersListRoster[playerIndex].Awareness += 2;
                gameManager.playerTeam.playersListRoster[playerIndex].Shooting += 2;
                gameManager.playerTeam.playersListRoster[playerIndex].Inside += 2;
                gameManager.playerTeam.playersListRoster[playerIndex].Outside += 2;
                gameManager.playerTeam.playersListRoster[playerIndex].Mid += 2;
                break;
            case 2:
                drillChoosed.text = gameManager.playerTeam.playersListRoster[playerIndex].playerFirstName + " + Great training day" + " All stats +3";
                gameManager.playerTeam.playersListRoster[playerIndex].Awareness += 3;
                gameManager.playerTeam.playersListRoster[playerIndex].Shooting += 3;
                gameManager.playerTeam.playersListRoster[playerIndex].Inside += 3;
                gameManager.playerTeam.playersListRoster[playerIndex].Outside += 3;
                gameManager.playerTeam.playersListRoster[playerIndex].Mid += 3;
                break;
            case 3:
                drillChoosed.text = gameManager.playerTeam.playersListRoster[playerIndex].playerFirstName + " + Outstanding Training day" + " All stats +4";
                gameManager.playerTeam.playersListRoster[playerIndex].Awareness += 4;
                gameManager.playerTeam.playersListRoster[playerIndex].Shooting += 4;
                gameManager.playerTeam.playersListRoster[playerIndex].Inside += 4;
                gameManager.playerTeam.playersListRoster[playerIndex].Outside += 4;
                gameManager.playerTeam.playersListRoster[playerIndex].Mid += 4;
                break;
            case 4:
                drillChoosed.text = gameManager.playerTeam.playersListRoster[playerIndex].playerFirstName + "+ Perfect Training day" + " All stats +8";
                gameManager.playerTeam.playersListRoster[playerIndex].Awareness += 8;
                gameManager.playerTeam.playersListRoster[playerIndex].Shooting += 8;
                gameManager.playerTeam.playersListRoster[playerIndex].Inside += 8;
                gameManager.playerTeam.playersListRoster[playerIndex].Outside += 8;
                gameManager.playerTeam.playersListRoster[playerIndex].Mid += 8;
                break;
            default:
                break;
        }
        */
        if(leagueManager.canTrain == true)
        {
            _trainingIndex = Random.Range(0, _trainingDrillAmount);
            int boost = 0;
            string label = "";

            switch (_trainingIndex)
            {
                case 0: boost = 1; label = "Minimum Boost"; break;
                case 1: boost = 2; label = "Good Training day"; break;
                case 2: boost = 3; label = "Great training day"; break;
                case 3: boost = 4; label = "Outstanding Training day"; break;
                case 4: boost = 8; label = "Perfect Training day"; break;
                default: return;
            }

            Player player = gameManager.playerTeam.playersListRoster[playerIndex];
            drillChoosed.text = $"{player.playerFirstName} + {label} - All stats +{boost}";

            player.Awareness = Mathf.Min(player.Awareness + boost, 99);
            player.Shooting = Mathf.Min(player.Shooting + boost, 99);
            player.Inside = Mathf.Min(player.Inside + boost, 99);
            player.Outside = Mathf.Min(player.Outside + boost, 99);
            player.Mid = Mathf.Min(player.Mid + boost, 99);

            leagueManager.canTrain = false;
            for (int i = 0; i < gameManager.leagueTeams.Count; i++)
            {
                gameManager.saveSystem.SaveTeam(gameManager.leagueTeams[i]);
            }
        }
        else
        {
            drillChoosed.text = "training session completed";
        }
        
    }
    public void CheckIfTrainingIsCompleted()
    {
        if(leagueManager.canTrain == false)
        {
            //teamManagerUI._trainengCompletePanel.SetActive(true);
            //drillChoosed.text = "training session completed";
        }
    }
}
