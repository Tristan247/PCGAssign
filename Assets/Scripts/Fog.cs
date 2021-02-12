using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    private void Start()
    {
        FogEnable();
    }
    void FogEnable()
    {
        //enables fog in the scene
        RenderSettings.fog = true;
    }
}
