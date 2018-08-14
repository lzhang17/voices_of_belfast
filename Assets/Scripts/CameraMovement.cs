using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    // ATTACH TO CAMERA
    Camera cam;
    GameObject map;
    GameObject crosshair;

    float speed = 0.05f;//0.03F;
    float zOffset, xOffset, yOffset;
    float MIN_Z, MAX_Z, MIN_X, MAX_X, MIN_Y, MAX_Y;

    bool[] canMove; //0=left, 1=up, 2=right, 3=down, 4=forward, 5=back

    // Use this for initialization
    void Start () {
        //get objects
        map = (GameObject.FindGameObjectsWithTag("map"))[0];
        crosshair = (GameObject.FindGameObjectsWithTag("crosshair"))[0];//crosshair.transform.position = cam.transform.position;

        //create moves array
        canMove = new bool[6];
        SetAllMoves(true);

        //bound zooming ability
        Vector3 mapPos = map.transform.position;
        MIN_Z = transform.position.z;
        MAX_Z = mapPos.z + zOffset;
    }

    // Update is called once per frame
    void Update () {
        UpdateCanMove();
        Vector3 dir = DirToMove();
        MoveInDir(dir);
        SetAllMoves(true);
    }

    void UpdateCanMove()
    {
        if (transform.position.z > MAX_Z) canMove[4] = false; //forward
        if (transform.position.z < MIN_Z) canMove[5] = false; //back

        // ideally, limit user to move around  map only (can go anywhere rn)
        //if (transform.position.x > MAX_X) canMove[2] = false; //right
        //if (transform.position.x < MIN_X) canMove[0] = false; //left
        //if (transform.position.y > MAX_Y) canMove[1] = false; //up
        //if (transform.position.y < MIN_Y) canMove[3] = false; //down
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
        //backward = movement in -z direction
        else if (DetKey(KeyCode.X) && canMove[4])
            dir += Vector3.forward;
        return dir;
    }

}
