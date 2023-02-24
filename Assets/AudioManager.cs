using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource backsound;

    void Start(){
        SceneManager.sceneLoaded += LoadedsceneEvent;
        DontDestroyOnLoad(this.gameObject);
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        try{
            if(gameObject!=null)
                if(scene.name.Contains("Tile"))
                    Destroy(gameObject);
        }catch(Exception e){

        }
    }

}
