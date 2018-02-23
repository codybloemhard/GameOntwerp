using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum Phase
{
    NONE,
    BUILDING,
    PLAYING
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
    //editable vars
    [SerializeField]
    private int roundTime = 30;
    [SerializeField]
    private Treasure targetA, targetB;

    private void Awake () {
        if (instance != null)
            Destroy(this);
        else instance = this;
        phase = Phase.BUILDING;
	}
	
	private void Update () {
        if (!isServer) return;
        timer += Time.deltaTime;
        if (timer >= roundTime)
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
    }

    public float GetTimeLeft()
    {
        return roundTime - timer;
    }
    
    public int GetNewPlayer(string target)
    {
        Debug.Log("NEW PLAYER");
        Treasure t = target == "A" ? targetA : targetB;
        t.InitTreasure(players);
        return players++;
    }

    public void SetWinner(int w)
    {
        CmdWinner(w);
    }

    [Command]
    private void CmdWinner(int w)
    {
        winner = w;
        RpcPrintWinner();
        Debug.Log("Winner: player" + winner);
    }

    [ClientRpc]
    private void RpcPrintWinner()
    {
        Debug.Log("Winner: player" + winner);
    }
}