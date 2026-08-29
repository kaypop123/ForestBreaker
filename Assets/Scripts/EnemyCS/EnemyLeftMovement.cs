using UnityEngine;
using System.Collections;
public class EnemyLeftMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 2f;   

    private Rigidbody2D rb;
    private bool isKnockback; 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isKnockback)
        {
            MoveLeft();
        }
    }

    private void MoveLeft()
    {
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
    }

    // 플레이어가 호출할 넉백 함수
    public void GetKnockback(float force, float duration)
    {
        if (isKnockback) return; 
        StartCoroutine(KnockbackRoutine(force, duration));
    }

    private IEnumerator KnockbackRoutine(float force, float duration)
    {
        isKnockback = true;

      
        rb.linearVelocity = Vector2.zero; 
        rb.AddForce(Vector2.right * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        isKnockback = false;
    }
}