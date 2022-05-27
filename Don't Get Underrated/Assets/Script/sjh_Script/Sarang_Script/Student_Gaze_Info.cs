using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Student_Gaze_Info : Slider_Viewer
{
    // Start is called before the first frame update

    Camera selectedCamera;

    Heart_Gaze_Viewer heart_Gaze_Viewer;

    ArrayList Interrupt_NonActive; // 맵을 떠도는 인터럽트

    PlayerCtrl_Sarang playerCtrl_Sarang;

    Dictionary<GameObject, IEnumerator> Interrupt_Active;

    [SerializeField]
    GameObject YeonTa;

    GameObject YeonTa_Copy;

    int StudentLayerMask;  // Player 레이어만 충돌 체크함

    void Init_Start()
    {
        StudentLayerMask = 1 << LayerMask.NameToLayer("Student");
        selectedCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (GameObject.FindGameObjectWithTag("HeartSlider").TryGetComponent(out Heart_Gaze_Viewer user1))
            heart_Gaze_Viewer = user1;
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerCtrl_Sarang user2))
            playerCtrl_Sarang = user2;
        Interrupt_NonActive = new ArrayList();
        Interrupt_Active = new Dictionary<GameObject, IEnumerator>();
    }
    private new void Awake()
    {
        base.Awake();
        Init_Start();
    }
    public void When_Fever_End()
    {
        StopAllCoroutines();
        Init_Start();
        Change_Red_Slider();
    }
    public int Check_Student_HP(Vector3 TempPosition)
    {
        if (playerCtrl_Sarang.Is_Fever)
            slider.value += (Time.deltaTime / 2) * playerCtrl_Sarang.Student_Power * playerCtrl_Sarang.Fever_Power;
        else
            slider.value += (Time.deltaTime / 2) * playerCtrl_Sarang.Student_Power;

        if (slider.value >= 0.5f && Interrupt_NonActive.Count >= 1 && Interrupt_Active.Count == 0)
        {
            Interrupt_Active.Clear();
            foreach (var interrupt in Interrupt_NonActive)
            {
                GameObject copy_interrupt = (GameObject)interrupt;

                if (copy_interrupt.TryGetComponent(out Interrupt user))
                {
                    IEnumerator temp_s = user.Trigger_Lazor(TempPosition);
                    user.StartCoroutine(temp_s);
                    Interrupt_Active.Add(copy_interrupt, temp_s);
                }
            }
            return 0;
        }
        if (slider.value >= 1 && Interrupt_NonActive.Count == 0 && Interrupt_Active.Count == 0)
            return 1;
         if (slider.value >= 0.35f && Interrupt_Active.Count == 0)
            return 3;
        return 2;
    }

    public void Empty_HP()
    {
        slider.value = 0;
    }
    public void Change_Pink_Slider()
    {
        YeonTa_Copy = Instantiate(YeonTa, transform.position + 0.5f * Vector3.up, Quaternion.identity);
        if (GameObject.FindGameObjectWithTag("StudentSlider") && GameObject.FindGameObjectWithTag("StudentSlider").TryGetComponent(out Image user1))
            user1.color = new Color(1, 0.2f, 0.6f, 1);
        transform.localScale = new Vector3(2, 1.5f, 1);
        transform.position = transform.position + 0.7f * Vector3.down;
    }
    public void Change_Red_Slider()
    {
        if (YeonTa_Copy != null)
            Destroy(YeonTa_Copy);
        slider.value = 0;
        if (GameObject.FindGameObjectWithTag("StudentSlider") && GameObject.FindGameObjectWithTag("StudentSlider").TryGetComponent(out Image user1))
            user1.color = Color.red;
        transform.localScale = new Vector3(1, 1, 1);
        // 슬라이더를 분홍색으로 바꾸면서 변경된 위치를 여기서 재정의 할 필요가 없는 이유는 
        // 플레이어 쪽에서 학생의 게이지의 위치를 알아서 설정해주기 때문이다.
    }
    public IEnumerator Competition(GameObject student, float student_power)
    {
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        if (GameObject.Find("Flash_Interrupt") && GameObject.Find("Flash_Interrupt").TryGetComponent(out BackGroundColor user1))
            user1.StartCoroutine(user1.Flash(new Color(1, 1, 1, 1), 0.3f, 3));
        
        Change_Pink_Slider();
        
        while (true)
        {
            if (!playerCtrl_Sarang.Is_Fever)
            {
                if (4 - Interrupt_Active.Count > 0)
                    slider.value -= Time.deltaTime / (4 - Interrupt_Active.Count);
                else
                    slider.value -= Time.deltaTime; // 예외 처리
            }

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, StudentLayerMask);

            if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.transform.gameObject.CompareTag("Student"))
                slider.value += 0.15f * student_power;

            if (slider.value <= 0 || slider.value >= 1)
            {
                foreach (var e in Interrupt_Active)
                {
                   if (e.Key.TryGetComponent(out Interrupt user))
                        user.StopCoroutine(e.Value);
                }
                if (slider.value <= 0)
                {
                    foreach (var e in Interrupt_Active)
                    {
                        if (e.Key.TryGetComponent(out Interrupt user))
                            user.Start_Move();
                    }
                    heart_Gaze_Viewer.When_Player_Defeat();
                    playerCtrl_Sarang.animator.SetBool("IsDead", true);
                }
                else if (slider.value >= 1)
                {
                    foreach (var e in Interrupt_Active)
                        Destroy(e.Key);

                    heart_Gaze_Viewer.When_Interrupt_Defeat();
                    slider.value = 0;
                    if (!playerCtrl_Sarang.Is_Fever)
                    {
                        playerCtrl_Sarang.animator.SetBool("Heart_Gain", true);
                        playerCtrl_Sarang.This_Scale = new Vector3(1.1f, 1.1f, 0);
                    }
                }
                Change_Red_Slider();
                Interrupt_Active.Clear();
                Interrupt_NonActive.Clear();
                yield break;
            }
            yield return null;
        }
    }
    void CheckInterrupt()
    {
        GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
        if (e != null)
        {
            Interrupt_NonActive.Clear();
            foreach (var u in e)
            {
                Vector3 screenPoint = selectedCamera.WorldToViewportPoint(u.transform.position);
                bool OnScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (OnScreen)
                    Interrupt_NonActive.Add(u);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckInterrupt();
    } // 학생이 레이저 빔을 과제, 시험같은 방해 오브젝트 및 플레이어에게 맞았을 때 처리한 코드
    private void LateUpdate()
    {
        if (YeonTa_Copy != null)
            YeonTa_Copy.transform.position = new Vector3(transform.position.x - 1.78f + (slider.value * 3.56f), transform.position.y + 1f, 1);
    }
}