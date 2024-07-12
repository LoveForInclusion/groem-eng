using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalSceneController : SceneController
{
   

   protected override void SetupController(){
       base.SetupController();
       this.transform.Find("Button").GetComponent<Button>().onClick.AddListener(FinalButton);
   }

   public void FinalButton(){
       Debug.Log("Exiting");
       AssetBundle.UnloadAllAssetBundles(false);
       Resources.UnloadUnusedAssets();
       System.GC.Collect();
       Caching.ClearCache();
       SceneManager.LoadScene("ScenaScelta", LoadSceneMode.Single);
   }

}
