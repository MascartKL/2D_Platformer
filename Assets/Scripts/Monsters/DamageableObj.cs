using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObj : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private Image hpBarEffect;
    [SerializeField] private float hp;

    private float hpSpeed = 0.002f;

    [Header("Resistance")]
    [SerializeField] public float physRes;

    public delegate void DeathDelegate(GameObject go);

    public static event DeathDelegate Death;

    public void TakeDamage(float damage)
    {
        hp -= (damage - damage*(physRes/100));

        hpBar.fillAmount = hp * 0.01f;
        

        Debug.Log(hp);
        Debug.Log("hit");
        if (hp <= 0)
        {
            Destroy(gameObject);
            Debug.Log("death");
            Death(this.gameObject);
        }
    }

    private void Update()
    {
        if (hpBar.fillAmount < hpBarEffect.fillAmount)
            hpBarEffect.fillAmount -= hpSpeed;
        else
            hpBarEffect.fillAmount = hpBar.fillAmount;
    }
}