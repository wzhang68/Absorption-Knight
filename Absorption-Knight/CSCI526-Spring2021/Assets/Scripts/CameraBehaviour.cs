using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehaviour : MonoBehaviour
{
    private float devHeight = 6.4f;

    private float devWidth = 9.6f;
    // Start is called before the first frame update
    void Start()
    {
        float screenHeight = Screen.height;
        Debug.Log("Current Screen Height = "+ screenHeight);
        float screenWidth = Screen.width;
        Debug.Log("Current Screen Width = "+ screenWidth);
        float orthographicSize = this.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        float cameraHeight = orthographicSize * 2 * aspectRatio;
        float cameraWidth = orthographicSize * 2 * aspectRatio;
        Debug.Log("CameraWidth = " + cameraWidth);
        if (cameraWidth < devWidth)
        {
            orthographicSize = devWidth / (2 * aspectRatio);
            this.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = orthographicSize;
        }
        if (cameraHeight < devHeight)
        {
            orthographicSize = devHeight / (2 * aspectRatio);
            this.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = orthographicSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
