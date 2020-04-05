﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadGame : MonoBehaviour
{

     public void loadG(string nomeScena)
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene(nomeScena);
        AvviaMusicaGioco();
    }

    public void AvviaMusicaGioco()
    {
        GameObject.Find("MenuMusic").GetComponent<AudioSource>().Stop();
        GameObject.Find("MenuMusic").GetComponent<AudioSource>().enabled = false;
    }
}
