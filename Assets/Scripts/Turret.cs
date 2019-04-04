using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Camera cam;
    private Vector3 point;
    public Vector3 playerGivenTarget;

    private Transform _muzzlePosition;
    
    private Queue<GameObject> _bulletQueue;
    
    void Start()
    {
        cam = Camera.main;
        _muzzlePosition = transform.Find("Muzzle").transform;
       
        _bulletQueue = ObjectPools.SharedInstance.GetQueue("bullet");
    }

    void OnGUI()
    {
    }

    private void Update()
    {
        //var ray = new Ray(cam.transform.position, point);
        //var raycast = Physics.Raycast(ray, out var hit);
        var direction = playerGivenTarget - transform.position;
        Debug.DrawRay(transform.position,  direction, Color.red);

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void Shoot()
    {
        if (_bulletQueue.Count < 1)
        {
            return;
        }
        var newBullet = _bulletQueue.Dequeue();
        newBullet.SetActive(true);
        newBullet.transform.position = _muzzlePosition.position;
        newBullet.transform.rotation = _muzzlePosition.rotation;
    }
}
