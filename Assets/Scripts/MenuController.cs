using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{

    GameObject gamePanel;
    GameObject mainMenu;
    GameObject startButton;

    float defaultVol = 1f;
    bool showDefault = true;

    void Start()
    {

        //menu stuff
        gamePanel = GameObject.Find("GamePanel");
        mainMenu = GameObject.Find("MainMenuPanel");
        startButton = GameObject.Find("StartButton");

        //make start button invisible at first
        startButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        startButton.GetComponentsInChildren<CanvasRenderer>()[1].SetAlpha(0);

        SetSelectMenu(false);

        //set start button to fade in
        startButton.GetComponentsInChildren<CanvasRenderer>()[1].SetAlpha(0.25f);
        Invoke("ButtonFadeIn", 3.0f);
    }



    void Update()
    {
        //if (showDefault)
        //    ChangeVolText(defaultVol);
    }

    private void LateUpdate()
    {
        //bool showDefault should have been changed if something was selected
        SetSelectMenu(!showDefault);
        showDefault = true;
    }

    public void SetShowDefaultGM(bool b)
    {
        showDefault = b;
    }

    public void Test()
    {
        Debug.Log("menuController works");
    }

    void ButtonFadeIn()
    {
        startButton.GetComponentsInChildren<CanvasRenderer>()[1].SetAlpha(0.5f);
        startButton.GetComponent<Image>().CrossFadeAlpha(1, 6.0f, false);
        startButton.GetComponentsInChildren<CanvasRenderer>()[1].SetAlpha(1f);
    }

    public bool OnMainMenu() {
        return mainMenu.activeInHierarchy;
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

}