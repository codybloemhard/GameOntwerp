using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour {

    [SerializeField]
    private int playerOwner = -1;

	public void InitTreasure(int player)
    {
        if (playerOwner != -1) return;
        playerOwner = player;
    }
    
    private void OnTriggerEnter(Collider o)
    {
        if (playerOwner == -1) return;
        if (!o.name.Contains("bullet")) return;
        if (Center.instance.GetPhase() != Phase.PLAYING) return;
        int bulletFrom = o.name[o.name.Length - 1] - 48;
        if (bulletFrom != playerOwner)
            Center.instance.SetWinner(bulletFrom);
    }
}