using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientGameMenu : MonoBehaviour
{
    [HideInInspector]
    public ClientGameMenu RefclientGameMenu;
    public static ClientGameMenu _clientGameMenu;

    public GameObject ReviveGameObject;
    public GameObject MultiplayerInGameMenu;
    public Text RoundText;
    public GameObject[] DropIcon;
    public Text[] DropTimeText;
    public int ConnectedClientsAmmount;
    public ClientUIClass[] ClientsUIFields;
    public int[] DropTimes;
    [Space]
    public bool DoublePointsIsActive;
    public bool InstantKillIsActive;
    public bool BlindEyeIsActive;
    public bool FlameThrowerIsActive;
    public bool MiniGunIsActive;

    private void Start()
    {
        RefclientGameMenu = this;
        _clientGameMenu = RefclientGameMenu;
    }
    public void UpdateClientFileds()
    {
        if (MultiplayerManager.instance.Username.Length > 0)
        {
            ConnectedClientsAmmount = 1;
            if (MultiplayerManager.instance.Username.Length > 1)
            {
                ConnectedClientsAmmount = 2;
                if (MultiplayerManager.instance.Username.Length > 2)
                {
                    ConnectedClientsAmmount = 3;
                    if (MultiplayerManager.instance.Username.Length > 3)
                    {
                        ConnectedClientsAmmount = 4;
                    }
                }
            }
        }
        foreach (ClientUIClass ClientClass in ClientsUIFields)
        {
            foreach (GameObject Coin in ClientClass.Coins)
            {
                Coin.SetActive(false);
            }
            foreach (GameObject Heart in ClientClass.Hearts)
            {
                Heart.SetActive(false);
            }
            ClientClass.Score.text = "";
            ClientClass.Usernames.text = "";
            ClientClass.Root.SetActive(false);
        }
        switch (ConnectedClientsAmmount) 
        {
            case 1:
                Debug.Log("Connected Client 1 Called");
                EnablePlayerField(0);
                break;
            case 2:
                Debug.Log("Connected Client 2 Called");
                EnablePlayerField(0);
                EnablePlayerField(1);
                break;
            case 3:
                Debug.Log("Connected Client 3 Called");
                EnablePlayerField(0);
                EnablePlayerField(1);
                EnablePlayerField(2);
                break;
            case 4:
                Debug.Log("Connected Client 4 Called");
                EnablePlayerField(0);
                EnablePlayerField(1);
                EnablePlayerField(2);
                EnablePlayerField(3);
                break;
        }
    }
    public void UpdateCoinCount(int ClientId) 
    {
        int UIcLIENTvALUE = ClientId - 1;
        switch (GameManager.players[ClientId].CoinAmmount)
        {
            case 0:
                ClientsUIFields[UIcLIENTvALUE].Coins[0].SetActive(false);
                ClientsUIFields[UIcLIENTvALUE].Coins[1].SetActive(false);
                ClientsUIFields[UIcLIENTvALUE].Coins[2].SetActive(false);
                break;
            case 1:
                ClientsUIFields[UIcLIENTvALUE].Coins[0].SetActive(true);
                ClientsUIFields[UIcLIENTvALUE].Coins[1].SetActive(false);
                ClientsUIFields[UIcLIENTvALUE].Coins[2].SetActive(false);
                break;
            case 2:
                ClientsUIFields[UIcLIENTvALUE].Coins[0].SetActive(true);
                ClientsUIFields[UIcLIENTvALUE].Coins[1].SetActive(true);
                ClientsUIFields[UIcLIENTvALUE].Coins[2].SetActive(false);
                break;
            case 3:
                ClientsUIFields[UIcLIENTvALUE].Coins[0].SetActive(true);
                ClientsUIFields[UIcLIENTvALUE].Coins[1].SetActive(true);
                ClientsUIFields[UIcLIENTvALUE].Coins[2].SetActive(true);
                break;
        }
    }
    public void UpdateHealth(int ClientId, float NewHealth) 
    {
        ClientId = ClientId - 1;
        for (int i = 0; i < ClientsUIFields[ClientId].Hearts.Length; i++)
        {
            ClientsUIFields[ClientId].Hearts[i].SetActive(false);
        }
        switch (NewHealth)
        {
            case 0:
                for (int i = 0; i < ClientsUIFields[ClientId].Hearts.Length; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(false);
                }
                break;
            case 12.5f:
                for (int i = 0; i < 1; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 25:
                for (int i = 0; i < 2; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 37.5f:
                for (int i = 0; i < 3; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 50:
                for (int i = 0; i < 4; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 62.5f:
                for (int i = 0; i < 5; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 75:
                for (int i = 0; i < 6; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 87.5f:
                for (int i = 0; i < 7; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 100:
                for (int i = 0; i < 8; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 112.5f:
                for (int i = 0; i < 9; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 125:
                for (int i = 0; i < 10; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 137.5f:
                for (int i = 0; i < 11; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 150:
                for (int i = 0; i < 12; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 162.5f:
                for (int i = 0; i < 13; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 175:
                for (int i = 0; i < 14; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 187.5f:
                for (int i = 0; i < 15; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
            case 200:
                for (int i = 0; i < 16; i++)
                {
                    ClientsUIFields[ClientId].Hearts[i].SetActive(true);
                }
                break;
        }
    }
    public void UpdateScore(int PlayerId,float NewScore) 
    {
        PlayerId = PlayerId + 1;
        switch (PlayerId)
        {
            case 1:
                ClientsUIFields[0].Score.text = NewScore.ToString();
                break;
            case 2:
                ClientsUIFields[1].Score.text = NewScore.ToString();
                break;
            case 3:
                ClientsUIFields[0].Score.text = NewScore.ToString();
                break;
            case 4:
                ClientsUIFields[1].Score.text = NewScore.ToString();
                break;
        }
    }
    public void DropCalculations(int DropLayer) 
    {
        Debug.Log(DropLayer + " drop calculations");
        switch (DropLayer) 
        {
            //blind eye
            case 2:
                DropIcon[0].SetActive(true);
                DropTimeText[0].gameObject.SetActive(true);
                BlindEyeIsActive = true;
                StartCoroutine(BlinEyeDropTime());
                break;
                //douple points
            case 0:
                DropIcon[1].SetActive(true);
                DropTimeText[1].gameObject.SetActive(true);
                DoublePointsIsActive = true;
                StartCoroutine(DouplePointsTime());
                break;
                //instant kill
            case 1:
                DropIcon[2].SetActive(true);
                DropTimeText[2].gameObject.SetActive(true);
                InstantKillIsActive = true;
                StartCoroutine(InstantKillTime());
                break;
                //flame thrower 
            case 4:
                DropIcon[3].SetActive(true);
                DropTimeText[3].gameObject.SetActive(true);
                FlameThrowerIsActive = true;
                StartCoroutine(FlameThrowerTime());
                break;
                //mini gun
            case 3:
                DropIcon[4].SetActive(true);
                DropTimeText[4].gameObject.SetActive(true);
                MiniGunIsActive = true;
                StartCoroutine(MiniGunTime());
                break;
        }
    }
    IEnumerator BlinEyeDropTime()
    {
        DropTimes[0] = 30;
        while (BlindEyeIsActive)
        {
            DropTimes[0] = DropTimes[0] - 1;
            DropTimeText[0].text = DropTimes[0].ToString();
            yield return new WaitForSecondsRealtime(1);
            if (DropTimes[0] <= 0) 
            {
                BlindEyeIsActive = false;
                DropIcon[0].SetActive(false);
                DropTimeText[0].gameObject.SetActive(false);
            }
            
        }
    }
    IEnumerator DouplePointsTime()
    {
        DropTimes[1] = 30;
        while (DoublePointsIsActive)
        {
            DropTimes[1] = DropTimes[1] - 1;
            DropTimeText[1].text = DropTimes[1].ToString();
            yield return new WaitForSecondsRealtime(1);
            if (DropTimes[1] <= 0)
            {
                DoublePointsIsActive = false;
                DropIcon[1].SetActive(false);
                DropTimeText[1].gameObject.SetActive(false);
            }

        }
    }
    IEnumerator InstantKillTime()
    {
        DropTimes[2] = 30;
        while (InstantKillIsActive)
        {
            DropTimes[2] = DropTimes[2] - 1;
            DropTimeText[2].text = DropTimes[2].ToString();
            yield return new WaitForSecondsRealtime(1);
            if (DropTimes[2] <= 0)
            {
                InstantKillIsActive = false;
                DropIcon[2].SetActive(false);
                DropTimeText[2].gameObject.SetActive(false);
            }

        }
    }
    IEnumerator FlameThrowerTime()
    {
        DropTimes[3] = 30;
        while (FlameThrowerIsActive)
        {
            DropTimes[3] = DropTimes[3] - 1;
            DropTimeText[3].text = DropTimes[3].ToString();
            yield return new WaitForSecondsRealtime(1);
            if (DropTimes[3] <= 0)
            {
                FlameThrowerIsActive = false;
                DropIcon[3].SetActive(false);
                DropTimeText[3].gameObject.SetActive(false);
            }

        }
    }
    IEnumerator MiniGunTime()
    {
        DropTimes[4] = 30;
        while (MiniGunIsActive)
        {
            DropTimes[4] = DropTimes[4] - 1;
            DropTimeText[4].text = DropTimes[4].ToString();
            yield return new WaitForSecondsRealtime(1);
            if (DropTimes[4] <= 0)
            {
                MiniGunIsActive = false;
                DropIcon[4].SetActive(false);
                DropTimeText[4].gameObject.SetActive(false);
            }

        }
    }
    public void EnablePlayerField(int PlayerValue) 
    {
        DropIcon[0].SetActive(false);
        DropTimeText[0].gameObject.SetActive(false);
        DropIcon[1].SetActive(false);
        DropTimeText[1].gameObject.SetActive(false);
        DropIcon[2].SetActive(false);
        DropTimeText[2].gameObject.SetActive(false);
        DropIcon[3].SetActive(false);
        DropTimeText[3].gameObject.SetActive(false);
        DropIcon[4].SetActive(false);
        DropTimeText[4].gameObject.SetActive(false);
        ClientsUIFields[PlayerValue].Root.SetActive(true);
        ClientsUIFields[PlayerValue].Usernames.text = MultiplayerManager.instance.Username[PlayerValue];
        ClientsUIFields[PlayerValue].Score.text = "0";
        ClientsUIFields[PlayerValue].Hearts[0].SetActive(true);
        ClientsUIFields[PlayerValue].Hearts[1].SetActive(true);
        ClientsUIFields[PlayerValue].Hearts[2].SetActive(true);
        ClientsUIFields[PlayerValue].Hearts[3].SetActive(true);
        ClientsUIFields[PlayerValue].Hearts[4].SetActive(true);
        ClientsUIFields[PlayerValue].Hearts[5].SetActive(true);
        ClientsUIFields[PlayerValue].Hearts[6].SetActive(true);
        ClientsUIFields[PlayerValue].Hearts[7].SetActive(true);
        ClientsUIFields[PlayerValue].Hearts[8].SetActive(false);
        ClientsUIFields[PlayerValue].Hearts[9].SetActive(false);
        ClientsUIFields[PlayerValue].Hearts[10].SetActive(false);
        ClientsUIFields[PlayerValue].Hearts[11].SetActive(false);
        ClientsUIFields[PlayerValue].Hearts[12].SetActive(false);
        ClientsUIFields[PlayerValue].Hearts[13].SetActive(false);
        ClientsUIFields[PlayerValue].Hearts[14].SetActive(false);
        ClientsUIFields[PlayerValue].Hearts[15].SetActive(false);
        ClientsUIFields[PlayerValue].Coins[0].SetActive(true);
        ClientsUIFields[PlayerValue].Coins[1].SetActive(true);
        ClientsUIFields[PlayerValue].Coins[2].SetActive(true);
    }


}
[System.Serializable]
public class ClientUIClass 
{
    public GameObject Root;
    public Text Usernames;
    public Text Score;
    public GameObject[] Hearts;
    public GameObject[] Coins;
}
