using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.iOS;

public class CanvasController : MonoBehaviour
{
	public GameObject ScanImage;
	public GameObject TargetScanText;
	public GameObject PortalScanText;
	public GameObject WallScanText;
	public GameObject ResetButton;
	public UnityARCameraManager ArCameraManager;
	public UnityARHitTestExample ArHitTestExample;
	public GenerateImageAnchor GenerateImageAnchor;
    public int changeHouseMode;

	private Vector3 _scanScale;
	
	private void Start () {
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent += OnTargetModelPlaced;
		_scanScale = ScanImage.transform.localScale;
		ScanPulsingEffect();
        PortalButtonPressedMini();

    }

	private void OnDestroy()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= OnTargetModelPlaced;
	}

	public void TargetButtonPressed()
	{
		ChangeScanText(true, false, false);
		ArHitTestExample.Reset(false);
		ArHitTestExample.SetHorizontalScanning(false);
		GenerateImageAnchor.IsAnchorActive = true;
		ResetButton.SetActive(false);
	}

	public void PortalButtonPressed()
	{
		ArCameraManager.planeDetection = UnityARPlaneDetection.Horizontal;
		ChangeScanText(false, true, false);
		ArHitTestExample.Reset(true);
		GenerateImageAnchor.IsAnchorActive = false;
		ArHitTestExample.SetHorizontalScanning(true);
		ResetButton.SetActive(false);
        changeHouseMode = 1;
	}

    public void PortalButtonPressedMini()
    {
        ArCameraManager.planeDetection = UnityARPlaneDetection.Horizontal;
        ChangeScanText(false, true, false);
        ArHitTestExample.Reset(true);
        GenerateImageAnchor.IsAnchorActive = false;
        ArHitTestExample.SetHorizontalScanning(true);
        ResetButton.SetActive(false);
        changeHouseMode = 2;
    }

    public void WallButtonPressed()
	{
		ArCameraManager.planeDetection = UnityARPlaneDetection.Vertical;
		ChangeScanText(false, false, true);
		ArHitTestExample.Reset(true);
		GenerateImageAnchor.IsAnchorActive = false;
		ArHitTestExample.SetHorizontalScanning(false);
		ResetButton.SetActive(false);
	}

	public void OnTargetModelPlaced(ARImageAnchor arImageAnchor)
	{
		ScanImage.SetActive(false);
	}

	public void OnModelPlaced()
	{
		Debug.Log("Hide image");
		ScanImage.SetActive(false);
	}

	public void OnRecognitionReset()
	{
		Debug.Log("Show image");
		ScanImage.SetActive(true);
	}

	private void ScanPulsingEffect()
	{
		ScanImage.transform.DOScale(1.5f, 1f).SetLoops(-1, LoopType.Yoyo);
	}

	private void ChangeScanText(bool isTarget, bool isPortal, bool isWall)
	{
		TargetScanText.SetActive(isTarget);
		PortalScanText.SetActive(isPortal);
		WallScanText.SetActive(isWall);
	}

	public void ShowResetButton()
	{
		ResetButton.SetActive(true);
	}
	public void ResetButtonPressed()
	{
		ArHitTestExample.ResetWall();
	}
}
