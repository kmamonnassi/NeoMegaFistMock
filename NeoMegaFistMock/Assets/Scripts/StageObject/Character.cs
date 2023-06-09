﻿using Cysharp.Threading.Tasks;
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
    public event Action OnStun;
    public event Action OnStunReleased;

    public override StageObjectType StageObjectType => StageObjectType.Character;
    public abstract CharacterType CharacterType { get; }

    protected override void Start()
    {
        HP = maxHp;
        StunGauge = maxStunGauge;
    }

    protected override void Update()
    {
    
    }

    public async void Stun()
    {
        if (IsStun) return;

        IsStun = true;
        OnStun?.Invoke();
        Rb.velocity = Vector2.zero;

        await UniTask.Delay(TimeSpan.FromSeconds(stunDuration));

        IsStun = false;
        StunGauge = maxStunGauge;
        OnChangeStunGauge?.Invoke(StunGauge);
        OnStunReleased?.Invoke();
    }

    public override bool CanCatch()
    {
        if (Size == Size.Small && IsStun) return true;
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

        if (this is Player) Debug.Log("DMG" + pt + "/HP" + HP + "/MAX_HP" + MaxHP);

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

    public void Dead()
    {
        IsDead = true;
        Kill();
    }

    protected override void OnHitStageObject_Virtual(StageObject obj)
    {
        if (IsThrowned && obj.IsHitThrownObject)
        {
            Damage(1);
        }
    }
}
