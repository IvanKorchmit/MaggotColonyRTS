using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour, IAttackable, IDamagable, IHoverable
{

    [SerializeField] private GameObject displayHealth;


    public void OnHover()
    {
        displayHealth.SetActive(true);
    }

    public void OnUnHover()
    {
        displayHealth.SetActive(false);
    }








    [SerializeField] private GameObject bug;
    [SerializeField] private float health = 300;
    private float maxHealth;
    public float Health => health;

    public float MaxHealth => maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnCycle), 60 * 5, Random.Range(60,90));
        maxHealth = health;
    }

    private void Spawn()
    {
        Instantiate(bug, transform.position, Quaternion.identity);
    }
    private void SpawnCycle()
    {
        for (int i = 0; i < 5; i++)
        {
            Spawn();
        }
    }
    public void Damage(float damage, IDamagable owner)
    {
        health -= damage;
        if (Random.value >= 0.5f)
        {
            // Trigger spawning enemies on attack
            for (int i = 0; i < ((int)damage == 0 ? 1 : (int)damage / 2); i++)
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
