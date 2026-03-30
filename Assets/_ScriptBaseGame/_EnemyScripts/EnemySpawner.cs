using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private DifficultyManager difficultyManager;

    public void Initialize(DifficultyManager dm)
    {
        difficultyManager = dm;
        Debug.Log($"[Spawner] Initialized with DifficultyManager: {(dm != null)}");
    }

    void Awake()
    {
        // Fallback if Initialize() wasn’t called
        if (difficultyManager == null)
        {
            difficultyManager = UnityEngine.Object.FindFirstObjectByType<DifficultyManager>();
            Debug.Log("[Spawner] Auto-found DifficultyManager in Awake.");
        }
    }

    // Called by WaveManager
    public GameObject SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("[Spawner] No enemyPrefab assigned!");
            return null;
        }

        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Debug.Log($"[Spawner] Enemy spawned at {transform.position}");
        return enemy;
    }
}
