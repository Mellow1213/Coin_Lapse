using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    //목적지
    public Transform target;

    public GameObject bullet;
    
    NavMeshAgent agent;

    // public Animator anim;

    //열거형으로 정해진 상태값을 사용
    enum State
    {
        Idle,
        Run,
        Attack,
        Dead
    }
    //상태 처리
    State state;
    
    void Start()
    {
        //생성시 상태를 Idle로 한다.
        state = State.Idle;

        agent = GetComponent<NavMeshAgent>();
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
            case State.Dead:
                UpdateDead();
                break;
        }
    }
   
    private void UpdateAttack()
    {
        agent.speed = 0;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        
        // 플레이어 공격
        Instantiate(bullet, transform.position, transform.rotation);
        
        
        // 다시 거리가 멀어지면 이동상태로 전환
        if (distance > 15)
        {
            state = State.Run;
            // anim.SetTrigger("Run");
        }
    }

    private void UpdateRun()
    {
        //남은 거리가 15미터라면 공격한다.
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= 15)
        {
            state = State.Attack;
            // anim.SetTrigger("Attack");
        }

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
        //target을 찾으면 Run상태로 전이하고 싶다.
        if (target != null)
        {
            state = State.Run;
            //이렇게 state값을 바꿨다고 animation까지 바뀔까? no! 동기화를 해줘야한다.
            //anim.SetTrigger("Run");
        }
    }

    private void UpdateDead()
    {
        // 애니메이션을 dead로 바꾸고
        Destroy(this.gameObject, 2.0f);
    }
}
