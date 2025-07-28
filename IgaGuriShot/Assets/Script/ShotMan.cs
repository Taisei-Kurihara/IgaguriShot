using R3;
using UnityEngine;
public class ShotMan : MonoBehaviour
{
    [SerializeField]
    GameObject IgaPrefab;

    void Start()
    {
        Observable
            .EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => ShotIga())
            .AddTo(this);
    }

    private void ShotIga()
    {
        GameObject Iga = Instantiate(IgaPrefab, transform.position, Quaternion.identity);
        Ball ball = Iga.GetComponent<Ball>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        ball.SetFireDir(ray.direction.normalized);
    }



}
