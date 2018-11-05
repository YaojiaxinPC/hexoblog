using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyManager() { }
    private void Awake()
    {
        if (SingletonControl.Instance.enemyManager != null)
        {
            if (SingletonControl.Instance.enemyManager != this)
                DestroyImmediate(this.gameObject);
        }
        else
        {
            SingletonControl.Instance.enemyManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public class TargetMsgClass
    {
        public GameObject gameObject;
        public Vector3 vector;
    }


    [SerializeField]
    private EnemyControl[] enemys;

    /// <summary>
    /// 当前场景存在的怪物
    /// </summary>
    private List<EnemyTypeSC> nowexits;
    /// <summary>
    /// 缓存全部怪物
    /// </summary>
    private Dictionary<int, List<TargetMsgClass>> enemylst;
    /// <summary>
    /// 怪物位置集合
    /// </summary>
    private List<Vector3> positions;
    [SerializeField]
    private Transform enemyhold;

    //怪物生成的间隔时长
    [SerializeField]
    private float bornTime = 5.0f;
    //怪物生成间隔时间的计时器
    private float bornTimer = 0;

    private GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = SingletonControl.Instance.gameManager;
        positions = new List<Vector3>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).tag != "enemyposition") continue;
            positions.Add(this.transform.GetChild(i).transform.position);
        }

        enemylst = new Dictionary<int, List<TargetMsgClass>>();
        NowBorn();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (nowexits != null)
        {
            bornTimer += Time.deltaTime;
            //怪物已经全部打完 || 超过设定时间
            if (nowexits.Count == 0 || bornTimer > bornTime) NowBorn();
        }
    }

    private void NowBorn(List<EnemyTypeSC> enemies = null)
    {
        if (nowexits == null) nowexits = new List<EnemyTypeSC>();

        for (int i = 0; i < enemyhold.childCount; i++)
        {
            enemyhold.GetChild(i).gameObject.SetActive(false);
        }

        nowexits.Clear();
        bornTimer = 0;
        InitEnemys(enemies);
    }


    private void InitEnemys(List<EnemyTypeSC> enemies)
    {
        //全新开局
        if (enemies == null)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (Random.Range(0, 1.0f) > 0.6f)
                {
                    int type = Random.Range(0, enemys.Length);
                    InitEnemy(type, i);
                }
            }
        }
        else//读取记录
        {
            foreach (var item in enemies)
            {
                InitEnemy(item.TypeIndex, item.PositionIndex, item.ColorIndex);
            }
        }
    }

    /// <summary>
    /// 生成怪物
    /// </summary>
    /// <param name="type"></param>
    /// <param name="i"></param>
    /// <param name="colorindex"></param>
    private void InitEnemy(int type, int i, int colorindex = -1)
    {
        EnemyControl e = enemys[type];
        if (!enemylst.ContainsKey(type)) enemylst.Add(type, new List<TargetMsgClass>());

        List<TargetMsgClass> lst = enemylst[type];
        TargetMsgClass g = null;
        for (int li = 0; li < lst.Count; li++)
        {
            if (!lst[li].gameObject.activeSelf)
            {
                g = lst[li];
                break;
            }
        }
        if (g == null)
        {
            g = new TargetMsgClass();
            g.gameObject = Instantiate(e.gameObject, e.transform.position, Quaternion.identity);
            g.gameObject.transform.parent = enemyhold;
            g.gameObject.transform.rotation = e.transform.rotation;
            g.vector = g.gameObject.transform.position;
            enemylst[type].Add(g);
        }
        else
        {
            g.gameObject.SetActive(true);
        }

        Vector3 p = g.vector + positions[i];
        g.gameObject.transform.position = p;
        if (!colorindex.Equals(-1))
            g.gameObject.GetComponent<EnemyControl>().SetMaterial(colorindex);

        nowexits.Add(new EnemyTypeSC() { ColorIndex = e.colorindex, TypeIndex = type, PositionIndex = i });
    }

    public List<EnemyTypeSC> Getnowexits()
    {
        return nowexits;
    }

    /// <summary>
    /// 并没有直接修改nowexits，而是通过NowBorn来修改
    /// </summary>
    /// <param name="value"></param>
    public void Setnowexits(List<EnemyTypeSC> value)
    {
        NowBorn(value);
    }
}
