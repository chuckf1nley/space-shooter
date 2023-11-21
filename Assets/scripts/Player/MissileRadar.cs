using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRadar : MonoBehaviour
{
    [SerializeField] private Missile _parent;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            _parent.MoveUp();
        }
       
    }



}
