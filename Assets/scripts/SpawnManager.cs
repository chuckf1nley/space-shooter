using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;      
    private bool _stopSpawning = false;
    private int _enemyID;
    private Vector3 _enemySpaawnPos;
    private float Range;
    private float Length;

    public List<Enemy> enemies = new List<Enemy>();
    public int currWave;
    public int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform spawnLocation;
    public int waveDuration;
    public float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        GenerateWave();
    }

    void FixedUpdate()
    {
        if (spawnTimer < -0)
        {
            if(enemiesToSpawn.Count >0)
            {
                Instantiate(enemiesToSpawn[0], spawnLocation.position, Quaternion.identity);
                enemiesToSpawn.RemoveAt(0);
                spawnTimer = spawnInterval;
            }
            else
            {
                waveTimer = 0;
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
    }

    public void GenerateWave()
    {
        waveValue = currWave * 10;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count;
        waveTimer = waveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0)
        {
            int randEnemyId = UnityEngine.Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost > -0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if(waveValue <= 0)
            {
                break;
            }
        
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }


    [System.Serializable]
    public class Enemy
    {
        public GameObject enemyPrefab;
        public int cost;
    
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            int randomEnemy = UnityEngine.Random.Range(0, _enemyPrefab.Length);
            Vector3 _enemySpawnPos = GetEnemySpawnPos(randomEnemy);
            GameObject _enemy = Instantiate(_enemyPrefab[randomEnemy], _enemySpawnPos, Quaternion.identity);

            _enemy.transform.parent = _enemyContainer.transform;
            Enemy _enemyScript = _enemy.GetComponent<Enemy>();
          // _enemyScript.SetID(randomEnemy);

            _enemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-18f, 18f), 6, 0);           
            int randomPowerup = UnityEngine.Random.Range(0, 6);
            GameObject newPowerup = Instantiate(_powerups[randomPowerup], new Vector3(UnityEngine.Random.Range(-18f, 18f), 6, 0), Quaternion.identity);
           // Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(3, 8));
        }

    }

    private Vector3 GetEnemySpawnPos(int EnemyID)
    {
        float _xSpawnPos;
        float _ySpawnPos;
        Vector3 _enemySpawnPos;

        switch (_enemyID)
        {
            case 1:
                _xSpawnPos = UnityEngine.Random.Range(-18f, 18f);
                _ySpawnPos = UnityEngine.Random.Range(9.11f, 8f);
                _enemySpawnPos = new Vector3(_xSpawnPos, _ySpawnPos, 0f);
                break;

            case 2:
                _xSpawnPos = UnityEngine.Random.Range(18f, 18f);
                _ySpawnPos = UnityEngine.Random.Range(9.11f, 8f);
                _enemySpawnPos = new Vector3(_xSpawnPos, _ySpawnPos, 0f);
                break;

            default:
                _xSpawnPos = Mathf.Round(UnityEngine.Random.Range(-9.0f, 9.0f));
                _enemySpawnPos = new Vector3(_xSpawnPos, 9.11f, 0f);
                break;
        }
        return _enemySpawnPos;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}