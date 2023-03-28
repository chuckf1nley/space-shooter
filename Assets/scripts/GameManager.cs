using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    
    [SerializeField]
    private bool _isGameOver;
    public UIManager uIManager;
    private readonly UIManager uiManager;

    void Start()
    {
       

    }

    private void Update()
    {
        //if the r key was pressed
        //restart current scene
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
    }

    public void UpdateShieldStrength(int shieldStrength)
    {
        this.uiManager.UpdateShieldStrength(shieldStrength);
        GameObject managerObject = GameObject.Find("Shield");
        UIManager uiManager = managerObject.GetComponent<UIManager>();
       
    }
}
