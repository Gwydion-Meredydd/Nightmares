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
    public int LeaderboardValue;
    const string PrivateCode1 = "P1WnnoraLkOghI860ic3yg6p3shG4iDkSCKAZYvOC9lg";
    const string PublicCode1 = "5fd6a659eb36fe271432acbd";
    const string PrivateCode2 = "mY1LW5ca7U2WDgeTclF_0guWMEyu7rw0eHXBC1dkr3Jg";
    const string PublicCode2 = "6012c3438f40bb2a70327e47";
    const string PrivateCode3 = "dcaKvFEDU0y9JIQYDwRswwEOsqZ4Z4j06CHTWK6IAgYw";
    const string PublicCode3 = "6012d86e8f40bb2a7032d529";
    const string PrivateCode4 = "iuddHQ89AUemDsLldEy9XQVYVf_gyKRUSflddiOLggeA";
    const string PublicCode4 = "601327888f40bb2a703436c5";
    const string PrivateCode5 = "6knyfApwAEKviwqQqmbNeQ7rv-9iscF0C-QfCOuYt_nQ";
    const string PublicCode5 = "601327a98f40bb2a70343772";
    const string WebUrl = "http://dreamlo.com/lb/";
    public bool BadWordsLibraryInitlised;
    public List<string> BadWords;
    public TextAsset BadWordsFile;
    [Space]
    public string ErrorCode1, ErrorCode2, ErrorCode3, ErrorCode4;
    [Header("LeaderBoard1")]
    public int UserNameInLeaderboard1;
    public GameObject HighScoreRoot1;
    public GameObject[] HighScoreFieldsPerent1;
    public Text[] HighScoreFieldsName1;
    public Text[] HighScoreFieldsScore1;
    public GameObject YourScore1;
    public Text HighScoreName1;
    public Text HighScoreScore1;
    public string[] UserNamesInLeaderboard1;
    public int[] ScoreInLeaderboard1;
    [Header("LeaderBoard2")]
    public int UserNameInLeaderboard2;
    public GameObject HighScoreRoot2;
    public GameObject[] HighScoreFieldsPerent2;
    public Text[] HighScoreFieldsName2;
    public Text[] HighScoreFieldsScore2;
    public GameObject YourScore2;
    public Text HighScoreName2;
    public Text HighScoreScore2;
    public string[] UserNamesInLeaderboard2;
    public int[] ScoreInLeaderboard2;
    [Header("LeaderBoard3")]
    public int UserNameInLeaderboard3;
    public GameObject HighScoreRoot3;
    public GameObject[] HighScoreFieldsPerent3;
    public Text[] HighScoreFieldsName3;
    public Text[] HighScoreFieldsScore3;
    public GameObject YourScore3;
    public Text HighScoreName3;
    public Text HighScoreScore3;
    public string[] UserNamesInLeaderboard3;
    public int[] ScoreInLeaderboard3;
    [Header("LeaderBoard4")]
    public int UserNameInLeaderboard4;
    public GameObject HighScoreRoot4;
    public GameObject[] HighScoreFieldsPerent4;
    public Text[] HighScoreFieldsName4;
    public Text[] HighScoreFieldsScore4;
    public GameObject YourScore4;
    public Text HighScoreName4;
    public Text HighScoreScore4;
    public string[] UserNamesInLeaderboard4;
    public int[] ScoreInLeaderboard4;
    [Header("LeaderBoard5")]
    public int UserNameInLeaderboard5;
    public GameObject HighScoreRoot5;
    public GameObject[] HighScoreFieldsPerent5;
    public Text[] HighScoreFieldsName5;
    public Text[] HighScoreFieldsScore5;
    public GameObject YourScore5;
    public Text HighScoreName5;
    public Text HighScoreScore5;
    public string[] UserNamesInLeaderboard5;
    public int[] ScoreInLeaderboard5;

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
    public void FindAllHighScoreFields()
    {
        YourScore1.SetActive(false);
        SM.MainMenuScript.HighScoreLoading[0].SetActive(true);
        //SM.FadeManager.InstantFadeIn();
        SM.MainMenuScript.HighScoreRoot.SetActive(true);
        HighScoreRoot1.SetActive(true);
        HighScoreFieldsPerent1 = GameObject.FindGameObjectsWithTag("HighScoreMenu1");
        HighScoreFieldsName1 = new Text[HighScoreFieldsPerent1.Length];
        HighScoreFieldsScore1 = new Text[HighScoreFieldsPerent1.Length];
        GameObject[] TempTextName1 = GameObject.FindGameObjectsWithTag("HighScoreTextName1");
        GameObject[] TempTextScore1 = GameObject.FindGameObjectsWithTag("HighScoreTextScore1");
        for (int i = 0; i < HighScoreFieldsPerent1.Length; i++)
        {
            HighScoreFieldsName1[i] = TempTextName1[i].GetComponent<Text>();
            HighScoreFieldsScore1[i] = TempTextScore1[i].GetComponent<Text>();
        }
        YourScore2.SetActive(false);
        SM.MainMenuScript.HighScoreLoading[1].SetActive(true);
        //SM.FadeManager.InstantFadeIn();
        SM.MainMenuScript.HighScoreRoot.SetActive(true);
        HighScoreRoot2.SetActive(true);
        HighScoreFieldsPerent2 = GameObject.FindGameObjectsWithTag("HighScoreMenu2");
        HighScoreFieldsName2 = new Text[HighScoreFieldsPerent2.Length];
        HighScoreFieldsScore2 = new Text[HighScoreFieldsPerent2.Length];
        GameObject[] TempTextName2 = GameObject.FindGameObjectsWithTag("HighScoreTextName2");
        GameObject[] TempTextScore2 = GameObject.FindGameObjectsWithTag("HighScoreTextScore2");
        for (int i = 0; i < HighScoreFieldsPerent2.Length; i++)
        {
            HighScoreFieldsName2[i] = TempTextName2[i].GetComponent<Text>();
            HighScoreFieldsScore2[i] = TempTextScore2[i].GetComponent<Text>();
        }
        //YourScore3.SetActive(false);
        //SM.MainMenuScript.HighScoreLoading[2].SetActive(true);
        //SM.FadeManager.InstantFadeIn();
        SM.MainMenuScript.HighScoreRoot.SetActive(true);
        //HighScoreRoot3.SetActive(true);
        //HighScoreFieldsPerent3 = GameObject.FindGameObjectsWithTag("HighScoreMenu3");
      //  HighScoreFieldsName3 = new Text[HighScoreFieldsPerent3.Length];
        //HighScoreFieldsScore3 = new Text[HighScoreFieldsPerent3.Length];
       /// GameObject[] TempTextName3 = GameObject.FindGameObjectsWithTag("HighScoreTextName3");
       // GameObject[] TempTextScore3 = GameObject.FindGameObjectsWithTag("HighScoreTextScore3");
        //for (int i = 0; i < HighScoreFieldsPerent3.Length; i++)
       // {
        //    HighScoreFieldsName3[i] = TempTextName3[i].GetComponent<Text>();
        //    HighScoreFieldsScore3[i] = TempTextScore3[i].GetComponent<Text>();
        //}
        //YourScore4.SetActive(false);
        //.MainMenuScript.HighScoreLoading[3].SetActive(true);
        //SM.FadeManager.InstantFadeIn();
        SM.MainMenuScript.HighScoreRoot.SetActive(true);
       // HighScoreRoot4.SetActive(true);
       // HighScoreFieldsPerent4 = GameObject.FindGameObjectsWithTag("HighScoreMenu4");
       // HighScoreFieldsName4 = new Text[HighScoreFieldsPerent4.Length];
       // HighScoreFieldsScore4 = new Text[HighScoreFieldsPerent4.Length];
       // GameObject[] TempTextName4 = GameObject.FindGameObjectsWithTag("HighScoreTextName4");
       // GameObject[] TempTextScore4 = GameObject.FindGameObjectsWithTag("HighScoreTextScore4");
       // for (int i = 0; i < HighScoreFieldsPerent4.Length; i++)
       // {
       //     HighScoreFieldsName4[i] = TempTextName4[i].GetComponent<Text>();
        //    HighScoreFieldsScore4[i] = TempTextScore4[i].GetComponent<Text>();
       // }
        YourScore5.SetActive(false);
        SM.MainMenuScript.HighScoreLoading[4].SetActive(true);
        SM.FadeManager.InstantFadeIn();
        SM.MainMenuScript.HighScoreRoot.SetActive(true);
        HighScoreRoot5.SetActive(true);
        HighScoreFieldsPerent5 = GameObject.FindGameObjectsWithTag("HighScoreMenu5");
        HighScoreFieldsName5 = new Text[HighScoreFieldsPerent5.Length];
        HighScoreFieldsScore5 = new Text[HighScoreFieldsPerent5.Length];
        GameObject[] TempTextName5 = GameObject.FindGameObjectsWithTag("HighScoreTextName5");
        GameObject[] TempTextScore5 = GameObject.FindGameObjectsWithTag("HighScoreTextScore5");
        for (int i = 0; i < HighScoreFieldsPerent5.Length; i++)
        {
            HighScoreFieldsName5[i] = TempTextName5[i].GetComponent<Text>();
            HighScoreFieldsScore5[i] = TempTextScore5[i].GetComponent<Text>();
        }
        StartCoroutine(FetchDataAmmount());

    }
    IEnumerator FetchDataAmmount()
    {
        yield return new WaitForSecondsRealtime(1);
        switch (LeaderboardValue)
        {
            case 1:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode1 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();
                        DataAmmount(webRequest.downloadHandler.text);
                    }
                }
                break;
            case 2:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode2 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        Debug.Log(1);
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();
                        Debug.Log(2);
                        DataAmmount(webRequest.downloadHandler.text);
                    }
                }
                break;
            case 3:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode3 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        Debug.Log(1);
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();
                        Debug.Log(2);
                        DataAmmount(webRequest.downloadHandler.text);
                    }
                }
                break;
            case 4:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode4 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        Debug.Log(1);
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();
                        Debug.Log(2);
                        DataAmmount(webRequest.downloadHandler.text);
                    }
                }
                break;
            case 5:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode5 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        Debug.Log(1);
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();
                        Debug.Log(2);
                        DataAmmount(webRequest.downloadHandler.text);
                    }
                }
                break;
        }
    }
    void DataAmmount(string DownloadedData)
    {
        switch (LeaderboardValue)
        {
            case 1:
                string[] Data1 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data1[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data1[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data1[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data1[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    HighScoreFieldsPerent1 = GameObject.FindGameObjectsWithTag("HighScoreMenu1");
                    HighScoreFieldsName1 = new Text[HighScoreFieldsPerent1.Length];
                    HighScoreFieldsScore1 = new Text[HighScoreFieldsPerent1.Length];
                    GameObject[] TempTextName1 = GameObject.FindGameObjectsWithTag("HighScoreTextName1");
                    GameObject[] TempTextScore1 = GameObject.FindGameObjectsWithTag("HighScoreTextScore1");
                    for (int i = 0; i < HighScoreFieldsPerent1.Length; i++)
                    {
                        HighScoreFieldsName1[i] = TempTextName1[i].GetComponent<Text>();
                        HighScoreFieldsScore1[i] = TempTextScore1[i].GetComponent<Text>();
                    }
                    UserNamesInLeaderboard1 = new string[Data1.Length];
                    ScoreInLeaderboard1 = new int[Data1.Length];
                    for (int i = 0; i < Data1.Length; i++)
                    {
                        string[] DataInfo1 = Data1[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard1[i] = DataInfo1[0];
                        int.TryParse(DataInfo1[1], out ScoreInLeaderboard1[i]);
                        HighScoreFieldsName1[i].text = UserNamesInLeaderboard1[i];
                        HighScoreFieldsScore1[i].text = ScoreInLeaderboard1[i].ToString();
                    }
                    UserNameInLeaderboard1 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard1)
                    {
                        UserNameInLeaderboard1 = UserNameInLeaderboard1 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent1)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard1; i++)
                    {
                        HighScoreFieldsPerent1[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[0].SetActive(false);
                SM.MainMenuScript.HighScoreRoot.SetActive(false);
                SM.FadeManager.FadeOut();
                break;
            case 2:
                string[] Data2 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data2[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data2[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data2[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data2[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    HighScoreFieldsPerent2 = GameObject.FindGameObjectsWithTag("HighScoreMenu2");
                    HighScoreFieldsName2 = new Text[HighScoreFieldsPerent2.Length];
                    HighScoreFieldsScore2 = new Text[HighScoreFieldsPerent2.Length];
                    GameObject[] TempTextName2 = GameObject.FindGameObjectsWithTag("HighScoreTextName2");
                    GameObject[] TempTextScore2 = GameObject.FindGameObjectsWithTag("HighScoreTextScore2");
                    for (int i = 0; i < HighScoreFieldsPerent2.Length; i++)
                    {
                        HighScoreFieldsName2[i] = TempTextName2[i].GetComponent<Text>();
                        HighScoreFieldsScore2[i] = TempTextScore2[i].GetComponent<Text>();
                    }
                    UserNamesInLeaderboard2 = new string[Data2.Length];
                    ScoreInLeaderboard2 = new int[Data2.Length];
                    for (int i = 0; i < Data2.Length; i++)
                    {
                        Debug.Log(Data2[i]);
                        string[] DataInfo2 = Data2[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard2[i] = DataInfo2[0];
                        Debug.Log(DataInfo2[0] + " " + DataInfo2[1]);
                        int.TryParse(DataInfo2[1], out ScoreInLeaderboard2[i]);
                        HighScoreFieldsName2[i].text = UserNamesInLeaderboard2[i];
                        HighScoreFieldsScore2[i].text = ScoreInLeaderboard2[i].ToString();
                    }
                    UserNameInLeaderboard2 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard2)
                    {
                        UserNameInLeaderboard2 = UserNameInLeaderboard2 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent2)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard2; i++)
                    {
                        HighScoreFieldsPerent2[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[1].SetActive(false);
                SM.MainMenuScript.HighScoreRoot.SetActive(false);
                SM.FadeManager.FadeOut();
                break;
            case 3:
                string[] Data3 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data3[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data3[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data3[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data3[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    HighScoreFieldsPerent3 = GameObject.FindGameObjectsWithTag("HighScoreMenu3");
                    HighScoreFieldsName3 = new Text[HighScoreFieldsPerent3.Length];
                    HighScoreFieldsScore3 = new Text[HighScoreFieldsPerent3.Length];
                    GameObject[] TempTextName3 = GameObject.FindGameObjectsWithTag("HighScoreTextName3");
                    GameObject[] TempTextScore3 = GameObject.FindGameObjectsWithTag("HighScoreTextScore3");
                    for (int i = 0; i < HighScoreFieldsPerent3.Length; i++)
                    {
                        HighScoreFieldsName3[i] = TempTextName3[i].GetComponent<Text>();
                        HighScoreFieldsScore3[i] = TempTextScore3[i].GetComponent<Text>();
                    }
                    UserNamesInLeaderboard3 = new string[Data3.Length];
                    ScoreInLeaderboard3 = new int[Data3.Length];
                    for (int i = 0; i < Data3.Length; i++)
                    {
                        Debug.Log(Data3[i]);
                        string[] DataInfo3 = Data3[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard3[i] = DataInfo3[0];
                        Debug.Log(DataInfo3[0] + " " + DataInfo3[1]);
                        int.TryParse(DataInfo3[1], out ScoreInLeaderboard3[i]);
                        HighScoreFieldsName3[i].text = UserNamesInLeaderboard3[i];
                        HighScoreFieldsScore3[i].text = ScoreInLeaderboard3[i].ToString();
                    }
                    UserNameInLeaderboard3 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard3)
                    {
                        UserNameInLeaderboard3 = UserNameInLeaderboard3 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent3)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard3; i++)
                    {
                        HighScoreFieldsPerent3[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[2].SetActive(false);
                SM.MainMenuScript.HighScoreRoot.SetActive(false);
                SM.FadeManager.FadeOut();
                break;
            case 4:
                string[] Data4 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data4[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data4[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data4[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data4[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    HighScoreFieldsPerent4 = GameObject.FindGameObjectsWithTag("HighScoreMenu4");
                    HighScoreFieldsName4 = new Text[HighScoreFieldsPerent4.Length];
                    HighScoreFieldsScore4 = new Text[HighScoreFieldsPerent4.Length];
                    GameObject[] TempTextName4 = GameObject.FindGameObjectsWithTag("HighScoreTextName4");
                    GameObject[] TempTextScore4 = GameObject.FindGameObjectsWithTag("HighScoreTextScore4");
                    for (int i = 0; i < HighScoreFieldsPerent4.Length; i++)
                    {
                        HighScoreFieldsName4[i] = TempTextName4[i].GetComponent<Text>();
                        HighScoreFieldsScore4[i] = TempTextScore4[i].GetComponent<Text>();
                    }
                    UserNamesInLeaderboard4 = new string[Data4.Length];
                    ScoreInLeaderboard4 = new int[Data4.Length];
                    for (int i = 0; i < Data4.Length; i++)
                    {
                        Debug.Log(Data4[i]);
                        string[] DataInfo4 = Data4[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard4[i] = DataInfo4[0];
                        Debug.Log(DataInfo4[0] + " " + DataInfo4[1]);
                        int.TryParse(DataInfo4[1], out ScoreInLeaderboard4[i]);
                        HighScoreFieldsName4[i].text = UserNamesInLeaderboard4[i];
                        HighScoreFieldsScore4[i].text = ScoreInLeaderboard4[i].ToString();
                    }
                    UserNameInLeaderboard4 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard4)
                    {
                        UserNameInLeaderboard4 = UserNameInLeaderboard4 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent4)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard4; i++)
                    {
                        HighScoreFieldsPerent4[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[3].SetActive(false);
                SM.MainMenuScript.HighScoreRoot.SetActive(false);
                SM.FadeManager.FadeOut();
                break;
            case 5:
                string[] Data5 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data5[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data5[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data5[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data5[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    HighScoreFieldsPerent5 = GameObject.FindGameObjectsWithTag("HighScoreMenu5");
                    HighScoreFieldsName5 = new Text[HighScoreFieldsPerent5.Length];
                    HighScoreFieldsScore5 = new Text[HighScoreFieldsPerent5.Length];
                    GameObject[] TempTextName5 = GameObject.FindGameObjectsWithTag("HighScoreTextName5");
                    GameObject[] TempTextScore5 = GameObject.FindGameObjectsWithTag("HighScoreTextScore5");
                    for (int i = 0; i < HighScoreFieldsPerent5.Length; i++)
                    {
                        HighScoreFieldsName5[i] = TempTextName5[i].GetComponent<Text>();
                        HighScoreFieldsScore5[i] = TempTextScore5[i].GetComponent<Text>();
                    }
                    UserNamesInLeaderboard5 = new string[Data5.Length];
                    ScoreInLeaderboard5 = new int[Data5.Length];
                    for (int i = 0; i < Data5.Length; i++)
                    {
                        Debug.Log(Data5[i]);
                        string[] DataInfo5 = Data5[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard5[i] = DataInfo5[0];
                        Debug.Log(DataInfo5[0] + " " + DataInfo5[1]);
                        int.TryParse(DataInfo5[1], out ScoreInLeaderboard5[i]);
                        HighScoreFieldsName5[i].text = UserNamesInLeaderboard5[i];
                        HighScoreFieldsScore5[i].text = ScoreInLeaderboard5[i].ToString();
                    }
                    UserNameInLeaderboard5 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard5)
                    {
                        UserNameInLeaderboard5 = UserNameInLeaderboard5 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent5)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard5; i++)
                    {
                        HighScoreFieldsPerent5[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[4].SetActive(false);
                SM.MainMenuScript.HighScoreRoot.SetActive(false);
                SM.FadeManager.FadeOut();
                break;
        }
    }
    public void RefreshLeaderboardOnUpload()
    {
        HighScoreRoot1.SetActive(false);
        HighScoreRoot2.SetActive(false);
        //HighScoreRoot3.SetActive(false);
       // HighScoreRoot4.SetActive(false);
        HighScoreRoot5.SetActive(false);
        switch (LeaderboardValue)
        {
            case 1:
                SM.MainMenuScript.HighScoreLoading[0].SetActive(true);
                HighScoreRoot1.SetActive(true);
                break;
            case 2:
                SM.MainMenuScript.HighScoreLoading[1].SetActive(true);
                HighScoreRoot2.SetActive(true);
                break;
            case 5:
                SM.MainMenuScript.HighScoreLoading[4].SetActive(true);
                HighScoreRoot5.SetActive(true);
                break;
        }
        StartCoroutine(RefreshLeaderboardWebRequest());
    }
    IEnumerator RefreshLeaderboardWebRequest()
    {
        switch (LeaderboardValue)
        {
            case 1:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode1 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        yield return webRequest.SendWebRequest();
                        RefreshLeaderboard(webRequest.downloadHandler.text);
                    }
                }
                break;
            case 2:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode2 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        yield return webRequest.SendWebRequest();
                        RefreshLeaderboard(webRequest.downloadHandler.text);
                    }
                }
                break;
            case 3:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode3 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        yield return webRequest.SendWebRequest();
                        RefreshLeaderboard(webRequest.downloadHandler.text);
                    }
                }
                break;
            case 4:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode4 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        yield return webRequest.SendWebRequest();
                        RefreshLeaderboard(webRequest.downloadHandler.text);
                    }
                }
                break;
            case 5:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode5 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        yield return webRequest.SendWebRequest();
                        RefreshLeaderboard(webRequest.downloadHandler.text);
                    }
                }
                break;

        }
    }
    public void RefreshLeaderboard(string DownloadedData)
    {
        switch (LeaderboardValue)
        {
            case 1:
                string[] Data1 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data1[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data1[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data1[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data1[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    UserNamesInLeaderboard1 = new string[Data1.Length];
                    ScoreInLeaderboard1 = new int[Data1.Length];
                    for (int i = 0; i < Data1.Length; i++)
                    {
                        Debug.Log(Data1[i]);
                        string[] DataInfo1 = Data1[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard1[i] = DataInfo1[0];
                        Debug.Log(DataInfo1[0] + " " + DataInfo1[1]);
                        int.TryParse(DataInfo1[1], out ScoreInLeaderboard1[i]);
                        HighScoreFieldsName1[i].text = UserNamesInLeaderboard1[i];
                        HighScoreFieldsScore1[i].text = ScoreInLeaderboard1[i].ToString();
                    }
                    UserNameInLeaderboard1 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard1)
                    {
                        UserNameInLeaderboard1 = UserNameInLeaderboard1 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent1)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard1; i++)
                    {
                        HighScoreFieldsPerent1[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[0].SetActive(false);
                SM.FadeManager.FadeOut();
                break;
            case 2:
                string[] Data2 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data2[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data2[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data2[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data2[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    UserNamesInLeaderboard2 = new string[Data2.Length];
                    ScoreInLeaderboard2 = new int[Data2.Length];
                    for (int i = 0; i < Data2.Length; i++)
                    {
                        Debug.Log(Data2[i]);
                        string[] DataInfo2 = Data2[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard2[i] = DataInfo2[0];
                        Debug.Log(DataInfo2[0] + " " + DataInfo2[1]);
                        int.TryParse(DataInfo2[1], out ScoreInLeaderboard2[i]);
                        HighScoreFieldsName2[i].text = UserNamesInLeaderboard2[i];
                        HighScoreFieldsScore2[i].text = ScoreInLeaderboard2[i].ToString();
                    }
                    UserNameInLeaderboard2 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard2)
                    {
                        UserNameInLeaderboard2 = UserNameInLeaderboard2 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent2)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard2; i++)
                    {
                        HighScoreFieldsPerent2[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[1].SetActive(false);
                SM.FadeManager.FadeOut();
                break;
            case 3:
                string[] Data3 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data3[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data3[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data3[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data3[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    UserNamesInLeaderboard3 = new string[Data3.Length];
                    ScoreInLeaderboard3 = new int[Data3.Length];
                    for (int i = 0; i < Data3.Length; i++)
                    {
                        Debug.Log(Data3[i]);
                        string[] DataInfo3 = Data3[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard3[i] = DataInfo3[0];
                        Debug.Log(DataInfo3[0] + " " + DataInfo3[1]);
                        int.TryParse(DataInfo3[1], out ScoreInLeaderboard3[i]);
                        HighScoreFieldsName3[i].text = UserNamesInLeaderboard3[i];
                        HighScoreFieldsScore3[i].text = ScoreInLeaderboard3[i].ToString();
                    }
                    UserNameInLeaderboard3 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard3)
                    {
                        UserNameInLeaderboard3 = UserNameInLeaderboard3 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent3)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard3; i++)
                    {
                        HighScoreFieldsPerent3[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[2].SetActive(false);
                SM.FadeManager.FadeOut();
                break;
            case 4:
                string[] Data4 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data4[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data4[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data4[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data4[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    UserNamesInLeaderboard4 = new string[Data4.Length];
                    ScoreInLeaderboard4 = new int[Data4.Length];
                    for (int i = 0; i < Data4.Length; i++)
                    {
                        Debug.Log(Data4[i]);
                        string[] DataInfo4 = Data4[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard4[i] = DataInfo4[0];
                        Debug.Log(DataInfo4[0] + " " + DataInfo4[1]);
                        int.TryParse(DataInfo4[1], out ScoreInLeaderboard4[i]);
                        HighScoreFieldsName4[i].text = UserNamesInLeaderboard4[i];
                        HighScoreFieldsScore4[i].text = ScoreInLeaderboard4[i].ToString();
                    }
                    UserNameInLeaderboard4 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard4)
                    {
                        UserNameInLeaderboard4 = UserNameInLeaderboard4 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent4)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard4; i++)
                    {
                        HighScoreFieldsPerent4[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[3].SetActive(false);
                SM.FadeManager.FadeOut();
                break;
            case 5:
                string[] Data5 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data5[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data5[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data5[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data5[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                }
                else
                {
                    SM.MainMenuScript.HighScoreRoot.SetActive(true);
                    UserNamesInLeaderboard5 = new string[Data5.Length];
                    ScoreInLeaderboard5 = new int[Data5.Length];
                    for (int i = 0; i < Data5.Length; i++)
                    {
                        Debug.Log(Data5[i]);
                        string[] DataInfo5 = Data5[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard5[i] = DataInfo5[0];
                        Debug.Log(DataInfo5[0] + " " + DataInfo5[1]);
                        int.TryParse(DataInfo5[1], out ScoreInLeaderboard5[i]);
                        HighScoreFieldsName5[i].text = UserNamesInLeaderboard5[i];
                        HighScoreFieldsScore5[i].text = ScoreInLeaderboard5[i].ToString();
                    }
                    UserNameInLeaderboard5 = 0;
                    foreach (var CheckedUserName in UserNamesInLeaderboard5)
                    {
                        UserNameInLeaderboard5 = UserNameInLeaderboard5 + 1;
                    }
                    foreach (GameObject HighScoreFieldPerent in HighScoreFieldsPerent5)
                    {
                        HighScoreFieldPerent.SetActive(false);
                    }
                    for (int i = 0; i < UserNameInLeaderboard5; i++)
                    {
                        HighScoreFieldsPerent5[i].SetActive(true);
                    }
                }
                SM.MainMenuScript.HighScoreLoading[4].SetActive(false);
                //SM.FadeManager.FadeOut();
                break;
        }
    }
    void TestHighScore()
    {
        AddNewHighScore(UsernameForTesting, ScoreForTesting);
    }
    public void AddNewHighScore(string UserName, int NewScore)
    {
        SM.FadeManager.InstantFadeIn();
        SM.MainMenuScript.HighScoreRoot.SetActive(true);
        StartCoroutine(UserNameDataFetcher(UserName.ToLower(), NewScore, true));
    }
    IEnumerator UploadNewHighScore(string UserName, int NewScore)
    {
        switch (LeaderboardValue)
        {
            case 1:
                Debug.Log("UserName is okay");
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PrivateCode1 + "/add/" + UserName + "/" + NewScore))
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
                        using (UnityWebRequest DownloadedData = UnityWebRequest.Get(WebUrl + PublicCode1 + "/pipe/"))
                        {
                            if (webRequest.isNetworkError)
                            {
                                Debug.Log(": Error: " + webRequest.error);
                            }
                            else
                            {
                                // Request and wait for the desired page.
                                yield return DownloadedData.SendWebRequest();
                            }
                        }
                        SM.MainMenuScript.HighScoreRoot.SetActive(false);
                        yield return new WaitForSecondsRealtime(2);
                        SM.MainMenuScript.HighScoreToggle();
                        YourScore1.SetActive(true);
                        HighScoreName1.text = SM.MainMenuScript.UserName.ToString();
                        HighScoreScore1.text = Score.ToString();
                        SM.FadeManager.FadeOut();
                    }
                }
                break;
            case 2:
                Debug.Log("UserName is okay");
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PrivateCode2 + "/add/" + UserName + "/" + NewScore))
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
                        using (UnityWebRequest DownloadedData = UnityWebRequest.Get(WebUrl + PublicCode2 + "/pipe/"))
                        {
                            if (webRequest.isNetworkError)
                            {
                                Debug.Log(": Error: " + webRequest.error);
                            }
                            else
                            {
                                // Request and wait for the desired page.
                                yield return DownloadedData.SendWebRequest();
                            }
                        }
                        SM.MainMenuScript.HighScoreRoot.SetActive(false);
                        yield return new WaitForSecondsRealtime(2);
                        SM.MainMenuScript.HighScoreToggle();
                        YourScore2.SetActive(true);
                        HighScoreName2.text = SM.MainMenuScript.UserName.ToString();
                        HighScoreScore2.text = Score.ToString();
                        SM.FadeManager.FadeOut();
                    }
                }
                break;
            case 3:
                Debug.Log("UserName is okay");
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PrivateCode3 + "/add/" + UserName + "/" + NewScore))
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
                        using (UnityWebRequest DownloadedData = UnityWebRequest.Get(WebUrl + PublicCode3 + "/pipe/"))
                        {
                            if (webRequest.isNetworkError)
                            {
                                Debug.Log(": Error: " + webRequest.error);
                            }
                            else
                            {
                                // Request and wait for the desired page.
                                yield return DownloadedData.SendWebRequest();
                            }
                        }
                        SM.MainMenuScript.HighScoreRoot.SetActive(false);
                        yield return new WaitForSecondsRealtime(2);
                        SM.MainMenuScript.HighScoreToggle();
                        YourScore3.SetActive(true);
                        HighScoreName3.text = SM.MainMenuScript.UserName.ToString();
                        HighScoreScore3.text = Score.ToString();
                        SM.FadeManager.FadeOut();
                    }
                }
                break;
            case 4:
                Debug.Log("UserName is okay");
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PrivateCode4 + "/add/" + UserName + "/" + NewScore))
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
                        using (UnityWebRequest DownloadedData = UnityWebRequest.Get(WebUrl + PublicCode4 + "/pipe/"))
                        {
                            if (webRequest.isNetworkError)
                            {
                                Debug.Log(": Error: " + webRequest.error);
                            }
                            else
                            {
                                // Request and wait for the desired page.
                                yield return DownloadedData.SendWebRequest();
                            }
                        }
                        SM.MainMenuScript.HighScoreRoot.SetActive(false);
                        yield return new WaitForSecondsRealtime(2);
                        SM.MainMenuScript.HighScoreToggle();
                        YourScore4.SetActive(true);
                        HighScoreName4.text = SM.MainMenuScript.UserName.ToString();
                        HighScoreScore4.text = Score.ToString();
                        SM.FadeManager.FadeOut();
                    }
                }
                break;
            case 5:
                Debug.Log("UserName is okay");
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PrivateCode5 + "/add/" + UserName + "/" + NewScore))
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
                        using (UnityWebRequest DownloadedData = UnityWebRequest.Get(WebUrl + PublicCode5 + "/pipe/"))
                        {
                            if (webRequest.isNetworkError)
                            {
                                Debug.Log(": Error: " + webRequest.error);
                            }
                            else
                            {
                                // Request and wait for the desired page.
                                yield return DownloadedData.SendWebRequest();
                            }
                        }
                        SM.MainMenuScript.HighScoreRoot.SetActive(false);
                        yield return new WaitForSecondsRealtime(2);
                        SM.MainMenuScript.HighScoreToggle();
                        YourScore5.SetActive(true);
                        HighScoreName5.text = SM.MainMenuScript.UserName.ToString();
                        HighScoreScore5.text = Score.ToString();
                        SM.FadeManager.FadeOut();
                    }
                }
                break;
        }
    }
    public void CheckUserNameFromMainMenu(string UserName)
    {
        StartCoroutine(UserNameDataFetcher(UserName.ToLower().Trim(), 0, false));
    }
    IEnumerator UserNameDataFetcher(string UserName, int NewScore, bool NewScoreInput)
    {
        switch (SM.LevelScript.LevelValue)
        {
            case 1:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode1 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();

                        UserNameChecker(webRequest.downloadHandler.text, UserName, NewScore, NewScoreInput);
                    }
                }
                break;
            case 2:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode2 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();

                        UserNameChecker(webRequest.downloadHandler.text, UserName, NewScore, NewScoreInput);
                    }
                }
                break;
            case 3:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode3 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();

                        UserNameChecker(webRequest.downloadHandler.text, UserName, NewScore, NewScoreInput);
                    }
                }
                break;
            case 4:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode4 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();

                        UserNameChecker(webRequest.downloadHandler.text, UserName, NewScore, NewScoreInput);
                    }
                }
                break;
            case 5:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(WebUrl + PublicCode5 + "/pipe/"))
                {
                    if (webRequest.isNetworkError)
                    {
                        Debug.Log(": Error: " + webRequest.error);
                    }
                    else
                    {
                        // Request and wait for the desired page.
                        yield return webRequest.SendWebRequest();

                        UserNameChecker(webRequest.downloadHandler.text, UserName, NewScore, NewScoreInput);
                    }
                }
                break;
        }
    }
    void UserNameChecker(string DownloadedData, string UserName, int NewScore, bool NewScoreInput)
    {
        switch (LeaderboardValue)
        {
            case 1:
                bool UserNameisTaken1 = false;
                bool UserNameisNotSutable1 = false;
                string[] Data1 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data1[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data1[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data1[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data1[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                    SM.MainMenuScript.ServerIsDown();
                }
                else
                {
                    UserNamesInLeaderboard1 = new string[Data1.Length];
                    ScoreInLeaderboard1 = new int[Data1.Length];
                    UserNameInLeaderboard1 = 0;
                    for (int i = 0; i < Data1.Length; i++)
                    {
                        string[] DataInfo1 = Data1[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard1[i] = DataInfo1[0];
                        UserNameInLeaderboard1 = UserNameInLeaderboard1 + 1;
                        int.TryParse(DataInfo1[1], out ScoreInLeaderboard1[i]);
                    }
                    foreach (var CheckedUserName in UserNamesInLeaderboard1)
                    {
                        if (CheckedUserName.Equals(UserName))
                        {
                            Debug.Log("Username AllreadyTaken");
                            UserNameisTaken1 = true;
                        }
                    }
                    foreach (var CheckedBadWords in BadWords)
                    {
                        if (CheckedBadWords.ToString().Trim().ToLower().Equals(UserName.Trim().ToLower()))
                        {
                            UserNameisNotSutable1 = true;
                        }
                    }
                    if (!UserNameisTaken1 && !UserNameisNotSutable1)
                    {
                        if (NewScoreInput)
                        {
                            StartCoroutine(UploadNewHighScore(UserName, NewScore));
                        }
                        else
                        {
                            SM.MainMenuScript.UserNameIsOkay();
                        }
                    }
                    else if (UserNameisTaken1)
                    {
                        if (NewScoreInput)
                        {
                            UserNameTaken();
                        }
                        else
                        {
                            SM.MainMenuScript.UserNameNotAvailable();
                        }
                    }
                    else if (UserNameisNotSutable1)
                    {
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
                break;
            case 2:
                bool UserNameisTaken2 = false;
                bool UserNameisNotSutable2 = false;
                string[] Data2 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data2[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data2[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data2[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data2[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                    SM.MainMenuScript.ServerIsDown();
                }
                else
                {
                    UserNamesInLeaderboard2 = new string[Data2.Length];
                    ScoreInLeaderboard2 = new int[Data2.Length];
                    UserNameInLeaderboard2 = 0;
                    for (int i = 0; i < Data2.Length; i++)
                    {
                        string[] DataInfo2 = Data2[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard2[i] = DataInfo2[0];
                        UserNameInLeaderboard2 = UserNameInLeaderboard2 + 1;
                        int.TryParse(DataInfo2[1], out ScoreInLeaderboard2[i]);
                    }
                    foreach (var CheckedUserName in UserNamesInLeaderboard2)
                    {
                        Debug.Log(CheckedUserName + " " + UserName);
                        if (CheckedUserName.Equals(UserName))
                        {
                            Debug.Log("Username AllreadyTaken");
                            UserNameisTaken2 = true;
                        }
                    }
                    Debug.Log(UserName);
                    foreach (var CheckedBadWords in BadWords)
                    {
                        if (CheckedBadWords.ToString().Trim().ToLower().Equals(UserName.Trim().ToLower()))
                        {
                            Debug.Log("Username AllreadyTaken");
                            UserNameisNotSutable2 = true;
                        }
                    }
                    if (!UserNameisTaken2 && !UserNameisNotSutable2)
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
                    else if (UserNameisTaken2)
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
                    else if (UserNameisNotSutable2)
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
                break;
            case 3:
                bool UserNameisTaken3 = false;
                bool UserNameisNotSutable3 = false;
                string[] Data3 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data3[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data3[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data3[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data3[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                    SM.MainMenuScript.ServerIsDown();
                }
                else
                {
                    UserNamesInLeaderboard3 = new string[Data3.Length];
                    ScoreInLeaderboard3 = new int[Data3.Length];
                    UserNameInLeaderboard3 = 0;
                    for (int i = 0; i < Data3.Length; i++)
                    {
                        string[] DataInfo3 = Data3[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard3[i] = DataInfo3[0];
                        UserNameInLeaderboard3 = UserNameInLeaderboard3 + 1;
                        int.TryParse(DataInfo3[1], out ScoreInLeaderboard3[i]);
                    }
                    foreach (var CheckedUserName in UserNamesInLeaderboard3)
                    {
                        Debug.Log(CheckedUserName + " " + UserName);
                        if (CheckedUserName.Equals(UserName))
                        {
                            Debug.Log("Username AllreadyTaken");
                            UserNameisTaken3 = true;
                        }
                    }
                    Debug.Log(UserName);
                    foreach (var CheckedBadWords in BadWords)
                    {
                        if (CheckedBadWords.ToString().Trim().ToLower().Equals(UserName.Trim().ToLower()))
                        {
                            Debug.Log("Username AllreadyTaken");
                            UserNameisNotSutable3 = true;
                        }
                    }
                    if (!UserNameisTaken3 && !UserNameisNotSutable3)
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
                    else if (UserNameisTaken3)
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
                    else if (UserNameisNotSutable3)
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
                break;
            case 4:
                bool UserNameisTaken4 = false;
                bool UserNameisNotSutable4 = false;
                string[] Data4 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data4[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data4[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data4[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data4[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                    SM.MainMenuScript.ServerIsDown();
                }
                else
                {
                    UserNamesInLeaderboard4 = new string[Data4.Length];
                    ScoreInLeaderboard4 = new int[Data4.Length];
                    UserNameInLeaderboard4 = 0;
                    for (int i = 0; i < Data4.Length; i++)
                    {
                        string[] DataInfo4 = Data4[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard4[i] = DataInfo4[0];
                        UserNameInLeaderboard4 = UserNameInLeaderboard4 + 1;
                        int.TryParse(DataInfo4[1], out ScoreInLeaderboard4[i]);
                    }
                    foreach (var CheckedUserName in UserNamesInLeaderboard4)
                    {
                        Debug.Log(CheckedUserName + " " + UserName);
                        if (CheckedUserName.Equals(UserName))
                        {
                            Debug.Log("Username AllreadyTaken");
                            UserNameisTaken4 = true;
                        }
                    }
                    Debug.Log(UserName);
                    foreach (var CheckedBadWords in BadWords)
                    {
                        if (CheckedBadWords.ToString().Trim().ToLower().Equals(UserName.Trim().ToLower()))
                        {
                            Debug.Log("Username AllreadyTaken");
                            UserNameisNotSutable4 = true;
                        }
                    }
                    if (!UserNameisTaken4 && !UserNameisNotSutable4)
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
                    else if (UserNameisTaken4)
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
                    else if (UserNameisNotSutable4)
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
                break;
            case 5:
                bool UserNameisTaken5 = false;
                bool UserNameisNotSutable5 = false;
                string[] Data5 = DownloadedData.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (Data5[0].ToString().Trim().Equals(ErrorCode1.ToString().Trim()) ||
                    Data5[0].ToString().Trim().Equals(ErrorCode2.ToString().Trim()) ||
                    Data5[0].ToString().Trim().Equals(ErrorCode3.ToString().Trim()) ||
                    Data5[0].ToString().Trim().Equals(ErrorCode4.ToString().Trim()))
                {
                    Debug.Log("ServerDown");
                    SM.MainMenuScript.ServerIsDown();
                }
                else
                {
                    UserNamesInLeaderboard5 = new string[Data5.Length];
                    ScoreInLeaderboard5 = new int[Data5.Length];
                    UserNameInLeaderboard5= 0;
                    for (int i = 0; i < Data5.Length; i++)
                    {
                        string[] DataInfo5 = Data5[i].Split(new char[] { '|' });
                        UserNamesInLeaderboard5[i] = DataInfo5[0];
                        UserNameInLeaderboard5 = UserNameInLeaderboard5 + 1;
                        int.TryParse(DataInfo5[1], out ScoreInLeaderboard5[i]);
                    }
                    foreach (var CheckedUserName in UserNamesInLeaderboard5)
                    {
                        Debug.Log(CheckedUserName + " " + UserName);
                        if (CheckedUserName.Equals(UserName))
                        {
                            Debug.Log("Username AllreadyTaken");
                            UserNameisTaken5 = true;
                        }
                    }
                    Debug.Log(UserName);
                    foreach (var CheckedBadWords in BadWords)
                    {
                        if (CheckedBadWords.ToString().Trim().ToLower().Equals(UserName.Trim().ToLower()))
                        {
                            Debug.Log("Username AllreadyTaken");
                            UserNameisNotSutable5 = true;
                        }
                    }
                    if (!UserNameisTaken5 && !UserNameisNotSutable5)
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
                    else if (UserNameisTaken5)
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
                    else if (UserNameisNotSutable5)
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
                break;
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
