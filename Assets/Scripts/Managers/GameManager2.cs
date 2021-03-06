﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager2 : MonoBehaviour
{
    public float StartDelay = 3f;
    public float EndDelay = 5f;
    public CameraControl2 myCameraControl;
    public Text MessageText;
    public GameObject TankPrefab;
    public TankManager[] Tanks;
    public Transform[] MonsterGen;
    public Transform[] ToolGen;
    public Transform MonsPrefab;
    public Transform BossPrefab;
    public Transform BossGen;
    public Transform Bomb;
    public Transform Tool;
    static public List<Transform> MonsterList = new List<Transform>();
    static public List<Transform> BossList = new List<Transform>();
    static public int MonsterKills = 0;
    static public int BossKills = 0;
    static public int skill1 = 2;
    static public int skill2 = 2;


    private int StageNumber = 1;
    private WaitForSeconds StartWait;
    private WaitForSeconds EndWait;
    private int MonsterMax = 2;
    private int MonsterNum = 0;
    private bool Generating = false;
    private bool NextStage = false;
    private Transform tool1;
    private Transform tool2;



    private void Start()
    {
        StartWait = new WaitForSeconds(StartDelay);
        EndWait = new WaitForSeconds(EndDelay);

        SpawnAllTanks();
        SetCameraTargets();
        EnableTankControl();
        MonsterKills = 0;
        BossKills = 0;
        //ResetAllTanks();
        //myCameraControl.SetStartPositionAndSize();
        InvokeRepeating("GenerateBoss", 10f, 30f);
        StartCoroutine(GameLoop());
    }
    private void Update()
    {
        if (tool1 == null)
            InvokeRepeating("Generatetool1", 60f, 30f);
        if (tool2 == null)
            InvokeRepeating("Generatetool2", 60f, 30f);
        if (GameObject.Find("Tower") == null || NoTankLeft())
        {
            CancelInvoke();
            foreach (Transform target in MonsterList)
            {
                if (target != null)
                    Destroy(target.gameObject);
            }
            MonsterList.Clear();
            foreach (Transform target in BossList)
            {
                if (target != null)
                    Destroy(target.gameObject);
            }
            BossList.Clear();
            SceneManager.LoadScene(3);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            GameObject tower = GameObject.Find("Tower");
            if (skill1 <= 0)
                return;
            for(int i=0; i<100; ++i)
            {
                Debug.Log("Bomb");
                float x = Random.Range(-80, 80);
                float z = Random.Range(-80, 80);
                Transform bomb = Instantiate(Bomb);
                bomb.parent = tower.transform;
                bomb.transform.localPosition = new Vector3(x, 0, z);
            }
            foreach(Transform target in MonsterList)
            {
                if(target != null)
                {
                    Destroy(target.gameObject);
                    MonsterKills++;
                }
                    
            }
            MonsterList.Clear();   
            foreach(Transform target in BossList)
            {
                if (target != null)
                    target.gameObject.GetComponent<BossHealth>().TakeDamage(600f);       
            }
            skill1--;
            GameObject.Find("SkillText1").GetComponent<Text>().text = "" + skill1;
        }
        if (Input.GetKeyUp(KeyCode.RightControl))
        {
            GameObject tower = GameObject.Find("Tower");
            if (skill2 <= 0)
                return;
            for (int i = 0; i < 100; ++i)
            {
                Debug.Log("Bomb");
                float x = Random.Range(-60, 60);
                float z = Random.Range(-60, 60);
                Transform bomb = Instantiate(Bomb);
                bomb.parent = tower.transform;
                bomb.transform.localPosition = new Vector3(x, 0, z);
            }
            foreach (Transform target in MonsterList)
            {
                if (target != null)
                {
                    Destroy(target.gameObject);
                    MonsterKills++;
                }

            }
            MonsterList.Clear();
            foreach (Transform target in BossList)
            {
                if (target != null)
                    target.gameObject.GetComponent<BossHealth>().TakeDamage(600f);
            }
            skill2--;
            GameObject.Find("SkillText2").GetComponent<Text>().text = "" + skill2;
        }
    }

    private void SpawnAllTanks()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            Tanks[i].Instance =
                Instantiate(TankPrefab, Tanks[i].SpawnPoint.position, Tanks[i].SpawnPoint.rotation) as GameObject;
            Tanks[i].PlayerNumber = i + 1;
            Tanks[i].Setup();
        }
    }
    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = Tanks[i].Instance.transform;
        }
        myCameraControl.Tanks = targets;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(StageStarting());
        yield return StartCoroutine(StagePlaying());
        yield return StartCoroutine(StageEnding());
        StartCoroutine(GameLoop());
    }


    private IEnumerator StageStarting()
    {
        MessageText.text = "Stage" + StageNumber;
        ++StageNumber;
        MonsterNum = 0;
        Generating = false;
        NextStage = false;
        yield return StartWait;
    }

    private IEnumerator StagePlaying()
    {   
        MessageText.text = "";
        while (!NextStage)
        {
            if (!Generating)
            {
                Generating = true;
                InvokeRepeating("GenerateMonster", 2f, 4f);
            }
            yield return null;
        }     
    }
    private IEnumerator StageEnding()
    {
        MonsterMax += 1;
        MessageText.text = "Next Wave !";
        yield return EndWait;
    }
    private bool NoTankLeft()
    {
        int numTanksLeft = 0;
        for (int i = 0; i < Tanks.Length; i++)
        {
            if (Tanks[i].Instance.activeSelf)
                numTanksLeft++;
        }
        return numTanksLeft == 0;
    }

    private void EnableTankControl()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            Tanks[i].EnableControl();
        }
    }
    private void GenerateMonster()
    {
        for (int i = 0; i < 4; ++i)
        {
            Transform Mon = Instantiate(MonsPrefab);
            MonsterList.Add(Mon);
            Mon.parent = MonsterGen[i].transform;
            Mon.transform.localPosition = Vector3.zero;
        }
        ++MonsterNum;
        Debug.Log(MonsterNum);
        if (MonsterNum >= MonsterMax)
        {
            CancelInvoke("GenerateMonster");
            NextStage = true;
        }
    }
    private void GenerateBoss()
    {
        Transform Mon = Instantiate(BossPrefab);
        BossList.Add(Mon);
        Mon.parent = BossGen.transform;
        Mon.transform.localPosition = Vector3.zero;
    }
    private void Generatetool1()
    {
        tool1 = Instantiate(Tool);
        tool1.parent = ToolGen[0].transform;
        tool1.transform.localPosition = new Vector3(0, 1, 0);
        CancelInvoke("Generatetool1");
    }
    private void Generatetool2()
    {
        tool2 = Instantiate(Tool);
        tool2.parent = ToolGen[1].transform;
        tool2.transform.localPosition = new Vector3(0, 1, 0);
        CancelInvoke("Generatetool2");
    }
}