using UnityEngine;
using System.Collections;

public class PlayerFactoryMenuScript : MonoBehaviour {
	public RectTransform hatRect;
	public Camera camera;
	GUIContent[] hatList;
	private ComboBox comboBoxControl;// = new ComboBox();
	private GUIStyle listStyle = new GUIStyle();
	// Use this for initialization
	void Start () {
		Vector3 [] corners = new Vector3[4];
		hatRect.GetWorldCorners(corners);

		Rect r = new Rect (corners [0].x, corners [0].y, corners [2].x - corners [0].x, corners [2].y - corners [0].y);
		Debug.Log (hatRect.rect.x + " " + hatRect.rect.y + " " + hatRect.rect.height + " " + hatRect.rect.width);
		hatList = new GUIContent[4];
		hatList[0] = new GUIContent("test");
		hatList[1] = new GUIContent("speed");
		hatList[2] = new GUIContent("dash");
		hatList[3] = new GUIContent("dodge");


		listStyle.normal.textColor = Color.white; 
		listStyle.onHover.background =
			listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.left =
			listStyle.padding.right =
				listStyle.padding.top =
				listStyle.padding.bottom = 4;

		comboBoxControl = new ComboBox(hatRect.rect, new GUIContent("Hat"), hatList, "button", "box", listStyle);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		bool showlist = false;
		int picked_item = comboBoxControl.Show ();
		//GUI.Box(this.GetComponent<RectTransform>().rect,picked_item.ToString());
	}
}
