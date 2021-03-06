﻿using UnityEngine;

public class ShellExplosion2 : MonoBehaviour
{
    public LayerMask TankMask;
    public ParticleSystem ExplosionParticles;
    public AudioSource ExplosionAudio;
    public float MaxDamage = 100f;
    public float ExplosionForce = 1000f;
    public float MaxLifeTime = 2f;
    public float ExplosionRadius = 5f;


    private void Start()
    {
        Destroy(gameObject, MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] c = Physics.OverlapSphere(transform.position, ExplosionRadius, TankMask);
        for (int i = 0; i < c.Length; ++i)
        {
            Rigidbody target = c[i].GetComponent<Rigidbody>();
            if (!target)
                continue;

            target.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);

            //TankHealth healthPlayer = target.GetComponent<TankHealth>();
            MonsterHealth healthMonster = target.GetComponent<MonsterHealth>();
            BossHealth healthBoss = target.GetComponent<BossHealth>();
            //if (healthPlayer)
            //{
            //    float damage = CalculateDamage(target.position);
            //    healthPlayer.TakeDamage(damage);
            //}
            if (healthMonster)
            {
                float damage = CalculateDamage(target.position);
                healthMonster.TakeDamage(damage);
            }
            if (healthBoss)
            {
                float damage = CalculateDamage(target.position);
                healthBoss.TakeDamage(damage);
            }
        }
        ExplosionParticles.transform.parent = null;
        ExplosionParticles.Play();
        ExplosionAudio.Play();
        Destroy(ExplosionParticles.gameObject, ExplosionParticles.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;
        float Distance = explosionToTarget.magnitude;
        float relativeDistance = (ExplosionRadius - Distance) / ExplosionRadius;
        float damage = MaxDamage * relativeDistance;

        damage = Mathf.Max(0f, damage);

        return damage;
    }
}