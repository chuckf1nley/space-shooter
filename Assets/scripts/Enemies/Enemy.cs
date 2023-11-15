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
    [SerializeField] private GameObject _enemyShieldPrefab;
    [SerializeField] private GameObject _enemyRight;
    [SerializeField] private int _enemyID; //0 normal enemy, 1 Fast Enemy
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _enemyShieldStrength = 1;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public float _playerProx = 2f;
    private Missile _missile;
    private float _fireRate = 3f;
    private float _canfire = -1f;
    private float _canMissileFire = -1.5f;
    private float _startX;
    private Player _player;
    private Animator _enemyDeathAnim;
    private bool _isEnemyShieldActive = false;
    private bool _isFastEnemy = true;
    private bool _isEnemyRight = true;
    private bool _canFire;
    private bool _canFireMissile;
    private bool _isEnemyAlive = true;
    private int _enemyLives = 1;
    private int _enemyShieldLives = 1;
    private int _direction;
    private SpawnManager _spawnManager;
    private Transform player;

    private Vector3 _laserOffset = new Vector3(-.1f, -.4f, 0);
    private Vector3 _missileOffset = new Vector3(0f, 0f, 0);




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
      //  _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _enemyDeathAnim = transform.GetComponent<Animator>();


        _isEnemyRight = true;
        _isFastEnemy = true;
        _startX = transform.position.x;
        _direction = Random.Range(0, 2);

        if (_direction == 0)
            _direction = -1;

        if (_player == null)
        {
            Debug.LogError("player is null");
        }

        if (_enemyDeathAnim == null)
        {
            Debug.Log("animator is null!");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Enemy is null!");
        }
        else
        {
            _audioSource.clip = _audioClip;
        }

        int rng = Random.Range(0, 60);
        GenerateShieldIndex(rng);
    }

    // Update is called once per frame
    //use laser offset to decide which enemy is firinig laser, change accordingly
    //organize code from top to bottom, grouped by segment
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
        LaserOffSet();

        if (Time.time > _canfire && _isEnemyRight == true && _isEnemyAlive == true)
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
    public void LaserOffSet()
    {
        if (_canFire == true)
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
            Vector3 _laserPos = transform.TransformPoint(_laserOffset);
            GameObject _laser = Instantiate(_laserPrefab, _laserPos, this.transform.rotation);
            _laser.tag = "Enemy Laser";
        }

    }

    public void FireMissile()
    {
        FireMissileCoroutine();

        if (Time.time > _canMissileFire && _isFastEnemy == true)
        {
            _fireRate = Random.Range(2f, 5f);
            _canMissileFire = Time.time + _fireRate;
            GameObject fireMissile = Instantiate(_missilePrefab, transform.position, Quaternion.identity);
            Missile[] missile = fireMissile.GetComponentsInChildren<Missile>();
            for (int i = 0; i < missile.Length; i++)
            {
                missile[i].AssignEnemyMissile();
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
            _missile.tag = "Enemy Missile";
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

    public void RegularEnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.x > _startX + 4)
            _direction = -1;
        else if (transform.position.x < _startX - 4)
            _direction = 1;

        transform.Translate(Vector3.right * _direction * _speed * Time.deltaTime);
    }
    public void EnemyRight()
    {
        _isEnemyRight = true;
        RegularEnemyMovement();
        CalculateMovement();
        FireLaser();
        
    }

    public  void FastEnemyMovement()
    {
        transform.Translate(Vector3.down * _fastSpeed * Time.deltaTime);

        if (transform.position.x > _startX + 8)
            _direction = -1;
        else if (transform.position.x < _startX - 8)
            _direction = 1;

        transform.Translate(Vector3.left * _direction * _fastSpeed * Time.deltaTime);
    }

    public void FastEnemy()
    {
        _isFastEnemy = true;
        FastEnemyMovement();
        FireMissile();
        CalculateMovement();
      
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
        if (_enemyDeathAnim != null)
        {
            _enemyDeathAnim.SetTrigger("OnEnemyDeath");
        }
        _speed = 0;
        _fastSpeed = 0;
        _audioSource.Play();
        _isEnemyRight = false;
        _isFastEnemy = false;
        _isEnemyAlive = false;
        _spawnManager.EnemyDeath();
        Destroy(this.gameObject, 1.6f);
        Destroy(GetComponent<Collider2D>());
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyRight == true || _isFastEnemy == true)
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