using System.Collections.Generic;
using System.Drawing;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
public class ShotMan : MonoBehaviour
{
    [SerializeField]
    GameObject IgaPrefab;

    [SerializeField]
    int IgaCount = 100; // Igaの数
    int MaxIgaCount = 100; // Igaの最大数

    [SerializeField]
    private GameObject _ballSimPrefab; // 何でもOK。予測位置を表示するオブジェクト

    private const int SIMULATE_COUNT = 20; // いくつ先までシュミレートするか

    private Vector3 _startPosition; // 発射開始位置
    private List<GameObject> _simuratePointList; // シュミレートするゲームオブジェクトリスト

    private float _shotPower = 10f; // 発射パワー

    GameObject Iga;
    void Start()
    {
        Ball ball = Instantiate(IgaPrefab, transform.position, Quaternion.identity).GetComponent<Ball>();
        _shotPower = ball.Speed;
        Destroy(ball.gameObject);

        MaxIgaCount = IgaCount; // Igaの最大数を設定


        PointManager.Instance.Game
            .Subscribe(_ => 
            {
                if (_ == 0)
                {
                    Init();
                }
            })
            .AddTo(this); // ゲーム開始時にポイントをリセット




        Observable
            .EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => 
            {
                if (IgaCount <= 0) return; // Igaの数が0なら何もしない
                IgaCount--;
                PointManager.Instance.IgaCount.Value = IgaCount; // Igaの数を更新


                ShotIga();
                if (IgaCount <= 0) { AsyncLastShot().Forget(); };

            })
            .AddTo(this);

        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Simulate(IgaPrefab, ray.direction.normalized * _shotPower);
            })
            .AddTo(this);
    }

    private void ShotIga()
    {
        Iga = Instantiate(IgaPrefab, transform.position, Quaternion.identity);
        Ball ball = Iga.GetComponent<Ball>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        ball.SetFireDir(ray.direction.normalized);
        PointManager.Instance.IntIga.Value++; // ポイントを追加
    }

    async UniTask AsyncLastShot()
    {
        await UniTask.WaitUntil(() => PointManager.Instance.IntIga.Value == 0);
        PointManager.Instance.Game.Value = 1;
    }

    // 初期化
    private void Init()
    {
        if (_simuratePointList != null && _simuratePointList.Count > 0)
        {
            foreach (var go in _simuratePointList)
            {
                Destroy(go.gameObject);
            }
        }

        // 位置を表示するオブジェクトを予め作っておく
        if (_ballSimPrefab != null)
        {
            _simuratePointList = new List<GameObject>();
            for (int i = 0; i < SIMULATE_COUNT; i++)
            {
                var go = Instantiate(_ballSimPrefab);
                go.transform.SetParent(this.transform);
                go.transform.position = Vector3.zero;
                _simuratePointList.Add(go);
            }
        }

        IgaCount = MaxIgaCount; // Igaの数をリセット
        PointManager.Instance.IgaCount.Value = IgaCount; // Igaの数を更新

        PointManager.Instance.point.Value = 0f; // ポイントをリセット
    }

    /**
     * 弾道を予測計算する。オブジェクトを再生成せず、位置だけ動かす。
     * targetにはRigidbodyが必須です
     **/
    public void Simulate(GameObject target, Vector3 _vec)
    {
        if (_simuratePointList != null && _simuratePointList.Count > 0)
        {
            // 発射位置を保存する
            _startPosition = transform.position;
            var r = target.GetComponent<Rigidbody>();
            if (r != null)
            {
                // ベクトルはmassで割る
                Vector3 force = (_vec / r.mass);

                //弾道予測の位置に点を移動
                for (int i = 0; i < SIMULATE_COUNT; i++)
                {
                    var t = (i * 0.1f); // 0.5秒ごとの位置を予測。
                    var x = t * force.x;
                    var y = (force.y * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f);
                    var z = t * force.z;

                    _simuratePointList[i].transform.position = _startPosition + new Vector3(x, y, z);
                }
            }
        }
    }


}
