using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    public float StartDelay = 3f;
    public float EndDelay = 5f;
    public CameraControl myCameraControl;
    public Text MessageText;
    public GameObject TankPrefab;
    public TankManager[] Tanks;
    public Transform[] MonsterGen;
    public Transform MonsPrefab;
    public Transform BossPrefab;
    public Transform BossGen;


    private int StageNumber = 1;
    private WaitForSeconds StartWait;
    private WaitForSeconds EndWait;
    private int MonsterMax = 2;
    private int MonsterNum = 0;
    private bool Generating = false;
    private bool NextStage = false;



    private void Start()
    {
        StartWait = new WaitForSeconds(StartDelay);
        EndWait = new WaitForSeconds(EndDelay);

        SpawnAllTanks();
        SetCameraTargets();
        EnableTankControl();
        //ResetAllTanks();
        //myCameraControl.SetStartPositionAndSize();
        InvokeRepeating("GenerateBoss", 10f, 30f);
        StartCoroutine(GameLoop());
    }
    private void Update()
    {
        if (GameObject.Find("Tower") == null || NoTankLeft())
        {
            SceneManager.LoadScene(3);
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
        Mon.parent = BossGen.transform;
        Mon.transform.localPosition = Vector3.zero;
    }
}