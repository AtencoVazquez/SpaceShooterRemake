using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUps
{
    shield,
    speedUp,
    tripleShot
}

public class PowerUp : MonoBehaviour
{
    [SerializeField] private int identifier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerController tempPlayerCtrl = collision.gameObject.GetComponent<PlayerController>();

            switch (identifier)
            {
                case (int)PowerUps.shield:
                    tempPlayerCtrl.EnableShieldActivation();
                    break;
                case (int)PowerUps.speedUp:
                    tempPlayerCtrl.IncreaseSpeed();
                    break;
                case (int)PowerUps.tripleShot:
                    tempPlayerCtrl.ActivateTripleShot();
                    break;
                default:
                    break;
            }

            Destroy(this.gameObject);
        }
    }
}
