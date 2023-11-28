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

    }


    public void AssignBossWeaponA()
    {
        _isWeaponActive = true;
    }

}
