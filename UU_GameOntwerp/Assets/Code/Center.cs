using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum Phase
{
    NONE,
    PREGAME,
    BUILDING,
    PLAYING,
    POSTGAME
}

public class Center : NetworkBehaviour {

    public static Center instance;
    //all networked vars
    [SerializeField]
    [SyncVar]
    public Phase phase;
    [SyncVar]
    private float timer;
    [SyncVar]
    private int players = 0;
    [SyncVar]
    private int winner = -1;
    [SyncVar]
    private int roundTime = 0;
    //editable vars
    [SerializeField]
    private int buildTime = 60, playTime = -1;
    [SerializeField]
    private Treasure targetA, targetB;

    private void Awake () {
        if (instance != null)
            Destroy(this);
        else instance = this;
        phase = Phase.PREGAME;
        SetRoundTimer();
    }
	
	private void Update () {
        if (!isServer) return;
        if (players < 2)//second player not yet connected
        {
            phase = Phase.PREGAME;
            SetRoundTimer();
        }
        else if(phase == Phase.PREGAME)//second player connected, start game
        {
            phase = Phase.BUILDING;
            SetRoundTimer();
        }

        if (roundTime < 0) timer = 0;
        else timer += Time.deltaTime;

        if (timer >= roundTime && roundTime > 0)
        {
            SwitchMode();
            timer = 0f;
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            SwitchMode();
            timer = 0f;
        }
	}

    private void SwitchMode()
    {
        if (phase == Phase.BUILDING) phase = Phase.PLAYING;
        else if (phase == Phase.PLAYING) phase = Phase.BUILDING;
        SetRoundTimer();
    }

    private void SetRoundTimer()
    {
        if (phase == Phase.NONE) roundTime = -1;
        else if (phase == Phase.BUILDING) roundTime = buildTime;
        else if (phase == Phase.PLAYING) roundTime = playTime;
        else if (phase == Phase.PREGAME) roundTime = -2;
        else if (phase == Phase.POSTGAME) roundTime = -3;
    }

    public float GetTimeLeft()
    {
        return roundTime - timer;
    }
    
    public int GetNewPlayer(string target)
    {
        Treasure t = target == "A" ? targetA : targetB;
        t.InitTreasure(players);
        return players++;
    }
    
    public void SetWinner(int w)
    {
        CmdWinner(w);
    }

    public int GetWinner()
    {
        return winner;
    }

    [Command]
    private void CmdWinner(int w)
    {
        winner = w;
        phase = Phase.POSTGAME;
        SetRoundTimer();
        RpcPrintWinner();
        Debug.Log("Winner: player" + winner);
    }

    [ClientRpc]
    private void RpcPrintWinner()
    {
        Debug.Log("Winner: player" + winner);
    }
}