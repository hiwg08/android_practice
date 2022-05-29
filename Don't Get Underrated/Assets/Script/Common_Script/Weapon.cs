using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected GameObject Explosion;

    private Movement2D movement2D;

    private CameraShake cameraShake;

    private IEnumerator camera_shake;

    protected virtual void Awake()
    {
        if (TryGetComponent(out Movement2D M_2D))
            movement2D = M_2D;
        if (TryGetComponent(out CameraShake CS) && GameObject.Find("Main Camera"))
        {
            cameraShake = CS;
            cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
    public void W_MoveTo(Vector3 Origin)
    {
        movement2D.MoveTo(Origin);
    }
    public void W_MoveSpeed(float Origin)
    {
        movement2D.MoveSpeed = Origin;
    }
    public virtual void Weak_Weapon()
    {
        Destroy(gameObject);
    }
    protected void Start_Camera_Shake(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (camera_shake != null)
            cameraShake.StopCoroutine(camera_shake);
        camera_shake = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
        cameraShake.StartCoroutine(camera_shake);
    }
    protected IEnumerator Start_Camera_Shake_For_Wait(float shake_intensity, float time_persist, bool is_Decline_Camera_Shake, bool is_Continue)
    {
        if (camera_shake != null)
            cameraShake.StopCoroutine(camera_shake);
        camera_shake = cameraShake.Shake_Act(shake_intensity, time_persist, is_Decline_Camera_Shake, is_Continue);
        yield return cameraShake.StartCoroutine(camera_shake);
    } // 되도록이면 사용 자제
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
