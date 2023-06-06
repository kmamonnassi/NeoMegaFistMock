public class Wall : StageObject
{
    public override StageObjectID ID => StageObjectID.Wall;
    public override StageObjectType StageObjectType => StageObjectType.Wall;
    public override Size Size => Size.UltraBig;
}