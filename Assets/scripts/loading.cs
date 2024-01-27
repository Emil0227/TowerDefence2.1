using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loading : MonoBehaviour
{
    private AsyncOperation m_ao;

    void Start()
    {
        gameObject.GetComponent<Image>().fillAmount =0;
        singleClass.currentLevel += 1;
        m_ao = SceneManager.LoadSceneAsync("" + singleClass.currentLevel);
        m_ao.allowSceneActivation = false;
    }

    void Update()
    {
        //do not automatically switch to next scene when loading is compelte
        //slow down progress bar and change loading progress from 0.9 to 1
        if (m_ao.progress < 0.9f)
        {
            gameObject.GetComponent<Image>().fillAmount += 0.002f;
        }
        else
        {
            if (gameObject.GetComponent<Image>().fillAmount < 1.0f)
            {
                gameObject.GetComponent<Image>().fillAmount += 0.002f;
            }
            else
            {
                m_ao.allowSceneActivation = true;
            }
        }
    }
}
