using UnityEngine;

public class PlayerArmController : MonoBehaviour
{
    [SerializeField] private PlayerArm arm;
    [SerializeField] private GameObject body;
    [SerializeField] private int mouseId;
    [SerializeField] private float punchDuration = 0.3f;
    [SerializeField] private float punchPower = 1f;

    private void Start()
    {
        arm.OnHitHand += objs =>
        {
            for (int i = 0; i < objs.Count; i++)
            {
                if(objs[i].CanCatch())
                {
                    arm.Catch(objs[i]);
                    return;
                }
            }
        };
    }

    private void Update()
    {
        if(arm.CatchingObject == null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (new Vector3(mousePos.x, mousePos.y) - body.transform.position).normalized;
            if (Input.GetMouseButtonDown(mouseId))
            {
                Vector2 punchDir = direction;
                arm.Shot(punchDir, punchPower, punchDuration / 2);
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(mouseId))
            {
                arm.Throw();
            }
        }

        if (!arm.IsShotArm)
        {
            arm.SetPosition();
        }
    }
}
