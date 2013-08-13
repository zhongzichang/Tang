/*
 * Created by SharpDevelop.
 * User: zzc
 * Date: 2013/8/12
 * Time: 17:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using UnityEngine;
using UnityEditor;

namespace Tang
{
	/// <summary>
	/// Description of AnimationGenerator.
	/// </summary>
	public class AnimationGenerator
	{
		
		private const string npcPrefabDirPath = "Assets/Resources/Tang/Objects/Animations";
		
		/// <summary>
		/// 生成 NPC 动画
		/// </summary>
		/// <param name="gobj">npc 的 prefab</param>
		public static void GenerateAnimation(GameObject obj){
			
					
			// 判断该对象是不是
			Sprite sprite = obj.GetComponent<Sprite>();
			Debug.Log(sprite);
			if(sprite != null) {
				
				GameObject animationObj = new GameObject();
				animationObj.name = obj.name + "-animation";
				SpriteAnimation animation = animationObj.AddComponent<SpriteAnimation>();
				
				// animation.
				SpriteLayer clothesLayer = new ClothesLayer(obj.GetComponent<Sprite>());
				animation.PutLayer(clothesLayer);
				
				string prefabPath = npcPrefabDirPath + "/" + animationObj.name + ".prefab";
				UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
				PrefabUtility.ReplacePrefab(animationObj, prefab, ReplacePrefabOptions.ConnectToPrefab);
				
				// destory game object
				GameObject.DestroyImmediate(animationObj);
				
			}
			
		}
	}
}
