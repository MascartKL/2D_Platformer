using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class ButtonController : MonoBehaviour
{
    public GameObject inventory;
    public GameObject stashSword;
    public GameObject stashChest;
    public GameObject stashHelmet;
    

    public List<GameObject> stashes = new List<GameObject>();




    public void OnInvButton()
    {
        inventory.SetActive(true);
    }

    public void BackToMenu()
    {
        inventory.SetActive(false);
    }

    public void OnLevelButton()
    {
        SceneManager.LoadScene(1);
    }

    public void BackFromScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenSSword()
    {       
        for(int i =0; i<3;i++)
        {
            if (stashes[i].activeSelf == true)
                stashes[i].SetActive(false);
        }
        stashes[0].SetActive(true);
    }
    public void OpenSChest()
    {
        for (int i = 0; i < 3; i++)
        {
            if (stashes[i].activeSelf == true)
                stashes[i].SetActive(false);
        }
        stashes[1].SetActive(true);
    }
    public void OpenSHelmet()
    {
        for (int i = 0; i < 3; i++)
        {
            if (stashes[i].activeSelf == true)
                stashes[i].SetActive(false);
        }
        stashes[2].SetActive(true);
    }

}
