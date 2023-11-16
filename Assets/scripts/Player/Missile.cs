using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    [SerializeField] private float _speed = 10f;
    private bool _isEnemyMissile;
    private bool _playerMissileRadar = false;
    private bool _isPowerupActive = false;
    private float _enemyMissileRange = -4f;
    private float _playerMissileRange = 3.5f;
    private Animator _missileExplosion;
    private BoxCollider2D _missileCollider;
    private Enemy _enemy;
    private Player _player;
    private Transform Enemy;
    private Transform Player;
    private SmartEnemy _smartEnemy;
    private AggressiveEnemy _aggressiveEnemy;
    private AvoidShot _avoidShot;
    private Enemy _fastenemy;


    [SerializeField] Collider2D[] affectedColliders = new Collider2D[20];

    void Start()
    {
        _missileCollider = GetComponent<BoxCollider2D>();
        _missileExplosion = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_isEnemyMissile == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    //check which object its hitting, add score, damage, change to homing for player and enemy (section requirement)    
    public void MoveUp()
    {
        if (_playerMissileRadar == true)
        {
            HomingActive();
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

         if (transform.position.y > 8.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void HomingActive()
    {
        if (_isPowerupActive && _playerMissileRange < 4)
        {
            _speed += 1;

            Vector3 direction = _enemy.transform.position - transform.position;
            direction = direction.normalized;

            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -3.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignPlayerMissile()
    {
        _isEnemyMissile = false;
    }

    public void AssignEnemyMissile()
    {
        _isEnemyMissile = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyMissile == true)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
         if (other.tag == "Enemy" && _isEnemyMissile == false)
         {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Damage();
                }
         }
         if (other.tag == "FastEnemy" && _isEnemyMissile == false)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage();
            }
        }
         if (other.tag == "SmartEnemy" && _isEnemyMissile == false)
        {
            SmartEnemy smartEnemy = other.GetComponent<SmartEnemy>();
            if (smartEnemy != null)
            {
                smartEnemy.Damage();
            }
        }
         if (other.tag == "AggroEnemy" && _isEnemyMissile == false)
        {
            AggressiveEnemy aggressiveEnemy = other.GetComponent<AggressiveEnemy>();
            if (aggressiveEnemy != null)
            {
                aggressiveEnemy.Damage();
            }
        }
        if (other.tag == "AvoidShot" && _isEnemyMissile == false)
        {
            AvoidShot avoidShot = other.GetComponent<AvoidShot>();
            if (avoidShot != null)
            {
                avoidShot.Damage();
            }
        }

    }


}
