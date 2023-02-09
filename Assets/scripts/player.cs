using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    public string _playerName = "samaxe";
    private float _horizontal;
    private float _vertical;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private bool _isTriple_ShotActive = false;
    private Vector3 laserOffset = new Vector3(0, .884f, 0);
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canfire = -2f;
    [SerializeField]
    private int Player_lives = 3;
    private SpawnManager _spawnManager;
    
   

    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError(" The Spawn Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {


        CalculaateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
        {
            FireLaser();
        }



        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);



    }

    void CalculaateMovement()

    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(_horizontal, _vertical, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);

        }
        else if (transform.position.y <= -4.8f)
        {
            transform.position = new Vector3(transform.position.x, -4.8f, 0);
        }

        if (transform.position.x > 18f)
        {
            transform.position = new Vector3(-18f, transform.position.y, 0);
        }
        else if (transform.position.x < -18f)
        {
            transform.position = new Vector3(18f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canfire = Time.time + _fireRate;


        if (_isTriple_ShotActive == true)

        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }

        else

        {
            Instantiate(_laserPrefab, transform.position + laserOffset, Quaternion.identity);
        }

    }



    public void Damage()
    {
        Player_lives--;

        if (Player_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTriple_ShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    
    }

    IEnumerator TripleShotPowerDownRoutine()
    { 
        yield return new WaitForSeconds(5.0f);
        _isTriple_ShotActive = false;
    }
      
}