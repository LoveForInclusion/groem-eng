using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAssetBundles
{
	[MenuItem ("Assets/Build AssetBundlesAndroid")]
	static void BuildAllAssetBundlesAndroid ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles/Android", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);

	}

	[MenuItem ("Assets/Build AssetBundlesIos")]
	static void BuildAllAssetBundlesIos ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles/Ios", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);

	}
}

public class testbundle : MonoBehaviour {

}
