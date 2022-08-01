using UnityEngine;

public struct DamageTakenEvent
{
    public UnityEngine.Object Source;
    public DamageList Damage;
    public Vector3 HitDir;
    public Vector3 HitPoint;
    public float HitAngle;
    public bool Blocked;
    public Character Dealer;
    public float Knockback;
}
