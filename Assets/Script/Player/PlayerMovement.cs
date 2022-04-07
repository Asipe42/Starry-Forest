using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerAnimation thePlayerAnimation;
    [SerializeField] float jumpPower;
    [SerializeField] float defaultGravityScale;
    [SerializeField] float downhillGravityScale;
    
    Rigidbody2D rigid;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        defaultGravityScale = rigid.gravityScale;
    }

    public void Movement_Jump()
    {
        rigid.velocity += Vector2.up * jumpPower;
    }

    public void Movement_Downhill(bool state)
    {
        if (state)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 2f);
            rigid.gravityScale = downhillGravityScale;
        }
        else
        {
            rigid.gravityScale = defaultGravityScale;
        }
    }
}
