using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //Player instance
    private int allScenes = 10;
    private int currentScene;
    private float delay = 0;
    private bool waypointSet;
    private bool controllerReady;
    private bool controllerOverridden;
    private bool bgReady;
    private bool ssReady;
    private int actionsPending;
    private SceneProductionInterface sceneInfo;
    private GameObject waypoint;
    private Dictionary<string, GameObject> actors;
    private Dictionary<string, GameObject> props;
    public Dictionary<string, GameObject> waypoints;//Clear ad ogni scena(se necessario)

    //Controllers
    private AssetHandler handler;
    private SceneController controller;


    //Graphic Components
    [Header("Graphic Components")]
    public GameObject gui;
    public GameObject BG;
    public GameObject loadCanvas;

    //Misc
    [Header("Are you testing?")]
    public bool init = false;
    [HideInInspector]
    public OpzioniAudio opzioniAudio;
    public PlayerGUIControl UIControl;
    private bool skipping = false;


    //Methods
    private void Start()
    {
        opzioniAudio = new OpzioniAudio();
        opzioniAudio.caricaOpzioniAudio();

        /********* MODIFICHE PROVA ******/
        //opzioniAudio.option.tipoLettura = 1;
        /***************************** */

        handler = this.GetComponent<AssetHandler>();
        handler.SetupLoader(this); //Commenting for testing purposes
        waypoint = new GameObject();
        waypoint.tag = "Waypoint";
        actors = new Dictionary<string, GameObject>();
        props = new Dictionary<string, GameObject>();
        waypoints = new Dictionary<string, GameObject>();
        currentScene = 0;
        UIControl.ManageButtons(0, 99, null, null);     
    }

    private void Update(){
        if(!init){
            init = true;
            Camera.main.clearFlags = CameraClearFlags.Nothing;
            Camera.main.cullingMask = 0;
            SceneFetch();
            BuildScene();
            StartCoroutine(PlayScene());
        }
    }

    public void SetAllScenes(int numScenes){
        this.allScenes = numScenes;
    }

    public void Action(EntityAction action, int index){

        if(action.type == 0){
            GameObject entity, wp1;
            if(actors.TryGetValue(action.name, out entity)){
                if(action.action > 0){
                    
                    wp1 = waypoints[action.startWP];
                    
                    if(action.targerWP.Length != 0){
                        GameObject[] wps = new GameObject[action.targerWP.Length];
                        for(int i = 0; i< action.targerWP.Length; i++){
                           wps[i] = waypoints[action.targerWP[i]];
                        }
                        entity.GetComponent<Actor>().Action(action.action, action.speed, action.distance, wp1, wps, action.stateLayer);
                    } else {
                        entity.GetComponent<Actor>().setFlip(action.flip);
                        entity.GetComponent<Actor>().Action(action.action, wp1, action.stateLayer);
                    }
                }            
                else{
                    //Debug.Log(action.name + " - Mi muovo da:" + action.startWP + " verso " + action.targerWP[0] + "?");
                    entity.GetComponent<Actor>().ChangeLayer(action.stateLayer, false);

                    GameObject[] wps = new GameObject[action.targerWP.Length];
                        
                    wp1 = waypoints[action.startWP]; 
                    entity.GetComponent<Actor>().setFlip(action.flip);
                    //Debug.Log(wp1.name);
                    if(action.targerWP.Length != 0){
                        for(int i = 0; i< action.targerWP.Length; i++){
                           wps[i] = waypoints[action.targerWP[i]];
                        }
                        //Debug.Log(wps[0].name);
                        entity.GetComponent<Actor>().Move(action.speed, action.distance, wp1, wps);
                    }
                    else{
                        entity.GetComponent<Actor>().RemoveTarget();
                        entity.transform.localScale = wp1.transform.localScale;
                        if(index == 0)
                            entity.GetComponent<Actor>().SetStartingPoint(true, wp1.transform.position, action.distance, entity.GetComponent<SpriteRenderer>() != null ? entity.GetComponent<SpriteRenderer>().sortingOrder : 0);
                        else
                            entity.GetComponent<Actor>().SetStartingPoint(false, wp1.transform.position, action.distance, entity.GetComponent<SpriteRenderer>() != null ? entity.GetComponent<SpriteRenderer>().sortingOrder : 0);
                    }

                }
                actionsPending++;
            }
        }
        else {
            GameObject entity;
            if(props.TryGetValue(action.name, out entity)){
                entity.GetComponent<Prop>().ChangeState(action.action);
            }
        }
    }

    public void ActionReturn(){
        actionsPending--;
    }

    IEnumerator PlayScene(){

        yield return null;

        //[1]Preparo Scena
        yield return new WaitUntil(() => ((actors.Count + props.Count) == sceneInfo.entities.Length) && waypointSet && controller != null && bgReady && ssReady);

        if(currentScene == 0)
            loadCanvas.SetActive(false);
        
        //Aggiustamento UI
        UIControl.ManageButtons(currentScene, allScenes, opzioniAudio, controller);
        UIControl.donTouch.transform.SetAsLastSibling();
        UIControl.audioButton.transform.SetAsLastSibling();
        UIControl.donTouch.SetActive(true);
        //Fine Aggiustamento UI

        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.cullingMask = LayerMask.NameToLayer("Everything");

        //[2]Play Scena
        if(opzioniAudio.option.standard){
            for(int i = 0; i< controller.figures.Length; i++){
                controllerReady = false;
                controllerOverridden = false;
                controller.PlayFigure(i);
                yield return new WaitUntil(() => controllerOverridden || controllerReady);//Waiting for controller to be ready by either ending the audio or being overridden
                if(!controllerOverridden){
                    //Debug.Log("Overridden!");
                    if(this.delay != 0)
                        yield return new WaitForSeconds(this.delay);
                    else
                        yield return new WaitForSeconds(0);
                }
                    
            }
        } else if(opzioniAudio.option.personalizzata){
           if(currentScene > 0 && currentScene <= allScenes){
               controllerReady = false;
               controller.PlayFigure(-1);
               yield return new WaitUntil(() => controllerReady);
           }
        }

        UIControl.donTouch.SetActive(false);

        //[3]Fine Loop Play - Preparo Cambio Scena
        //->Aspetto input per Next o Previous Scene

    }


    public void SceneFetch(){
        //Debug.Log("Scena"+currentScene);
        sceneInfo = handler.ReleaseScene(currentScene);

    }

    public void BuildScene(){//Verrà convertito in un IENUM

        if(currentScene == 0){//Se è scena zero cerca sceneskip
            Debug.Log("Cerco sceneSkip");
            ssReady = false;
            handler.loadAsset("Scenes " + (opzioniAudio.option.tipoLettura == 1 ? "Elb" : "Smp"), "sceneskip");
        }
        //Controlla il BG se è diverso da quello corrente ricaricalo. OPPURE È NULLO
        if(BG.GetComponent<SpriteRenderer>().sprite == null){
            handler.loadAsset(sceneInfo.background, "background");
        } else {
            if(!BG.GetComponent<SpriteRenderer>().sprite.name.Equals(sceneInfo.background)){
                bgReady = false;
                handler.loadAsset(sceneInfo.background, "background");
            }
        }

        Camera.main.transform.position = new Vector3(sceneInfo.camera.startPoint.x, sceneInfo.camera.startPoint.y, Camera.main.transform.position.z);
        Camera.main.GetComponent<Camera>().orthographicSize = sceneInfo.camera.size;

        //CleanScene(); //Tolgo la roba che non serve //Dovrebbe stare in pulizia scena.

        //Riprovo caricamento entità
        for (int i = 0; i < sceneInfo.entities.Length; i++){
            switch(sceneInfo.entities[i].type){
                case 0:
                    if(!actors.ContainsKey(sceneInfo.entities[i].name))
                        handler.loadAsset(sceneInfo.entities[i].name, "actor");
                    else{
                        actors[sceneInfo.entities[i].name].GetComponent<Actor>().RemoveTarget();
                        actors[sceneInfo.entities[i].name].GetComponent<Actor>().SetStartingPoint(false, sceneInfo.entities[i].start, 0, sceneInfo.entities[i].renderLayer);
                        actors[sceneInfo.entities[i].name].GetComponent<Actor>().setFlip(sceneInfo.entities[i].flip);
                        actors[sceneInfo.entities[i].name].transform.localScale = sceneInfo.entities[i].scale;
                        Debug.Log(sceneInfo.entities[i].stateLayer);
                        actors[sceneInfo.entities[i].name].GetComponent<Actor>().ChangeLayer(sceneInfo.entities[i].stateLayer, false);                        
                    }
                    break;
                case 1:
                    if(!props.ContainsKey(sceneInfo.entities[i].name))
                        handler.loadAsset(sceneInfo.entities[i].name, "prop");
                    else{
                        props[sceneInfo.entities[i].name].GetComponent<Prop>().SetStartingPoint(true, sceneInfo.entities[sceneInfo.findEntityIndex(sceneInfo.entities[i].name, 1)].start, 0,sceneInfo.entities[sceneInfo.findEntityIndex(sceneInfo.entities[i].name, 1)].renderLayer);
                        props[sceneInfo.entities[i].name].transform.localScale = sceneInfo.entities[i].scale;
                        props[sceneInfo.entities[i].name].GetComponent<Prop>().SetStartingState(sceneInfo.entities[i].idleState);
                    }
                    break;
            }
        }

        //Definisco i waypoint
        for (int i = 0; i < sceneInfo.waypoints.Length; i++){
            GameObject go = Instantiate(waypoint, sceneInfo.waypoints[i].position, new Quaternion());
            go.name = sceneInfo.waypoints[i].name;
            go.transform.localScale = sceneInfo.waypoints[i].scale;
            waypoints.Add(go.name, go);
        }
        waypointSet = true;

        //foreach(KeyValuePair<string, GameObject> wp in waypoints)
        //Debug.Log(wp.Key + "!");

        //Carico Controller
        //Debug.Log("Starto sto controller?");
        string difficulty = opzioniAudio.option.tipoLettura == 0? "S" : "E";
        
        if(currentScene > 0 && currentScene <= allScenes)
            handler.loadAsset("Scena" + currentScene + difficulty, "controller");
        else
            handler.loadAsset("Scena" + (currentScene == 0 ? "0" : "Final"), "controller");
        
    }

    //Quando ha finito di caricare il background torna qui
    public void ReturnBackground(Sprite background){
        Debug.Log("Returning Background");
        BG.GetComponent<SpriteRenderer>().sprite = background;
        bgReady = true;
        }

    //Torna dal caricamento di entity
    public void ReturnEntity(GameObject entity){

        if(entity.GetComponent<Actor>() is Actor){
            Debug.Log("Returning actor");
            entity.GetComponent<Actor>().SetStartingPoint(true, sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 0)].start, 0, sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 0)].renderLayer);
            entity.GetComponent<Actor>().ChangeLayer(sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 0)].stateLayer, false);  
            entity.GetComponent<Actor>().setFlip(sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 0)].flip);
            entity.transform.localScale = sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 0)].scale;
            actors.Add(entity.name, entity);

        } else {         
            Debug.Log("Returning prop");
            entity.GetComponent<Prop>().SetStartingPoint(true, sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 1)].start, 0,sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 1)].renderLayer);
            entity.GetComponent<Prop>().SetStartingState(sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 1)].idleState);
            if(sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 1)].scale != Vector2.one)
                entity.transform.localScale = sceneInfo.entities[sceneInfo.findEntityIndex(entity.name, 1)].scale;
            props.Add(entity.name, entity);
        }
    }

    //Torna dal caricamento SceneController
    public void ReturnController(GameObject entity){
        Debug.Log("Returning controller");
        this.controller = entity.GetComponent<SceneController>();
        this.controller.transform.SetParent(this.gui.transform);
        this.controller.transform.position = Vector3.zero;
        this.controller.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        this.controller.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        this.controller.transform.localScale = new Vector3(1, 1, 1);
        this.controller.transform.rotation = new Quaternion();
        if(currentScene == 0){//Opt for a delay in Scene 0 to define how much should i wait between tabs
            if((this.controller as Scene0Controller).delay != null || (this.controller as Scene0Controller).delay != 0){
                Debug.Log("Delayed!");
                this.delay = (this.controller as Scene0Controller).delay;
            }
        }
    }

    public void ReturnSceneSkip(GameObject entity){
        Debug.Log("Returning Scene Skip");
        entity.transform.SetParent(UIControl.sceneSkip.transform.Find("Scroll View").Find("Viewport"));
        entity.transform.position = Vector3.zero;
        entity.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        entity.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        entity.transform.localScale = new Vector3(1, 1, 1);
        entity.transform.rotation = new Quaternion();
        entity.transform.parent.parent.GetComponent<ScrollRect>().content = entity.transform.GetComponent<RectTransform>();
        ssReady = true;
    }

    //Per pulire scena[3]
    public void CleanScene(int scene){//Wrapper           
            SceneFetch();
            controller = null;

            //Cancello GUI ELEMENTS
            Debug.Log("Esiste il " + this.gui.transform.Find("Scena"+scene));
            Debug.Log("Esiste il " + UIControl.sceneSkip.transform.Find("Scroll View").Find("Viewport").Find("Content"));
            Debug.Log("Conteggio children: "+ UIControl.sceneSkip.transform.Find("Scroll View").Find("Viewport").childCount);
            Debug.Log("Nome child: "+ UIControl.sceneSkip.transform.Find("Scroll View").Find("Viewport").GetChild(0).name);

            string difficulty = "";
            if(scene > 0 && scene <= allScenes){
                difficulty = opzioniAudio.option.tipoLettura == 0? "S" : "E";
            }
            Debug.Log("Scena" + scene + difficulty);

            Destroy(this.gui.transform.Find("Scena" + scene + difficulty).gameObject);

            if(scene==0){
                Destroy(UIControl.sceneSkip.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject);
            }
            //FINE

            Debug.Log("Cancellata GUI");

            cleanActors();
            Debug.Log("Cancellati Actors");
            cleanProps();
            Debug.Log("Cancellate Props");
            cleanWaypoints();
            Debug.Log("Cancellati WP");

            //Ho usato il salta scena?
            if(skipping){
                UIControl.sceneSkip.SetActive(false);
                StartCoroutine(PlayScene());
                skipping = false;                
            }
            AssetBundle.UnloadAllAssetBundles(false);
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            Caching.ClearCache();
    }

    //3 METODI UGUALI PERCHÉ SAREBBE LA STESSA COSA SE LI SCHIAFFO IN UNO SOLO
    public void cleanActors(){
        //Lista per attori da levare di mezzo
        List<string> deadMen = new List<string>();
        //Mi carico quelli inutili in questa scena
        foreach(KeyValuePair<string, GameObject> actor in actors){  
            if(!sceneInfo.findEntity(actor.Key, 0)){
                Destroy(actor.Value);
                deadMen.Add(actor.Key);
            }
        }
        //Li uccido
        deadMen.ForEach(i => actors.Remove(i));
        deadMen.Clear();
        deadMen = null;
    }

    public void cleanProps(){
        //Lista per props da levare di mezzo
        List<string> deadMen = new List<string>();
        //Mi carico quelle inutili in questa scena
        foreach (KeyValuePair<string, GameObject> prop in props)
        {
            if (!sceneInfo.findEntity(prop.Key, 1))
            {
                Destroy(prop.Value);
                deadMen.Add(prop.Key);
            }
        }
        //Le uccido
        deadMen.ForEach(i => props.Remove(i));
        deadMen.Clear();
        deadMen = null;
    }

    public void cleanWaypoints()
    {
        //Waypoints go bye bye
        foreach (GameObject go in waypoints.Values)
            Destroy(go);
        
        waypoints.Clear();
    }
    //FINE

    public void ReadyController(){
        //Debug.Log("Ready");
        controllerReady = true;
    }

    public void OverrideController(){
        //Debug.Log("Over");
        controllerOverridden = true;
    }


    public void NextScene(){
        
        UIControl.donTouch.transform.SetAsLastSibling();
        UIControl.donTouch.SetActive(true);
        Camera.main.clearFlags = CameraClearFlags.Nothing;
        Camera.main.cullingMask = 0;
        currentScene++;
        CleanScene(currentScene-1);
        BuildScene();
        StartCoroutine(PlayScene());
        Debug.Log(currentScene + " - " + allScenes);
        if(currentScene > 0){
            gui.transform.Find("ButtonInfo").gameObject.SetActive(false);
        }
    }

    public void PreviousScene(){
        
        UIControl.donTouch.transform.SetAsLastSibling();
        UIControl.donTouch.SetActive(true);
        Camera.main.clearFlags = CameraClearFlags.Nothing;
        Camera.main.cullingMask = 0;
        currentScene--;
        CleanScene(currentScene+1);
        BuildScene();
        StartCoroutine(PlayScene());
        if(currentScene == 0){
            gui.transform.Find("ButtonInfo").gameObject.SetActive(true);
        }
    }

    public void SetCurrentScene(int scene){
        this.currentScene = scene;
        skipping = true;
    }

    public int GetCurrentScene(){
        return currentScene;
    }

    public int GetAllScenes(){
        return allScenes;
    }

    public SceneController GetController(){
        return this.controller;
    }

    public void CleanBundle(){
        AssetBundle.UnloadAllAssetBundles(true);
    }

}