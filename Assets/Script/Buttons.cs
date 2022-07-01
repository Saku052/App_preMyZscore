using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    //データセットの名前を表示する
    [SerializeField] private GameObject DataName;

    //Inputフィールド関連の取得
    [SerializeField] private GameObject InputName;
    [SerializeField] private GameObject BackGround;
    private InputField inputField;



    private void Start() 
    {
        //InputFieldコンポーネントの取得
        this.inputField = InputName.transform.GetChild(1).gameObject.GetComponent<InputField>();

        //ポップアップ関連の非表示
        InputName.SetActive(false);
        BackGround.SetActive(false);

        //選択したデータセットの名前を表示する
        DisplayName();
    }


    //ボタンでメインシーンに戻る
   public void GoToMain()
   {
       SceneManager.LoadScene("MainScene");
   }
   //インプットフィールドの表示
   public void showInput()
   {
       InputName.SetActive(true);
       BackGround.SetActive(true);
   }
   //インプットフィールドの非表示
   public void closeInput()
   {
       InputName.SetActive(false);
       BackGround.SetActive(false);
   }
   //新しく入力された名前を保存して表示
   public void saveName()
   {
       //現在のデータセットのタイトルの変更
       this.DataName.GetComponent<Text>().text = this.inputField.text;
       //タイトルの永久保存
       PlayerPrefs.SetString(DataControl.ButtonCode + "title", this.inputField.text);
       PlayerPrefs.Save();
       Debug.Log(DataControl.ButtonCode);
   }






    //選択したデータセットの名前を表示する
   private void DisplayName()
   {
        this.DataName.GetComponent<Text>().text = DataControl.nameButton.Replace("\n", "");
   }
}
