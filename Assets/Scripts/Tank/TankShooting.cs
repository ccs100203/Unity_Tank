using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int PlayerNumber = 1;       
    public Rigidbody Shell;            
    public Transform FireTransform;    
    public Slider Power;           
    public AudioSource ShootingAudio;  
    public AudioClip ChargingClip;     
    public AudioClip FireClip;         
    public float MinLaunchForce = 15f; 
    public float MaxLaunchForce = 30f; 
    public float MaxChargeTime = 0.75f;

    
    private string FireButton;         
    private float CurrentLaunchForce;  
    private float ChargeSpeed;         
    private bool Fired;                


    private void OnEnable()
    {
        CurrentLaunchForce = MinLaunchForce;
        Power.value = MinLaunchForce;
    }


    private void Start()
    {
        FireButton = "Fire" + PlayerNumber;

        ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / MaxChargeTime;
    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        Power.value = MinLaunchForce;
        if(CurrentLaunchForce >= MaxLaunchForce && !Fired)
        {
            // Not release the button before max power
            CurrentLaunchForce = MaxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(FireButton))
        {
            // First press down
            Fired = false;
            CurrentLaunchForce = MinLaunchForce;
            ShootingAudio.clip = ChargingClip;
            ShootingAudio.Play();
        }
        else if(Input.GetButton(FireButton) && !Fired)
        {
            // Holding fire button
            CurrentLaunchForce += ChargeSpeed * Time.deltaTime;
            Power.value = CurrentLaunchForce;
        }
        else if(Input.GetButtonUp(FireButton) && !Fired){
            // Release fire button
            Fire();
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        Fired = true;

        Rigidbody shell = Instantiate(Shell, FireTransform.position, FireTransform.rotation) as Rigidbody;
        shell.velocity = CurrentLaunchForce * FireTransform.forward;

        ShootingAudio.clip = FireClip;
        ShootingAudio.Play();

        CurrentLaunchForce = MinLaunchForce;
    }
}