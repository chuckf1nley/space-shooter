using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject _weapon;    
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _enemyID;
    [SerializeField] private GameObject _bossWeaponPrefab;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _bossSpawn;
    [SerializeField] private AudioClip _audioDeathClip;
    private float _positionX;
    private float _fireRate = 2f;
    private float _startX;
    private float _direction;
    private float _canfire = 1f;
    private int _bossHealth;
    private bool _isBossAlive = true;
    private Player _player;
    private AudioSource _audioSource;
    private Animator _enemyDeathAnim;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _bossSpawn = GetComponent<AudioClip>();
        _audioDeathClip = GetComponent<AudioClip>();
        _enemyDeathAnim = transform.GetComponent<Animator>();

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
        if(_isBossAlive == true)
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

    }

    public void BossMovementBelow50()
    {

    }

    public void BossWeaponA()
    {
        //if (Time.time > _canfire && _isBossAlive == true)
        //{
        //    _fireRate = Random.Range(3f, 5f);
        //    _canfire = Time.time + _fireRate;
        //    GameObject bossweapon = Instantiate(_bossWeaponPrefab, transform.position, Quaternion.identity);
        //    BossWeapon[] bossWeapons = bossweapon.GetComponentInChildren<BossWeapon>();
        //}for (int i = 0; i < bossWeapons.Length; i++)
        //{
        //    bossWeapons[i].AssignBossWeaponA();
        //}
    }


    public void BossWeaponB()
    {
        /*
        
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
        */

        if (Time.time > _canfire && _isBossAlive == true)
        {
            _fireRate = Random.Range(2f, 7f);
            _canfire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
           // Laser[] lasers = bossLaser.GetComponentInChildren<Laser>();
            //for (int i = 0; i < lasers.Length; i++)
            //{
            //    lasers[i].AssignBossLaser();
            //}
        }


    }
}
