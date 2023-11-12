using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    //handle to text 
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartText;
    [SerializeField] private TMP_Text _takeTheL;
    [SerializeField] private TMP_Text _laserAmmoText;
    [SerializeField] private TMP_Text _thrusterText;
    [SerializeField] private TMP_Text _thrusterInactiveText;
    [SerializeField] private TMP_Text _shield_Lives_Display;
    [SerializeField] private GameManager _gameManager;
  
    public void UpdateScore(int playerscore)
    {
        _scoreText.text = "Score:" + playerscore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives < 0)
            currentLives = 0;

        _livesImage.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }

    }

    public void thruster() //the text should appear only when the shift key is pressed
    {
        _thrusterText.text = "thruster active";
        
    }
    public void thrusterInactive()
    {
        _thrusterInactiveText.text = "thruster on cooldown";
    }

    public void shieldLives(int shield)
    {
        _shield_Lives_Display.text = "shield lives";
    }

    public void UpdateAmmo(int currentAmmo)
    {

        _laserAmmoText.text = "Ammo:" + currentAmmo.ToString();
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _takeTheL.gameObject.SetActive(true);
        GameOverFlickerRoutine();
       
        
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);

        }
    }

}
