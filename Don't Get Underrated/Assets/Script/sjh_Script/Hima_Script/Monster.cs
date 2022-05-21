using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Monster : Enemy_Info
{

    Animator animator;
    private new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(I_Start());
    }
    IEnumerator I_Start()
    {
        transform.DOMove(new Vector3(-7, 4, 0), 1f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            animator.SetTrigger("hehe");
        });
        yield return null;
    }
    public void OnLazor()
    {
        StartCoroutine(Monster_Only_Lazor(Vector3.zero, transform.position, 1));
    }

    IEnumerator Monster_Only_Lazor(Vector3 Target, Vector3 Origin, float time_persist)
    {
        percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime / time_persist;
            Launch_Weapon_For_Move_Blink(Weapon[0], Target - Origin, Quaternion.identity, 8, false, Origin);
            yield return null;
        }
        animator.SetTrigger("die");
        yield return null;
    }

    public override void OnDie()
    {
        base.OnDie();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
