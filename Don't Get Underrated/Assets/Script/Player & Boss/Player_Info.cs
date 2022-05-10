using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Info : Player_And_Boss
{
    // Start is called before the first frame update

    [SerializeField]
    protected int LifeTime = 5;

    [SerializeField]
    protected GameObject[] Item;

    protected bool weapon_able;

    private int main_1_score, main_2_score, main_3_score, final_score;

    public int Main_1_Score
    {
        set { main_1_score = value; }
        get { return main_1_score; }
    }

    public int Main_2_Score
    {
        set { main_2_score = value; }
        get { return main_2_score; }
    }
    public int Main_3_Score
    {
        set { main_3_score = value; }
        get { return main_3_score; }
    }
    public int Final_Score
    {
        set { final_score = value; }
        get { return final_score; }
    }
    public virtual void TakeDamage()
    {
        LifeTime -= 1;
    }
    public override void OnDie()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
