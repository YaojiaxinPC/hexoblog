using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine("DisableBullet");
    }
    private void OnDisable()
    {
        //位置归零
        this.gameObject.transform.position = Vector3.zero;
        //清除外力
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    IEnumerator DisableBullet()
    {
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }
}
