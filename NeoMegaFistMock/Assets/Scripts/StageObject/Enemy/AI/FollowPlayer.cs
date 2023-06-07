using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private StageObject target;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AttackCollider attackCol;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float followDistance = 0;

    private bool isStun;

	private void Start()
	{
		if(attackCol != null)
        {
            target.OnCatched += () => attackCol.enabled = false;
            target.OnReleased += () => attackCol.enabled = true;
		}

        if (target is Character) 
        {
            Character chara = target as Character;
            chara.OnStun += () => isStun = true;
            chara.OnStunReleased += () => isStun = false;
        }
    }

	private void FixedUpdate()
    {
        if (target.IsCatched || target.IsThrowned || isStun || Player.Instance == null) return;
        if (Vector2.Distance(Player.Instance.transform.position, transform.position) <= followDistance) return;
        Vector3 dir = (Player.Instance.transform.position - transform.position).normalized;
        rb.velocity = dir * moveSpeed * Time.deltaTime;
    }
}
