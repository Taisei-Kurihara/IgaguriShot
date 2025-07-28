using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;

public class Ball : MonoBehaviour
{
    float speed = 1000f;

    Rigidbody rbdata;
    private Rigidbody rb { get { if (rbdata == null) { rbdata = GetComponent<Rigidbody>(); } return rbdata; } set { rbdata = value; } }

    [SerializeField]
    ParticleSystem ps;

    float deleteTime = 5f;
    bool delete = true;
    void CancelDelete() { delete = false; }

    async UniTask SetAsyncDelete()
    {
        var token = this.GetCancellationTokenOnDestroy();

        float startTime = Time.time;
        await UniTask.WaitUntil(() => Time.time - startTime >= deleteTime || !delete,PlayerLoopTiming.FixedUpdate, cancellationToken: token);

        if (delete) { Destroy(gameObject); }
    }



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

    }

    private void Start()
    {
        GetComponent<Collider>().OnCollisionEnterAsObservable()
            .Subscribe(collision =>
            {
                if(rb.linearVelocity.magnitude > 10)
                {
                    ps.Play();
                }
            })
            .AddTo(this);
    }

    public void SetFireDir(Vector3 dir)
    {
        rb.AddForce(dir * speed);

        SetAsyncDelete().Forget();
    }

    public void StopAndSticky(Transform tr)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.SetParent(tr);

        ps.Play();

        CancelDelete();
    }
}

public interface DeleteController
{

}
