using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class FocusEllipse : MonoBehaviour {

	public enum FocusState {
		Initializing,
		Finding,
		Found
	}

	public GameObject ScanImage;
	public GameObject FoundEllipse;

	//for editor version
	public float maxRayDistance = 30.0f;
	public LayerMask collisionLayerMask;
	public float findingSquareDist = 0.5f;
	public bool IsScanning = false;
	public bool IsHorizontal = true;

	private FocusState squareState;
	public FocusState SquareState { 
		get {
			return squareState;
		}
		set {
			squareState = value;
			FoundEllipse.SetActive (squareState == FocusState.Found);
			ScanImage.SetActive (squareState != FocusState.Found);
		} 
	}

	bool trackingInitialized;

	private void Start()
	{
		SquareState = FocusState.Initializing;
		trackingInitialized = true;
	}

	bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
	{
		List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
		if (hitResults.Count > 0) {
			foreach (var hitResult in hitResults) {
				FoundEllipse.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
				FoundEllipse.transform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
				Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", FoundEllipse.transform.position.x, FoundEllipse.transform.position.y, FoundEllipse.transform.position.z));
				return true;
			}
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		if (IsScanning)
		{
			//use center of screen for focusing
			Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, findingSquareDist);

			var screenPosition = Camera.main.ScreenToViewportPoint(center);
			ARPoint point = new ARPoint {
				x = screenPosition.x,
				y = screenPosition.y
			};

			ARHitTestResultType[] resultTypes;
						
			if (IsHorizontal)
			{
				resultTypes = new ARHitTestResultType[]
				{
					//ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
					//ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent,
					// if you want to use infinite planes use this:
					//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
					ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
					//ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
					//ARHitTestResultType.ARHitTestResultTypeFeaturePoint
				};
			}
			else
			{
				resultTypes = new ARHitTestResultType[]
				{
					//ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
					//ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent,
					// if you want to use infinite planes use this:
					//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
					//ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
					ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
					//ARHitTestResultType.ARHitTestResultTypeFeaturePoint
				};
			}

			foreach (ARHitTestResultType resultType in resultTypes)
			{
				if (HitTestWithResultType (point, resultType))
				{
					SquareState = FocusState.Found;
					return;
				}
			}

			//if you got here, we have not found a plane, so if camera is facing below horizon, display the focus "finding" square
			if (trackingInitialized)
			{
				SquareState = FocusState.Finding;

			}
		}
	}

	public void HideFoundEllipse()
	{
		FoundEllipse.SetActive(false);
	}

}
