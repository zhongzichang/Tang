using UnityEngine;
using System.Collections;

namespace Tang {
	
	public class Sprite : MonoBehaviour {
		
		#region Public Fields
		public bool m_flipHorizontal = false;
		public bool m_flipVertical = false;
		public int m_currentIndex = 0;
		#endregion
		
		#region Protected Fields
		protected int m_maxIndex = 0;
		#endregion
		
		#region Private Fields
		private MeshFilter mf = null;
		private bool frameUpdated = false;
		private Frame m_currentFrame = null;
		#endregion
		
		#region Public Properties 
		public int maxIndex {
			get { return m_maxIndex;}
		}
		public virtual int currentIndex {
			get { return m_currentIndex; }
			set { m_currentIndex = value; }
		}
		public Frame CurrentFrame {
			get {
				return m_currentFrame;
			}
			set {
				if(value != null){
					m_currentFrame = value;
					frameUpdated = true;
				}
			}
		}
		public bool flipHorizontal {
			get { return m_flipHorizontal; }
			set {
				if( m_flipHorizontal != value ) {
					m_flipHorizontal = value;
					frameUpdated = true;
				}
			}
		}
		public bool flipVertical {
			get { return m_flipVertical; }
			set {
				if( m_flipVertical != value ){
					m_flipVertical = value;
					frameUpdated = true;
				}
			}
		}
		#endregion
				
		#region Protected Methods
		protected void Init(){
			InitComponents();
		}
		
		protected void InitComponents(){
			
			mf = gameObject.GetComponent<MeshFilter>();
			if(mf == null)
				mf = gameObject.AddComponent<MeshFilter>();
			Mesh mesh = MeshOne.NewMesh();
			mf.mesh = mesh;
			
		}
		
		protected void UpdateMesh(Frame fr){
			
			Vector2[] uv = fr.uv;			
			
			if( flipHorizontal ){
				uv = new Vector2[fr.uv.Length];
				for(int i=0; i<uv.Length; i++){
					uv[0] = fr.uv[1];
					uv[1] = fr.uv[0];
					uv[2] = fr.uv[3];
					uv[3] = fr.uv[2];
				}
				transform.localPosition = new Vector3( -fr.offset.x, 0, fr.offset.y );
			}
			
			if( flipVertical ){
				uv = new Vector2[fr.uv.Length];
				for(int i=0; i<uv.Length; i++){
					uv[0] = fr.uv[3];
					uv[3] = fr.uv[0];
					uv[1] = fr.uv[2];
					uv[2] = fr.uv[1];
				}
				transform.localPosition = new Vector3( fr.offset.x, 0, -fr.offset.y );
			}
			
			#if UNITY_EDITOR
			mf.sharedMesh.uv = uv;
			#else
			mf.mesh.uv = uv;
			#endif
			
			if( !flipHorizontal && !flipVertical ){
				transform.localPosition = new Vector3( fr.offset.x, 0, fr.offset.y );
			}
			transform.localScale = new Vector3( fr.size.x, 1, fr.size.y);
			
		}
		#endregion
		
		#region Mono Methods
		void Update(){
			if(frameUpdated){
				frameUpdated = false;
				UpdateMesh(m_currentFrame);
			}
		}
		void OnDestroy(){
			
			if(mf != null){
				#if UNITY_EDITOR
				DestroyImmediate(mf.sharedMesh);
				#else
				Destroy(mf.mesh);
				#endif
			}
		}
		#endregion
	}
	
}
