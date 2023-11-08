using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidShot : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private GameObject _enemyShieldPrefab;
    [SerializeField] private int _enemyID; // 5 avoid shot
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private float _enemyShieldStrength = 1;
    private float _laserDistance = 2.5f;
    private float _startX;
    private Player _player;
    private Laser _laser;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = false;
    private SpawnManager _spawnManager;
    private int _enemyLives;
    private int _enemyShieldLives = 1;
    private int _direction;


    // Start is called before the first frame update
    void Start()
    {
        //if (_direction == 0)
        //    _direction = -1;
        _laser = GameObject.Find("Laser").GetComponent<Laser>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _enemyDeathAnim = transform.GetComponent<Animator>();

        _isEnemyAlive = true;
        _direction = Random.Range(0, 2);

        if (_player == null)
        {
            Debug.Log("player is null");
        }

        if (_enemyDeathAnim == null)
        {
            Debug.Log("animator is null");
        }

        if (_audioSource == null)
        {
            Debug.Log("audiosource on avoid enemy is null");
        }
        else
        {
            _audioSource.clip = _audioClip;
        }

        int rng = Random.Range(0, 60);
        GenerateShieldIndex(rng);
    }

    // Update is called once per frame
    void Update()
    {



        _laserDistance = Vector3.Distance(transform.position, _laser.transform.position);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (_laserDistance > 2.5f)
        {
            Movement();
        }
        else
        {
            AvoidLaser();
        }

    }


    public void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.x > _startX + 4)
            _direction = 1;
        else if (transform.position.x < _startX - 4)        
            _direction = -1;
        
        if (transform.position.y < -7.5)
        {
            float randomx = Random.Range(-18f, 18f);
            transform.position = new Vector3(randomx, 9f, 0);
        }

    }
    public void AvoidLaser()
    {
        if (_laserDistance < 2.5)
            _speed += 2;
    
        Vector3 direction = _laser.transform.position + transform.position;
        direction = direction.normalized;

        transform.Translate(direction * _speed * Time.deltaTime * 2);
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
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                Damage();
            }
            if (other.CompareTag("Shield"))
            {
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                Damage();
            }
            if (other.CompareTag("PlayerMIssile"))
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
