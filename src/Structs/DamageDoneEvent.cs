using UnityEngine;

public struct DamageDoneEvent
{
    public Weapon Weapon;
    public float Damage;
    public Vector3 HitDir;
    public Vector3 HitPoint;
    public float HitAngle;
    public bool Blocked;
    public Character Target;
    public float Knockback;
}