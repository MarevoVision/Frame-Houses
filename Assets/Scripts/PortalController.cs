using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.XR.iOS
{
	public class PortalController : MonoBehaviour
	{
		public Material[] Materials;
		public MeshRenderer MeshRenderer;
		public UnityARVideo UnityArVideo;
	
		private void Start () {
			OutsidePortal ();
		}
	
		public void OutsidePortal(){
			StartCoroutine (DelayChangeMat((int)CompareFunction.Equal));
		}

		public void InsidePortal(){
			StartCoroutine (DelayChangeMat ((int)CompareFunction.NotEqual));
		}

		IEnumerator DelayChangeMat(int stencilNum){
			UnityArVideo.shouldRender = false;
			yield return new WaitForEndOfFrame ();
			MeshRenderer.enabled = false;
			ChangeStencilShader(stencilNum);
			yield return new WaitForEndOfFrame ();
			MeshRenderer.enabled = true;
			UnityArVideo.shouldRender = true;
		}

		private void ChangeStencilShader(int stencilNum)
		{
			foreach (Material mat in Materials) {
				Debug.Log(mat.name + " " + stencilNum);
				mat.SetInt ("_Stencil", stencilNum);
			}
		}

		private void OnDestroy()
		{
			ChangeStencilShader((int)CompareFunction.NotEqual);
		}
	}
}