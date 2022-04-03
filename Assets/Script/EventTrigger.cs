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

    [Header("downhill")]
    [SerializeField] SpriteRenderer downhillGuide;

    Color panelColor;

    bool onEvent;

    void Awake()
    {
        panelColor = panel.color;
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
                yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
                playerController.PermitAction(targetActionName, false);
                break;
            case "sliding":
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
                playerController.PermitAction(targetActionName, false);
                break;
            case "downhill":
                #region Jump
                playerController.PermitAction("jump");
                yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
                GameObject.FindObjectOfType<Scroll>().GetComponent<Scroll>().canScroll = true;
                #endregion

                #region Jump-Downhill
                yield return new WaitForSeconds(0.5f);
                playerController.PermitAction(targetActionName);
                GameObject.FindObjectOfType<Scroll>().GetComponent<Scroll>().canScroll = false;

                rigid.velocity = Vector2.zero;
                rigid.gravityScale = 0f;

                guide.DOColor(Color.clear, duration);
                downhillGuide.DOColor(Color.white, duration);

                yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
                rigid.gravityScale = 0.2f;
                #endregion

                playerController.PermitAction("jump");
                playerController.PermitAction("sliding");
                playerController.PermitAction("donwhill");
                break;
        }

        playerAnimation.SetAnimationClipSpeed(1f);

        panel.DOColor(new Color(panelColor.r, panelColor.g, panelColor.b, 0), duration);
        guide.DOColor(Color.clear, duration);

        if (downhillGuide != null)
            downhillGuide.DOColor(Color.clear, duration);

        GameObject.FindObjectOfType<Scroll>().GetComponent<Scroll>().canScroll = true;

        if (targetActionName == "downhill")
        {
            yield return new WaitForSeconds(3f);
            UIManager.instance.ActivateUI(UI.HUD);
            UIManager.instance.ActivateUI(UI.Popup);
        }
    }
}
