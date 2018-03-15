using UnityEngine.UI;
using UnityEngine;

public class UpgradeInventory : MonoBehaviour {
    
    [SerializeField]
    private int[] costs;
    [SerializeField]
    private Sprite[] images;
    [SerializeField]
    private Image indicator;
    [SerializeField]
    private Text costText;

    private int index;

    private void Start () { }

    private void Update () {
        indicator.sprite = images[index];
        costText.text = "Costs: " + costs[index];
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            DecreaseIndex();
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            IncreaseIndex();
        if (Input.GetMouseButtonDown(1))
            UseCurrent();
    }

    private void DecreaseIndex()
    {
        index--;
        if (index < 0)
            index = costs.Length - 1;
    }

    private void IncreaseIndex()
    {
        index++;
        if (index > costs.Length - 1)
            index = 0;
    }
    
    private void UseCurrent()
    {
        if (costs[index] > Center.instance.money) return;
        Center.instance.money -= costs[index];
        if (index == 0) Center.instance.dmgMultiplier += 0.05f;
        else Center.instance.toBeSpawned = index - 1;
    }
}