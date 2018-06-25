using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEntity : GravityEntity {

    public GameObject target;
    public int moveSpeed = 1;

    public GameObject hpBarPool;
    HpBarFactory hpBarPoolInstance;

    Slider hpBar;

    Animator ani;

    public Slider testHpBar;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        ani = GetComponent<Animator>();
        hpBarPoolInstance = hpBarPool.GetComponent<HpBarFactory>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void setHpBar()
    {
        Vector3 headPos = transform.position;
        headPos.y = headPos.y + cc.height;
        hpBar.transform.position = Camera.main.WorldToScreenPoint(headPos);
    }

    // Update is called once per frame
    void Update() {
        

        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(1);

        if (state.IsName("HandWalk"))
        {
            this.transform.LookAt(target.transform);
            cc.SimpleMove(transform.forward * moveSpeed);
        }
        

        if (Vector3.Distance(target.transform.position, this.transform.position) < 1.8)
        {
            ani.Play("Attack", 1);
        }

        if (hpBar)
        {
            setHpBar();
        }

        //testHpBar.transform.LookAt(Camera.main.transform);
    }

    void OnBeCameInvisible()
    {
        if (hpBar)
        {
            hpBar.enabled = false;
            hpBar = null;
        }  
    }

    private void OnBecameVisible()
    {
        //hpBar = hpBarPoolInstance.Get();
        //setHpBar();
    }
}
