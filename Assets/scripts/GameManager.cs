using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class GameManager : MonoBehaviour {

    public GameObject vrSphere;

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (VRSettings.enabled)
            {
                VRSettings.enabled = false;
                vrSphere.SetActive(false);
            }
            else
            {
                Application.Quit();
            }
        }
	}
}
