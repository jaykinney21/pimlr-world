using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestoryOnLoadHelixUI : MonoBehaviour
{ 
    void Awake() { 
        DontDestroyOnLoad(gameObject);
    }
}
