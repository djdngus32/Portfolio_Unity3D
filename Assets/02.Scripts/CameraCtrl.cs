using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject target;
    private float mouseY = 0f;
    private float distance = 5f;
    private float rotSpeed = 3f;
    private Vector2 _look;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _look.x = Input.GetAxis("Mouse X");
        _look.y = Input.GetAxis("Mouse Y");

        #region Horizontal Rotation
        target.transform.parent.transform.rotation *= Quaternion.AngleAxis(_look.x * rotSpeed, Vector3.up);
        #endregion

        #region Vertical Rotation
        target.transform.rotation *= Quaternion.AngleAxis(-_look.y * rotSpeed, Vector3.right);

        Vector3 angle = target.transform.localEulerAngles;
        angle.z = 0;

        float angleX = target.transform.localEulerAngles.x;

        if (angleX > 180 && angleX < 340)
        {
            angle.x = 340;
        }
        else if (angleX < 180 && angleX > 40)
        {
            angle.x = 40;
        }

        target.transform.localEulerAngles = angle;
        #endregion
        //Rotate();
        FollowTarget();
    }

    private void Rotate()
    {
        Vector3 rot = transform.rotation.eulerAngles; // 현재 카메라의 각도를 Vector3로 반환
        //rot.y += Input.GetAxis("Mouse X") * rotSpeed;
        Vector3 dir = transform.forward.normalized;
        float angle = Input.GetAxis("Mouse Y");
        if (Vector3.Dot(target.transform.forward.normalized, dir) < 0)
            angle = 0f;
        rot.x += -1 * angle * rotSpeed;
        Debug.Log(rot.x.ToString());
        Quaternion rotQ = Quaternion.Euler(rot);
        rotQ.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotQ, 2f);

    }

    private void FollowTarget()
    {
        Vector3 dir = target.transform.forward.normalized;
        transform.position = target.transform.position - (dir * distance);
        transform.LookAt(target.transform);
    }
}
