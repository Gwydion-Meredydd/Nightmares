using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public ScriptsManager SM;
    [Space]
    public int Score;
    [Space]
    public bool Testing;
    public string UsernameForTesting;
    public int ScoreForTesting;
    public string[] UserNamesInLeaderboard;
    public int[] ScoreInLeaderboard;
    const string PrivateCode = "P1WnnoraLkOghI860ic3yg6p3shG4iDkSCKAZYvOC9lg";
    const string PublicCode = "5fd6a659eb36fe271432acbd";
    const string WebUrl = "http://dreamlo.com/lb/";
    public bool BadWordsLibraryInitlised;
    public List<string> BadWords;
    public TextAsset BadWordsFile;
    [Space]
    public int UserNameInLeaderboard;
    public GameObject[] HighScoreFieldsPerent;
    public Text[] HighScoreFieldsName;
    public Text[] HighScoreFieldsScore;
    void Start()
    {
        FindAllHighScoreFields();
    }
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
    void FindAllHighScoreFields() 
    {
        SM.FadeManager.InstantFadeIn();
        SM.FadeManager.FadeOut();
        HighScoreFieldsPerent = GameObject.FindGameObjectsWithTag("HighScoreMenu");
        HighScoreFieldsName = new Text[HighScoreFieldsPerent.Length];
        HighScoreFieldsScore = new Text[HighScoreFieldsPerent.Length];
        GameObject[] TempTextName = GameObject.FindGameObjectsWithTag("HighScoreTextName");
        GameObject[] TempTextScore = GameObject.FindGameObjectsWithTag("HighScoreTextScore");
        for (int i = 0; i < HighScoreFieldsPerent.Length; i++)
        {
            HighScoreFieldsName[i] = TempTextName[i].GetComponent<Text>();
            HighScoreFieldsScore[i] = TempTextScore[i].GetComponent<Text>();
        }
        StartCoroutine(FetchDataAmmount());
    }
    IEnumerator FetchDataAmmount()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode + "/pipe/"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            DataAmmount(webRequest.downloadHandler.text);
        }

    }
    void DataAmmount(string DownloadedData)
    {
        string[] Data = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        UserNamesInLeaderboard = new string[Data.Length];
        ScoreInLeaderboard = new int[Data.Length];
        for (int i = 0; i < Data.Length; i++)
        {
            Debug.Log(Data[i]);
            string[] DataInfo = Data[i].Split(new char[] { '|' });
            UserNamesInLeaderboard[i] = DataInfo[0];
            int.TryParse(DataInfo[1], out ScoreInLeaderboard[i]);
            HighScoreFieldsName[i].text = UserNamesInLeaderboard[i];
            HighScoreFieldsScore[i].text = ScoreInLeaderboard[i].ToString();
        }
        UserNameInLeaderboard = 0;
        foreach (var CheckedUserName in UserNamesInLeaderboard)
        {
            UserNameInLeaderboard = UserNameInLeaderboard + 1;
        }
        foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent)
        {
            HighScoreFieldPerent.SetActive(false);
        }
        for (int i = 0; i < UserNameInLeaderboard; i++)
        {
            HighScoreFieldsPerent[i].SetActive(true);
        }
        SM.MainMenuScript.HighScoreObj.SetActive(false);
    }
    void TestHighScore() 
    {
        AddNewHighScore(UsernameForTesting, ScoreForTesting);
    }
    public void AddNewHighScore(string UserName, int NewScore) 
    {
        StartCoroutine(UserNameDataFetcher(UserName.ToLower(), NewScore, true));
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
    public void CheckUserNameFromMainMenu(string UserName) 
    {
        StartCoroutine(UserNameDataFetcher(UserName.ToLower().Trim(), 0, false));
    }
    IEnumerator UserNameDataFetcher(string UserName,int NewScore,bool NewScoreInput)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode + "/pipe/"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            UserNameChecker(webRequest.downloadHandler.text, UserName, NewScore, NewScoreInput);
        }
    }
    void UserNameChecker(string DownloadedData, string UserName,int NewScore, bool NewScoreInput)
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
        UserNameInLeaderboard = 0;
        foreach (var CheckedUserName in UserNamesInLeaderboard)
        {
            UserNameInLeaderboard = UserNameInLeaderboard + 1;
            Debug.Log(CheckedUserName + " " + UserName);
            if (CheckedUserName.Equals(UserName))
            {
                Debug.Log("Username AllreadyTaken");
                UserNameisTaken = true;
            }
        }
        Debug.Log(UserName);
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
            Debug.Log("Name Accepted");
            if (NewScoreInput)
            {
                StartCoroutine(UploadNewHighScore(UserName, NewScore));
            }
            else
            {
                SM.MainMenuScript.UserNameIsOkay();
            }
        }
        else if (UserNameisTaken)
        {
            Debug.Log("Name Taken");
            if (NewScoreInput)
            {
                UserNameTaken();
            }
            else
            {
                SM.MainMenuScript.UserNameNotAvailable();
            }
        }
        else if (UserNameisNotSutable)
        {
            Debug.Log("Name not Appropriate");
            if (NewScoreInput)
            {
                UserNameNotSuitable();
            }
            else
            {
                SM.MainMenuScript.UserNameNotSuitable();
            }
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
