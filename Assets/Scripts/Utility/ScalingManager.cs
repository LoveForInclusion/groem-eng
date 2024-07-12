using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalingManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        this.transform.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

    }
}