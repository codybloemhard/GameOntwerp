using UnityEngine;
using UnityEngine.UI;

public class UI_PhaseIndicator : MonoBehaviour {

    private Image image;
    [SerializeField]
    private Sprite buildImg, fightImg, preImg, postImg;

    private void Start () {
        image = GetComponent<Image>();
        if (image == null)
            Debug.Log("Image is NULL in UI_PhaseIndicator");
    }
    
    private void Update () {
        if (image == null)
            return;

        if (Center.instance.GetPhase() == Phase.NONE)
            image.sprite = null;
        else if (Center.instance.GetPhase() == Phase.BUILDING)
            image.sprite = buildImg;
        else if (Center.instance.GetPhase() == Phase.PLAYING)
            image.sprite = fightImg;
        else if (Center.instance.GetPhase() == Phase.PREGAME)
            image.sprite = preImg;
        else if (Center.instance.GetPhase() == Phase.POSTGAME)
            image.sprite = postImg;
        else if (Center.instance.GetPhase() == Phase.POSTROUND)
            image.sprite = postImg;
    }
}