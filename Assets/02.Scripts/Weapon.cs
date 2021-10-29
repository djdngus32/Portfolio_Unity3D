using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range};
    public Type type;
    public float damage;
    public float rate;

    public BoxCollider hitBox;
    public TrailRenderer trail;

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Slash");
            StartCoroutine("Slash");
            Debug.Log("무기사용!");
        }
    }

    IEnumerator Slash()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("공격이 발생했다네!!!");
        hitBox.enabled = true;
        trail.enabled = true;
        yield return new WaitForSeconds(0.3f);
        hitBox.enabled = false;
        yield return new WaitForSeconds(0.3f);
        trail.enabled = false;
    }
}
