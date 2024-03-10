using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    
    [SerializeField] private bool _isGameOver;
    private bool _isPlayerVictorious;
    public UIManager uIManager;

    void Start()
    {
       

    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); // current game scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        
    }
    
    public void GameOver()
    {
        _isGameOver = true;
        _isPlayerVictorious = false;
    }

   public void YouWin()
    {
        _isPlayerVictorious = true;
        _isGameOver = false;

    }
}
