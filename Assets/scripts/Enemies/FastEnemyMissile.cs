using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemyMissile : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    private bool _isEnemyMissile = true;  
    private Animator _missileExplosion;
    private BoxCollider2D _missileCollider;
    // Start is called before the first frame update
    void Start()
    {
        _missileCollider = GetComponent<BoxCollider2D>();
        _missileExplosion = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveDown();
        if (transform.position.y < -6.5)
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

    public void AssignEnemyMissile()
    {
        _isEnemyMissile = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

        }
    }
}
