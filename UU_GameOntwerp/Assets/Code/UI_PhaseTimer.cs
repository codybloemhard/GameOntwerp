using UnityEngine;
using UnityEngine.UI;

public class UI_PhaseTimer : MonoBehaviour {
    
    private Text text;
    
    private void Start () {
        text = GetComponent<Text>();
	}

    private void Update () {
        if (text == null)
        {
            Debug.Log("Text is NULL in UI_PhaseTimer");
            return;
        }
        float time = Center.instance.GetTimeLeft();
        if(time < 0)
        {
            text.text = "Infinite";
            return;
        }
        int whole = (int)time;
        text.text = "" + whole + ":" + (int)((time - whole) * 10);
    }
}