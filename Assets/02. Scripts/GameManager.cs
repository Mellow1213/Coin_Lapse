using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스 저장
    public static GameManager Instance { get; private set; }

    // 게임 관련 변수 및 메서드를 여기에 추가

    private void Awake()
    {
        // 싱글톤 인스턴스가 이미 있으면 파괴, 아니면 현재 인스턴스를 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // 씬이 바뀌어도 파괴되지 않게 설정
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log("gold = " + gold);
    }

    public int gold = 0;
    // 다른 메서드 및 로직을 여기에 추가할 수 있습니다.
}