using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color PlayerColor;            
    public Transform SpawnPoint;         
    [HideInInspector] public int PlayerNumber;             
    [HideInInspector] public string ColoredPlayerText;
    [HideInInspector] public GameObject Instance;          
    [HideInInspector] public int Wins;                     


    private TankMovement Movement;       
    private TankShooting Shooting;
    private GameObject CanvasGameObject;


    public void Setup()
    {
        Movement = Instance.GetComponent<TankMovement>();
        Shooting = Instance.GetComponent<TankShooting>();
        CanvasGameObject = Instance.GetComponentInChildren<Canvas>().gameObject;

        Movement.PlayerNums = PlayerNumber;
        Shooting.PlayerNumber = PlayerNumber;

        ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerColor) + ">PLAYER " + PlayerNumber + "</color>";

        MeshRenderer[] renderers = Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = PlayerColor;
        }
    }


    public void DisableControl()
    {
        Movement.enabled = false;
        Shooting.enabled = false;

        CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        Movement.enabled = true;
        Shooting.enabled = true;

        CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        Instance.transform.position = SpawnPoint.position;
        Instance.transform.rotation = SpawnPoint.rotation;

        Instance.SetActive(false);
        Instance.SetActive(true);
    }
}
