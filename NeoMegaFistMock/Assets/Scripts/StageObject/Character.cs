using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class Character : StageObject
{
    [SerializeField] private int maxHp = 8;
    [SerializeField] private int maxStunGauge = 3;
    [SerializeField] private int stunDuration = 4;
    
    public int MaxHP => maxHp;
    public int HP { get; private set; }
    public int MaxStunGauge => maxStunGauge;
    public int StunGauge { get; private set; }
    public bool IsStun { get; private set; } = false;
    public bool IsDead { get; private set; }

    public event Action<int> OnChangeHP;
    public event Action<int> OnChangeStunGauge;
    public event Action<int, int> OnDamage;
    public event Action<int, int> OnHeal;

    public override StageObjectType StageObjectType => StageObjectType.Character;
    public abstract CharacterType CharacterType { get; }

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
        StunGauge = maxStunGauge;
        OnChangeStunGauge?.Invoke(StunGauge);
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
        OnChangeHP?.Invoke(HP);
        StunGauge -= pt;
        OnChangeStunGauge?.Invoke(StunGauge);

        if (HP <= 0)
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
        OnChangeHP?.Invoke(HP);
        if (HP > maxHp)
        {
            HP = maxHp;
        }
    }

    public async void Dead()
    {
        IsDead = true;
        await UniTask.WaitUntil(() => !IsThrowned);
        Kill();
    }

    protected override void OnHitStageObject_Virtual(StageObject obj)
    {
        if (IsThrowned)
        {
            Damage(5);
        }
    }
}
