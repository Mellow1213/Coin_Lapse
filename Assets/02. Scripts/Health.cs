using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;
    public GameObject deathEffect;
    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log(transform.name + " 객체의 HP가 0 이하가 되어 사망합니다.");
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
