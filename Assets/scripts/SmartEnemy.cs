using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyShieldPrefab;
    [SerializeField] private GameObject _smartEnemy;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] int _enemyID; // 4 smart enemy
    private Animator _enemyDeathAnim;
    private AudioSource audioSource;
    private float _startx;
    private float _distance;
    private float _enemyShieldStrength = 1;
    private Player _player;
    private bool _isEnemyAlive = true;
    private bool _isEnemyShieldActive = false;
    private SpawnManager spawnManager;
    private int _direction;
    private int _enemyShieldLives = 1;
    private int _enemyLives;
   
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Movement()
    {

    }

    public void Weapon()
    {

    }

    public void GenerateShield()
    {
        
    }
    public void ShieldStrength()
    {
        
    }

    public void EnemyDeathSequence()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
    }


}
