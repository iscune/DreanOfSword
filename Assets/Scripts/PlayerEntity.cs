using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : GravityEntity {

    public GameObject sword;
    public GameObject hand;
    public GameObject hips;

    public int moveSpeed;
    public int rotateSpeed;

    Animator ani;

    Energy energy;

    public GameObject energyBar;

    enum SwordState {
        InHand = 0,
        InBody = 1,
    }

    enum WalkState {
        stand = 0,
        walk = 1,
        run = 2,
    }


    SwordState swordState = SwordState.InBody;

    WalkState walkState = WalkState.stand;

    float speedRotio = 0;

    // Use this for initialization
    protected override void Start() {
        base.Start();

        ani = GetComponent<Animator>();

        energy = new Energy(energyBar);
        StartCoroutine(EnergyAutoAdd());
        StartCoroutine(SpeedChange());
    }

    IEnumerator SpeedChange()
    {
        while(true)
        {
            if (walkState == WalkState.walk)
            {
                speedRotio = speedRotio + (float)0.2;
            }

            if (speedRotio > 1)
            {
                walkState = WalkState.run;
                speedRotio = 1;
            }

            ani.SetFloat("runSpeed", speedRotio);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator EnergyAutoAdd()
    {
        while (true)
        {
            energy.Value = 1;
            yield return new WaitForSeconds((float)0.1);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Update is called once per frameh
    void Update() {
        if (!cc.isGrounded)
        {
            return;
        }

        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(1);
        
        if (Input.GetMouseButtonDown(0) && stateInfo.IsName("handRun"))
        {
            if (swordState == SwordState.InBody)
            {
                ani.SetInteger("AttackMode", 2);
                ani.SetFloat("reverse", 1);
            }
            else
            {
                energy.Value = -10;
                ani.SetBool("isAttack", true);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
           if(swordState == SwordState.InHand && stateInfo.IsName("handRun"))
           {

                ani.Play("PickSword", 1,1);
                ani.SetFloat("reverse", -1);
           }   
        }

        if (Input.GetKeyDown(KeyCode.Space) && swordState == SwordState.InHand)
        {
            ani.SetInteger("AttackMode", 1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ani.SetInteger("AttackMode", 3);
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
    
        if ((Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f) && !stateInfo.IsName("LongDistanceAttack"))
        {

            Vector3 targetpos = new Vector3(-h, 0, -v);
            //print (targetpos);  
            transform.LookAt(targetpos + transform.position);
            cc.SimpleMove(transform.forward * moveSpeed * speedRotio);

            if (walkState != WalkState.run)
            {
                walkState = WalkState.walk;
            }
        }
        else
        {
            speedRotio = (float)0.1;
            walkState = WalkState.stand;
            ani.SetFloat("runSpeed", speedRotio);
        }
    }

    public void AddSwordToHand()
    {
        if (hand != null && sword != null)
        {
            sword.transform.SetParent(hand.transform);

            sword.transform.localRotation = new Quaternion((float)10.6,(float)-25.822, (float)20.285, (float)1.0);
       
            sword.transform.localPosition = new Vector3((float)0.595, (float)0.249, (float)0.212);

            swordState = SwordState.InHand;
            //  Debug.Log(sword.transform.position);
        }
    }

    public void AddHandToBody()
    {
        if (hand != null && sword != null)
        {
            sword.transform.SetParent(hips.transform);

            sword.transform.localRotation = new Quaternion();

            sword.transform.localPosition = new Vector3((float)-1.25,(float)-0.75, (float)-8.7);

            swordState = SwordState.InBody;

           // ani.SetFloat("reverse", 1);
            //   Debug.Log(sword.transform.position);
        }
    }

    public void EndAttack()
    {
        ani.SetBool("isAttack", false);
        ani.SetInteger("AttackMode", 0);
    }
}
