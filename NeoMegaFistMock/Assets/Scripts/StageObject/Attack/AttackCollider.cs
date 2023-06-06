using System;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public CharacterType characterType;
    public int damage = 1;
    public int penetration = 1;
    public int knockBackPower = 1;

    public event Action<StageObject> OnHit;

    private int nowPenetration;

    private void OnDisable()
    {
        nowPenetration = penetration;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        CheckHit(col.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        CheckHit(col.gameObject);
    }

    private void CheckHit(GameObject col)
    {
        if (!enabled) return;
        StageObject obj = col.GetComponent<StageObject>();
        if (obj is Character)
        {
            Character target = obj as Character;
            if(target.CharacterType != characterType || characterType == CharacterType.Other)
            {
                target.Damage(damage);
                target.KnockBack(((Vector2)(transform.position - target.transform.position)).normalized * knockBackPower);

                nowPenetration--;
                if (nowPenetration == 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
