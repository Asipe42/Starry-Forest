using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpPower;
    [SerializeField] float defaultGravityScale;
    [SerializeField] float downhillGravityScale;
    [SerializeField] float flyGravityScale;
    
    Rigidbody2D rigid;
    
    void Awake()
    {
        Initialize();
    }

    #region Initial Setting
    void Initialize()
    {
        rigid = GetComponent<Rigidbody2D>();

        defaultGravityScale = rigid.gravityScale;
    }
    #endregion

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

    public void Movement_Fly(bool state)
    {
        if (state)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 3f);
            rigid.gravityScale = flyGravityScale;
        }
        else
        {
            rigid.gravityScale = defaultGravityScale;
        }
    }
}
