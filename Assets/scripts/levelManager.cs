using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    public GameObject ShowWaveUI;
    public ArrayList EnemyList = null;
    public ArrayList TurretList = null;
    public ArrayList TurretBaseList = null;

    private ArrayList m_enemyInfoList;
    private int m_spawningBatches = 0;
    private int m_enemyIndex = 0;

    void Awake()
    {
        EnemyList = new ArrayList();
        TurretList = new ArrayList();
        TurretBaseList = new ArrayList();
        m_enemyInfoList = new ArrayList();
    }

    void Start()
    {
        //read data from file <enemyConfig.txt>
        FileInfo fi = new FileInfo(Application.dataPath + 
            "/myLevel" + singleClass.currentLevel + "/enemyConfig.txt");
        StreamReader sr = fi.OpenText();
        m_spawningBatches = int.Parse(sr.ReadLine());
        for (int i = 0; i < m_spawningBatches; i++)
        {
            EnemyInfo roleInfo = new EnemyInfo();
            roleInfo.EnemyObj = Resources.Load<GameObject>(sr.ReadLine());
            roleInfo.EnemySpawningStartTime = float.Parse(sr.ReadLine());
            roleInfo.EnemyCount = int.Parse(sr.ReadLine());
            roleInfo.EnemySpawningInterval = float.Parse(sr.ReadLine());
            roleInfo.EnemyMovingSpeed = float.Parse(sr.ReadLine());
            roleInfo.EnemyHealth = int.Parse(sr.ReadLine());
            m_enemyInfoList.Add(roleInfo);
        }
        sr.Close();
        EnemyInfo temp = (EnemyInfo)m_enemyInfoList[m_enemyIndex];
        StartCoroutine(Wait(temp.EnemySpawningStartTime, temp.EnemySpawningInterval, temp.EnemyCount));
    }

    //instantiate enemies
    IEnumerator Wait(float startTime, float interval, int roleCount)
    {
        yield return new WaitForSeconds(startTime);
        ShowWaveUI.GetComponent<showWave>().SetWaveText("Wave " + (m_enemyIndex + 1) + "/" + m_spawningBatches);
        for (int j = 0; j < roleCount; j++)
        {
            Create();
            yield return new WaitForSeconds(interval);
        }
        m_enemyIndex += 1;
        if (m_enemyIndex < m_spawningBatches)
        {
            EnemyInfo temp = (EnemyInfo)m_enemyInfoList[m_enemyIndex];
            StartCoroutine(Wait(temp.EnemySpawningStartTime, temp.EnemySpawningInterval, temp.EnemyCount));
        }
    }
    void Create()
    {
        switch (singleClass.currentLevel)
        {
            case 1:
                EnemyInfo temp = (EnemyInfo)m_enemyInfoList[m_enemyIndex];
                GameObject obj = GameObject.Instantiate(temp.EnemyObj);
                obj.GetComponent<enemy>().InitEnemy("path1", temp.EnemyMovingSpeed, temp.EnemyHealth);
                EnemyList.Add(obj);
                break;
            case 2:
                EnemyInfo temp1 = (EnemyInfo)m_enemyInfoList[m_enemyIndex];
                GameObject obj1 = GameObject.Instantiate(temp1.EnemyObj);
                obj1.GetComponent<enemy>().InitEnemy("path1", temp1.EnemyMovingSpeed, temp1.EnemyHealth);
                EnemyInfo temp2 = (EnemyInfo)m_enemyInfoList[m_enemyIndex];
                GameObject obj2 = GameObject.Instantiate(temp2.EnemyObj);
                obj2.GetComponent<enemy>().InitEnemy("path2", temp2.EnemyMovingSpeed, temp1.EnemyHealth);
                EnemyList.Add(obj1);
                EnemyList.Add(obj2);
                break;
        }
    }

    //check if level is passed
    void CheckWin()
    {
        bool isSpawningComplete = true;
        if (m_enemyIndex < m_spawningBatches)
        {
            isSpawningComplete = false;
        }
        if (isSpawningComplete == true)
        {
            if (EnemyList.Count == 0)//level passed
            {
                Camera.main.GetComponent<gameState>().GameState = 1;
                //set UI
                GameObject showLevelInfo = Camera.main.GetComponent<resLoad>().ShowLevelInfoUI;
                GameObject canvas = Camera.main.GetComponent<resLoad>().Canvas;
                showLevelInfo.SetActive(true);
                canvas.GetComponent<Animator>().SetBool("showInfo", true);
                showLevelInfo.GetComponent<showLevelInfo>().SetTitle("You Win!");
                showLevelInfo.GetComponent<showLevelInfo>().SetButtonText("Next Level");
            }
        }
    }

    //reset level
    public void ResetLevel()
    {
        StopAllCoroutines();
        m_enemyIndex = 0;
        //clear enemy list
        foreach (GameObject obj in EnemyList)
        {
            Destroy(obj);
        }
        EnemyList.Clear();
        //clear turret list
        foreach (GameObject obj in TurretList)
        {
            Destroy(obj);
        }
        TurretList.Clear();
        //clear turret base list
        foreach (GameObject obj in TurretBaseList)
        {
            obj.GetComponent<turretBase>().IsHasTurret = null;
        }
        TurretBaseList.Clear();
        Start();
    }
    void Update()
    {
        if (Camera.main.GetComponent<gameState>().GameState != 1)
        {
            CheckWin();
        }
    }
}


