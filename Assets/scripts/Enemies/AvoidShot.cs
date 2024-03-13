using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidShot : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _enemyID; // 5 avoid shot
    [SerializeField] private SpriteRenderer _shieldSpriteRenderer;
    [SerializeField] private AudioClip _deathAudioClip;
    private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private float _avoidSpeed = 4.5f;
    private float _startX;
    private float _distanceX;
    private float _rangeX;
    private float _laserX;
    private Player _player;
    private Laser _laser;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = false;
    private bool _isLaserClose = false;
    private bool _avoidShot = false;
    private SpawnManager _spawnManager;
    private int _enemyLives;
    private int _enemyShieldLives = 1;
    private int _movement = 2;


    // Start is called before the first frame update
    void Start()
    {
        _enemyDeathAnim = transform.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _deathAudioClip = GetComponent<AudioClip>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _startX = transform.position.x;
        _enemyLives = 1;

        _isEnemyAlive = true;
        _movement = Random.Range(1, 3);

        if (_player == null)
        {
            Debug.Log("player is null - avoidShot");
        }

        if (_enemyDeathAnim == null)
        {
            Debug.Log("animator is null - avoidShot");
        }

        //if (_laser == null)
        //{
        //    Debug.Log("Laser is null - avoidShot");
        //}

        if (_audioSource == null)
        {
            Debug.Log("audiosource on avoidShot is null");
        }
        else
        {
            _audioSource.clip = _deathAudioClip;
        }

        int rng = Random.Range(0, 70);
        GenerateShieldIndex(rng);
    }

    // BUGS - movement, shields - sometimes enemy will stay where it spawns, no shields



    // Update is called once per frame
    void Update()
    {
        Movement();

    }

    public void AvoidShots()
    {
        Debug.Log("AvoidShot - parent of radar");
        StartCoroutine(LaserInRangeRoutine());

    }

    public void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -7.5)
        {
            float randomx = Random.Range(-18f, 18f);
            transform.position = new Vector3(randomx, 9f, 0);
        }

    }

    IEnumerator LaserInRangeRoutine()
    {
        //    //use while loop not (true)
        //    // have move for set time / distsnce
        if (_avoidShot != true)
        {
            _avoidShot = true;

            float moveTime = 2f;
            Debug.Log("Laser in range called");
            while (moveTime > 0)
            {

                transform.Translate(Vector3.left * _avoidSpeed * Time.deltaTime * _movement);


                yield return null;
                moveTime -= Time.deltaTime;
            }
            _avoidShot = false;
        }
    }

    public void ShieldActive(bool state)
    {   //active method called
        _shieldSpriteRenderer.gameObject.SetActive(state);
        _isEnemyShieldActive = state;
        if (state == true)
        {
            _enemyShieldLives = 1;
        }
    }
    public void GenerateShieldIndex(int random)
    {
        if (_enemyID == 5)
        {
            if (random >= 60 && random <= 70)
                ShieldActive(true);
        }
    }


    public void Damage()
    {
        //Debug.Log("damage called - avoid shot");
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
        //Debug.Log("death sequence called - avoid shot");
        if (_enemyDeathAnim != null)
        {
            _enemyDeathAnim.SetTrigger("OnEnemyDeath");
        }
        _speed = 0;
        _avoidSpeed = 0;
        _audioSource.Play();
        _isEnemyAlive = false;
        _spawnManager.EnemyDeath();
        Destroy(this.gameObject, .6f);
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
                }
                Damage();
            }
            if (other.CompareTag("Laser"))
            {
                //Debug.Log("laser called avoid shot");
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
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                Damage();
            }
        }
    }
}
