using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nachi_X : Enemy_Info
{
    TrailRenderer trailRenderer;

    float Decide_Camera_Shake;
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        trailRenderer = GetComponent<TrailRenderer>();
        Decide_Camera_Shake = transform.position.x;
    }
    IEnumerator X_Color_Change(Color Origin_C, Color Change_C, float time_persist)
    {
        for (int i = 0; i < 3; i++)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / time_persist;
                trailRenderer.endColor = Color.Lerp(Origin_C, Change_C, percent);
                trailRenderer.startColor = Color.Lerp(Origin_C, Change_C, percent);
                yield return null;
            }
        }
    }
    public IEnumerator Move(int flag)
    {
        trailRenderer.enabled = true;
        if (TryGetComponent(out TrailCollisions user1))
            user1.Draw_Collision_Line();
        yield return StartCoroutine(Circle_Move(90, flag * 4, 0, 0.3f, 0.3f, transform.position.x, transform.position.y, 0.3f));

        yield return StartCoroutine(Move_Straight(transform.position, transform.position + new Vector3(-4 * flag, 2f, 0), 0.4f, declineCurve));
        yield return StartCoroutine(Move_Straight(transform.position, transform.position + new Vector3(16 * flag, -8f, 0), 0.1f, OriginCurve));

        if (Decide_Camera_Shake < 0)
            Camera_Shake(0.03f, 2f, true, false);
        yield return StartCoroutine(X_Color_Change(Color.white, new Color(1, 1, 1, 0), 1));

        trailRenderer.enabled = false;
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
