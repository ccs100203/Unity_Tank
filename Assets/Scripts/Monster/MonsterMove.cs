﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]

public class MonsterMove : MonoBehaviour
{

    public float speed;

    private GameObject target;
    //private GameObject tower;
    private GameObject player;
    private Animator animator;
    private Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        speed = 5;
        target = GameObject.Find("Tower");
        
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        targetPos = target.transform.position;
        transform.LookAt(targetPos);
        move();
        if ((targetPos - transform.position).magnitude <= 13)
        {
            animator.SetBool("NearTarget", true);
            return;
        }
        animator.SetBool("NearTarget", false);

    }
    void move()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
