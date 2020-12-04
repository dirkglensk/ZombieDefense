using UnityEngine;
using UnityEngine.Serialization;

namespace Profiles
{
    [CreateAssetMenu(menuName = "Weapon Profiles/Weapon Profile")]
    public class WeaponProfile : ScriptableObject
    {
        public float timeBetweenShots;
        public int magazineSize;
        public float reloadTime;
        
        public float maxBulletSpread;
        public float accuracy;

        public int bulletsPerShot = 1;

        public string projectileTag;
    }
}
