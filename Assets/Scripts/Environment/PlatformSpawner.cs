using UnityEngine;
using Manager;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private List<MovePlatform> platforms;

    [Header("Platform Prefab")]
    [SerializeField] private MovePlatform platformPrefab;

    [Header("Platform Spawn Setting")]
    [SerializeField, ReadOnlyField] private float spawnTimer = float.MaxValue;
    [Range(1.0f, 5.0f)]
    [SerializeField] private float spawnCycleTime;
    [SerializeField] private float spawnPositionX;
    // [SerializeField] private int spawnPlatformCount;

    [Header("Reposition Setting")]
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;

    private void Awake()
    {
        platforms = GetComponentsInChildren<MovePlatform>().ToList();

        GameManager.Instance.OnInitializeGame += Initialize;
    }

    private void Start()
    {
        if (platforms.Count > 0)
        {
            foreach (var platform in platforms)
            {
                RepositionPlatform(platform.gameObject);
                platform.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.IsGameOver == true)
            return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnCycleTime)
            return;

        SpawnPlatform();
    }

    private void SpawnPlatform()
    {
        // 타이머 초기화
        spawnTimer = 0.0f;

        // 비활성화된 플랫폼이 있으면 재사용
        for (int index = 0; index < platforms.Count; ++index)
        {
            if (platforms[index].gameObject.activeSelf == false)
            {
                RepositionPlatform(platforms[index].gameObject);

                platforms[index].gameObject.SetActive(true);
                return;
            }
        }

        // 없으면 새로 생성
        MovePlatform platform = Instantiate(platformPrefab, this.transform);
        RepositionPlatform(platform.gameObject);
        platforms.Add(platform);
        platform.gameObject.SetActive(true);

        return;
    }

    private void RepositionPlatform(GameObject platform)
    {
        float randomY = Random.Range(minHeight, maxHeight);
        platform.transform.position = new Vector2(spawnPositionX, randomY);
    }

    private void Initialize()
    {
        spawnTimer = float.MaxValue;
    }
}
