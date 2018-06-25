using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelImporter;

public class EnemyEntity : GravityEntity {

    private GameObject target;
    public int moveSpeed = 1;

    //public GameObject hpBarPool;
    HpBarFactory hpBarPoolInstance;

    Slider hpBar;

    Animator ani;

    public Slider testHpBar;

    public float lifeTime = 1f;
    public bool rebirth = true;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        ani = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player");
       // hpBarPoolInstance = hpBarPool.GetComponent<HpBarFactory>();
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Sword")
        {
            var playerEntity = hit.gameObject.GetComponentInParent<PlayerEntity>();

            if (playerEntity != null)
            {
                if (playerEntity.stateInfo.IsName("handRun"))
                {
                    return;
                }
            }
            testHpBar.gameObject.SetActive(false);
            var skinnedVoxelExplosion = this.GetComponent<VoxelSkinnedAnimationObjectExplosion>();
            if (skinnedVoxelExplosion != null)
            {
                if (!skinnedVoxelExplosion.enabled)
                {
                    var collider = this;
                    collider.enabled = false;

                    skinnedVoxelExplosion.SetExplosionCenter(skinnedVoxelExplosion.transform.worldToLocalMatrix.MultiplyPoint3x4(hit.point));

                    var animator = collider.GetComponent<Animator>();
                    var animatorEnabled = false;
                    if (animator != null)
                    {
                        animatorEnabled = animator.enabled;
                        animator.enabled = false;
                    }
                    skinnedVoxelExplosion.BakeExplosionPlay(lifeTime, () =>
                    {
                        Destroy(skinnedVoxelExplosion.gameObject);
                        NotifierManager.SendNotification(PublicEnum.NotifierSendType.EnemyDie);
                    });
                }
            }
        }
    }
}
