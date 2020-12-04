using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    private Game _game;
    void Start()
    {
        _game = Game.SharedInstance;
        
    }
}
