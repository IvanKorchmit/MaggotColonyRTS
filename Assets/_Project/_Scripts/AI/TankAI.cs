using UnityEngine;

public class TankAI : UnitAI
{
    [SerializeField] protected SpriteRotation headRotation;
    [SerializeField] protected float currentAngle;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected LayerMask explosionMask;
    [SerializeField] protected AudioEvent explosion;
    [SerializeField] protected GameObject explosionParticle;
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
        if (order != null && order is OrderBase.AttackOrder attack)
        {
            if (attack.target != null && attack.target.IsAlive() && range.HasTarget(attack.target.transform))
            {
                float angle = Rotate(range.ClosestTarget);
                if (AttackOnRotation(angle))
                {
                    TimerUtils.AddTimer(3f, Attack);
                    move.canMove = false;
                }
            }
            else if (attack.target != null && attack.target.IsAlive() && !range.HasTarget(attack.target.transform))
            {
                seeker.StartPath(transform.position, attack.target.transform.position);
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
        currentAngle %= 360f;
        return angle;
    }
    protected float Rotate(Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, angle, Time.deltaTime * rotationSpeed);
        currentAngle %= 360f;

        return angle;
    }
    protected bool AttackOnRotation(float angle)
    {
        return Mathf.RoundToInt(angle) == Mathf.RoundToInt(currentAngle);
    }
}
