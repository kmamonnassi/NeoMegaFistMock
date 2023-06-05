using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class Character : StageObject
{
    [SerializeField] private int maxHp = 8;
    [SerializeField] private int maxStunGauge = 3;
    [SerializeField] private int stunDuration = 4;

    public override StageObjectType StageObjectType => StageObjectType.Character;
    public abstract CharacterType CharacterType { get; }
    
    public int MaxHP => maxHp;
    public int HP { get; private set; }
    public int MaxStunGauge => maxStunGauge;
    public int StunGauge { get; private set; }
    public bool IsStun { get; private set; } = false;
    public bool IsDead { get; private set; }

    private void Start()
    {
        HP = maxHp;
        StunGauge = maxStunGauge;
    }

    public async void Stun()
    {
        if (IsStun) return;
        IsStun = true;
        await UniTask.Delay(TimeSpan.FromSeconds(stunDuration));
        IsStun = false;
    }

    public override bool CanCatch()
    {
        if (Size == Size.Small) return true;
        if (Size == Size.Midium && IsStun) return true;
        if (Size == Size.Big && IsStun) return true;

        return false;
    }

    public void Damage(int pt)
    {
        if (IsDead) return;

        HP -= pt;
        StunGauge -= pt;

        if(HP <= 0)
        {
            Dead();
            return;
        }
        if(StunGauge <= 0)
        {
            Stun();
        }
    }

    public void Heal(int pt)
    {
        if (IsDead) return;

        HP += pt;
        if(HP > maxHp)
        {
            HP = maxHp;
        }
    }

    public async void Dead()
    {
        IsDead = true;
        Debug.Log(IsThrowned);
        await UniTask.WaitUntil(() => !IsThrowned);
        await UniTask.WaitUntil(() => !IsThrowned);
        Debug.Log(IsThrowned);
        gameObject.SetActive(false);
    }

    protected override void OnHitStageObject(StageObject obj)
    {
        if (obj.IsThrowned && CharacterType == CharacterType.Enemy)
        {
            Damage(obj.ThrownDamage);
        }
        if (IsThrowned)
        {
            if(obj.StageObjectType == StageObjectType.Wall)
            {
                Damage(5);
            }
            else
            {
                Damage(1);
            }
        }
    }
}
