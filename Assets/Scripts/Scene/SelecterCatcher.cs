using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelecterCatcher : MonoBehaviour
{
    public static int selectedAnim;
    [SerializeField] private GameObject thirdViewChar;

    //recojer información de Animation Selecter
    private void Start()
    {
        ApplyAnimation();
    }

    private void ApplyAnimation()
    {
        Animator animator = thirdViewChar.GetComponent<Animator>();

        if (selectedAnim == 1)
        {
            animator.SetBool("Wave", true);
        }
        if (selectedAnim == 2)
        {
            animator.SetBool("House", true);
        }
        if (selectedAnim == 3)
        {
            animator.SetBool("Macarena", true);
        }

    }
}
