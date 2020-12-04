using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Poolable))]
public class OneShotVfx : MonoBehaviour
{
    private float _timer;

    private float _recycleTimeInSec;

    private Poolable _poolable;
    private ParticleSystem _particleSystem;

    private void OnEnable()
    {
        _timer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        _poolable = GetComponent<Poolable>();
        _particleSystem = GetComponent<ParticleSystem>();

        _recycleTimeInSec = _particleSystem.main.duration;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _recycleTimeInSec)
        {
            _poolable.Requeue();
        }
    }
}
