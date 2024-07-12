using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VuotaCache : MonoBehaviour {

	IEnumerator Start () {
		Debug.Log (Application.temporaryCachePath);

		yield return new WaitForSeconds(.5f);

		if (Caching.ClearCache ())
			Debug.Log ("Cache Svuotata");
		else
			Debug.Log ("Cache in uso");
		
	}
		
}
