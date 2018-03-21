using UnityEngine.UI;
using UnityEngine;

public class UI_ShootPercentage : MonoBehaviour {

    private Slider slider;

    private void Start () {
        slider = GetComponent<Slider>();

    }

    private void Update () {
        slider.value = Center.instance.shootPercentage;
	}
}