using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private float _speed = 4f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private int _enemyID; //0 normal enemy, 1 enemy at right angle, 2 enemy at left angle
    [SerializeField] private AudioClip _laserSoundClip;
    
    private float _fireRate = 3f;
    private float _canfire = -1f;
    private Player _player;
    private Animator _Anim;
    private GameObject Shield;
    private bool _isEnemyAlive = false;
    private bool _canFire;
    private SpawnManager _spawnManager;
    private float fMinX = 50.0f;
    private float fMaxX = 250.0f;
    private int Direction = -18;

    public Vector3 _laserOffset { get; private set; }


    //after 3 minutes increase enemy spawns/ create a second enemy so 2 spawn
    // after 120 seconds decrease spawn timer from 5 seconds to 3 seconds

    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
       // _audioClip = GetComponent<AudioClip>();
        _isEnemyAlive = true;
        if (_player == null)
        {
            Debug.LogError("player is null");
        }

        _Anim = GetComponent<Animator>();

        if (_Anim == null)
        {
            Debug.LogError("animator is null!");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Enemy is null!");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        if (Time.time > _canfire && _isEnemyAlive == true)
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
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
        if (transform.position.x <= 18)
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        if (transform.position.x <= -18)
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
      
        Debug.Log("enemy moves left and right");
       

        if (transform.position.y < -7.5f)
        {
            float randomx = Random.Range(-18f, 18f);
            transform.position = new Vector3(randomx, 9f, 0);
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
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
                _Anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                _isEnemyAlive = false;
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
                _Anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                _isEnemyAlive = false;
                Destroy(GetComponent<Collider2D>());
                Destroy(GetComponent<EnemyLaser>());
                Destroy(this.gameObject, 2.6f);
            }
            if (other.CompareTag("Shield"))
                if (Shield != null)
                {
                    other.GetComponent<Shield>().Damage();
                    _player.AddScore(10);
                    _audioSource.Play();
                    _isEnemyAlive = false;
                    Destroy(GetComponent<EnemyLaser>());
                    Destroy(GetComponent<Collider2D>());
                }
        }
    }

    public void SetID(int _ID)
    {
        _enemyID = _ID;

        switch (_enemyID)
        {
            default:
                transform.rotation = Quaternion.identity;
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0, 0, 75);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0, 0, -75);
                break;

        }

    }

    private IEnumerator FireLaserCoroutine()
    {
        while (_canFire == true)
        {
            Vector3 _laserPos = transform.TransformPoint(_laserOffset);
            GameObject _laser = Instantiate(_laserPrefab, _laserPos, this.transform.rotation);
           _audioSource.Play();

            _laser.tag = "Enemy Laser";
           

            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

}
