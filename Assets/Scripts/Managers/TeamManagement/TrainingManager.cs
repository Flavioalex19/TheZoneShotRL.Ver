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
        teamManagerUI.training_playerOVR.text = gameManager.playerTeam.playersListRoster[index].SetOVR().ToString();
        teamManagerUI.training_playerAge.text = gameManager.playerTeam.playersListRoster[index].Age.ToString();
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
        if (effort == 0) cost = 20;
        else if (effort == 1) cost = 40;
        else cost = 50;

        //discount based on financaes facilty
        int financesDiscount = 0;
        if (gameManager.playerTeam.FinancesLvl > 0)
        {
            financesDiscount = Mathf.Min(30, gameManager.playerTeam.FinancesLvl * 4); // máx 30 no lvl 7+
        }
        cost = Mathf.Max(10, cost - financesDiscount);

        if (gameManager.playerTeam.EffortPoints > cost && _trainingTypeIndex > -1)
        {
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
            int equipmentLvl = gameManager.playerTeam.TeamEquipmentLvl; // Assuma que existe essa propriedade
            int minBoost = Mathf.Max(1, equipmentLvl / 2);
            int maxBoostExclusive = Mathf.Min(8, equipmentLvl + 3);
            if (equipmentLvl >= 4)
            {
                minBoost += 1;
                maxBoostExclusive += 2;
            }
            //int minBoost = Mathf.Max(1, equipmentLvl / 2); // Ex: lvl 0=1, lvl 7=3
            //int maxBoostExclusive = Mathf.Min(6, equipmentLvl + 2); // Ex: lvl 0=2 (boost 1), lvl 7=6 (boost 1-5)
            int personality = player.Personality;  // 1-5, assume maior = melhor boost
            minBoost += Mathf.Max(0, (personality - 1) / 2);  // Ex: +0 para 1, +2 para 5
            maxBoostExclusive += personality - 1;  // Ex: +0 para 1, +4 para 5 (aumenta range)
                                                   // === Selecionar atributos baseados em effort ===
            List<TrainingAttribute> selectedAttributes = new List<TrainingAttribute>();
            if (effort == 0)
            {
                // Apenas 1 aleatório
                selectedAttributes.Add(targetAttributes[Random.Range(0, targetAttributes.Count)]);
            }
            else if (effort == 1)
            {
                // Metade aleatória (arredonda para baixo, min 1)
                int halfCount = Mathf.Max(1, targetAttributes.Count / 2);
                List<TrainingAttribute> shuffled = new List<TrainingAttribute>(targetAttributes);
                shuffled.Sort((a, b) => Random.value.CompareTo(Random.value)); // Shuffle aleatório
                selectedAttributes = shuffled.GetRange(0, halfCount);
            }
            else if (effort == 2)
            {
                // Todos
                selectedAttributes = new List<TrainingAttribute>(targetAttributes);
            }
            // === Aplicar boost nos atributos selecionados ===
            string trainingResult = $"{player.playerFirstName} {player.playerLastName} improved:\n";
            foreach (TrainingAttribute attr in selectedAttributes)
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
            teamManagerUI.UpdateAssistancePortrait(teamManagerUI.transform_assistance_ResultPortrait, true);
            leagueManager.canTrain = false;
            teamManagerUI.SetTrainingGrade();
            player.UpdateOVR();
            gameManager.playerTeam.EffortPoints -= cost;
            teamManagerUI.UpdateMoralAndPointsTexts();
            gameManager.saveSystem.SaveLeague();
            for (int i = 0; i < gameManager.leagueTeams.Count; i++)
            {
                gameManager.saveSystem.SaveTeamInfo(gameManager.leagueTeams[i]);
            }
            targetAttributes.Clear();
            selectedAttributes.Clear();
        }
        else
        {
            teamManagerUI._textDrillSelected.text = "Not enought points to train";
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
