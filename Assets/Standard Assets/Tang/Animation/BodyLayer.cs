/*
 * Created by SharpDevelop.
 * User: zzc
 * Date: 2013/8/13
 * Time: 15:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Tang
{
	/// <summary>
	/// Description of BodyLayer.
	/// </summary>
	public class BodyLayer : CommonLayer
	{
		public const int ID = LayerID.BODY;
		public const string NAME = "body";
		
		public BodyLayer() : base(ID, NAME)
		{
		}
		
		public BodyLayer(Sprite spritePrefab) : base(ID, NAME, spritePrefab){
		}
	}
}
