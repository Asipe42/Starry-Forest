using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] string targetActionName;

    [Space] 
    [SerializeField] Image panel;
    [SerializeField] SpriteRenderer guide;
    [SerializeField] float alpha = 0.35f;
    [SerializeField] float duration = 0.3f;

    [Space]
    [SerializeField] AudioClip tutorialPopupClip;

    [Header("Downhill")]
    [SerializeField] GameObject downhillGuide;

    [Header("Dash")]
    [SerializeField] Animator dashAnim;

    Color panelColor;

    bool onEvent;

    void Awake()
    {
        panelColor = panel.color;

        tutorialPopupClip = Resources.Load<AudioClip>("Audio/SFX/SFX_TutorialPopup");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onEvent)
            return;

        if (collision.CompareTag("Player"))
        {
            onEvent = true;

            GameObject.FindObjectOfType<Scroll>().GetComponent<Scroll>().canScroll = false;

            GameObject player = collision.gameObject;

            PlayerController playerController = player.GetComponent<PlayerController>();
            PlayerAnimation playerAnimation = player.GetComponent<PlayerAnimation>();

            if (targetActionName != "downhill")
            {
                playerController.PermitAction(targetActionName);
            }

            playerAnimation.SetAnimationClipSpeed(0f);
            playerController.onWalk = false;

            panel.DOColor(new Color(panelColor.r, panelColor.g, panelColor.b, alpha), duration);
            guide.DOColor(Color.white, duration);

            StartCoroutine(WaitAction(targetActionName, playerController, playerAnimation, player.GetComponent<Rigidbody2D>())) ;
        }
    }

    IEnumerator WaitAction(string targetActionName, PlayerController playerController, PlayerAnimation playerAnimation, Rigidbody2D rigid)
    {
        switch (targetActionName)
        {
            case "jump":
                AudioManager.instance.PlaySFX(tutorialPopupClip, 0, 1.5f);
                yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
                playerController.PermitAction(targetActionName, false);
                break;
            case "sliding":
                AudioManager.instance.PlaySFX(tutorialPopupClip, 0, 1.5f);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
                playerController.PermitAction(targetActionName, false);
                break;
            case "downhill":
                #region Jump
                AudioManager.instance.PlaySFX(tutorialPopupClip, 0, 1.5f);
                playerController.PermitAction("jump");
                yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
                guide.DOColor(Color.clear, duration);
                GameObject.FindObjectOfType<Scroll>().GetComponent<Scroll>().canScroll = true;
                #endregion

                #region Jump-Downhill
                yield return new WaitForSeconds(0.5f);
                playerController.PermitAction(targetActionName);
                GameObject.FindObjectOfType<Scroll>().GetComponent<Scroll>().canScroll = false;

                rigid.velocity = Vector2.zero;
                rigid.gravityScale = 0f;

                downhillGuide.SetActive(true);

                yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
                rigid.gravityScale = 0.2f;

                playerController.PermitAction("jump", false);
                playerController.PermitAction(targetActionName, false);
                #endregion
                break;
            case "dash":
                AudioManager.instance.PlaySFX(tutorialPopupClip, 0, 1.5f);
                dashAnim.enabled = true;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
                playerController.PermitAction("jump");
                playerController.PermitAction("sliding");
                playerController.PermitAction("downhill");
                playerController.PermitAction(targetActionName);
                playerController.onTutorial = false;
                dashAnim.enabled = false;
                break;
        }

        playerAnimation.SetAnimationClipSpeed(1f);

        panel.DOColor(new Color(panelColor.r, panelColor.g, panelColor.b, 0), duration);
        guide.DOColor(Color.clear, duration);

        if (downhillGuide != null)
            downhillGuide.SetActive(false);

        GameObject.FindObjectOfType<Scroll>().GetComponent<Scroll>().canScroll = true;

        if (targetActionName == "dash")
        {
            yield return new WaitForSeconds(0.3f);
            UIManager.instance.ActivateUI(UI.HUD);
            UIManager.instance.ActivateUI(UI.Popup);
        }
    }
}
