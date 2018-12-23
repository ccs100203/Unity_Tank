using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]

public class BossMove : MonoBehaviour
{

    public float speed = 5f;

    private GameObject target;
    private GameObject player;
    private Animator animator;
    private Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        target = GameObject.Find("Tank2(Clone)");
        Debug.Log(target);
        targetPos = target.transform.position;
        transform.LookAt(targetPos);
        move();
        if ((targetPos - transform.position).magnitude <= 7)
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
