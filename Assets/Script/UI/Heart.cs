using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum FillState
{
    Up,
    Down
}

public class Heart : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] float fillCooltime;

    Status theStatus;

    const float FILL_VALUE = 2f;

    int hp;

    void Start()
    {
        theStatus = GameObject.FindObjectOfType<Status>().GetComponent<Status>();
        hp = theStatus.hp;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].fillAmount = 1;
        }
    }

    public void CheckHp(int preHp)
    {
        if (preHp < 0 || preHp > images.Length)
            return;

        if (hp == 0)
            return;

        FillState state;

        if (preHp < hp)
            state = FillState.Down;
        else
            state = FillState.Up;

        hp = preHp;

        StartCoroutine(FillHeart(preHp, fillCooltime, state));
    }

    IEnumerator FillHeart(int index, float cooltime, FillState state)
    {

        if (state == FillState.Down)
        {
            while (images[index].fillAmount > 0)
            {
                images[index].fillAmount -= Time.deltaTime * FILL_VALUE;
                yield return new WaitForSeconds(cooltime);
            }
        }
        else
        {
            while (images[index - 1].fillAmount < 1)
            {
                images[index - 1].fillAmount += Time.deltaTime * FILL_VALUE;
                yield return new WaitForSeconds(cooltime);
            }
        }
    }
}
