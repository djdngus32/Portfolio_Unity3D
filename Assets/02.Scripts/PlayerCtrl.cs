using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float jumpPower = 6f;
    public GameObject avatar;

    private bool isJumping = false;
    private float h = 0f;
    private float v = 0f;
    private float speed = 0f;
    private float speedBack = 1f;
    public float rotSpeed = 1.5f;
    private Animator animator;
    // Start is called before the first frame update

    private void Awake()
    {
        animator = avatar.GetComponent<Animator>();
    }
    void Start()
    {
        
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
