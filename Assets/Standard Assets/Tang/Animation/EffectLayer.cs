/*
 * Created by SharpDevelop.
 * User: zzc
 * Date: 2013/8/12
 * Time: 15:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Tang
{
	/// <summary>
	/// Description of EffectLayer.
	/// </summary>
	public class EffectLayer : CommonLayer
	{
		public const int ID = LayerID.EFFECT;
		public const string NAME = "effect";
		
		public EffectLayer() : base(ID, NAME)
		{
		}
		
		public EffectLayer(Sprite spritePrefab) : base(ID, NAME, spritePrefab){
		}
	}
}
