using UnityEngine;

public class ShellExplosion : MonoBehaviour
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
        for(int i=0; i<c.Length; ++i)
        {
            Rigidbody target = c[i].GetComponent<Rigidbody>();
            if (!target)
                continue;

            target.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);

            TankHealth health = target.GetComponent<TankHealth>();
            if (!health)
                continue;

            float damage = CalculateDamage(target.position);
            health.TakeDamage(damage);
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