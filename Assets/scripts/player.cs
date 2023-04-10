using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    private float _speedMultiplier = 3;
    public string _playerName = "samaxe";
    private float _horizontal;
    private float _vertical;
    private Vector3 laserOffset = new Vector3(0, .884f, 0);
    [SerializeField] private float _fireRate = 0.25f;
    private float _canfire = -2f;
    private SpawnManager _spawnManager;
    private bool _isShieldActive = false;
    private bool _isTriple_ShotActive = false;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private GameObject _SpeedBoostPrefab;
    [SerializeField] private GameObject _ammoRefillPrefab;
    [SerializeField] private GameObject _healthPrefab;
    [SerializeField] private Shield _shieldVisualizer;
    [SerializeField] private GameObject _rightengine;
    [SerializeField] private GameObject _leftengine;
    [SerializeField] private int _score;
    [SerializeField] private int _maxLives = 3;
    private int _minLives = 0;
    [SerializeField] private int _currentLives;
    private int _currentAmmo;
    [SerializeField] private int _maxAmmo = 15;
    private int _minAmmo = 0;
    private GameObject _shield;
    private UIManager _uiManager;
    private AudioSource _audioSource;
   
    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private AudioClip _playerDeathSoundClip;

    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _currentAmmo = _maxAmmo;
        _currentLives = _maxLives;

        if (_spawnManager == null)
        {
            Debug.LogError(" The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI manager is null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on player is null!");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculaateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
        {
            // Debug.Log("Fired");
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

        {
            if (Input.GetKey(KeyCode.LeftShift)) _speed = 7f;

            else _speed = 3.5f;
        }

    }

    void FireLaser()
    {
        _canfire = Time.time + _fireRate;


        if (_isTriple_ShotActive == true && _currentAmmo > _minAmmo)

        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

            _audioSource.Play();
        }

        else if (_currentAmmo > _minAmmo)

        {
            Instantiate(_laserPrefab, transform.position + laserOffset, Quaternion.identity);

            _audioSource.Play();
        }

        else
        {
            Debug.Log("Player ammo = 0");
        }

        int _laserAmmoClamp = Mathf.Clamp(_currentAmmo, _minAmmo, _maxAmmo);

        _currentAmmo--;
        _uiManager.UpdateAmmo(_currentAmmo);
    }

    void FireMissile()
    {
        _canfire = Time.time + _fireRate;

        Instantiate(_missilePrefab, transform.position, Quaternion.identity);

    }


    public void Damage()
    {
        _currentLives--;
        int _currentLivesDamageClamp = Mathf.Clamp(_currentLives, _minLives, _maxLives);
        _currentLives = _currentLivesDamageClamp;
        _maxLives = 3;

        if (_isShieldActive == true)
        {
            _shieldVisualizer.Damage();
            if (_shieldVisualizer.ShieldStrength() <= 0)
            {
                _isShieldActive = false;
                _shieldVisualizer.ShieldActive(false);
            }

            return;
        }

        if (_currentLives == 2)
        {
            _leftengine.SetActive(true);
        }

        else if (_currentLives == 1)
        {
            _rightengine.SetActive(true);
        }

        _uiManager.UpdateLives(_currentLives);

        if (_currentLives == 0)
        {
            Debug.Log("player out of lives" + _minLives);
            _spawnManager.OnPlayerDeath();
            _audioSource.Play();
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

        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ActivateShield()
    {
        if (_shield != null)
        {
            Destroy(_shield);
        }

        _isShieldActive = true;
        _shieldVisualizer.ShieldActive(true);


    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);


    }

    public void AmmooRefillActive()
    {

        _currentAmmo = _maxAmmo;
       // int _currentAmmoLaserAmmoClamp = Mathf.Clamp(_currentAmmo, _maxAmmo, _minAmmo);
        //_currentAmmo = _currentAmmoLaserAmmoClamp;
        _uiManager.UpdateAmmo(_currentAmmo);

    }

    public void Heal()
    {
        _currentLives++;
        Debug.Log("health powerup collected");
        int _currentLivesHealClamp = Mathf.Clamp(_currentLives, _minLives, _maxLives);
        _currentLives = _currentLivesHealClamp;
      
        _uiManager.UpdateLives(_currentLives);
        RestoreHealthVisualizer();
    }
    public void RestoreHealthVisualizer()
    {
        if (_currentLives > 2)
        {
            _leftengine.SetActive(false);
        }

        else if (_currentLives > 1)
        {
            _rightengine.SetActive(false);
        }
    }
}
