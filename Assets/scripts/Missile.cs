using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    [SerializeField] private float _speed = 12f;
    private bool _isEnemyMissile = false;
    BoxCollider2D _missileCollider;
    private float _enemyMissileRange = -4f;
    private float _playerMissileRange = 7.5f;

    void Start()
    {
        _missileCollider = GetComponent<BoxCollider2D>();
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

    //explode range 7.5 to 8.5
    public void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y < 9f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }
    
    //explode range -4 to -6.12
    public void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4f)
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

        _missileCollider.edgeRadius += 3f;
    }

    

}
