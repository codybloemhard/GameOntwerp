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
    UPGRADE,
    POSTGAME,
    POSTROUND
}

public class Center : NetworkBehaviour {

    public static Center instance;
    //all networked vars
    [SerializeField]
    [SyncVar]
    private Phase phase;
    [SyncVar]
    private float timer;
    [SyncVar]
    private int players = 0;
    [SyncVar]
    private int winner = -1, player0Wins = 0, player1Wins  = 0, gameWinner = -1;
    [SyncVar]
    private int roundTime = 0;
    [SyncVar]
    private int roundNr = 0;
    //editable vars
    [SerializeField]
    private int buildTime = 60, playTime = -1, postRoundTime = 5, upgradeTime = 60;
    [SerializeField]
    private Treasure targetA, targetB;
    [SyncVar]
    private string nameA = "player0", nameB = "player1";
    private int namePointer = 0;
    private string localName = "";
    public int toBeSpawned = -1;
    [SerializeField]
    public BuildInventory inv;
    private List<Dragable> blocks;
    private float dmgA = 0f, dmgB = 0f;
    public float shootPercentage;
    public int money = 100;
    public float dmgMultiplier = 1f;
    [SyncVar]
    public int buildingRound = 0;
    public bool gameStarted = false;
    
    private void Awake () {
        if (instance != null)
            Destroy(this);
        else instance = this;
        DontDestroyOnLoad(gameObject);
        blocks = new List<Dragable>();
        phase = Phase.PREGAME;
        SetRoundTimer();
    }

    private void Update() {
        if (!isServer) return;
        //server only part
        if (players < 2)//second player not yet connected
        {
            phase = Phase.PREGAME;
            SetRoundTimer();
        }
        else if(phase == Phase.PREGAME && !gameStarted)//second player connected, start game
        {
            phase = Phase.BUILDING;
            gameStarted = true;
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

    public void Reset()
    {
        phase = Phase.PREGAME;
        SetRoundTimer();
        timer = 0f;
        players = 0;
        winner = -1;
        player0Wins = 0;
        player1Wins = 0;
        gameWinner = -1;
        nameA = "player0";
        nameB = "player1";
        namePointer = 0;
        roundNr = 0;
        TutFlag.instance.needHelp = false;
        inv.Reset();
        blocks.Clear();
        dmgA = 0f;
        dmgB = 0f;
        money = 100;
        dmgMultiplier = 1f;
        buildingRound = 0;
        gameStarted = false;
    }

    private void SwitchMode()
    {
        if (phase == Phase.BUILDING)
        {
            phase = Phase.PLAYING;
            SaveBlocks();
        }
        else if (phase == Phase.PLAYING)
        {
            SetDamage();
            winner = dmgA > dmgB ? 1 : 0;
            if (winner == 0) player0Wins++;
            else if (winner == 1) player1Wins++;
            gameWinner = player0Wins > player1Wins ? 0 : 1;
            if (player0Wins >= 2 || player1Wins >= 2) phase = Phase.POSTGAME;
            else phase = Phase.POSTROUND;
        }
        else if (phase == Phase.POSTROUND)
        {
            phase = Phase.UPGRADE;
            winner = -1;
            roundNr++;
            buildingRound++;
            DeleteBullets();
            RebuildBlocks();
        }
        else if (phase == Phase.UPGRADE)
        {
            phase = Phase.PLAYING;
        }
        SetRoundTimer();
    }

    private void SetRoundTimer()
    {
        if (phase == Phase.NONE) roundTime = -1;
        else if (phase == Phase.BUILDING) roundTime = buildTime;
        else if (phase == Phase.PLAYING) roundTime = playTime;
        else if (phase == Phase.PREGAME) roundTime = -2;
        else if (phase == Phase.POSTGAME) roundTime = -3;
        else if (phase == Phase.POSTROUND) roundTime = postRoundTime;
        else if (phase == Phase.UPGRADE) roundTime = upgradeTime;
        timer = 0f;
    }

    private void DeleteBullets()
    {
        GameObject[] allBullets = GameObject.FindGameObjectsWithTag("Bullet");
        if (allBullets == null) return;
        for (int i = 0; i < allBullets.Length; i++)
            Destroy(allBullets[i]);
    }

    private void SaveBlocks()
    {
        blocks.Clear();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("BuildingBlock");
        for (int i = 0; i < objects.Length; i++)
        {
            Dragable d = objects[i].GetComponent<Dragable>();
            d.SaveState();
            blocks.Add(d);
        }
        Dragable t0 = targetA.gameObject.GetComponent<Dragable>();
        Dragable t1 = targetB.gameObject.GetComponent<Dragable>();
        t0.SaveState();
        t1.SaveState();
        blocks.Add(t0);
        blocks.Add(t1);
    }

    private void SetDamage()
    {
        dmgA = 0f;
        dmgB = 0f;
        for (int i = 0; i < blocks.Count; i++) {
            Vector2 res = blocks[i].GetDamage();
            if (res.y == 0) dmgA += res.x;
            else if (res.y == 1) dmgB += res.x;
        }
    }

    private void RebuildBlocks()
    {
        for (int i = 0; i < blocks.Count; i++)
            blocks[i].ResetState();
    }
    
    public Phase GetPhase()
    {
        return phase;
    }

    public float GetTimeLeft()
    {
        return roundTime - timer;
    }
    
    public int GetNewPlayer(string target)
    {
        if (players < 2)
        {
            Treasure t = target == "A" ? targetA : targetB;
            t.InitTreasure(players);
        }
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

    public int GetGameWinner()
    {
        return gameWinner;
    }

    public int GetRoundNr()
    {
        return roundNr;
    }

    [Command]
    private void CmdWinner(int w)
    {
        winner = w;
        if (winner == 0) player0Wins++;
        else if (winner == 1) player1Wins++;
        gameWinner = player0Wins > player1Wins ? 0 : 1;
        if (player0Wins >= 2 || player1Wins >= 2) phase = Phase.POSTGAME;
        else phase = Phase.POSTROUND;
        SetRoundTimer();
    }

    public void SetLocalName(string name)
    {
        localName = name;
    }

    public string GetLocalName()
    {
        return localName;
    }
    
    public void SetName(string name)
    {
        if (namePointer == 0) nameA = name;
        else if (namePointer == 1) nameB = name;
        namePointer++;
    }

    public string GetName(int player)
    {
        if (player == 0) return nameA;
        else if (player == 1) return nameB;
        return "";
    }
}