using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret1 : turret
{
    public GameObject TurretAim = null;
    
    private float m_targetDistance = 0;
    private bool m_isTurretShooting = false;
    private GameObject m_resBullet1 = null;
    private GameObject m_oldTurretAim = null;
    private Transform m_transBarrel = null;
    private ArrayList m_enemyList;
    private AudioSource m_shootingAudio;

    void Start()
    {
        m_resBullet1 = Camera.main.GetComponent<resLoad>().Bullet1;
        m_transBarrel = transform.Find("gun");
        m_enemyList = Camera.main.GetComponent<levelManager>().EnemyList;
        m_shootingAudio = gameObject.GetComponent<AudioSource>();
    }

    //release 1 bullet at one time tracking targeted enemy
    void StartShooting()
    {
        if (m_isTurretShooting == false)
        {
            InvokeRepeating("CreateBullet", 1.0f, m_turretFireRate);
            m_isTurretShooting = true;
        }
    }
    void StopShooting()
    {
        if (m_isTurretShooting == true)
        {
            CancelInvoke("CreateBullet");
            m_isTurretShooting = false;
        }
    }

    void CreateBullet()
    {
        m_shootingAudio.Play();
        GameObject bullet1 = GameObject.Instantiate(m_resBullet1);
        bullet1.GetComponent<bullet>().BulletSpeed = BulletSpeed;
        bullet1.GetComponent<bullet>().BulletDamage = BulletDamage;
        bullet1.transform.position = m_transBarrel.position;
        bullet1.transform.rotation = m_transBarrel.rotation;
    }

    void Update()
    {
        //find closest target
        TurretAim = null;
        m_targetDistance = 0.0f;
        foreach (GameObject r in m_enemyList)
        {
            float d = Vector3.Distance(r.transform.position, transform.position);
            if (TurretAim == null)
            {
                TurretAim = r;
                m_targetDistance = d;
            }
            else
            {
                if (m_targetDistance > d)
                {
                    TurretAim = r;
                    m_targetDistance = d;
                }
            }
        }
        //aim at target and shoot
        if (TurretAim != null && m_targetDistance > 0 && m_targetDistance < m_turretAttackRange)
        {
            //put turret in target's turret list
            if (TurretAim != m_oldTurretAim)
            {
                m_oldTurretAim = TurretAim;
                m_oldTurretAim.GetComponent<enemy>().AddTurret1(this);
            }
            float currentAngleY = m_transBarrel.eulerAngles.y;
            float currentAngleX = m_transBarrel.eulerAngles.x;
            m_transBarrel.LookAt(TurretAim.transform);
            float destAngleY = m_transBarrel.eulerAngles.y;
            float destAngleX = m_transBarrel.eulerAngles.x-10;
            float angleY = Mathf.MoveTowardsAngle(currentAngleY, destAngleY, Time.deltaTime * m_turretRotateSpeed);
            float angleX = Mathf.MoveTowardsAngle(currentAngleX, destAngleX, Time.deltaTime * m_turretRotateSpeed);
            m_transBarrel.eulerAngles = new Vector3(angleX, angleY, 0);
            StartShooting();
        }
        else
        {
            StopShooting();
        }
    }
}
