using UnityEngine;

public class SlashWave : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float lifeTime = 2f;
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    private void Update()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }
}
