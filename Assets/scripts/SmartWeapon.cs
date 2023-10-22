using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartWeapon : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private Player _player;
    private SmartEnemy _smartEnemy;
    private float _playerDistance;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _smartEnemy = GameObject.Find("Smart Enemy").GetComponent<SmartEnemy>();

        if(_player != null)
        {
            Debug.Log("player is null on smartweapon");
        }
        if(_smartEnemy != null)
        {
            Debug.Log("Smart Enemy is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _playerDistance = Vector3.Distance(transform.position, _player.transform.position);
        if (_playerDistance >= 0)
        {
            Weapon();
        }
    }

    public void Weapon()
    {

        if (_playerDistance > 5)
            _speed += 1;

        Vector3 direction = _player.transform.position - transform.position;
        direction = direction.normalized;

        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        if (transform.position.y > 9f)
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
