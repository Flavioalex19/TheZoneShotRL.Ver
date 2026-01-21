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
    [SerializeField] int _trainingTypeIndex = -1;

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
        //Sprite alteration/update
        Sprite[] sprites = Resources.LoadAll<Sprite>("2D/Characters/Alpha/Players");
        Sprite sprite = sprites[gameManager.playerTeam.playersListRoster[index].ImageCharacterPortrait];
        teamManagerUI.image_currentPlayerPortraitToTrain.sprite = sprite;
    }

    public void SetTraining(int effort)
    {
        teamManagerUI._trainingResultPanel.SetActive(true);
        int cost = 0;
        if (effort == 0) cost = 25;
        else if (effort == 1) cost = 40;
        else cost = 50;

        if (leagueManager.canTrain == true && gameManager.playerTeam.EffortPoints > cost && _trainingTypeIndex >-1)
        {
            /*
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
            */
            // --- BOOST BASEADO NO EFFORT (agora com ranges por atributo) ---
            int minBoost = 1;
            int maxBoostExclusive = 4; // padrão effort 0: 1-3

            if (effort == 1)
            {
                minBoost = 2;
                maxBoostExclusive = 5; // 2-4
            }
            else if (effort == 2)
            {
                minBoost = 3;
                maxBoostExclusive = 6; // 3-5
            }
            // ---------------------------------

            Player player = gameManager.playerTeam.playersListRoster[playerIndex];

            // === Lista dos atributos do grupo selecionado ===
            List<TrainingAttribute> targetAttributes = new List<TrainingAttribute>();

            if (_trainingTypeIndex == 0)
            {
                targetAttributes.Add(TrainingAttribute.Awareness);
                targetAttributes.Add(TrainingAttribute.Juking);
                targetAttributes.Add(TrainingAttribute.Control);
                targetAttributes.Add(TrainingAttribute.Consistency);
            }
            else if (_trainingTypeIndex == 1)
            {
                targetAttributes.Add(TrainingAttribute.Defending);
                targetAttributes.Add(TrainingAttribute.Stealing);
                targetAttributes.Add(TrainingAttribute.Guarding);
            }
            else if (_trainingTypeIndex == 2)
            {
                targetAttributes.Add(TrainingAttribute.Inside);
                targetAttributes.Add(TrainingAttribute.Mid);
                targetAttributes.Add(TrainingAttribute.Outside);
                targetAttributes.Add(TrainingAttribute.Shooting);
            }

            // Segurança caso index inválido
            if (targetAttributes.Count == 0)
            {
                teamManagerUI._textDrillSelected.text = "Invalid training type selected.";
                return;
            }

            // === Aplicar boost em TODOS os atributos do grupo ===
            string trainingResult = $"{player.playerFirstName} {player.playerLastName} improved:\n";

            foreach (TrainingAttribute attr in targetAttributes)
            {
                int boost = Random.Range(minBoost, maxBoostExclusive);

                switch (attr)
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

                trainingResult += $"+{boost} to {attr}\n";
            }

            trainingResult += "Training session completed.";
            teamManagerUI._textDrillSelected.text = trainingResult;
            // =====================================================================

            teamManagerUI.UpdateAssistancePortrait();
            leagueManager.canTrain = false;
            teamManagerUI.SetTrainingGrade();
            player.UpdateOVR();
            gameManager.playerTeam.EffortPoints -= cost;
            gameManager.saveSystem.SaveLeague();

            for (int i = 0; i < gameManager.leagueTeams.Count; i++)
            {
                gameManager.saveSystem.SaveTeamInfo(gameManager.leagueTeams[i]);
            }
            targetAttributes.Clear();
        }
        else
        {
            
            teamManagerUI._textDrillSelected.text = "Cannot train this week anymore";
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
    public void SetTrainingType(int index)
    {
        _trainingTypeIndex = index;
        if (index == 0) teamManagerUI.text_trainingType.text = "Offense";
        else if (index == 1) teamManagerUI.text_trainingType.text = "Defense";
        else if(index == 2) teamManagerUI.text_trainingType.text = "Shooting";
    }
    public void ResetTrainingType()
    {
        _trainingTypeIndex = -1;
    }
}
