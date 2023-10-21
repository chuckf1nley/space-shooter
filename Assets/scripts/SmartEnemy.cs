using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyShieldPrefab;
    [SerializeField] private GameObject _smartWeaponPrefab;
    [SerializeField] private GameObject _smartEnemy;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] int _enemyID; // 4 smart enemy
    private SmartWeapon _smartWeapon;
    private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private float _playerVelocity = 2f;
    private float _startx;
    private float _enemyShieldStrength = 1;
    private float _canFire = -1f;
    private float _fireRate = 2f;
    private float _enemyDistance;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = false;
    private SpawnManager _spawnManager;
    private Player _player;
    private int _direction;
    private int _enemyShieldLives = 1;
    private int _enemyLives;

    private Vector3 _smartWeaponOffset = new Vector3(0.1f, 0.08f, 0);


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _enemyDeathAnim = transform.GetComponent<Animator>();
        Player player = GetComponent<Player>();
        SmartWeapon smartWeapon = GetComponent<SmartWeapon>();

        _isEnemyAlive = true;
        _startx = transform.position.x;
        _direction = Random.Range(0, 2);

        int rng = Random.Range( 0, 70);
        GenerateShieldIndex(rng);

        if (_direction == 0)
            _direction = -1;

        if (_player != null)
        {
            Debug.Log("Player is null");
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
                FireWeapon();
                return;
        }

    }

    // Update is called once per frame
    void  Update()
    {
        if (_playerVelocity > 0)
        {
            FireWeapon();
            Movement();
        }
        else
        {
            Movement();
        }

        var mc = new SmartEnemy();
        Transform transform1 = mc._player.transform;
        Vector3 direction = (transform1.position - transform.position).normalized * _speed;
        if (direction.magnitude > transform1.position.magnitude + 2)
        {
            GetComponent<Rigidbody2D>().velocity = direction * Time.deltaTime;
            _playerVelocity = Vector3.Distance(transform.position, _player.transform.position);
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
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

    public void FireWeapon()
    {
        //python def SmartWeapon(playerPosition, _playerVelocity, _enemyPosition);
        //_enemyDistance = calculateDistance(_enemyPosition, playerPosition);

        //if (_enemyDistance <= shootingRange: predictedPosition = calculatePredictedPosition(_playerPosition, _playerVelocity, _enemyDistance))
        //{ 
        // aimDirection = normalize(predictedPosition - _playerPosition) * -1 SmartEnemy.FireWeapon(aimDirection);
        //}
        GameObject smartWeapon = Instantiate(_smartWeaponPrefab, transform.position, Quaternion.identity);
        Instantiate(_smartWeaponPrefab, transform.position, 0, 2);


        if (Time.time > _canFire && _isEnemyAlive == true)
        {
            _fireRate = Random.Range(2f, 4f);
            _canFire = Time.time + _fireRate;
            SmartWeapon[] smartWeapons = smartWeapon.GetComponentsInChildren<SmartWeapon>();
            for (int i = 0; i < smartWeapons.Length; i++)
            {
                smartWeapons[i].Weapon();
            }
        }
    }

    private void Instantiate(GameObject smartWeaponPrefab, Vector3 position, int v1, int v2)
    {
        
    }

    public void EnemyShield()
    {
        EnemyShieldStrength();
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
        return _enemyLives;
    }
    public int EnemyShieldStrength()
    {

        return _enemyShieldLives;
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
