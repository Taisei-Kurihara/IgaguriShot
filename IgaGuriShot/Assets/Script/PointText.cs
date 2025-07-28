using TMPro;
using UnityEngine;
using R3;
using R3.Triggers;

public class PointText : MonoBehaviour
{
    TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();

        PointManager.Instance.point.Subscribe(value =>
        {
            tmp.text = "Point:"+value.ToString("F0");
        }).AddTo(this);
    }


}
