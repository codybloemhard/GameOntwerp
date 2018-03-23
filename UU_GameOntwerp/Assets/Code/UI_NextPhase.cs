using UnityEngine;
using UnityEngine.UI;

public class UI_NextPhase : MonoBehaviour {

    public Image canvas, border;
    public Text title;
    public float showTime;
    public Sprite buildImg, playImg, upgradeImg;
    private Phase lastPhase;
    private float timer;

	private void Start () {
        lastPhase = Phase.NONE;
        canvas.enabled = false;
        border.enabled = false;
        title.enabled = false;
    }

    private void Update () {
        Phase phase = Center.instance.GetPhase();
        if(phase != lastPhase)
        {
            lastPhase = phase;
            timer = 0f;
            canvas.enabled = true;
            border.enabled = true;
            title.enabled = true;
            if (phase == Phase.BUILDING)
                canvas.sprite = buildImg;
            else if (phase == Phase.PLAYING)
                canvas.sprite = playImg;
            else if (phase == Phase.UPGRADE)
                canvas.sprite = upgradeImg;
            else
            {
                canvas.enabled = false;
                border.enabled = false;
                title.enabled = false;
            }
        }
        timer += Time.deltaTime;
        if (timer > showTime)
        {
            canvas.enabled = false;
            border.enabled = false;
            title.enabled = false;
        }
	}
}