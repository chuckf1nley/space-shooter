using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private GameObject _enemyLaserPrefab;
    private Vector3 _offset = new Vector3(0, 0.5f, 0);
    private float _fireRate = 3f - 7f;

    private Player _player;
    private Animator _Anim;
    private AudioSource _audioSource;
    private bool _isAlive;

    //after 3 minutes increase enemy spawns/ create a second enemy so 2 spawn
    // after 180 seconds decrease spawn timer from 5 seconds to 3 seconds
    // after 300 seconds decrease spawn timer to 2 second
    // after 600 seconds decrease spawn timer to 1 enemy a second


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(FireRoutihe);
        if (_player == null)
        {
            Debug.LogError("player is null");
        }

        _Anim = GetComponent<Animator>();

        if (_Anim == null)
        {
            Debug.LogError("animator is null!");
        }
    
     }

    private void StartCoroutine(System.Func<IEnumerator> fireRoutihe)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.5f)
        {
            float randomx = Random.Range(-12f, 12f);
            transform.position = new Vector3(randomx, 10f, 0);
        }

    }

    IEnumerator FireRoutihe()
    {
        while (_isAlive)
        {
            yield return new WaitForSeconds(_fireRate);
            Instantiate(_enemyLaserPrefab, transform.position + _offset, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _Anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.6f);
            
        }

        if (other.tag == "laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _Anim.SetTrigger("OnEnemyDeath");
            _speed =  0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
            
        }
    }

}
