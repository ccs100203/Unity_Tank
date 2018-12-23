using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenutoDual : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData e)
    {
        SceneManager.LoadScene(1);
    }
}
