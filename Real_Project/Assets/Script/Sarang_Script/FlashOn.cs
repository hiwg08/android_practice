using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlashOn : MonoBehaviour
{
    Image image;
    float flashSpeed = 5f;
    Color flashColor = new Color(1, 1, 1, 1);
    // Start is called before the first frame update

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        
    }

    public IEnumerator Get_Flash()
    {
        image.color = flashColor;
        yield return new WaitForSeconds(.05f);

        while(true)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
            if (image.color.a <= 0)
                yield break;
            image.color = Color.Lerp(image.color, Color.clear, flashSpeed * Time.deltaTime);
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
