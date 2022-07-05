using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowHealthWorld : MonoBehaviour
{
    [SerializeField] Image health;
    private IDamagable target;
    private void Start()
    {
        target = GetComponentInParent<IDamagable>();
        if (target == null)
        {
            target = transform.parent.GetComponentInChildren<IDamagable>();
        }
    }
    private void OnGUI()
    {
        health.fillAmount = target.Health / target.MaxHealth;
    }
}
