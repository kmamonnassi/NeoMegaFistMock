using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArm : MonoBehaviour
{
    [SerializeField] private GameObject master;
    [SerializeField] private float armAngle;
    [SerializeField] private float armSize = 1.1f;
    [SerializeField] private float armOffset;
    [SerializeField] private float throwSpeed = 1;

    public bool IsShotArm { get; private set; }
    public StageObject CatchingObject { get; private set; }

    public Action<Vector2> OnStartShot;
    public Action<Vector2> OnEndShot;
    public Action<List<StageObject>> OnHitHand;

    private void Update()
    {
        if (CatchingObject != null)
        {
            CatchingObject.transform.position = transform.position;
        }
    }

    //public void Shot(Vector2 dir, float power, float duration)
    //{
    //if (IsShotArm) return;
    //Vector2 targetPos = (Vector2)transform.position + dir * power;

    //IsShotArm = true;
    //OnStartShot?.Invoke(targetPos);
    //bool isHitHand = false;
    //Sequence seq = DOTween.Sequence();
    //seq.Append(DOVirtual.Float(0, 1, duration, x =>
    //{
    //    Vector2 pos = Vector2.Lerp(CalcPosition(), targetPos, x);
    //    transform.position = pos;

    //    if(!isHitHand)
    //    if(CheckHit() != null)
    //    {
    //        isHitHand = true;
    //    }
    //}));
    //seq.Append(DOVirtual.Float(0, 1, duration, x =>
    //{
    //    Vector2 pos = Vector2.Lerp(targetPos, CalcPosition(), x);
    //    transform.position = pos;
    //}));
    //seq.onComplete += () =>
    //{
    //    IsShotArm = false;
    //    OnStartShot?.Invoke(targetPos);
    //};
    //seq.Play();
    //}

    public void Catch(StageObject obj)
    {
        if (obj.IsCatched || CatchingObject != null) return;

        obj.Catch();
        CatchingObject = obj;
    }

    public void Throw()
    {
        CatchingObject.Thrown(((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)master.transform.position).normalized, throwSpeed);
        CatchingObject = null;
    }

    //private List<StageObject> CheckHit()
    //{
    //    int count = 7;
    //    float angleDiff = 360f / (float)count;
    //    List<StageObject> objs = new List<StageObject>();
    //    for (int i = 0; i < count; i++)
    //    {
    //        Vector3 pos = transform.position;

    //        float angle = (90 - angleDiff * i) * Mathf.Deg2Rad;
    //        pos.x += Mathf.Cos(angle) * armSize;
    //        pos.y += Mathf.Sin(angle) * armSize;

    //        //Rayの長さは手の大きさ÷2
    //        float maxDistance = transform.localScale.x / 2;

    //        RaycastHit2D hit = Physics2D.Raycast(pos, (transform.position - pos).normalized, maxDistance, LayerMask.GetMask("StageObject"));
    //        //StageObjectと衝突した時だけその名前をログに出す
    //        StageObject obj = hit.collider?.GetComponent<StageObject>();
    //        if (obj != null)
    //        {
    //            if(obj is not Player)
    //            {
    //                objs.Add(obj);
    //            }
    //        }
    //    }
    //    if(objs.Count > 0)
    //    {
    //        OnHitHand?.Invoke(objs);
    //        return objs;
    //    }
    //    return null;
    //}

    //private Vector2 CalcPosition()
    //{
    //    Vector2 pos = master.transform.position;

    //    float angle = (armAngle + 90 + master.transform.eulerAngles.z) * Mathf.Deg2Rad;
    //    pos.x += armOffset * Mathf.Cos(angle);
    //    pos.y += armOffset * Mathf.Sin(angle);

    //    return pos;
    //}

    //public void SetPosition()
    //{
    //    transform.position = CalcPosition();
    //}
}
