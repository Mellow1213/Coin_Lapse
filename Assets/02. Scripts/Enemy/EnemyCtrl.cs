using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    //목적지
    private Transform target;

    public Transform bullet;
    public Transform atk_point;
    private float power = 50.0f;
    private bool isShoot;
    
    private NavMeshAgent agent;
    private Animator anim;

    public GameObject[] secretObjs;

    //열거형으로 정해진 상태값을 사용
    enum State
    {
        Idle,
        Run,
        Attack
    }
    //상태 처리
    State state;
    
    void Start()
    {
        //생성시 상태를 Idle로 한다.
        state = State.Idle;

        // 랜덤으로 숨겨진 오브젝트를 활성화
        int rnd = Random.Range(0, secretObjs.Length);
        secretObjs[rnd].SetActive(true);
        
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Run:
                UpdateRun();
                break;
            case State.Attack:
                UpdateAttack();
                break;
        }
    }
   
    private void UpdateAttack()
    {
        // 멈추기
        agent.speed = 0;
        anim.SetBool("Run", false);
        float distance = Vector3.Distance(transform.position, target.transform.position);
        
        // 플레이어 공격
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (!isShoot)
        {
            StartCoroutine(Attack(fwd));
        }

        // 다시 거리가 멀어지면 이동상태로 전환
        if (distance > 10)
        {
            state = State.Run;
        }
    }

    IEnumerator Attack(Vector3 dir)
    {
        isShoot = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);
        Transform enemyBullet = Instantiate(bullet, atk_point.transform.position, Quaternion.identity); 
        enemyBullet.GetComponent<Rigidbody>().AddForce(dir * power);
        yield return new WaitForSeconds(2.0f);
        isShoot = false;
    }

    private void UpdateRun()
    {
        //남은 거리가 15미터라면 공격한다.
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= 10)
        {
            state = State.Attack;
        }
        // 런 애니메이션 실행
        anim.SetBool("Run", true);
        //타겟 방향으로 이동하다가
        agent.speed = 3.5f;
        //요원에게 목적지를 알려준다.
        agent.destination = target.transform.position;
    }

    private void UpdateIdle()
    {
        agent.speed = 0;
        //생성될때 목적지(Player)를 찿는다.
        target = GameObject.Find("Player").transform;
        //target을 찾으면 Run상태로 변경
        if (target != null)
        {
            state = State.Run;
        }
    }
}
