using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match_PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxRadius = 3f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 1f;

    [Header("Abrupt Y Settings")]
    [SerializeField] private bool allowAbruptY = false;
    [SerializeField] private float abruptYAmount = 1f;
    [SerializeField] private float abruptChance = 0.2f;

    private Vector2 center;
    private Vector2 targetPoint;
    private bool goingOut = true;
    private bool isWaiting = false;

    private void Start()
    {
        center = transform.position;
        PickNewDirection();
    }

    private void Update()
    {
        if (isWaiting) return;

        Vector2 currentPos = transform.position;
        Vector2 direction = (targetPoint - currentPos).normalized;
        Vector2 nextPos = currentPos + direction * moveSpeed * Time.deltaTime;

        transform.position = nextPos;

        // If reached destination
        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            if (goingOut)
            {
                targetPoint = center; // Go back to center
                goingOut = false;
            }
            else
            {
                // At center — wait before choosing next move
                StartCoroutine(WaitAndChooseNext());
            }
        }
    }

    private IEnumerator WaitAndChooseNext()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        PickNewDirection(); // Choose new direction
        goingOut = true;
        isWaiting = false;
    }

    private void PickNewDirection()
    {

        float angle = Random.Range(0f, 2f * Mathf.PI);

        // CLAMP DISTANCE between half radius and full radius
        float distance = Random.Range(maxRadius * 0.5f, maxRadius);

        float x = center.x + Mathf.Cos(angle) * distance;
        float y = center.y + Mathf.Sin(angle) * distance;
        targetPoint = new Vector2(x, y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Application.isPlaying ? center : (Vector2)transform.position, maxRadius);
    }
}
