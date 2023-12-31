using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

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

    public GameObject muzzle;

    public GameObject gunSlot;
    [SerializeField] private GameObject currentWeapon;
    private GunProperty currentGunProperty;

    [SerializeField] private float currentFireDelay;
    public float currentDamage;

    public ParticleSystem muzzleFlashParticle;
    public ParticleSystem bulletShellParticle;

    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI DamageText;

    
    public GameObject floatingDamage;


    public TextMeshProUGUI gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        characterController = GetComponent<CharacterController>();
        currentWeapon = gunSlot.transform.GetChild(0).GetComponent<Transform>().gameObject;
        currentGunProperty = currentWeapon.GetComponent<GunProperty>();

        currentFireDelay = currentGunProperty.delay;
        currentDamage = currentGunProperty.damage;
    }

    float gravity = -3.8f;
    float yVelocity = 0;
    private Camera _camera;

    private bool isDeath = false;

    // Update is called once per frame
    void Update()
    {
        Movement();
        Fire();
        GoldUpgrade();

        if (GameManager.Instance.gold <= 0 && !isDeath)
        {
            isDeath = true;
            StartCoroutine(DeathText());
        }
            
    }
    IEnumerator DeathText(){
        
        gameOverText.DOText("Game Over", 4f);
        yield return new WaitForSeconds(4f);
        Time.timeScale = 0.005f;
    }

    void GoldUpgrade()
    {
        currentFireDelay = Mathf.Clamp(currentGunProperty.delay / (GameManager.Instance.gold * 0.005f), 0.001f, 0.15f);
        currentDamage = Mathf.Clamp(currentGunProperty.damage + GameManager.Instance.gold * 0.001f, 1f, 10f);

        Debug.Log("currentFireDelay = " + currentFireDelay);
        fireRateText.text = "CurrentFireDelay\n" + currentFireDelay.ToString("F3");
        DamageText.text = "CurrentDamage\n" + currentDamage.ToString("F2");

        if (currentFireDelay < 0.025f)
        {
            var emissionModule = bulletShellParticle.emission;
            emissionModule.rateOverTime = 12;
        }
        else if (currentFireDelay < 0.03f)
        {
            var emissionModule = bulletShellParticle.emission;
            emissionModule.rateOverTime = 8;
            
        }
        else if (currentFireDelay < 0.035f)
        {
            var emissionModule = bulletShellParticle.emission;
            emissionModule.rateOverTime = 6;
            
        }
        else if (currentFireDelay < 0.04f)
        {
            var emissionModule = bulletShellParticle.emission;
            emissionModule.rateOverTime = 4;
            
        }
        else
        {
            var emissionModule = bulletShellParticle.emission;
            emissionModule.rateOverTime = 2;
        }
    }

    void Fire()
    {
        if (delayTimer <= currentFireDelay)
        {
            delayTimer += Time.deltaTime;
        }


        if (Input.GetMouseButton(0) && delayTimer >= currentFireDelay && GameManager.Instance.gold >= 0)
        {
            delayTimer = 0f;


            GameManager.Instance.gold -= currentGunProperty.cost;

            RecoilAnim();

            RaycastHit hit;
            // Create a ray that goes through the center of the screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out hit, range))
            {
                Instantiate(floatingDamage, hit.point, Quaternion.identity);
                Instantiate(temp_Particle, hit.point, Quaternion.identity);
                if (hit.transform.CompareTag("Enemy"))
                {
                    Health health = hit.transform.GetComponent<Health>();
                    if (health != null)
                        health.Damage(currentDamage);
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (GameManager.Instance.gold >= 0)
            {
                var bulletModule = bulletShellParticle.emission;
                bulletModule.enabled = true;

                var flashModule = muzzleFlashParticle.emission;
                flashModule.enabled = true;
            }
            else
            {
                var bulletModule = bulletShellParticle.emission;
                bulletModule.enabled = false;

                var flashModule = muzzleFlashParticle.emission;
                flashModule.enabled = false;
            }
        }
        else
        {
            var bulletModule = bulletShellParticle.emission;
            bulletModule.enabled = false;

            var flashModule = muzzleFlashParticle.emission;
            flashModule.enabled = false;
        }
    }

    void RecoilAnim()
    {
        currentWeapon.transform
            .DOLocalMoveZ(currentWeapon.transform.localPosition.z - currentGunProperty.recoilDistance,
                currentFireDelay * 0.5f, false).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo);
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