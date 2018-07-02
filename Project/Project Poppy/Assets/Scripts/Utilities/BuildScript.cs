using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using System.Diagnostics;

public class BuildScript : IPostprocessBuild
{


	public int callbackOrder {get {return 0;}}
	
	public void OnPostprocessBuild(BuildTarget target, string path)
    {
		UnityEngine.Debug.Log("Post Processing Build");

		//string buildNumber = PlayerSettings.

		#if UNITY_STANDALONE_WIN


		#endif
    }
	
}
