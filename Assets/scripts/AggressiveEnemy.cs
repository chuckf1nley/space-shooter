using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyShieldPrefab;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _enemyThruster;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private int _enemyID; // 3 aggro enemy
    private Animator _enemyDeathAnim;
    private AudioSource _audioSource;
    private float _startX;
    private Player _player;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = true;
    private SpawnManager _spawnManager;
    private int _direction;


    //enemy needs to move towards and kamikaze the player
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _enemyDeathAnim = transform.GetComponent<Animator>();

        _isEnemyAlive = true; 
        _startX = transform.position.x;
        _direction = Random.Range(0, 2);

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
            Debug.Log("AudioSource on aggro enemy is null");
        }



    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    public void CalculateMovement()
    {
        

    }

    public void Damage()
    {


    }

}
