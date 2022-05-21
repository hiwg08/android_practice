using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Quantum_Bit : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("심마니");
        Sequence mysequence = DOTween.Sequence();
        mysequence.Append(transform.DOMove(new Vector3(Mathf.Sign(transform.position.x) * 5, 2, 0), 1f).SetEase(Ease.OutBounce));
        mysequence.Append(transform.DOMove(new Vector3(0, 2, 0), 1f).SetEase(Ease.InExpo));
        mysequence.OnComplete(() =>
        {
            StartCoroutine(Tr_Co());
        });
        Debug.Log("이거 웬걸");
   
    }
    public IEnumerator Tr_Co()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        // 여기에 뭘 넣을까
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
