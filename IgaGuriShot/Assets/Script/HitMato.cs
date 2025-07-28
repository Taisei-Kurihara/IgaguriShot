using R3;
using R3.Triggers;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HitMato : MonoBehaviour
{
    [SerializeField]
    Collider MatoCollider;

    [SerializeField]
    float Point = 1000f;

    void Start()
    {
        MatoCollider.OnCollisionEnterAsObservable()
            .Where(collision => collision.gameObject.GetComponent<Ball>() != null)
            .Subscribe(collision =>
            {
                collision.gameObject.GetComponent<Ball>().StopAndSticky(transform);

                Vector3 colpos = collision.gameObject.transform.position;
                Vector3 matopos = MatoCollider.transform.position;

                float dis = (colpos - matopos).magnitude;

                float min = 0.8f;
                float max = 1.7f;
                float normalized = Mathf.Clamp01((dis - min) / (max - min));
                normalized = Mathf.Abs(normalized - 1); // Normalize to [0, 1] range
                Debug.Log($"Hit Mato! Distance: {normalized}");

                PointManager.Instance.AddPoint(normalized * Point);
            })
            .AddTo(this);
    }
}
