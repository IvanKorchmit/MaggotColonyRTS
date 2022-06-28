using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour, IAttackable, IDamagable
{
    [SerializeField] private GameObject bug;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0, Random.Range(3f, 5f));
    }

    private void Spawn()
    {
        Instantiate(bug, transform.position, Quaternion.identity);
    }

    public void Damage(float damage, IDamagable owner)
    {
        throw new System.NotImplementedException();
    }
}
