using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class DataControl : MonoBehaviour
{
//表示するリストの取得
[SerializeField] private GameObject savelist;
[SerializeField] private GameObject openlist;

//押されたボタンの情報
[SerializeField] private EventSystem eventSystem;
static public GameObject buttonObj;
static public string nameButton;
static public string ButtonCode;

//Listの管理
static public List<int> hourData;
static public List<int> minData;
static public List<float> secData;
static public List<string> DateATime;
static public int numOfData;

//保存用の時間情報
private int hour;
private int min;
private float sec;


private void Start() 
{
    //新しく入力された名前に変える
    ChangeNameOfButton();

    //データの数が０ならば表示するパネルを非表示にする
    closeDataZero();

    //リストの初期化
    DataControl.hourData = new List<int>();
    DataControl.minData = new List<int>();
    DataControl.secData = new List<float>();
    DataControl.DateATime = new List<string>();
    DataControl.hourData.Clear();
    DataControl.minData.Clear();
    DataControl.secData.Clear();
    DataControl.DateATime.Clear();
}

//データが０の時に表示するパネルを取得する
[SerializeField] private GameObject CoverNoDataPanel;
[SerializeField] private GameObject noDataPanel;

private void closeDataZero()
{
    this.CoverNoDataPanel.SetActive(false);
    this.noDataPanel.SetActive(false);
}
public void OpenData()
{
    //押されたボタンを取得する
    DataControl.buttonObj = eventSystem.currentSelectedGameObject;
    //押されたボタンの名前
    DataControl.nameButton = DataControl.buttonObj.transform.GetChild(0).gameObject.GetComponent<Text>().text;
    //押されたボタンのオブジェクトネーム
    DataControl.ButtonCode = DataControl.buttonObj.name;

    //読み込むのが初めてなら初期化する
    CheckDataInit();
    //初めてでなければ、以前のデータを取得する
    AddSavedNumData();

    //セーブしてあるデータを全てリストに追加する
    for(int i = 1; i <= DataControl.numOfData; i++){
        if(PlayerPrefs.HasKey(DataControl.buttonObj.name + "Data" + i + "hour")){
        DataControl.hourData.Add(PlayerPrefs.GetInt(DataControl.buttonObj.name + "Data" + i + "hour"));
        DataControl.minData.Add(PlayerPrefs.GetInt(DataControl.buttonObj.name + "Data" + i + "Min"));
        DataControl.secData.Add(PlayerPrefs.GetFloat(DataControl.buttonObj.name + "Data" + i + "Sec"));
        DataControl.DateATime.Add(PlayerPrefs.GetString(DataControl.buttonObj.name + i + "Date"));
        }
    }

    //シーンを切り替えるかパネルを表示する
    if(DataControl.numOfData == 0){
        this.CoverNoDataPanel.SetActive(true);
        this.noDataPanel.SetActive(true);
    }
    else{
    SceneManager.LoadScene("DataScene");
    }
}
    


public void SaveData()
{
    //押されたボタンを取得する
    DataControl.buttonObj = eventSystem.currentSelectedGameObject;

    //時間データを一旦こちらにセーブする
    this.hour = GameManager.Gethour();
    this.min = GameManager.GetMin();
    this.sec = GameManager.GetSec();
    
    //セーブするのが初めてなら初期化する
    CheckDataInit();
    //初めてでなければ、以前のデータを取得する
    AddSavedNumData();

    //ボタンが押された時間を取得する
    setTime();

    //セーブするたびにリストの数を追加
    DataControl.numOfData++;

    //PlayerPrefsに全てのデータを永久保存
    PlayerPrefs.SetInt(DataControl.buttonObj.name + "num", DataControl.numOfData);
    PlayerPrefs.SetString(DataControl.buttonObj.name + DataControl.numOfData + "Date", this.dateTimeT);
    PlayerPrefs.SetInt(DataControl.buttonObj.name + "Data" + DataControl.numOfData + "hour", this.hour);
    PlayerPrefs.SetInt(DataControl.buttonObj.name + "Data" + DataControl.numOfData + "Min", this.min);
    PlayerPrefs.SetFloat(DataControl.buttonObj.name + "Data" + DataControl.numOfData + "Sec", this.sec);
    PlayerPrefs.Save();
}

private string dateTimeT;

private void setTime()
{
    //時間を取得
        DateTime TodayNow = DateTime.Now;

        //テキストUIに年・月・日・秒を表示させる
        this.dateTimeT = TodayNow.Year.ToString() + "年 " + TodayNow.Month.ToString() + "月" + TodayNow.Day.ToString() + "日" + DateTime.Now.ToLongTimeString();
}


//データがゼロなら表示するパネルの戻るボタン
public void zeroButtonModoru()
{
    this.noDataPanel.SetActive(false);
    this.CoverNoDataPanel.SetActive(false);
}

private void CheckDataInit()
{
    if(!PlayerPrefs.HasKey(DataControl.buttonObj.name + "num")){
        DataControl.numOfData = 0;
    }
}
private void AddSavedNumData()
{
    if(PlayerPrefs.HasKey(DataControl.buttonObj.name + "num")){
        DataControl.numOfData = PlayerPrefs.GetInt(DataControl.buttonObj.name + "num");
    }
}
private void ChangeNameOfButton()
{
    //一番最初に名前を変える
    for(int i = 1; i <= 20; i++){
        if(PlayerPrefs.HasKey("Item" + i + "title")){
            
            this.savelist.transform.Find("Item" + i).GetComponentInChildren<Text>().text = 
            PlayerPrefs.GetString("Item" + i + "title");

            this.openlist.transform.Find("Item" + i).GetComponentInChildren<Text>().text = 
            "\n   " + PlayerPrefs.GetString("Item" + i + "title") + "\n";
        }
    }


    //新しく名前を変更した時に名前を変える
    if((DataControl.ButtonCode != null) & PlayerPrefs.HasKey(DataControl.ButtonCode + "title")){

        this.savelist.transform.Find(DataControl.ButtonCode).GetComponentInChildren<Text>().text = 
        PlayerPrefs.GetString(DataControl.ButtonCode + "title");

        this.openlist.transform.Find(DataControl.ButtonCode).GetComponentInChildren<Text>().text = 
        "\n   " + PlayerPrefs.GetString(DataControl.ButtonCode + "title") + "\n";
    }
}



}
