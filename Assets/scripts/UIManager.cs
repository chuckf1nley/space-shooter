using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //handle to text 
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;

    // Start is called before the first frame update
    void Start()
    {
       // _liveSprites(CurrentPlayerLives = 3);
        _scoreText.text = "Score: " + 0;

    }

    public void UpdateScore(int playerscore)
    {
        _scoreText.text = "Score:" + playerscore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
       
        _livesImage.sprite = _liveSprites[currentLives];
    
    }

}
