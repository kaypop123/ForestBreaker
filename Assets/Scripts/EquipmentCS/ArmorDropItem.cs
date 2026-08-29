using UnityEngine;

public class ArmorDropItem : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private string playerTag = "Player";

    private Transform target;

    private void Start()
    {
        FindTarget();
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );
    }

    private void FindTarget()
    {
        GameObject player = GameObject.FindWithTag(playerTag);
        if (player != null)
        {
            target = player.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        if (EquipmentDataManager.Instance != null)
        {
            EquipmentDataManager.Instance.UnlockArmor();
        }

        Debug.Log("∞©ø  æ∆¿Ã≈€ »πµÊ");
        Destroy(gameObject);
    }
}