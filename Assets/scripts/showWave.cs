using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showWave : MonoBehaviour
{
    private Text m_waveText = null;

    void Awake()
    {
        m_waveText = transform.Find("Text").gameObject.GetComponent<Text>();
    }

    public void SetWaveText(string sText)
    {
        m_waveText.text = sText;
    }
}
