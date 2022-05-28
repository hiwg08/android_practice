using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class For_Continuous_Slerp_Move
{
    private Vector3 next_Position;
    private string dir;
    public For_Continuous_Slerp_Move(Vector3 _next_Position, string _dir)
    {
        next_Position = _next_Position;
        dir = _dir;
    }
    public Vector3 Next_Position => next_Position;
    public string Dir => dir;
}
public class Life : MonoBehaviour, Life_Of_Basic
{
    [SerializeField]
    protected GameObject[] Weapon; // 상위

    [SerializeField]
    protected AnimationCurve declineCurve; // 상위

    [SerializeField]
    protected AnimationCurve OriginCurve;

    [SerializeField]
    protected AnimationCurve inclineCurve;

    [SerializeField]
    protected AnimationCurve De_In_Curve;

    [SerializeField]
    protected GameObject When_Dead_Effect;

    [SerializeField]
    protected StageData stageData;

    protected SpriteRenderer spriteRenderer;

    protected Movement2D movement2D;

    protected ImageColor backGroundColor;

    private CameraShake cameraShake;

    private IEnumerator camera_shake_i;

    private IEnumerator back_ground_color_i;

    private bool unbeatable;

    public bool Unbeatable
    {
        get { return unbeatable; }
        set { unbeatable = value; }    
    }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Unbeatable = false;
        if (TryGetComponent(out CameraShake user))
            cameraShake = user;
        if (cameraShake != null && cameraShake.mainCamera == null && GameObject.Find("Main Camera").TryGetComponent(out Camera camera))
            cameraShake.mainCamera = camera;
    }
    public Color SpriteRenderer_Color
    {
        get { return spriteRenderer.color; }
        set { spriteRenderer.color = value; }
    }
    public Vector3 My_Scale
    {
        get { return transform.localScale; }
        set { transform.localScale = value; }
    }
    public Vector3 My_Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public virtual void TakeDamage(float damage) 
    {
        if (When_Dead_Effect != null)
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damage) 
    {
        if (When_Dead_Effect != null)
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public virtual void OnDie()
    {
        if (When_Dead_Effect != null)
            Instantiate(When_Dead_Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    protected IEnumerator Circle_Move(int Degree, int is_ClockWise_And_Speed, float Start_Degree, float x, float y, float start_x, float start_y, float time_persist)
    {
        // 각도, 시계/반시계 방향, x축 원, y축 원

        for (int th = 0; th < Degree; th++)
        {
            float rad = Mathf.Deg2Rad * (is_ClockWise_And_Speed * th + Start_Degree);

            float rad_x = x * Mathf.Sin(rad);
            float rad_y = y * Mathf.Cos(rad);

            transform.position = new Vector3(start_x + rad_x, start_y + rad_y, 0);
            yield return YieldInstructionCache.WaitForSeconds(Time.deltaTime * time_persist);
        }
        yield return null;
    }
    protected void Launch_Weapon(ref GameObject weapon, Vector3 Direction, Quaternion Degree, float speed, Vector3 Instantiate_Dir) // 이외의 나머지
    {
        GameObject Copy = Instantiate(weapon, Instantiate_Dir, Degree);
        if (Copy.TryGetComponent(out Weapon_Devil user1))
        {
            user1.W_MoveTo(Direction);
            user1.W_MoveSpeed(speed);
        }
        else if (Copy.TryGetComponent(out Weapon_Player user2))
        {
            user2.W_MoveTo(Direction);
            user2.W_MoveSpeed(speed);
        }
        else
            Destroy(Copy);
    }
    protected IEnumerator Change_My_Size(Vector3 Origin, Vector3 Change, float time_persist, AnimationCurve curve)
    {
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime / time_persist;
            transform.localScale = Vector3.Lerp(Origin, Change, curve.Evaluate(percent));
            yield return null;
        }
    }
    protected IEnumerator Change_Color_Return_To_Origin(Color Origin_C, Color Change_C, float time_persist, bool is_Continue)
    {
        float percent;
        while (true)
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                spriteRenderer.color = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                spriteRenderer.color = Color.Lerp(Change_C, Origin_C, percent);
                yield return null;
            }
            if (!is_Continue)
                break;
        }
    }
    protected IEnumerator My_Color_When_UnBeatable()
    {
        bool ee = false;

        while (true)
        {
            if (!ee)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            yield return new WaitForSeconds(0.2f);
            ee = !ee;
        }
    }
    protected IEnumerator Change_My_Color_Lerp(Color Origin_C, Color Change_C, float time_persist, float Wait_Second, GameObject Effect)
    {
        float percent;
        if (Effect != null)
            Instantiate(Effect, transform.position, Effect.transform.localRotation);
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / time_persist;
            spriteRenderer.color = Color.Lerp(Origin_C, Change_C, percent);
            yield return null;
        }
        yield return YieldInstructionCache.WaitForSeconds(Wait_Second);
    }

    public IEnumerator Move_Straight(Vector3 start_location, Vector3 last_location, float time_ratio, AnimationCurve curve)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime / time_ratio);
            transform.position = Vector3.Lerp(start_location, last_location, curve.Evaluate(percent));
            yield return null;
        }
    }
    protected IEnumerator Move_Curve(Vector3 start_location, Vector3 last_location, Vector3 center, float time_ratio, AnimationCurve curve)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / time_ratio;
            Vector3 riseRelCenter = start_location - center;
            Vector3 setRelCenter = last_location - center;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, curve.Evaluate(percent));

            transform.position += center;
            yield return null;
        }
        yield return null;
    }
    protected float Get_Curve_Distance(Vector3 Start, Vector3 End, Vector3 Center)
    {
        Vector3 Center_to_Start = Start - Center;
        Vector3 Center_to_Last = End - Center;

        float theta = Vector3.Dot(Center_to_Start, Center_to_Last) / (Center_to_Start.magnitude * Center_to_Last.magnitude);
        float angle = Mathf.Acos(theta) * Mathf.Rad2Deg;
        return (2 * Mathf.PI * Vector3.Distance(Center_to_Start, Vector3.zero) * angle) / 360;
    }
    protected Vector3 Get_Center_Vector(Vector3 Start, Vector3 End, float Center_To_Real_Center_Distance, string is_Clock)
    {
        Vector3 Origin_V = Start - End;
        Vector3 Center = (Start + End) / 2;
        Vector3 Dot_Vector = Vector3.zero;

        if (Origin_V.x != 0 && Origin_V.y == 0)
            Dot_Vector = Vector3.up;

        else if (Origin_V.x == 0 && Origin_V.y != 0)
            Dot_Vector = Vector3.right;

        else if (Origin_V.x != 0 && Origin_V.y != 0)
            Dot_Vector = new Vector3(1, -Origin_V.x / Origin_V.y, 0);

        // 기본적으로는 아랫방향이다.
        if (is_Clock == "anti_clock")
            Dot_Vector = -Dot_Vector; // 이거는 윗방향일때 한정.

        Dot_Vector = Dot_Vector.normalized;

        return new Vector3(Center.x - Dot_Vector.x * Center_To_Real_Center_Distance, Center.y - Dot_Vector.y * Center_To_Real_Center_Distance);
    }
    protected IEnumerator Change_My_Size_Infinite(float delta_ratio)
    {
        while (true)
        {
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * delta_ratio, transform.localScale.y + Time.deltaTime * delta_ratio, 0);
            yield return null;
        }
    }
    protected IEnumerator My_Rotate_Dec(Quaternion A, Quaternion B, float time_persist, AnimationCurve curve)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += (Time.deltaTime / time_persist);
            transform.rotation = Quaternion.Lerp(A, B, curve.Evaluate(percent));
            yield return null;
        }
    }
    protected void Camera_Shake(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (cameraShake != null)
        {
            if (camera_shake_i != null)
                cameraShake.StopCoroutine(camera_shake_i);
            camera_shake_i = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
            cameraShake.StartCoroutine(camera_shake_i);
        }
      
    }
    protected IEnumerator Camera_Shake_And_Wait(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (cameraShake != null)
        {
            if (camera_shake_i != null)
                cameraShake.StopCoroutine(camera_shake_i);
            camera_shake_i = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
            yield return cameraShake.StartCoroutine(camera_shake_i);
        }
        else
            yield return null;
    } // 되도록이면 사용 자제

    protected void Flash(Color FlashColor, float wait_second, float time_persist)
    {
        if (backGroundColor != null)
        {
            if (back_ground_color_i != null)
                backGroundColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = backGroundColor.Flash(FlashColor, wait_second, time_persist);
            backGroundColor.StartCoroutine(back_ground_color_i);
        }
    }
    protected IEnumerator Flash_And_Wait(Color FlashColor, float wait_second, float time_persist)
    {
        if (backGroundColor != null)
        {
            if (back_ground_color_i != null)
                backGroundColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = backGroundColor.Flash(FlashColor, wait_second, time_persist);
            yield return backGroundColor.StartCoroutine(back_ground_color_i);
        }
        else
            yield return null;
    } // 되도록이면 사용 자제
    protected void Change_BG(Color Change, float time_persist)
    {
        if (backGroundColor != null)
        {
            if (back_ground_color_i != null)
                backGroundColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = backGroundColor.Change_BG(Change, time_persist);
            backGroundColor.StartCoroutine(back_ground_color_i);
        }
    }
    protected IEnumerator Change_BG_And_Wait(Color Change, float ratio)
    {
        Color Origin = Color.red;
        if (backGroundColor != null)
        {
            if (back_ground_color_i != null)
                backGroundColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = backGroundColor.Change_BG(Change, ratio);
            yield return backGroundColor.StartCoroutine(back_ground_color_i);
        }
        else
            yield return null;
    } // 되도록이면 사용 자제
    protected void Change_Origin_BG(Color Change, float time_persist)
    {
        if (backGroundColor != null)
        {
            if (back_ground_color_i != null)
                backGroundColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = backGroundColor.Change_Origin_BG(Change, time_persist);
            backGroundColor.StartCoroutine(back_ground_color_i);
        }
    }
    protected IEnumerator Change_Origin_BG_And_Wait(Color Change, float time_persist)
    {
        if (backGroundColor != null)
        {
            if (back_ground_color_i != null)
                backGroundColor.StopCoroutine(back_ground_color_i);
            back_ground_color_i = backGroundColor.Change_Origin_BG(Change, time_persist);
            yield return backGroundColor.StartCoroutine(back_ground_color_i);
        }
        else
            yield return null;
    }
}