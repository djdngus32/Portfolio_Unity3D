using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float jumpPower = 6f;
    public GameObject avatar;

    private bool isJumping = false;
    private bool isAttackReady = true;

    private float h = 0f;
    private float v = 0f;
    private float speed = 0f;
    private float speedBack = 1f;
    private float attackdelay = 0f;

    private Animator animator;

    public Weapon equipWeapon;
    // Start is called before the first frame update

    private void Awake()
    {
        animator = avatar.GetComponent<Animator>();
    }
    void Start()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach(Transform Child in allChildren)
        {
            if(Child.name == "Weapon_RH")
            {
                if (Child != null)
                {
                    GameObject Weapon = Child.transform.GetChild(0).gameObject;
                    if (Weapon != null)
                    {
                        Debug.Log("무기 찾았데!");
                        equipWeapon = Weapon.GetComponent<Weapon>();
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        speed = new Vector3(h, 0f, v).normalized.magnitude;
        animator.SetFloat("h", h);
        animator.SetFloat("v", v);
        animator.SetFloat("speed", speed);

        if (v < 0)
            speedBack = 0.6f;
        else
            speedBack = 1f;

        transform.Translate(new Vector3(h, 0f, v).normalized * walkSpeed * speedBack * Time.deltaTime);

        Jump();
        Attack();
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
            isAttackReady = equipWeapon.rate < attackdelay;
        }

        if(Input.GetMouseButtonDown(0) && isAttackReady && !isJumping)
        {
            Debug.Log("공격!");
            isAttackReady = false;
            equipWeapon.Use();
            attackdelay = 0f;
            StopCoroutine("TestCoru");
            StartCoroutine("TestCoru");
        }

    }

    IEnumerator TestCoru()
    {
        Debug.Log("테스트 코루틴입니당.");
        yield return null;
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
