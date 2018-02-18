using UnityEngine;
using UnityEngine.UI;

public class UI_PhaseIndicator : MonoBehaviour {

    private Image image;
    [SerializeField]
    private Sprite buildImg, fightImg;

    private void Start () {
        image = GetComponent<Image>();
	}

    private void Update () {
        if (image == null)
        {
            Debug.Log("Image is NULL in UI_PhaseIndicator");
            return;
        }
        if (Center.instance.phase == Phase.NONE)
            image.sprite = null;
        else if (Center.instance.phase == Phase.BUILDING)
            image.sprite = buildImg;
        else if (Center.instance.phase == Phase.PLAYING)
            image.sprite = fightImg;
    }
}