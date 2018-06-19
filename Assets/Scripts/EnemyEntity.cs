using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : GravityEntity {

    public GameObject target;
    public int moveSpeed = 2;

    Animator ani;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        ani = GetComponent<Animator>();
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Update is called once per frame
    void Update() {
        this.transform.LookAt(target.transform);

        cc.SimpleMove(transform.forward * moveSpeed);

        if (Vector3.Distance(target.transform.position, this.transform.position) < 20)
        {
            ani.Play("Attack", 1);
        }
	}
}
