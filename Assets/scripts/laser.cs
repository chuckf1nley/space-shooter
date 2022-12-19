using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    [SerializeField]
    private float VerticalMovement;
    private float _speed = 8f;
      
   

    // Update is called once per frame

    void Update()
   //void CalculateMovement()
    {
       
        transform.Translate(Vector3.up * _speed * Time.deltaTime);


        //if ;aser position is greater thsn 8 on the y
        //destroy the object

        if (transform.position.y > 9f)
        {
            Destroy(this.gameObject);
        }    
    
    } 
    

}
