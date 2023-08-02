using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private int count;

    public GameObject[] Enemies;
    public Transform sp_Point;

    private bool isDestroy;
    private float hp;
    
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Police"))
        {
            count = Random.Range(2, 7);
        }
        else
        {
            count = Random.Range(8, 13);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 차가 파괴되지 않았다면 계속 생성
        if (!isDestroy)
        {
            if (count < 7)
            {
                StartCoroutine(SpawnPolice(count));
            }
            else
            {
                StartCoroutine(SpawnSwat(count));
            }
        }
    }

    IEnumerator SpawnPolice(int a)
    {
        for (int i = 1; i <= a; i++)
        {
            int rnd = Random.Range(0, 2);
            Instantiate(Enemies[rnd], sp_Point.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    IEnumerator SpawnSwat(int a)
    {
        for (int i = 1; i <= a; i++)
        {
            Instantiate(Enemies[2], sp_Point.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
