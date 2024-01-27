using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class button : MonoBehaviour, IPointerClickHandler
{
    public string ButtonName;
    public bool IsButtonDisabled = false;
    public turretBase TurretBase;
    
    private GameObject m_turret = null;
    private AudioSource m_buttonSfx = null;

    void Awake()
    {
        Texture2D tex = Resources.Load<Texture2D>(ButtonName);
        Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
        gameObject.GetComponent<Button>().image.sprite = spr;
        m_buttonSfx = gameObject.GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_buttonSfx.Play();
        resLoad rl = Camera.main.GetComponent<resLoad>();
        //do not respond to click when a button is disabled
        if (IsButtonDisabled == true)
        {
            return;
        }
        //click button to create a turret
        if (gameObject.name == "turret1Button(Clone)")
        {
            m_turret = GameObject.Instantiate(rl.TurretList[0]);
            m_turret.transform.position = TurretBase.transform.position;
            TurretBase.DestoryAllButton();
            TurretBase.IsHasTurret = m_turret;
            rl.ShowGoldUI.GetComponent<showGold>().SetGold
                (rl.ShowGoldUI.GetComponent<showGold>().GetGold() - m_turret.GetComponent<turret1>().TurretPrice);
            Camera.main.GetComponent<levelManager>().TurretList.Add(m_turret);
        }
        else if (gameObject.name == "turret2Button(Clone)")
        {
            m_turret = GameObject.Instantiate(rl.TurretList[1]);
            m_turret.transform.position = TurretBase.transform.position;
            TurretBase.DestoryAllButton();
            TurretBase.IsHasTurret = m_turret;
            rl.ShowGoldUI.GetComponent<showGold>().SetGold
                (rl.ShowGoldUI.GetComponent<showGold>().GetGold() - m_turret.GetComponent<turret2>().TurretPrice);
            Camera.main.GetComponent<levelManager>().TurretList.Add(m_turret);
        }
        //click button to upgrade a turret
        else if (gameObject.name == "ButtonUpgrade(Clone)")
        {
            if (TurretBase.IsHasTurret.gameObject.name == "turret1(Clone)")
            {
                turret1 t = TurretBase.IsHasTurret.GetComponent<turret1>();
                if (t.CurrentTurretLevel < 4)
                {
                    t.UpgradeTurret();
                    TurretBase.SetRange();
                    rl.ShowGoldUI.GetComponent<showGold>().SetGold
                        (rl.ShowGoldUI.GetComponent<showGold>().GetGold() - 20);
                }
                if (t.CurrentTurretLevel == 4)
                {
                    DisableButton();
                }
                else if(rl.ShowGoldUI.GetComponent<showGold>().GetGold() < 20)
                {
                    DisableButton();
                }
            }
            else if(TurretBase.IsHasTurret.gameObject.name == "turret2(Clone)")
            {
                turret2 t = TurretBase.IsHasTurret.GetComponent<turret2>();
                if (t.CurrentTurretLevel < 4)
                {
                    t.UpgradeTurret();
                    TurretBase.SetRange();
                    rl.ShowGoldUI.GetComponent<showGold>().SetGold
                        (rl.ShowGoldUI.GetComponent<showGold>().GetGold() - 20);
                }
                if (t.CurrentTurretLevel == 4)
                {
                    DisableButton();
                }
                else if (rl.ShowGoldUI.GetComponent<showGold>().GetGold() < 20)
                {
                    DisableButton();
                }
            }
        }
        //click button to destory a turret
        else if (gameObject.name == "ButtonDestory(Clone)")
        {
            Destroy(TurretBase.IsHasTurret);
            TurretBase.IsHasTurret = null;
            TurretBase.DestoryAllButton();
        }
    }

    public void DisableButton()
    {
        IsButtonDisabled = true;
        ButtonName += "Disable";
        Destroy(gameObject.GetComponent<Button>().GetComponent<Image>().sprite);
        Texture2D tex = Resources.Load<Texture2D>(ButtonName);
        Sprite spr = Sprite.Create
            (tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
        gameObject.GetComponent<Button>().image.sprite = spr;
    }
}

