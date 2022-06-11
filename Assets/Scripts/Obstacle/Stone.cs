using UnityEngine;

public class Stone : Obstacle
{
    [SerializeField] float fallGravityScale;
    [SerializeField] Transform startTriggerTransform;
    [SerializeField] Transform endTriggerTransform;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] ParticleSystem stoneDust;

    Rigidbody2D rigid;
    PolygonCollider2D stoneCollider;
    AudioClip crashClip;

    bool onPlayer;
    bool onApeear;

    void Awake()
    {
        Initialize();
        GetAudioClip();
    }

    void Update()
    {
        CheckTrigger();
        Fall();
    }

    #region Initial Setting
    void Initialize()
    {
        rigid = GetComponent<Rigidbody2D>();
        stoneCollider = GetComponent<PolygonCollider2D>();
    }

    void GetAudioClip()
    {
        base.hitClip = Resources.Load<AudioClip>("Audio/SFX/SFX_StoneHit");
        crashClip = Resources.Load<AudioClip>("Audio/SFX/SFX_Crash");
    }
    #endregion

    void CheckTrigger()
    {
        onPlayer = Physics2D.Linecast(startTriggerTransform.position, endTriggerTransform.position, whatIsPlayer);
    }

    void Fall()
    {
        if (!onPlayer)
            return;

        if (!onApeear)
        {
            onApeear = true;

            rigid.gravityScale = fallGravityScale;
            stoneCollider.isTrigger = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            SFXController.instance.PlaySFX(crashClip);
            SetObstacle();
            PlayStoneDust();
        }
    }

    void SetObstacle()
    {
        rigid.gravityScale = 0f;
        stoneCollider.isTrigger = true;
    }

    void PlayStoneDust()
    {
        stoneDust.Emit(16);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startTriggerTransform.position, endTriggerTransform.position);
    }
}
