using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public event EventHandler<ProjectileImpactEventArgs> OnProjectileImpact;
    
    public static GameEvents SharedInstance;
    
    private void Awake()
    {
        if (SharedInstance != null)
        {
            Destroy(this);
        }
        
        SharedInstance = this;
    }
    
    void Start()
    {
        OnProjectileImpact += HandleProjectileImpact;
    }

    private void HandleProjectileImpact(object sender, ProjectileImpactEventArgs eventArgs)
    {
        var woundable = eventArgs.ImpactedObject.GetComponent<Woundable>();
        if (woundable)
        {
            woundable.TakeDamage(eventArgs.ProjectileObject.GetDamage(), eventArgs.ImpactLocation, eventArgs.ProjectileOrigin, eventArgs.OriginCharacter);
        }
    }

    public void RegisterProjectileImpact(ProjectileImpactEventArgs eventArgs)
    {
        OnProjectileImpact?.Invoke(this, eventArgs);
    }
}
