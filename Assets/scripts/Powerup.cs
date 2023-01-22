using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{ 
    private float vertical; 
    private float _speed = 3f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //move down at speed of 3 (adjust in the inspector)
        //when reaches bottom of screen
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }


    //OnTriggerCollision
    //only be collectable by player
    //on collected destroy object

}
