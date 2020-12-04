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

    private Stats _stats;

    public GameObject PhysicalHitParticleObject;

    private void Start()
    {
        _stats = Stats.SharedInstance;
        _stats.RegisterWoundable(this);
        
        Initialize();
    }

    private void Initialize()
    {
        CurrentHealth = StartingHealth;
    }

    public void TakeDamage(float damageAmount, Transform damageTransform, Vector3 damageOrigin, Transform damageSource)
    {
        var vfx = ObjectPools.SharedInstance.GetObjectFromQueue("blood_splatter_impact");
        vfx.transform.position = damageTransform.position;
        vfx.SetActive(true);
        
        CurrentHealth -= damageAmount;
        
        OnDamageTaken?.Invoke(this, new DamageTakenEventArgs() { DamageAmount = damageAmount, DamageSource = damageSource, DamageOrigin = damageOrigin});

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

    private void OnEnable()
    {
        Initialize();
    }
}

public class DamageTakenEventArgs : EventArgs
{
    public float DamageAmount { get; set; }
    public Transform DamageSource { get; set; }
    public Vector3 DamageOrigin { get; set; }
}

public class DeathEventArgs : EventArgs
{
    public Woundable Woundable { get; set; }
}