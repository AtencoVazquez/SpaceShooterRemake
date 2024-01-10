using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;
    private Transform enemyTransform;
    private BoxCollider2D enemyBC;
    private CannonPoint[] cannonPoints;
    [SerializeField] private GameObject[] thrustersGO;
    [SerializeField] private GameObject laserPrefab;
    private GameObject shieldGO;

    private int _healthPoints;
    private int shieldEndurancePoints;
    [SerializeField] private float moveSpeed;
    private float[] tripleShotProbabilities = {0.7f, 0.3f};
    private float[] activateShieldProbabilities = { 0.2f, 0.8f };
    private int shootProbability;
    private int activateShieldProbability;
    private bool canMove;
    private bool canShoot;
    private bool canTripleShot;
    private bool canActivateShield;
    private bool isShieldActive;

    private int iDExplosionTrigger;

    public int HealthPoints { get => _healthPoints; set => _healthPoints = value; }

    private void Awake()
    {
        enemyTransform = GetComponent<Transform>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        enemyBC = GetComponent<BoxCollider2D>();

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
        shieldGO = this.GetComponentInChildren<ShieldController>(true).gameObject;
        cannonPoints = this.GetComponentsInChildren<CannonPoint>();
        iDExplosionTrigger = Animator.StringToHash("ExplosionTrigger");
        canMove = true;
        canShoot = true;
        HealthPoints = 4;
        shieldEndurancePoints = 2;
        StartCoroutine(ShootTimer());

        if (canActivateShield)
        {
            ActivateShield(canActivateShield);
        }
    }

    private void ActivateShield(bool activateShield)
    {
        shieldGO.SetActive(activateShield);
        isShieldActive = activateShield;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (canMove)
            enemyRB.velocity = new Vector2(0, -moveSpeed);
        else
            enemyRB.velocity = Vector2.zero;
    }

    private void Shoot()
    {
        if (canTripleShot)
        {
            for (int i = 0; i < cannonPoints.Length; i++)
            {
                Vector2 laserPosition = new Vector2(cannonPoints[i].transform.position.x, cannonPoints[i].transform.position.y - 0.4f);
                GameObject tempLaserGO = Instantiate(laserPrefab, laserPosition, Quaternion.identity);
                LaserController tempLaserCtrl = tempLaserGO.GetComponent<LaserController>();
                tempLaserCtrl.Direction = Vector2.down;
                tempLaserCtrl.AssignLaserToEnemy();
            }
        }
        else
        {
            Vector2 laserPosition = new Vector2(cannonPoints[0].transform.position.x, cannonPoints[0].transform.position.y - 0.4f);
            GameObject tempLaserGO = Instantiate(laserPrefab, laserPosition, Quaternion.identity);
            LaserController tempLaserCtrl = tempLaserGO.GetComponent<LaserController>();
            tempLaserCtrl.Direction = Vector2.down;
            tempLaserCtrl.AssignLaserToEnemy();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            Destroy(collision.gameObject);
            DamageCalculation();
        }
    }

    public void DamageCalculation()
    {
        if (!isShieldActive)
        {
            HealthPoints--;
        }
        else
        {
            shieldEndurancePoints--;
        }


        if (HealthPoints <= 3 && HealthPoints > 0)
        {
            cannonPoints[HealthPoints - 1].ActivateDamage();
        }
        else if (HealthPoints <= 0)
        {
            DestroyEnemy();
        }
        else if (shieldEndurancePoints <= 0)
        {
            ActivateShield(false);
        }
    }

    public void DestroyEnemy()
    {
        for (int i = 0; i < cannonPoints.Length; i++)
        {
            cannonPoints[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < thrustersGO.Length; i++)
        {
            thrustersGO[i].SetActive(false);
        }

        StopAllCoroutines();
        enemyBC.enabled = false;
        canMove = false;
        enemyAnim.SetTrigger(iDExplosionTrigger);
        Destroy(this.gameObject, 3.2f);
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(1);
        float timeDelayShooting;

        while(canShoot)
        {
            Shoot();
            timeDelayShooting = Random.Range(0.5f, 2.5f);
            yield return new WaitForSeconds(timeDelayShooting);
        }
    }
}
