using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class Match_PlayerMovement : MonoBehaviour
{
    [SerializeField] private RectTransform playerUI;  // O elemento UI do jogador
    [SerializeField] private float moveDistance = 100f; // Distância do avanço no eixo X
    [SerializeField] private float moveSpeed = 2f;     // Velocidade de movimento
    [SerializeField] private float holdTime = 0.5f;    // Tempo parado antes de voltar
    [SerializeField] float waitTime = 1f;
    private Vector2 originalPosition;
    private Vector2 targetPosition;
    private bool isMoving;
    [SerializeField] private float moveRange = 50f; // Alcance máximo do movimento

    private void Start()
    {
        if (playerUI == null)
            playerUI = GetComponent<RectTransform>();

        originalPosition = playerUI.anchoredPosition;
        StartCoroutine(RandomMovement());
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            // Espera um tempo aleatório antes de iniciar o movimento
            float waitTime = Random.Range(0.3f, 1f);
            yield return new WaitForSeconds(waitTime);

            // Define nova posição alvo
            Vector2 randomOffset = new Vector3(
                Random.Range(-moveRange, moveRange),
                Random.Range(-moveRange, moveRange),
                0f
            );
            Vector2 targetPosition = originalPosition + randomOffset;

            // Move suavemente até a nova posição
            yield return MoveSmoothly(targetPosition);

            // Espera novamente um tempo aleatório antes de retornar
            waitTime = Random.Range(0.3f, 1f);
            yield return new WaitForSeconds(waitTime);

            // Retorna suavemente à posição original
            yield return MoveSmoothly(originalPosition);
        }
    }

    private IEnumerator MoveSmoothly(Vector2 targetPos)
    {
        isMoving = true;
        Vector2 startPos = playerUI.anchoredPosition;
        float randomSpeed = Random.Range(moveSpeed / 2f, moveSpeed);
        float elapsed = 0f;
        float distance = Vector2.Distance(startPos, targetPos);

        while (elapsed < distance)
        {
            elapsed += Time.deltaTime * randomSpeed;
            float t = Mathf.Clamp01(elapsed / distance);
            playerUI.anchoredPosition = Vector2.Lerp(startPos, targetPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        playerUI.anchoredPosition = targetPos;
        isMoving = false;
    }
}
