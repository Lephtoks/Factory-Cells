using UnityEngine;

namespace Entities
{
    public static class BulletTypes
    {
        public static BulletType DEFAULT = new BulletType(Resources.Load<Sprite>("Textures/Bullets/DefaultBullet"));
    }
}