using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Wave;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject waveListManager;
    private List<Wave> waveList;
    [SerializeField] private Transform spawnPoint;
    public static EnemySpawner Instance { get; private set; }

    private int enemyCount = 0;
    private Coroutine spawnEnemy;
    [SerializeField] private TextMeshProUGUI levelText;
    private int level;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        level = 0;
        InitWaveList();
    }

    private void InitWaveList()
    {
        Wave[] temp = waveListManager.GetComponentsInChildren<Wave>();
        waveList = new List<Wave>(temp);
        spawnEnemy = StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        foreach (Wave wave in waveList)
        {
            level++;
            UpdateUI();
            foreach (EnemyType enemy in wave.enemyList)
            {
                yield return new WaitForSeconds(enemy.spawnDelay);
                for (int i = 0; i < enemy.count; i++)
                {
                    Instantiate(enemy.enemyPrefab, spawnPoint.position, Quaternion.identity);
                    enemyCount++;
                    if (i < enemy.count - 1)
                    {
                        yield return new WaitForSeconds(enemy.rate);
                    }
                }
            }
            while (enemyCount > 0)
            {
                yield return null;
            }
        }
        yield return null;

        while (enemyCount > 0)
        {
            yield return new WaitForSeconds(0.2f);
        }

        GameManager.Instance.Success();
    }

    public void DecreaseEnemyCount()
    {

        if (enemyCount > 0)
        {
            enemyCount--;
        }
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnEnemy);
    }

    private void UpdateUI()
    {
        levelText.text = "Level: " + level;
    }

    public int getLevel() => level;

}
