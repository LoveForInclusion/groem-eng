using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class ControllerSceltaKevin : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform wp1; // Scatola in mano

    private Transform wp;
    public Text quesiton;
    private string target;
    public float speed;
    private bool resume;
    public Transform actor;
    private bool finish;
    [Header("Debug Variables")]
    public float distance;
    public GameObject[] hardObject;
    //private bool difficulty; // false = Easy ; true = Hard; 
    // Start is called before the first frame update
    void Start()
    {
        wp = wp1;
        finish = false;
        resume = false;
  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && resume)
        {

            resume = false;
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(point, Vector3.forward, Mathf.Infinity);

            if (hit.collider != null)
            {
                   
                Transform selected = hit.transform;
                Debug.Log("Hit : " + selected.name);
            
                StartCoroutine(Right(selected));
                

            }
            else{
                resume = true;
            }

        }
    }

    IEnumerator Right(Transform tr){
        wp.gameObject.SetActive(false);
        // UnityEditorInternal.ComponentUtility.CopyComponent(tr.GetComponent(typeof(SpriteRenderer)));
        // UnityEditorInternal.ComponentUtility.PasteComponentValues(wp.GetComponent<SpriteRenderer>());
        wp.GetComponent<SpriteRenderer>().sprite = tr.GetComponent<SpriteRenderer>().sprite;
        wp.GetComponent<SpriteRenderer>().enabled = true;

        while (Vector2.Distance(tr.position, wp.position) > distance)
        {
            tr.position = Vector2.Lerp(tr.position, wp.position, Time.deltaTime + speed);
            yield return new WaitForEndOfFrame();
        }

        wp.gameObject.SetActive(true);
        Destroy(tr.gameObject);
        
        actor.GetComponent<Animator>().Play("action5");
        yield return new WaitForSeconds(1f);
        while(Camera.main.orthographicSize > 3.01f){
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(-5, 0, -10), Time.deltaTime*1.5f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 3, Time.deltaTime*1.5f);
            yield return new WaitForEndOfFrame();
        }
        Camera.main.transform.position = new Vector3(-5, 0, -10);
        Camera.main.orthographicSize = 3;
            
        

    }



    public void SetupScene(bool difficulty){ // false = Easy ; true = Hard; 

      
        for(int i = 0; i < hardObject.Length; i++){
            hardObject[i].SetActive(difficulty);
        }
        resume = true;
    

    }

    public void ReloadScene(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

  

    
}
