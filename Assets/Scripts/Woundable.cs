using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Woundable : MonoBehaviour
{

    public float StartingHealth = 5;
    private float CurrentHealth;

    public event EventHandler<DamageTakenEventArgs> OnDamageTaken;
    public event EventHandler<DeathEventArgs> OnDeath;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        CurrentHealth = StartingHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        
        Debug.Log("DAMAGE TAKEN: " + damageAmount + ", Health left: " + CurrentHealth);
        OnDamageTaken?.Invoke(this, new DamageTakenEventArgs() { DamageAmount = damageAmount });

        if (IsKilled())
        {
            OnDeath?.Invoke(this, new DeathEventArgs() { Woundable = this });
        }
    }

    public float GetHealth()
    {
        return CurrentHealth;
    }

    public bool IsKilled()
    {
        return CurrentHealth <= 0;
    }

    public void Reset()
    {
        Initialize();
    }
}

public class DamageTakenEventArgs : EventArgs
{
    public float DamageAmount { get; set; }
}

public class DeathEventArgs : EventArgs
{
    public Woundable Woundable { get; set; }
}