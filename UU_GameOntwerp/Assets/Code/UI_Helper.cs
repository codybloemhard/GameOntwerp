using UnityEngine.UI;
using UnityEngine;

public class UI_Helper : MonoBehaviour {

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Text text;
    private float timer;
    private int collection, msg, index, prevCollection;
    private int[][] collections = new int[][] { new int[] { 0, 1, 2, 3, 4 }, new int[] { 0, 5, 6, 7 }, new int[] { 0, 1, 3, 8} };
    private string[] msgs = new string[] { "Move with WASD", "Fly with Q/E", "Hold left mouse button to drag object around",
    "Use the ScrollWheel to select items in your inventory", "Use right mouse button to spawn a item", "Shoot with left mouse",
    "Try to hit the enemy's treasure", "Press SPACE to jump", "Use right mouse to buy an item if you have money enough" };
    
    public void Awake()
    {
        panel.active = false;
        text.text = "";
        collection = -1;
    }
    
    public void SetNeedHelp(bool need)
    {
        TutFlag.instance.needHelp = need;
        Debug.Log("new help val: " + TutFlag.instance.needHelp);
    }

    private void Update () {
        if (Center.instance == null) return;

        prevCollection = collection;
        Phase phase = Center.instance.GetPhase();
        if (phase == Phase.BUILDING)
            collection = 0;
        else if (phase == Phase.PLAYING)
            collection = 1;
        else if (phase == Phase.UPGRADE)
            collection = 2;
        else collection = -1;
        
        if (!TutFlag.instance.needHelp || collection == -1 || phase == Phase.PREGAME)
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
            if (index > collections[collection].Length - 1)
                index = 0;
            msg = collections[collection][index];
        }
        panel.active = true;
        text.text = msgs[msg];
    }
}