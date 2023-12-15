using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 _direction;
    private Transform laserTransform;
    private Rigidbody2D laserRB;

    public Vector2 Direction { get => _direction; set => _direction = value; }

    private void Awake()
    {
        laserTransform = GetComponent<Transform>();
        laserRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        laserRB.velocity = Direction * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
