﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicObjectHealth : MonoBehaviour
{
    public int MaxHealth = 30;
    public float CurrentHealth = 30;
    public int HealthModifier = 1;

    public void UpdateHealth(float damage) {
        
        CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        Debug.Log("Health: " + CurrentHealth);

        if (CurrentHealth == 0) {
            
        };
    }
}
