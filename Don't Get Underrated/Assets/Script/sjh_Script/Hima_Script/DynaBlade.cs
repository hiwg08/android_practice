using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynaBlade : Enemy_Info
{
    // Start is called before the first frame update

    Dictionary<int, Vector3> D;


    private new void Awake()
    {
        base.Awake();
        D = new Dictionary<int, Vector3>();
        D.Add(0, new Vector3(6.8f, 4.46f, 0));
        D.Add(1, new Vector3(-5.67f, 2.44f, 0));
        D.Add(2, new Vector3(-5.67f, 0.99f, 0));
        D.Add(3, new Vector3(5.81f, -1.29f, 0));
        D.Add(4, new Vector3(5.81f, -2.99f, 0));
        D.Add(5, new Vector3(-13.77f, 3.77f, 0));
    }
    void Start()
    {
        transform.position = new Vector3(6.97f, 2.77f, 0);
        transform.localScale = new Vector3(0.3f, 0.3f, 0);
        StartCoroutine(Move());
    }
   

    IEnumerator Move() // 루트3 / 2 (0.85)로 끝맺음 짓는게 좋다.
    {
        IEnumerator size = Size_Change_Infinite(1.3f);
        StartCoroutine(size);
        Plus_Speed = 0;
        
        float A = Get_Slerp_Distance(D[0], D[1], Get_Center_Vector(D[0], D[1], Vector3.Distance(D[0], D[1]) * 0.85f, "anti_clock"));

        float B = Get_Slerp_Distance(D[1], D[2], Get_Center_Vector(D[1], D[2], Vector3.Distance(D[1], D[2]) * 0.85f, "anti_clock"));

        float C = Get_Slerp_Distance(D[2], D[3], Get_Center_Vector(D[2], D[3], Vector3.Distance(D[2], D[3]) * 0.85f, "anti_clock"));

        float Q = Get_Slerp_Distance(D[3], D[4], Get_Center_Vector(D[3], D[4], Vector3.Distance(D[3], D[4]) * 0.85f, "clock"));

        float W = Get_Slerp_Distance(D[4], D[5], Get_Center_Vector(D[4], D[5], Vector3.Distance(D[4], D[5]) * 0.85f, "anti_clock"));

       // camera_shake = cameraShake.Shake_Act(.02f, .15f, 1, true);
        //StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(D[0], D[1], Get_Center_Vector(D[0], D[1], Vector3.Distance(D[0], D[1]) * 0.85f, "anti_clock"), 15f, OriginCurve, true));
       // StopCoroutine(camera_shake);

       // camera_shake = cameraShake.Shake_Act(.025f, .16f, 1, true);
       // StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(D[1], D[2], Get_Center_Vector(D[1], D[2], Vector3.Distance(D[1], D[2]) * 0.85f, "anti_clock"), B/A * 15f, OriginCurve, true));
        //StopCoroutine(camera_shake);

        //camera_shake = cameraShake.Shake_Act(.03f, .18f, 1, true);
        //StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(D[2], D[3], Get_Center_Vector(D[2], D[3], Vector3.Distance(D[2], D[3]) * 0.85f, "anti_clock"), C/A * 15f, OriginCurve, true));
        //StopCoroutine(camera_shake);

       // camera_shake = cameraShake.Shake_Act(.035f, .2f, 1, true);
       // StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(D[3], D[4], Get_Center_Vector(D[3], D[4], Vector3.Distance(D[3], D[4]) * 0.85f, "clock"), Q/A * 15f, OriginCurve, true));
        //StopCoroutine(camera_shake);

       // camera_shake = cameraShake.Shake_Act(.04f, .2f, 1.5f, false);
       // StartCoroutine(camera_shake);
        yield return StartCoroutine(Position_Slerp_Temp(D[4], D[5], Get_Center_Vector(D[4], D[5], Vector3.Distance(D[4], D[5]) * 0.85f, "anti_clock"), W/A * 20f, OriginCurve, true));
        //StopCoroutine(size);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
