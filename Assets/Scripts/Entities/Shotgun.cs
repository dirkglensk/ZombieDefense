using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Profiles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Shotgun : MonoBehaviour, IWeapon
{
    private Vector3 point;
    private Vector3 _targetPoint;

    private Transform _muzzlePosition;
    
    private float _shotTimer;
    private int _bulletsInMagazine = 0;

    public WeaponProfile weaponProfile;

    private ICharacter _user;
    
    void Start()
    {
        _muzzlePosition = transform.Find("Muzzle").transform;

        _bulletsInMagazine = weaponProfile.magazineSize;
    }

    private void Update()
    {
        //Debug.Log("Weapon Update! " + _shotTimer);
        _shotTimer += Time.deltaTime;
        
        //var ray = new Ray(cam.transform.position, point);
        //var raycast = Physics.Raycast(ray, out var hit);
        var direction = _targetPoint - transform.position;
        Debug.DrawRay(transform.position,  direction, Color.red);

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
        
        for (int i = 0; i < weaponProfile.bulletsPerShot; i++)
        {
            Debug.Log("bullet created");
            var newBullet = ObjectPools.SharedInstance.GetObjectFromQueue(weaponProfile.projectileTag);
            newBullet.transform.position = _muzzlePosition.position;
            newBullet.transform.rotation = _muzzlePosition.rotation * InaccuracyQuaternion();
            
            newBullet.SetActive(true);
        }
    }
    
    public void SetTargetPoint(Vector3 newTarget)
    {
        _targetPoint = newTarget;
    }

    public void SetUser(ICharacter character)
    {
        _user = character;
    }

    private Quaternion InaccuracyQuaternion()
    {
        var accValue = weaponProfile.maxBulletSpread * Mathf.Abs((weaponProfile.accuracy - 1));
        var randomVector = new Vector3(Random.Range(-accValue, accValue), Random.Range(-accValue, accValue), Random.Range(-accValue, accValue));
        var randomQuaternion = Quaternion.Euler(randomVector);
        return randomQuaternion;
    }


}
