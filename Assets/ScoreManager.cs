using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public int Score;
    [Space]
    public bool Testing;
    public string UsernameForTesting;
    public int ScoreForTesting;
    public string[] UserNamesInLeaderboard;
    const string PrivateCode = "P1WnnoraLkOghI860ic3yg6p3shG4iDkSCKAZYvOC9lg";
    const string PublicCode = "5fd6a659eb36fe271432acbd";
    const string WebUrl = "http://dreamlo.com/lb/";
    public bool BadWordsLibraryInitlised;
    public List<string> BadWords;
    public TextAsset BadWordsFile;

    private void Update()
    {
        if (Testing) 
        {
            TestHighScore();
            Testing = false;
        }
        if (!BadWordsLibraryInitlised) 
        {
            BadWordsLibraryInitlised = true;
            BadWords = new List<string>();
            string[] lines = BadWordsFile.text.Split('\n');
            foreach (string line in lines)
            {
                BadWords.Add(line);
            }
        }
    }
    void TestHighScore() 
    {
        AddNewHighScore(UsernameForTesting, ScoreForTesting);
    }
    public void AddNewHighScore(string UserName, int NewScore) 
    {
        StartCoroutine(UserNameDataFetcher(UserName.ToLower(), NewScore));
    }
    IEnumerator UploadNewHighScore(string UserName, int NewScore) 
    {
        Debug.Log("UserName is okay");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PrivateCode + "/add/" + UserName + "/" + NewScore))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = UserName.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
    IEnumerator UserNameDataFetcher(string UserName,int NewScore)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode + "/pipe/"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            UserNameChecker(webRequest.downloadHandler.text, UserName,NewScore);
        }
    }
    void UserNameChecker(string DownloadedData, string UserName,int NewScore)
    {
        bool UserNameisTaken = false;
        bool UserNameisNotSutable = false;
        string[] Data = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        UserNamesInLeaderboard = new string[Data.Length];
        for (int i = 0; i < Data.Length; i++)
        {
            string[] DataInfo = Data[i].Split(new char[] { '|' });
            UserNamesInLeaderboard[i] = DataInfo[0];
        }
        foreach (var CheckedUserName in UserNamesInLeaderboard)
        {
            Debug.Log(CheckedUserName + " " + UserName);
            if (CheckedUserName.Equals(UserName))
            {
                Debug.Log("Username AllreadyTaken");
                UserNameisTaken = true;
            }
        }
        foreach (var CheckedBadWords in BadWords)
        {
            if (CheckedBadWords.ToString().Trim().ToLower().Equals(UserName.Trim().ToLower()))
            {
                Debug.Log("Username AllreadyTaken");
                UserNameisNotSutable = true;
            }
        }
        if (!UserNameisTaken && !UserNameisNotSutable)
        {
            StartCoroutine(UploadNewHighScore(UserName, NewScore));
        }
        else if (UserNameisTaken) 
        {
            UserNameTaken();
        }
        else if (UserNameisNotSutable) 
        {
            UserNameNotSuitable();
        }
    }
    void UserNameTaken() 
    {
        Debug.Log("USER NAME TAKEN METHOD CALLED");
    }
    void UserNameNotSuitable()
    {
        Debug.Log("USER NAME NOT SUITABLE METHOD CALLED");
    }
}
