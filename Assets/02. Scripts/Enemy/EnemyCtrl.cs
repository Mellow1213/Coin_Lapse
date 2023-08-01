using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    //목적지
    private Transform target;
    private float Hp;

    public Transform bullet;
    public Transform atk_point;
    private float power = 50.0f;
    
    private NavMeshAgent agent;
    private float elapsed_time = 0.0f;
    private float fire_interval = 2.0f;

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
        // anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // hp가 0이하면 즉시 사망
        if (Hp <= 0)
        {
            state = State.Dead;
        }
        
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
        // 멈추기
        agent.speed = 0;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        
        // 플레이어 공격
        elapsed_time += Time.deltaTime;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        if (elapsed_time > fire_interval)
        {
            Transform enemyBullet = Instantiate(bullet, atk_point.transform.position, Quaternion.identity);
            enemyBullet.GetComponent<Rigidbody>().AddForce(fwd * power);
            elapsed_time = 0.0f;
        }
        
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
        //target을 찾으면 Run상태로 변경
        if (target != null)
        {
            state = State.Run;
            //이렇게 state값을 바꿨다고 animation까지 바뀔까? no! 동기화를 해줘야한다.
            //anim.SetTrigger("Run");
        }
    }

    private void UpdateDead()
    {
        // 죽은 애니메이션을 실행 후
        
        // 삭제
        Destroy(this.gameObject, 2.0f);
    }
}
