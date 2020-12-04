using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

    public static GameSettings SharedInstance;

    public LevelProfile[] levelProfiles;
    private LevelProfile _currentLevelProfile;
    
    private void Awake()
    {
        if (SharedInstance != null)
        {
            Destroy(this);
        }
        
        SharedInstance = this;
    }

    private void Start()
    {
        if (levelProfiles.Length < 1) return;

        _currentLevelProfile = levelProfiles[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
