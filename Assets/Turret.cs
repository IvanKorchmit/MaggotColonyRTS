using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : TankAI
{
    [SerializeField] private AudioEvent shoot;
    [SerializeField] private AudioEvent detection;
    [SerializeField] private ConstructionMenu constructionPreset;
    [SerializeField] private SpriteRotation spriteRotation;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private float cooldown;
    [SerializeField] private float damage;
    private void OnEnable()
    {
        ConstructBehaviour cb = GetComponent<ConstructBehaviour>();
        foreach (var item in constructionPreset.options)
        {
            UnityEngine.Events.UnityEvent e = new UnityEngine.Events.UnityEvent();
            e.AddListener(() => cb.ConstructBuilding(item.building));
            contextMenu.Add(item.option, e);
        }
    }
    protected override void Attack()
    {
        if (range.ClosestTarget == null || !range.ClosestTarget.IsAlive()) return;
        shoot.Play(audioSource);
        if (range.ClosestTarget.TryGetComponent(out IAttackable attackable))
        {
            attackable.Damage(damage, this);
        }
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
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, (currentAngle + 45f), Time.deltaTime * rotationSpeed);
        }
        currentAngle %= 360;
        spriteRotation.SetAngle((int)currentAngle);
    }
}