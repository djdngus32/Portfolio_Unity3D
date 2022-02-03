using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public bool isDeath = false;
    public bool isDamaged = false;

    public Image healthBar;

    private float hp;
    private float mp;
    [SerializeField] private float maxHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float atk;
    private GameObject healthCanvas;


    private void Awake()
    {
        hp = maxHp;
        mp = maxMp;
        FindHealthCanvas();
        healthBar = healthCanvas.transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        healthBar.fillAmount = hp / maxHp;
        if(hp <= 0 && healthCanvas != null)
        {
            Destroy(healthCanvas);
        }
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

    private void FindHealthCanvas()
    {
        // HealthCanvas는 무조건 하위 오브젝트에 있어야함
        foreach(Transform child in transform)
        {
            if(child.name == "Health Canvas")
            {
                healthCanvas = child.gameObject;
                break;
            }
        }
    }
}
