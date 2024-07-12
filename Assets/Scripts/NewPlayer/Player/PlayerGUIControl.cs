using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUIControl : MonoBehaviour {
    
    public Player player;

	 [Header("Buttons")]
    public GameObject homeBtn;
    public GameObject nxElb;
    public GameObject nxSmp;
    public GameObject pvElb;
    public GameObject pvSmp;
    public GameObject tabcomE;
    public GameObject tabSkipS;
    public GameObject audioButton;

    [Header("Panels")]
    public GameObject tabcom;
    public GameObject sceneSkip;
    public GameObject donTouch;
    public GameObject audioPanel;

    [Header("Button Sprites")]
    public Sprite tabcomSprite;
    public Sprite sceneSkipSprite;
    [Header("Opzioni Audio")]
    public Toggle pers;
    public Toggle std;
    public Toggle mute;
    public Toggle tasti;
    public Text numRep;
    public Dropdown profileDropDown;
    public GameObject saveToast;

	public void ManageButtons(int currentScene, int allScenes, OpzioniAudio opzioniAudio, SceneController controller){

        if(currentScene > 0 && currentScene <= allScenes){
            audioButton.SetActive(false);
            if(opzioniAudio.option.tipoLettura == 1){
                nxElb.SetActive(true);
                pvElb.SetActive(true);
                tabcomE.SetActive(true);
                tabSkipS.SetActive(false);
            } else {
                if(controller.figures.Length>7){
                    nxElb.SetActive(true);
                    pvElb.SetActive(true);
                    tabcomE.SetActive(true);
                    tabSkipS.SetActive(false);
                    nxSmp.SetActive(false);
                    pvSmp.SetActive(false);
                } else {
                    nxSmp.SetActive(true);
                    pvSmp.SetActive(true);
                    tabSkipS.SetActive(true);
                    tabcomE.SetActive(false);
                    nxElb.SetActive(false);
                    pvElb.SetActive(false);
                    tabSkipS.GetComponent<Button>().onClick.RemoveAllListeners();
                    tabSkipS.GetComponent<Image>().sprite = tabcomSprite;
                    tabSkipS.GetComponent<Button>().onClick.AddListener(ActivateTabcom);
                }
            }

        } else {
            nxElb.SetActive(false);
            nxSmp.SetActive(false);
            pvElb.SetActive(false);
            pvSmp.SetActive(false);
            tabcomE.SetActive(false);
            tabSkipS.SetActive(true);
            audioButton.SetActive(true);
            tabSkipS.GetComponent<Button>().onClick.RemoveAllListeners();
            tabSkipS.GetComponent<Image>().sprite = sceneSkipSprite;
            tabSkipS.GetComponent<Button>().onClick.AddListener(ActivateSceneSkip);
            if(currentScene>allScenes){
                homeBtn.SetActive(false);
                tabSkipS.SetActive(false);
                audioButton.SetActive(false);
            }
        }

    }

    public void ActivateTabcom(){
        //#if(!UNITY_EDITOR)
        tabcom.SetActive(true);
        tabcom.transform.SetAsLastSibling();
        //#else
        Debug.Log("Sono un tabcom");
        //#endif
    }

    public void ActivateSceneSkip(){
        //#if(!UNITY_EDITOR)
        sceneSkip.SetActive(true);
        sceneSkip.transform.SetAsLastSibling();
        //#else
        //Debug.Log("Sono un salta scene");
        //#endif
    }

    public void ActivateOptions(){
        audioPanel.transform.SetAsLastSibling();

        if(player.opzioniAudio.option.standard)
            std.isOn = true;
        else if(player.opzioniAudio.option.muto)
            mute.isOn = true;
        else if(player.opzioniAudio.option.tasti)
            tasti.isOn = true;
        else
            pers.isOn = true;

        numRep.text = player.opzioniAudio.option.ripetizioni+"";
        profileDropDown.options.Clear();
        if(player.opzioniAudio.option.tipoLettura == 1){
            for(int i = 0; i < player.opzioniAudio.option.nomiProfiliModificatiAdvanced.Length; i++){
                profileDropDown.options.Add(new Dropdown.OptionData(player.opzioniAudio.option.nomiProfiliModificatiAdvanced[i]));
            }
        } else {
            for(int i = 0; i < player.opzioniAudio.option.nomiProfiliModificatiEasy.Length; i++){
                profileDropDown.options.Add(new Dropdown.OptionData(player.opzioniAudio.option.nomiProfiliModificatiEasy[i]));
            }
        }

         profileDropDown.value = player.opzioniAudio.option.profileSelected;
         audioPanel.SetActive(true);        
    }

    public void IncreaseReps(){
        if(player.opzioniAudio.option.ripetizioni < 5){
            Debug.Log("Ripetizione: " + player.opzioniAudio.option.ripetizioni);
            player.opzioniAudio.option.ripetizioni++;
            numRep.text = player.opzioniAudio.option.ripetizioni+"";
        }
    }

    public void DecreaseReps(){
        if(player.opzioniAudio.option.ripetizioni > 1){
            player.opzioniAudio.option.ripetizioni--;
            numRep.text = player.opzioniAudio.option.ripetizioni+"";
        }
    }

    public void SaveAudio(){
        player.opzioniAudio.option.standard = std.isOn;
        player.opzioniAudio.option.muto = mute.isOn;
        player.opzioniAudio.option.tasti = tasti.isOn;
        player.opzioniAudio.option.personalizzata = pers.isOn;
        player.opzioniAudio.option.ripetizioni = int.Parse(numRep.text);
        player.opzioniAudio.option.profileSelected = profileDropDown.value;
        player.opzioniAudio.salvaOpzioniAudio();
        player.opzioniAudio.caricaOpzioniAudio();
        saveToast.SetActive(true);
        StartCoroutine(saveMsg());
    }

    IEnumerator saveMsg(){
        yield return new WaitForSeconds(1);
        saveToast.SetActive(false);
        audioPanel.SetActive(false);
    }

    public void InfoButton(){

        audioButton.gameObject.SetActive(false);
        Transform panelInfo = GameObject.Find("Scena0").transform.GetChild(1); //mettere indice 1
        panelInfo.gameObject.SetActive(true);
        Button buttonClose = panelInfo.transform.GetChild(0).GetComponent<Button>();
        buttonClose.onClick.AddListener(delegate
        {
            audioButton.gameObject.SetActive(true);
        });
    }

}
