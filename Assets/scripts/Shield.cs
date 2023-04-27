using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int _lives = 3;
    private SpriteRenderer _spriteRenderer;
    private UIManager _uiManager;
    private Color _auxColor;
    private UIManager uIManager;




    private void Start()
    {
        //Debug.Log("Shield start");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _auxColor = _spriteRenderer.color;

        if (_spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI manager is null");
        }
    }
    public void Update()
    {
        //_uiManager = GameObject.Find("Canvas").GetComponent<_shield_Lives_Display>;
    }

    public void Damage()
    {
        //Debug.Log("Shield:: Damage()");
        _lives--;

        if (_lives <= 0)
        {
            _auxColor.a = 1;
            _auxColor = Color.white;

            return;
        }
       
        switch (_lives)
        {
            case 2:
                _auxColor = Color.yellow;
                break;
            case 1:
                _auxColor = Color.red;
                break;
        }

        _spriteRenderer.color = _auxColor;

    }
    public void ShieldActive(bool state)
    {
        if (state == true)
        {
            _lives = 3;
            _auxColor.a = 1;
            _auxColor = Color.white;
        }
        _spriteRenderer.enabled = state;
    }

    public int ShieldStrength()
    {
        return _lives;

    }
    
}
