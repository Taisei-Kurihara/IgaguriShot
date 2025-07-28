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
    int IgaCount = 100; // Iga�̐�
    int MaxIgaCount = 100; // Iga�̍ő吔

    [SerializeField]
    private GameObject _ballSimPrefab; // ���ł�OK�B�\���ʒu��\������I�u�W�F�N�g

    private const int SIMULATE_COUNT = 20; // ������܂ŃV���~���[�g���邩

    private Vector3 _startPosition; // ���ˊJ�n�ʒu
    private List<GameObject> _simuratePointList; // �V���~���[�g����Q�[���I�u�W�F�N�g���X�g

    private float _shotPower = 10f; // ���˃p���[

    GameObject Iga;
    void Start()
    {
        Ball ball = Instantiate(IgaPrefab, transform.position, Quaternion.identity).GetComponent<Ball>();
        _shotPower = ball.Speed;
        Destroy(ball.gameObject);

        MaxIgaCount = IgaCount; // Iga�̍ő吔��ݒ�


        PointManager.Instance.Game
            .Subscribe(_ => 
            {
                if (_ == 0)
                {
                    Init();
                }
            })
            .AddTo(this); // �Q�[���J�n���Ƀ|�C���g�����Z�b�g




        Observable
            .EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => 
            {
                if (IgaCount <= 0) return; // Iga�̐���0�Ȃ牽�����Ȃ�
                IgaCount--;
                PointManager.Instance.IgaCount.Value = IgaCount; // Iga�̐����X�V


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
        PointManager.Instance.IntIga.Value++; // �|�C���g��ǉ�
    }

    async UniTask AsyncLastShot()
    {
        await UniTask.WaitUntil(() => PointManager.Instance.IntIga.Value == 0);
        PointManager.Instance.Game.Value = 1;
    }

    // ������
    private void Init()
    {
        if (_simuratePointList != null && _simuratePointList.Count > 0)
        {
            foreach (var go in _simuratePointList)
            {
                Destroy(go.gameObject);
            }
        }

        // �ʒu��\������I�u�W�F�N�g��\�ߍ���Ă���
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

        IgaCount = MaxIgaCount; // Iga�̐������Z�b�g
        PointManager.Instance.IgaCount.Value = IgaCount; // Iga�̐����X�V

        PointManager.Instance.point.Value = 0f; // �|�C���g�����Z�b�g
    }

    /**
     * �e����\���v�Z����B�I�u�W�F�N�g���Đ��������A�ʒu�����������B
     * target�ɂ�Rigidbody���K�{�ł�
     **/
    public void Simulate(GameObject target, Vector3 _vec)
    {
        if (_simuratePointList != null && _simuratePointList.Count > 0)
        {
            // ���ˈʒu��ۑ�����
            _startPosition = transform.position;
            var r = target.GetComponent<Rigidbody>();
            if (r != null)
            {
                // �x�N�g����mass�Ŋ���
                Vector3 force = (_vec / r.mass);

                //�e���\���̈ʒu�ɓ_���ړ�
                for (int i = 0; i < SIMULATE_COUNT; i++)
                {
                    var t = (i * 0.1f); // 0.5�b���Ƃ̈ʒu��\���B
                    var x = t * force.x;
                    var y = (force.y * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f);
                    var z = t * force.z;

                    _simuratePointList[i].transform.position = _startPosition + new Vector3(x, y, z);
                }
            }
        }
    }


}
