using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _negSpeedMultiplier = 1f;
    [SerializeField] private float _fireRate = 0.25f;
    [SerializeField] private float _missileFireRate = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private GameObject _SpeedBoostPrefab;
    [SerializeField] private GameObject _ammoRefillPrefab;
    [SerializeField] private GameObject _healthPrefab;
    [SerializeField] private GameObject _altFirePrefab;
    [SerializeField] private GameObject _negSpeeedPrefab;
    [SerializeField] private GameObject _altFire;
    [SerializeField] private GameObject _rightengine;
    [SerializeField] private GameObject _leftengine;
    [SerializeField] private GameObject _thruster;
    [SerializeField] private GameObject _shield;
    [SerializeField] private Shield _shieldVisualizer;
    [SerializeField] private TMP_Text _thrusterText;
    [SerializeField] private int _maxLives = 3;
    [SerializeField] private int _currentLives;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _missileMaxAmmo = 5;
    private string _playerName = "samaxe";
    private float _speedMultiplier = 3f;
    private float _horizontal;
    private float _vertical;
    private float _canfire = -2f;
    private float _missileFire;
    private float _powerupCollected;
    private float _powerupsOnScreen;
    private bool _powerupCollect;
    private bool _canMissileFire = false;
    private bool _isShieldActive = false;
    private bool _isTriple_ShotActive = false;
    private bool _isAltFireActive = false;
    private bool _firingConstantly = false;
    private bool _negSpeed = false;
    private int _maxAmmo = 15;
    private int _score;
    private int _minLives = 0;
    private int _minAmmo = 0;
    private int _currentMissiles;
    private SpawnManager _spawnManager;
    private Missile _missile;
    private Powerup _powerup;
    private Vector3 _missileOffset = new Vector3(0, 1f, 0);
    private TMP_Text _shield_Lives_Display;
    private CameraShake _camShake;
    private Vector3 _laserOffset = new Vector3(0, .884f, 0);
    private UIManager _uiManager;
    private Transform Powerup;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private AudioClip _playerDeathSoundClip;

    // Start is called before the first frame update
    void Start()
    {
        _camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_camShake == null)
        {
            Debug.LogError("Main Camera - CameraShake Script is null");
        }
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        Powerup = GameObject.FindGameObjectWithTag("Powerup").transform;
        // _missile =  GameObject.Find("Missile").GetComponent<Missile>();
        _audioSource = GetComponent<AudioSource>();
        _currentAmmo = _maxAmmo;
        _currentMissiles = _missileMaxAmmo;
        _currentLives = _maxLives;


        if (_spawnManager == null)
        {
            Debug.Log(" The Spawn Manager is NULL.");
            return;
        }

        if (_uiManager == null)
        {
            Debug.Log("The UI manager is null");
            return;
        }
        if (_audioSource == null)
        {
            Debug.Log("AudioSource on player is null!");
            return;
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        //CollectPowerups();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
        {
            // Debug.Log("Fired");
            FireLaser();
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (Input.GetKeyDown(KeyCode.E) && Time.time > _missileFireRate)
        {
            FireMissile();
        }
        
        if (Input.GetKeyDown(KeyCode.C) && Time.time > _powerupCollected)
        {
            PowerupCollected();
        }
    }

    void CalculateMovement()
    {
        Thruster();
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && _negSpeed == false) _speed = 7f;
        if (Input.GetKeyUp(KeyCode.LeftShift) && _negSpeed == false) _speed = 3.5f;

    }

    public void Thruster()
    {
        _thrusterText.text = "thruster active";
    }

    IEnumerator thrusterPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _thruster.SetActive(false);
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
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);

            _audioSource.Play();
        }
        else
        {
            Debug.Log("Player ammo = 0");
        }

        if (_isAltFireActive == true)
        {
            Instantiate(_altFirePrefab);

        }

        int _laserAmmoClamp = Mathf.Clamp(_currentAmmo, _minAmmo, _maxAmmo);
        _currentAmmo--;
        _uiManager.UpdateAmmo(_currentAmmo);
    }

    void FireMissile()
    {
        FireMissileCoroutine();

        _missileFire = Time.time + _missileFireRate;
        Instantiate(_missilePrefab, transform.position + _missileOffset, Quaternion.identity);

    }
    IEnumerator FireMissileCoroutine()
    {
        while (_canMissileFire == true)
        {
            Instantiate(_missilePrefab, transform.position + _missileOffset, Quaternion.identity);
            Vector3 _missilePos = transform.TransformPoint(_missileOffset);
            GameObject _missile = Instantiate(_missilePrefab, _missilePos, this.transform.rotation);
            _missile.tag = "Player Missile";
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 5f));

        }

    }

    public void Damage()
    {

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
        _currentLives--;

        if (_currentLives == 2)
        {
            _leftengine.SetActive(true);
            _camShake.ShakeCamera();
        }
        else if (_currentLives == 1)
        {
            _rightengine.SetActive(true);
            _camShake.ShakeCamera();
        }

        _uiManager.UpdateLives(_currentLives);
        if (_currentLives <= 0)
        {
            _camShake.ShakeCamera();
            _audioSource.Play();
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

    }
    public void ActivateShield()
    {
        _isShieldActive = true;
        _shieldVisualizer.ShieldActive(true);

    }
    public void shieldLives(int shield)
    {
        _shield_Lives_Display.text = "shield lives";
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

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AmmooRefillActive()
    {
        _currentAmmo = _maxAmmo;
        _currentMissiles = _missileMaxAmmo;
        int _currentAmmoLaserAmmoClamp = Mathf.Clamp(_currentAmmo, _minAmmo, _maxAmmo);
        _currentAmmo = _currentAmmoLaserAmmoClamp;
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

    public void AltFire()
    {
        _firingConstantly = true;
        StartCoroutine(AltFirePowerDownRoutine());
        StartCoroutine(AltFiring());
    }
    IEnumerator AltFiring()
    {
        while (_firingConstantly)
        {
            yield return new WaitForSeconds(_fireRate);
            Instantiate(_altFire, transform.position + _laserOffset, Quaternion.identity);
        }
    }
    IEnumerator AltFirePowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isAltFireActive = false;
        _firingConstantly = false;
    }

    public void NegSpeed()
    {
        if (_negSpeed == false)
        {
            _negSpeed = true;
            _speed *= _negSpeedMultiplier;
            StartCoroutine(NegSpeedPowerDownRoutine());
            StartCoroutine(thrusterPowerDownRoutine());
            Debug.Log("debuff collected");
        }
    }
    IEnumerator NegSpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _negSpeed = false;
        _speed /= _negSpeedMultiplier;
    }

    public void PowerupCollected()
    {

        Vector3 direction = _powerup.transform.position - transform.position;
        direction = direction.normalized;
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    //public void CollectPowerup(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        _powerup = FindObjectsOfType<Powerup>();
    //        _powerupCollect = true;
    //    }

    //    if (context.canceled)
    //    {
    //        _powerupCollect = false;
    //        _powerupsOnScreen.ToList().ForEach(print => print.ResumeDefaultMovement());
    //    }

    //}

    //public void CollectPowerups()
    //{
    //    if (_powerupCollect && _powerupsOnScreen.Any())
    //    {
    //        _powerupsOnScreen.ToList().ForEach(p =>
    //        {
    //            if (p != null)
    //            {
    //                p.MoveTowardsPosition(transform.position);
    //            }

    //        });
    //    }
    //}

}