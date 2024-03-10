using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyShieldPrefab;
    [SerializeField] private GameObject _smartWeaponPrefab;
    [SerializeField] private GameObject _smartEnemy;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private SpriteRenderer _shieldSpriteRenderer;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private int _enemyID; // 3 smart enemy
    [SerializeField] private float _fireRate = 2f;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotationModifier;
    [SerializeField] private float _distanceFrom;
    [SerializeField] private float rangeX = 10f;
    private Quaternion _startRotaion;
    private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private float _canFire = -1f;
    private float _enemyShieldStrength;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = false;
    private bool _isBehindPlayer = false;
    private bool _isPlayerAlive = true;
    private SpawnManager _spawnManager;
    private Player _player;
    private int _enemyLives;
    private int _direction;
    private int _enemyShieldLives = 1;
    private Transform _playerPos;
    

    // Start is called before the first frame update
    void Start()
    {
         _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _enemyDeathAnim = transform.GetComponent<Animator>();
        _startRotaion = transform.rotation;
        SmartWeapon smartWeapon = GetComponent<SmartWeapon>();

        _isEnemyAlive = true;
        _isBehindPlayer = false;
        _direction = Random.Range(0, 2);

        if (_direction == 0)
            _direction = -1;

        if (_player == null)
        {
            Debug.Log("Player is null = smart enemy");
        }

        if (_enemyDeathAnim == null)
        {
            Debug.Log("Animator is null - smart enemy");
        }
        if (_audioSource == null)
        {
            Debug.Log("AudioSource is null - smart enemy");
        }
        else
        {
            _audioSource.clip = _audioClip;
        }

        EnemyShield();
        StartCoroutine(PlayerInRange());

        int rng = Random.Range( 0, 70);
        GenerateShieldIndex(rng);
    }


    //BUGS - shield spawns on enemy but stays there, inactive


    // Update is called once per frame
    void  Update()
    {
        CalculateMovement();
        FireWeapon();
    }     

    public void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
       
        if (transform.position.y < -7.5)
        {
            float random = Random.Range(-18f, 18f);
            transform.position = new Vector3(random, 9f, 0);
        }

    }

    public void FireWeapon()
    {
        if (_isEnemyAlive == true)
        {
           // Debug.Log("Smart enemy is alive");

            if (Time.time > _canFire && _isEnemyAlive == true && _isBehindPlayer == true)
            {
                // Debug.Log("Smart Enemy firing");
                _fireRate = Random.Range(2f, 5f);
                _canFire = Time.time + _fireRate;
                GameObject smartWeapons = Instantiate(_smartWeaponPrefab, transform.position, Quaternion.identity);
                SmartWeapon[] smartWeapon = smartWeapons.GetComponentsInChildren<SmartWeapon>();
                for (int i = 0; i < smartWeapon.Length; i++)
                {
                    smartWeapon[i].Weapon();
                }
                transform.rotation = _startRotaion;
                _speed = 3f;
                transform.Translate(Vector3.up * _speed * Time.deltaTime);

                if (transform.position.y <= -7f)
                {
                    Destroy(this.gameObject);
                }
            }

        }
    }


    private IEnumerator PlayerInRange()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            float distanceX = Mathf.Abs(_playerPos.position.x - transform.position.x);
            if (_player != null)
            {
                if (distanceX <= rangeX && transform.position.y < _playerPos.position.y)
                {
                    _isBehindPlayer = true;
                    _isPlayerAlive = true;
                }

                else
                {
                    if (_isBehindPlayer)
                        _isBehindPlayer = false;
                    _isPlayerAlive = false;
                }
            }

            yield return wait;
        }

    }

    public int EnemyShield()
    {
        _isEnemyShieldActive = true;
        GameObject.Instantiate(_enemyShieldPrefab, transform.position, Quaternion.identity);
        _enemyShieldStrength = 1;
        return _enemyLives;
    }
    public void GenerateShieldIndex(int random)
    {
        if (_enemyID == 3)
        {
            if (random >= 60 && random < 70)
                ShieldActive(true);
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
                    player.Damage();
                    _player.AddScore(10);                }
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
