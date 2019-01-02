using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverdata : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = "Monster kills: " + GameManager2.MonsterKills + "\r\nBoss kills: " + GameManager2.BossKills;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
