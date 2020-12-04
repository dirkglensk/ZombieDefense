using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Woundable))]
[RequireComponent(typeof(Poolable))]
public class Walker : Enemy, IWoundableEnemy
{
    private Rigidbody _rigidbody;
    public AttackerProfile Profile;

    private float _moveSpeed;
    private Woundable _woundable;
    private Poolable _poolable;

    [SerializeField] 
    private bool wantsDirectionChange = false;
    private Quaternion _directionChangeDirection;
    private float _directionChangeChance = 0.01f;

    private Transform ThreatTarget;
    private bool _hasThreatTarget;

    private Animator _animator;
    private bool hasRootMotion = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _poolable = GetComponent<Poolable>();
        
        _rigidbody = GetComponent<Rigidbody>();
        _moveSpeed = Profile.moveSpeed * (1 + Random.Range(0, Profile.moveSpeedVariation));
        _woundable = GetComponent<Woundable>();
        
        _woundable.OnDamageTaken += OnWoundableDamageTaken;
        _woundable.OnDeath += WoundableOnDeath;

        _animator = GetComponent<Animator>();
        if (_animator != null)
        {
            hasRootMotion = _animator.hasRootMotion;
            _animator.speed = _moveSpeed;
        }
    }

    public void SetThreatTarget(Transform target)
    {
        wantsDirectionChange = false;
        _hasThreatTarget = true;
        ThreatTarget = target;
    }

    public Poolable GetPoolable()
    {
        return _poolable;
    }

    public void RestartCoroutines()
    {
        //for essential coroutines
    }
    
    private void WoundableOnDeath(object sender, DeathEventArgs e)
    {
        Despawn(e.Woundable);
    }

    private void OnWoundableDamageTaken(object sender, DamageTakenEventArgs eventArgs)
    {
        wantsDirectionChange = false;
        ChangeDirection(eventArgs.DamageSource);
    }

    private void FixedUpdate()
    {
        if (hasRootMotion) return;
        //_rigidbody.AddRelativeForce(Vector3.forward * _moveSpeed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(Vector3.forward) * _moveSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (_hasThreatTarget)
        {
            ChangeDirection(ThreatTarget);
        }
    }

    private void OnEnable()
    {
        RestartCoroutines();
    }

    private void OnDisable()
    {
        Reset();
    }

    private void Reset()
    {
        _hasThreatTarget = false;
    }

    public void Despawn(Woundable woundable)
    {
        _poolable.Requeue();
    }

    IEnumerator DirectionChangeCoroutine()
    {
        while (wantsDirectionChange)
        {
            if (Random.value < _directionChangeChance)
            {
                wantsDirectionChange = false;
                ChangeDirection(_directionChangeDirection);
            }
    
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void ChangeDirection(Quaternion newDirection)
    {
        var direction = Quaternion.RotateTowards(transform.rotation, newDirection, 180f);
        transform.rotation = Quaternion.Euler(0, direction.eulerAngles.y, direction.eulerAngles.z);
    }

    public void ChangeDirection(Transform target)
    {
        Debug.Log(target);
        var direction = target.position - transform.position;
        ChangeDirection(Quaternion.LookRotation(direction));
    }

    public void ContemplateDirectionChange(Quaternion newDirection, float changeFrequency)
    {
        _directionChangeChance = changeFrequency;
        _directionChangeDirection = newDirection;
        wantsDirectionChange = true;

        StartCoroutine(DirectionChangeCoroutine());
    }

    public void PreventDirectionChange(Walker walker)
    {
        wantsDirectionChange = false;
    }

    public Woundable GetWoundable()
    {
        return _woundable;
    }
}
