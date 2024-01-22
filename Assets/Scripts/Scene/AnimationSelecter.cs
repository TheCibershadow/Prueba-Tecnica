using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationSelecter : MonoBehaviour
{
    public GameObject player;
    public GameObject selectButton;
    public GameObject popUpWarning;
    public LevelLoader levelLoader;
    private Animator animator;
    private int selectedAnim = 0;
    private string selectedData;

    void Start()
    {
        animator = player.GetComponent<Animator>();
    }

    // en caso de seleccionar uno de los tres botones se activa la animación 
    // se selecciona la info para el selecter catcher
    public void SelectWave()
    {
        animator.SetBool("Wave", true);
        animator.SetBool("House", false);
        animator.SetBool("Macarena", false);
        selectButton.SetActive(true);
        selectedAnim = 1;
    }

    public void SelectHouse()
    {
        animator.SetBool("Wave", false);
        animator.SetBool("House", true);
        animator.SetBool("Macarena", false);
        selectButton.SetActive(true);
        selectedAnim = 2;
    }

    public void SelectMacarena()
    {
        animator.SetBool("Wave", false);
        animator.SetBool("House", false);
        animator.SetBool("Macarena", true);
        selectButton.SetActive(true);
        selectedAnim = 3;
    }

    // confirmar la selección de una animación
    public void ConfirmSelection()
    {
        // en caso de que no se haya seleccionado un botón sale un Pop Up
        if (selectedAnim == 0) {
            popUpWarning.SetActive(true);
        }

        //Se selecciona una animación y se manda info al Selecter Catcher
        if(selectedAnim > 0)
        {
            SelecterCatcher.selectedAnim = selectedAnim;
            levelLoader.LoadNextLevel(1);
        }

       
    }

    // quitar el Pop Up
    public void PopUpButton()
    {
        popUpWarning.SetActive(false);
    }

}
