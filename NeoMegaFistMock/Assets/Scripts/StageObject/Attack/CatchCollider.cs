using System;
using UnityEngine;

public class CatchCollider : MonoBehaviour
{
    public event Action<StageObject> OnHit;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Check(col.gameObject);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        Check(col.gameObject);
    }

    private void Check(GameObject col)
    {
        if (!enabled) return;
        StageObject obj = col.gameObject.GetComponent<StageObject>();
        if (obj.CanCatch())
        {
            OnHit?.Invoke(obj);
        }
    }
}
