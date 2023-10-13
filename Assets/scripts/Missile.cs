using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    [SerializeField] private float _speed = 12f;
    private bool _isEnemyMissile;
    private float _enemyMissileRange = -4f;
    private float _playerMissileRange = 7.5f;
    private Animator _missileExplosion;
    private BoxCollider2D _missileCollider;
    private Enemy _enemy;
    private Player _player;

    [SerializeField] private float _explosionForce = 3.5f;
    [SerializeField] private float _explosionRadius = 3.5f;
    [SerializeField] ContactFilter2D contactFilter;
    [SerializeField] Collider2D[] affectedColliders = new Collider2D[20];

    void Start()
    {

        _missileCollider = GetComponent<BoxCollider2D>();
        _missileExplosion = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            Explode();
        }

    }
    //check which object its hitting, add score, damage, change to homing for player and enemy (section requirement)
    void Explode()
    {
        int numColliders = Physics2D.OverlapCircle(transform.position, _explosionRadius, contactFilter, affectedColliders);
        if (numColliders > 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                Destroy(affectedColliders[i].gameObject);
            }
        }

    }

    //explode range 7.5 to 8.5
    public void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8.5f)
        {
            if (transform.parent != null)
            {
                Explode();
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }
    
    //explode range -4 to -6.12
    public void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -3.5f)
        {
            if (transform.parent != null)
            {
                Explode();
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
            else if (other.tag == "Enemy" && _isEnemyMissile == false)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Damage();
                }
            }
        }

    }


}
