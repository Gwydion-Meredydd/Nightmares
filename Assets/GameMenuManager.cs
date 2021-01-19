using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMenuManager : MonoBehaviour
{
    public ScriptsManager SM;

    public bool MenuHasBeenInitilised;
    public GameObject InGameMenu;
    public Text RoundText;
    public Text ScoreText;
    public GameObject InsertCoin;
    public GameObject[] HealthHearts;
    public GameObject[] Coins;
    public GameObject[] DropsIcon;
    public Text[] DropsIconText;
    void Update()
    {
        if (SM.GameScript.InGame == true)
        {
            if (SM.GameScript.Paused == false)
            {
                if (MenuHasBeenInitilised == false) 
                {
                    InitiliseInGameMenu();
                }
            }
        }
    }
    void InitiliseInGameMenu() 
    {
        InGameMenu.SetActive(true);
        MenuHasBeenInitilised = true;
        foreach (GameObject DropIcon in DropsIcon)
        {
            DropIcon.SetActive(false);
        }
        foreach (Text DropTimeText in DropsIconText)
        {
            DropTimeText.text = "";
        }
    }
    public void CoinMethod()
    {
        switch (SM.CoinScript.CoinAmmount) 
        {
            case 0:
                Coins[0].SetActive(false);
                Coins[1].SetActive(false);
                Coins[2].SetActive(false);
                break;
            case 1:
                Coins[0].SetActive(true);
                Coins[1].SetActive(false);
                Coins[2].SetActive(false);
                break;
            case 2:
                Coins[0].SetActive(true);
                Coins[1].SetActive(true);
                Coins[2].SetActive(false);
                break;
            case 3:
                Coins[0].SetActive(true);
                Coins[1].SetActive(true);
                Coins[2].SetActive(true);
                break;
        }
    }
    public void HealthMethod(float NewHealth)
    {
        for (int i = 0; i < HealthHearts.Length; i++)
        {
            HealthHearts[i].SetActive(false);
        }
        switch (NewHealth)
        {
            case 0:
                for (int i = 0; i < HealthHearts.Length; i++)
                {
                    HealthHearts[i].SetActive(false);
                }
                break;
            case 12.5f:
                for (int i = 0; i < 1; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 25:
                for (int i = 0; i < 2; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 37.5f:
                for (int i = 0; i < 3; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 50:
                for (int i = 0; i < 4; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 62.5f:
                for (int i = 0; i < 5; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 75:
                for (int i = 0; i < 6; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 87.5f:
                for (int i = 0; i < 7; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 100:
                for (int i = 0; i < 8; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 112.5f:
                for (int i = 0; i < 9; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 125:
                for (int i = 0; i < 10; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 137.5f:
                for (int i = 0; i < 11; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 150:
                for (int i = 0; i < 12; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 162.5f:
                for (int i = 0; i < 13; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 175:
                for (int i = 0; i < 14; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 187.5f:
                for (int i = 0; i <15; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
            case 200:
                for (int i = 0; i < 16; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                break;
        }
    }
}
