using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class gameChoiceButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.Find("Text").GetComponent<Text>().text == "Try Again")
        {
            //reset game state
            Camera.main.GetComponent<gameState>().GameState = 0;
            //reset golds
            Camera.main.GetComponent<resLoad>().ShowGoldUI.GetComponent<showGold>().Start();
            //reset level
            Camera.main.GetComponent<levelManager>().ResetLevel();
            //reset UI
            transform.parent.gameObject.SetActive(false); 
            transform.parent.parent.gameObject.GetComponent<Animator>().SetBool("showInfo", false);
        }
        else if (transform.Find("Text").GetComponent<Text>().text == "Next Level")
        {
            SceneManager.LoadScene("Loading");
        }
        else if (transform.Find("Text").GetComponent<Text>().text == "Quit Game")
        {
            Application.Quit();
        }
    }
}
