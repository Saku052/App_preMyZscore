using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //時間操作に必要なオブジェクトの取得
    [SerializeField] private GameObject Play;
    [SerializeField] private GameObject Pause;
    [SerializeField] private GameObject Clock;
    //ゲーム画面に表示する経過時間
    static private float seconds;
    static private int minutes;
    static private int hours;
    private string timer;
    //ボタンの切り替わり用Bool
    private bool ActiveButton = true;

    //設定画面パネルの取得
    [SerializeField] private GameObject SettingsPanel;




    private void Start() 
    {
        //初期値の設定
        GameManager.seconds = 0.0f;  
        GameManager.minutes = 0;
        GameManager.hours = 0;

        //とりまパネルの非表示
        this.SettingsPanel.SetActive(false);
        this.SettingBack.SetActive(false);
    }
    private void Update()
    {
        //PlayボタンがONの時Pauseボタンを無効化する
        //逆もあり
        Play.SetActive(this.ActiveButton);
        Pause.SetActive(!this.ActiveButton);

        //Playボタンを押したらScoreをカウントするスクリプト
        ScoreAdd();

        //Scoreを画面に表示するスクリプト
        ShowScore();
    }



    //ボタンの処理

    //Play, Pause　ボタンを押した時の処理
    public void PressPlay()
    {
        //Playボタンを非表示にする
        //Pauseボタンを表示する
        this.ActiveButton = false;
    }
    public void PressPause()
    {
        //Playボタンを表示する
        //pauseボタンを非表示にする
        this.ActiveButton = true;
    }
    public void PressRestart()
    {
        //とりあえずPlayボタンを表示して
        //カウントを停止する
        this.ActiveButton = true;
        //タイマーの数値を初期化
        GameManager.seconds = 0.0f;
        GameManager.minutes = 0;
        GameManager.hours = 0;
    }





    //プライベートな処理のメソッド

    //Playボタンを押したらScoreをカウントするスクリプト
    private void ScoreAdd()
    {
        if(this.ActiveButton == false){
            GameManager.seconds += Time.deltaTime;
            if(GameManager.seconds >= 60.0){
                GameManager.minutes++;
                GameManager.seconds -= 60.0f;
            }
            if(GameManager.minutes >= 60){
                GameManager.hours++;
                GameManager.minutes -= 60;
            }
        }
    }

    //Scoreを画面に表示するスクリプト
    private void ShowScore()
    {
        //Staticな変数に時間の情報を入力する
        this.timer = "" +
        GameManager.hours.ToString("00") + ":" +
        GameManager.minutes.ToString("00") + ":" +
        GameManager.seconds.ToString("00.00");

        //画面に表示する
        this.Clock.transform.GetComponentInChildren<Text>().text = 
        GameManager.hours.ToString("00") + ":" +
        GameManager.minutes.ToString("00");

        this.Clock.transform.GetChild(1).GetComponent<Text>().text = 
        GameManager.seconds.ToString("00.00");
    }

    [SerializeField] private GameObject SettingBack;

    //設定パネルの表示
    public void OpenSettingsPanel()
    {
        this.SettingBack.SetActive(true);
        this.SettingsPanel.SetActive(true);
    }
    //設定パネルの非表示
    public void CloseSettingPanel()
    {
        this.SettingBack.SetActive(false);
        this.SettingsPanel.SetActive(false);
    }
    public void openURL()
    {
        Application.OpenURL("https://saku052.github.io/PrivacyPolicy/");
    }

    private float sysTimeNow;
    private float sysTimeDiffer;



    //離れた時にも換算するようにする
    private void OnApplicationPause(bool pauseStatus) {
    if (pauseStatus & !this.ActiveButton) {
        
        this.sysTimeNow = Time.realtimeSinceStartup;
        
        Debug.Log(Time.realtimeSinceStartup);

    }
    else if(!pauseStatus & !this.ActiveButton){

        this.sysTimeDiffer = Time.realtimeSinceStartup - this.sysTimeNow;

        while(this.sysTimeDiffer >= 3600.0f){
            GameManager.hours++;
            this.sysTimeDiffer -= 3600.0f;
        }
        while(this.sysTimeDiffer >= 60.0f){
            GameManager.minutes++;
            this.sysTimeDiffer -= 60.0f;
        }
        while(this.sysTimeDiffer >= 0.0f){
            GameManager.seconds++;
            this.sysTimeDiffer -= 1.0f;
        }
    }
    }




    //時間系のゲーター
    static public int Gethour()
    {
        return GameManager.hours;
    }
    static public int GetMin()
    {
        return GameManager.minutes;
    }
    static public float GetSec()
    {
        return GameManager.seconds;
    }
}
