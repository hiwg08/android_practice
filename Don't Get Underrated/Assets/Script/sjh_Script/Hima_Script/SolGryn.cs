using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn : Boss_Info
{
    // Start is called before the first frame update

    // Update is called once per frame
    
    [SerializeField]
    GameObject SolGryn_HP;

    HimaController himaController;

    float[,] move_random =
    {
        {4.81f, -0.38f },
        { -6.38f, -1.95f },
        { 7.36f, -2.8f },
        { 7.09f, 3.3f },
        { -5.02f, 2.76f }
    };
    private new void Awake()
    {
        base.Awake();
        CurrentHP = MaxHP;
        
        flashOn = GameObject.Find("Flash").GetComponent<FlashOn>();
        himaController = GameObject.FindGameObjectWithTag("Player").GetComponent<HimaController>();
        SolGryn_HP.SetActive(false);
        transform.position = new Vector3(0, -15.81f, 0);
        transform.localScale = new Vector3(6, 6, 0);
    }

    void Start()
    {
     
    }
    public void WelCome()
    {
        // StartCoroutine(I_WelCome());
        StartCoroutine(Boss_Pattern());
    }
    IEnumerator I_WelCome()
    {
        camera_shake = cameraShake.Shake_Act(.07f, .16f, 1, true);
        StartCoroutine(camera_shake); // 등장할 때 한번 흔들어 재껴줘야함

        yield return StartCoroutine(Position_Lerp(new Vector3(0, -15.81f, 0), new Vector3(0, -3, 0), 5f, OriginCurve));
        yield return YieldInstructionCache.WaitForSeconds(1.5f); // 웅장한 이동 다음 1.5초 정지

        StartCoroutine(flashOn.Change_Color(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1));
        yield return StartCoroutine(Position_Lerp(new Vector3(0, -3f, 0), new Vector3(0, 13, 0), 1f, declineCurve)); // 플래시 키는 동시에 빠른 위로 이동(?)

        StopCoroutine(camera_shake);
        yield return YieldInstructionCache.WaitForSeconds(1.5f); // 이동 후 카메라 정지 + 1.5초 정지

        StartCoroutine(flashOn.Change_Color(new Color(1, 1, 1, 1), new Color(0, 0, 0, 1), 1));
        yield return YieldInstructionCache.WaitForSeconds(1.5f);  // 검정색 플래시 후 1.5초 정지

        StartCoroutine(flashOn.Change_Color(new Color(0, 0, 0, 1), new Color(1, 1, 1, 0), 1));
        StartCoroutine(Boss_Pattern());
    }
    IEnumerator Boss_Pattern()
    {
        himaController.IsMove = true;
        transform.position = new Vector3(7, 4, 0);
        transform.localScale = new Vector3(1.4f, 1.4f, 0);
        yield return YieldInstructionCache.WaitForSeconds(.5f);

        SolGryn_HP.SetActive(true);
        SolGryn_HP.GetComponent<BossHPSliderViewer>().F_HPFull(gameObject.GetComponent<SolGryn>());
        StartCoroutine(HP_Decrease());

        camera_shake = cameraShake.Shake_Act(.035f, .2f, 1, true);
        StartCoroutine(camera_shake);

        for (int i = 0; i < 2; i++)
        {
            yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 0, 0), .5f, declineCurve));
            yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 4, 0), .5f, declineCurve));
        }

        yield return StartCoroutine(First_Move());
        
        //yield return StartCoroutine(Pattern_1());
        //yield return StartCoroutine(Pattern_2());
        yield return StartCoroutine(Pattern_3());
        //yield return StartCoroutine(Pattern_4());
    }
    IEnumerator HP_Decrease()
    {
        while(true)
        {
            CurrentHP -= 10;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator Pattern_3()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        cameraShake.Shake_Act(.9f, .6f, 1, false);

        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 3, 0), 1, inclineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, 11, 0), 1, declineCurve));

        Instantiate(Weapon[4], new Vector3(2.52f, -8.31f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1.4f);

        Instantiate(Weapon[4], new Vector3(0, -8.31f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1.4f);

        Instantiate(Weapon[4], new Vector3(-2.52f, -8.31f, 0), Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(1);

        GameObject W1 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        W1.GetComponent<DynaBlade>().Dyna_Start(false);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
        yield return null;
        GameObject W2 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        W2.GetComponent<DynaBlade>().Dyna_Start(true);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        cameraShake.mainCamera.transform.position = new Vector3(0, 0, -10);
        cameraShake.mainCamera.transform.rotation = Quaternion.identity;
        cameraShake.mainCamera.transform.localScale = new Vector3(1, 1, 1);
        yield return null;
        GameObject W3 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        GameObject W4 = Instantiate(Weapon[3], transform.position, Quaternion.identity);
        W3.GetComponent<DynaBlade>().Dyna_Start(false);
        W4.GetComponent<DynaBlade>().Dyna_Start(true);
        yield return YieldInstructionCache.WaitForSeconds(2);

    }
    IEnumerator Pattern_4()
    {
        IEnumerator rotate_bullet = Rotate_Bullet(7, 200, 0.02f, 4, Weapon[2]);

        while (true)
        {
            yield return Change_Color_Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 0), 0.33f, 1.5f, DisAppear_Effect_2);

            camera_shake = cameraShake.Shake_Act(.035f, .2f, 1, true);
            StartCoroutine(camera_shake);
            StartCoroutine(rotate_bullet);

            int x = Random.Range(0, 5);
            transform.position = new Vector3(move_random[x, 0], move_random[x, 1], 0);

            yield return StartCoroutine(Change_Color_Return_To_Origin(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.67f, false));
            StopCoroutine(camera_shake);
            cameraShake.Origin_Camera();
            StopCoroutine(rotate_bullet);
            
            yield return null;
        }
    }
    IEnumerator Pattern_1()
    {
        IEnumerator move_Second = Move_Round_Trip(7, -2, 7, 2);
        IEnumerator rotate = Rotate(150);
        StartCoroutine(move_Second);
        StartCoroutine(rotate);

        for (int i = 0; i < 7; i++)
        {
            StartCoroutine(cameraShake.Shake_Act(.07f, .2f, 0.1f, false));
            StartCoroutine(Boss_W1(72 + (i * 20), 25, 360));
            yield return new WaitForSeconds(0.6f);
        }

        StopCoroutine(move_Second);
        StopCoroutine(rotate);
        yield return StartCoroutine(Change_Color_Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.33f, 0, DisAppear_Effect_1));
        yield break;
    }
    IEnumerator Pattern_2() // 각도 수정 & 
    { 
        transform.position = new Vector3(-2, 2, 0);
        StartCoroutine(Change_Color_Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.33f, 0, DisAppear_Effect_1));

        IEnumerator move_Second = Move_Round_Trip(-2f, 2, 2f, 2);
        IEnumerator rotate = Rotate(150);
        StartCoroutine(move_Second);
        StartCoroutine(rotate);
       
       
        for (int i = 0; i < 6; i++)
        {
            StartCoroutine(cameraShake.Shake_Act(.07f, .2f, 0.2f, false));

            StartCoroutine(Boss_W1(110 + (i * 20), 30, 360));
            yield return new WaitForSeconds(0.4f); // 탄알 발사

            Instantiate(Weapon[1], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.4f); // 땅콩 발사
        }

        StopCoroutine(move_Second);
        StopCoroutine(rotate);
        yield break;
    }
    IEnumerator First_Move()
    {
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, -3, 0), .5f, declineCurve));
        yield return StartCoroutine(Boss_W1(72, 9, 180));

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 45), 13));

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(-7, 3, 0), .5f, declineCurve));
        yield return StartCoroutine(Boss_W1(252, 9, 180));

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 45), Quaternion.Euler(0, 0, 180), 13));

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(-7, -3, 0), .5f, declineCurve));
        yield return StartCoroutine(Boss_W1(-18, 9, 180));

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 180), Quaternion.Euler(0, 0, 315), 13));

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 3, 0), .5f, declineCurve));
        yield return StartCoroutine(Boss_W1(162, 9, 180));

        yield return StartCoroutine(Rotate_Dec(Quaternion.Euler(0, 0, 315), Quaternion.Euler(0, 0, 360), 13));

        yield return StartCoroutine(Change_Color_Return_To_Origin(Color.white, new Color(1, 69 / 255, 69 / 255, 1), 1, false));

        yield return StartCoroutine(Position_Slerp(transform.position, new Vector3(-7, -4, 0), 0.45f, 7, "up", declineCurve));
        yield return StartCoroutine(Position_Slerp(transform.position, new Vector3(7, 0, 0), 0.45f, 7, "down", declineCurve));

        StopCoroutine(camera_shake);
        GameObject.Find("Main Camera").transform.position = new Vector3(0, 0, -10);
        GameObject.Find("Main Camera").transform.rotation = Quaternion.identity;
        GameObject.Find("Main Camera").transform.localScale = new Vector3(1, 1, 1);

        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 2, 0), .7f, declineCurve));
        yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(7, 0, 0), .7f, declineCurve));
    }
    IEnumerator Move_Round_Trip(float x_f, float y_f, float x_l, float y_l)
    {
        bool move_dir = false;
        while (true)
        {
            if (move_dir)
                yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(x_f, y_f, 0), 1, De_In_Curve));
            else
                yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(x_l, y_l, 0), 1, De_In_Curve));

            move_dir = !move_dir;
        }
    }
    IEnumerator Rotate(int Degree)
    {
        bool rotate_dir = true;
        for (int i = 0; i < Degree; i++)
        {
            if (i == Degree - 1)
            {
                i = 0;
                rotate_dir = !rotate_dir;
            }
            if (rotate_dir)
                transform.Rotate(Vector3.forward * 100 *  Time.deltaTime);
            else
                transform.Rotate(Vector3.back * 100 * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator Boss_W1(float start_angle, int count, float range_angle)
    {
        float count_per_radian = range_angle / count;
        float intervalAngle = start_angle;
        for (int i = 0; i < count; i++)
        {
            float angle = intervalAngle + (i * count_per_radian);
            float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
            float y = Mathf.Sin(angle * Mathf.PI / 180.0f);
            Launch_Weapon_For_Move(Weapon[0], new Vector3(x, y), Quaternion.identity, 2f);
        }
        yield return null;
    }
    void Update()
    {

    }
}
