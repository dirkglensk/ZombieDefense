using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Stats : MonoBehaviour
{
    public static Stats SharedInstance;
    
    private Game _game;

    private int _kills;

    private float _damage;

    private void AddDamage(float damageAmount)
    {
        _damage += damageAmount;
        InvokeStatUpdateEvent();
    }

    private void AddKills(int killCount)
    {
        _kills += killCount;
        InvokeStatUpdateEvent();
    }
    
    public event EventHandler<StatUpdateEventArgs> StatUpdate;
    
    private void InvokeStatUpdateEvent()
    {
        StatUpdate?.Invoke(this, new StatUpdateEventArgs()
        {
            TotalDamage = _damage, 
            TotalKills = _kills
        });
    }

    private void Awake()
    {
        if (SharedInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            SharedInstance = this;
        }
    }

    public void RegisterWoundable(Woundable woundable)
    {
        woundable.OnDamageTaken += WoundableOnDamageTaken;
        woundable.OnDeath += WoundableOnDeath;
    }

    private void WoundableOnDeath(object sender, DeathEventArgs e)
    {
        AddKills(1);
    }

    private void WoundableOnDamageTaken(object sender, DamageTakenEventArgs e)
    {
        AddDamage(e.DamageAmount);
    }

    // Start is called before the first frame update
    void Start()
    {
        _game = Game.SharedInstance;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

public class StatUpdateEventArgs : EventArgs
{
    public int TotalKills { get; set; }
    public float TotalDamage { get; set; }
}
