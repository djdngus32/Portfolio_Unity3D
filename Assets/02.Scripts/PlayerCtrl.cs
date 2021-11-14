using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float jumpPower = 6f;
    public GameObject avatar;

    private bool isMove = true;
    private bool isJumping = false;
    private bool isAttackReady = true;
    private bool isMotionEnd = false;

    private float h = 0f;
    private float v = 0f;
    private float speed = 0f;
    private float speedBack = 1f;
    private float attackdelay = 0f;

    private Vector3 moveVec;

    private Animator animator;

    public Weapon equipWeapon;
    // Start is called before the first frame update

    private void Awake()
    {
        animator = avatar.GetComponent<Animator>();
    }
    void Start()
    {
        animator.SetBool("isEquipWeapon", true);
    }

    // Update is called once per frame
    void Update()
    {
        CheckMotion();
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        speed = new Vector3(h, 0f, v).normalized.magnitude;
        moveVec = new Vector3(h, 0f, v).normalized;
        if (!isAttackReady)
        {
            moveVec = Vector3.zero;

        }

        if (isMove)
        {
            animator.SetFloat("h", h);
            animator.SetFloat("v", v);
            animator.SetFloat("speed", speed);

            if (v < 0)
                speedBack = 0.6f;
            else
                speedBack = 1f;

            transform.Translate( moveVec * walkSpeed * speedBack * Time.deltaTime);
        }
        else
        {

        }

        Jump();
        Attack();
    }

    private void CheckMotion()
    {
        if (!isMotionEnd && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.82f)
            isMotionEnd = true;
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isJumping)
            {
                isJumping = true;
                animator.SetBool("isJumping", isJumping);
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
    }

    private void Attack()
    {
        if (equipWeapon == null)
            return;
        if (!isAttackReady)
        {
            attackdelay += Time.deltaTime;
            if (isMotionEnd)
            {
                isAttackReady = equipWeapon.rate < attackdelay;
                isMove = true;
            }
        }

        if(Input.GetMouseButtonDown(0) && isAttackReady && !isJumping && isMove)
        {
            isAttackReady = false;
            isMove = false;
            isMotionEnd = false;
            equipWeapon.Use();
            animator.SetTrigger("doAttack");
            attackdelay = 0f;
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            if (isJumping)
            {
                isJumping = false;
                animator.SetBool("isJumping", isJumping);
            }
        }
    }
}
