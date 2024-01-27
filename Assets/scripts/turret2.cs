using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret2 : turret
{
    public GameObject TurretAim = null;

    private int m_bulletCount = 0;
    private float m_targetDistance = 0;
    private bool m_isTurretShooting = false;
    private GameObject m_resBullet2 = null;
    private GameObject m_oldTurretAim = null;
    private Transform m_transBarrel = null;
    private Transform m_transRotate = null;
    private ArrayList m_enemyList;
    private AudioSource m_shootingAudio;

    void Start()
    {
        m_resBullet2 = Camera.main.GetComponent<resLoad>().Bullet2;
        m_transBarrel = transform.Find("base").Find("rotation").Find("arm").Find("gunBody").Find("barrel");
        m_transRotate = transform.Find("base").Find("rotation");
        m_enemyList = Camera.main.GetComponent<levelManager>().EnemyList;
        m_shootingAudio = gameObject.GetComponent<AudioSource>();
    }

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

    //randomly release 3 bullets at one time tracking targeted enemy
    void CreateBullet()
    {
        InvokeRepeating("CreateScattershot", 0.0f, 0.1f);
    }
    void CreateScattershot()
    {
        m_shootingAudio.Play();
        GameObject bullet2 = GameObject.Instantiate(m_resBullet2);
        if (TurretAim != null)
        {
            bullet2.GetComponent<bullet>().BulletAim = TurretAim;
            TurretAim.GetComponent<enemy>().AddBullet(bullet2.GetComponent<bullet>());
        }
        bullet2.GetComponent<bullet>().BulletSpeed = BulletSpeed;
        bullet2.GetComponent<bullet>().BulletDamage = BulletDamage;
        bullet2.transform.position = m_transBarrel.position;
        bullet2.transform.eulerAngles = new Vector3(m_transBarrel.transform.eulerAngles.x, 
            m_transBarrel.transform.eulerAngles.y + Random.Range(-15, 15), 
            m_transBarrel.transform.eulerAngles.z);
        m_bulletCount++;
        if (m_bulletCount == 3)
        {
            m_bulletCount = 0;
            CancelInvoke("CreateScattershot");
        }
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
                m_oldTurretAim.GetComponent<enemy>().AddTurret2(this);
            }
            float currentAngleY = m_transRotate.eulerAngles.y;
            float currentAngleX = m_transRotate.eulerAngles.x;
            m_transRotate.LookAt(TurretAim.transform);
            float destAngleY = m_transRotate.eulerAngles.y;
            float destAngleX = m_transRotate.eulerAngles.x;
            float angleY = Mathf.MoveTowardsAngle(currentAngleY, destAngleY, Time.deltaTime * m_turretRotateSpeed);
            float angleX = Mathf.MoveTowardsAngle(currentAngleX, destAngleX, Time.deltaTime * m_turretRotateSpeed);
            m_transRotate.eulerAngles = new Vector3(angleX, angleY, 0);
            StartShooting();
        }
        else
        {
            StopShooting();
        }
    }
}