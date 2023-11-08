using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartWeapon : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _chaseSpeed = 2.5f;
    private Player _player;
    private GameObject Player;
    private GameObject Projectile;
    private float _playerDistance;
    private float _interceptDistance;
    private bool _isPlayerAlive;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _isPlayerAlive = true;
      

        if(_player == null)
        {
            Debug.Log("player is null on smartweapon");
        }

        //Debug.Break();
    }

    // Update is called once per frame
    void Update()
    {
            if (_playerDistance >= -1 )
            {
                Weapon();
            }
       
        float step = _speed * Time.deltaTime;
        Projectile.transform.position = Vector3.MoveTowards(Projectile.transform.position, Player.transform.position, _speed);

    }

    public void Weapon()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
       
        if (transform.position.y > 9f)
        {            
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        if(transform.position.x > 18f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        else if (transform.position.x < -18f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);

            }
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isPlayerAlive == true)
        {
            if (other.tag == "Player")
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                    _isPlayerAlive = false;
                }
            }
        }
    }


}
