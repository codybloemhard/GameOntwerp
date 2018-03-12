using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [SerializeField]
    private int[] amounts;
    private int[] am;
    [SerializeField]
    private Sprite[] images;
    [SerializeField]
    private Image indicator;
    [SerializeField]
    private Text amountText;

    private int index;

    private void Start()
    {
        am = new int[amounts.Length];
        Reset();
    }

    private void Update () {
        indicator.sprite = images[index];
        amountText.text = "Items left: " + am[index];
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            DecreaseIndex();
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            IncreaseIndex();
        if (Input.GetMouseButtonDown(1))
            SpawnCurrent();
    }
    
    private void DecreaseIndex()
    {
        index--;
        if (index < 0)
            index = amounts.Length - 1;
    }

    private void IncreaseIndex()
    {
        index++;
        if (index > amounts.Length - 1)
            index = 0;
    }

    private void SpawnCurrent()
    {
        if (am[index] <= 0) return;
        Center.instance.toBeSpawned = index;
        am[index]--;
    }

    public void Reset()
    {
        for (int i = 0; i < amounts.Length; i++)
            am[i] = amounts[i];
    }
}