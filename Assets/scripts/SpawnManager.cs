using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
            Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-18f, 18f), 6, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning  == false)
        {
           Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-10f, 10f), 7, 0);
            int randomPowerup = UnityEngine.Random.Range(0, 3);
           Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
           yield return new WaitForSeconds(UnityEngine.Random.Range(3, 8));
        }
    
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}