using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpPower;
    public bool isJumping = false;
    
    CharacterController characterController;


    public float range = 100.0f;
    public float fireDelay = 0.1f;

    private float delayTimer = 0f;

    public GameObject temp_Particle;
    public GameObject temp_FireParticle;

    public GameObject muzzle;

    public GameObject gun;
    [SerializeField] private GameObject currentWeapon;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        characterController = GetComponent<CharacterController>();
        currentWeapon = gun.transform.GetChild(0).GetComponent<Transform>().gameObject;
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
        if (delayTimer <= fireDelay)
        {
            delayTimer += Time.deltaTime;
        }
        
        
        if (Input.GetMouseButton(0) && delayTimer >= fireDelay)
        {
            delayTimer = 0f;
            
            GameObject fireParticle = Instantiate(temp_FireParticle, muzzle.transform.position, muzzle.transform.rotation);
            fireParticle.transform.parent = _camera.transform;
            
            RaycastHit hit; 
            // Create a ray that goes through the center of the screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
            
            if (Physics.Raycast(ray, out hit, range))
            {
                Instantiate(temp_Particle, hit.point, Quaternion.identity);
                if (hit.transform.CompareTag("Enemy"))
                {
                    Debug.Log("hit name: " + hit.transform.name); // Output the name of the object we hit
                    Health health = hit.transform.GetComponent<Health>();
                    if(health != null)
                        health.Damage(currentWeapon.GetComponent<GunProperty>().damage);
                }
                
                // Here you can implement the effects of the shooting, for example:
                // hit.transform.gameObject.GetComponent<HealthSystem>()?.TakeDamage(damageAmount);
            }
        }
    }
    
    void Movement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);

        dir = Camera.main.transform.TransformDirection(dir);


        yVelocity += gravity * Time.deltaTime / 1.3f;
        dir.y = yVelocity;

        characterController.Move(dir * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 20;
        else
            moveSpeed = 11;

        if (characterController.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            if(isJumping)
                isJumping = false;
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
    }

    
}