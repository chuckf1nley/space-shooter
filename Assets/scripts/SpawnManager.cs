using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefab;
   // [SerializeField] private GameObject[] _AggroEnemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
    private bool _stopSpawning = false;
    private int _enemyID;
    private Vector3 _enemySpawnPos;
    private float _spawnPowerupDelay = 3f;
    private float Range;
    private float Length;

    private int currWave;
    private int _waveValue;
    private int _enemyCount;
    private int _waveTotal;

    private Transform spawnLocation;
    

    // Start is called before the first frame update
    void Start()
    {

    }

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
                int randomEnemy = GenerateEnemyIndex(UnityEngine.Random.Range(0, 30));
                Vector3 _enemySpawnPos = GetEnemySpawnPos(randomEnemy);
                GameObject _enemy = Instantiate(_enemyPrefab[randomEnemy], _enemySpawnPos, Quaternion.identity);
                //GameObject _AggroEnemy = Instantiate(_AggroEnemyPrefab[randomEnemy], _enemySpawnPos, Quaternion.identity);

                _enemy.transform.parent = _enemyContainer.transform;
                //_AggroEnemy.transform.parent = _enemyContainer.transform;
                //Enemy _enemyScript = _enemy.GetComponent<Enemy>();
                //AggressiveEnemy _AggressiveEnemyScript = _AggroEnemy.GetComponent<AggressiveEnemy>(); 

                _enemy.transform.parent = _enemyContainer.transform;
                _waveValue--;
                _enemyCount++;
                yield return new WaitForSeconds(5.0f);

            }
            if (_enemyCount <= 0)
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
            float randomXPosition = UnityEngine.Random.Range(-18f, 18f);
            Vector3 RandomPosition = new Vector3(randomXPosition, 8f, transform.position.z);

            int randomPowerup = GeneratePowerupIndex(UnityEngine.Random.Range(0, 70));

            GameObject newPowerup = Instantiate(_powerups[randomPowerup], RandomPosition, Quaternion.identity);
            newPowerup.transform.parent = this.transform;
            yield return new WaitForSeconds(_spawnPowerupDelay);
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
                _xSpawnPos = UnityEngine.Random.Range(-18f, 18f);
                _ySpawnPos = UnityEngine.Random.Range(9.11f, 8f);
                _enemySpawnPos = new Vector3(_xSpawnPos, _ySpawnPos, 0f);
                break;

            case 3:
                _xSpawnPos = UnityEngine.Random.Range(-18f, 18f);
                _ySpawnPos = UnityEngine.Random.Range(-18f, 18f);
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

    public int GeneratePowerupIndex(int random)
    {
        if (random >= 0 && random < 10)
        {
            return 0; //tripleshot
        } else if (random >= 10 && random < 20)
        {
            return 1; //speed boost
        } else if (random >= 20 && random < 30)
        {
            return 2; // shield
        } else if (random >= 30 && random < 40)
        {
            return 3; //ammo
        } else if (random >= 40 && random < 50)
        {
            return 4; //health
        } else if (random >= 50 && random < 60)
        {
            return 5; // altfire
        } else if (random >= 60 && random > 70)
        {
            return 6; // negspeed
        }
        { 
            return 3;
        }
    }

    public int GenerateEnemyIndex(int random)
    {
        if (random >= 0 && random < 10)
        {
            return 0; //normalEnemy
        }else if (random >= 10 && random < 20)
        {
            return 1; //fastEnemy
        }else if (random >= 20 && random <30)
        {
            return 2; //aggroEnemy
        }
        else
        {
            
            return 0;
        }
    }
}