using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunControl : MonoBehaviour
{
    //射击的间隔时长
    private float shootTime = 1;
    //射击间隔时间的计时器
    private float shootTimer = 0;
    //子弹的游戏物体，和子弹的生成位置
    [SerializeField]
    private GameObject bulletGO;

    private List<GameObject> bullets;
    [SerializeField]
    private Transform firePosition;
    [SerializeField]
    private float firepower = 2200;

    // Use this for initialization
    void Start()
    {
        if (!firePosition)
            firePosition = this.transform.Find("FirePosition");
        if (bullets == null)
        {
            bullets = new List<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                InitBullet();
            }
        }

        SingletonControl.Instance.gameManager.setUICallBack(null, DisableBullets);
    }

    private void DisableBullets()
    {
        if(bullets!=null && bullets.Count > 0)
        {
            foreach (var item in bullets)
            {
                item.gameObject.SetActive(false);
            }
        }

        this.shootTimer = shootTime;
    }

    private GameObject InitBullet()
    {
        bullets.Add(GameObject.Instantiate(bulletGO, firePosition.position, Quaternion.identity));
        bullets[bullets.Count - 1].SetActive(false);
        return bullets[bullets.Count - 1];
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootTime)
        {
            //点击鼠标左键，进行射击
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()
                    && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject)
                {
                    //点击在齿轮上时不触发射击
                    if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                        return;
                }

                //实例化子弹
                GameObject g = null;
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (!bullets[i].activeSelf)
                    {
                        g = bullets[i];
                        break;
                    }
                }

                shootTimer = 0;
                if (!g)
                {
                    g = InitBullet();
                }

                //通过刚体组件给子弹添加一个正前方向上的力，以达到让子弹向前运动的效果
                g.gameObject.SetActive(true);
                g.transform.position = firePosition.position;
                g.GetComponent<Rigidbody>().AddForce(transform.forward * firepower);

                //播放手枪开火的动画
                gameObject.GetComponent<Animation>().Play();

                //播放手枪开火的音效
                SingletonControl.Instance.audioControl.PlayFire();
                //增加射击数
                SingletonControl.Instance.gameManager.AddShutNum();
            }
        }

        //方法①：按顺序绕轴旋转 （欧拉旋转）
        ////保存主相机正前方
        //Vector3 mcamera_fwd;
        ////相机前和右的叉积，得到垂直这两个轴的向上的Y
        //Vector3 mcamera_y;
        ////相机前和上的叉积，得到垂直这两个轴的向右的X
        //Vector3 mcamera_x;
        //mcamera_fwd = Camera.main.transform.forward;
        //////归一化，然后再求叉积
        ////mcamera_fwd.Normalize();
        ////相机前和右的叉积，得到垂直这两个轴的向上的Y
        //mcamera_y = Vector3.Cross(mcamera_fwd, Vector3.right);
        ////相机前和上的叉积，得到垂直这两个轴的向右的X
        //mcamera_x = Vector3.Cross(mcamera_fwd, Vector3.up);
        ////旋转速度
        //transform.Rotate(mcamera_y, Input.GetAxis("Mouse X"), Space.World);
        //transform.Rotate(mcamera_x, Input.GetAxis("Mouse Y"), Space.World);

        ////方法②： 使用LookAt，注意Z轴坐标，是取相对主相机的点来转世界坐标。由于笼子位于z=0处，这里主相机和z=0的距离应该取反
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        //transform.LookAt(new Vector3(mousePos.x, mousePos.y, 0));

        //方法③：属性面板rotation赋值
        //通过旋转模型，得到范围： 竖直: ⬆-35 ~ ⬇+10 ， 水平: ⬅-30 ~ ➡+30
        //根据鼠标在屏幕上的位置，按比例分配
        //-30~+30
        float xPosPrecent = (Input.mousePosition.x / Screen.width) * 60 - 30;
        if (xPosPrecent > 30f) xPosPrecent = 30f;
        else if (xPosPrecent < -30f) xPosPrecent = -30f;

        //-35~+10 (鼠标坐标，以屏幕左上为原点0，但是计算中是以左下为0，所以要取反)
        float yPosPrecent = (1 - Input.mousePosition.y / Screen.height) * 45 - 35;
        if (yPosPrecent > 10f) yPosPrecent = 10f;
        else if (yPosPrecent < -35f) yPosPrecent = -35f;

        transform.transform.rotation = Quaternion.Euler(new Vector3(yPosPrecent, xPosPrecent, 0));
    }
}
