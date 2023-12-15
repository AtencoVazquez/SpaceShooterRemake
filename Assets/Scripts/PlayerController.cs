using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private int iDHorzVelocity;

    private Rigidbody2D playerRB;
    private Transform playerTransform;
    private Transform cannonPoint;
    private Animator playerAnimator;
    [SerializeField] private GameObject laserShot;

    private float screenBoundLeft, screenBoundRight, screenBoundUp, screenBoundDown;
    private const float horzSpriteOffset = 0.7f, vertSpriteOffset = 0.75f;


    private void Awake()
    {
        playerTransform = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cannonPoint = GameObject.Find("CannonPoint").GetComponent<Transform>();

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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            LaserController tempLaserCtrl = Instantiate(laserShot, cannonPoint.transform.position, Quaternion.identity).GetComponent<LaserController>();
            tempLaserCtrl.Direction = Vector2.up;
            
        }
        
    }
}
