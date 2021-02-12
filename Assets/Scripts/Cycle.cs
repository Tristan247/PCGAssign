using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cycle : MonoBehaviour
{
    void Start()
    {
        //x-axis rotates the directional light which creates a day and night cycle
        GetComponent<Rigidbody>().angularVelocity = new Vector3(.2f, 0, 0);
    }
}
