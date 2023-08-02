using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private int count;

    public GameObject[] Enemies;
    public Transform sp_Point;

    public bool isStartSpawn;

    // Start is called before the first frame update
    void Start()
    {
        isStartSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStartSpawn)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        isStartSpawn = true;
        if (gameObject.CompareTag("Police"))
        {
            int rnd = Random.Range(0, 2);
            Instantiate(Enemies[rnd], sp_Point.position, Quaternion.identity);
        }
        else
        {
            Instantiate(Enemies[2], sp_Point.position, Quaternion.identity);
        }
        
        yield return new WaitForSeconds(1.0f);
        isStartSpawn = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        // 임시 시작용 코드
        isStartSpawn = false;
    }
}
