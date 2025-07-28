using UnityEngine;
using DG.Tweening;
using R3;
using Cysharp.Threading.Tasks;	//DOTween���g���Ƃ��͂���using������

public class MatoMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveLoop().Forget();
    }

    async UniTask MoveLoop()
    {
        var token = this.GetCancellationTokenOnDestroy();
        while (true)
        {
            // this��MonoBehaviour���p�����Ă���̂ŁAMonoBehaviour�̃��C�t�T�C�N���ɍ��킹�Ď����I��Dispose�����
            this.transform.DOMove(transform.position + new Vector3(20f, 0f, 0f), 1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuint);

            await UniTask.WaitForSeconds(2f, cancellationToken: token);

        }
    }
}
