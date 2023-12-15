using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D enemyRB;
    private Transform enemyTransform;
    private CannonPoint[] cannonPoints;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject shield;

    [SerializeField] private float moveSpeed;
    private float[] tripleShotProbabilities = {0.7f, 0.3f};
    private float[] activateShieldProbabilities = { 0.8f, 0.2f };
    private int shootProbability;
    private int activateShieldProbability;
    private bool canShoot;
    private bool canTripleShot;
    private bool canActivateShield;

    private void Awake()
    {
        enemyTransform = GetComponent<Transform>();
        enemyRB = GetComponent<Rigidbody2D>();
        shootProbability = (int)RandomValueProbabilities.GenerateValueWithProbabilities(tripleShotProbabilities);
        activateShieldProbability = (int)RandomValueProbabilities.GenerateValueWithProbabilities(activateShieldProbabilities);

        if (shootProbability == 1)
            canTripleShot = false;
        else
            canTripleShot = true;

        if (activateShieldProbability == 1)
            canActivateShield = false;
        else
            canActivateShield = true;

    }

    private void Start()
    {
        cannonPoints = this.GetComponentsInChildren<CannonPoint>();
        canShoot = true;
        StartCoroutine(ShootTimer());

        if (canActivateShield)
        {
            ActivateShield();
        }
    }

    private void ActivateShield()
    {
        Transform shieldTransform = Instantiate(shield, enemyTransform.position, Quaternion.identity).GetComponent<Transform>();
        shieldTransform.parent = enemyTransform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        enemyRB.velocity = new Vector2(0, -moveSpeed);
    }

    private void Shoot()
    {
        if (canTripleShot)
        {
            for (int i = 0; i < cannonPoints.Length; i++)
            {
                LaserController tempLaserCtrl = Instantiate(laserPrefab, cannonPoints[i].transform.position, Quaternion.identity).GetComponent<LaserController>();
                tempLaserCtrl.Direction = Vector2.down;
            }
        }
        else
        {
            LaserController tempLaserCtrl = Instantiate(laserPrefab, cannonPoints[0].transform.position, Quaternion.identity).GetComponent<LaserController>();
            tempLaserCtrl.Direction = Vector2.down;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(2.5f);
        float timeDelayShooting;

        while(canShoot)
        {
            Shoot();
            timeDelayShooting = Random.Range(1, 4);
            yield return new WaitForSeconds(timeDelayShooting);
        }
    }
}
