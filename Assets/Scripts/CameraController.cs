using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour
{
    Camera cam;
    public GameObject[] soundObjs;
    public GameObject map;
    public GameObject crosshair;

    void Start()
    {
        //get camera and mvt stuff
        cam = GetComponent<Camera>();
        map = GameObject.FindGameObjectWithTag("map");
        crosshair = GameObject.FindGameObjectWithTag("crosshair");
    }

    void Update()
    {
    }

    //public bool OnScreen(GameObject sound)
    public bool OnScreen(Vector3 soundPos)
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(soundPos);
        bool inCamView = (viewportPoint.z > 0) && (viewportPoint.x > 0) && (viewportPoint.x < 1) && (viewportPoint.y > 0) && (viewportPoint.y < 1);
        return inCamView;
    }

    public bool IsSelected(GameObject soundObj)
    {
        Vector3 chPos = crosshair.transform.position;
        Vector3 soPos = soundObj.transform.position;
        Vector3 objSize = soundObj.GetComponent<Renderer>().bounds.size;
        float bound = objSize.x / 2;
        bool inBound = (chPos.x < soPos.x + bound) && (chPos.x > soPos.x - bound) && (chPos.y < soPos.y + bound) && (chPos.y > soPos.y - bound);
        if (inBound)
            return true;
        else
            return false;
    }
}