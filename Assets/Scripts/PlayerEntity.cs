using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour {

    public GameObject sword;
    public GameObject hand;
    public GameObject hips;

    public int moveSpeed;
    public int rotateSpeed;

    CharacterController cc;
    float _vertSpeed;
    Vector3 movement;

    Animator ani;

    enum SwordState {
        InHand = 0,
        InBody = 1,
    }

    SwordState swordState = SwordState.InBody;
    // Use this for initialization
    void Start() {
        cc = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!cc.isGrounded)
        {
            _vertSpeed += Physics.gravity.y * 5 * Time.deltaTime;//不在地面上 
            if (_vertSpeed < -10.0f)
            {
                _vertSpeed = -10.0f;
            }

            movement.y = _vertSpeed;
            movement *= Time.deltaTime;
            cc.Move(movement);
            return;
        }
    }

    // Update is called once per frameh
    void Update() {
        if (!cc.isGrounded)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (swordState == SwordState.InBody)
            {
                ani.Play("PickSword", 1);
            }
            else
            {
                ani.SetBool("isAttack", true);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
    
        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
        {

            Vector3 targetpos = new Vector3(-h, 0, -v);
            //print (targetpos);  
            transform.LookAt(targetpos + transform.position);
            cc.SimpleMove(transform.forward * moveSpeed);
        }
    }

    public void AddSwordToHand()
    {
        if (hand != null && sword != null)
        {
            sword.transform.SetParent(hand.transform);

            sword.transform.localRotation = new Quaternion((float)10.6,(float)-25.822, (float)20.285, (float)1.0);
       
            sword.transform.localPosition = new Vector3((float)5.95, (float)1.49, (float)2.12);

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

            sword.transform.localPosition = new Vector3((float)-5.5,(float)4, (float)2);

            swordState = SwordState.InBody;
            //   Debug.Log(sword.transform.position);
        }
    }

    public void EndAttack()
    {
        ani.SetBool("isAttack", false);
    }
}
