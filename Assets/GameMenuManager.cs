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
        switch (NewHealth)
        {
            case 0:
                for (int i = 0; i < HealthHearts.Length; i++)
                {
                    HealthHearts[i].SetActive(false);
                }
                break;
            case 12.5f:
                for (int i = 0; i < HealthHearts.Length - 7; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                for (int i = 1; i < HealthHearts.Length; i++)
                {
                    HealthHearts[i].SetActive(false);
                }
                break;
            case 25:
                for (int i = 0; i < HealthHearts.Length - 6; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                for (int i = 2; i < HealthHearts.Length; i++)
                {
                    HealthHearts[i].SetActive(false);
                }
                break;
            case 37.5f:
                for (int i = 0; i < HealthHearts.Length - 5; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                for (int i = 3; i < HealthHearts.Length; i++)
                {
                    HealthHearts[i].SetActive(false);
                }
                break;
            case 50:
                for (int i = 0; i < HealthHearts.Length - 4; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                for (int i = 4; i < HealthHearts.Length; i++)
                {
                    HealthHearts[i].SetActive(false);
                }
                break;
            case 62.5f:
                for (int i = 0; i < HealthHearts.Length - 3; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                for (int i = 5; i < HealthHearts.Length; i++)
                {
                    HealthHearts[i].SetActive(false);
                }
                break;
            case 75:
                for (int i = 0; i < HealthHearts.Length - 2; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                HealthHearts[6].SetActive(false);
                HealthHearts[7].SetActive(false);
                break;
            case 87.5f:
                for (int i = 0; i < HealthHearts.Length - 1; i++)
                {
                    HealthHearts[i].SetActive(true);
                }
                HealthHearts[7].SetActive(false);
                break;
            case 100:
                foreach (var Heart in HealthHearts)
                {
                    Heart.SetActive(true);
                }
                break;
        }
    }
}
