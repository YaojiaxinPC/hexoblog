using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJson;

public class SingletonControl
{
    private class InnerClass
    {
        static InnerClass() { }

        internal static SingletonControl instance = new SingletonControl();
    }

    private SingletonControl() { }
    public static SingletonControl Instance
    {
        get { return InnerClass.instance; }
    }


    public AudioControl audioControl = null;
    public GameManager gameManager = null;
    public EnemyManager enemyManager = null;
}


public class GameManager : MonoBehaviour
{
    private GameManager() { }
    private void Awake()
    {
        if (SingletonControl.Instance.gameManager != null)
        {
            if (SingletonControl.Instance.gameManager != this)
                DestroyImmediate(this.gameObject);
        }
        else
        {
            SingletonControl.Instance.gameManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private int _shutnum = 0;
    public int shutnum
    {
        get { return _shutnum; }
        private set { _shutnum = value; }
    }

    private int _goalnum = 0;
    public int goalnum
    {
        get { return _goalnum; }
        private set { _goalnum = value; }
    }

    private bool _musicon = true;
    public bool musicon
    {
        get { return _musicon; }
        private set { _musicon = value; }
    }

    private bool _ispause = false;
    public bool ispause
    {
        get { return _ispause; }
        private set { _ispause = value; }
    }

    public void AddGoalNum(int num = 1)
    {
        goalnum += num;
    }

    public void SetGoalNum(int num = 0)
    {
        goalnum = num;
    }

    public void AddShutNum(int num = 1)
    {
        shutnum += num;
    }

    public void SetShutNum(int num = 0)
    {
        shutnum = num;
    }

    public void SetMousicOn(bool value)
    {
        musicon = value;
    }

    public void IsPause(bool value)
    {
        ispause = value;
        Time.timeScale = ispause ? 0 : 1;
    }

    const string prefskey = "hitghost";
    private EnemyManager enemyManager = null;
    private Action<string> action = null;
    private Action Continue = null;
    private void Start()
    {
        enemyManager = SingletonControl.Instance.enemyManager;
    }

    public void setUICallBack(Action<string> _action, Action _continue)
    {
        if (_action != null)
            action += _action;
        if (_continue != null)
            Continue += _continue;
    }

    private void OnDestroy()
    {
        action = null;
        Continue = null;
    }

    public void NewGame()
    {
        SetGoalNum(0);
        SetShutNum(0);

        PlayerPrefs.SetString(prefskey, "");
        enemyManager.Setnowexits(null);
    }

    public void SaveNow()
    {
        List<EnemyTypeSC> list = enemyManager.Getnowexits();

        SaveJson saveJson = new SaveJson()
        {
            enemyTypeSCs = list,
            goalnum = this.goalnum,
            musicon = this.musicon,
            shutnum = this.shutnum
        };

        string savestr = dealserialize(saveJson);
        if (!string.IsNullOrEmpty(savestr))
        {
            PlayerPrefs.SetString(prefskey, savestr);
            setmsg("保存成功！");
        }
        else
            setmsg("保存失败!请稍后再试");
    }

    private string dealserialize(SaveJson saveJson)
    {
        JsonArray jsonArray = new JsonArray();

        foreach (var item in saveJson.enemyTypeSCs)
        {
            JsonObject pairs = new JsonObject();
            pairs["ColorIndex"] = item.ColorIndex;
            pairs["PositionIndex"] = item.PositionIndex;
            pairs["TypeIndex"] = item.TypeIndex;
            jsonArray.Add(pairs);
        }

        JsonObject result = new JsonObject();

        result["enemyTypeSCs"] = jsonArray;
        result["goalnum"] = saveJson.goalnum;
        result["shutnum"] = saveJson.shutnum;
        result["musicon"] = saveJson.musicon;

        return SimpleJson.SimpleJson.SerializeObject(result);
    }

    private SaveJson dealdeserial(string jsonstr)
    {
        JsonObject result = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsonstr);
        SaveJson saveJson = new SaveJson();

        JsonArray jsonArray = (JsonArray)result["enemyTypeSCs"];
        saveJson.goalnum = int.Parse(result["goalnum"].ToString());
        saveJson.shutnum = int.Parse(result["shutnum"].ToString());
        saveJson.musicon = bool.Parse(result["musicon"].ToString());
        saveJson.enemyTypeSCs = new List<EnemyTypeSC>();

        foreach (JsonObject item in jsonArray)
        {

            EnemyTypeSC enemyTypeSC = new EnemyTypeSC();
            enemyTypeSC.ColorIndex = int.Parse(item["ColorIndex"].ToString());
            enemyTypeSC.PositionIndex = int.Parse(item["PositionIndex"].ToString());
            enemyTypeSC.TypeIndex = int.Parse(item["TypeIndex"].ToString());

            saveJson.enemyTypeSCs.Add(enemyTypeSC);
        }

        return saveJson;
    }

    public void LoadLast()
    {
        if (PlayerPrefs.HasKey(prefskey))
        {
            string jsonstr = PlayerPrefs.GetString(prefskey);

            if (!string.IsNullOrEmpty(jsonstr))
            {
                SaveJson saveJson = dealdeserial(jsonstr);
                if (saveJson != null)
                {
                    enemyManager.Setnowexits(saveJson.enemyTypeSCs);

                    SetGoalNum(saveJson.goalnum);
                    SetShutNum(saveJson.shutnum);
                    this.musicon = saveJson.musicon;
                    if (Continue != null) Continue();
                }

                return;
            }
            //空
        }

        setmsg("当前没有存档！请新建游戏");
    }

    private void setmsg(string str)
    {
        if (action != null)
            action(str);
    }

    public void ExitGame()
    {

    }
}
