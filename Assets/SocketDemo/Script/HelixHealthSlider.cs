using JUTPS;
using JUTPS.FX;
using JUTPSEditor.JUHeader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HelixHealthSlider : MonoBehaviour
{

    [Header("Settings")]
    public float Health = 100f;
    public float MaxHealth = 100f;
    public Slider slider;
    public Image fillImage;

    [Header("Effects")]
    public bool BloodScreenEffect = false;
    public GameObject BloodHitParticle;

    [Header("On Death Event")]
    public UnityEvent OnDeath;

    [Header("Stats")]
    public bool IsDead;

    void Start()
    {
        try
        {
            LimitHealth();
            CheckHealthState();
            UpdateHealthBar();
            //Debug.Log("slider.value =" + slider.value);
        }
        catch (Exception e)
        {
            Debug.Log($"HelixHealthSlider ---> Start ---> {e.ToString()}");
        }
    }
    private void LimitHealth()
    {
        Health = Mathf.Clamp(Health, 0, MaxHealth);
    }
    public void CheckHealthState()
    {
        LimitHealth();

        if (Health <= 0 && IsDead == false)
        {
            Health = 0;
            IsDead = true;
            //Disable all damagers0

            OnDeath.Invoke();
        }

        if (Health > 0) IsDead = false;
    }
    // Set the current health of the player
    public void SetHealth(float health)
    {
        try
        {
            Health = health;
            UpdateHealthBar();
        }
        catch (Exception e)
        {
            Debug.Log($"HelixHealthSlider ---> SetHealth ---> {e.ToString()}");
        }
       
    }

    public static void DoDamage(JUHealth health, float damage, Vector3 hitPosition = default(Vector3))
    {
        health.DoDamage(damage, hitPosition);

    }
    public void DoDamage(float damage, Vector3 hitPosition = default(Vector3))
    {

        Health -= damage;
        LimitHealth();
        Invoke(nameof(CheckHealthState), 0.016f);

        if (BloodScreenEffect) BloodScreen.PlayerTakingDamaged();
        if (hitPosition != Vector3.zero && BloodHitParticle != null)
        {
            GameObject fxParticle = Instantiate(BloodHitParticle, hitPosition, Quaternion.identity);
            fxParticle.hideFlags = HideFlags.HideInHierarchy;
            Destroy(fxParticle, 3);
        }
    }

    // Modify the health (e.g., decrease health when taking damage)
    public void ModifyHealth(float amount)
    {
        try
        {
            Health -= amount;
            //Debug.Log("currentHealth =" + currentHealth);
            Health = Mathf.Clamp(Health, 0f, MaxHealth);
            //Debug.Log("currentHealth =" + currentHealth);
            UpdateHealthBar();
        }
        catch (Exception e)
        {
            Debug.Log($"HelixHealthSlider ---> ModifyHealth ---> {e.ToString()}");
        }
       
    }

    // Update the UI elements of the health bar
    void UpdateHealthBar()
    {
        try
        {
            float fillAmount = Health / MaxHealth;
            //Debug.Log("FillAmount =" + fillAmount);
            slider.value = fillAmount;
            //Debug.Log("slider.value =" + slider.value);
            fillImage.fillAmount = fillAmount;
        }
        catch (Exception e)
        {
            Debug.Log($"HelixHealthSlider ---> UpdateHealthBar ---> {e.ToString()}");
        }
        
    }
}
