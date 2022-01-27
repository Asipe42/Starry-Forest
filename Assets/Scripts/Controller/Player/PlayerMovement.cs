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
    [SerializeField] float _downhillPower = 1.5f;
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
    [SerializeField] float _flyPower = 1f;
    [SerializeField] float _flyMaxTime = 3.5f;
    [SerializeField] float _flyUpValue = 0.6f;
    [SerializeField] float _flyDownValue = 1f;
    [SerializeField] float _decreaseGravitySpeed = 0.05f;
    [SerializeField] SpriteRenderer _dandelionBuds;
    bool _up;
    public bool _onFly;
    

    bool _onGround;
    static bool _onStop;

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

    public static void StopPlayerMovement()
    {
        _onStop = true;
    }

    public static void ContinuePlayerMovement()
    {
        _onStop = false;
    }

    [System.Obsolete]
    void Update()
    {
        if (_onStop)
            return;

        if (GameManager.instance.StageManagerInstance.end)
        {
            Movement_Rest();
        }
        else
        {
            Movement_Walk();
            Movement_Fly();
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

    void Movement_Jump()
    {
        Movement_Downhill();

        if (!_onGround)
            return;

        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D[] hits = DetectGround();

            if (hits[0].collider != null && hits[1].collider == null)
            {
                _onGround = false;
                _onJump = true;

                playerAnim.PlayAnimationClip(Definition.ANIM_JUMP, true);

                Invoke("OnDownhill", _downhillWaitingTime);

                rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

                audioManager.PlaySFX(Definition.JUMP_CLIP);
            }
        }

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

        if (_onFly)
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

                audioManager.PlaySFX(Definition.DASH_UPGRADE_CLIP);
                playerVFX.ChangeDashEffectColor(_dashColors[(int)_dashLevel - 1]);
            }

            yield return null;
        }
    }

    void Movement_Fly()
    {
        if (_onGround)
            return;

        if (!_onFly)
        {
            _flyCurrentTime = 0;
            return;
        }

        playerAnim.PlayAnimationClip(Definition.ANIM_FLY, true);
        playerVFX.PlayVFX(Definition.VFX_DANDELION);

        _flyCurrentTime += Time.deltaTime;

        _dandelionBuds.color = new Color(1, 1, 1, 1.2f - _flyCurrentTime / _flyMaxTime);

        if (_flyCurrentTime >= _flyMaxTime)
        {
            _onFly = false;
            OnDownhill();

            playerAnim.PlayAnimationClip(Definition.ANIM_FLY, false);
            playerVFX.StopVFX(Definition.VFX_DANDELION);

            StartCoroutine(DecreaseGravityValue());

            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _up = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            _up = false;
        }

        if (_up)
        {
            if (rigid.gravityScale != _flyUpValue)
                rigid.gravityScale = _flyUpValue;

            rigid.AddForce(Vector2.up * _flyPower, ForceMode2D.Force);
        }
        else
        {
            rigid.gravityScale = _flyDownValue;
        }
    }

    IEnumerator DecreaseGravityValue()
    {
        while (rigid.gravityScale <= _defaultGravityValue)
        {
            rigid.gravityScale += Time.deltaTime;
            yield return new WaitForSeconds(_decreaseGravitySpeed);               
        }
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

            rigid.gravityScale = _defaultGravityValue;
        }
    }

    void SetGrounding(PlayerState state)
    {
        SetBoolVariables();

        playerAnim.PlayAnimationClip(Definition.ANIM_STANDING, false);
        playerAnim.PlayAnimationClip(Definition.ANIM_JUMP, false);
        playerAnim.PlayAnimationClip(Definition.ANIM_DOWNHILL, false);
        playerAnim.PlayAnimationClip(Definition.ANIM_FLY, false);

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
}
