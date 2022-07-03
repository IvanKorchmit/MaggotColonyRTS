using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ArtilleryAI : TankAI
{
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private bool isReady;
    [SerializeField] private Vector2? point = null;
    /// <summary>
    /// Invoked via context menu
    /// </summary>
    public void Prepare()
    {
        TimerUtils.AddTimer(0.5f, () => isReady = true);
        move.canMove = false;

    }
    /// <summary>
    /// Invoked on click
    /// </summary>
    public void Launch()
    {
        if (isReady)
        {
            point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
    }
    private void Shoot(Vector2 position)
    {
        if (Vector2.Distance(position, transform.position) <= range.Radius)
        {
            explosion.Play(audioSource);
            var pos = position + Random.insideUnitCircle * 10f;
            var coll = Physics2D.OverlapCircleAll(pos, 10,explosionMask);
            Instantiate(explosionParticle, pos, Quaternion.identity);
            foreach (var item in coll)
            {
                if (item.TryGetComponent(out IDamagable damagable))
                {
                    damagable.Damage(damage, this);
                }
            }
        }
    }
    protected override void Update()
    {
        if (isReady && point != null)
        {
            float a = Rotate(point.Value);
            headRotation.SetAngle((int)currentAngle);
            if (AttackOnRotation(a))
            {
                Shoot(point.Value);
                move.canMove = true;
                isReady = false;
                point = null;
            }
        }
    }
}
