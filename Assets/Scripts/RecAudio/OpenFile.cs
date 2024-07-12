using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OpenFile : MonoBehaviour {
  /*  string path;
    string extension;
    public GameObject musicAnalysis;
    string songName;
    float length;
    AudioSource song;

    // Use this for initialization
    void Start () {
        song =  musicAnalysis.GetComponent<AudioSource>();
		FileSelect ();
    }

    // Update is called once per frame
    void Update () {
        if(song.isPlaying != true){
            song.Play();
        }
    }

    public void FileSelect(){
        //Open windows Exploer 
        path = EditorUtility.OpenFilePanel("Select a Song","","");

        print(path);

        //Take the end of the the path and sasve it to another string
        extension = path.Substring(path.IndexOf('.') + 1);

        print (extension);
        //Check if the user has select the correct file
        if(extension == "mp3" || extension == "wav" || extension == "ogg"){
            //if correct file process file
            print ("You have selected the correct file type congrats");

            LoadSong();
            print ("Song Name: " + songName);
            print ("Song Length: " + length);
        }
        //if the user selects the wrong file type
        else{
            //pop up box that tells the user that they have selected the wrong file
            EditorUtility.DisplayDialog("Error","Incorrect File Type Please select another","Ok");
            ////Open windows Exploer 
            path = EditorUtility.OpenFilePanel("Select a Song","","");
        }
    }

    void LoadSong(){
        WWW www = new WWW("file://" + path);
		song.clip = www.GetAudioClip();
		songName =  www.GetAudioClip().name;
		length = www.GetAudioClip().length;

        while(!www.isDone){
            print ("Processing File" + path);
        }

        if(www.isDone == true){
            print ("Song has been processed");
        }
    }*/
}