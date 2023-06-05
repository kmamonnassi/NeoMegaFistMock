using UnityEngine;

public class Player : Character
{
    public override CharacterType CharacterType => CharacterType.Player;
    public override Size Size => Size.Midium;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float punchDuration = 0.3f;
    [SerializeField] private float punchPower = 1f;

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 toDirection = new Vector3(mousePos.x, mousePos.y) - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, toDirection);

        Vector2 dir = Vector2.zero;
        if(Input.GetKey(KeyCode.W))
        {
            dir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += Vector2.right;
        }
        transform.position += (Vector3)dir.normalized * moveSpeed * Time.deltaTime;
    }
}
