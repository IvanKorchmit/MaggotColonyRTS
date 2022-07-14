using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicAI : UnitAI,IHealer
{
    [SerializeField] float healHealth = 10;

    [SerializeField] protected float currentAngle;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected LayerMask explosionMask;
    [SerializeField] protected AudioEvent explosion;
    [SerializeField] protected GameObject explosionParticle;

    protected bool isHealed(Transform target)
    {
        return true;
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

}
