using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private Transform player;

    [Header("도달 좌표 리스트")]
    [SerializeField] private List<float> targetXList = new List<float>();

    [Header("활성화할 콜라이더 오브젝트 리스트")]
    [SerializeField] private List<GameObject> colliderObjects = new List<GameObject>();

    private int currentIndex = 0;

    private void Start()
    {
        for (int i = 0; i < colliderObjects.Count; i++)
        {
            if (colliderObjects[i] != null)
                colliderObjects[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        if (currentIndex >= targetXList.Count || currentIndex >= colliderObjects.Count)
            return;

        if (player.position.x >= targetXList[currentIndex])
        {
            if (colliderObjects[currentIndex] != null)
                colliderObjects[currentIndex].SetActive(true);

            currentIndex++;
        }
    }
}