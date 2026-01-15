using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TrainingAttribute
{
    Awareness,
    Shooting,
    Inside,
    Outside,
    Mid,
    Defending,
    Consistency,
    Juking,
    Stealing,
    Control,
    Guarding
}
public class TrainingManager : MonoBehaviour
{
    [SerializeField] int playerIndex;
    [SerializeField] int _trainingIndex;
    [SerializeField] int _trainingDrillAmount = 5;

    public TrainingAttribute selectedTrainingAttribute = TrainingAttribute.Awareness;

    GameManager gameManager;
    LeagueManager leagueManager;
    [SerializeField]TeamManagerUI teamManagerUI;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        leagueManager = GameObject.Find("League/Season Manager").GetComponent<LeagueManager>();
    }

    
    public void SetPlayerToTrainIndex(int index)
    {
        playerIndex = index;
        teamManagerUI._textPlayerSelected.text = gameManager.playerTeam.playersListRoster[index].playerFirstName.ToString() + " " + 
            gameManager.playerTeam.playersListRoster[index].playerLastName.ToString();
        //SetTraining(index, drillChoosed);
    }

    public void SetTraining(int effort)
    {
        teamManagerUI._trainingResultPanel.SetActive(true);
        int cost = 0;
        if (effort == 0) cost = 25;
        else if (effort == 1) cost = 40;
        else cost = 50;

        if (leagueManager.canTrain == true && gameManager.playerTeam.EffortPoints > cost)
        {
            
            // --- BOOST BASEADO NO EFFORT ---
            int maxBoost = 1; // effort 0
            if (effort == 1) maxBoost = 2;
            else if (effort == 2) maxBoost = 4;

            int boost = Random.Range(maxBoost, maxBoost + 1);
            // ---------------------------------

            selectedTrainingAttribute = (TrainingAttribute)Random.Range(0,
                System.Enum.GetValues(typeof(TrainingAttribute)).Length);

            Player player = gameManager.playerTeam.playersListRoster[playerIndex];

            // Aplicar boost ao atributo
            switch (selectedTrainingAttribute)
            {
                case TrainingAttribute.Awareness: player.Awareness = Mathf.Min(player.Awareness + boost, 99); break;
                case TrainingAttribute.Shooting: player.Shooting = Mathf.Min(player.Shooting + boost, 99); break;
                case TrainingAttribute.Inside: player.Inside = Mathf.Min(player.Inside + boost, 99); break;
                case TrainingAttribute.Outside: player.Outside = Mathf.Min(player.Outside + boost, 99); break;
                case TrainingAttribute.Mid: player.Mid = Mathf.Min(player.Mid + boost, 99); break;
                case TrainingAttribute.Defending: player.Defending = Mathf.Min(player.Defending + boost, 99); break;
                case TrainingAttribute.Consistency: player.Consistency = Mathf.Min(player.Consistency + boost, 99); break;
                case TrainingAttribute.Juking: player.Juking = Mathf.Min(player.Juking + boost, 99); break;
                case TrainingAttribute.Stealing: player.Stealing = Mathf.Min(player.Stealing + boost, 99); break;
                case TrainingAttribute.Control: player.Control = Mathf.Min(player.Control + boost, 99); break;
                case TrainingAttribute.Guarding: player.Guarding = Mathf.Min(player.Guarding + boost, 99); break;
            }

            // Resultado simples, direto no atributo
            teamManagerUI._textDrillSelected.text =
                $"{player.playerFirstName} {player.playerLastName} +{boost} to {selectedTrainingAttribute}. Training session completed.";

            teamManagerUI.UpdateAssistancePortrait();
            leagueManager.canTrain = false;
            teamManagerUI.SetTrainingGrade();
            player.UpdateOVR();
            gameManager.playerTeam.EffortPoints -= cost;

            gameManager.saveSystem.SaveLeague();
            for (int i = 0; i < gameManager.leagueTeams.Count; i++)
            {
                //gameManager.saveSystem.SaveTeam(gameManager.leagueTeams[i]);
                gameManager.saveSystem.SaveTeamInfo(gameManager.leagueTeams[i]);
            }
        }
        else
        {
            teamManagerUI._textDrillSelected.text = "No training this week";
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
