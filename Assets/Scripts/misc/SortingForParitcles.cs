using UnityEngine;

public class SortingForParitcles : MonoBehaviour {
	void Start () {
            //Change Foreground to the layer you want it to display on 
            //You could prob. make a public variable for this
            particleSystem.renderer.sortingLayerName = "Fore";
	}
}
