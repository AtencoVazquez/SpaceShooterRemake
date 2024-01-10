using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool _isEnemyLaser = false;
    private Vector2 _direction;
    private Transform laserTransform;
    private Rigidbody2D laserRB;

    public Vector2 Direction { get => _direction; set => _direction = value; }
    public bool IsEnemyLaser { get => _isEnemyLaser; set => _isEnemyLaser = value; }

    private void Awake()
    {
        laserTransform = GetComponent<Transform>();
        laserRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        laserRB.velocity = Direction * speed;
    }

    public void AssignLaserToEnemy()
    {
        IsEnemyLaser = true;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            Destroy(this.gameObject);
        }
    }
}
