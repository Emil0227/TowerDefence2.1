using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject BulletAim = null;
    public float BulletSpeed = 0;
    public int BulletDamage = 0;
    
    private ArrayList m_enemyList;

    void Start()
    {
        m_enemyList = Camera.main.GetComponent<levelManager>().EnemyList;
    }

    private void OnTriggerEnter(Collider other)
    {
        resLoad rl = Camera.main.GetComponent<resLoad>();

        //when bullet hits an enemy
        if (other.gameObject.tag == "enemy")
        {
            //set life bar change
            enemy e = other.gameObject.GetComponent<enemy>();
            e.SetHealth(e.GetHealth() - BulletDamage);
            //when enemy dies
            if (e.GetHealth() <= 0)
            {
                e.DisconnectTurret1();
                e.DisconnectTurret2();
                e.DisconnectBullet();  
                other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                m_enemyList.Remove(other.gameObject);
                Destroy(this.gameObject);
                e.SetHealth(0);
                //get gold reward
                rl.ShowGoldUI.GetComponent<showGold>().SetGold(rl.ShowGoldUI.GetComponent<showGold>().GetGold() + 15);
                rl.ShowGoldUI.GetComponent<showGold>().PlayBonusSFX();
                //effects and animations
                GameObject resParticleEliminate = Camera.main.GetComponent<resLoad>().EliminateParticleEffect;
                GameObject particleEliminate = GameObject.Instantiate(resParticleEliminate);
                particleEliminate.transform.position = transform.position;
                Animator anim = other.gameObject.GetComponent<Animator>();
                anim.SetBool("die", true);
                Destroy(other.gameObject, 1.0f);
                Destroy(particleEliminate, 1.7f);
            }
            //when enemy only gets hurt
            else
            {
                Destroy(this.gameObject);
                //effects and animations
                GameObject resParticleHit = Camera.main.GetComponent<resLoad>().HitParticleEffect;
                GameObject particleHit = GameObject.Instantiate(resParticleHit);
                particleHit.transform.position = transform.position;
                Destroy(particleHit, 1.0f);
            }
        }
        //when bullet hits terrain
        else if (other.gameObject.tag == "terrain")
        {
            if (BulletAim != null)
            {
                BulletAim.GetComponent<enemy>().RemoveBullet(this);
            }
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        //destory bullet if it goes beyond map edge
        if ((transform.position.y < 0) || (transform.position.y > 12) || 
            (transform.position.x < 0) || (transform.position.x > 100) || 
            (transform.position.z < 10) || (transform.position.z > 70))
        {
            if (BulletAim != null)
            {
                BulletAim.GetComponent<enemy>().RemoveBullet(this);
            }
            Destroy(this.gameObject);
        }
        //bullet movement
        if (gameObject.name == "bullet1(Clone)" || BulletAim == null)
        {
            transform.position += transform.forward * Time.deltaTime * BulletSpeed;
        }
        else if (gameObject.name == "bullet2(Clone)")
        {
            transform.position += transform.forward * Time.deltaTime * BulletSpeed;
            Quaternion q1 = transform.rotation;
            Vector3 destPosition = BulletAim.transform.position;
            destPosition.y += 1.5f;
            transform.LookAt(destPosition);
            Quaternion q2 = transform.rotation;
            transform.rotation = Quaternion.RotateTowards(q1, q2, Time.deltaTime * 180);
        }
    }
}

