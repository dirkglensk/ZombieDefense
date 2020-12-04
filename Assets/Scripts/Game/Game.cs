using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game SharedInstance;

    private GameSettings _gameSettings;
    private GameEvents _gameEvents;
    private ObjectPools _objectPools;
    
    public ObjectPools Pools 
    {
        get => _objectPools;
    }

    public GameEvents Events
    {
        get => _gameEvents;
    }

    private void Awake()
    {
        if (SharedInstance != null)
        {
            Destroy(this);
        }
        
        SharedInstance = this;
        _gameSettings = GameSettings.SharedInstance;
        _gameEvents = GameEvents.SharedInstance;
        _objectPools = ObjectPools.SharedInstance;
    }    
    
}

public class ProjectileImpactEventArgs : EventArgs
{
    public Projectile ProjectileObject { get; set; }
    public GameObject ImpactedObject { get; set; }
    public Transform ImpactLocation { get; set; }
    public Vector3 ProjectileOrigin { get; set; }
    public Transform OriginCharacter { get; set; }
}