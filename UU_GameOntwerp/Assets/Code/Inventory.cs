using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [SerializeField]
    private int[] amounts;
    [SerializeField]
    private Sprite[] images;
    [SerializeField]
    private Image indicator;
    [SerializeField]
    private Text amountText;

    private int index;

    private void Update () {
        indicator.sprite = images[index];
        amountText.text = "Items left: " + amounts[index];
    }

    public void DecreaseIndex()
    {
        index--;
        if (index < 0)
            index = amounts.Length - 1;
    }

    public void IncreaseIndex()
    {
        index++;
        if (index > amounts.Length - 1)
            index = 0;
    }

    public void SpawnCurrent()
    {
        if (amounts[index] <= 0) return;
        Center.instance.toBeSpawned = index;
        amounts[index]--;
    }
}