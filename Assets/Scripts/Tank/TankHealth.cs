using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float StartingHealth = 100f;          
    public Slider HealthBar;                        
    public Image FillImage;                      
    public Color FullHealthColor = Color.green;  
    public Color ZeroHealthColor = Color.red;    
    public GameObject ExplosionPrefab;
    
    
    private AudioSource ExplosionAudio;          
    private ParticleSystem ExplosionParticles;   
    private float CurrentHealth;  
    private bool Dead;            


    private void Awake()
    {
        ExplosionParticles = Instantiate(ExplosionPrefab).GetComponent<ParticleSystem>();
        ExplosionAudio = ExplosionParticles.GetComponent<AudioSource>();

        ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        CurrentHealth = StartingHealth;
        Dead = false;

        SetHealthUI();

        //InvokeRepeating("Toxin", 1f, 1f);
    }
    

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        CurrentHealth -= amount;
        SetHealthUI();

        if(CurrentHealth <= 0f && !Dead)
        {
            OnDeath();
        }
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        HealthBar.value = CurrentHealth;
        FillImage.color = Color.Lerp(ZeroHealthColor, FullHealthColor, CurrentHealth / StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        Dead = true;
        ExplosionParticles.transform.position = transform.position;
        ExplosionParticles.gameObject.SetActive(true);
        ExplosionParticles.Play();
        ExplosionAudio.Play();
        gameObject.SetActive(false);
    }
    void OnCollisionEnter(Collision c)
    {
        //Debug.Log(c.transform.name);
        if (c.transform.name == "Skeleton(Clone)")
        {
            TakeDamage(3f);
            Rigidbody rb1 = GetComponent<Rigidbody>();
            Rigidbody rb2 = c.gameObject.GetComponent<Rigidbody>();
            rb1.AddForce(-500 * transform.forward);
            rb2.AddForce(500 * transform.forward);
        }
        if (c.transform.name == "Boss(Clone)")
        {
            TakeDamage(10f);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(1000 * c.transform.forward);
        }
    }

    /*void OnTriggerStay(Collider c)
    {
        if (c.gameObject.name == "Zoom(Clone)")
        {
            Debug.Log(c.gameObject.name + " Stay");
            Inside = true;
        }
    }*/
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "Zoom(Clone)")
        {
            Debug.Log(c.gameObject.name + " Enter");
            //Inside = true;
            CancelInvoke("Toxin");
        }
    }
    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.name == "Zoom(Clone)")
        {
            Debug.Log(c.gameObject.name + " Exit");
            Inside = false;
            InvokeRepeating("Toxin", 1f, 1f);
        }
    }

    private void Toxin()
    {
     //   if (!Inside)
            TakeDamage(3f);
    }
    void Update()
    {
        if (GameManager.ForceFinish) 
            TakeDamage(0.1f);
    }
    private bool Inside = true;
}