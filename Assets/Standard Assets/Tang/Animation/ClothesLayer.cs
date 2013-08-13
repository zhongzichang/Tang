/*
 * Created by SharpDevelop.
 * User: zzc
 * Date: 2013/8/12
 * Time: 15:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Tang
{
	/// <summary>
	/// Description of ClothesLayer.
	/// </summary>
	public class ClothesLayer : CommonLayer
	{
		public const int ID = LayerID.CLOTHES;
		public const string NAME = "clothes";
		
		public ClothesLayer() : base(ID, NAME)
		{
		}
		
		public ClothesLayer(Sprite spritePrefab) : base(ID, NAME, spritePrefab){
		}
	}
}
