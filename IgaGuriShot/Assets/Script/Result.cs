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
        re.SetActive(false); // 初期状態では非表示
        PointManager.Instance.Game
            .Subscribe(_ =>
            {
                if (_ == 0)
                {
                    re.SetActive(false); // ゲーム終了時に結果画面を表示
                }
                if (_ == 1)
                {
                    re.SetActive(true); // ゲーム終了時に結果画面を表示
                    reButton.onClick.AddListener(() =>
                    {
                        PointManager.Instance.Game.Value = 0; // ゲームをリセット
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ゲームシーンを再読み込み

                    });
                }
            })
            .AddTo(this); // ゲーム開始時にポイントをリセット
    }

}
