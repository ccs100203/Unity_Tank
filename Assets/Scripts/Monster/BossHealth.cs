﻿using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float StartingHealth = 1000f;
    public Slider HealthBar;
    public Image FillImage;
    public Color FullHealthColor = Color.red;
    public Color ZeroHealthColor = Color.yellow;

    private float CurrentHealth;
    private bool Dead;



    private void OnEnable()
    {
        CurrentHealth = StartingHealth;
        Dead = false;

        SetHealthUI();
    }


    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        CurrentHealth -= amount;
        SetHealthUI();

        if (CurrentHealth <= 0f && !Dead)
        {
            OnDeath();
        }
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        HealthBar.value = CurrentHealth;
        FillImage.color = Color.Lerp(ZeroHealthColor, FullHealthColor, CurrentHealth / StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        Dead = true;
        GameManager2.BossKills++;
        float TowerHp = GameObject.Find("Tower").GetComponent<TowerHealth>().CurrentHealth;
        TowerHp += 10;
        if (TowerHp >= 200)
            GameObject.Find("Tower").GetComponent<TowerHealth>().CurrentHealth = 200;
        else
            GameObject.Find("Tower").GetComponent<TowerHealth>().CurrentHealth = TowerHp;
        GameObject.Find("Tower").GetComponent<TowerHealth>().SetHealthUI();
        Destroy(gameObject);
    }
}