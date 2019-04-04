using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public event EventHandler<ProjectileImpactEventArgs> OnProjectileImpact;

    public static Game SharedInstance;

    private void Awake()
    {
        if (SharedInstance != null)
        {
            Destroy(this);
        }
        
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnProjectileImpact += HandleProjectileImpact;
    }

    private void HandleProjectileImpact(object sender, ProjectileImpactEventArgs eventArgs)
    {
        // set projectile inactive and requeue
        eventArgs.ProjectileObject.gameObject.SetActive(false);
        var queue = ObjectPools.SharedInstance.GetQueue(eventArgs.ProjectileObject.ObjectPoolTag);
        queue.Enqueue(eventArgs.ProjectileObject.gameObject);
        
        Debug.Log("Projectile " + eventArgs.ProjectileObject.name  + " hit object " + eventArgs.ImpactedObject.name);

        var woundable = eventArgs.ImpactedObject.GetComponent<Woundable>();
        if (woundable)
        {
            woundable.TakeDamage(eventArgs.ProjectileObject.GetDamage());
        }
    }

    public void RegisterProjectileImpact(ProjectileImpactEventArgs eventArgs)
    {
        OnProjectileImpact?.Invoke(this, eventArgs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}

public class ProjectileImpactEventArgs : EventArgs
{
    public Projectile ProjectileObject { get; set; }
    public GameObject ImpactedObject { get; set; }
}