using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPoint : MonoBehaviour
{
    private Transform damageGO;

    private void Start()
    {
        damageGO = this.transform.GetChild(0);
        damageGO.gameObject.SetActive(false);
    }

    public void ActivateDamage()
    {
        damageGO.gameObject.SetActive(true);
    }
}
