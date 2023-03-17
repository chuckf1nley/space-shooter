using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _targetPlayer;
    [SerializeField] private float _speed = 2.5f;
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _targetPlayer = _player.transform.position;
        }

        else
        {
            Debug.LogError("EnemyShot.Player is null");
        }
        //calculate direction to move (normalized scales values of vector to be max)
        _direction = (_targetPlayer = transform.position).normalized * _speed;
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()

    {
        transform.Translate(_direction * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }
    }
}
