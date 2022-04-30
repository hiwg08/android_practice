using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolGryn : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
    CameraShake cameraShake;


    private void Awake()
    {
        cameraShake = GetComponent<CameraShake>();
    }

    void Start()
    {
        transform.position = new Vector3(7, 3, 0);
        StartCoroutine("Move");
    }
    IEnumerator Move()
    {
        float temp = 0.4f;
        while (transform.position.y >= -3)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.02f;
            yield return null;
        }
        temp = 0.4f;
        cameraShake.Shake();
        while (transform.position.x >= -7 && transform.position.y <= 3)
        {
            transform.position = new Vector3(transform.position.x - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.y + (0.4286f * Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.01f;
            yield return null;
        }
        temp = 0.4f;
        cameraShake.Shake();
        while (transform.position.y >= -3)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.02f;
            yield return null;
        }
        temp = .4f;
        cameraShake.Shake();
        while (transform.position.x <= 7 && transform.position.y <= 3)
        {
            transform.position = new Vector3(transform.position.x + (Mathf.Pow(5, temp) * Time.deltaTime), transform.position.y + (0.4286f * Mathf.Pow(5, temp) * Time.deltaTime), transform.position.z);
            temp += 0.01f;
            yield return null;
        }
        cameraShake.Shake();
    }
}
