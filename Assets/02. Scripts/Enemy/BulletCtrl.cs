using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 3.0f);
        transform.LookAt(GameObject.Find("Player").transform); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.gold -= 3;
        }
        
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
