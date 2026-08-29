using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChemistryBars : MonoBehaviour
{
    GameManager gameManager;
    public Image image_teamStats_ChemestryBar;
    public TextMeshProUGUI text_chmestryValue;
    int initialValue = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void AnimateFillBar()
    {
        float duration = 0.8f;
        if (image_teamStats_ChemestryBar == null) return;

        float target = Mathf.Clamp01(gameManager.playerTeam.ChemistryPts / 100f);
        image_teamStats_ChemestryBar.fillAmount = 0f;
        StartCoroutine(FillBarRoutine(image_teamStats_ChemestryBar, target, duration));
    }

    private IEnumerator FillBarRoutine(Image fillImage, float targetFill, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float smoothT = t * t * (3f - 2f * t); // ease in-out
            fillImage.fillAmount = Mathf.Lerp(0f, targetFill, smoothT);
            yield return null;
        }

        fillImage.fillAmount = targetFill;
    }

    public void AnimateTextValue()
    {
        float duration = 0.8f;
        if (text_chmestryValue == null) return;

        text_chmestryValue.text = "0";
        StartCoroutine(CountTextRoutine(text_chmestryValue, gameManager.playerTeam.ChemistryPts, duration));
    }

    private IEnumerator CountTextRoutine(TextMeshProUGUI text, float targetValue, float duration)
    {
        float elapsed = 0f;
        float startValue = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float smoothT = t * t * (3f - 2f * t); // ease in-out
            float current = Mathf.Lerp(startValue, targetValue, smoothT);

            text.text = Mathf.RoundToInt(current).ToString();
            yield return null;
        }

        text.text = Mathf.RoundToInt(targetValue).ToString();
    }
}
