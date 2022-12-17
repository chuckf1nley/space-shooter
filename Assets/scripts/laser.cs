using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    [SerializeField]
    private float VerticalMovement;
    private float _speed = 8f;

    //speed variable up
    //laser up

    // Update is called once per frame

    void Update()
   //void CalculateMovement()
    { 
    float verticalMovement;
    
    
         transform.Translate(Vector3.up * _speed * Time.deltaTime);
    } 
    


}