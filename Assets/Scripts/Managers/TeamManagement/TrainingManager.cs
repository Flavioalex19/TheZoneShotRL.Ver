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
        if (effort == 0)
            cost = 200;      // Treino leve
        else if (effort == 1)
            cost = 300;      // Treino médio
        else
            cost = 400;      // Treino pesado (effort 2+)
                             // ============================================================================

        // ====================== DESCONTO DO FINANCES FACILITY ======================
        int financesLvl = gameManager.playerTeam.FinancesLvl;
        float discountPercent = 0f;

        if (financesLvl >= 7)
            discountPercent = 0.25f;      // 25%
        else if (financesLvl >= 5)
            discountPercent = 0.15f;      // 15%
        else if (financesLvl >= 3)
            discountPercent = 0.10f;      // 10%
        else if (financesLvl >= 1)
            discountPercent = 0.05f;      // 5%

        int discountAmount = Mathf.RoundToInt(cost * discountPercent);
        cost -= discountAmount;
        // ============================================================================

        // ====================== CHANCE DE TREINO GRÁTIS (OFFICE LEVEL) ======================
        int officeLvl = gameManager.playerTeam.OfficeLvl;
        float freeChance = 0f;

        if (officeLvl >= 7)
            freeChance = 0.25f;      // 40% de chance de ser grátis
        else if (officeLvl >= 5)
            freeChance = 0.15f;      // 25%
        else if (officeLvl >= 3)
            freeChance = 0.10f;      // 15%
        else if (officeLvl >= 1)
            freeChance = 0.05f;      // 8%

        if (Random.value < freeChance)
        {
            cost = 0;
            Debug.Log("[Training] Treino saiu de graça graças ao Office Level!");
        }
        // ====================================================================================

        // Garante que o custo não fique negativo
        cost = Mathf.Max(0, cost);

        if (gameManager.playerTeam.CurrentBudget > cost && _trainingTypeIndex > -1)
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
            int equipmentLvl = gameManager.playerTeam.TeamEquipmentLvl;
            int playerAge = player.Age;
            int personality = player.Personality;

            // === BASE BOOST POR IDADE ===
            int baseMin = 1;
            int baseMax = 5; // padrão para jogadores mais novos

            if (playerAge >= 30)
            {
                baseMax = 2; // jogadores de 30+ têm limite menor
            }
            else if (playerAge >= 26 && playerAge <= 29)
            {
                baseMax = 3; // meio termo
            }

            // Aplica Equipment Level em cima da base de idade
            int minBoost = Mathf.Max(1, baseMin + (equipmentLvl / 2));
            int maxBoostExclusive = Mathf.Min(8, baseMax + equipmentLvl);

            // Bônus extra de Equipment em níveis altos
            if (equipmentLvl >= 5)
            {
                minBoost += 1;
                maxBoostExclusive += 1;
            }

            // === BÔNUS / PENALIDADE POR PERSONALIDADE ===
            if (personality >= 4) // Personalidade 4 e 5 = mais ofensivo
            {
                if (_trainingTypeIndex == 2) // Shooting training
                {
                    minBoost += 1;
                    maxBoostExclusive += 2;
                }
            }
            else if (personality == 2 || personality == 3) // Personalidade 2 e 3 = bom em shooting
            {
                if (_trainingTypeIndex == 2) // Shooting training
                {
                    minBoost += 1;
                    maxBoostExclusive += 1;
                }
            }
            else if (personality == 1) // Personalidade 1 = melhor em defesa
            {
                if (_trainingTypeIndex == 1) // Defense training
                {
                    minBoost += 1;
                    maxBoostExclusive += 2;
                }
            }

            // === REDUÇÃO POR MORAL BAIXO ===
            if (gameManager.playerTeam.Moral < 20)
            {
                minBoost = Mathf.Max(1, minBoost - 1);
                maxBoostExclusive = Mathf.Max(2, maxBoostExclusive - 2);
                Debug.Log("Training effect reduced due to low morale.");
            }
            //======================================================================
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
            gameManager.playerTeam.CurrentBudget -= cost;
            teamManagerUI.UpdateMoralAndPointsTexts();
            gameManager.saveSystem.SaveLeague();
            for (int i = 0; i < gameManager.leagueTeams.Count; i++)
            {
                gameManager.saveSystem.SaveTeamInfo(gameManager.leagueTeams[i]);
            }
            targetAttributes.Clear();
            selectedAttributes.Clear();
            teamManagerUI.SetTrainingGrade();
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
