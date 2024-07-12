using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;            
using UnityEngine;

public class AssetHandler : MonoBehaviour {

    AssetBundle assetBundle;
    AssetBundleManifest manifest;
    Player player;
    string basePath;
    int depCount;
	
    public void SetupLoader(Player player){
        
        basePath = Application.persistentDataPath + "/dir/" + caricaIdStoria() + "/" + getOS() + "/";
        
        assetBundle = AssetBundle.LoadFromFile(basePath + getOS());
        this.player = player;
        manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        depCount = 0;
    }

    public void UnsetLoader(){
        basePath = null;
        assetBundle.Unload(true);
        assetBundle = null;
        manifest = null;
    }


    public void loadAsset(string asset, string type){

        string assetPath = type + "/" + asset.ToLower();

       //Debug.Log(assetPath);

        if (File.Exists(basePath + assetPath))
        {
            //Debug.Log("File trovato." + basePath + assetPath);
            //carico i file che mi occorrono per il bundle principale
            string[] dependencies = manifest.GetAllDependencies(assetPath);

            //Debug.Log("Numero dipendenze per scena " + numeroScena + ": " + dependencies.Length);

            IEnumerator sha = loadShared(dependencies, asset, type);
            //Debug.Log(sha.Current != null);
            StartCoroutine(sha);
        }

    }

    IEnumerator loadShared(string[] dependencies, string asset, string type)
    {
        //Debug.Log("LOADSHARED::Numero dipendenze per scena " + numeroScena + ": " + dependencies.Length);

        yield return null;//[*]Può essere problematico[*]

        foreach (string depe in dependencies)
        {
            IEnumerator depen = sharedRequest(depe);
            StartCoroutine(depen);

        }
       //Debug.Log(asset + " Dependencies: " + dependencies.Length);
        yield return new WaitUntil(() => depCount == dependencies.Length);

        depCount = 0;

        IEnumerator num = loadObject(asset, type);
        StartCoroutine(num);
    }

    IEnumerator sharedRequest(string depe)
    {


        if (!checkDependencies(depe))
        {
            AsyncOperation asyncOperation1 = AssetBundle.LoadFromFileAsync(basePath + depe);

            yield return new WaitUntil(() => asyncOperation1.isDone);


            AssetBundle bundleshared = (asyncOperation1 as AssetBundleCreateRequest).assetBundle;
            yield return bundleshared;

            AsyncOperation asyncOperation2 = bundleshared.LoadAllAssetsAsync<GameObject>();
            yield return new WaitUntil(() => asyncOperation2.isDone);
           //Debug.Log("!!!!!! " + depe);
            //depCount++;
        }
        depCount++;
       //Debug.Log("Aggiunto: " + depe + " - Conto dipendenze caricate: " + depCount);
    }

    IEnumerator loadObject(string asset, string type)
    {
        string assetPath = type + "/" + asset.ToLower();

        if (File.Exists(basePath + assetPath))
        {
           //Debug.Log(asset + "Exists.");
            AssetBundleCreateRequest bundleLoadRequest = null;
            AssetBundleRequest assetLoadRequest = null;//Idem

            bundleLoadRequest = AssetBundle.LoadFromFileAsync(basePath + assetPath);
            yield return bundleLoadRequest;
           //Debug.Log(asset + " Request done.");
            var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
            if (myLoadedAssetBundle == null)
            {
               //Debug.Log("Failed to load AssetBundle!");
                yield break;
            }

            if (!type.Equals("background")) {
                assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>(asset);
                yield return assetLoadRequest;

                GameObject cube = assetLoadRequest.asset as GameObject;

                //Chiedi a Player dove caspito metterlo
                //Il player intanto lo metterà nel dizionario appropriato
                GameObject go = Instantiate(cube);
               //Debug.Log("Instantiating: " + go.name);
                go.name = go.name.Replace("(Clone)", "");

                if(type.Equals("controller")){
                    player.ReturnController(go);
                } else if(type.Equals("sceneskip")){
                    player.ReturnSceneSkip(go);
                    go.name = "Content";
                } else {
                    player.ReturnEntity(go);
                }
                    

                //Debug.Log("instanzio");

            } else {
                assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<Sprite>(asset);
                yield return assetLoadRequest;

                Sprite cube = assetLoadRequest.asset as Sprite;

                //Passa al player il background
                player.ReturnBackground(cube);
                //E finisce qui
            }

            //Pulisco dalla cache il bundle scaricato.
            myLoadedAssetBundle.Unload(false);
            Resources.UnloadUnusedAssets();
            assetLoadRequest = null;
            bundleLoadRequest = null;
            System.GC.Collect();
            Caching.ClearCache();
        }
    }


    public static string getOS(){
        //Controlla ambiente
        string pathName = "";
        if (SystemInfo.operatingSystem.Contains("iOS"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Android"))
        {
            pathName = "Android";
        }
        else if (SystemInfo.operatingSystem.Contains("Mac"))
        {
            pathName = "Ios";
        }
        else if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            pathName = "Windows";
        }
        else
        {
            pathName = "Linux";
        }

        return pathName;
    }

    public static string caricaIdStoria()
    {
        string storyId;
            //carico la directory dei salvataggi
            string destinationUtente = Application.persistentDataPath + "/myIdStoria.dat";
            FileStream fileUtente = null;

            //se i file esistono li apre
            if (File.Exists(destinationUtente))
                fileUtente = File.OpenRead(destinationUtente);
            else
            {
                return null;
            }

            //leggo i file locali
            BinaryFormatter bf = new BinaryFormatter();

            storyId = (string)bf.Deserialize(fileUtente);

            fileUtente.Close();

        return storyId;
    }

    public bool checkDependencies(string s)
    {
        IEnumerable b = AssetBundle.GetAllLoadedAssetBundles();
        foreach (AssetBundle a in b)
        {
            if (a.name.Equals(s))
                return true;
        }
        return false;
    }

    public string ReadJSON(int sceneNum)
    {
        string difficulty = "";
        string path = basePath+"SceneJSON/Scena" + sceneNum + ".json";
        //[*]Si può modificare per velocizzare produzione scene.
        if(sceneNum > 0 && sceneNum <= player.GetAllScenes()){
            difficulty = "ES";//Controllare le opzioni audio per definire la difficoltà.
            path = basePath+"SceneJSON/Scena" + sceneNum + difficulty + ".json";
        
            if(!File.Exists(path)){
                difficulty = player.opzioniAudio.option.tipoLettura == 0? "S" : "E";
                path = basePath+"SceneJSON/Scena" + sceneNum + difficulty + ".json";
            }
        } else {
            path = basePath+"SceneJSON/Scena" + (sceneNum == 0 ? "0" : "Final") + ".json";
        }

        Debug.Log(path);
        //[*]Fino a qui


        //Read the text from directly from the json file
        StreamReader reader = new StreamReader(path);

        string json = reader.ReadToEnd();
        reader.Close();
        Debug.Log("Finito di leggere:\n" + json);
        return json;
    }

    public SceneProductionInterface ReleaseScene(int sceneNum)
    {
        string json = ReadJSON(sceneNum);

        return JsonUtility.FromJson<SceneProductionInterface>(json);
    }

}
