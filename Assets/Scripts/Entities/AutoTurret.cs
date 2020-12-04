using System.Collections;
using System.Collections.Generic;
using Profiles;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Weapon))]
public class AutoTurret : MonoBehaviour
{
    private Vector3 point;
    private Walker _focusTarget;
    private bool _hasTarget;
    public bool _isOperated = true;

    private Transform _muzzlePosition;
    
    private LayerMask _enemiesLayerMask = 1 << 9;

    public TurretProfile _TurretProfile;

    private float _shotTimer;
    private int _bulletsInMagazine = 0;
    
    void Start()
    {
        _muzzlePosition = transform.Find("Muzzle").transform;

        StartCoroutine(TrackActiveTarget());
    }

    public void SetOperated(bool value)
    {
        _isOperated = value;
    }

    private void Update()
    {
        _shotTimer += Time.deltaTime;

        if (_hasTarget && _isOperated)
        {
            TrackPosition(_focusTarget.transform.position);
            Shoot();
        }
    }

    private void TrackPosition(Vector3 target)
    {
        var direction = target - transform.position;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction),
            _TurretProfile.turnSpeed * Time.deltaTime);
        
        Debug.DrawRay(transform.position, direction, Color.red);
    }

    private IEnumerator TrackActiveTarget()
    {
        while (true) { 
            
            if (!_hasTarget)
            {
                FindNewTarget();
            }
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void FindNewTarget()
    {
        var colliders = Physics.OverlapSphere(transform.position, 30f, _enemiesLayerMask);
        if (colliders.Length < 1) return;
            
        var randomIndex = Random.Range(0, colliders.Length);
        _focusTarget = colliders[randomIndex].GetComponent<Walker>();
        if (ReferenceEquals(_focusTarget, null))
        {
            return; 
        }
        
        _hasTarget = true;
        _focusTarget.GetWoundable().OnDeath += OnFocusTargetDeath; 
    }

    private void Shoot()
    {
        if (_shotTimer < _TurretProfile.timeBetweenShots) return;
        
        if (_bulletsInMagazine < 1)
        {
            if (_shotTimer >= _TurretProfile.reloadTime)
            {
                _bulletsInMagazine += _TurretProfile.magazineSize;
            }
            
            return;
        }

        _shotTimer = 0;
        _bulletsInMagazine -= 1;
        var newBullet = ObjectPools.SharedInstance.GetObjectFromQueue(_TurretProfile.projectileTag);
        newBullet.SetActive(true);
        newBullet.transform.position = _muzzlePosition.position;
        newBullet.transform.rotation = _muzzlePosition.rotation * InaccuracyQuaternion();
    }
    
    private Quaternion InaccuracyQuaternion()
    {
        var accValue = _TurretProfile.maxBulletSpread * Mathf.Abs((_TurretProfile.accuracy - 1));
        var randomVector = new Vector3(Random.Range(-accValue, accValue), Random.Range(-accValue, accValue), Random.Range(-accValue, accValue));
        var randomQuaternion = Quaternion.Euler(randomVector);
        return randomQuaternion;
    }

    private void OnFocusTargetDeath(object sender, DeathEventArgs e)
    {
        _hasTarget = false;
    }
}
