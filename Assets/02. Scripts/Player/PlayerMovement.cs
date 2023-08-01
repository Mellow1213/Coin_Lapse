using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float jumpPower;
    public bool isJumping = false;

    CharacterController characterController;


    public float range = 100.0f;

    private float delayTimer = 0f;

    public GameObject temp_Particle;
    public GameObject temp_FireParticle;

    public GameObject muzzle;

    public GameObject gunSlot;
    [SerializeField] private GameObject currentWeapon;
    private GunProperty currentGunProperty;

    public ParticleSystem bulletShellParticle;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        characterController = GetComponent<CharacterController>();
        currentWeapon = gunSlot.transform.GetChild(0).GetComponent<Transform>().gameObject;
        currentGunProperty = currentWeapon.GetComponent<GunProperty>();
        
    }

    float gravity = -3.8f;
    float yVelocity = 0;
    private Camera _camera;

    // Update is called once per frame
    void Update()
    {
        Movement();
        Fire();
    }

    void Fire()
    {
        if (delayTimer <= currentGunProperty.delay)
        {
            delayTimer += Time.deltaTime;
        }


        if (Input.GetMouseButton(0) && delayTimer >= currentGunProperty.delay && GameManager.Instance.gold >= 0)
        {
            delayTimer = 0f;

            GameObject fireParticle =
                Instantiate(temp_FireParticle, muzzle.transform.position, muzzle.transform.rotation);
            fireParticle.transform.parent = currentWeapon.transform;

            GameManager.Instance.gold -= currentGunProperty.cost;

            RecoilAnim();
            
            RaycastHit hit;
            // Create a ray that goes through the center of the screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out hit, range))
            {
                Instantiate(temp_Particle, hit.point, Quaternion.identity);
                if (hit.transform.CompareTag("Enemy"))
                {
                    Health health = hit.transform.GetComponent<Health>();
                    if (health != null)
                        health.Damage(currentGunProperty.damage);
                }
            }
        }
    }

    void RecoilAnim()
    {
        currentWeapon.transform.DOLocalMoveZ(currentWeapon.transform.localPosition.z-currentGunProperty.recoilDistance, currentGunProperty.delay*0.25f, false).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo);
    }

    void Movement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);

        dir = Camera.main.transform.TransformDirection(dir);


        yVelocity += gravity * Time.deltaTime / 1.3f;
        dir.y = yVelocity;


        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            moveSpeed = runSpeed;
        else
            moveSpeed = walkSpeed;

        characterController.Move(dir * (moveSpeed * Time.deltaTime));

        if (characterController.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            if (isJumping)
                isJumping = false;
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
    }
}