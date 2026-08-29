using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 targetPosition;
    private bool isMoving = false;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 currentPos = transform.position;

            // x축만 이동
            float newX = Mathf.MoveTowards(currentPos.x, targetPosition.x, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, currentPos.y, currentPos.z);

            // x축 기준 도착 체크
            if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.1f)
            {
                StopMoving();
            }
        }
    }

    public void MoveTo(Vector3 destination)
    {
        // 현재 y,z 유지하고 x만 목표로 사용
        targetPosition = new Vector3(destination.x, transform.position.y, transform.position.z);
        isMoving = true;

        if (anim != null)
        {
            anim.SetBool("IsWalking", true);
        }
    }

    private void StopMoving()
    {
        isMoving = false;

        if (anim != null)
        {
            anim.SetBool("IsWalking", false);
        }

        Debug.Log("플레이어가 다음 구역에 도착했습니다.");
    }

    public bool IsMoving() => isMoving;
}