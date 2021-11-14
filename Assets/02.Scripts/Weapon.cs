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
        }
    }

    IEnumerator Slash()
    {
        yield return new WaitForSeconds(0.3f);
        hitBox.enabled = true;
        yield return new WaitForSeconds(0.1f);
        trail.enabled = true;
        yield return new WaitForSeconds(0.3f);
        hitBox.enabled = false;
        trail.enabled = false;
    }
}
