using UnityEngine.UI;
using UnityEngine;

public class UI_Winner : MonoBehaviour {

    private Text text;

    private void Start () {
        text = GetComponent<Text>();
        if (text == null)
            Debug.Log("Text is NULL in UI_Winner");
        else text.text = "";
    }

    private void Update () {
        if (text == null) return;
        int winner = Center.instance.GetWinner();
        if (winner == -1) return;
        text.text = "Winner: " + Center.instance.GetName(winner) + " !";
	}
}