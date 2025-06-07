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
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    
    public void SetPlayerToTrainIndex(int index, TextMeshProUGUI playerChoosed, TextMeshProUGUI drillChoosed)
    {
        playerIndex = index;
        playerChoosed.text = gameManager.playerTeam.playersListRoster[index].playerFirstName.ToString();
        SetTraining(index, drillChoosed);
    }

    void SetTraining(int playerIndex,TextMeshProUGUI drillChoosed)
    {
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
    }
}
