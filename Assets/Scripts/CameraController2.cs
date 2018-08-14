using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController2 : MonoBehaviour
{
    private float speed = 0.05f;//0.03F;
    Camera cam;
    public GameObject[] soundObjs;
    public GameObject map;
    float zOffset = -1F;
    float xOffset = 50f;// 6.25f;
    float yOffset = 50f;//11.5f;
    float MIN_Z, MAX_Z, MIN_X, MAX_X, MIN_Y, MAX_Y;
    bool[] canMove; //0=left, 1=up, 2=right, 3=down, 4=forward, 5=back

    public GameObject crosshair;
    GameObject gamePanel;
    GameObject mainMenu;

    float volIncrement = 0.1F;
    float defaultVol = 1f;

    GameObject startButton;

    void Start()
    {
        //get camera and mvt stuff
        cam = GetComponent<Camera>();
        map = (GameObject.FindGameObjectsWithTag("map"))[0];
        crosshair = (GameObject.FindGameObjectsWithTag("crosshair"))[0];//crosshair.transform.position = cam.transform.position;

        //menu stuff
        //gamePanel = (GameObject.FindGameObjectsWithTag("gamePanel"))[0];
        gamePanel = GameObject.FindGameObjectWithTag("gamePanel");
        mainMenu = GameObject.FindGameObjectWithTag("mainMenu");
        startButton = GameObject.FindGameObjectWithTag("startButton");

        //sound stuff
        soundObjs = GameObject.FindGameObjectsWithTag("sound");

        //make start button invisible at first
        startButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        startButton.GetComponentsInChildren<CanvasRenderer>()[1].SetAlpha(0);

        //set positions
        //cam.transform.position = new Vector3 (0, 5, -30);

        //create bounds array
        canMove = new bool[6];
        SetAllMoves(true);

        //get bounds and offsets
        xOffset = map.GetComponent<Collider2D>().bounds.size.x / 3f;
        yOffset = map.GetComponent<Collider2D>().bounds.size.y / 3f;
        Vector3 mapPos = map.transform.position;
        MIN_Z = transform.position.z; MAX_Z = mapPos.z + zOffset;
        //MIN_X = mapPos.x - xOffset; MAX_X = mapPos.x + xOffset;
        //MIN_Y = mapPos.y - yOffset; MAX_Y = mapPos.y + yOffset;

        //stuff for each obj
        foreach (GameObject soundObj in soundObjs)
        {
            //Vector3 p = soundObj.transform.position;
            //soundObj.transform.position = new Vector3 (p.x,p.y,map.transform.position.z);
            SetHalo(soundObj, false);
            soundObj.GetComponent<AudioSource>().volume = defaultVol;
        }
        SetSelectMenu(false);

        //set start button to fade in
        startButton.GetComponentsInChildren<CanvasRenderer>()[1].SetAlpha(0.25f);
        Invoke("ButtonFadeIn", 3.0f);
    }



    void Update()
    {
        ChangeVolText(defaultVol);
        UpdateCanMove();
        Vector3 dir = DirToMove();
        MoveInDir(dir);
        if (!mainMenu.activeInHierarchy) PlaySounds();
        //Debug.Log("camera pos = " + transform.position);
        SetAllMoves(true);
    }

    public bool GameStart(bool b)
    {
        return b;
    }

    public bool OnScreen(GameObject sound)
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(sound.transform.position);
        Vector3 camPoint = cam.WorldToViewportPoint(transform.position);
        bool inCamView = (viewportPoint.z > 0) && (viewportPoint.x > 0) && (viewportPoint.x < 1) && (viewportPoint.y) > 0 && (viewportPoint.y < 1);
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
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Plane[] GetClippingPlane()
    {
        return GeometryUtility.CalculateFrustumPlanes(cam);

    }


    /******************** CameraMovement ***************************** */

    void UpdateCanMove()
    {
        //Debug.Log("before check: can move left=" + canMove[0]);
        if (transform.position.z > MAX_Z) canMove[4] = false; //forward
        if (transform.position.z < MIN_Z) canMove[5] = false; //back
        //if (transform.position.x > MAX_X) canMove[2] = false; //right
        //if (transform.position.x < MIN_X) canMove[0] = false; //left
        //if (transform.position.y > MAX_Y) canMove[1] = false; //up
        //if (transform.position.y < MIN_Y) canMove[3] = false; //down
        //Debug.Log("after check: can move left=" + canMove[0]);
    }

    void SetAllMoves(bool b)
    {
        for (int i = 0; i < 6; i++)
            canMove[i] = b;
    }

    void MoveInDir(Vector3 dir)
    {
        transform.position = transform.position + speed * dir;
        crosshair.transform.position = crosshair.transform.position + speed * dir;
    }

    private bool DetKey(UnityEngine.KeyCode key)
    {
        return Input.GetKey(key);
    }

    Vector3 DirToMove()
    {
        Vector3 dir = Vector3.zero;
        if (DetKey(KeyCode.LeftArrow) && canMove[0])
            dir += Vector3.left;
        else if (DetKey(KeyCode.RightArrow) && canMove[2])
            dir += Vector3.right;
        if (DetKey(KeyCode.UpArrow) && canMove[1])
            dir += Vector3.up;
        else if (DetKey(KeyCode.DownArrow) && canMove[3])
            dir += Vector3.down;
        if (DetKey(KeyCode.Z) && canMove[5])
            dir += Vector3.back;
        //NOTE - backward = movement in -z direction
        else if (DetKey(KeyCode.X) && canMove[4])
            dir += Vector3.forward;
        //+ z dir
        return dir;
    }




    /******************** MenuController ***************************** */
    void ButtonFadeIn()
    {
        startButton.GetComponentsInChildren<CanvasRenderer>()[1].SetAlpha(0.5f);
        startButton.GetComponent<Image>().CrossFadeAlpha(1, 6.0f, false);
        startButton.GetComponentsInChildren<CanvasRenderer>()[1].SetAlpha(1f);
    }

    public void SetSelectMenu(bool b)
    {
        GameObject textArea = gamePanel.transform.GetChild(0).gameObject;
        GameObject instr = gamePanel.transform.GetChild(1).gameObject;
        textArea.SetActive(b);
        instr.SetActive(!b);
    }

    public void ChangeVolText(float v)
    {
        GameObject vol = gamePanel.transform.GetChild(0).GetChild(0).gameObject;
        string volInstr = "( S ) to toggle volume :    ";
        if (v > 0) vol.GetComponent<Text>().text = volInstr + "ON / ___";
        else vol.GetComponent<Text>().text = volInstr + "__ / OFF";
    }

    public void ChangeDescripText(string d)
    {
        GameObject descrip = gamePanel.transform.GetChild(0).GetChild(1).gameObject;
        descrip.GetComponent<Text>().text = d;
    }

    /******************** SoundObjController ***************************** */
    void PlaySounds()
    {
        //soundObjs = GameObject.FindGameObjectsWithTag("sound");
        bool someSound = false;
        foreach (GameObject soundObj in soundObjs)
        {
            AudioSource s = soundObj.GetComponent<AudioSource>();
            //if (OnScreen(soundObj)) Debug.Log("sound obj on screen");
            if (OnScreen(soundObj) && !s.isPlaying) PlayAudioSrc(s);
            else if (!OnScreen(soundObj)) s.Stop();
            if (IsSelected(soundObj))
            {
                Debug.Log(soundObj.name + " is selected.");
                SetHalo(soundObj, true);
                someSound = true;
                if (Input.GetKey(KeyCode.S)) UpdateAudioVol(s);
                ChangeVolText(s.volume);
                ChangeDescripText(soundObj.GetComponent<GUIText>().text);
                //SetGameMenu(true);
                //ChangeVolText(s.volume);
            }
            else
            {
                SetHalo(soundObj, false);
            }
        }
        SetSelectMenu(someSound);
    }




    void SetHalo(GameObject soundObj, bool b)
    {
        Behaviour halo = (Behaviour)soundObj.GetComponent("Halo");
        halo.enabled = b;
    }

    void UpdateAudioVol(AudioSource s)
    {
        Debug.Log("updating volume to" + (1 - s.volume));
        s.volume = 1 - s.volume;
    }

    void PlayAudioSrc(AudioSource s)
    {
        float clipLen = s.clip.length;
        float randomStartingTime = Random.Range(0.0F, clipLen);
        s.time = randomStartingTime;
        s.Play();
    }




}