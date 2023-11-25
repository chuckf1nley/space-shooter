using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject _weapon;    
    [SerializeField] private float _speed = 3f;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private int _enemyID;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _audioClip;
    private float _positionX;
    private float _fireRate = 2f;
    private float _startX;
    private int _bossHealth;
    private Player _player;
    private AudioSource _audioSource;
    private Animator _enemyDeathAnim;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _audioClip = GetComponent<AudioClip>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
