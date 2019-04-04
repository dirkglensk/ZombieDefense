using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Woundable))]
public class Walker : Enemy
{
    private Rigidbody _rigidbody;
    public AttackerProfile Profile;

    private float _moveSpeed;
    private Woundable _woundable;

    [SerializeField] 
    private bool wantsDirectionChange = false;
    private Quaternion _directionChangeDirection;
    public float directionChangeChance = 0.01f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _moveSpeed = Profile.moveSpeed * (1 + Random.Range(0, Profile.moveSpeedVariation));
        _woundable = GetComponent<Woundable>();
        
        _woundable.OnDamageTaken += OnDamageTaken;
        _woundable.OnDeath += WoundableOnOnDeath;

        RestartCoroutines();
    }

    public void RestartCoroutines()
    {
        StartCoroutine(DirectionChangeCoroutine());
    }
    
    private void WoundableOnOnDeath(object sender, DeathEventArgs e)
    {
        Debug.Log("UNNATURAL DEATH!");
        Despawn(e.Woundable);
    }

    private void OnDamageTaken(object sender, DamageTakenEventArgs eventArgs)
    {
        wantsDirectionChange = false;
        ChangeDirection();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(Vector3.forward) * _moveSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {

    }

    public void Despawn(Woundable woundable)
    {
        woundable.Reset();
        Despawn();
    }

    IEnumerator DirectionChangeCoroutine()
    {
        while (true)
        {
            if (wantsDirectionChange && Random.value < directionChangeChance)
            {
                wantsDirectionChange = false;
                ChangeDirection();
            }
    
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void ChangeDirection()
    {
        var direction = Quaternion.RotateTowards(transform.rotation, _directionChangeDirection, 180f);
        transform.rotation = direction;
    }

    public void Despawn()
    {
        GameObject go;
        (go = gameObject).SetActive(false);
        ObjectPools.SharedInstance.ReturnToQueue(Profile.tag, go);
    }

    public void ContemplateDirectionChange(Quaternion newDirection)
    {
        _directionChangeDirection = newDirection;
        wantsDirectionChange = true;
    }

    public void PreventDirectionChange(Walker walker)
    {
        wantsDirectionChange = false;
    }
}
