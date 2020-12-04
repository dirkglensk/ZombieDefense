using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private GameObject _activeWeapon;
    private IWeapon _activeWeaponScript;
    [FormerlySerializedAs("availableWeapons")] public GameObject[] enabledWeapons;
    private GameObject[] _initializedWeapons;
    private Camera _playerCamera;

    private Vector3 currentTarget;

    private LayerMask aimMask;

    private Player _player;
    
    void Start()
    {
        _playerCamera = Camera.main;
        _player = GetComponent<Player>();
        aimMask = LayerMask.GetMask("Default", "Enemies");

        InitWeapons();
    }

    private void InitWeapons()
    {
        var weaponMount = RecursiveFindChild(transform, "WeaponMount").transform;

        _initializedWeapons = new GameObject[enabledWeapons.Length];
        
        for (int i = 0; i < enabledWeapons.Length; i++)
        {
            _initializedWeapons[i] = Instantiate(enabledWeapons[i], weaponMount.position, weaponMount.rotation, weaponMount);
            var weaponScript = _initializedWeapons[i].GetComponent<IWeapon>();
            weaponScript.SetUser(_player.GetCharacter());
            _initializedWeapons[i].SetActive(false);
        }

        _activeWeapon = _initializedWeapons[0];
        
        SetActiveWeapon(0);
    }

    private void SetActiveWeapon(int index)
    {
        _activeWeapon.SetActive(false);
        
        _activeWeapon = _initializedWeapons[index];
        _activeWeaponScript = _activeWeapon.GetComponent<IWeapon>();
        _activeWeapon.SetActive(true);
    }

    void Update()
    {
        //LayerMask aimTargetMask = 1 << LayerMask.NameToLayer("AimPlane");
        //LayerMask aimTargetMask = LayerMask.GetMask("")
        
        _activeWeaponScript.SetTargetPoint(PlayerAimLocation());
        
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Weapon Switch!");
            SetActiveWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Weapon Switch!");
            SetActiveWeapon(1);
        }
    }
    
    void Shoot()
    {
        _activeWeaponScript.Shoot();
    }
    
    private Vector3 PlayerAimLocation()
    {
        var ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        var aimPoint = Vector3.zero;

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, aimMask))
        {
            aimPoint = hit.point;
            Debug.DrawLine(ray.origin, hit.point, Color.magenta);
        }

        return aimPoint;
    }

    GameObject RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent) {
            if(child.name == childName)
                return child.gameObject;
            else 
                return RecursiveFindChild(child, childName);
        }

        return null;
    }
}
