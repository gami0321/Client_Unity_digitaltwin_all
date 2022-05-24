using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
    // Start is called before the first frame update
    public void SceneChange_Cam() {
        SceneManager.LoadScene("mode2");
    }
}
