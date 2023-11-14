using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidShot : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private GameObject _enemyShieldPrefab;
    [SerializeField] private int _enemyID; // 5 avoid shot
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private float _enemyShieldStrength = 1;
    private float _avoidDistance = 2.5f;
    private float _avoidSpeed = 4f;
    private float _startX;
    private float _distanceX;
    private float _rangeX;
    private Player _player;
    private Laser _laser;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = false;
    private bool _isLaserClose = false;
    private SpawnManager _spawnManager;
    private int _enemyLives;
    private int _enemyShieldLives = 1;
    private int _movement;
    private Vector3 _direction;
    private Transform laser;

    // Start is called before the first frame update
    void Start()
    {
        // _player = GameObject.Find("Player").GetComponent<Player>();
        // player = GameObject.FindGameObjectWithTag("Player").transform;
        laser = GameObject.FindGameObjectWithTag("Laser").transform;
        _laser = GameObject.Find( "Laser").GetComponent<Laser>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _enemyDeathAnim = transform.GetComponent<Animator>();
        _startX = transform.position.x;
        _speed = 5;
        _enemyLives = 1;

        _isEnemyAlive = true;
        _movement = Random.Range(0, 3);

        if (_player == null)
        {
            Debug.Log("player is null - avoidShot");
        }

        if (_enemyDeathAnim == null)
        {
            Debug.Log("animator is null- avoidShot");
        }

        if (_laser == null)
        {
            Debug.Log("Laser is null - avoidShot");
        }

        if (_audioSource == null)
        {
            Debug.Log("audiosource on avoidShot is null");
        }
        else
        {
            _audioSource.clip = _audioClip;
        }

        int rng = Random.Range(0, 70);
        GenerateShieldIndex(rng);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        Debug.Log("Laser not called avoidShot");
        LaserInRange();
        //_avoidDistance = Vector3.Distance(transform.position, _laser.transform.position);
        //if (_avoidDistance > 2.5f)
        //{
        //    Movement();
        //}
        //else
        //{
        //    AvoidLaser();
        //}

        if (transform.position.y < -7.5)
        {
            float randomx = Random.Range(-18f, 18f);
            transform.position = new Vector3(randomx, 9f, 0);
        }
    }


    public void Movement()
    {

        if (transform.position.x > _startX + 4)
            _movement = -1;
        else if (transform.position.x < _startX - 4)        
            _movement = 1;
        

    }
    public void AvoidLaser()
    {
        _direction = _laser.transform.position + transform.position;
         _direction.Normalize();
        transform.Translate(_direction * _avoidSpeed * Time.deltaTime);

    //    if (_avoidDistance < 2.5)
    //        _avoidSpeed += 2;

    //     _avoidDistance = Vector3.Distance(transform.position, _laser.transform.position);
    //    Vector3 direction = _laser.transform.position + transform.position;
    //    if (direction == null)
    //    {
    //        Debug.Log("laser not found, avoidshot");
    //    }



    //    transform.Translate(direction *  _avoidSpeed * Time.deltaTime * 2);
    }

    private IEnumerator LaserInRange()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            float distancex = Mathf.Abs(laser.position.x + transform.position.x);

            if (_distanceX <= _rangeX && transform.position.y < laser.position.y)
            {
                _isLaserClose = true;
            }
            else
            {
                if (_isLaserClose)
                    _isLaserClose = false;
            }
            yield return wait;
        }
    }


    public void EnemyShield()
    {
        _isEnemyShieldActive = true;
        ShieldActive(true);
        ShieldStrength();
        EnemyShieldStrength();


    }
    public int ShieldStrength()
    {
        EnemyShield();
        GameObject.Instantiate(_enemyShieldPrefab, transform.position, Quaternion.identity);
        _enemyShieldStrength = 1;
        return _enemyLives = 1;

    }
    public void ShieldActive(bool state)
    {
        _spriteRenderer.gameObject.SetActive(state);
        _isEnemyShieldActive = state;
        if (state == true)
        {
            _enemyShieldLives = 1;
        }
    }
    public void GenerateShieldIndex(int random)
    {
        if (_enemyID == 5 )
        {
            if (random >= 60 && random <= 70)
            ShieldActive(true);
        }
    }
    public int EnemyShieldStrength()
    {
        return _enemyShieldLives; 
    }

    public void Damage()
    {
        if (_isEnemyShieldActive == true)
        {
            _enemyShieldLives--;
            ShieldActive(false);
            return;
        }
        EnemyDeathSequence();
    }
    public void EnemyDeathSequence()
    {
        if (_enemyDeathAnim != null)
        {
            _enemyDeathAnim.SetTrigger("OnEnemyDeath");
        }
        _speed = 0;
        _avoidSpeed = 0;
        _audioSource.Play();
        _isEnemyAlive = false;
        _spawnManager.EnemyDeath();
        Destroy(this.gameObject, .6f);
        Destroy(GetComponent<Collider2D>());
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyAlive == true)
        {
            if (other.CompareTag("Player"))
            {
                Player player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    _player.Damage();
                }
                Damage();
            }
            if (other.CompareTag("Laser"))
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                Damage();
            }
            if (other.CompareTag("Shield"))
            {
                other.GetComponent<Shield>().Damage();
                Damage();
            }
            if (other.CompareTag("PlayerMissile"))
            {
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                Damage();
            }
        }
    }
}
