using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private int oldAngle;
    private int angle;
    public void SetAngle(int angle)
    {
        this.angle = Mathf.RoundToInt(angle / 45f) * 45;
        if (this.angle < 0) this.angle += 360;
    }
    private void OnAngleChange()
    {
        animator.SetInteger("Angle", angle);
        animator.SetTrigger("ChangeAngle");
    }
    public void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }
    private void Update()
    {
        if (oldAngle != angle)
        {
            OnAngleChange();
            oldAngle = angle;
        }
    }
}
