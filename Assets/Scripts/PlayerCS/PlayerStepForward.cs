using UnityEngine;
using System.Collections;

public class PlayerStepForward : MonoBehaviour
{
    [Header("ÂªÀº ÀüÁø")]
    [SerializeField] private float stepDistance = 0.35f;
    [SerializeField] private float stepDuration = 0.06f;

    private Coroutine stepCoroutine;

    public void AnimEvent_StepForward()
    {
        if (stepCoroutine != null)
            StopCoroutine(stepCoroutine);

        stepCoroutine = StartCoroutine(StepForwardRoutine());
    }

    private IEnumerator StepForwardRoutine()
    {
        Vector2 startPos = transform.position;
        Vector2 targetPos = startPos + Vector2.right * stepDistance;

        float elapsed = 0f;

        while (elapsed < stepDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / stepDuration;

            Vector2 currentPos = Vector2.Lerp(startPos, targetPos, t);
            transform.position = new Vector3(currentPos.x, currentPos.y, transform.position.z);

            yield return null;
        }

        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        stepCoroutine = null;
    }
}