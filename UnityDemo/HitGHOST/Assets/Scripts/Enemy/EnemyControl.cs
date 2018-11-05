using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;

    private Animation anim;
    [SerializeField]
    private AnimationClip idleclip;
    [SerializeField]
    private AnimationClip dieclip;


    private SkinnedMeshRenderer skinnedMeshRenderer;
    private int _colorindex = 0;
    public int colorindex
    {
        get { return _colorindex; }
        private set { _colorindex = value; }
    }

    // Use this for initialization
    private void Awake()
    {
        anim = this.GetComponent<Animation>();
        Transform childm = this.transform.Find("Mesh");
        skinnedMeshRenderer = childm.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer == null)
            skinnedMeshRenderer = childm.gameObject.AddComponent<SkinnedMeshRenderer>();
    }

    private void OnEnable()
    {
        //染色
        colorindex = Random.Range(0, materials.Length);
        SetMaterial(colorindex);

        //激活动画
        anim.clip = idleclip;
        
        anim.Play();
    }

    public void SetMaterial(int index)
    {
        colorindex = index % materials.Length;
        skinnedMeshRenderer.materials = new Material[1];
        skinnedMeshRenderer.material = materials[colorindex];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            collision.gameObject.SetActive(false);

            SingletonControl.Instance.audioControl.PlayKick();
            anim.clip = dieclip;
            anim.Play();
            SingletonControl.Instance.gameManager.AddGoalNum();

            StartCoroutine("Deactivate");
        }
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.8f);

        this.gameObject.SetActive(false);
    }
}
