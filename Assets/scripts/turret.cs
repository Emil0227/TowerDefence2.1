using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    public int m_turretPrice;
    public int m_turretRotateSpeed;
    public int m_bulletDamage;
    public int m_currentTurretLevel = 1;
    public float m_turretFireRate;
    public float m_turretAttackRange;
    public float m_turretAttackRangeUpgradeFactor;
    public float m_bulletSpeed;
    
    public int TurretPrice
    {
        get => m_turretPrice;
        set => m_turretPrice = value;
    }

    public int TurretRotateSpeed
    {
        get => m_turretRotateSpeed;
        set => m_turretRotateSpeed = value;
    }

    public float TurretFireRate
    {
        get => m_turretFireRate;
        set => m_turretFireRate = value;
    }

    public float TurretAttackRange
    {
        get => m_turretAttackRange;
        set => m_turretAttackRange = value;
    }

    public float TurretAttackRangeUpgradeFactor
    {
        get => m_turretAttackRangeUpgradeFactor;
        set => m_turretAttackRangeUpgradeFactor = value;
    }

    public float BulletSpeed
    {
        get => m_bulletSpeed;
        set => m_bulletSpeed = value;
    }

    public int BulletDamage
    {
        get => m_bulletDamage;
        set => m_bulletDamage = value;
    }

    public int CurrentTurretLevel
    {
        get => m_currentTurretLevel;
        set => m_currentTurretLevel = value;
    }

    public void UpgradeTurret()
    {
        if (CurrentTurretLevel == 4)
        {
            return;
        }
        TurretAttackRange *= TurretAttackRangeUpgradeFactor;
        CurrentTurretLevel += 1;
    }
}
