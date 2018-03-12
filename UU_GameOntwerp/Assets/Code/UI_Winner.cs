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
        if (winner == -1)
        {
            text.text = "";
            return;
        }
        int roundNr = Center.instance.GetRoundNr();
        bool roundEnd = Center.instance.GetPhase() == Phase.POSTROUND;
        if (!roundEnd) winner = Center.instance.GetGameWinner();
        string name = Center.instance.GetName(winner) + " !";
        string prestring = roundEnd ? "Winner of round " + (roundNr + 1) + ": " : "Winner of the game: ";
        text.text = prestring + name;
	}
}