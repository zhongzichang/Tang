using UnityEngine;
using System.Collections;

namespace Tang 
{
	
	public class SpriteAnimationTestCase : UUnitTestCase {
		
		private string clothesPath = "Tang-Test/Output/MultiSet/Prefab/player_lygf0";
		private string weaponPath = "Tang-Test/Output/MultiSet/Prefab/weapon_hf3";
		private string clothesPath1 = "Tang-Test/Output/MultiSet/Prefab/player_lygf1";
		
		private Sprite clothesPrefab = null;
		private Sprite weaponPrefab = null;
		private Sprite clothesPrefab1 = null;
		
		private SpriteAnimation animation = null;
		
		protected override void SetUp(){ 
			
			animation = GameObject.FindObjectOfType(typeof(SpriteAnimation)) as SpriteAnimation;
			UUnitAssert.NotNull(Resources.Load(clothesPath));
			GameObject clothesGo = Resources.Load(clothesPath) as GameObject;
			GameObject weaponGo = Resources.Load(weaponPath) as GameObject;
			GameObject clothesGo1 = Resources.Load(clothesPath1) as GameObject;
			
			clothesPrefab = clothesGo.GetComponent<Sprite>();
			weaponPrefab = weaponGo.GetComponent<Sprite>();
			clothesPrefab1 = clothesGo1.GetComponent<Sprite>();
			
		}
	
		[UUnitTest]
		public void TestAllMethods(){
			
			PrefixTest();
			
			SpriteLayer clothesLayer = new ClothesLayer();
			
			SpriteLayer weaponLayer = new WeaponLayer();
			
			animation.PutLayer(clothesLayer);
			animation.PutLayer(weaponLayer);
			
			
			if(animation.IsPlaying){
				animation.Stop();
				UUnitAssert.False(animation.IsPlaying);
				animation.Play();
				UUnitAssert.True(animation.IsPlaying);
			} else {
				animation.Play();
				UUnitAssert.True(animation.IsPlaying);
				animation.Stop();
				UUnitAssert.False(animation.IsPlaying);
			}
			
			weaponLayer = animation.GetLayer(LayerID.WEAPON);
			UUnitAssert.NotNull(weaponLayer);			
			animation.Clear(weaponLayer);
			
			weaponLayer = animation.GetLayer(weaponLayer.id);
			UUnitAssert.NotNull(weaponLayer);
			weaponLayer.spritePrefab = weaponPrefab;
			animation.PutLayer(weaponLayer);
			
			animation.Open(clothesPrefab1, LayerID.CLOTHES);
			
			
		}
		
	
		
		private void PrefixTest(){
			
			UUnitAssert.NotNull(animation);
			UUnitAssert.NotNull(clothesPrefab);
			UUnitAssert.NotNull(weaponPrefab);
			
		}
	
		
	}
}
