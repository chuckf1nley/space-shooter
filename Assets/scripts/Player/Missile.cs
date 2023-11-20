using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    [SerializeField] private float _speed = 8f;
    private bool _playerMissileRadar = true;
    private bool _isPowerupActive = false;
    private float _interceptDistance = 2.5f;
    private Animator _missileExplosion;
    private GameObject _enemy;    
    private Vector3 _direction;

    void Start()
    {
        //nrf lines 25, 43
        _enemy = GameObject.FindGameObjectWithTag("Enemy");
        

        if (_enemy == null)
        {
            Debug.Log("Enemy is null on missile");
        }

        _playerMissileRadar = true;
        _missileExplosion = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_enemy != null)
        {
            _interceptDistance = Vector3.Distance(transform.position, _enemy.transform.position);
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (_interceptDistance > 4)
            {
                MoveUp();
            }
            else if (_interceptDistance < 4)
            {
                HomingActive();
            }
        }
        else
        {
            MoveUp();
        }
        if (transform.position.y > 9.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    //check which object its hitting, add score, damage, change to homing for player and enemy (section requirement)    
    public void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    public void HomingActive()
    {
       
        if (_isPowerupActive && _interceptDistance < 4)
        _direction = _enemy.transform.position - transform.position;
        _direction.Normalize();
        transform.Translate(_direction * _speed * Time.deltaTime);
        _playerMissileRadar = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage();
            }
        }
        if (other.tag == "FastEnemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage();
            }
        }
        if (other.tag == "SmartEnemy")
        {
            SmartEnemy smartEnemy = other.GetComponent<SmartEnemy>();
            if (smartEnemy != null)
            {
                smartEnemy.Damage();
            }
        }
        if (other.tag == "AggroEnemy")
        {
            AggressiveEnemy aggressiveEnemy = other.GetComponent<AggressiveEnemy>();
            if (aggressiveEnemy != null)
            {
                aggressiveEnemy.Damage();
            }
        }
        if (other.tag == "AvoidShot")
        {
            AvoidShot avoidShot = other.GetComponent<AvoidShot>();
            if (avoidShot != null)
            {
                avoidShot.Damage();
            }
        }

    }

}
