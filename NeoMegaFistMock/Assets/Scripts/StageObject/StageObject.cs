using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class StageObject : MonoBehaviour
{
    [SerializeField] private float thrownSpeedMultiply = 1;
    [SerializeField] private float thrownDuration = 5;
    [SerializeField] private int thrownPenetration = 1;
    [SerializeField] private bool isHitThrownObject = true;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private AttackCollider thrownCollider;
    [SerializeField] private AttackCollider attackCollider;

    public Rigidbody2D Rb => rb;

    public abstract StageObjectID ID { get; }
    public abstract StageObjectType StageObjectType { get; }
    public abstract Size Size { get; }

    public Action<StageObject> OnHitStageObjectEventListener;
    public Action OnCatched;
    public Action OnReleased;
    public Action OnThrown;
    public Action OnEndThrown;
    public Action OnKill;

    public int ThrownPenetration => thrownPenetration;
    public bool IsHitThrownObject => isHitThrownObject;
    public bool IsCatched { get; private set; } = false;
    public bool IsThrowned { get; private set; } = false;
    public Vector2 NowThrownedDirection { get; private set; }
    public float NowThrownedSpeed { get; private set; }
    public float NowThrownedPenetration { get; private set; }

    private Tween thrownTween;

    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }

    public virtual bool CanCatch()
    {
        if (Size == Size.Small) return true;
        if (Size == Size.Midium && StageObjectType == StageObjectType.Object) return true;

        return false;
    }

    public void Catch()
    {
        IsCatched = true;
        rb.velocity = Vector2.zero;
        OnCatched?.Invoke();
        col.isTrigger = true;
    }

    public void EndCatch()
    {
        IsCatched = false;
        rb.velocity = Vector2.zero;
        OnReleased?.Invoke();
        col.isTrigger = false;
    }

    public void Thrown(Vector3 dir, float speed)
    {
        EndCatch();
        IsThrowned = true;
        NowThrownedDirection = dir;
        NowThrownedSpeed = speed;
        NowThrownedPenetration = thrownPenetration;

        col.isTrigger = true;
        if (thrownCollider != null) thrownCollider.enabled = true;
        if (attackCollider != null) attackCollider.enabled = false;

        thrownTween = DOVirtual.DelayedCall(thrownDuration, () => { });
        rb.AddForce(NowThrownedDirection * NowThrownedSpeed * thrownSpeedMultiply);
        thrownTween.onComplete += EndThrown;
        OnThrown?.Invoke();
    }

    public void EndThrown()
    {
        IsThrowned = false;
        rb.velocity = Vector2.zero;
        thrownTween.Kill();
        thrownTween = null;

        col.isTrigger = false;
        if (thrownCollider != null) thrownCollider.enabled = false;
        if (attackCollider != null) attackCollider.enabled = true;
        OnEndThrown?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        StageObject obj = col.gameObject.GetComponent<StageObject>();
        if (obj != null)
        {
            OnHitStageObject(obj);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        StageObject obj = col.gameObject.GetComponent<StageObject>();
        if (obj != null)
        {
            OnHitStageObject(obj);
        }
    }

    private void OnHitStageObject(StageObject obj)
    {
        OnHitStageObjectEventListener?.Invoke(obj);
        OnHitStageObject_Virtual(obj);

        if (IsThrowned && obj.isHitThrownObject)
        {
            NowThrownedPenetration--;
            if (NowThrownedPenetration <= 0)
            {
                EndThrown();
            }
        }
    }

    protected virtual void OnHitStageObject_Virtual(StageObject obj)
    {
    }

    public void Kill()
    {
        Destroy(gameObject);
        OnKill?.Invoke();

    }

    public void KnockBack(Vector3 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
	}
}