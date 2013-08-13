/*
 * Created by SharpDevelop.
 * User: zzc
 * Date: 2013/8/9
 * Time: 14:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using UnityEngine;

namespace Tang
{
	/// <summary>
	/// Description of MultiLayerAnimcationTestRunner.
	/// </summary>
	public class SpriteAnimationTestRunner : MonoBehaviour
	{
		void Start() {
			
			UUnitTestSuite suite = new UUnitTestSuite();
			suite.AddAll(typeof(SpriteAnimationTestCase));
			UUnitTestResult result = suite.Run ();
			Debug.Log (result.Summary ());
			
		}
	}
}
