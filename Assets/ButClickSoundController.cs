using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButClickSoundController : MonoBehaviour
{

    [SerializeField]
    AudioSource butclick;


    public void ButClickSound(){
        butclick.Play();
    }
}
