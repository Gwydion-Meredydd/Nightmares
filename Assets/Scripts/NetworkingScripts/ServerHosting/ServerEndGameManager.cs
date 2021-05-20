using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ServerEndGameManager : MonoBehaviour
{
    public bool EndGame;
    public bool EndGameInitlised2;
    public bool EndGameInitlised;
    [HideInInspector]
    public static ServerEndGameManager _serverEndGameManager;
    public ServerEndGameManager RefserverEndGameManager;
    const string PrivateCode = "6knyfApwAEKviwqQqmbNeQ7rv-9iscF0C-QfCOuYt_nQ";
    const string PublicCode = "601327a98f40bb2a70343772";
    const string WebUrl = "http://dreamlo.com/lb/";
    private void Start()
    {
        RefserverEndGameManager = this;
        _serverEndGameManager = RefserverEndGameManager;
    }

    private void Update()
    {
        if (!EndGame)
        {
            if (ServerHostingManager.Instance.SpawnLevel)
            {
                if (!EndGameInitlised)
                {
                    EndGameInitlised = true;
                    StartCoroutine(StartingCooldown());
                }
                if (EndGameInitlised2)
                {
                    bool PlayersRNotDead = false;
                    for (int i = 0; i < ServerHostingManager.Instance.ConnectedClientsClass.Count; i++)
                    {
                        if (!ServerHostingManager.Instance.ConnectedClientsClass[i].IsDead)
                        {
                            PlayersRNotDead = true;
                        }
                    }
                    if (!PlayersRNotDead)
                    {
                        EndGame = true;
                        Debug.Log("EndGame");
                        EndGameMethod();
                    }
                }
            }
        }
    }
    IEnumerator StartingCooldown()
    {
        yield return new WaitForSecondsRealtime(2f);
        EndGameInitlised2 = true;
    }
    public void EndGameMethod() 
    {
        string CombinedUsernames = "";
        foreach (string username in ServerHostingManager.Instance.ConnectedClientsUsernames)
        {
            CombinedUsernames = CombinedUsernames + username + ",";
        }
        int CombinedScore = 0;
        foreach (int score in ServerPoints._serverPoints.Points)
        {
            CombinedScore = CombinedScore + score;
        }
        Debug.Log("Starting adding of highscore: ");
        StartCoroutine(GetRequest("http://dreamlo.com/lb/6knyfApwAEKviwqQqmbNeQ7rv-9iscF0C-QfCOuYt_nQ/add/" + CombinedUsernames + "/" + CombinedScore));
    }
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
