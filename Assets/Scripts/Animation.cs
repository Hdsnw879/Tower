using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;
    public GameObject up,down,left,right;
    void Start()
    {
        up.SetActive(false);
        down.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);

    }
    public void TriggerNextImageObject(int index)
    {
        if (index == 1)
        {
            right.SetActive(false);
            left.SetActive(true);
            animator1.ResetTrigger("Next");
            animator2.SetTrigger("Next");
            
        }
        else if (index == 2)
        {
            left.SetActive(false);
            down.SetActive(true);
            animator2.ResetTrigger("Next");
            animator3.SetTrigger("Next");
            
        }
        else if (index == 3)
        {   
            down.SetActive(false);
            up.SetActive(true);
            animator3.ResetTrigger("Next");
            animator4.SetTrigger("Next");
            
        }else if (index == 4)
        {
            up.SetActive(false);
            right.SetActive(true);
            animator4.ResetTrigger("Next");
            animator1.SetTrigger("Next");
            
        }
    }
}
