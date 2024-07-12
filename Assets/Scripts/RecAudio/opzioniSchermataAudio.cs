using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class opzioniSchermataAudio : MonoBehaviour {

	public GameObject canvasOption;


	public GameObject textMod;
	public GameObject textPers;
	public GameObject textRip;

	public Text ripetizioni;
	public Toggle checkMuto;
	public Toggle checkStandard;
	public Toggle checkTasti;
	public Toggle checkPersonalizza;
	public Toggle checkAdvanced;

	public Button visualizzato;
	public Button profilo1;
	public Button profilo2;
	public Button profilo3;

	public Dropdown profil;
	public Text primoElement;

	public GameObject recordCanvas;

	public GameObject haveSaved;

	OpzioniAudio op;

	float timer;

	bool changed;
    
	void Update(){

		if (changed != checkAdvanced.isOn)
		{
			modificaListaProfili();
			changed = !changed;
		}
		
		if (haveSaved.activeSelf) {
			if (timer <= 0) {
				haveSaved.SetActive (false);
				timer = 1.5f;
				this.transform.gameObject.SetActive(false);
				//if (SceneManager.GetActiveScene().name.Equals("ScenaLettura"))
				//{
				//	//this.transform.gameObject.SetActive(false);
				//	GameObject.Find("Canvas(Clone)").transform.GetComponent<RegistaStoria>().caricaSceltaMenu();
				//}
			}
			else
				timer -= Time.deltaTime;
		}
	}

	public void loadScegli(){

		SceneManager.LoadScene ("ScenaScelta",LoadSceneMode.Single);

	}

	void Start(){

		timer = 1.5f;
		loadAndSetOption();

	}

	public void loadAndSetOption(){
		
		op = new OpzioniAudio();
        op.caricaOpzioniAudio();
		if (op.isloaded)
        {

            ripetizioni.text = op.option.ripetizioni + "";

            if (op.option.muto)
                checkMuto.isOn = true;
            else if (op.option.standard)
                checkStandard.isOn = true;
            else if (op.option.personalizzata)
                checkPersonalizza.isOn = true;
            else
                checkTasti.isOn = true;

            if (op.option.tipoLettura == 1)
                checkAdvanced.isOn = true;

			changed = checkAdvanced.isOn;

            visualizzato.name = op.option.profileSelected + "";
            modificaListaProfili();

        }
        else
        {

            ripetizioni.text = "5";
            checkStandard.isOn = true;
            checkMuto.isOn = false;
            checkTasti.isOn = false;
            checkAdvanced.isOn = false;
            checkPersonalizza.isOn = false;
            visualizzato.name = "0";
        }

		checkAdvanced.interactable = false;
		checkAdvanced.transform.parent.transform.Find("ToggleSemplce").GetComponent<Toggle>().interactable = false;
	}

	//da cambiare
	public void modificaListaProfili(){

		Debug.Log(changed);
		op.caricaOpzioniAudio ();
		if (!checkAdvanced.isOn)
		{
			Debug.Log("easy changed");
			profilo1.GetComponentInChildren<Text>().text = op.option.nomiProfiliModificatiEasy[0];
			profilo2.GetComponentInChildren<Text>().text = op.option.nomiProfiliModificatiEasy[1];
			profilo3.GetComponentInChildren<Text>().text = op.option.nomiProfiliModificatiEasy[2];
			visualizzato.GetComponentInChildren<Text>().text = op.option.nomiProfiliModificatiEasy[op.option.profileSelected];
		}
		else{
			Debug.Log("advanced changed");
			profilo1.GetComponentInChildren<Text>().text = op.option.nomiProfiliModificatiAdvanced[0];
			profilo2.GetComponentInChildren<Text>().text = op.option.nomiProfiliModificatiAdvanced[1];
			profilo3.GetComponentInChildren<Text>().text = op.option.nomiProfiliModificatiAdvanced[2];
			visualizzato.GetComponentInChildren<Text>().text = op.option.nomiProfiliModificatiAdvanced[op.option.profileSelected];
		}



	}

	public void salvaOpzioni(){

		//if (SceneManager.GetActiveScene().name.Equals("ScenaLettura"))
   //     {
   //         Transform panel = GameObject.Find("ErrorCanvas").transform.Find("PanelError").transform;
   //         panel.Find("PanelAlert").Find("Messaggio").GetComponent<Text>().text = "Per cambiare la modalità di lettura verrai riportato alla Home della storia.";
			//panel.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        //}

		op.option.muto = checkMuto.isOn;
		op.option.standard = checkStandard.isOn;
		op.option.tasti = checkTasti.isOn;
		op.option.personalizzata = checkPersonalizza.isOn;

		if (checkAdvanced.isOn)
			op.option.tipoLettura = 1;
		else
			op.option.tipoLettura = 0;
        /*
		l.nomiProfiliModificati[0] = profilo1.GetComponentInChildren<Text>().text;
		l.nomiProfiliModificati[1] = profilo2.GetComponentInChildren<Text>().text;
		l.nomiProfiliModificati[2] = profilo3.GetComponentInChildren<Text>().text;
		*/
		op.option.profileSelected = int.Parse(visualizzato.name);
		op.option.ripetizioni = int.Parse(ripetizioni.text);
        

		op.salvaOpzioniAudio ();

		haveSaved.SetActive (true);
	}

	public void optionCanvasViever(){


		op.caricaOpzioniAudio ();
		modificaListaProfili ();
		recordCanvas.SetActive (false);
		canvasOption.SetActive (true);


	}

	public void registra(){

		this.gameObject.SetActive (false);
		recordCanvas.SetActive (true);
		
	}

	public void infoMod(){
		textMod.SetActive (true);
	}
	public void infoPers(){
		textPers.SetActive (true);
	}
	public void infoRip(){
		textRip.SetActive (true);
	}
	public void chiudiInfo(){
		textMod.SetActive (false);
		textPers.SetActive (false);
		textRip.SetActive (false);
	}

	public void addrip(){
		if(int.Parse(ripetizioni.text)<5)
		ripetizioni.text = int.Parse(ripetizioni.text) + 1+"";
	}

	public void reducerip(){
		if(int.Parse(ripetizioni.text)>1)
			ripetizioni.text = int.Parse(ripetizioni.text) - 1+"";
	}

	public void caricaStartPage(){
	}
}
