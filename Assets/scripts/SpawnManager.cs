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

   
    private int currWave;
    private int _waveValue;
    private int _enemyCount;
    private int _waveTotal;

    private Transform spawnLocation;
    private int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    //set enemy spwnn couroutine have set numbr of enemies
    //wave value = currwave *10
    //in coroutine while currcount is less than wave value spanwn enemy
    //second system for enemy checking enemy count
    //when enemy count hits 0 start next wave

   
    public void StartSpawning()
    {
        currWave = 1;
        _waveValue = 10;
        _enemyCount = 0;
        _waveTotal = _waveValue;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(3.0f);

            while (_stopSpawning == false && _waveValue > 0)
            {
                int randomEnemy = UnityEngine.Random.Range(0, _enemyPrefab.Length);
                Vector3 _enemySpawnPos = GetEnemySpawnPos(randomEnemy);
                GameObject _enemy = Instantiate(_enemyPrefab[randomEnemy], _enemySpawnPos, Quaternion.identity);

                _enemy.transform.parent = _enemyContainer.transform;
                Enemy _enemyScript = _enemy.GetComponent<Enemy>();
                // _enemyScript.SetID(randomEnemy);

                _enemy.transform.parent = _enemyContainer.transform;
                _waveValue--;
                _enemyCount++;
                yield return new WaitForSeconds(5.0f);

            }
            if(_enemyCount <= 0)
            {
                currWave++;
                _waveValue = currWave * 10;

            }
        }
    }
    public void EnemyDeath()
    {
        _enemyCount--;
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