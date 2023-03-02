using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum alphaValue
{ 
    Shrinking, 
    Growing
}


//create ui for the powerups so i can have 3 of each and activate them at will

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
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;

    private GameManager _gameManager;
   
    public alphaValue currentAlphaValue;
    public float CommentMinAlpha;
    public float CommentMaxAlpha;
    public float CommentCurrentAlpha;

    // Start is called before the first frame update
    void Start()
    {
        CommentCurrentAlpha = 0.2f;
        CommentMinAlpha = 1f;
        CommentMaxAlpha = 1f;
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    private void Update()
    {
        AlphaComments(); 
    }
    public void AlphaComments()
    {
        if (currentAlphaValue == alphaValue.Shrinking)
        {
            CommentCurrentAlpha = CommentCurrentAlpha - 0.1f;
            _gameOverText.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, CommentCurrentAlpha);
            if (CommentCurrentAlpha <= CommentMinAlpha)
            {
                currentAlphaValue = alphaValue.Growing;
            }
        }
        else if (currentAlphaValue == alphaValue.Growing)
        {
            CommentCurrentAlpha = CommentCurrentAlpha + 0.1f;
            _gameOverText.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, CommentCurrentAlpha);
            if(CommentCurrentAlpha>= CommentMaxAlpha)
            {
                currentAlphaValue = alphaValue.Shrinking;
            }
        }

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

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        
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
