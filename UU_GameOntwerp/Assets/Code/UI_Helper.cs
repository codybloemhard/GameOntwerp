using UnityEngine.UI;
using UnityEngine;

public class UI_Helper : MonoBehaviour {

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Text text;
    private float timer;
    private int collection, msg, index, prevCollection;
    private int[][] msgs = new int[][] { new int[] { 0, 1, 2, 3 }, new int[] { 0, 4, 5 } };

    public void Awake()
    {
        panel.active = false;
        text.text = "";
        collection = -1;
    }

    public void SetNeedHelp(bool need)
    {
        Center.instance.needHelp = need;
    }

    private void Update () {
        prevCollection = collection;

        Phase phase = Center.instance.GetPhase();
        if (phase == Phase.BUILDING)
            collection = 0;
        else if (phase == Phase.PLAYING)
            collection = 1;
        else collection = -1;

        if (!Center.instance.needHelp || collection == -1 || Center.instance.inventoryOpen || phase == Phase.PREGAME)
        {
            panel.active = false;
            text.text = "";
            return;
        }

        timer += Time.deltaTime;
        if (timer > 5f || prevCollection != collection)
        {
            timer = 0f;
            index++;
            if (index > msgs[collection].Length - 1)
                index = 0;
            msg = msgs[collection][index];
        }

        panel.active = true;

        if (msg == 0)
            text.text = "Move with WASD";
        else if (msg == 1)
            text.text = "Fly with Q/E";
        else if (msg == 2)
            text.text = "Open and close the inventory with F";
        else if (msg == 3)
            text.text = "Hold left mouse button to drag object around";
        else if (msg == 4)
            text.text = "Shoot with left mouse";
        else if (msg == 5)
            text.text = "Try to hit the enemy's treasure";
    }
}