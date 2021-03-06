﻿using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int PlayerNums = 1;         
    public float Speed = 12f;            
    public float TurnSpeed = 180f;       
    public AudioSource MovementAudio;    
    public AudioClip EngineIdling;       
    public AudioClip EngineDriving;      
    public float PitchRange = 0.2f;

    
    private string MovementAxisName;     
    private string TurnAxisName;         
    private Rigidbody rb;         
    private float MovementInputValue;    
    private float TurnInputValue;        
    private float OriginalPitch;         


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        rb.isKinematic = false;
        MovementInputValue = 0f;
        TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        rb.isKinematic = true;
    }


    private void Start()
    {
        MovementAxisName = "Vertical" + PlayerNums;
        TurnAxisName = "Horizontal" + PlayerNums;

        OriginalPitch = MovementAudio.pitch;
    }
    

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        MovementInputValue = Input.GetAxis(MovementAxisName);
        TurnInputValue = Input.GetAxis(TurnAxisName);

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if(Mathf.Abs(MovementInputValue) < 0.1f && Mathf.Abs(TurnInputValue) < 0.1f)
        {
            if(MovementAudio.clip == EngineDriving)
            {
                MovementAudio.clip = EngineIdling;
                MovementAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                MovementAudio.Play();
            }
        }
        else
        {
            if (MovementAudio.clip == EngineIdling)
            {
                MovementAudio.clip = EngineDriving;
                MovementAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * MovementInputValue * Speed * Time.deltaTime;

        rb.MovePosition(rb.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = TurnInputValue * TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}