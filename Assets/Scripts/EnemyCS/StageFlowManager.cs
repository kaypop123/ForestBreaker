using UnityEngine;

[System.Serializable]
public class StagePoint
{
    public Transform playerMovePoint;
    public Transform enemySpawnPoint;
    public Transform cameraCenterPoint;
    public int enemyCount = 3;
}

public class StageFlowManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private SmoothFollowCam cameraFollow;
    [SerializeField] private StagePoint[] stagePoints;

    [Header("Boss Timeline Object")]
    [SerializeField] private GameObject bossTimelineObject;

    private int currentStageIndex = 0;
    private bool isBossTimelineActivated = false;

    private void Start()
    {
        if (enemySpawner != null)
        {
            enemySpawner.OnAllEnemiesCleared += MoveToNextStage;
        }

        ApplyStage(currentStageIndex);
    }

    private void OnDestroy()
    {
        if (enemySpawner != null)
        {
            enemySpawner.OnAllEnemiesCleared -= MoveToNextStage;
        }
    }

    public void MoveToNextStage()
    {
        currentStageIndex++;

        if (currentStageIndex >= stagePoints.Length)
        {
            ActivateBossTimeline();
            return;
        }

        ApplyStage(currentStageIndex);
    }

    private void ApplyStage(int index)
    {
        if (index < 0 || index >= stagePoints.Length)
            return;

        StagePoint stage = stagePoints[index];

        Debug.Log($"--- {index + 1} 스테이지 시작 ---");

        if (stage.playerMovePoint != null)
            player.MoveTo(stage.playerMovePoint.position);

        if (stage.enemySpawnPoint != null)
            enemySpawner.transform.position = stage.enemySpawnPoint.position;

        if (cameraFollow != null && stage.cameraCenterPoint != null)
            cameraFollow.SetCenterPoint(stage.cameraCenterPoint);

        enemySpawner.StartNextStage(index + 1, stage.enemyCount);
    }

    private void ActivateBossTimeline()
    {
        if (isBossTimelineActivated)
            return;

        isBossTimelineActivated = true;

        if (bossTimelineObject != null)
        {
            bossTimelineObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("bossTimelineObject가 연결되지 않았습니다.");
        }
    }
}