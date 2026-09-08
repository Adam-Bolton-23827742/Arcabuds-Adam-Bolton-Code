using UnityEngine;

public class CamersSetUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<CameraSwitch>().SwitchCam();
    }
}
