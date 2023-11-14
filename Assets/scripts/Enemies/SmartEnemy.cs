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
    [SerializeField] private int _enemyID; // 3 smart enemy
    [SerializeField] private float _fireRate = 2f;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotationModifier;
    [SerializeField] private float _distanceFrom;
    [SerializeField] private float rangeX = 10f;
    Quaternion _startRotaion;
    private SmartWeapon _smartWeapon;
    private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private float _playerY;
    private float _enemyShieldStrength = 1;
    private float _canFire = -1f;
    private float _playerDistance = -1f;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = false;
    private bool _canEnemyFire = true;
    private bool _isBehindPlayer = false;
    private SpawnManager _spawnManager;
    private Player _player;
    private int _direction;
    private int _enemyShieldLives = 1;
    private int _enemyLives;
    private Transform player;

    private Vector3 _smartWeaponOffset = new Vector3(0.1f, 0.08f, 0);


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _enemyDeathAnim = transform.GetComponent<Animator>();
       // SmartWeapon smartWeapon = GetComponent<SmartWeapon>();
        _startRotaion = transform.rotation;

        _isEnemyAlive = true;
        _isBehindPlayer = false;
        _direction = Random.Range(0, 2);

        int rng = Random.Range( 0, 70);
        GenerateShieldIndex(rng);

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
            Debug.Log("AudioSource is null");
        }
        else
        {
            _audioSource.clip = _audioClip;
        }

        StartCoroutine(PlayerInRange());

    }

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

            if (Time.time > _canFire && _isEnemyAlive && _isBehindPlayer)
            {
               // Debug.Log("Smart Enemy firing");
                _canFire = Time.time + _fireRate;
                GameObject smartWeapon = Instantiate(_smartWeaponPrefab, transform.position, Quaternion.identity);
                SmartWeapon[] smartWeapons = smartWeapon.GetComponentsInChildren<SmartWeapon>();
                for (int i = 0; i < smartWeapons.Length; i++)
                {
                    smartWeapons[i].Weapon();
                }
                transform.rotation = _startRotaion;
                _speed = 3f;
                transform.Translate(Vector3.down * _speed * Time.deltaTime);

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
        while(true)
        {
            float distanceX = Mathf.Abs(player.position.x - transform.position.x);
                       

            if(distanceX <= rangeX && transform.position.y < player.position.y)
            {
                _isBehindPlayer = true;
            }
            else 
            {
                if (_isBehindPlayer)
                    _isBehindPlayer = false;
            } yield return wait;
        }

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
        if (_enemyID == 3)
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
