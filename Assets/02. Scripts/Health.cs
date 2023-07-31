using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log(transform.name + " 객체의 HP가 0 이하가 되어 사망합니다.");
            Destroy(gameObject);
        }
    }

}
