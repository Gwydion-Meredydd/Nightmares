using System.Collections;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerModels;

public class MatchmakerManager : MonoBehaviour
{
    private string ticketId;
    private Coroutine pollTicketCoroutine;

    private static string QueueName = "Matchmaking_Queue";
    public ScriptsManager SM;
    public void StartMatchmaking()
    {

        string RandomId = Random.Range(100000, 999999).ToString(); 
        PlayFabMultiplayerAPI.CreateMatchmakingTicket(
            new CreateMatchmakingTicketRequest
            {
                Creator = new MatchmakingPlayer
                {
                    Entity = new EntityKey
                    {
                        Id = SM.logingManager.Entity_ID ,
                        Type = "title_player_account",
                    },
                    Attributes = new MatchmakingPlayerAttributes
                    {
                        DataObject = new {
                            latencies = new object[]
                            {
                                new {
                                    region = "NorthEurope",
                                    latency = 150
                                }
                            }
                        }
                    }
                },

                GiveUpAfterSeconds = 120,

                QueueName = QueueName
            },
            OnMatchmakingTicketCreated,
            OnMatchmakingError
        );
    }

    public void LeaveQueue()
    {
        PlayFabMultiplayerAPI.CancelMatchmakingTicket(
            new CancelMatchmakingTicketRequest
            {
                QueueName = QueueName,
                TicketId = ticketId
            },
            OnTicketCanceled,
            OnMatchmakingError
        );
    }

    private void OnTicketCanceled(CancelMatchmakingTicketResult result)
    {
        //playButton.SetActive(true);
    }

    private void OnMatchmakingTicketCreated(CreateMatchmakingTicketResult result)
    {
        ticketId = result.TicketId;
        pollTicketCoroutine = StartCoroutine(PollTicket(result.TicketId));
        Debug.Log("Matchmaking tiket created");
        //leaveQueueButton.SetActive(true);
       // queueStatusText.text = "Ticket Created";
    }

    private void OnMatchmakingError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private IEnumerator PollTicket(string ticketId)
    {
        while (true)
        {
            Debug.Log("Waiting For Match");
            PlayFabMultiplayerAPI.GetMatchmakingTicket(
                new GetMatchmakingTicketRequest
                {
                    TicketId = ticketId,
                    QueueName = QueueName
                },
                OnGetMatchMakingTicket,
                OnMatchmakingError
            );

            yield return new WaitForSeconds(6);
        }
    }

    private void OnGetMatchMakingTicket(GetMatchmakingTicketResult result)
    {
       // queueStatusText.text = $"Status: {result.Status}";

        switch (result.Status)
        {
            case "Matched":
                StopCoroutine(pollTicketCoroutine);
                StartMatch(result.MatchId);
                break;
            case "Canceled":
                StopCoroutine(pollTicketCoroutine);
                //leaveQueueButton.SetActive(false);
                //queueStatusText.gameObject.SetActive(false);
               // playButton.SetActive(true);
                break;
        }
    }

    private void StartMatch(string matchId)
    {
       // queueStatusText.text = $"Starting Match";

        PlayFabMultiplayerAPI.GetMatch(
            new GetMatchRequest
            {
                MatchId = matchId,
                QueueName = QueueName
            },
            OnGetMatch,
            OnMatchmakingError
        );
    }

    private void OnGetMatch(GetMatchResult result)
    {
        Debug.Log("match found");
        //Debug.Log(result.ServerDetails.ToString());
        Debug.Log(result.MatchId.ToString());
        Debug.Log(result.Members.ToString());
        // queueStatusText.text = $"{result.Members[0].Entity.Id} vs {result.Members[1].Entity.Id}";
        
    }
}
