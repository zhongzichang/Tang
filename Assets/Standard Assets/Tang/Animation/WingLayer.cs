/*
 * Created by SharpDevelop.
 * User: zzc
 * Date: 2013/8/12
 * Time: 15:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Tang
{
	/// <summary>
	/// Description of WingLayer.
	/// </summary>
	public class WingLayer : CommonLayer
	{
		public const int ID = LayerID.WING;
		public const string NAME = "wing";
		
		public WingLayer() : base(ID, NAME)
		{
		}
		
		public WingLayer(Sprite spritePrefab) : base(ID, NAME, spritePrefab)
		{
		}
	}
}

