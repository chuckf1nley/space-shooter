using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _enemyID;
    [SerializeField] private GameObject _bossFlameThrowerPrefab;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private AudioClip _bossSpawn;
    [SerializeField] private AudioClip _audioDeathClip;
    [SerializeField] private int _currentBossHealth;
    [SerializeField] private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private BoxCollider2D[] _cols;
    private UIManager _ui;

    private BossHealthBar _healthBarScript;

    private int _weaponID; //0 - lasers, 1 - flamethrower
    private int _maxBossHealth = 60;
    private int _minBossHealth = 0;
    private float _positionX;
    private float _fireRate = 2f;
    private float _startX;
    private float _direction;
    private float _canfire = 1f;
    private bool _isBossAlive = true;
    private bool _isFlameActive;
    private Player _player;
    private SpawnManager _spawnManager;
    private Vector3 _endPos = new Vector3(0, 3.5f, 0);

    //private void Awake()
    //{
    //    _healthBar = Instantiate(_healthBarPrefab).GetComponent<BossHealthBar>();
    //}

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _currentBossHealth = _maxBossHealth;
        _audioSource = GetComponent<AudioSource>();
        _audioDeathClip = GetComponent<AudioClip>();
        _cols = GetComponents<BoxCollider2D>();
        _ui = GameObject.Find("Canvas").GetComponent<UIManager>();


        //_healthBarSlider = GameObject.Find("BossHealthBar").gameObject;
        //_healthBarSlider.transform.GetChild(0).gameObject.SetActive(true);
        //_healthBarScript = _healthBarSlider.transform.GetChild(0).GetComponent<BossHealthBar>();
        //_healthBarScript.SetMaxHealth(_maxBossHealth);

        _isBossAlive = true;

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

        int rng = Random.Range(0, 20);
        //GenerateWeaponIndex(rng);

        _ui.BossHealthBar(_currentBossHealth);
    }

    //BUGS - boss health doesnt show up, boss doesnt fire
    //create an index for firing weapons, either flame or lasser
    //add boss health bar to script

    // Update is called once per frame
    void Update()
    {
        BossLogic();
    }

    public void BossLogic()
    {
        _healthBarScript.SetHealth(_currentBossHealth);
        BossPhases();

    }

    public void BossPhases()
    {
        if (_currentBossHealth > 30)
        {
            BossMovement();
            BossFireLaser();
        }
        if (_currentBossHealth <= 30)
        {
            BossMovementBelowHalf();
            int rng = Random.Range(0, 50);
            GeneratWeaopnIndex(rng);
        }
        if (_currentBossHealth <= 0)
        {
            _currentBossHealth = _minBossHealth;
            EnemyDeathSequence();
        }

    }

    public void BossMovement()
    {
        //move to 0, 3.5, 0 and move left to right
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _endPos.y)
            transform.position = _endPos;

    }

    public void BossMovementBelowHalf()
    {
        if (transform.position.y <= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y >= 4f)
        {
            transform.position = new Vector3(transform.position.x, 4f, 0);
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

    }

    public void GeneratWeaopnIndex(int random)
    {

        if (_weaponID == 0)
        {
            if (random >= 20 && random < 30)
            {
                BossFlameThrower(true);
            }
        }
        else if (_weaponID == 1)
        {
            if (random >= 30 && random < 40)
            {
                BossFireLaser();
                BossFlameThrower(false);
            }
        }

    }

    public void BossFlameThrower(bool state)
    {
        //turn on instead / set active - enemy shield / powerup
       
        _bossFlameThrowerPrefab.gameObject.SetActive(state);
        _isFlameActive = state;
        if (state == true)
        {
            _isFlameActive = true;
        }
    }


    public void BossFireLaser()
    {
        if (Time.time > _canfire && _isBossAlive == true)
        {
            Debug.Log("BossLaserCalled");
            _fireRate = Random.Range(2f, 5f);
            _canfire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    public void Damage()
    {
        //_healthBar.SetHealth(_currentBossHealth);

        _currentBossHealth--;
        _ui.BossHealth(_currentBossHealth);
        BossPhases();
    }

    public void EnemyDeathSequence()
    {
        Debug.Log("Enemy Death Sequence Started");
        _isBossAlive = false;
        if (_enemyDeathAnim != null)
            _enemyDeathAnim.SetTrigger("OnEnemyDeath");
        if (_audioSource != null)
            _audioSource.Play();

        _ui.GameWon();
        Destroy(this.gameObject, -3f);
        Destroy(GetComponent<Collider2D>());
        Debug.Log("Death Sequence Ended");
    }

    public void OnDestroy()
    {
        if (_healthBarScript != null)
            GameObject.Destroy(_healthBarScript.gameObject);
    }

    //set the laser to damage boss before being destroyed
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
            Damage();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Shield"))
        {
            Damage();
            other.GetComponent<Shield>();
        }
        if (other.CompareTag("PlayerMissile"))
        {
            Damage();
            Destroy(other.gameObject);
        }

    }

}
