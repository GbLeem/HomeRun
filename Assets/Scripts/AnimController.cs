using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public Animator anim;
    public BatController batController;

    private void Start()
    {
        anim = GetComponent<Animator>();
        batController = GetComponent<BatController>();
    }

    private void Update()
    {
        //if(batController.)
        if(Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("IsSwing", true);
            StartCoroutine(ResetSwing());
        }
    }

    IEnumerator ResetSwing()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("IsSwing", false);
    }
}
