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

    // Key kd : key down, 
    private bool kdInteraction = false;

    private float h = 0f;
    private float v = 0f;
    private float speed = 0f;
    private float speedBack = 1f;
    private float attackdelay = 0f;

    private Vector3 moveVec;

    private Animator animator;

    private GameObject nearObject;
    private GameObject rightHand;
    public GameObject curWeapon;
    // Start is called before the first frame update

    private void Awake()
    {
        animator = avatar.GetComponent<Animator>();

        #region FindRightHand
        rightHand = FindInnerObject(avatar, "Weapon_RH");
        #endregion
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
        kdInteraction = Input.GetButtonDown("Interaction");
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
        Interaction();
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
        Weapon weapon = null;
        if (curWeapon != null)
        {
            weapon = curWeapon.GetComponent<Weapon>();
        }
        else
            return;
        if (!isAttackReady)
        {
            attackdelay += Time.deltaTime;
            if (isMotionEnd)
            {
                isAttackReady = weapon.rate < attackdelay;
                isMove = true;
            }
        }

        if(Input.GetMouseButtonDown(0) && isAttackReady && !isJumping && isMove)
        {
            isAttackReady = false;
            isMove = false;
            isMotionEnd = false;
            weapon.Use();
            animator.SetTrigger("doAttack");
            attackdelay = 0f;
        }

    }

    private void Interaction()
    {
        if (kdInteraction && !isJumping)
        {
            if (nearObject != null)
            {
                if (nearObject.tag == "Item")
                {
                    if (nearObject.GetComponent<Item>().type == Item.Type.Weapon)
                    {
                        Equip(nearObject);
                    }
                }
            }
        }
    }

    private void Equip(GameObject item)
    {
        #region Weapon
        if(item.GetComponent<Item>().type == Item.Type.Weapon)
        {
            GameObject weapon = item.transform.GetChild(0).gameObject;
            if (rightHand != null)
            {
                Debug.Log("무기 있데");
                
                if (rightHand.transform.GetChild(0) != null)
                {
                    Debug.Log("원래 무기 가지고있으니가 없애고 바꾼다");
                    Debug.Log(item.name);
                    Debug.Log(rightHand.name);
                    Destroy(rightHand.transform.GetChild(0).gameObject);
                    weapon.transform.parent = rightHand.transform;
                    //item.transform.GetChild(0).parent = rightHand.transform;
                    weapon.transform.localPosition = Vector3.zero;
                    weapon.transform.localRotation = Quaternion.identity;
                    weapon.transform.localScale = Vector3.one;
                    Destroy(item);
                    //Destroy(item.GetComponent<Rigidbody>());
                    //Destroy(item.GetComponent<Collider>());

                }
                else
                {
                    //item.transform.parent = rightHand.transform;
                    weapon.transform.parent = rightHand.transform;
                    weapon.transform.localPosition = Vector3.zero;
                    weapon.transform.localRotation = Quaternion.identity;
                    weapon.transform.localScale = Vector3.one;
                    //Destroy(item.GetComponent<Rigidbody>());
                    //Destroy(item.GetComponent<Collider>());
                    Destroy(item);
                }
            }
            curWeapon = weapon;
        }
        #endregion
    }

    private static GameObject innerObject;

    private static GameObject FindInnerObject(GameObject parent, string name)
    {
        foreach(Transform child in parent.transform)
        {
            //Debug.Log("찾아볼까?");
            if(child.name == name)
            {
                //Debug.Log("찾았다!");
                innerObject =  child.gameObject;
                break;
            }
            else
            {
                //Debug.Log("깊숙히");
                FindInnerObject(child.gameObject, name);
            }
        }
        return innerObject;
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

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            nearObject = other.gameObject;
        }
    }
}
