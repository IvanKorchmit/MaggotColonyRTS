using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour, IAttackable, IDamagable
{
    [SerializeField] private GameObject bug;
    [SerializeField] private float health = 300;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 6f, Random.Range(50f, 60f));
    }

    private void Spawn()
    {
        Instantiate(bug, transform.position, Quaternion.identity);
    }

    public void Damage(float damage, IDamagable owner)
    {
        health -= damage;
        if (Random.value >= 0.5f)
        {
            // Trigger spawning enemies on attack
            for (int i = 0; i < (int)damage / 2; i++)
            {
                Spawn();
            }
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
