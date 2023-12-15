using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpTracker : MonoBehaviour
{
    [SerializeField] private bool _isTripleShotActive, _isShieldActive, _isSpeedUpActive;

    public bool IsTripleShotActive { get => _isTripleShotActive; set => _isTripleShotActive = value; }
    public bool IsShieldActive { get => _isShieldActive; set => _isShieldActive = value; }
    public bool IsSpeedUpActive { get => _isSpeedUpActive; set => _isSpeedUpActive = value; }
}
