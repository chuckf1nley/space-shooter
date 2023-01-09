using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private object EnemyPrefab;
    private object GameObject;
    private float _time = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //while
        {
            //Instantiate(GameObject && Time.time);
        }

    }

    //spawn game objects every 5 seconds
    //create a coroutine of type IEnumerator -- Yield Events
    //while loop

    IEnumerator SpawnRoutine()
    {
        //while loop (infinite loop)
        //instantiate enemy prefab
        //yield wait for 5 seconds
               
        int Enemy = 5;
        while (Enemy > 0)
        //Instantiate(EnemyPrefab (&& _time ));
        yield return new WaitForSeconds(5);
    }

       internal class Instantiate
    {
        Instantiate Enemy;

        internal Instantiate Enemy1 { get => Enemy; set => Enemy = value; }
    }
}