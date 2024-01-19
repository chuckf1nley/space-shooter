using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    private GameObject _player;
    private float _fireRate = 2f;
    private bool _isWeaponActive = false;


    private void Start()
    {
        _isWeaponActive = true;
        _player = GameObject.Find("Player");
    }


    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void AssignBossFlameThrower()
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            other.GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }
    }

}
