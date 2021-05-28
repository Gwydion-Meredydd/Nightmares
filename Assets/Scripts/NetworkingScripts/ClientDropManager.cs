using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientDropManager : MonoBehaviour
{
    public static ClientDropManager _clientDropManager;
    public ClientDropManager RefClientDropManager;

    public GameObject[] DropPrefabs;
    public GameObject[] ActiveDropsArray;
    public List<GameObject> ActiveDrops;
    public bool[] DropStatus;
    public int[] MaxTiming;
    public int[] Timing;
    public GameObject[] DropUI;
    public Text[] DropUIText;
    // Start is called before the first frame update
    void Start()
    {
        RefClientDropManager = this;
        _clientDropManager = RefClientDropManager;
    }

    public void SpawnPrefab(int DropValue,Vector3 DropPos) 
    {
        switch (DropValue)
        {
            case 0:
                Instantiate(DropPrefabs[DropValue], DropPos, new Quaternion(0, 0, 0, 0));
                break;
            case 1:
                Instantiate(DropPrefabs[DropValue], DropPos, new Quaternion(0, 0, 0, 0));
                break;
            case 2:
                Instantiate(DropPrefabs[DropValue], DropPos, new Quaternion(0, 0, 0, 0));
                break;
            case 3:
                Instantiate(DropPrefabs[DropValue], DropPos, new Quaternion(0, 0, 0, 0));
                break;
            case 4:
                Instantiate(DropPrefabs[DropValue], DropPos, new Quaternion(0, 0, 0, 0));
                break;
            case 5:
                Instantiate(DropPrefabs[DropValue], DropPos, new Quaternion(0, 0, 0, 0));
                break;
        }
        ActiveDropsArray = new GameObject[0];
        ActiveDropsArray = GameObject.FindGameObjectsWithTag("Drop");
        ActiveDrops = new List<GameObject>(0);
        foreach (GameObject Drop in ActiveDropsArray)
        {
            ActiveDrops.Add(Drop);
        }
    }
    public void RemoveDrop(int DropValue)
    {
        Destroy(ActiveDrops[DropValue]);
        ActiveDrops.RemoveAt(DropValue);
        StartCoroutine(RemoveTiming());
    }
    IEnumerator RemoveTiming() 
    {
        yield return new WaitForEndOfFrame();
        ActiveDropsArray = GameObject.FindGameObjectsWithTag("Drop");
    }
    public void DropsPickedUpOn(int DropValue)
    {
        if (DropValue < DropStatus.Length)
        {
            DropStatus[DropValue] = true;
            Timing[DropValue] = MaxTiming[DropValue];
            ClientGameMenu._clientGameMenu.DropCalculations(DropValue);
        }
    }
    public void DropsPickedUpOff(int DropValue) 
    {

        DropStatus[DropValue] = false;
    }
    IEnumerator DropTiming() 
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < DropStatus.Length; i++)
        {
            if (DropStatus[i])
            {
                if (DropUI[i] != null)
                {
                    DropUI[i].SetActive(true);
                    DropUIText[i].gameObject.SetActive(true);
                    if (Timing[i] > 0)
                    {
                        DropUIText[i].text = Timing[i].ToString();
                    }
                }
            }
            else
            {
                if (DropUI != null)
                {
                    DropUI[i].SetActive(false);
                    DropUIText[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
