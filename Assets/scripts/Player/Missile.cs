using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    [SerializeField] private float _speed = 10f;
    private bool _isEnemyMissile;
    private float _enemyMissileRange = -4f;
    private float _playerMissileRange = 7.5f;
    private Animator _missileExplosion;
    private BoxCollider2D _missileCollider;
    private Enemy _enemy;
    private Player _player;
    private Transform Enemy;
    private Transform Player;

   
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] Collider2D[] affectedColliders = new Collider2D[20];

    void Start()
    {

        _missileCollider = GetComponent<BoxCollider2D>();
        _missileExplosion = GetComponent<Animator>();
        //Player = GameObject.FindGameObjectWithTag("Player").transform;
        //Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

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
   

    //explode range 7.5 to 8.5
    public void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
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

    }


}