using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPerksManager : MonoBehaviour
{
    [HideInInspector]
    public static ServerPerksManager _serverPerksManager;
    [HideInInspector]
    public ServerPerksManager RefpekrManager;

    public Transform PerkSpawnPoint;
    public bool[] PlayerIsCloseEnough;
    public int CurrentPerkValue;
    public int OldPerkValue;
    public string CurrentMessage;
    [Header("Perk Costs")]
    public int CurrentCostOfPerk;
    [Space]
    public int HealthPerkCost;
    public int IncreasedPointsPerkCost;
    public int SpeedPerkCost;
    public int DamagePerkCost;
 

    private void Start()
    {
        RefpekrManager = this;
        _serverPerksManager = RefpekrManager;
    }

    public void PlayerCloseToPerkMachine(ServerPlayer ClosestPlayer)
    {
        switch (ClosestPlayer.id) 
        {
            case 1:
                PlayerIsCloseEnough[0] = true;
                ServerSend.SendPerkData(ClosestPlayer.id, true, CurrentMessage);
                break;
            case 2:
                PlayerIsCloseEnough[1] = true;
                ServerSend.SendPerkData(ClosestPlayer.id, true, CurrentMessage);
                break;
            case 3:
                PlayerIsCloseEnough[2] = true;
                ServerSend.SendPerkData(ClosestPlayer.id, true, CurrentMessage);
                break;
            case 4:
                PlayerIsCloseEnough[3] = true;
                ServerSend.SendPerkData(ClosestPlayer.id, true, CurrentMessage);
                break;

        }
    }
    public void PlayerIsNotCloseToPerkMachine(ServerPlayer ClosestPlayer)
    {
        switch (ClosestPlayer.id)
        {
            case 1:
                PlayerIsCloseEnough[0] = false;
                ServerSend.SendPerkData(ClosestPlayer.id, false, "");
                break;
            case 2:
                PlayerIsCloseEnough[1] = false;
                ServerSend.SendPerkData(ClosestPlayer.id,false, "");
                break;
            case 3:
                PlayerIsCloseEnough[2] = false;
                ServerSend.SendPerkData(ClosestPlayer.id, false, "");
                break;
            case 4:
                PlayerIsCloseEnough[3] = false;
                ServerSend.SendPerkData(ClosestPlayer.id, false, "");
                break;

        }
    }
    public bool CheckifHasPerk (ServerPlayer RefrencePlayer) 
    {
        bool returnbool = false;
        switch (CurrentPerkValue) 
        {
            case 0:
                returnbool = RefrencePlayer.HealthBaught;
                break;
            case 1:
                returnbool = RefrencePlayer.IncreasedPointsBaught;
                break;
            case 2:
                returnbool = RefrencePlayer.SpeedBaught;
                break;
            case 3:
                returnbool = RefrencePlayer.DamageBaught;
                break;
        }
        return (returnbool);
    }
    public void PlayerTryingToBuyPerk(ServerPlayer RefrencePlayer) 
    {
        int PlayerValue = RefrencePlayer.id - 1;
        if (ServerPoints._serverPoints.Points[PlayerValue] >= CurrentCostOfPerk)
        {
            ServerPoints._serverPoints.Points[PlayerValue] = ServerPoints._serverPoints.Points[PlayerValue] - CurrentCostOfPerk;
            ServerSend.UpdateScore(PlayerValue, ServerPoints._serverPoints.Points[PlayerValue]);
            switch (CurrentPerkValue) 
            {
                case 0:
                    BaughtHealthPerk(RefrencePlayer);
                    ServerSend.SendBaughtPerk(PlayerValue + 1, CurrentPerkValue);
                    break;
                case 1:
                    BaughtIncreasePoints(RefrencePlayer);
                    ServerSend.SendBaughtPerk(PlayerValue + 1, CurrentPerkValue);
                    break;
                case 2:
                    BaughtSpeed(RefrencePlayer);
                    ServerSend.SendBaughtPerk(PlayerValue + 1, CurrentPerkValue);
                    break;
                case 3:
                    DamageBaught(RefrencePlayer);
                    ServerSend.SendBaughtPerk(PlayerValue + 1, CurrentPerkValue);
                    break;

            }
        }
    }
    public void BaughtHealthPerk(ServerPlayer RefrencePlayer) 
    {
        RefrencePlayer.Health = 200;
        RefrencePlayer.CurrentMaxHealth = 200;
        RefrencePlayer.HealthBaught = true;
    }
    public void BaughtIncreasePoints(ServerPlayer RefrencePlayer)
    {
        RefrencePlayer.IncreasedPointsBaught = true;
    }
    public void BaughtSpeed(ServerPlayer RefrencePlayer)
    {
        RefrencePlayer.SpeedBaught = true;
        RefrencePlayer.moveSpeed = RefrencePlayer.moveSpeed * 2;
    }
    public void DamageBaught(ServerPlayer RefrencePlayer)
    {
        RefrencePlayer.DamageBaught = true;
        RefrencePlayer.ARDamage = RefrencePlayer.ARDamage * 2;
        RefrencePlayer.SGDamage = RefrencePlayer.SGDamage * 2;
        RefrencePlayer.SADamage = RefrencePlayer.SADamage * 2;
        RefrencePlayer.MGDamage = RefrencePlayer.MGDamage * 2;
        RefrencePlayer.FTDamage = RefrencePlayer.FTDamage * 2;
    }
    public void SpawnPerkMachine() 
    {
        OldPerkValue = CurrentPerkValue;
        CurrentPerkValue = Random.Range(0, 4);
        switch (CurrentPerkValue) 
        {
            case 0:
                CurrentCostOfPerk = HealthPerkCost;
                CurrentMessage = " Press F to Purchase Health Boost for " + HealthPerkCost;
                break;
            case 1:
                CurrentCostOfPerk = IncreasedPointsPerkCost;
                CurrentMessage = " Press F to Purchase Points Boost for " + IncreasedPointsPerkCost;
                break;
            case 2:
                CurrentCostOfPerk = SpeedPerkCost;
                CurrentMessage = " Press F to Purchase Speed Boost for " + SpeedPerkCost;
                break;
            case 3:
                CurrentCostOfPerk = DamagePerkCost;
                CurrentMessage = " Press F to Purchase Damage Boost for " + DamagePerkCost;
                break;
        }
        if (CurrentPerkValue != OldPerkValue)
        {
            SpawningPerkMachine();
        }
        else
        {
            SpawnPerkMachine();
        }
    }
    public void SpawningPerkMachine() 
    {
        if (PerkSpawnPoint == null) 
        {
            Debug.Log("Spawning Perk Machine");
            GameObject PerkSpawnPointGO = GameObject.FindGameObjectWithTag("PerkSpawnPoint");
            PerkSpawnPoint = PerkSpawnPointGO.transform;
        }
        ServerSend.SendPerkValue(CurrentPerkValue);


    }
    
}
