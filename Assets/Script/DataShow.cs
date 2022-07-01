using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class DataShow : MonoBehaviour
{
    //データを表示するブロックの取得
    [SerializeField] private GameObject blockData;
    [SerializeField] private GameObject blockList;
    //新しく生成する子要素のリストオブジェクト
    private GameObject childObject;

    //データ詳細パネルを取得する
    [SerializeField] private GameObject InfoOfData;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject AddDataP;
    //選択されたGameObjectの取得
    private GameObject ListofData;
    //選択された子要素リスト
    private GameObject DeleteThis;
    //EventSystemの取得
    [SerializeField] private EventSystem eventSystem;
    
    //パラメータ用のイント値
    private int Onum;
    
    //Zscore関連の変数
    private List<float> zscore;
    private float average;
    private string AvValuse;
    [SerializeField] private GameObject DataAverageShow;

    private void Start() 
    {
        //偏差値の取得
        GetZScore();

        //子要素一つ一つに取得したデータを表示する
        showDataSet();

        //平均を表示する
        showAverage();

        //とりあえずInfoOfDataからListを取得する
        this.ListofData = this.InfoOfData.transform.GetChild(3).GetChild(0).GetChild(0).gameObject;
        //一つ一つのリストを非表示にする
        DisableeachButton();

        //データパネルの非表示
        this.BackToTitle.SetActive(false);
        this.InfoOfData.SetActive(false);
        this.AddDataP.SetActive(false);
        this.rDeleteCover.SetActive(false);
        this.rDeleteAll.SetActive(false);
        this.rDeleteOne.SetActive(false);
    }

    //平均を表示する
    private void showAverage()
    {
        this.DataAverageShow.GetComponent<Text>().text = this.AvValuse;
    }



    private void showDataSet()
    {
        for(int i = 1; i <= DataControl.numOfData; i++){
            //データを表示するブロックを生成する
            this.childObject = Instantiate(this.blockData) as GameObject;
            this.childObject.name = "Item" + i.ToString();
            this.childObject.transform.SetParent(this.blockList.transform, false);

            //時分秒のデータをstring型にする
            string showtext = 
            DataControl.hourData[i-1].ToString("00") + ":" +
            DataControl.minData[i-1].ToString("00") + ":" +
            DataControl.secData[i-1].ToString("00.00");

            //偏差値を追加する
            string showZscore = this.zscore[i - 1].ToString("f1");

            //それぞれのテキストコンポーネントに追加する
            this.childObject.transform.GetComponentInChildren<Text>().text = " " + showtext;
            this.childObject.transform.GetChild(1).GetComponent<Text>().text = "偏差値： " + showZscore;
            this.childObject.transform.GetChild(2).GetComponent<Text>().text = DataControl.DateATime[i-1];
        }
        
    }


    //データ詳細パネルの非表示
    public void closeInfoOfData()
    {
        this.InfoOfData.SetActive(false);
        this.backGround.SetActive(false);
    }

    //データ詳細パネルの表示
    public void showInfoOfData()
    {
        //パネルを両方表示する
        this.InfoOfData.SetActive(true);
        this.backGround.SetActive(true);

        ShowNewestData();
    }

    //データ追加パネルの表示
    public void showAddDataPanel()
    {
        AddDataP.SetActive(true);
        backGround.SetActive(true);
    }
    //データ追加パネルの非表示
    public void closeAddDataPanel()
    {
        AddDataP.SetActive(false);
        backGround.SetActive(false);
    }

    //データをセーブする変数を定義
    private int AddNewHour = 0;
    private int AddNewMin = 0;
    private int AddNewSec = 0;


    //入力されたデータを取得する
    public void savenewHour()
    {
        if(!int.TryParse(this.AddDataP.transform.GetChild(2).GetComponent<InputField>().text, out this.AddNewHour)){
            this.AddNewHour = 0;
        } 
    }
    public void savenewMin()
    {
        if(!int.TryParse(this.AddDataP.transform.GetChild(3).GetComponent<InputField>().text, out this.AddNewMin)){
            this.AddNewMin = 0;
        } 
    }
    public void savenewSec()
    {
        if(!int.TryParse(this.AddDataP.transform.GetChild(4).GetComponent<InputField>().text, out this.AddNewSec)){
            this.AddNewSec = 0;
        } 
    }


    //データの追加
    public void AddCreatedData()
    {
        setTime();

        DataControl.hourData.Add(this.AddNewHour);
        DataControl.minData.Add(this.AddNewMin);
        DataControl.secData.Add(this.AddNewSec);
        DataControl.DateATime.Add(this.dateTimeT);

        DataControl.numOfData++;

        PlayerPrefs.SetInt(DataControl.ButtonCode + "Data" + DataControl.numOfData + "hour", DataControl.hourData[DataControl.numOfData - 1]);
        PlayerPrefs.SetInt(DataControl.ButtonCode + "Data" + DataControl.numOfData + "Min", DataControl.minData[DataControl.numOfData - 1]);
        PlayerPrefs.SetFloat(DataControl.ButtonCode + "Data" + DataControl.numOfData + "Sec", DataControl.secData[DataControl.numOfData - 1]);
        PlayerPrefs.SetInt(DataControl.ButtonCode + "num", DataControl.numOfData);
        PlayerPrefs.SetString(DataControl.ButtonCode + DataControl.numOfData + "Date", this.dateTimeT);
        PlayerPrefs.Save();

        //データシーンをリロードする
        SceneManager.LoadScene("DataScene");
        
    }

    //時間の取得
    private string dateTimeT;

    private void setTime()
    {
    //時間を取得
        DateTime TodayNow = DateTime.Now;

        //テキストUIに年・月・日・秒を表示させる
        this.dateTimeT = TodayNow.Year.ToString() + "年 " + TodayNow.Month.ToString() + "月" + TodayNow.Day.ToString() + "日" + DateTime.Now.ToLongTimeString();
    }



    [SerializeField] private GameObject rDeleteOne;
    [SerializeField] private GameObject rDeleteAll;
    [SerializeField] private GameObject rDeleteCover;



    //データの削除
    public void deleteData()
    {
        //押されたボタンの取得
        this.DeleteThis = eventSystem.currentSelectedGameObject;

        this.rDeleteOne.SetActive(true);
        this.rDeleteCover.SetActive(true);
    }

    public void ReallyDeleteOne()
    {
        //押されたボタンのリストコード
        int i = DataControl.numOfData - int.Parse(this.DeleteThis.name.Replace("Item", ""));

        //そのコードでの削除
        DataControl.hourData.RemoveAt(i);
        DataControl.minData.RemoveAt(i);
        DataControl.secData.RemoveAt(i);
        DataControl.DateATime.RemoveAt(i);

        //永久的にデータを削除する
        for(int x = 1; x <= DataControl.numOfData; x++){
            PlayerPrefs.DeleteKey(DataControl.ButtonCode + "Data" + x + "hour");
            PlayerPrefs.DeleteKey(DataControl.ButtonCode + "Data" + x + "Min");
            PlayerPrefs.DeleteKey(DataControl.ButtonCode + "Data" + x + "Sec");
            PlayerPrefs.DeleteKey(DataControl.ButtonCode + DataControl.numOfData + "Date");
        }
        DataControl.numOfData--;

        for(int y = 1; y <= DataControl.numOfData; y++){
            PlayerPrefs.SetInt(DataControl.ButtonCode + "Data" + y + "hour", DataControl.hourData[y-1]);
            PlayerPrefs.SetInt(DataControl.ButtonCode + "Data" + y + "Min", DataControl.minData[y-1]);
            PlayerPrefs.SetFloat(DataControl.ButtonCode + "Data" + y + "Sec", DataControl.secData[y-1]);
            PlayerPrefs.SetString(DataControl.ButtonCode + y + "Date", DataControl.DateATime[y-1]);
        }

        PlayerPrefs.SetInt(DataControl.ButtonCode + "num", DataControl.numOfData);
        PlayerPrefs.Save();

        
        
    }

    

    public void closeDeleteOne()
    {
        this.rDeleteOne.SetActive(false);
        this.rDeleteCover.SetActive(false);
    }

    
    //データの一括消去
    public void deleteAllData()
    {
        this.rDeleteCover.SetActive(true);
        this.rDeleteAll.SetActive(true);
    }

    public void ReallyDeleteAll()
    {
        //永久的にデータを削除する
        for(int x = 1; x <= DataControl.numOfData; x++){
            PlayerPrefs.DeleteKey(DataControl.ButtonCode + "Data" + x + "hour");
            PlayerPrefs.DeleteKey(DataControl.ButtonCode + "Data" + x + "Min");
            PlayerPrefs.DeleteKey(DataControl.ButtonCode + "Data" + x + "Sec");
            PlayerPrefs.DeleteKey(DataControl.ButtonCode + x + "Date");
        }

        //データの数をゼロにする
        DataControl.numOfData = 0;

        //データの数を永久的にゼロにする
        PlayerPrefs.SetInt(DataControl.ButtonCode + "num", DataControl.numOfData);
        PlayerPrefs.Save();

        //デリートコード
        //string deleteCode = DataControl.ButtonCode.Replace("Item", "");
        //PlayerPrefs.SetString("DeleteCode" + deleteCode, deleteCode);
        //PlayerPrefs.Save();

        
    }
    [SerializeField] private GameObject BackToTitle;

    public void ReallyDeleteAllScene()
    {
        //メインシーンに飛ぶ
        this.BackToTitle.SetActive(true);
    }
    public void GoBackTMain()
    {
        SceneManager.LoadScene("MainScene");
    }


    public void closeDeleteAll()
    {
        this.rDeleteAll.SetActive(false);
        this.rDeleteCover.SetActive(false);
    }



    //最新のデータを表示する
    private void ShowNewestData()
    {
        if(DataControl.numOfData < 10){
            this.Onum = 0;
        }
        else{
            this.Onum = DataControl.numOfData - 10;
        }

        int x = 1;

        for(int i = DataControl.numOfData - 1; i >= this.Onum; i--){

            //使用するだけのリストボタンを表示にする
            this.ListofData.transform.GetChild(x).gameObject.SetActive(true);

            //スコアを取得する
            string showtext = 
            DataControl.hourData[i].ToString("00") + ":" +
            DataControl.minData[i].ToString("00") + ":" +
            DataControl.secData[i].ToString("00.00");

            //スコアの表示
            this.ListofData.transform.GetChild(x).GetComponentInChildren<Text>().text = showtext;
            //偏差値の表示
            this.ListofData.transform.GetChild(x).GetChild(1).GetComponent<Text>().text = "偏差値:  " + this.zscore[i].ToString("f1");
            //日付の表示
            this.ListofData.transform.GetChild(x).GetChild(2).GetComponent<Text>().text = DataControl.DateATime[i];

            x++;
        }
    }

    //最新データ表示パネルのボタンの非表示
    private void DisableeachButton()
    {
        for(int i = 1; i <= 10; i++){
            this.ListofData.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void GetZScore()
    {   
        //とりあえずリストのインスタント化
        this.zscore = new List<float>();

        //標準偏差の二乗
        float Std = 0.0f;

        //時間を数値化したもののリスト化
        List<float> time = new List<float>();
        List<float> timeStdSqr = new List<float>();
        
        //時間を数値化したものを取得する
        for(int i = 0; i < DataControl.hourData.Count; i++){
            float realTime = 
            (float)DataControl.hourData[i] +
            (float)DataControl.minData[i]/60 +
            DataControl.secData[i]/3600;

            time.Add(realTime);
        }

        //時間の標準化の二乗を取得する
        for(int i = 0; i < time.Count; i++){
            float stdSqr = 
            Mathf.Pow(time[i] - time.Average(), 2.0f);

            timeStdSqr.Add(stdSqr);
        }

        //標準偏差を取得する
        Std = Mathf.Pow(timeStdSqr.Sum()/timeStdSqr.Count, 0.5f);

        //偏差値を取得する
        for(int i = 0; i < time.Count; i++){
            float hensachi = 
            (time[i] - time.Average())/Std;

        //偏差値をリストに追加する
            if(!PlayerPrefs.HasKey("hensachich")){
            this.zscore.Add((hensachi*10) + 50);
            }else{
            this.zscore.Add((hensachi*-10) + 50);
            }
        }

        //スコアの平均だけ取得する
        int Ahour = (int)time.Average();
        int Amin = -1;
        float Asec = 0.0f;

        this.average = time.Average();
        for(int i = 0; i < (int)this.average; i++){
            this.average--;
        }
        
        while(this.average > 0.0f){
            this.average -= 1.0f/60.0f;
            Amin++;
        }
        this.average += 1.0f/60.0f;

        Asec = this.average*3600.0f;

        //平均を取得する
        this.AvValuse = 
        Ahour.ToString("00") +":" +
        Amin.ToString("00") +":" +
        Asec.ToString("00.00");


    }
    




    private bool hensachiCh;

    //偏差値反転ボタンを押したら
    public void hensachichange()
    {
        

        if(!PlayerPrefs.HasKey("hensachich")){
            PlayerPrefs.SetInt("hensachich", 1);
        }else{
            PlayerPrefs.DeleteKey("hensachich");
        }
        PlayerPrefs.Save();

        //データシーンをリロードする
        SceneManager.LoadScene("DataScene");
    }



}
