using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : TankAI
{
    [SerializeField] private ConstructionMenu constructionPreset;
    [SerializeField] private SpriteRotation spriteRotation;
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
        if (range.ClosestTarget.TryGetComponent(out IAttackable attackable))
        {
            attackable.Damage(3, this);
        }
    }
    protected override void Update()
    {
        if (range.ClosestTarget != null && range.ClosestTarget.IsAlive())
        {
            float angle = Rotate();
            if (AttackOnRotation(angle))
            {
                TimerUtils.AddTimer(0.5f, Attack);
            }
        }
        spriteRotation.SetAngle((int)currentAngle);
    }
}
