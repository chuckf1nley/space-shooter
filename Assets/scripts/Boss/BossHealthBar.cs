using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private UIManager _uimanager;
    private Image _healthBar;
    private float _healthTotal = 40f;
    private Laser _laser;
    private Missile _missile;
    private float _currentHealth;

    void Update()
    {
        if (_healthTotal <= 0)
        {
            _uimanager.GameWonSequence();
        }
        if (_laser)
        {
            TakeDamage(1);
        }
        if (_missile)
        {
            TakeDamage(5);
        }

        HealthBarCoroutine();
    }

    public void DisplayHealthBar()
    {
        if (!this.gameObject.activeSelf)
            this.gameObject.SetActive(true);
    }

    public void HideHealthBar()
    {
        if (this.gameObject.activeSelf)
            this.gameObject.SetActive(false);

    }

    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;
    }

    public void SetHealth(int health)
    {
        _slider.value = health;
    }

    public void TakeDamage(float damage)
    {
        _healthTotal -= damage;
        _healthBar.fillAmount = _healthTotal / 40f;
    }

     IEnumerator HealthBarCoroutine()
     {
        HideHealthBar();
        yield return new WaitForSeconds(3f);
        DisplayHealthBar();

        if (_currentHealth == 0)
        {
            HideHealthBar();
        }

     }
    /*
       transform.parent = _bossContainer.transform;
       HideHealthBar();
        yield return wait;
       DisplayHealthBar();
     */

}
