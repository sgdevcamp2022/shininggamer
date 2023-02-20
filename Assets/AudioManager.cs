using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(scene.name.Contains("Tile")||scene.name.Contains("Fight"))
            Destroy(gameObject);
    }

}
