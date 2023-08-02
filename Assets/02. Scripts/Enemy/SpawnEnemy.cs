using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private int count;

    public GameObject[] Enemies;
    public Transform sp_Point;
    public Transform endPoint;

    private bool startSpawn = false;
    private float timer = 99;

    public float spawnTime;

    public bool isPolice;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(endPoint);
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawn)
        {
            timer += Time.deltaTime;
            SpawnStart();
        }
    }

    void SpawnStart()
    {
        if (timer >= spawnTime)
        {
            timer = 0f;
            if (isPolice)
            {
                int rnd = Random.Range(0, 2);
                Instantiate(Enemies[rnd], sp_Point.position, Quaternion.identity);
            }
            else
            {
                Instantiate(Enemies[2], sp_Point.position, Quaternion.identity);
            }
        }
    }

    IEnumerator Spawn()
    {
        transform.DOMove(endPoint.position, 5f);
        yield return new WaitForSeconds(5f);
        startSpawn = true;
    }
}