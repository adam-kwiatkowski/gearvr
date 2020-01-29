using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public void TouchpadPressed()
    {
        //MeshRenderer renderer = GetComponent<MeshRenderer>();
        //renderer.enabled = !renderer.enabled;
        PlayAnimation();
    }

    public void TriggerPressed()
    {
        PlayAnimation();
    }

    void PlayAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("question");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
