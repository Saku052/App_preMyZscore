using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAdd : MonoBehaviour
{
//データ関連のパネル取得
[SerializeField] private GameObject AddList;
[SerializeField] private GameObject DataList;
[SerializeField] private GameObject DataBackGround;
private Transform Addlist;
private Transform Datalist;

//データセットの数
private int numOfDataSet = 1;



private void Start() 
{
    //データリストを管理するGameObjectを取得
    this.Addlist = this.AddList.transform.GetChild(0).GetChild(0);
    this.Datalist = this.DataList.transform.GetChild(0).GetChild(0).GetChild(0);

    //オープンにするデータセットの数を取得する
    //GetNumOfDataSet();
    
    //追加してないデータを非表示にする
    //DisableList();

    //AddListを非表示にする
    this.AddList.SetActive(false);
    this.DataList.SetActive(false);
    this.DataBackGround.SetActive(false);
}


//オープンにするデータセットの数を取得する
private void GetNumOfDataSet()
{
    if(PlayerPrefs.HasKey("numberOfData")){
        this.numOfDataSet = PlayerPrefs.GetInt("numberOfData");
    }
}

//追加してないデータを非表示にする
private void DisableList()
{
    for(int i = this.numOfDataSet; i < 20; i++){
        this.Addlist.GetChild(i + 1).gameObject.SetActive(false);
        this.Datalist.GetChild(i + 1).gameObject.SetActive(false);
    }

    for(int i = 1; i <=20; i++){
        if(PlayerPrefs.HasKey("DeleteCode" + i)){
            this.Addlist.GetChild(i).gameObject.SetActive(false);
            this.Datalist.GetChild(i).gameObject.SetActive(false);
        }
    }
}



//データセットの追加
public void AddDataSet()
{
    if(PlayerPrefs.HasKey("numberOfData")){
        this.numOfDataSet = PlayerPrefs.GetInt("numberOfData");
    }
    this.numOfDataSet++;

    //追加した分のデータセットを追加する
    //this.Addlist.GetChild(this.numOfDataSet).gameObject.SetActive(true);
    //this.Datalist.GetChild(this.numOfDataSet).gameObject.SetActive(true);

    PlayerPrefs.SetInt("numberOfData", this.numOfDataSet);
    PlayerPrefs.Save();
}

//データ関連パネルを表示・非表示にするボタン
public void OpenDataAdd()
{
    this.AddList.SetActive(true);
    this.DataBackGround.SetActive(true);
}
public void CloseDataAdd()
{
    this.AddList.SetActive(false);
    this.DataBackGround.SetActive(false);
}
public void OpenDataList()
{
    this.DataList.SetActive(true);
}
public void CloseDataList()
{
    this.DataList.SetActive(false);
}

}
