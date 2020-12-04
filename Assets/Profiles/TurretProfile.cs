using UnityEngine;
using UnityEngine.Serialization;

namespace Profiles
{
    [CreateAssetMenu(menuName = "Turret Profiles/Turret Profile")]
    public class TurretProfile : ScriptableObject
    {
        public float timeBetweenShots;
        public int magazineSize;
        public float reloadTime;
        
        public float turnSpeed;
        [FormerlySerializedAs("maxInaccuracy")] public float maxBulletSpread;
        public float accuracy;

        public string projectileTag;
    }
}
