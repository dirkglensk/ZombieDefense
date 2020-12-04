using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Walker))]
public class Aggroable : MonoBehaviour
{
    private float _aggroRange = 2f;
    private readonly Collider[] possibleThreats = new Collider[1];
    private LayerMask threatLayerMask;
    private float _timeBetweenChecks = 2f;

    public bool _isAggroable = true;

    private Transform _aggroTarget;
    private bool _hasAggroTarget = false;

    private Walker _walker;

    // Start is called before the first frame update
    void Start()
    {
        threatLayerMask = LayerMask.GetMask("Friendlies", "Player");
        _walker = GetComponent<Walker>();

        StartCoroutine(CheckThreats());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckThreats()
    {
        while (_isAggroable)
        {
            if (_hasAggroTarget) yield return new WaitForSeconds(5f);
            
            var size = Physics.OverlapSphereNonAlloc(transform.position, _aggroRange, possibleThreats, threatLayerMask);
            if (size > 0)
            {
                _aggroTarget = possibleThreats.First().transform;
                _hasAggroTarget = true;
                _walker.SetThreatTarget(_aggroTarget);
            }
            yield return new WaitForSeconds(_timeBetweenChecks);
        }
    }
    
}
