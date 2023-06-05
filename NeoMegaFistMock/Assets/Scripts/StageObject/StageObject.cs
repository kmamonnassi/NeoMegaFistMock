using DG.Tweening;
using System;
using UnityEngine;

public abstract class StageObject : MonoBehaviour
{
    [SerializeField] private float thrownSpeedMultiply = 1;
    [SerializeField] private float thrownDuration = 5;
    [SerializeField] private int thrownDamage = 5;
    [SerializeField] private bool thrownReflectWall = false;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private Collider2D thrownCollider;

    public abstract StageObjectType StageObjectType { get; }
    public abstract Size Size { get; }

    public Action<StageObject> OnHitStageObjectEventListener;

    public int ThrownDamage => thrownDamage;
    public bool ThrownReflectWall => thrownReflectWall;
    public bool IsCatched { get; private set; } = false;
    public bool IsThrowned { get; private set; } = false;
    public Vector2 NowThrownedDirection { get; private set; }
    public float NowThrownedSpeed { get; private set; }

    private Tween thrownTween;

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
    }

    public void EndCatch()
    {
        IsCatched = false;
        rb.velocity = Vector2.zero;
    }

    public void Thrown(Vector3 dir, float speed)
    {
        EndCatch();
        IsThrowned = true;
        NowThrownedDirection = dir;
        NowThrownedSpeed = speed;

        col.isTrigger = true;
        thrownCollider.gameObject.SetActive(true);

        thrownTween = DOVirtual.DelayedCall(thrownDuration, () => { });
        thrownTween.SetUpdate(UpdateType.Fixed).onUpdate += () =>
        {
            rb.velocity = NowThrownedDirection * NowThrownedSpeed * thrownSpeedMultiply * Time.deltaTime;
        };
        thrownTween.onComplete += () =>
        {
            IsThrowned = false;
            rb.velocity = Vector2.zero;
            thrownTween = null;

            col.isTrigger = false;
            thrownCollider.gameObject.SetActive(false);
        };
    }

    public void Reflect()
    {
        thrownTween.Kill();
        NowThrownedDirection = Quaternion.Euler(0, 0, -90) * NowThrownedDirection;
        Thrown(NowThrownedDirection, NowThrownedSpeed);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        StageObject obj = col.gameObject.GetComponent<StageObject>();
        if (obj != null)
        {
            OnHitStageObjectEventListener?.Invoke(obj);
            OnHitStageObject_Virtual(obj);
        }
    }

    protected virtual void OnHitStageObject_Virtual(StageObject obj)
    {
    }

    public void Kill()
    {
        Destroy(gameObject);
	}
}