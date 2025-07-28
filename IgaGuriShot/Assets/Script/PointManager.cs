using R3;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager instance { get; private set; }

    public static PointManager Instance
    {
        get
        {
            if (instance == null)
            {
                // �����̃C���X�^���X��T��
                instance = FindObjectOfType<PointManager>();

                // �Ȃ���ΐV�K����
                if (instance == null)
                {
                    GameObject obj = new GameObject("PointManager");
                    instance = obj.AddComponent<PointManager>();
                }
            }
            return instance;
        }
    }

    public ReactiveProperty<float> point = new ReactiveProperty<float>(0f);

    public float Point
    {
        get { return point.Value; }
        set
        {
            point.Value = value;
            Debug.Log($"Point updated: {point}");
        }
    }
    public void AddPoint(float value)
    {
        Point += value;
        Debug.Log($"Point added: {value}, Total: {Point}");
    }
}
