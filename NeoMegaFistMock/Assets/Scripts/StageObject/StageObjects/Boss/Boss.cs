using UnityEngine;

public class Boss : Character
{
    public override CharacterType CharacterType => CharacterType.Enemy;
    public override StageObjectID ID => StageObjectID.Boss;
    public override Size Size => Size.UltraBig;
}
