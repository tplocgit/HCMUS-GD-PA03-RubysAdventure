using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void GameOverScene() {
        ChangeScene("GameOver");
    }

    public void MainScene() {
        ChangeScene("MainScene");
    }

    public void Quit() {
        Application.Quit();
    }

    public void LoadGame()
    {
        LoadGameMenu loadGameMenu = this.gameObject.GetComponent<LoadGameMenu>();
        if(loadGameMenu == null) {  Debug.Log("Can't find LoadMenu"); return; }
        loadGameMenu.SetLoadData();
        MainScene();
    }

    public void StartGame()
    {
        MyGameManager.Instance.isNewGame = true;
        MainScene();
    }

    public void MainMenu()
    {
        ChangeScene("MenuScene");
    }

    public void LoadMenu()
    {
        ChangeScene("LoadGameScene");
    }
}
