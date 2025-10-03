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
    private float spawnTimer = float.MaxValue;
    [SerializeField] private float spawnCycleTime;
    [SerializeField] private float spawnPositionX;
    [SerializeField] private int spawnPlatformCount;

    [Header("Reposition Setting")]
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;

    private void Awake()
    {
        platforms = GetComponentsInChildren<MovePlatform>().ToList();
    }

    private void Start()
    {
        Debug.Log(platforms.Count);
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
        if (PlatformManager.Instance.CanMovePlatform == false)
            return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnCycleTime)
            return;

        SpawnPlatform();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // 플랫폼 리포지셔닝
            RepositionPlatform(other.gameObject);
        }
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
                platforms[index].gameObject.SetActive(true);
                return;
            }
        }

        // 비활성화된 플랫폼이 없으면 새로 생성
        Debug.Log("There is no available platform, spawning a new one.");
        MovePlatform platform = Instantiate(platformPrefab, this.transform);
        RepositionPlatform(platform.gameObject);
        platform.gameObject.SetActive(true);
        platforms.Add(platform);

        return;
    }

    public void RepositionPlatform(GameObject platform)
    {
        float randomY = Random.Range(minHeight, maxHeight);
        platform.transform.position = new Vector2(spawnPositionX, randomY);
    }
}
