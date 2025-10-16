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

    public void SetTraining(/*int playerIndex,TextMeshProUGUI drillChoosedText*/)
    {
        teamManagerUI._trainingResultPanel.SetActive(true);

        if (leagueManager.canTrain == true)
        {
            _trainingIndex = Random.Range(0, _trainingDrillAmount);
            int boost = 0;
            boost = Random.Range(0, 4);//Change this later
            selectedTrainingAttribute = (TrainingAttribute)Random.Range(0, System.Enum.GetValues(typeof(TrainingAttribute)).Length);
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

            // Apply boost only to chosen attribute
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

            teamManagerUI._textDrillSelected.text = $"{player.playerFirstName} {player.playerLastName} - {label} +{boost} to {selectedTrainingAttribute}. Training session for this week is completed.";
            teamManagerUI.UpdateAssistancePortrait();
            leagueManager.canTrain = false;
            player.UpdateOVR();
            for (int i = 0; i < gameManager.leagueTeams.Count; i++)
            {
                gameManager.saveSystem.SaveTeam(gameManager.leagueTeams[i]);
            }
        }
        else
        {
            teamManagerUI._textDrillSelected.text = "Training session for this week is completed";
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
