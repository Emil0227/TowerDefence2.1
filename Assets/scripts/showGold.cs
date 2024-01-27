using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showGold : MonoBehaviour
{
    public int Gold = 0;

    private AudioSource m_bonusAudio;

    public void Start()
    {
        m_bonusAudio = gameObject.GetComponent<AudioSource>();
        if (singleClass.currentLevel == 1)
        {
            SetGold(20);
        }
        if (singleClass.currentLevel == 2)
        {
            SetGold(30);
        }
    }

    public void SetGold(int x)
    {
        Gold = x;
        transform.Find("Text").gameObject.GetComponent<Text>().text = "Gold: " + Gold;
    }

    public int GetGold()
    {
        return Gold;
    }

    public void PlayBonusSFX()
    {
        m_bonusAudio.Play();
    }
}
