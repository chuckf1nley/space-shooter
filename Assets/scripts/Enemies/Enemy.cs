using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _fastSpeed = 4.5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private GameObject _fastEnemy;
    [SerializeField] private int _enemyID; //0 normal enemy, 1 Fast Enemy
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _enemyShieldStrength = 1;
    [SerializeField] private SpriteRenderer _shieldSpriteRenderer;
    public float _playerProx = 2f;
    private float _fireRate = 3f;
    private float _canfire = -1f;
    private float _canMissileFire = -1.5f;
    private float _startX;
    private float _positionX;
    private Player _player;
    private Animator _enemyDeathAnim;
    private bool _isEnemyShieldActive = false;
    private bool _isFastEnemy = true;
    private bool _isEnemy = true;
    private bool _canFireMissile;
    private bool _isEnemyAlive = true;
    private int _enemyLives = 1;
    private int _enemyShieldLives = 1;
    private int _direction;
    private int _enemy;
    private SpawnManager _spawnManager;

    private Vector3 _laserOffset = new Vector3(-.1f, -.4f, 0);
    private Vector3 _missileOffset = new Vector3(0f, 0f, 0);

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _enemyDeathAnim = transform.GetComponent<Animator>();

        _isEnemyAlive = true;
        _isEnemy = true;
        _isFastEnemy = true;
        _startX = transform.position.x;
        _positionX = transform.position.x;
        _direction = Random.Range(0, 2);

        if (_direction == 0)
            _direction = -1;

        if (_player == null)
        {
            Debug.Log("player is null - enemy");
        }

        if (_enemyDeathAnim == null)
        {
            Debug.Log("animator is null!");
        }
        if (_audioSource == null)
        {
            Debug.Log("AudioSource on Enemy is null!");
        }
        else
        {
            _audioSource.clip = _audioClip;
        }

        int rng = Random.Range(0, 60);
        GenerateShieldIndex(rng);
        
    }


    //BUGS - no shields, fast enemy screen glitch, movement

    // Update is called once per frame
    void Update()
    {
        switch (_enemyID)
        {
            default:
                EnemyRight();
                break;
            case 1:
                FastEnemy();
                return;
        }

    }

    public void FireLaser()
    {

        if (Time.time > _canfire && _isEnemy == true && _isEnemyAlive == true)
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
    
    public void FireMissile()
    {
        FireMissileCoroutine();

        if (Time.time > _canMissileFire && _isFastEnemy == true && _isEnemyAlive == true)
        {
            _fireRate = Random.Range(2f, 5f);
            _canMissileFire = Time.time + _fireRate;
            GameObject fireMissile = Instantiate(_missilePrefab, transform.position, Quaternion.identity);
            FastEnemyMissile[] fastenemymissile = fireMissile.GetComponentsInChildren<FastEnemyMissile>();
            for (int i = 0; i < fastenemymissile.Length; i++)
            {
                fastenemymissile[i].AssignEnemyMissile();
            }
        }
    }
    IEnumerator FireMissileCoroutine()
    {
        while (_canFireMissile == true)
        {
            Instantiate(_missilePrefab, transform.position + _missileOffset, Quaternion.identity);
            Vector3 _missilePos = transform.TransformPoint(_missileOffset);
            GameObject _missile = Instantiate(_missilePrefab, _missilePos, this.transform.rotation);
            _missile.tag = "EnemyMissile";
            yield return new WaitForSeconds(Random.Range(2f, 5f));

        }

    }

    public void CalculateMovement()
    {
        if (transform.position.y < -7.5f)
        {
            float randomx = Random.Range(-18f, 18f);
            transform.position = new Vector3(randomx, 9f, 0);
        }
    }

    public void EnemyRight()
    {
        _isEnemy = true;
        RegularEnemyMovement();
        CalculateMovement();
        FireLaser();
        
    }
    public void RegularEnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.x > _startX + 4)
            _direction = -1;
        else if (transform.position.x < _startX - 4)
            _direction = 1;

        transform.Translate(Vector3.right * _direction * _speed * Time.deltaTime);
    }

    public void FastEnemy()
    {
        _isFastEnemy = true;
        FastEnemyMovement();
        FireMissile();
        CalculateMovement();      
    }
    public void FastEnemyMovement()
    {
        transform.Translate(Vector3.down * _fastSpeed * Time.deltaTime);

        if (transform.position.x > _startX + 8)
            _direction = -1;
        else if (transform.position.x < _startX - 8)
            _direction = 1;

        transform.Translate(Vector3.left * _direction * _fastSpeed * Time.deltaTime);

        //edge detection / placement are same position
        if (transform.position.x > _positionX)
        {
            transform.position = new Vector3(-17.5f, transform.position.y, 0);
        }

        if (transform.position.x < -_positionX)
        {
            transform.position = new Vector3(17.5f, transform.position.y, 0);
        }

    }    

    public void ShieldActive(bool state)
    {
        _shieldSpriteRenderer.gameObject.SetActive(state);
        _isEnemyShieldActive = state;
        if (state == true)
        {
            _enemyShieldLives = 1;
        }
    }
    public void GenerateShieldIndex(int random)
    {
        if (_enemyID == 0)
        {
            if (random >= 20 && random < 30)
                ShieldActive(true);
        }
        else if (_enemyID == 1)
        {
            if (random >= 30 && random < 40)
                ShieldActive(true);
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
        _fastSpeed = 0;
        _audioSource.Play();
        _isFastEnemy = false;
        _isEnemy = false;
        _isEnemyAlive = false;
        _spawnManager.EnemyDeath();
        Destroy(this.gameObject, 1.6f);
        Destroy(GetComponent<Collider2D>());
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if ( _isFastEnemy == true || _isEnemy == true)
        {           
            if (other.CompareTag("Player"))
            {
                Player player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
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
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                Damage();
            }
        }
    }
}