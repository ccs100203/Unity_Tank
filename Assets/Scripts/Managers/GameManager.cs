using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int NumRoundsToWin = 5;        
    public float StartDelay = 3f;         
    public float EndDelay = 3f;           
    public CameraControl myCameraControl;   
    public Text MessageText;              
    public GameObject TankPrefab;         
    public TankManager[] Tanks;
    public Transform ZoomPrefab;

    private int RoundNumber;              
    private WaitForSeconds StartWait;     
    private WaitForSeconds EndWait;       
    private TankManager RoundWinner;
    private TankManager GameWinner;
    private Transform Zoom;
    private bool IsCanceled = false;


    private void Start()
    {
        StartWait = new WaitForSeconds(StartDelay);
        EndWait = new WaitForSeconds(EndDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
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
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();
        myCameraControl.SetStartPositionAndSize();
        ++RoundNumber;
        MessageText.text = "ROUND " + RoundNumber;
        SetPoisonArea();
        IsCanceled = false;
        ForceFinish = false;
        yield return StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();
        MessageText.text = "";
        while (!OneTankLeft())
        {
            yield return null;
        }
        
    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        RoundWinner = null;
        RoundWinner = GetRoundWinner();
        if (RoundWinner != null)
            RoundWinner.Wins++;

        GameWinner = GetGameWinner();

        string message = EndMessage();
        MessageText.text = message;
        if (!IsCanceled)
        {
            CancelInvoke("ReduceRange");
            
        }
        IsCanceled = true;
        Destroy(Zoom.gameObject);
        //Zoom.gameObject.SetActive(false);
        yield return EndWait;
    }
    private void SetPoisonArea()
    {
        Zoom = Instantiate(ZoomPrefab);
        int n;
        n = Random.Range(0, 5);
        if (n == 0) Zoom.transform.position = new Vector3(15, 0, 10);
        else if (n == 1) Zoom.transform.position = new Vector3(35, 0, 30);
        else if (n == 2) Zoom.transform.position = new Vector3(30, 0, -30);
        else if (n == 3) Zoom.transform.position = new Vector3(-30, 0, 30);
        else if (n == 4) Zoom.transform.position = new Vector3(-30, 0, -30);

        //Zoom.transform.localScale = new Vector3(300, 40, 300);
        InvokeRepeating("ReduceRange", .1f, 0.25f);

        //Debug.Log("QQQ "+gameObject.GetComponent<Renderer>().material.color.a);
        Zoom.GetComponent<Renderer>().material.color = new Color(0.1f, 0.6f, 0.8f, 0.1f);
    }
    private void ReduceRange()
    {
        Zoom.transform.localScale = Zoom.transform.localScale - Vector3.forward - Vector3.right;
    }
    static public bool ForceFinish = false;
    void Update()
    {
        if (!IsCanceled && Zoom.localScale.x <= 0)
        {
            CancelInvoke("ReduceRange");
            IsCanceled = true;
            ForceFinish = true;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

    }

    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < Tanks.Length; i++)
        {
            if (Tanks[i].Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            if (Tanks[i].Instance.activeSelf)
                return Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            if (Tanks[i].Wins == NumRoundsToWin)
                return Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (RoundWinner != null)
            message = RoundWinner.ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < Tanks.Length; i++)
        {
            message += Tanks[i].ColoredPlayerText + ": " + Tanks[i].Wins + " WINS\n";
        }

        if (GameWinner != null)
            message = GameWinner.ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < Tanks.Length; i++)
        {
            Tanks[i].DisableControl();
        }
    }
}