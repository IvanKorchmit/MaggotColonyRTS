using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : TankAI
{
    [SerializeField] private AudioEvent shoot;
    [SerializeField] private AudioEvent detection;
    [SerializeField] private SpriteRotation spriteRotation;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private float cooldown;
    [SerializeField] private float damage;
    [SerializeField] private int direction;
    protected override void Attack()
    {
        if (range.ClosestTarget == null || !range.ClosestTarget.IsAlive()) return;
        shoot.Play(audioSource);
        if (range.ClosestTarget.TryGetComponent(out IAttackable attackable))
        {
            attackable.Damage(damage, this);
        }
    }
    private void Stop()
    {
        direction = 0;
    }
    private void SetDirection()
    {
        var random = new int[] { -1, 1};
        direction = random[Random.Range(0, random.Length)];
        Invoke(nameof(Stop), Random.Range(0.25f, 0.75f));
    }
    protected override void Update()
    {
        if (range.ClosestTarget != null && range.ClosestTarget.IsAlive())
        {
            float angle = Rotate(currentTarget == null ||
                !currentTarget.IsAlive() ||
                !range.HasTarget(currentTarget) ?
                range.ClosestTarget :
                currentTarget);




            if (AttackOnRotation(angle))
            {
                TimerUtils.AddTimer(cooldown, Attack);
                if (currentTarget != range.ClosestTarget)
                {
                    detection.Play(audioSource);
                }

            }
            currentTarget = range.ClosestTarget;
        }
        else
        {
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, (currentAngle + 45f * direction), Time.deltaTime * rotationSpeed / 2);
        }
        currentAngle %= 360;
        spriteRotation.SetAngle((int)currentAngle);
    }
}