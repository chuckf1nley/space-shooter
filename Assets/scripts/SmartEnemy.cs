using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyShieldPrefab;
    [SerializeField] private GameObject _smartEnemy;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] int _enemyID; // 4 smart enemy
    private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private float _playerDistance = 2f;
    private float _startx;
    private float _enemyShieldStrength = 1;
    private Player _player;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = false;
    private SpawnManager _spawnManager;
    private int _direction;
    private int _enemyShieldLives = 1;
    private int _enemyLives;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _enemyDeathAnim = transform.GetComponent<Animator>();

        _isEnemyAlive = true;
        _startx = transform.position.x;
        _direction = Random.Range(0, 2);

        if (_direction == 0)
            _direction = -1;

        if (_player != null)
        {
            Debug.Log("Playeer is null");
        }

        if (_enemyDeathAnim != null)
        {
            Debug.Log("Animator is null");
        }
        if (_audioSource != null)
        {
            Debug.Log("AudioSource is null");
        }
        else
        {
            _audioSource.clip = _audioClip;
        }

        switch (_enemyID)
        {

            default:
                Weapon();
                return;
        }
        int rng = Random.Range( 0, 70);
        GenerateShieldIndex(rng);
    }

    // Update is called once per frame
    void Update()
    {
        _playerDistance = Vector3.Distance(transform.position, _player.transform.position);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (_playerDistance > 0)
        {
            Weapon();
            Movement();
        }
        else
        {
            Movement();
        }
    }

    public void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.x > _startx + 2)
            _direction = -1;
        else if (transform.position.x < _startx - 2)
        {
            _direction = 1;
        }

        if (transform.position.y < -7.5)
        {
            float random = Random.Range(-18f, 18f);
            transform.position = new Vector3(random, 9f, 0);
        }
    }

    public void Weapon()
    {

    }
    public void EnemyShield()
    {
        _isEnemyShieldActive = true;
        ShieldActive(true);
        ShieldStrength();
    }
    public void GenerateShieldIndex(int random)
    {
        if (_enemyID == 4)
        {
            if (random >= 60 && random < 70)
                ShieldActive(true);
        }
    }
    public int ShieldStrength()
    {
        EnemyShield();
        GameObject.Instantiate(_enemyShieldPrefab, transform.position, Quaternion.identity);
        _enemyShieldStrength = 1;
        return _enemyShieldLives;
        return _enemyLives;
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
        Destroy(this.gameObject, 1f);
        Destroy(GetComponent<Collider2D>());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyAlive == true)
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
                _player.Damage();
            }
            Damage();
        }
        if (other.CompareTag("Shield"))
        {
            if (_player != null)
            {
                _player.Damage();
            }
            Damage();
        }
        if (other.CompareTag("Missile"))
        {
            if (_player != null)
            {
                _player.Damage();
            }
            Damage();
        }
    }
}
