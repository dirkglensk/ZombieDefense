using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Profiles/Attacker Profile")]
public class AttackerProfile : ScriptableObject
{
    public string tag;
    public float moveSpeed;
    public float moveSpeedVariation;
    public float attackSpeed;
    public float attackDamage;
}
