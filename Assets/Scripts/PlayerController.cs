using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Turret _playerControlledTurret;
    
    void Start()
    {
        _playerControlledTurret = FindObjectOfType<Turret>();
    }

    void Update()
    {
        LayerMask aimTargetMask = 1 << LayerMask.NameToLayer("AimPlane");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hit, 2000f, aimTargetMask)) {
            
            Debug.DrawRay(transform.position, hit.point - transform.position);

            _playerControlledTurret.playerGivenTarget = hit.point;
        }

        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        _playerControlledTurret.Shoot();
    }
    
    
}
