using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    
    public float _speed = 10f;
    [SerializeField]
    private float damage = 2f;
    private Game _game;
    private Poolable _poolable;
    private Rigidbody _rigidbody;
    private Vector3 origin;
    private Transform originCharacter;

    // Start is called before the first frame update
    void Start()
    {
        _game = Game.SharedInstance;
    }

    private void Awake()
    {
        _poolable = GetComponent<Poolable>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnEnable()
    {
        _rigidbody.AddRelativeForce(Vector3.forward * _speed, ForceMode.Impulse);
    }

    public float GetDamage()
    {
        return damage;
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.position = Vector3.zero;
        _rigidbody.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void SetOrigin(Vector3 projectileOrigin, Transform characterOrigin)
    {
        origin = projectileOrigin;
        this.originCharacter = characterOrigin;
    }

    private void OnTriggerEnter(Collider other)
    {      
        _game.Events.RegisterProjectileImpact(new ProjectileImpactEventArgs
        {
            ProjectileObject = this,
            ImpactedObject = other.gameObject,
            ImpactLocation = transform,
            OriginCharacter = originCharacter,
            ProjectileOrigin = origin
        });
        
        PlayProjectileVfx();
        
        _poolable.Requeue();
    }

    private void PlayProjectileVfx()
    {
        var vfx = ObjectPools.SharedInstance.GetObjectFromQueue("pea_impact_vfx");
        vfx.transform.position = transform.position;
        vfx.SetActive(true);
    }

}
