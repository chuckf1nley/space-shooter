using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _speed = 4f;
    private float _fastSpeed = 5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private int _enemyID; //0 normal enemy, 1 Fast Enemy
    [SerializeField] private AudioClip _audioClip;    
    private float _fireRate = 3f;
    private float _canfire = -1f;
    private float _canMissileFire = -1.5f;
    private Player _player;
    private Animator _enemyDeathAnim;
    private GameObject _shield;    
    private bool _isFastEnemy = true;
    private bool _isEnemyRight = true;
    private bool _canFire;
    private bool _canFireMissile; 
    private int _direction;
    private float _startX;
    private SpawnManager _spawnManager;

    public Vector3 _laserOffset = new Vector3(.006f, -.04f, 0);
    public Vector3 _missileOffset = new Vector3(0f, 0f, 0);


    //after 3 minutes increase enemy spawns/ create a second enemy so 2 spawn
    // after 120 seconds decrease spawn timer from 5 seconds to 3 seconds

    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _enemyDeathAnim = GetComponent<Animator>();
       
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

    }

    // Update is called once per frame
    //use laser offset to decide which enemy is firinig laser, change accordingly
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
    private void FireLaser()
    {
        FireLaserCoroutine();

        if (Time.time > _canfire && _isEnemyRight == true)
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


    private void FireMissile()
    {
        FireMissileCoroutine();

        if (Time.time > _canMissileFire  && _isFastEnemy == true)
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
    void CalculateMovement()
    { 
        if(transform.position.y < -7.5f)
        {
            float randomx = Random.Range(-18f, 18f);
            transform.position = new Vector3(randomx, 9f, 0);
        }
    }

    private void RegularEnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.x > _startX + 4)
            _direction = -1;
        else if (transform.position.x < _startX - 4)
            _direction = 1;

        transform.Translate(Vector3.right * _direction * _speed * Time.deltaTime);
    }

    private void FastEnemyMovement()
    {
        transform.Translate(Vector3.down * _fastSpeed * Time.deltaTime);

        if (transform.position.x > _startX + 8)
            _direction = -1;
        else if (transform.position.x < _startX - 8)
            _direction = 1;

        transform.Translate(Vector3.left * _direction * _fastSpeed * Time.deltaTime);
        
    }
    public void EnemyRight()
    {
        _isEnemyRight = true;
        RegularEnemyMovement();
        CalculateMovement();
        FireLaser();  
    }

    public void FastEnemy()
    {          
        _isFastEnemy = true;
        FastEnemyMovement();
        FireMissile();
        CalculateMovement();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyRight == true && _isFastEnemy == true)
        {

            if (other.CompareTag("Player"))

            {
                Player player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                }
                if (_enemyID == 0 && _enemyID == 1)
                    _enemyDeathAnim.SetTrigger("OnEnemyDeath");

                _speed = 0;
                _fastSpeed = 0;
                _audioSource.Play();
                _isEnemyRight = false;
                _isFastEnemy = false;
                _spawnManager.EnemyDeath();
                Destroy(this.gameObject, 2.6f);
                Destroy(GetComponent<EnemyLaser>());
                Destroy(GetComponent<Collider2D>());

            }
            if (other.CompareTag("Laser"))
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                if (_enemyDeathAnim != null)
                {
                    _enemyDeathAnim.SetTrigger("OnEnemyDeath");
                }
                _speed = 0;
                _fastSpeed = 0;
                _audioSource.Play();
                _isEnemyRight = false;
                _isFastEnemy = false;
                _spawnManager.EnemyDeath();
                Destroy(GetComponent<Collider2D>());
                Destroy(GetComponent<EnemyLaser>());
                Destroy(this.gameObject, 2.6f);


            }
            if (other.CompareTag("Shield"))

            {
                other.GetComponent<Shield>().Damage();
                _player.AddScore(10);
                _audioSource.Play();
                _isEnemyRight = false;
                _isFastEnemy = false;
                Destroy(GetComponent<EnemyLaser>());
                Destroy(GetComponent<Collider2D>());

            }
        }

    }

    IEnumerator FireLaserCoroutine()
    {
        while (_canFire == true)
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
            Vector3 _laserPos = transform.TransformPoint(_laserOffset);
            GameObject _laser = Instantiate(_laserPrefab, _laserPos, this.transform.rotation);
            _laser.tag = "Enemy Laser";
            _audioSource.Play();
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));

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

    
}
