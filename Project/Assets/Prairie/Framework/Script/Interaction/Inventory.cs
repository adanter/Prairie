using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour
{	
	public const int numSlots = 4;
	public InventoryContent[] contents = new InventoryContent[numSlots];

	public void AddToInventory (GameObject objToAdd)
	{
		for (int i = 0; i < contents.Length; i++) {
			if (contents [i].obj == null) {
				Image img = getImage (objToAdd);
				Debug.Log ("Add " + objToAdd.name + "to Inventory slot " + i.ToString () + ".");
				contents [i] = new InventoryContent(img, objToAdd);
				if (img != null) {
					contents [i].objImage.enabled = false;
				}
				return;
			}
		}
	}

	public void RemoveFromInventory (GameObject objToRemove)
	{
		for (int i = 0; i < contents.Length; i++) {
			if (contents [i].obj == objToRemove) {
				Debug.Log ("Remove " + objToRemove.name + "from Inventory slot " + i.ToString () + ".");
				contents [i] = null;
				if (contents [i].objImage != null) {
					contents [i].objImage.enabled = true;
				}
				return;
			}
		}
	}

	private Image getImage(GameObject o){
		Image[] imgs = o.GetComponents<Image>();
		if (imgs.Length > 0) {
			return imgs [0];
		}
		return null;
	}
}

[System.Serializable]
public class InventoryContent
{
	public Image objImage;
	public GameObject obj;

	public InventoryContent(Image i, GameObject o)
	{
		objImage = i;
		obj = o;
	}
}

