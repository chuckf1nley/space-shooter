using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//create ui for the powerups so i can have 3 of each and activate them on number press

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
    
    [SerializeField] private GameManager _gameManager;
    
   
    public void UpdateScore(int playerscore)
    {
        _scoreText.text = "Score:" + playerscore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
       
        _livesImage.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }

    }
    public void thruster(int _thruster) //the text should appear only when the shift key is pressed
    {
       _thrusterText.text = "thruster active";
    }
   

    public void UpdateAmmo(int currentAmmo)
    {

        _laserAmmoText.text = $"Ammo:{currentAmmo}";
    }

    
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _takeTheL.gameObject.SetActive(true);
       
        
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);

        }
    }

    internal float SetActive(bool v)
    {
        throw new NotImplementedException();
    }
}
