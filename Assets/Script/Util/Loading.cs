using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    static string nextScene;

    Slider loadingBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    void Awake()
    {
        loadingBar = GetComponent<Slider>();
        loadingBar.maxValue = 1;
    }

    void Start()
    {
        StartCoroutine(LoadingScene());
    }

    IEnumerator LoadingScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        const float LOAD_VALUE = 0.2f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < LOAD_VALUE)
            {
                loadingBar.value = op.progress;
            }
            else
            {
                timer += Time.deltaTime;
                loadingBar.value = Mathf.Lerp(LOAD_VALUE, 1.0f, timer);

                if (loadingBar.value >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
