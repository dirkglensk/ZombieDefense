using Interfaces;
using UnityEngine;

public interface IWeapon
{
    void Shoot();
    void SetTargetPoint(Vector3 newTarget);

    void SetUser(ICharacter character);
}
