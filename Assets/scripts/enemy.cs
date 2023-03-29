using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private GameObject _laserPrefab;
    // private Vector3 _offset = new Vector3(0, 0.5f, 0);
    private float _fireRate = 3f;
    private float _canfire = -1;
    private Player _player;
    private Animator _Anim;
    private AudioSource _audioSource;
    private GameObject Shield;
    private GameObject Explode;

    //after 3 minutes increase enemy spawns/ create a second enemy so 2 spawn
    // after 180 seconds decrease spawn timer from 5 seconds to 3 seconds
    // after 300 seconds decrease spawn timer to 2 second
    // after 600 seconds decrease spawn timer to 1 enemy a second


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("player is null");
        }

        _Anim = GetComponent<Animator>();

        if (_Anim == null)
        {
            Debug.LogError("animator is null!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canfire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canfire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.5f)
        {
            float randomx = Random.Range(-18f, 18f);
            transform.position = new Vector3(randomx, 10f, 0);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _Anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.6f);

        }

        if (other.tag == "laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _Anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Laser>());
            Destroy(this.gameObject, 2.6f);

        }

        if (other.CompareTag ("Shield"))
            if (Shield != null)
        {
            other.GetComponent<Shield>().Damage();
            _player.AddScore(10);
            //this.Explode;
        }
    }

}
