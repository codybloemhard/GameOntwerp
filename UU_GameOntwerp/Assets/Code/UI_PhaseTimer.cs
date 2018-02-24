using UnityEngine;
using UnityEngine.UI;

public class UI_PhaseTimer : MonoBehaviour {
    
    private Text text;
    private float time;
    
    private void Start () {
        text = GetComponent<Text>();
        if (text == null)
            Debug.Log("Text is NULL in UI_PhaseTimer");
    }

    private void Update () {
        if (text == null)
            return;
        time = Center.instance.GetTimeLeft();
        if (time == -1)
            text.text = "Infinite";
        else if (time == -2)
            text.text = "Waiting for opponent to connect!";
        else if (time == -3)
            text.text = "";
        else
        {
            int whole = (int)time;
            text.text = "" + whole + ":" + (int)((time - whole) * 10);
        }
    }
}