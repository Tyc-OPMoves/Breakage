using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _healthBar;
    [SerializeField] Image _healthBarOutline;
    Camera _mainCamera;

    float _healthBarDrainSpeed = 0.3f;
    float _currentHealthPercentage = 1;

    void Start()    
    {
        _mainCamera = Camera.main;
        UpdateHealthBarColor();
    }
    void Update()
    {
        RotateHealthBarTowardsCamera();
        UpdateHealthBarPercentage();
        UpdateHealthBarColor();

    }
    public void UpdateHealthbar(int maxHealth, int currentHealth)
    {
        _currentHealthPercentage = (float)currentHealth / maxHealth;
    }

    public void DisableHealthBar()
    {
        _healthBar.gameObject.SetActive(false);
        _healthBarOutline.gameObject.SetActive(false);
    }
    void RotateHealthBarTowardsCamera()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
    }
    void UpdateHealthBarColor()
    {
        _healthBar.color = new(1 - _healthBar.fillAmount, _healthBar.fillAmount, 0.2f);
    }   
    void UpdateHealthBarPercentage()
    {
        _healthBar.fillAmount = Mathf.MoveTowards(_healthBar.fillAmount,_currentHealthPercentage,_healthBarDrainSpeed*Time.deltaTime);
    }
}
