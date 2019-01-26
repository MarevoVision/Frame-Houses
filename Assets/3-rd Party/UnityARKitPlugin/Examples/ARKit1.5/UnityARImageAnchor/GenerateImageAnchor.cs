using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class GenerateImageAnchor : MonoBehaviour {

	
	public bool IsAnchorActive = true;

	[SerializeField]
	private ARReferenceImage referenceImage;

	[SerializeField]
	private GameObject prefabToGenerate;

	private GameObject imageAnchorGO;

	private ARImageAnchor _currentImageAnchor;

	private void Awake()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
	}

	void AddImageAnchor(ARImageAnchor arImageAnchor)
	{
		if (IsAnchorActive)
		{
			Debug.Log("image anchor added");
			if (arImageAnchor.referenceImageName == referenceImage.imageName)
			{
				Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
				Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
				_currentImageAnchor = arImageAnchor;
				imageAnchorGO = Instantiate<GameObject>(prefabToGenerate, position, rotation);
			}
		}
	}

	void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		if (IsAnchorActive)
		{
			Debug.Log("image anchor updated");
			if (arImageAnchor.referenceImageName == referenceImage.imageName)
			{
				imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
				imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
				if (!imageAnchorGO.active)
				{
					imageAnchorGO.SetActive(true);
				}
			}
		}
	}

	void RemoveImageAnchor(ARImageAnchor arImageAnchor)
	{
		if (IsAnchorActive)
		{
			Debug.Log("image anchor removed");
			if (imageAnchorGO)
			{
				GameObject.Destroy(imageAnchorGO);
			}
		}
	}

	public void HideGameObject()
	{
		if (imageAnchorGO) {
			Debug.Log("image anchor hidden");
			imageAnchorGO.SetActive(false);
			IsAnchorActive = false;
		}
	}

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

	}

}
