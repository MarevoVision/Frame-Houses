using System;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine.EventSystems;

namespace UnityEngine.XR.iOS
{
	public class UnityARHitTestExample : MonoBehaviour
	{
		public Transform m_HitTransform;
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
		public GameObject PortalRoom;
        public GameObject PortalRoomMini;
        public GameObject Wall;
        public GameObject HouseMini;
        public Vector3 houseMiniPosition;
        public Vector3 houseMiniRotation;
		public CanvasController CanvasController;
		public FocusEllipse FocusEllipse;
		public GenerateImageAnchor GenerateImageAnchor;
		public GameObject[] WallElements;
		public bool IsHorizontal = false;
		public bool IsScaning = false;
        //public PortalController PortalController;

        private bool _isPlaced = false;

        bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
					Debug.Log ("Got hit!");
					m_HitTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
					Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
					m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                    //if (IsHorizontal)
                    if (IsHorizontal && CanvasController.changeHouseMode == 1)
					{
						Vector3 currAngle = PortalRoom.transform.eulerAngles;
						PortalRoom.transform.LookAt (Camera.main.transform);
						PortalRoom.transform.eulerAngles = new Vector3 (currAngle.x,PortalRoom.transform.eulerAngles.y,currAngle.z);
						PortalRoom.SetActive(true);
						//PortalController.OutsidePortal();
					}
					if(IsHorizontal && CanvasController.changeHouseMode == 2)
					{
                        Vector3 currAngle = PortalRoomMini.transform.eulerAngles;
                        PortalRoomMini.transform.LookAt(Camera.main.transform);
                        PortalRoomMini.transform.eulerAngles = new Vector3(currAngle.x, PortalRoomMini.transform.eulerAngles.y, currAngle.z);
                        PortalRoomMini.SetActive(true);
                        PortalRoomMini.transform.position = m_HitTransform.position;
                        //PortalController.OutsidePortal();
                    }
                    _isPlaced = true;
					CanvasController.ScanImage.SetActive(false);
					Debug.Log("Call hide image");
					FocusEllipse.IsScanning = false;
					FocusEllipse.HideFoundEllipse();
					return true;
				}
			}
			return false;
		}
		
		// Update is called once per frame
		void Update () {
			if (!_isPlaced && IsScaning)
			{
				if (Input.touchCount > 0 && m_HitTransform != null && !EventSystem.current.IsPointerOverGameObject((0)))
				{
					var touch = Input.GetTouch(0);
					if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
					{
						var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
						ARPoint point = new ARPoint
						{
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
							if (HitTestWithResultType(point, resultType))
							{
								return;
							}
						}
					}
				}
			}
		}

		public void Reset(bool isFocusOn)
		{
			_isPlaced = false;
			IsScaning = isFocusOn;
			PortalRoom.SetActive(false);
            PortalRoomMini.SetActive(false);
            HouseMini.transform.position = houseMiniPosition;
            HouseMini.transform.rotation = new Quaternion(houseMiniRotation.x, houseMiniRotation.y, houseMiniRotation.z, HouseMini.transform.rotation.w);
			Wall.SetActive(false);
			//PortalController.OutsidePortal();
			GenerateImageAnchor.HideGameObject();
			FocusEllipse.IsScanning = isFocusOn;
			if (!isFocusOn)
			{
				FocusEllipse.HideFoundEllipse();
			}
			CanvasController.ScanImage.SetActive(true);
			Debug.Log("Call show image");
		}

		public void SetHorizontalScanning(bool isHorizontal)
		{
			IsHorizontal = isHorizontal;
			FocusEllipse.IsHorizontal = isHorizontal;
		}
		
		public void ResetWall()
		{
			for (int i = 0; i < WallElements.Length; i++)
			{
				Debug.Log("Reseted " + i);
				WallElements[i].SetActive(true);
			}
		}
	
	}
}

