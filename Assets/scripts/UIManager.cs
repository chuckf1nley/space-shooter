using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum alphaValue
{ 
    Shrinking, 
    Growing
}


//create ui for the powerups so i can have 3 of each and activate them at will

public class UIManager : MonoBehaviour

{
    //handle to text 
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartText;
    [SerializeField] private TMP_Text _takeTheL;
    //[SerializeField] private Sprite[] _shieldStrength;
    //[SerializeField] private Image _shieldStrengthimages = new Sprite[4];
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMP_Text _laserAmmoText;
   
    // Start is called before the first frame update
    void Start()
    {
                    
    }

    private void Update()
    {
       
    }
   

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

    public void UpdateAmmo(int currentAmmo)
    {

        _laserAmmoText.text = $"Ammo: { currentAmmo}";
    }

    //public void UpdateShiedstrength(int shieldStrength)
    //{
    //GameObject imageObject = _shieldStrengthimage.gameObject;
    //GameObject parentGameObject = imageGameObject.transform.parent.gaemObject;

    //_shieldStrengthimage.sprite = _shieldStrengthimage(shieldStrength);
    //_imageGameObject.SetActive(shieldStrength > 0);
    //parentGameObject.SetActive(shieldStrength > 0);
    //}

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
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
 
}
