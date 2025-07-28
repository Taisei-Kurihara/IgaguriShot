using TMPro;
using UnityEngine;
using R3;

public class IgaCountText : MonoBehaviour
{
    TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();

        PointManager.Instance.IgaCount.Subscribe(value =>
        {
            tmp.text = value.ToString("F0") + ":Iga";
        }).AddTo(this);
    }
}
