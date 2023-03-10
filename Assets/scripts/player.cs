using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    public string _playerName = "samaxe";
    private float _horizontal;
    private float _vertical;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private Vector3 laserOffset = new Vector3(0, .884f, 0);
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canfire = -2f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _SpeedBoostPrefab;
    [SerializeField]
    private GameObject _ShieldPrefab;
    private bool _isShieldActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isTriple_ShotActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightengine;
    [SerializeField]
    private GameObject _leftengine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError(" The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI manager is null");
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

        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        //if lives is 2
        //enable right engine
        //else if lives is 1
        //enable left engine
        if (_lives == 2)
        {
            _leftengine.SetActive(true);
        }

        else if (_lives == 1)
        {
            _rightengine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
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

    public void SpeedBoostActive()
    {

        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);

    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
       
    }
}
