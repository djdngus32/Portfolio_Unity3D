using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{

    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;

    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private GameObject avatar;

    private bool isFindTarget = false;
    private bool isAnimCRRunning = false;
    private bool isDeathCRRunning = false;

    public Animator animator;

    private Status status;
    
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = avatar.GetComponent<Animator>();
        status = transform.GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!status.isDeath)
        {
            View();
            animator.SetBool("Run", isFindTarget);
            if (status.isDamaged == true && isAnimCRRunning == false)
            {
                animator.SetTrigger("Damaged");
                StartCoroutine(CheckAnimationState());
            }
        }
        else if(isDeathCRRunning == false)
        {
            animator.SetTrigger("Death");
            StartCoroutine(Death());
        }
        else
        {

        }
    }

    private void View()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewDistance, targetLayer);

        foreach (Collider coll in colliders)
        {
            if (coll.transform.name == "Player")
            {
                float dist = Vector3.Distance(coll.transform.position, transform.position);
                if (dist <= viewDistance)
                {
                    Vector3 dir = (coll.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, dir);
                    if (angle <= viewAngle * 0.5f && !status.isDamaged)
                    {
                        isFindTarget = true;
                        //여기에다가 레이쏴서 벽인지 확인하는 코드 추가해야함
                        if (Vector3.Cross(transform.forward, dir).y < 0f)
                            angle *= -1f;
                        transform.Rotate(0f, angle, 0f);
                        //transform.Translate(transform.forward * Time.deltaTime);
                        transform.position = Vector3.Lerp(transform.position, coll.transform.position, Time.deltaTime * 0.7f);

                    }
                }
                else
                    isFindTarget = false;
            }

        }


    }

    IEnumerator CheckAnimationState()
    {
        isAnimCRRunning = true;
        while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
        {
            //애니메이션 재생 중에 실행되는 부분
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        status.isDamaged = false;
        isAnimCRRunning = false;
        yield return null;
    }

    IEnumerator Death()
    {
        isDeathCRRunning = true;

        yield return new WaitForSeconds(5f);

        Destroy(transform.gameObject);

        yield return null;
    }

}