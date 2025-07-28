using UnityEngine;
using R3;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    [SerializeField]
    GameObject re;

    [SerializeField]
    Button reButton;
    void Start()
    {
        re.SetActive(false); // ������Ԃł͔�\��
        PointManager.Instance.Game
            .Subscribe(_ =>
            {
                if (_ == 0)
                {
                    re.SetActive(false); // �Q�[���I�����Ɍ��ʉ�ʂ�\��
                }
                if (_ == 1)
                {
                    re.SetActive(true); // �Q�[���I�����Ɍ��ʉ�ʂ�\��
                    reButton.onClick.AddListener(() =>
                    {
                        PointManager.Instance.Game.Value = 0; // �Q�[�������Z�b�g
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // �Q�[���V�[�����ēǂݍ���

                    });
                }
            })
            .AddTo(this); // �Q�[���J�n���Ƀ|�C���g�����Z�b�g
    }

}
