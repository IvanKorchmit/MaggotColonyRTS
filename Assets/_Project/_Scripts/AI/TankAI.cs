using UnityEngine;

public class TankAI : UnitAI
{
    [SerializeField] private SpriteRotation headRotation;
    [SerializeField] protected float currentAngle;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] private LayerMask explosionMask;
    [SerializeField] private AudioEvent explosion;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] private GameObject explosionParticle;
    protected override void Attack()
    {
        Instantiate(explosionParticle, range.ClosestTarget.position, Quaternion.identity);
        explosion.Play(audioSource);
        var coll = Physics2D.CircleCastAll(range.ClosestTarget.position, 4f, new Vector2(), 0, explosionMask);
        foreach (var en in coll)
        {
            en.collider.GetComponent<IDamagable>().Damage(10, this);
        }
    }
    protected override void Update()
    {
        if (range.ClosestTarget != null && range.ClosestTarget.IsAlive())
        {
            float angle = Rotate(range.ClosestTarget);
            if (AttackOnRotation(angle))
            {
                TimerUtils.AddTimer(3f, Attack);
                move.canMove = false;
            }
        }
        else
        {
            move.canMove = true;
        }
        headRotation.SetAngle((int)currentAngle);
    }

    protected float Rotate(Transform target)
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, angle, Time.deltaTime * rotationSpeed);
        return angle;
    }

    protected bool AttackOnRotation(float angle)
    {
        return Mathf.RoundToInt(angle) == Mathf.RoundToInt(currentAngle);
    }
}
