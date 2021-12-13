using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    enum PlayerState
    {
        Rest,
        Default
    }

    [Header("Jumping")]
    [SerializeField] float _jumpPower = 7.5f;
    bool _onJump;

    [Header("Downhill")]
    [SerializeField] float _downhillPower = 1.25f;
    [SerializeField] float _defaultGravityValue = 1.5f;
    [SerializeField] float _downhillGravityValue = 0.15f;
    [SerializeField] float _downhillWaitingTime = 0.5f;
    IEnumerator _downhillCorutine;
    bool _validDownhill;
    bool _onDownhill;

    [Header("Sliding")]
    [SerializeField] float _slidingCooltime = 0.25f;
    [SerializeField] BoxCollider2D _slidingCollider;
    IEnumerator _slidingCorutine;
    bool _onSliding;
    bool _slidingOnce;

    [Header("Dash")]
    float _dashCurrentTime;
    [SerializeField] float[] _dashChangingTime;
    [SerializeField] Color[] _dashColors;
    AccelerationSpace.AccelerationLevel _dashLevel;
    IEnumerator _dashCorutine;
    bool _onDash;

    [Header("Flying")]
    float _flyCurrentTime;
    [SerializeField] float _flyMaxTime = 3f;
    [SerializeField] float _flyUpValue = 0.75f;
    [SerializeField] float _flyDownValue = 1f;
    [SerializeField] SpriteRenderer _dandelionBuds;
    bool _validFly;
    bool _onFly;

    bool _onGround;

    Rigidbody2D rigid;
    PlayerAnimation playerAnim;
    PlayerVFX playerVFX;
    AudioManager audioManager;
    [SerializeField] CapsuleCollider2D _defaultCollider;

    void Awake()
    {
        AllocateComponent();
    }

    void Start()
    {
        audioManager = GameManager.instance.AudioManagerInstance;
    }

    [System.Obsolete]
    void Update()
    {
        if (GameManager.instance.StageManagerInstance.end)
        {
            Movement_Rest();
        }
        else
        {
            Movement_Walk();

            Movement_Jump();

            Movement_Sliding();

            Movement_Dash();
        }
    }

    void AllocateComponent()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnimation>();
        playerVFX = GetComponent<PlayerVFX>();
    }

    void Movement_Rest()
    {
        SetGrounding(PlayerState.Rest);
    }

    void Movement_Walk()
    {
        if (_onGround)
        {
            SetGrounding(PlayerState.Default);
        }
        else
        {
            audioManager.PauseWalkCahnnel();
        }
    }

    RaycastHit2D[] DetectGround()
    {
        RaycastHit2D[] hits = new RaycastHit2D[2];

        Debug.DrawRay(transform.position, Vector2.down * 2.0f, Color.green);
        hits[0] = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, LayerMask.GetMask("Platform"));

        Debug.DrawRay(transform.position, Vector2.right * 1.0f, Color.green);
        hits[1] = Physics2D.Raycast(transform.position, Vector2.right, 1.0f, LayerMask.GetMask("Platform"));

        return hits;
    }

    void AirStateAnimation()
    {
        if (_onGround)
            return;

        if ( _onJump)
        {
            if (_onDownhill)
            {
                playerAnim.PlayAnimationClip(Definition.ANIM_DOWNHILL, true);
            }
            else if (_onFly)
            {
                playerAnim.PlayAnimationClip(Definition.ANIM_FLY, true);
            }
            else
            {
                playerAnim.PlayAnimationClip(Definition.ANIM_JUMP, true);
            }
        }
    }

    void Movement_Jump()
    {
        Movement_Downhill();

        Movement_Fly();

        if (!_onGround)
            return;

        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D[] hits = DetectGround();

            if (hits[0].collider != null && hits[1].collider == null)
            {
                _onGround = false;
                _onJump = true;

                Invoke("OnDownhill", _downhillWaitingTime);

                rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

                audioManager.PlaySFX(Definition.JUMP_CLIP);
            }
        }

        AirStateAnimation();
    }

    void OnDownhill()
    {
        _validDownhill = true;
    }

    void ResetVelocity_Y()
    {
        Vector2 downhillVec = new Vector2(rigid.velocity.x, 0.0f);

        rigid.velocity = downhillVec;
    }

    void Movement_Downhill()
    {        
        if (!_validDownhill)
            return;

        if (Input.GetButtonDown("Jump"))
        {
            _onDownhill = true;
            _validDownhill = false;

            ResetVelocity_Y();

            rigid.AddForce(Vector2.up * _downhillPower, ForceMode2D.Impulse);

            rigid.gravityScale = _downhillGravityValue;

            playerAnim.PlayAnimationClip(Definition.ANIM_DOWNHILL, true);

            audioManager.PlaySFX(Definition.DOWNHILL_CLIP);

            _downhillCorutine = CancleDownhill();

            StartCoroutine(_downhillCorutine);
        }
    }

    IEnumerator CancleDownhill()
    {
        while (_onDownhill)
        {
            if (Input.GetButtonUp("Jump"))
            {
                rigid.gravityScale = _defaultGravityValue;

                playerAnim.PlayAnimationClip(Definition.ANIM_DOWNHILL, false);
            }

            yield return null;
        }
    }

    void Movement_Sliding()
    {
        if (!_onGround)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            _onSliding = true;

            if (!_slidingOnce)
            {
                _slidingOnce = true;

                audioManager.PlaySFX(Definition.SLIDING_CLIP);

                playerAnim.PlayAnimationClip(Definition.ANIM_SLIDE, true);

                _defaultCollider.enabled = false;
                _slidingCollider.enabled = true;

                playerVFX.PlayVFX(Definition.VFX_DUST);

                _slidingCorutine = CancleSliding();
                StartCoroutine(_slidingCorutine);
            }
        }
    }

    IEnumerator CancleSliding()
    {
        while (_onSliding)
        {
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                _defaultCollider.enabled = true;
                _slidingCollider.enabled = false;

                playerAnim.PlayAnimationClip(Definition.ANIM_SLIDE, false);

                playerVFX.StopVFX(Definition.VFX_DUST);

                _onSliding = false;
                _slidingOnce = false;

                if (_slidingCorutine != null)
                    StopCoroutine(_slidingCorutine);
            }

            yield return null;
        }

        yield return null;
    }

    [System.Obsolete]
    void Movement_Dash()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _onDash = true;

            playerVFX.PlayVFX(Definition.VFX_DASH);
            audioManager.PlaySFX(Definition.DASH_CLIP);
            playerAnim.PlayAnimationClip(Definition.ANIM_DASH, true);

            _dashCorutine = ChangeDashGrade();

            StartCoroutine(_dashCorutine);
        }
    }

    [System.Obsolete]
    IEnumerator ChangeDashGrade()
    {
        _dashLevel = AccelerationSpace.AccelerationLevel.One;

        GameManager.instance.UIManagerInstance.runningBarInstance.IncreaseFillSpeed(_dashLevel);
        GameManager.instance.FloorManagerInstance.OnAcceleration(_dashLevel);
        playerVFX.ChangeDashEffectColor(_dashColors[(int)_dashLevel - 1]);
        playerVFX.PlayVFX(Definition.VFX_DASH);

        while (_onDash)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _onDash = false;

                _dashCurrentTime = 0f;

                _dashLevel = AccelerationSpace.AccelerationLevel.None;

                GameManager.instance.UIManagerInstance.runningBarInstance.IncreaseFillSpeed(_dashLevel);
                GameManager.instance.FloorManagerInstance.OnAcceleration(_dashLevel);

                playerVFX.StopVFX(Definition.VFX_DASH);
                playerAnim.PlayAnimationClip(Definition.ANIM_DASH, false);

                if (_dashCorutine != null)
                    StopCoroutine(_dashCorutine);

                yield return null;
            }

            _dashCurrentTime += Time.deltaTime;

            if (_onSliding || _onJump)
            {
                playerVFX.StopVFX(Definition.VFX_DASH);
            }
            else
            {
                if (!playerVFX.isPlaying(Definition.VFX_DASH))
                {
                    playerVFX.PlayVFX(Definition.VFX_DASH);
                }
            }

            if (_dashLevel < AccelerationSpace.AccelerationLevel.Max && _dashCurrentTime >= _dashChangingTime[(int)_dashLevel - 1])
            {
                _dashLevel++;

                GameManager.instance.UIManagerInstance.runningBarInstance.IncreaseFillSpeed(_dashLevel);
                GameManager.instance.FloorManagerInstance.OnAcceleration(_dashLevel);

                audioManager.PlaySFX(Definition.DASH_LEVEL_UP_CLIP);
                playerVFX.ChangeDashEffectColor(_dashColors[(int)_dashLevel - 1]);
            }

            yield return null;
        }
    }

    void Movement_Fly()
    {
        if (_onGround)
            return;

        //if (!_onFly)
        //{
        //    if (_flyCurrentTime != 0)
        //        _flyCurrentTime = 0;

        //    animator.SetBool("onFly", false);
        //    if (_dandelionEffect.isPlaying)
        //        _dandelionEffect.Stop();

        //    return;
        //}

        //animator.SetBool("onFly", true);
        //if (!_dandelionEffect.isPlaying)
        //    _dandelionEffect.Play();

        //_flyCurrentTime += Time.deltaTime;
        //_dandelionBuds.color = new Color(1, 1, 1, 1 - _flyCurrentTime / _flyTime);

        //if (_flyCurrentTime >= _flyTime)
        //{
        //    _onFly = false;
        //    rigidbody2D.gravityScale = FLY_DOWN_VALUE;
        //    return;
        //}

        //if (Input.GetButton("Jump"))
        //{
        //    rigidbody2D.gravityScale = FLY_UP_VALUE;
        //}
        //else
        //    rigidbody2D.gravityScale = FLY_DOWN_VALUE;
    }

    void SetDefaultGravityValue()
    {
        rigid.gravityScale = _defaultGravityValue;
    }

    void SetBoolVariables()
    {
        if (_onGround)
        {
            _onJump = false;
            _onDownhill = false;
            _onFly = false;

            _validDownhill = false;
            _validFly = false;
        }
    }

    void SetGrounding(PlayerState state)
    {
        SetBoolVariables();

        playerAnim.PlayAnimationClip(Definition.ANIM_STANDING, false);
        playerAnim.PlayAnimationClip(Definition.ANIM_JUMP, false);
        playerAnim.PlayAnimationClip(Definition.ANIM_DOWNHILL, false);

        if (_downhillCorutine != null)
            StopCoroutine(_downhillCorutine);

        if (state == PlayerState.Rest)
        {
            audioManager.PauseWalkCahnnel();
            playerAnim.PlayAnimationClip(Definition.ANIM_STANDING, true);
        }
        else if (state == PlayerState.Default)
        {
            audioManager.PlayWalkChannel();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            _onGround = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fly")
        {
            //_onFly = true;
            audioManager.PlaySFX(Definition.DANDELION_CLIP);
            Destroy(collision.gameObject);
        }
    }
}
