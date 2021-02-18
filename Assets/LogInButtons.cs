using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInButtons : MonoBehaviour
{
    public SceneField signUpScene;

    public void SignUpButtonPressed() {

        SceneManager.LoadScene(signUpScene);

    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
