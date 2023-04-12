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
           Vector3 posToSpawn = new Vector3(UnityEngine.Random.Range(-18f, 18f), 6, 0);
           // float randomPowerup = UnityEngine.Random.Range(0.0f, 7.0f);
            int randomPowerup = UnityEngine.Random.Range(0, 6);
            GameObject newPowerup = Instantiate(_powerups[randomPowerup], new Vector3(UnityEngine.Random.Range(-18f, 18f), 6, 0), Quaternion.identity);
            //Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
           yield return new WaitForSeconds(UnityEngine.Random.Range(3, 8));
        }
    
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}