using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class turretBase : MonoBehaviour
{
    public GameObject IsHasTurret = null;
    
    private GameObject m_canvas = null;
    private GameObject m_attackRangeBall = null;
    private GameObject[] m_buttonArray;
    private int m_buttonCount = 0;
    private float m_turretAttackRange;
    private bool m_isButtonPoped = false;
    private bool m_isMouseEnter = false;
    private resLoad m_rl;
    private AudioSource m_PopSfx;

    void Start()
    {
        m_rl = Camera.main.GetComponent<resLoad>();
        m_canvas = m_rl.Canvas;
        m_attackRangeBall = GameObject.Find("attackRange");
        m_buttonCount = Camera.main.GetComponent<resLoad>().TurretCount;
        Camera.main.GetComponent<levelManager>().TurretBaseList.Add(this.gameObject);
        m_PopSfx = gameObject.GetComponent<AudioSource>();
    }

    public void DestoryAllButton()
    {
        for (int j = 0; j < 2; j++)
        {
            GameObject.Destroy(m_buttonArray[j]);
            m_isButtonPoped = false;
        }
        m_attackRangeBall.transform.position = new Vector3(0, -1000, 0);
    }

    //show attack range ball
    public void SetRange()
    {
        m_attackRangeBall.transform.position = IsHasTurret.transform.position;
        if (IsHasTurret.gameObject.name == "turret1(Clone)")
        {
            m_turretAttackRange = IsHasTurret.GetComponent<turret1>().TurretAttackRange;
        }
        else if (IsHasTurret.gameObject.name == "turret2(Clone)")
        {
            m_turretAttackRange = IsHasTurret.GetComponent<turret2>().TurretAttackRange;
        }
        m_attackRangeBall.transform.localScale = new Vector3
            (m_turretAttackRange * 2, m_turretAttackRange * 2, m_turretAttackRange * 2);
    }

    void OnMouseDown()
    {
        if (m_isButtonPoped)
        {
            return;
        }
        m_isButtonPoped = true;

        //click base to show create button
        if (IsHasTurret == null) 
        {
            m_PopSfx.Play();
            m_buttonArray = new GameObject[m_buttonCount];
            for (int j = 0; j < m_buttonCount; j++)
            {
                m_buttonArray[j] = GameObject.Instantiate(m_rl.ButtonList[j]);
            }
            //check if there are enough gold
            if (m_rl.ShowGoldUI.GetComponent<showGold>().GetGold() < 
                m_rl.TurretList[0].GetComponent<turret1>().TurretPrice)
            {
                m_buttonArray[0].GetComponent<button>().DisableButton();
            }
            if (m_rl.ShowGoldUI.GetComponent<showGold>().GetGold() < 
                m_rl.TurretList[1].GetComponent<turret2>().TurretPrice)
            {
                m_buttonArray[1].GetComponent<button>().DisableButton();
            }
            SetButtonPosition(m_buttonCount);
        }
        //click base to show upgrade/destory button and attack range ball
        else
        {
            m_PopSfx.Play();
            m_buttonArray = new GameObject[2];
            m_buttonArray[0] = GameObject.Instantiate(m_rl.UpgradeButton);
            m_buttonArray[1] = GameObject.Instantiate(m_rl.DestoryButton);
            SetButtonPosition(2);
            SetRange();
            //check if there are enough golds for upgrade
            if (IsHasTurret.gameObject.name == "turret1(Clone)")
            {
                turret1 t = IsHasTurret.GetComponent<turret1>();
                if (t.CurrentTurretLevel == 4)
                {
                    m_buttonArray[0].GetComponent<button>().DisableButton();
                }
                else if (m_rl.ShowGoldUI.GetComponent<showGold>().GetGold() < 20)
                {
                    m_buttonArray[0].GetComponent<button>().DisableButton();
                }
            }
            if (IsHasTurret.gameObject.name == "turret2(Clone)")
            {
                turret2 t = IsHasTurret.GetComponent<turret2>();
                if (t.CurrentTurretLevel == 4)
                {
                    m_buttonArray[0].GetComponent<button>().DisableButton();
                }
                else if (m_rl.ShowGoldUI.GetComponent<showGold>().GetGold() < 20)
                {
                    m_buttonArray[0].GetComponent<button>().DisableButton();
                }
            }
        }
    }

    //set button positions
    void SetButtonPosition(int count)
    {
        float width = m_buttonArray[0].GetComponent<RectTransform>().rect.width;
        float height = m_buttonArray[0].GetComponent<RectTransform>().rect.height;
        float startX = Input.mousePosition.x - (width * 2 + 10) / 2 + width / 2.0f;
        for (int j = 0; j < count; j++)
        {
            m_buttonArray[j].GetComponent<button>().TurretBase = this;
            m_buttonArray[j].transform.SetParent(m_canvas.transform);
            m_buttonArray[j].transform.position = new Vector3(startX + (width+10)* j, Input.mousePosition.y+30, 0);
        }
    }
    void OnMouseEnter()
    {
        m_isMouseEnter = true;
    }
    void OnMouseExit()
    {
        m_isMouseEnter = false;
    }

    void Update()
    {
        //click to destory buttons
        if (!EventSystem.current.IsPointerOverGameObject() && 
            m_isButtonPoped && m_isMouseEnter == false && Input.GetMouseButton(0) == true)
        {
            DestoryAllButton();
        }
    }
}

