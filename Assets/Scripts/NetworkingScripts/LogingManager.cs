using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class LogingManager : MonoBehaviour
{
    public static string SessionTicket;
    public static string EntityId;
    public string Entity_ID;
    public string GeneratedName;
    public ScriptsManager SM;

    public void CreateAccount()
    {
        GeneratedName = "Guest" + Random.Range(1, 999999).ToString();
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = GeneratedName,
            Email = "guestemail"+ GeneratedName + "@gmail.com",
            Password = "Password"
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            EntityId = result.EntityToken.Entity.Id;
            SignIn();
            //signInDisplay.SetActive(false);
        }, error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
        //SignIn();
    }

    public void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = GeneratedName,
            Password = "Password"
        }, result =>
        {
            SessionTicket = result.SessionTicket;
            EntityId = result.EntityToken.Entity.Id;
            Entity_ID = EntityId;
            Debug.Log("FakeAccountCreatedSuccsefully");
            SM.matchmakerManager.StartMatchmaking();
        }, error =>
        {
            Debug.LogError(error.GenerateErrorReport());
        });
    }
}
