using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int _lives = 3;
    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        Debug.Log("Shield start");
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer is NULL");
        }
    }

    public void Damage()
    {
        Debug.Log("Shield:: Damage()");
        _lives--;

        if (_lives <= 0)
        {
            Destroy(gameObject);
            return;
        }

        Color auxColor = _spriteRenderer.color;

        switch (_lives)
        {
            case 2:
                auxColor.a = 0.1f;
                break;
            case 1:
                auxColor = Color.red;
                break;
        }

        _spriteRenderer.color = auxColor;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Shield:: Triggerenter" + other.name);
        if (other.CompareTag("Laser") || other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Damage();
        }
    }

    private void UpdateLives()
    {
        _lives = 3;
    }

}
