﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

    public Transform MonsPrefab;
	// Use this for initialization
	void Start () {
        InvokeRepeating("GenerateMonster", 2f, interval);
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    void GenerateMonster()
    {
        Transform Mon = Instantiate(MonsPrefab);
        Mon.parent = transform;
        Mon.transform.localPosition = Vector3.zero;
        interval -= 0.5f;
    }
    private float interval = 2;
}