using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3.0f;
    [SerializeField] private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    private bool _isBossActive = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }
    public void Damage()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _spawnManager.StartSpawning();
        _audioSource.Play();
        Destroy(this.gameObject, 0.15f);
        if (_isBossActive == true)
        {
            _spawnManager.StopSpawning();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser"))
        {
         Destroy(other.gameObject);
            Damage();
        }
        if (other.CompareTag("PlayerMissile"))
        {
            Destroy(other.gameObject);
            Damage();
        }
    }

}
