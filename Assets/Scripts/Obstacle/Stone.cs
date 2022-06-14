using UnityEngine;

public class Stone : Obstacle
{
    [SerializeField] float fallGravityScale;
    [SerializeField] Transform startTriggerTransform;
    [SerializeField] Transform endTriggerTransform;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] ParticleSystem stoneDust;
    [SerializeField] int emitCount = 16;

    Rigidbody2D rigid;
    PolygonCollider2D stoneCollider;
    AudioClip crashClip;

    bool reachPlayer;
    bool onAppear;

    void Awake()
    {
        Initialize();
        GetAudioClip();
    }

    void Update()
    {
        base.CheckTrigger(out reachPlayer, startTriggerTransform, endTriggerTransform, whatIsPlayer);
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

    void Fall()
    {
        if (!reachPlayer)
            return;

        if (!onAppear)
        {
            onAppear = true;

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
        stoneDust.Emit(emitCount);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startTriggerTransform.position, endTriggerTransform.position);
    }
}
