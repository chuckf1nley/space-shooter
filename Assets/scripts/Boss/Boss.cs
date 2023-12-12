using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _enemyID;
    [SerializeField] private GameObject _bossWeaponPrefab;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _bossSpawn;
    [SerializeField] private AudioClip _audioDeathClip;
    [SerializeField] private int _currentBossHealth;
    [SerializeField] private int _maxBossHealth = 40;
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private BoxCollider2D[] _cols;
    private UIManager _ui;
    private bool bossDefeatedCoroutineStarted = false;

    public BossHealthBar _healthBar;

    private float _positionX;
    private float _fireRate = 2f;
    private float _startX;
    private float _direction;
    private float _canfire = 1f;
    private bool _isBossAlive = true;
    private Player _player;
    private SpawnManager _spawnManager;
    private Vector3 _endPos = new Vector3(0, 5, 0);

    private void Awake()
    {
        _healthBar = Instantiate(_healthBarPrefab).GetComponent<BossHealthBar>();
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _currentBossHealth = _maxBossHealth;
        _healthBar.SetMaxHealth(_maxBossHealth);
        _healthBar.SetHealth(_currentBossHealth);
        _cols = GetComponents<BoxCollider2D>();
        _ui = Object.FindObjectOfType<UIManager>();

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();       

        _startX = transform.position.x;
        _positionX = transform.position.x;
        _direction = Random.Range(0, 3);

        if (_direction == 0)
            _direction = -1;

        if (_player == null)
        {
            Debug.Log("Player is null - boss");
        }
        if (_enemyDeathAnim == null)
        {
            Debug.Log("Animator is null - boss");
        }

        if (_audioSource == null)
        {
            Debug.Log("AudioSource is null - boss");
        }
        if (_isBossAlive == true)
        {
            _audioSource.clip = _bossSpawn;
        }
        else
        {
            _audioSource.clip = _audioDeathClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentBossHealth >= 20)
        {
            BossMovementBelow50();
        }
        if (_currentBossHealth <= 0 && bossDefeatedCoroutineStarted)
        {
            EnemyDeathSequence();
            bossDefeatedCoroutineStarted = true;
        }
        switch (_enemyID)
        {
            default:
                BossMovement();
                return;
        }
    }

    public void BossMovement()
    {
        //move to -4 and stay there, move left to right

        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _endPos.y)
            transform.position = _endPos;

        BossLaser();
    }

    public void BossMovementBelow50()
    {
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);
        }

        if (transform.position.x > _startX + 3)
        {
            _direction = -1;
        }
        else if (transform.position.x < _startX - 4)
        {
            _direction = 1;
        }
        transform.Translate(Vector3.right * _direction * _speed * Time.deltaTime);

        BossFlameThrower();

    }

    public void BossFlameThrower()
    {
        if (Time.time > _canfire && _isBossAlive == true)
        {
            _fireRate = Random.Range(3f, 5f);
            _canfire = Time.time + _fireRate;
            GameObject bossWeapon = Instantiate(_bossWeaponPrefab, transform.position, Quaternion.identity);
            BossWeapon[] bossWeapons = bossWeapon.GetComponentsInChildren<BossWeapon>();
            for (int i = 0; i < bossWeapons.Length; i++)
            {
                bossWeapons[i].AssignBossWeaponA();
            }
        }
    }


    public void BossLaser()
    {

        if (Time.time > _canfire && _isBossAlive == true)
        {
            _fireRate = Random.Range(2f, 7f);
            _canfire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }


    }
    public void Damage(int damage)
    {
        _currentBossHealth -= damage;
        _healthBar.SetHealth(_currentBossHealth);
    }


    public void EnemyDeathSequence()
    {
        if (_enemyDeathAnim == null)
            _enemyDeathAnim.SetTrigger("OnEnemyDeath");
        if (_audioSource == null)
            _audioSource.Play();
        //foreach (BoxCollider2D - char -in -cols)
        //    c.enabled = false;
        _ui.StartCoroutine(_ui.GameWonSequence());
        Destroy(this.gameObject, -3);
        Destroy(GetComponent<Collider2D>());

    }

    private void OnDestroy()
    {
        if (_healthBar == null)
            GameObject.Destroy(_healthBar.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isBossAlive == true)

            if (other.CompareTag("Player"))
            {
                Player player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                }
            }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Shield"))
        {
            other.GetComponent<Shield>();
        }
        if (other.CompareTag("PlayerMissile"))
        {
            Destroy(other.gameObject);
        }

    }

}
