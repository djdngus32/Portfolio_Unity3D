using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public bool isDeath = false;
    public bool isDamaged = false;

    private float hp;
    private float mp;
    [SerializeField] private float maxHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float atk;



    private void Awake()
    {
        hp = maxHp;
        mp = maxMp;
    }

    public void SetDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            isDeath = true;
        if (!isDamaged)
            isDamaged = true;
        Debug.Log(gameObject.name + "의 현재 남은 체력" + hp.ToString());
    }
}
