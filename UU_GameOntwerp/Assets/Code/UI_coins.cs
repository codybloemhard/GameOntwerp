using UnityEngine.UI;
using UnityEngine;

public class UI_coins : MonoBehaviour {

    private Text text;

	private void Start () {
        text = GetComponent<Text>();
	}
	
	private void Update () {
        text.text = "" + Center.instance.money;
	}
}