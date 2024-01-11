using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedUpTime;
    [SerializeField] private bool _isTripleShotActive;
    [SerializeField] private bool _enableShieldActivation;
    [SerializeField] private bool _enableSpeedUp;
    private bool isShieldActive;
    private bool enableMovement;
    private bool enableShooting;
    private float _totalShieldTime;
    //private float currentShieldTime;
    [SerializeField] private int _healthPoints;
    
    private int iDHorzVelocity;
    private int iDExplosionTrigger;

    private Rigidbody2D playerRB;
    private Transform playerTransform;
    private Animator playerAnimator;
    private CapsuleCollider2D playerCapCol;

    [SerializeField] private GameObject laserShotGO;
    private GameObject shieldGO;
    private CannonPoint[] cannonPoints;
    [SerializeField] private GameObject thrusterGO;

    private float screenBoundLeft, screenBoundRight, screenBoundUp, screenBoundDown;
    private const float horzSpriteOffset = 0.7f, vertSpriteOffset = 0.75f;

    //private GUIStyle myStyle;

    public bool IsTripleShotActive { get => _isTripleShotActive; set => _isTripleShotActive = value; }
    public bool EnableShieldActivation { get => _enableShieldActivation; set => _enableShieldActivation = value; }
    public bool EnableSpeedUp { get => _enableSpeedUp; set => _enableSpeedUp = value; }
    public float TotalShieldTime { get => _totalShieldTime; set => _totalShieldTime = value; }
    public int HealthPoints { get => _healthPoints; set => _healthPoints = value; }

    private void Awake()
    {
        playerTransform = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapCol = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cannonPoints = this.GetComponentsInChildren<CannonPoint>();
        shieldGO = this.GetComponentInChildren<ShieldController>(true).gameObject;
        screenBoundLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        screenBoundRight = Camera.main.orthographicSize * Camera.main.aspect;
        screenBoundUp = Camera.main.orthographicSize;
        screenBoundDown = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        enableMovement = true;
        enableShooting = true;

        iDHorzVelocity = Animator.StringToHash("HorzVelocity");
        iDExplosionTrigger = Animator.StringToHash("ExplosionTrigger");

        //myStyle = new GUIStyle();
        //myStyle.fontSize = 50;
        //myStyle.alignment = TextAnchor.MiddleCenter;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        ActivateShield();
    }

    private void Move()
    {
        if (enableMovement)
        {
            if (EnableSpeedUp)
            {
                moveSpeed = baseSpeed * speedMultiplier;
            }
            else
            {
                moveSpeed = baseSpeed;
            }

            float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;
            float inputY = Input.GetAxisRaw("Vertical") * moveSpeed;

            playerRB.velocity = new Vector2(inputX, inputY);
            playerAnimator.SetFloat(iDHorzVelocity, playerRB.velocity.x);

            if (playerTransform.position.x < screenBoundLeft + horzSpriteOffset)
                playerTransform.position = new Vector2(screenBoundLeft + horzSpriteOffset, playerTransform.position.y);
            else if (playerTransform.position.x > screenBoundRight - horzSpriteOffset)
                playerTransform.position = new Vector2(screenBoundRight - horzSpriteOffset, playerTransform.position.y);

            if (playerTransform.position.y < screenBoundDown + vertSpriteOffset)
                playerTransform.position = new Vector2(playerTransform.position.x, screenBoundDown + vertSpriteOffset);
            else if (playerTransform.position.y > screenBoundUp - vertSpriteOffset)
                playerTransform.position = new Vector2(playerTransform.position.x, screenBoundUp - vertSpriteOffset);
        }
        else
        {
            playerRB.velocity = Vector2.zero;
        }
    }

    private void Shoot()
    {
        if (enableShooting)
        {
            if (Input.GetMouseButtonDown(0) && !IsTripleShotActive)
            {
                Vector2 laserPosition = new Vector2(cannonPoints[0].transform.position.x, cannonPoints[0].transform.position.y + 0.7f);
                GameObject tempLaserGO = Instantiate(laserShotGO, laserPosition, Quaternion.identity);
                LaserController tempLaserCtrl = tempLaserGO.GetComponent<LaserController>();
                tempLaserCtrl.Direction = Vector2.up;
            }
            else if (Input.GetMouseButtonDown(0) && IsTripleShotActive)
            {
                for (int i = 0; i < cannonPoints.Length; i++)
                {
                    Vector2 laserPosition = new Vector2(cannonPoints[i].transform.position.x, cannonPoints[i].transform.position.y + 0.7f);
                    GameObject tempLaserGO = Instantiate(laserShotGO, laserPosition, Quaternion.identity);
                    LaserController tempLaserCtrl = tempLaserGO.GetComponent<LaserController>();
                    tempLaserCtrl.Direction = Vector2.up;
                }
            }
        }
    }

    private void ActivateShield()
    {
        if (EnableShieldActivation && !isShieldActive && Input.GetKeyDown(KeyCode.Space))
        {
            isShieldActive = true;
            shieldGO.SetActive(isShieldActive);
            
        }
        else if (EnableShieldActivation && isShieldActive && Input.GetKeyDown(KeyCode.Space))
        {
            isShieldActive = false;
            shieldGO.SetActive(isShieldActive);
        }
    }

    public void EnableShield()
    {
        if (EnableShieldActivation == false)
        {
            EnableShieldActivation = true;
            TotalShieldTime = 5;
        }
        else
        {
            TotalShieldTime += 1.5f;
        }

        StartCoroutine(ShieldActiveTime());
    }

    public void IncreaseSpeed()
    {
        StartCoroutine(SpeedUpTimer(speedUpTime));
    }

    public void ActivateTripleShot()
    {
        IsTripleShotActive = true;
    }

    IEnumerator SpeedUpTimer(float speedUpDuration)
    {
        EnableSpeedUp = true; 
        yield return new WaitForSeconds(speedUpDuration);
        EnableSpeedUp = false;
    }

    IEnumerator ShieldActiveTime()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < TotalShieldTime)
        {
            if (isShieldActive)
            {
                elapsedTime += Time.deltaTime;
                //currentShieldTime = elapsedTime;
            }
            
            yield return null;
        }

        EnableShieldActivation = false;
        TotalShieldTime = 0;
        isShieldActive = false;
        shieldGO.SetActive(isShieldActive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            Destroy(collision.gameObject);
            DamageCalculation();
        }
        else if (collision.CompareTag("Enemy"))
        {
            EnemyController tempEnemyCtrl = collision.GetComponent<EnemyController>();
            tempEnemyCtrl.DestroyEnemy();
            Destroy(this.gameObject);
        }
        
    }

    public void DamageCalculation()
    {
        if (IsTripleShotActive)
        {
            IsTripleShotActive = false;
        }

        if (!isShieldActive)
            HealthPoints--;


        if (HealthPoints <= 3 && HealthPoints > 0)
        {
            cannonPoints[HealthPoints - 1].ActivateDamage();
        }
        else if (HealthPoints <= 0)
            DestroyPlayer();
    }

    private void DestroyPlayer()
    {
        enableMovement = false;
        enableShooting = false;

        for (int i = 0; i < cannonPoints.Length; i++)
        {
            cannonPoints[i].gameObject.SetActive(false);
        }

        thrusterGO.SetActive(false);

        playerAnimator.SetTrigger(iDExplosionTrigger);
        playerCapCol.enabled = false;
        StopAllCoroutines();
        Destroy(this.gameObject, 3.2f);
    }



    //private void OnGUI()
    //{
    //    GUI.skin.button.fontSize = myStyle.fontSize;
    //    GUI.skin.button.alignment = myStyle.alignment;
    //    GUI.skin.label.fontSize = myStyle.fontSize;

    //    GUILayout.BeginHorizontal();
    //    GUI.Label(new Rect(Screen.width / 2 - 225, Screen.height / 2 - 140, 450, 60), TotalShieldTime.ToString());
    //    GUI.Label(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 70, 100, 60), currentShieldTime.ToString());

    //    GUILayout.EndHorizontal();
    //}
}
