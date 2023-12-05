using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour
{
    //handle to text 
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _exitGame;
    [SerializeField] private TMP_Text _restartText;
    [SerializeField] private TMP_Text _takeTheL;
    [SerializeField] private TMP_Text _laserAmmoText;
    [SerializeField] private TMP_Text _thrusterText;
    [SerializeField] private TMP_Text _thrusterInactiveText;
    [SerializeField] private TMP_Text _shield_Lives_Display;
    [SerializeField] private TMP_Text _bossDefatedPrefab;
    [SerializeField] private TMP_Text _youWinText;
    [SerializeField] private TMP_Text _currWave;
    private TMP_Text bossDefeated;
    private GameManager _gameManager;

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

    public void thruster() //the text should appear only when the left shift key is pressed
    {
        _thrusterText.text = "thruster active";

    }
    public void thrusterInactive()
    {
        _thrusterInactiveText.text = "thruster on cooldown";
    }

    public void ShieldLives()
    {
        _shield_Lives_Display.text = "shield lives";
    }

    public void UpdateAmmo(int currentAmmo)
    {

        _laserAmmoText.text = "Ammo:" + currentAmmo.ToString();
    }

    public void UpdateWave(int currentWave)
    {
        _currWave.text = "Wave:" + currentWave.ToString(); 
    }

    void GameOverSequence()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();       
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _takeTheL.gameObject.SetActive(true);
        GameOverFlickerRoutine();
        RestartDisplay();
        ExitDisplay();

    }
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);

        }
    }

    public IEnumerator GameWonSequence()
    {
        yield return new WaitForSeconds(3);
        bossDefeated = Component.Instantiate(_bossDefatedPrefab);
        bossDefeated.enabled = true;
        _gameManager.GameOver();
        _youWinText.gameObject.SetActive(true);
        RestartDisplay();
        ExitDisplay();
        yield break;
    }

    private void RestartDisplay()
    {
        _restartText.enabled = true;
        Assert.IsTrue(_restartText.isActiveAndEnabled, "The Restart Game Text Must Be both " + "active and enabled for the text to show");
    }

    private void ExitDisplay()
    {
        _exitGame.enabled = true;
        Assert.IsTrue(_exitGame.isActiveAndEnabled, " the exit game text must be both" + "active and enabled for the text to show");
    }

    internal void UpdateWave()
    {
        throw new NotImplementedException();
    }
}
