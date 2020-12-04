using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Profiles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour, IWeapon
{
    private Vector3 point;
    private Vector3 _targetPoint;

    private Transform _muzzlePosition;
    
    private float _shotTimer;
    private int _bulletsInMagazine = 0;

    public WeaponProfile weaponProfile;
    private string muzzleVfxName = "muzzle_smoke_heavy";

    private ICharacter _user;
    
    void Start()
    {
        _muzzlePosition = transform.Find("Muzzle").transform;
        _bulletsInMagazine = weaponProfile.magazineSize;
    }

    private void Update()
    {
        _shotTimer += Time.deltaTime;
        
        var direction = _targetPoint - transform.position;

        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    public void Shoot()
    {
        if (_shotTimer < weaponProfile.timeBetweenShots) return;
        if (_bulletsInMagazine < 1)
        {
            if (_shotTimer >= weaponProfile.reloadTime)
            {
                _bulletsInMagazine += weaponProfile.magazineSize;
            }
            
            return;
        }

        _shotTimer = 0;
        _bulletsInMagazine -= 1;
        
        CreateProjectiles();
    }

    private void CreateProjectiles()
    {
        var newBullet = ObjectPools.SharedInstance.GetObjectFromQueue<Projectile>(weaponProfile.projectileTag);
        newBullet.transform.position = _muzzlePosition.position;
        newBullet.transform.rotation = _muzzlePosition.rotation * InaccuracyQuaternion();
        newBullet.SetOrigin(this.transform.position, _user.GetTransform());
        newBullet.gameObject.SetActive(true);

        PlayMuzzleVfx();
    }

    private void PlayMuzzleVfx()
    {
        var vfx = ObjectPools.SharedInstance.GetObjectFromQueue(muzzleVfxName);
        vfx.transform.position = _muzzlePosition.position;
        vfx.transform.rotation = _muzzlePosition.rotation;
        vfx.SetActive(true);
    }

    public void SetTargetPoint(Vector3 newTarget)
    {
        _targetPoint = newTarget;
    }

    public void SetUser(ICharacter user)
    {
        _user = user;
    }

    private Quaternion InaccuracyQuaternion()
    {
        var accValue = weaponProfile.maxBulletSpread * Mathf.Abs((weaponProfile.accuracy - 1));
        var randomVector = new Vector3(Random.Range(-accValue, accValue), Random.Range(-accValue, accValue), Random.Range(-accValue, accValue));
        var randomQuaternion = Quaternion.Euler(randomVector);
        return randomQuaternion;
    }

}
