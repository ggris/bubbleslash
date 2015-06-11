using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerFactoryMenuScript : MonoBehaviour {
	private GameObject player_factory;
	public GameObject pf_prefab;
	public Color[] colors;


	//settings
	private int color_indice = 0;
	private Color color;
	private PlayerSettings.Hat hat = 0;

	//rendering on buttons
	UnityEngine.UI.Text text_hat;
	UnityEngine.UI.Button button;	
	void Awake(){
		color_indice = 0;

	}

	void OnDestroy(){
		GameObject.Destroy (player_factory);
	}
	void Start () {
		player_factory = Instantiate (pf_prefab);
		button = gameObject.transform.Find ("color").gameObject.GetComponent<UnityEngine.UI.Button> ();
		text_hat = gameObject.transform.Find ("hat").Find ("Text").GetComponent<UnityEngine.UI.Text> ();
	}
	
	// Update is called once per frame
	void Update () {

	}
	void changeHat(){
		hat = PlayerSettings.nextHat (hat);
		text_hat.text = PlayerSettings.ToString (hat);
	}
	void changeColor(){
		color_indice = (++color_indice) % colors.Length;
		color = colors [color_indice];
		button.image.color = color;
		player_factory.GetComponent<PlayerFactory> ().color = color;
	}
	void OnGUI(){
		//bool showlist = false;
		//int picked_item = comboBoxControl.Show ();
		//GUI.Box(this.GetComponent<RectTransform>().rect,picked_item.ToString());
	}
}
