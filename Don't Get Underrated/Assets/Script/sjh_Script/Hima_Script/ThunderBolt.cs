using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Enemy_Info
{
    // Start is called before the first frame update

    private new void Awake()
    {
        base.Awake();
        flashOn = GameObject.Find("Flash").GetComponent<FlashOn>();
        //cameraShake.mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }


    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        
        //yield return StartCoroutine(Position_Lerp(transform.position, new Vector3(transform.position.x, 0, 0), 0.5f, OriginCurve));
        StartCoroutine(flashOn.White_Flash());
        //camera_shake = cameraShake.Shake_Act(.05f, .26f, 1, false);
        //StartCoroutine(camera_shake);
        //Destroy(gameObject);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
