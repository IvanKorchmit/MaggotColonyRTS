using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour, IAttackable, IDamagable, IHoverable
{

    [SerializeField] private GameObject displayHealth;
    public static int currentBugs;
    public const int BUGS_LIMIT = 50;







    [SerializeField] private GameObject bug;
    [SerializeField] private float health = 300;
    private float maxHealth;

    public static event Action<Egg> ServerOnBaseSpawned;
    public static event Action<Egg> ServerOnBaseDespawned;


    public void OnEnable()
    {
        //health.ServerOnDie += ServerHandleDie;

        ServerOnBaseSpawned?.Invoke(this);
    }

    public void OnDisable()
    {
        //health.ServerOnDie -= ServerHandleDie;

        ServerOnBaseDespawned?.Invoke(this);
    }

    public void OnHover()
    {
        displayHealth.SetActive(true);
    }

    public void OnUnHover()
    {
        displayHealth.SetActive(false);
    }



    public float Health => health;

    public float MaxHealth => maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnCycle), 60 * 5, UnityEngine.Random.Range(60,90));
        maxHealth = health;
    }

    private void Spawn(bool ignoreLimit = false)
    {
        if (currentBugs >= BUGS_LIMIT && !ignoreLimit) return;
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
        if (UnityEngine.Random.value >= 0.9f)
        {
            // Trigger spawning enemies on attack
            for (int i = 0; i < ((int)damage == 0 ? 1 : (int)damage / 2); i++)
            {
                Spawn(true);
            }
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
