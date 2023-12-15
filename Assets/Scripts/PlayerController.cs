using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedUpTime;
    [SerializeField] private bool _isTripleShotActive;
    [SerializeField] private bool _isShieldActive;
    [SerializeField] private bool _isSpeedUpActive;
    private int iDHorzVelocity;

    private Rigidbody2D playerRB;
    private Transform playerTransform;
    private Animator playerAnimator;
    [SerializeField] private GameObject laserShot;
    [SerializeField] private GameObject shield;
    private CannonPoint[] cannonPoints;

    private float screenBoundLeft, screenBoundRight, screenBoundUp, screenBoundDown;
    private const float horzSpriteOffset = 0.7f, vertSpriteOffset = 0.75f;

    public bool IsTripleShotActive { get => _isTripleShotActive; set => _isTripleShotActive = value; }
    public bool IsShieldActive { get => _isShieldActive; set => _isShieldActive = value; }
    public bool IsSpeedUpActive { get => _isSpeedUpActive; set => _isSpeedUpActive = value; }

    private void Awake()
    {
        moveSpeed = baseSpeed;
        playerTransform = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cannonPoints = this.GetComponentsInChildren<CannonPoint>();
        screenBoundLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        screenBoundRight = Camera.main.orthographicSize * Camera.main.aspect;
        screenBoundUp = Camera.main.orthographicSize;
        screenBoundDown = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        iDHorzVelocity = Animator.StringToHash("HorzVelocity");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        //ActivateShield();
    }

    private void Move()
    {
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

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && !IsTripleShotActive)
        {
            LaserController tempLaserCtrl = Instantiate(laserShot, cannonPoints[0].transform.position, Quaternion.identity).GetComponent<LaserController>();
            tempLaserCtrl.Direction = Vector2.up;
        }
        else if (Input.GetMouseButtonDown(0) && IsTripleShotActive)
        {
            for (int i = 0; i < cannonPoints.Length; i++)
            {
                LaserController tempLaserCtrl = Instantiate(laserShot, cannonPoints[i].transform.position, Quaternion.identity).GetComponent<LaserController>();
                tempLaserCtrl.Direction = Vector2.up;
            }
        }
    }

    //private void ActivateShield()
    //{
    //    if (IsShieldActive && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Transform shieldTransform = Instantiate(shield, playerTransform.position, Quaternion.identity).GetComponent<Transform>();
    //        shieldTransform.parent = playerTransform;

    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            IsShieldActive = false;
    //        }
    //    }
        
    //}

    public void EnableShieldActivation()
    {
        IsShieldActive = true;
    }
    public void IncreaseSpeed()
    {
        IsSpeedUpActive = true;
        StartCoroutine(SpeedUpTimer(speedUpTime));
    }

    public void ActivateTripleShot()
    {
        IsTripleShotActive = true;
    }

    IEnumerator SpeedUpTimer(float speedUpDuration)
    {
        moveSpeed = baseSpeed * speedMultiplier;
        yield return new WaitForSeconds(speedUpDuration);
        moveSpeed = baseSpeed;
        IsSpeedUpActive = false;
    }
}
