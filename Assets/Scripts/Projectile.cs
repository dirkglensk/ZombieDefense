using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    
    public float _speed = 10f;
    [SerializeField]
    private float damage = 1f;
    private Game _game;

    public readonly string ObjectPoolTag = "bullet";
    // Start is called before the first frame update
    void Start()
    {
        _game = Game.SharedInstance;
    }

    public float GetDamage()
    {
        return damage;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {      
        _game.RegisterProjectileImpact(new ProjectileImpactEventArgs
        {
            ProjectileObject = this,
            ImpactedObject = other.gameObject
        });
    }

}
