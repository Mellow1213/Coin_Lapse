using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    public TextMeshPro text;
    Color alpha;


    private void DestroyObject()
    {
        Destroy(gameObject);
    }


    private GameObject player;
    private PlayerMovement _playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = player.GetComponent<PlayerMovement>();

        
        
        transform.LookAt(player.transform);
        
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;

        alpha = text.color;
        text.text = _playerMovement.currentDamage.ToString("F2");
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {


        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
        text.color = alpha;
    }
}