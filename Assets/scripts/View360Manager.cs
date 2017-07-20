using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class View360Manager : MonoBehaviour {

    public Texture2D[] views;
    private MeshRenderer renderer;

	private void Awake () {
        renderer = this.GetComponent<MeshRenderer>();
	}
    private void Start()
    {
        VRSettings.LoadDeviceByName("cardboard");
        this.gameObject.SetActive(false);
    }

    public void SetView(int _index)
    {
        this.gameObject.SetActive(true);
        renderer.material.mainTexture = views[_index];
        VRSettings.enabled = true;
        Debug.Log(VRSettings.enabled);
    }
}
