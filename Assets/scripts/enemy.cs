using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    private ArrayList m_turret1List;
    private ArrayList m_turret2List;
    private ArrayList m_bulletList;
    private GameObject m_path = null;
    private Transform m_nextTrans = null;
    private Vector3 m_vFullHealth;
    private float m_enemySpeed = 0;
    private int m_initialHealthValue = 0;
    private int m_currentHealthValue = 0;

    void Start()
    {
        m_turret1List = new ArrayList();
        m_turret2List = new ArrayList();
        m_bulletList = new ArrayList();
        m_vFullHealth = gameObject.transform.Find("lifeBar").localScale;
    }

    //add all turret1 targeting at the enemy
    public void AddTurret1(turret1 t)
    {
        m_turret1List.Add(t);
    }

    //add all turret2 targeting at the enemy
    public void AddTurret2(turret2 t)
    {
        m_turret2List.Add(t);
    }

    //add all bullets targeting at the enemy
    public void AddBullet(bullet b)
    {
        m_bulletList.Add(b);
    }

    //remove turret1 from the list
    public void RemoveTurret1(turret1 t)
    {
        m_turret1List.Remove(t);
    }

    //remove turret2 from the list
    public void RemoveTurret2(turret2 t)
    {
        m_turret2List.Remove(t);
    }

    //delete bullet from the list
    public void RemoveBullet(bullet b)
    {
        m_bulletList.Remove(b);
    }

    //disconnect all turret1 targeting at the enemy
    public void DisconnectTurret1()
    {
        foreach (turret1 t in m_turret1List)
        {
            t.TurretAim = null;
        }
    }

    //disconnect all turret2 targeting at the enemy
    public void DisconnectTurret2()
    {
        foreach (turret2 t in m_turret2List)
        {
            t.TurretAim = null;
        }
    }

    //disconnect all bullets targeting at the enemy
    public void DisconnectBullet()
    {
        foreach (bullet b in m_bulletList)
        {
            b.BulletAim = null;
        }
    }

    //set health bar length
    public void SetHealth(int h)
    {
        m_currentHealthValue = h;
        float percentage = m_currentHealthValue * 1.0f / m_initialHealthValue;
        gameObject.transform.Find("lifeBar").localScale = new Vector3
            (m_vFullHealth.x * percentage, m_vFullHealth.y, m_vFullHealth.z);
    }
    public int GetHealth()
    {
        return m_currentHealthValue;
    }

    //set enemy path, speed, health
    public void InitEnemy(string path, float speed, int health)
    {
        m_initialHealthValue = health;
        m_currentHealthValue = health;
        m_path = GameObject.Find(path);
        m_enemySpeed = speed;
        transform.position = m_path.transform.position;
        m_nextTrans = m_path.transform.Find("path");
        transform.LookAt(m_nextTrans);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void Update()
    {
        //enemy pathfollow
        if (m_nextTrans != null)
        {
            float distance = Vector3.Distance(transform.position, m_nextTrans.position);
            if (distance > Time.deltaTime * m_enemySpeed)
            {
                transform.position += transform.forward * Time.deltaTime * m_enemySpeed;
                float currentAngle = transform.eulerAngles.y;
                transform.LookAt(m_nextTrans);
                float destAngle = transform.eulerAngles.y;
                float angle = Mathf.MoveTowardsAngle(currentAngle, destAngle, Time.deltaTime * 400);
                transform.eulerAngles = new Vector3(0, angle, 0);
            }
            else
            {
                transform.position = m_nextTrans.position;
                m_nextTrans = m_nextTrans.Find("path");
                if (m_nextTrans == null)
                {
                    //set UI for game over
                    if (Camera.main.GetComponent<gameState>().GameState != 1)
                    {
                        Camera.main.GetComponent<gameState>().GameState = 1;
                        GameObject showLevelInfo = Camera.main.GetComponent<resLoad>().ShowLevelInfoUI;
                        GameObject canvas = Camera.main.GetComponent<resLoad>().Canvas;
                        showLevelInfo.SetActive(true);
                        canvas.GetComponent<Animator>().SetBool("showInfo", true);
                        showLevelInfo.GetComponent<showLevelInfo>().SetTitle("Game Over");
                        showLevelInfo.GetComponent<showLevelInfo>().SetButtonText("Try Again");
                    }
                }
            }
        }
    }
}


