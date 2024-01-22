using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator crossfade;


    //codigo que activa corutina para poder ser usado en otros scripts
    public void LoadNextLevel(int level)
    {
        StartCoroutine(LoadLevel(level));   
    }

    //corutina la cual maneja la animación entre escenas y la carga de nueva escenas
    IEnumerator LoadLevel(int level)
    {
        crossfade.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(level);
    }
}
