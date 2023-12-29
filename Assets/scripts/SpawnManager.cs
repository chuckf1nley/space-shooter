using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _powerupContainer;
    [SerializeField] private GameObject _bossContainer;
    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private int _currWave;
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private GameObject _bossHealthBar;

    private bool _stopSpawning = false;
    private bool _isRegularWave = false;
    private bool _isBossActive = false;
    private float _spawnPowerupDelay = 3f;
    private UIManager _uiManager;

    private int _enemyID;
    private int currWave;
    private int _waveValue;
    private int _enemyCount;
    private int _waveTotal;


    //private object NewWaveDisplay ShowWaveText();

    void Start()
    {
        _uiManager = Object.FindObjectOfType<UIManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

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

    public void StopSpawning()
    {
        if (_isBossActive == true)
        {
            _isBossActive = true;
            _stopSpawning = true;
            _isRegularWave = false;
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {

        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            float randomXPosition = Random.Range(-18f, 18f);
            Vector3 RandomPosition = new Vector3(randomXPosition, 8f, transform.position.z);
            int randomPowerup = GeneratePowerupIndex(Random.Range(0, 80));

            GameObject newPowerup = Instantiate(_powerups[randomPowerup], RandomPosition, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(_spawnPowerupDelay);


        }

    }
    public int GeneratePowerupIndex(int random)
    {
        if (random >= 0 && random < 10)
        {
            return 0; //tripleshot
        }
        else if (random >= 10 && random < 20)
        {
            return 1; //speed boost
        }
        else if (random >= 20 && random < 30)
        {
            return 2; // shield
        }
        else if (random >= 30 && random < 40)
        {
            return 3; //ammo
        }
        else if (random >= 40 && random < 50)
        {
            return 4; //health
        }
        else if (random >= 50 && random < 60)
        {
            return 5; // altfire
        }
        else if (random >= 60 && random > 70)
        {
            return 6; // negspeed
        }
        else if (random >= 70 && random > 80)
        {
            return 7; //homing Missile
        }
        {
            return 3;
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
                _xSpawnPos = Random.Range(-18f, 18f);
                _ySpawnPos = Random.Range(9.11f, 8f);
                _enemySpawnPos = new Vector3(_xSpawnPos, _ySpawnPos, 0f);
                break;

            case 2:
                _xSpawnPos = Random.Range(-18f, 18f);
                _ySpawnPos = Random.Range(9.11f, 8f);
                _enemySpawnPos = new Vector3(_xSpawnPos, _ySpawnPos, 0f);
                break;

            case 3:
                _xSpawnPos = Random.Range(-18f, 18f);
                _ySpawnPos = Random.Range(9.11f, 8f);
                _enemySpawnPos = new Vector3(_xSpawnPos, _ySpawnPos, 0f);
                break;

            case 4:
                _xSpawnPos = Random.Range(-18f, 18f);
                _ySpawnPos = Random.Range(9.11f, 8f);
                _enemySpawnPos = new Vector3(_xSpawnPos, _ySpawnPos, 0f);
                break;

            case 5:
                _xSpawnPos = Random.Range(-18f, 18f);
                _ySpawnPos = Random.Range(9.11f, 8f);
                _enemySpawnPos = new Vector3(_xSpawnPos, _ySpawnPos, 0f);
                break;

            default:
                _xSpawnPos = Mathf.Round(Random.Range(-9.0f, 9.0f));
                _enemySpawnPos = new Vector3(_xSpawnPos, 9.11f, 0f);
                break;
        }
        return _enemySpawnPos;
    }



    IEnumerator SpawnEnemyRoutine()
    {
       
        yield return new WaitForSeconds(3.0f);

        while (_waveValue > 0)
        {
            _isRegularWave = true;
            int randomEnemy = GenerateEnemyIndex(Random.Range(0, 50));
            Vector3 enemySpawnPos = GetEnemySpawnPos(randomEnemy);
            GameObject _enemy = Instantiate(_enemyPrefab[randomEnemy], enemySpawnPos, Quaternion.identity);

            _enemy.transform.parent = _enemyContainer.transform;
            _waveValue--;
            _enemyCount++;
            yield return new WaitForSeconds(5.0f);
        }
        while (_enemyCount > 0)
        {
            yield return null;
        }
      

        currWave++;
        _waveValue = currWave * 10;

        if (_isRegularWave == false || _isBossActive == true)

        StartCoroutine(WaitToStartNewWaveCouroutine());

    }


    public void EnemyDeath()
    {
        _enemyCount--;
    }
    public int GenerateEnemyIndex(int random)
    {
        if (random >= 0 && random < 10)
        {
            return 0; //normalEnemy
        }
        else if (random >= 10 && random < 20)
        {
            return 1; //fastEnemy
        }
        else if (random >= 20 && random < 30)
        {
            return 2; //aggroEnemy
        }
        else if (random >= 30 && random < 40)
        {
            return 3; //smartEnemy
        }
        else if (random >= 40 && random < 50)
        {
            return 4; //AvoidShotenemy
        }
        else
        {
            return 0;
        }

    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public IEnumerator WaitToStartNewWaveCouroutine()
    {
        _uiManager.UpdateWave(_currWave);
        WaitForSeconds wait = new WaitForSeconds(3);
        while (_enemyContainer.transform.childCount > 0)
        {
            yield return null;
        }
        //check wave number if wave 10 boss wave
        yield return wait;
       
            if (_currWave != 2)
            {
                SpawnEnemyRoutine();
            }
            else 
            {
                StopSpawning();
                SpawnBoss();
            }
        
    }

    public void SpawnBoss()
    {
        _isRegularWave = false;
        new WaitForSeconds(3);
        Vector3 startPos = new Vector3(0, 1, 0);
        _isBossActive = true;

        Instantiate(_bossPrefab, startPos, Quaternion.identity);
        _uiManager.BossHealth();

        //foreach (BoxCollider2D - c - in -Boss.GetComponents<BoxCollider2D>()) ;
        //c.enabled = true;
    }

}