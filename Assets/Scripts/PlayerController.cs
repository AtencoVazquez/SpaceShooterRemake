using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedUpTime;
    [SerializeField] private bool _isTripleShotActive;
    [SerializeField] private bool _enableShieldActivation;
    [SerializeField] private bool _isSpeedUpActive;
    private bool isShieldActive;
    [SerializeField] private float _totalShieldTime;
    private float currentShieldTime;
    
    private int iDHorzVelocity;

    private Rigidbody2D playerRB;
    private Transform playerTransform;
    private Animator playerAnimator;
    [SerializeField] private GameObject laserShotGO;
    private GameObject shieldGO;
    private CannonPoint[] cannonPoints;

    private float screenBoundLeft, screenBoundRight, screenBoundUp, screenBoundDown;
    private const float horzSpriteOffset = 0.7f, vertSpriteOffset = 0.75f;

    private GUIStyle myStyle;

    public bool IsTripleShotActive { get => _isTripleShotActive; set => _isTripleShotActive = value; }
    public bool EnableShieldActivation { get => _enableShieldActivation; set => _enableShieldActivation = value; }
    public bool IsSpeedUpActive { get => _isSpeedUpActive; set => _isSpeedUpActive = value; }
    public float TotalShieldTime { get => _totalShieldTime; set => _totalShieldTime = value; }

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
        shieldGO = this.GetComponentInChildren<ShieldController>(true).gameObject;
        screenBoundLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        screenBoundRight = Camera.main.orthographicSize * Camera.main.aspect;
        screenBoundUp = Camera.main.orthographicSize;
        screenBoundDown = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        iDHorzVelocity = Animator.StringToHash("HorzVelocity");

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
            LaserController tempLaserCtrl = Instantiate(laserShotGO, cannonPoints[0].transform.position, Quaternion.identity).GetComponent<LaserController>();
            tempLaserCtrl.Direction = Vector2.up;
        }
        else if (Input.GetMouseButtonDown(0) && IsTripleShotActive)
        {
            for (int i = 0; i < cannonPoints.Length; i++)
            {
                LaserController tempLaserCtrl = Instantiate(laserShotGO, cannonPoints[i].transform.position, Quaternion.identity).GetComponent<LaserController>();
                tempLaserCtrl.Direction = Vector2.up;
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

    IEnumerator ShieldActiveTime()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < TotalShieldTime)
        {
            if (isShieldActive)
            {
                elapsedTime += Time.deltaTime;
                currentShieldTime = elapsedTime;
            }
            
            yield return null;
        }

        EnableShieldActivation = false;
        TotalShieldTime = 0;
        isShieldActive = false;
        shieldGO.SetActive(isShieldActive);
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
