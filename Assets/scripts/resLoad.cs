using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class resLoad : MonoBehaviour
{
    public GameObject HitParticleEffect = null;
    public GameObject EliminateParticleEffect = null;
    public GameObject Bullet1 = null;
    public GameObject Bullet2 = null;
    public GameObject[] TurretList = null;
    public GameObject[] ButtonList = null;
    public GameObject UpgradeButton = null;
    public GameObject DestoryButton = null;
    public GameObject ShowGoldUI;
    public GameObject ShowLevelInfoUI;
    public GameObject Canvas;
    public int TurretCount = 0;

    void Awake()
    {
        //read configuration data from file <turretConfig.txt>
        FileInfo fi = new FileInfo(Application.dataPath + "/myLevel" + singleClass.currentLevel + "/turretConfig.txt");
        StreamReader sr = fi.OpenText();
        TurretCount = int.Parse(sr.ReadLine());
        TurretList = new GameObject[TurretCount];
        ButtonList = new GameObject[TurretCount];  
        for (int j = 0; j < TurretCount; j++)
        {
            string turretName = sr.ReadLine();
            TurretList[j] = Resources.Load<GameObject>(turretName);
            ButtonList[j] = Resources.Load<GameObject>("UI/"+ turretName+"Button");
            int turretPrice = int.Parse(sr.ReadLine());
            int turretRotateSpeed = int.Parse(sr.ReadLine());
            float turretFireRate = float.Parse(sr.ReadLine());
            float turretAttackRange = float.Parse(sr.ReadLine());
            float turretAttackRangeUpgradeFactor = float.Parse(sr.ReadLine());
            int bulletSpeed = int.Parse(sr.ReadLine());
            int bulletDamage = int.Parse(sr.ReadLine());
            turret t = null;
            if (TurretList[j].gameObject.name == "turret1")
            {
                t = TurretList[j].GetComponent<turret1>();

            }
            else if (TurretList[j].gameObject.name == "turret2")
            {
                t = TurretList[j].GetComponent<turret2>();
            }
            t.TurretPrice = turretPrice;
            t.TurretRotateSpeed = turretRotateSpeed;
            t.TurretFireRate = turretFireRate;
            t.TurretAttackRange = turretAttackRange;
            t.TurretAttackRangeUpgradeFactor = turretAttackRangeUpgradeFactor;
            t.BulletSpeed = bulletSpeed;
            t.BulletDamage = bulletDamage;
        }
        sr.Close();

        //load resources
        HitParticleEffect = Resources.Load<GameObject>("hit");
        EliminateParticleEffect = Resources.Load<GameObject>("eliminate");
        Bullet1 = Resources.Load<GameObject>("bullet1");
        Bullet2 = Resources.Load<GameObject>("bullet2");
        UpgradeButton = Resources.Load<GameObject>("UI/ButtonUpgrade");
        DestoryButton = Resources.Load<GameObject>("UI/ButtonDestory");
    }
}
