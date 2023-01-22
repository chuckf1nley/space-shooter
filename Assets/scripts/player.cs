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
    private GameObject _Triple_ShotPrefab;
    private Vector3 laserOffset = new Vector3(0, .884f, 0);
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canfire = -2f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
   
    //private bool Triple_ShotOffset = new Vector3(-0.529, 0.1061, -0.120(0.940, -0.454, 0.120));


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
      

        //if space key press,
        //if Triple_ShotActive is true
        //fire 3 lasers

        //else fire 1 laser

        //instantiate 3 lasers

        if (_isTripleShotActive == true)

        {
            Instantiate(_Triple_ShotPrefab, transform.position, Quaternion.identity);
        }

        else 
        
        {
            Instantiate(_laserPrefab, transform.position + laserOffset, Quaternion.identity);
        }

    }



    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }


}