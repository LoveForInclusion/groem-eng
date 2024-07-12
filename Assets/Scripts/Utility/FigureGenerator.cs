using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;

public class FigureGenerator : MonoBehaviour {

	/*
	public List<AudioClip> a = new List<AudioClip>();
	public Sprite image;
	public GameObject g;

	void Start () {

		for(int i = 0 ; i < a.Count ; i++){
	
			image = Resources.Load<Sprite> ("symbol/" + RemoveNumbers(a[i].name));
			g.GetComponent<Image> ().sprite = image;
		
			//a = Resources.Load<AudioClip> ("a_ridere");
			g.GetComponent<FigureController> ().audioTest = a[i];

			g.GetComponent<AudioSource> ().clip = null;

			Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/FigureGenerate/"+"Figure_"+ a[i].name +".prefab");
			PrefabUtility.ReplacePrefab(g, prefab, ReplacePrefabOptions.ConnectToPrefab);
		}
	}

	string RemoveNumbers(string text){
		string newText = "";
		string number = "";
		for(int i = 0; i < text.Length; i++){
			if ((text[i] < 48) || (text[i] > 57)){ //is a char
				
				newText += text[i];
				if(i==0){
					newText.ToUpper ();
				}
			}
			else{ //is number
				number += text[i];
			}
		}
		//int num = int.Parse(number);
		if (newText.Substring (newText.Length - 3, 3).Equals ("_qm")) {
			newText = newText.Substring ( 0 , newText.Length-3);
		} else {
			newText = newText.Substring (0, newText.Length-1);
		}
			
		return newText;
	}
*/
}
