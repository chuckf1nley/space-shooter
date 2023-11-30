using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    private float _fireRate = 2f;
    private bool _isWeaponActive = false;


    // Update is called once per frame
    void Update()
    {
        _isWeaponActive = true;

    }


    public void AssignBossWeaponA()
    {
        
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -9f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
        Destroy(this.gameObject);
    }

}
