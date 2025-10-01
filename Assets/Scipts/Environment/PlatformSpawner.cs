using UnityEngine;
using Manager;

public class PlatformSpawner : MonoBehaviour
{
    private MovePlatform[] platforms;

    private int maxPlatformCount;
    private int lastSpawnPlatformIndex = 0;

    private float lastSpawnPlatformTime = float.MinValue;

    [Header("Platform Spawn Setting")]
    [SerializeField] private float spawnDelayTime;
    [SerializeField] private float spawnPositionX;
    [SerializeField] private int spawnPlatformCount;

    [Header("Reposition Setting")]
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;

    private void Awake()
    {
        platforms = GetComponentsInChildren<MovePlatform>();

        maxPlatformCount = platforms.Length;
    }

    private void Update()
    {
        if (Time.time - lastSpawnPlatformTime < spawnDelayTime)
            return;

        SpawnPlatform(platforms[lastSpawnPlatformIndex]);
        lastSpawnPlatformIndex = (lastSpawnPlatformIndex + 1) % maxPlatformCount;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            // 플랫폼 리포지셔닝
            RepositionPlatform(other.gameObject);
        }
    }

    private void SpawnPlatform(MovePlatform platform)
    {
        // 플랫폼 추가 스폰(활성화)
        platform.gameObject.SetActive(true);
    }

    public void RepositionPlatform(GameObject platform)
    {
        float randomY = Random.Range(minHeight, maxHeight);
        platform.transform.position = new Vector2(spawnPositionX, randomY);
    }
}
