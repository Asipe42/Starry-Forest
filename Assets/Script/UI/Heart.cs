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
        if (hp == 0)
            return;

        FillState state;

        if (hp > preHp)
            state = FillState.Down;
        else
            state = FillState.Up;

        hp = preHp;

        StartCoroutine(FillHeart(preHp, fillCooltime, state));
    }

    IEnumerator FillHeart(int index, float cooltime, FillState state)
    {
        while (images[index].fillAmount > 0)
        {
            if (state == FillState.Up)
                images[index].fillAmount += Time.deltaTime * FILL_VALUE;
            else if (state == FillState.Down)
                images[index].fillAmount -= Time.deltaTime * FILL_VALUE;

            yield return new WaitForSeconds(cooltime);
        }

        yield return null;
    }
}
