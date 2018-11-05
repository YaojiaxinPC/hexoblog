using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private Text shootnumtxt;
    [SerializeField]
    private Text scoretxt;
    [SerializeField]
    private Text messagetxt;
    [SerializeField]
    private Toggle musictoggle;

    private float clearmsgTime = 2.0f;
    private float clearmsgTimer = 0;

    // Use this for initialization
    void Start()
    {
        gameManager = SingletonControl.Instance.gameManager;
        gameManager.setUICallBack(ShowMessage);
    }

    // Update is called once per frame
    void Update()
    {
        shootnumtxt.text = gameManager.shutnum.ToString();
        scoretxt.text = gameManager.goalnum.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
            menu.SetActive(!menu.activeSelf);

        if (musictoggle.isOn != gameManager.musicon)
        {
            gameManager.SetMousicOn(musictoggle.isOn);
        }
        if (menu.activeSelf != gameManager.ispause)
        {
            gameManager.IsPause(menu.activeSelf);
        }

        if (!string.IsNullOrEmpty(messagetxt.text))
        {
            clearmsgTimer += Time.unscaledDeltaTime;

            if (clearmsgTimer > clearmsgTime)
                messagetxt.text = string.Empty;
        }
    }

    public void ShowMessage(string str)
    {
        messagetxt.text = str;
        clearmsgTimer = 0;
    }


    public void ShowMenu(bool isshow)
    {
        if (isshow)
        {
            menu.SetActive(true);
        }
        else
        {
            menu.SetActive(false);
        }
    }

    public void NewGame()
    {
        gameManager.NewGame();
    }

    public void Continue()
    {
        menu.SetActive(false);
    }

    public void SaveNow()
    {
        gameManager.SaveNow();
    }

    public void LoadLast()
    {
        gameManager.LoadLast();
    }

    public void ExitGame()
    {
        gameManager.ExitGame();
    }
}
