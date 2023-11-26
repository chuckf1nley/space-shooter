using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject _weapon;    
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _enemyID;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _bossSpawn;
    [SerializeField] private AudioClip _audioDeathClip;
    private float _positionX;
    private float _fireRate = 2f;
    private float _startX;
    private float _direction;
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
    }



}
