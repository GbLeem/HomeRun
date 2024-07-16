using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera catcherCamera;
    public Camera sideCamera;
    public Camera topCamera;

    // Start is called before the first frame update
    void Start()
    {
        catcherCamera.gameObject.SetActive(true);
        sideCamera.gameObject.SetActive(false);
        topCamera.gameObject.SetActive(false);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToCatcherCamera();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToSideCamera();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToTopCamera();
        }
    }

    void SwitchToCatcherCamera()
    {
        catcherCamera.gameObject.SetActive(true);
        sideCamera.gameObject.SetActive(false);
        topCamera.gameObject.SetActive(false);
    }

    void SwitchToSideCamera()
    {
        catcherCamera.gameObject.SetActive(false);
        sideCamera.gameObject.SetActive(true);
        topCamera.gameObject.SetActive(false);
    }
    void SwitchToTopCamera()
    {
        catcherCamera.gameObject.SetActive(false);
        sideCamera.gameObject.SetActive(false);
        topCamera.gameObject.SetActive(true);
    }
}
