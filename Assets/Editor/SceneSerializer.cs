using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using Unity.EditorCoroutines.Editor;

public class SceneSerializer : MonoBehaviour {

    public SceneSerializer(){

    }

    #if(UNITY_EDITOR)
    [MenuItem("Tadà/Serializza Scena #&s")]
    #endif
    public static void SerializeScene()
    {
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        timer.Start();



        Debug.Log("Serializzo la scena...");
        GameObject go = GameObject.Find("Main Camera");
        string sceneName = go.GetComponent<SceneProduction>().sceneName;
        GameObject background = go.GetComponent<SceneProduction>().background;
        GameObject[] entities = go.GetComponent<SceneProduction>().entities;
        GameObject[] waypoints = go.GetComponent<SceneProduction>().waypoints;


        SceneProductionInterface scene = new SceneProductionInterface();

        scene.sceneName = sceneName;

        Debug.Log("SERIALIZZO BACKGROUND.");

        if (background != null)
        {
            scene.background = background.GetComponent<SpriteRenderer>().sprite.name;
            Debug.Log("Serializzato background : " + background.GetComponent<SpriteRenderer>().sprite.name);
        }

        scene.camera = new CameraInterface();
        scene.camera.startPoint = Camera.main.transform.position;
        scene.camera.size = Camera.main.GetComponent<Camera>().orthographicSize;

        scene.entities = new EntityInterface[entities.Length];

        Debug.Log("SERIALIZZO LE ENTITÁ.");

        for (int i = 0; i < entities.Length; i++)
        {

            EntityInterface ent = new EntityInterface();
            ent.name = entities[i].name;
            if (entities[i].GetComponent<Entity>() is Actor){
                ent.type = 0;
                ent.idleState = 0;
                ent.stateLayer = entities[i].GetComponent<Actor>().lastState;
            }
            else{
                ent.type = 1;
                ent.idleState = entities[i].GetComponent<Prop>().idleState;
            }

            if(entities[i].GetComponent<SpriteRenderer>() != null)
                ent.renderLayer = entities[i].GetComponent<SpriteRenderer>().sortingOrder;
            ent.start = entities[i].transform.position;
            ent.scale = entities[i].transform.localScale;
            if(entities[i].GetComponent<SpriteRenderer>() != null)
                ent.flip = entities[i].GetComponent<SpriteRenderer>().flipX;
            else
                ent.flip = entities[i].transform.GetChild(0).localScale.x == -1;
            
            scene.entities[i] = ent;

            Debug.Log("Serializzata entità : " + ent.name);

        }

        scene.waypoints = new WaypointInterface[waypoints.Length];

        Debug.Log("SERIALIZZO I WAYPOINTS.");


        for (int i = 0; i < waypoints.Length; i++)
        {

            WaypointInterface wp = new WaypointInterface();

            wp.name = waypoints[i].name;

            wp.position = waypoints[i].transform.position;
            wp.scale = waypoints[i].transform.localScale;

            scene.waypoints[i] = wp;

            Debug.Log("Serializzato waypoints : " + wp.name);

        }

        if(UnityEditor.EditorApplication.isPlaying)
            ScreenCapture.CaptureScreenshot("Assets/Resources/SceneJSON/" + sceneName + ".png");

        string json = JsonUtility.ToJson(scene, true);
        Debug.Log(json);

        Debug.Log("SCRIVO SU FILE JSON.");

        StreamWriter writer = new StreamWriter("Assets/Resources/SceneJSON/" + scene.sceneName + ".json", false);
        writer.WriteLine(json);
        writer.Close();
        //EditorApplication.isPlaying = false;

        timer.Stop();

        AssetDatabase.ImportAsset("Assets/Resources/SceneJSON/" + scene.sceneName + ".json");
        
        if(UnityEditor.EditorApplication.isPlaying)
            AssetDatabase.ImportAsset("Assets/Resources/SceneJSON/" + scene.sceneName + ".png");

        float t = (float)timer.ElapsedMilliseconds / 1000f;
        Debug.Log("SERIALIZZATO! Tempo impiegato: " + t.ToString("F3"));

        // if(go.GetComponent<SceneProduction>().sceneController != null){
        //     PrefabUtility.ApplyPrefabInstance(GameObject.Find("Canvas").transform.GetChild(0).gameObject, InteractionMode.AutomatedAction);
        //     Debug.Log("Prefab Applicato!");
        // }
        // else{
        //     Debug.LogError("ERROR: Aggiungere lo SceneController in " + sceneName);
        // }

        PrefabUtility.ApplyPrefabInstance(GameObject.Find("Canvas").transform.GetChild(0).gameObject, InteractionMode.AutomatedAction);
        Debug.Log("Prefab Applicato!");
    }
 

}

public class SceneSerializerX : EditorWindow
    {
        public string editorWindowText1 = "From Scene";
        public string editorWindowText = "To Scene";
        static int inputTextStart = 1;
        static int inputText = 1;
        static bool isElb = false;
        static bool screenOnly = false;
        //bool docked;
        bool repositioned = false;
        SceneSerializer sco;
        static bool skipScreen = false;
        static bool skipScene = false;
        public static GameObject scenesSmp, scenesElb;
        void OnGUI()
        {
            //Debug.Log(this.position);
            EditorGUILayout.Space();
            //EditorGUIUtility.labelWidth = 200;
            inputTextStart = EditorGUILayout.IntField(editorWindowText1, inputTextStart, GUILayout.ExpandWidth(false));

            EditorGUILayout.Space();

            inputText = EditorGUILayout.IntField(editorWindowText, inputText, GUILayout.ExpandWidth(false));
            
            EditorGUILayout.Space();
            
            isElb = EditorGUILayout.Toggle("ELB ", isElb);

            screenOnly= EditorGUILayout.Toggle("Solo Screenshot", screenOnly);
            


            if(isElb)
                EditorGUILayout.HelpBox("So you've chosen to serialize ELB.", MessageType.Info);
            else
                EditorGUILayout.HelpBox("You're about to serialize SMP.", MessageType.Info);

            if(screenOnly)
                EditorGUILayout.HelpBox("You're going screenshot.", MessageType.Info);
            else
                EditorGUILayout.HelpBox("You're going full serial.", MessageType.Info);
            //Debug.Log("Scenes: " + inputText);
            
            skipScreen = EditorGUILayout.Toggle("skipScreen ", skipScreen);
            EditorGUILayout.Space();
            //Prefab Salta Scena
            skipScene = EditorGUILayout.Toggle("Update SaltaScenea ", skipScene);
            scenesElb = EditorGUILayout.ObjectField("Prefab Elb", scenesElb, typeof(GameObject), true) as GameObject;
            scenesSmp = EditorGUILayout.ObjectField("Prefab Smp", scenesSmp, typeof(GameObject), true) as GameObject;

            if(this.docked){
                EditorGUILayout.Space();
                repositioned = false;
            } else {
                GUILayout.FlexibleSpace();
                if(!repositioned){
                    this.position = new Rect(735, 170, 400, 400);
                    repositioned = true;
                }
            }
            
            if (GUILayout.Button("Serialize")){
                //Debug.Log("Scenes2: " + inputText);
                EditorCoroutineUtility.StartCoroutine(serial(inputTextStart, inputText, isElb), this);
            }
 
            if (GUILayout.Button("Cancel"))
                Close();
        }
 
        #if(UNITY_EDITOR)
        [MenuItem("Tadà/Serializza Tutte Le Scene %&s")]
        #endif
        static void CreateProjectCreationWindow()
        {
            EditorWindow window = GetWindow(typeof(SceneSerializerX));
            window.position = new Rect(Screen.width/2, Screen.height/2, 400, 400);
            window.ShowUtility();
        }

        public IEnumerator serial(int startScene, int scenes, bool isElb){
            
            string path = Application.dataPath + "/Scenes/" + (isElb? "Elaborata/ScenaElb" : "TestScene");

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            
            Debug.Log("Scenes: " + scenes);
            for(int i = startScene; i<=scenes; i++){
                EditorSceneManager.OpenScene(path + i + ".unity");
                Debug.Log("CallingScene " + i);
                yield return new WaitUntil(() => EditorSceneManager.GetActiveScene().name.Equals((isElb ? "ScenaElb" : "TestScene") + i));
                Debug.Log("SceneCalled " + i);
                if(!screenOnly){
                    SceneSerializer.SerializeScene();
                    Debug.Log("SceneSerialized " + i);
                }
                
                if(!skipScreen){
                    UnityEditor.EditorApplication.isPlaying = true;
                    //Screenshotto
                    ScreenCapture.CaptureScreenshot("Assets/Resources/SceneJSON/Scena" + i + (isElb ? "E" : "S") + ".png");
                    
                    //Fine
                    yield return new EditorWaitForSeconds(1);
                    UnityEditor.EditorApplication.isPlaying = false;
                    yield return new EditorWaitForSeconds(1);
                }

                

                #if(UNITY_EDITOR)
                //Reimporto
                if(screenOnly)
                    AssetDatabase.ImportAsset("Assets/Resources/SceneJSON/Scena" + i + (isElb ? "E" : "S") + ".json");

                AssetDatabase.ImportAsset("Assets/Resources/SceneJSON/Scena" + i + (isElb ? "E" : "S") + ".png");

                
                #endif

                //Update SkipScene prefabs
                if(skipScene){
                    if(isElb){
                        
                        var spriteJ = Resources.Load<Sprite>("SceneJSON/Scena" + i + "E");
                        scenesElb.transform.GetChild(i-1).GetComponent<Image>().sprite = spriteJ;
                        //PrefabUtility.ApplyObjectOverride(scenesElb, "Assets/Prefab/NewPlayer/SceneSkipPackage/", InteractionMode.AutomatedAction);
                        PrefabUtility.SavePrefabAsset(scenesElb);
                    }
                    else {
                        var spriteJ = Resources.Load<Sprite>("SceneJSON/Scena" + i + "S");
                        scenesSmp.transform.GetChild(i-1).GetComponent<Image>().sprite = spriteJ;
                        //PrefabUtility.ApplyObjectOverride(scenesElb, "Assets/Prefab/NewPlayer/SceneSkipPackage/", InteractionMode.AutomatedAction);
                        PrefabUtility.SavePrefabAsset(scenesSmp);
                    }
                }

                PrefabUtility.ApplyPrefabInstance(GameObject.Find("Canvas").transform.GetChild(0).gameObject, InteractionMode.AutomatedAction);
                Debug.Log("Prefab Applicato!");
            }

            timer.Stop();
            float t = (float)timer.ElapsedMilliseconds / 1000f;
            Debug.Log("SERIALIZZATO! Tempo impiegato: " + t.ToString("F3"));

        }
        
        // public bool IsDocked
        // {
        //     get
        //     {
        //         BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
        //         MethodInfo method = GetType().GetProperty( "docked", flags ).GetGetMethod( true );
        //         return (bool)method.Invoke( this, null );
        //     }
        // }
}