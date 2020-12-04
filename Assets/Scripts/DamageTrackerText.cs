using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTrackerText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();

        Stats.SharedInstance.StatUpdate += (sender, args) => { _text.text = $"dmg: {args.TotalDamage} kills: {args.TotalKills}"; };
    }

}
