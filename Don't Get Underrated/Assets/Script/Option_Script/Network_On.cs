using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class Network_On : MonoBehaviour
{
    [SerializeField]
    GameObject Network_Off;

    [SerializeField]
    TextMeshProUGUI ErrorMessage;

    [SerializeField]
    GameObject Game_End;

    [SerializeField]
    GameObject Yes;

    SpriteColor spriteColor;

    IEnumerator is_network;
    private void Awake()
    {
        Network_Off.SetActive(false);
        is_network = Network_Check_Infinite();

        if (GameObject.Find("Network_Sprite") && GameObject.Find("Network_Sprite").TryGetComponent(out SpriteColor s1))
        {
            spriteColor = s1;
            spriteColor.Set_BG_Clear();
        }
    }
    private void Start()
    {
        if (is_network != null)
            StopCoroutine(is_network);
        is_network = Network_Check_Infinite();
        StartCoroutine(is_network);
    }

    [System.Serializable]
    public class Log_Message
    {
        public string user_info;
        public string message;
    }

    protected IEnumerator Network_Check_Infinite()
    {
        Player_Info player_info = null;
        Boss_Info boss_info = null;
        if (GameObject.Find("Player") && GameObject.Find("Player").TryGetComponent(out Player_Info PS_3))
            player_info = PS_3;
        if (GameObject.Find("Boss") && GameObject.Find("Boss").TryGetComponent(out Boss_Info BI))
            boss_info  = BI;

        while (true)
        {
            singleTone.request = UnityWebRequest.Get("http://localhost:3000/continue_connect");

            yield return singleTone.request.SendWebRequest();

            Game_End.SetActive(true);

            Yes.SetActive(false);

            if ((singleTone.request.result == UnityWebRequest.Result.ConnectionError) ||
                (singleTone.request.result == UnityWebRequest.Result.ProtocolError) ||
                (singleTone.request.result == UnityWebRequest.Result.DataProcessingError))
            {
                if (player_info != null)
                    player_info.Stop_When_Network_Stop();
                if (boss_info != null)
                    boss_info.Stop_When_Network_Stop();

                Time.timeScale = 0;
                Network_Off.SetActive(true);
                ErrorMessage.text = singleTone.request.error + '\n' + singleTone.request.downloadHandler.text + '\n';
                ErrorMessage.text += "서버가 끊겼거나 로그인 상태가 아닙니다.\n 게임을 종료합니다.";
                yield break;
            }
            else
            {
                Log_Message d = JsonUtility.FromJson<Log_Message>(singleTone.request.downloadHandler.text);
                if (singleTone.id != d.user_info)
                {
                    if (player_info != null)
                        player_info.Stop_When_Network_Stop();
                    if (boss_info != null)
                        boss_info.Stop_When_Network_Stop();

                    Time.timeScale = 0;
                    Network_Off.SetActive(true);
                    ErrorMessage.text = "계정 정보가 틀리거나 서버가 끊겼습니다.\n 게임을 종료합니다.";
                    yield break;
                }
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }
    public void Enter_End()
    {
        Game_End.SetActive(false);
        Yes.SetActive(true);
        ErrorMessage.text = "게임을 종료합니다.";
    }

    public void Real_End()
    {
        StartCoroutine(Fade_Out());
    }

    IEnumerator Fade_Out()
    {
        if (spriteColor != null)
        {
            yield return spriteColor.StartCoroutine(spriteColor.Change_Color_Real_Time(Color.black, 2));
            Time.timeScale = 1;
            SceneManager.LoadScene("LoginScene");
        }
        else
            yield return null;
    }
}
