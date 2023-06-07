using UnityEngine;

public class Bullet : StageObject
{
    public override StageObjectID ID => StageObjectID.Bullet;
    public override StageObjectType StageObjectType => StageObjectType.Other;
    public override Size Size => Size.UltraSmall;

    public float Speed { get; private set; }
    public Vector2 Direction { get; private set; }

    protected override void Start()
    {
        OnEndThrown += Kill;
    }

    public void Initalize(Vector2 direction, float speed)
    {
        Speed = speed;
        Direction = direction;
        Thrown(direction, speed);
    }
}
