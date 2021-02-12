using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //when the player hits the collider it loads next scene
        SceneManager.LoadScene("Track2");
    }
}
