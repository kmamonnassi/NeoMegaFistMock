using UnityEngine;

public class Kogata_Enkyori : Character
{
    [SerializeField] private float shotInterval = 1;
    [SerializeField] private float bulletSpeed = 1;
    [SerializeField] private Bullet bulletPrefab;

    public override CharacterType CharacterType => CharacterType.Enemy;
    public override StageObjectID ID => StageObjectID.Kogata_Enkyori;
    public override Size Size => Size.Small;

    private float nowInterval = 0;

    protected override void Update()
    {
        base.Update();
        nowInterval += Time.deltaTime;
        if(nowInterval >= shotInterval)
        {
            nowInterval = 0;
            if (IsCatched || IsThrowned || IsStun || Player.Instance == null) return;
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Initalize((Player.Instance.transform.position - transform.position).normalized, bulletSpeed);
        }
    }
}
