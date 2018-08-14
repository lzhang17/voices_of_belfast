using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{ 
    //for sound objects

    AudioSource s;
    CameraController camController;
    MenuController menuController;
    float defaultVol = 1.0f;

    // Use this for initialization
    void Start()
    {
        menuController = GameObject.FindGameObjectWithTag("menuCanvas").GetComponent<MenuController>();
        camController = GameObject.FindGameObjectWithTag("movingCam").GetComponent<CameraController>();
        s = GetComponent<AudioSource>();
        s.volume = defaultVol;
        SetHalo(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!menuController.OnMainMenu())
        {
            PlaySounds();
            UpdateForSelected();
        }
    }

    void PlaySounds()
    {
        if (camController.OnScreen(gameObject.transform.position))
        {
            if (!s.isPlaying)
                PlayAudioSrc();
        }
        else
            s.Stop();
    }

    void UpdateForSelected() {
        if (camController.IsSelected(gameObject))
        {
            SetHalo(true);
            UpdateAudioVol();
            //update menu stuff
            menuController.SetShowDefaultGM(false);
            menuController.ChangeVolText(s.volume);
            menuController.ChangeDescripText(gameObject.GetComponent<GUIText>().text);
        }
        else
        {
            SetHalo(false);
        }

    }

    void PlayAudioSrc()
    {
        float clipLen = s.clip.length;
        float randomStartingTime = Random.Range(0.0F, clipLen);
        s.time = randomStartingTime;
        s.Play();
    }

    void SetHalo(bool b)
    {
        Behaviour halo = (Behaviour)GetComponent("Halo");
        halo.enabled = b;
    }

    void UpdateAudioVol()
    {
        if (!Input.GetKeyDown(KeyCode.S))
            return;
        Debug.Log("updating volume to" + (1 - s.volume));
        s.volume = 1 - s.volume;
    }
}