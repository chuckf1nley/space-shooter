using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyShieldPrefab;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _chaseSpeed = 5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private int _enemyID; // 3 aggro enemy
    private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private float _startX;
    private float _interceptDistance = 4f;
    private float _enemyShieldStrength = 1;
    private Player _player;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = false;
    private SpawnManager _spawnManager;
    private int _direction;
    private int _enemyShieldLives = 1;
    private int _enemyLives;


    //enemy needs to move towards and kamikaze the player
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _enemyDeathAnim = transform.GetComponent<Animator>();

        _isEnemyAlive = true;
        _startX = transform.position.x;
        _direction = Random.Range(0, 2);

        if (_direction == 0)
            _direction = -1;

        if (_player == null)
        {
            Debug.Log("Player is null");
        }

        if (_enemyDeathAnim == null)
        {
            Debug.Log("Animator is null");
        }
        if (_audioSource == null)
        {
            Debug.Log("AudioSource on aggro enemy is null");
        }       
        else
        {
            _audioSource.clip = _audioClip;
        }

        switch (_enemyID)
        {
            default:
                AggroMovement();
                break;
        }

        int rng = Random.Range(0, 60);
        GenerateShieldIndex(rng);
    }
    //in update make method for determining which code to use, proximity to player, if statement
    // Update is called once per frame
    void Update()
    {
        _interceptDistance = Vector3.Distance(transform.position,  _player.transform.position);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (_interceptDistance > 4)
        {
          NormalMovement();
        }
        else
        {
          AggroMovement();
        }
    }

    public void NormalMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.x > _startX + 4)
            _direction = -1;
        else if (transform.position.x < _startX - 4)
            _direction = 1;

        if (transform.position.y < -7.5)
        {
            float randomx = Random.Range(-18f, 18f);
            transform.position = new Vector3(randomx, 9f, 0);

        }
    }

    public void AggroMovement()
    {

        //_interceptDistance = Vector3.Distance(transform.position, _player.transform.position);

        if (_interceptDistance < 5)
            _chaseSpeed += 1;

        Vector3 direction = _player.transform.position - transform.position;
        direction = direction.normalized;

       transform.Translate(direction * _chaseSpeed * Time.deltaTime);


    }

    public int ShieldStrength()
    {
        EnemyShield();
        GameObject.Instantiate(_enemyShieldPrefab, transform.position, Quaternion.identity);
        _enemyShieldStrength = 1;
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
    public void GenerateShieldIndex(int random)
    {

        if (_enemyID == 2)
        {
            if (random >= 50 && random < 60)
                ShieldActive(true);
        }
    }
    public int EnemyShieldStrength()
    {
        return _enemyShieldLives;
    }
    public void EnemyShield()
    {
        _isEnemyShieldActive = true;
        ShieldActive(true);
        ShieldStrength();
        EnemyShieldStrength();
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
       if (_enemyDeathAnim!=null)
        {
            _enemyDeathAnim.SetTrigger("OnEnemyDeath");
        }
        _speed = 0;
        _chaseSpeed = 0;
        _audioSource.Play();
        _isEnemyAlive = false;
        _spawnManager.EnemyDeath();
        Destroy(this.gameObject, 1.6f);
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
   
}
