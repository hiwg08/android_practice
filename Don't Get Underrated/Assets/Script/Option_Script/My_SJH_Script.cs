using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class My_SJH_Script : MonoBehaviour
{
    private static string nextScene;
    public float currentValue;
    public float speed = 25;

    [SerializeField]
    Text Wait_text;

    [SerializeField]
    Text Percent;

    [SerializeField]
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LoadScene_Real());
        StartCoroutine(LoadScene_Fake());
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    IEnumerator LoadScene_Real()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        currentValue = 0;
        yield return null;
        StartCoroutine("Wait_Text_IEnum");

        while (!op.isDone)
        {
            yield return null;
            if (currentValue < 100)
            {
                currentValue += speed * Time.deltaTime;
                Percent.text = ((int)currentValue).ToString() + "%";
            }
            else
            {
                Wait_text.text = "Done";
                StopCoroutine("Wait_Text_IEnum");
                yield return new WaitForSeconds(1f);
                op.allowSceneActivation = true;
                yield break;
            }
            slider.value = currentValue * 0.01f;
        }
    }
    IEnumerator LoadScene_Fake()
    {
        currentValue = 0;
        yield return null;
        IEnumerator wait_text_ienum = Wait_Text_IEnum();
        StartCoroutine(wait_text_ienum);

        while (true)
        {
           
            if (currentValue < 100)
            {
                currentValue += speed * Time.deltaTime;
                Percent.text = ((int)currentValue).ToString() + "%";
            }
            else
            {
                Wait_text.text = "Done";
                StopCoroutine(wait_text_ienum);
                yield return YieldInstructionCache.WaitForSeconds(1f);
                yield break;
            }
            slider.value = currentValue * 0.01f;
            yield return null;
        }
    }
    IEnumerator Wait_Text_IEnum()
    {
        while (true)
        {
            Wait_text.text = "잠시만 기다려주세요..";
            yield return YieldInstructionCache.WaitForSeconds(0.2f);

            Wait_text.text = "잠시만 기다려주세요....";
            yield return YieldInstructionCache.WaitForSeconds(0.2f);

            Wait_text.text = "잠시만 기다려주세요......";
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }

    }

    // Update is called once per frame

    // Update is called once per frame
    //public static string nextScene;

    //[SerializeField]
    //Image progressBar;

    //private void Start()
    //{
    //    StartCoroutine(LoadScene());
    //}

    //public static void LoadScene(string sceneName)
    //{
    //    nextScene = sceneName;
    //    SceneManager.LoadScene("LoadingScene");
    //}
    //IEnumerator LoadScene()
    //{
    //    yield return null;
    //    AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
    //    op.allowSceneActivation = false;
    //    float timer = 0.0f;
    //    while (!op.isDone)
    //    {
    //        yield return null;
    //        timer += Time.deltaTime;
    //        if (op.progress < 0.9f)
    //        {
    //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
    //            if (progressBar.fillAmount >= op.progress)
    //            {
    //                timer = 0f;
    //            }
    //        }
    //        else
    //        {
    //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
    //            if (progressBar.fillAmount == 1.0f)
    //            {
    //                op.allowSceneActivation = true;
    //                yield break;
    //            }
    //        }
    //    }

    //}
}
