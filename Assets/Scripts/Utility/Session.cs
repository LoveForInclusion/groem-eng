using UnityEngine;
using System.Collections;

public class Session : MonoBehaviour
{

    public bool sessionLoaded = false;
    private static bool createdSession = false;
	public static bool extraction = false;
	private static string idStoryToUnzip;
	private static string fileName;

    void Awake()
    {

        if (!createdSession)
        {
            DontDestroyOnLoad(this.gameObject);
            createdSession = true;
            Debug.Log("Sessione attivata");
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

	public static void setupExtraction(string id, string filename){
		idStoryToUnzip = id;
		fileName = filename;
		extraction = true;
	}

	public static string zipPath(){
		return (Application.persistentDataPath + "/" + fileName);
	}

	public static string dirPath(){
		return (Application.persistentDataPath + "/dir/" + idStoryToUnzip);
	}

	public static void resetExtraction(){
		extraction = false;
		idStoryToUnzip = null;
		fileName = null;
	}

	public void resetSession(){
		sessionLoaded = false;
		createdSession = false;
	}

}
