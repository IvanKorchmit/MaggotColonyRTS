using UnityEngine;

public class TankAI : UnitAI
{
    [SerializeField] private SpriteRotation headRotation;
    [SerializeField] private float currentAngle;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private LayerMask explosionMask;
    [SerializeField] private AudioEvent explosion;
    [SerializeField] private AudioSource audioSource;
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
        if (target.IsAlive())
        {
            seeker.StartPath(transform.position, targetPosition);
            targetPosition = target.transform.position;
            if (range.ClosestTarget == target.transform)
            {
                move.canMove = false;
                TimerUtils.AddTimer(2, Attack);
            }
            else
            {
                move.canMove = true;
            }
        }
        if (range.ClosestTarget != null && range.ClosestTarget.IsAlive())
        {
            Vector2 direction = range.ClosestTarget.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, angle, Time.deltaTime * rotationSpeed);
            if (Mathf.RoundToInt(angle) == Mathf.RoundToInt(currentAngle))
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
}
