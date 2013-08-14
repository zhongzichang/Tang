/*
 * Created by SharpDevelop.
 * User: zzc
 * Date: 2013/8/6
 * Time: 17:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tang
{
  /// <summary>
  /// Description of Animation.
  /// </summary>
  [ExecuteInEditMode]
  public class SpriteAnimation : MonoBehaviour
  {
    /// <summary>
    /// Prefab
    /// </summary>
    public SpriteLayer[] layers; // 层 1...*
    public int loop = -1; // 播放次数，<0 表示无限循环播放
    public int fps = Tang.FPS; // FPS，每秒多少帧
    public int m_currentIndex = 0; // 当前放到第几帧
    public bool m_flipHorizontal = false; // 是否水平翻转
    public bool m_flipVertical = false; // 是否垂直翻转
    public float delay = 0F; // 延迟多少秒开始
    public bool playOnStart = false; // 在 Mono 的 Start() 方法执行播放
		
    // 委派 ---
    public delegate void Callback(SpriteAnimation animation); // 播放状态回调
    public Callback OnFirstFrame; // 播放最后一帧前回调
    public Callback OnLastFrame; // 播放第一帧前回调
		
#region Private Fields
    private int maxIndex = 0; // 当前动画的最大帧的索引值，从0开始
		
    private float currentTime = 0F; // 当前帧已显示的时间，每一帧的现实时间不能超过 1F/FPS
    private bool playing = false; // 是否正在播放
    private float delayTimer = 0F; // 延迟计时器， 当计时器初始值 =delay，递减到0后开始播放内容
    private int loopCounter = 0; // 播放次数计算器
#endregion
		
#region Public Properties
    /// <summary>
    /// 当前帧索引，从0开始到 maxIndex
    /// </summary>
    public int currentIndex {
      get {
	return m_currentIndex;
      }
      private set {
	m_currentIndex = value;
	UpdateSpriteIndexes();
      }
    }
    /// <summary>
    /// 是否水平翻转
    /// </summary>
    public bool flipHorizontal {
      get { return m_flipHorizontal; }
      set {
	m_flipHorizontal = value;
	foreach(SpriteLayer layer in layers){
	  if(layer.spriteInstance != null)
	    layer.spriteInstance.flipHorizontal = m_flipHorizontal;
	}
      }
    }
    /// <summary>
    /// 是否垂直翻转
    /// </summary>
    public bool flipVertical {
      get { return m_flipVertical; }
      set {
	m_flipVertical = value;
	foreach(SpriteLayer layer in layers){
	  if(layer.spriteInstance != null)
	    layer.spriteInstance.flipVertical = m_flipVertical;
	}
      }
    }
    /// <summary>
    /// 是否正在播放
    /// </summary>
    public bool IsPlaying{
      get{
	return playing;
      }
    }
#endregion
		
#region Public Method
		
    /// <summary>
    /// 将一个精灵层放入动画中
    /// </summary>
    /// <param name="layer">精灵层</param>
    public void PutLayer(SpriteLayer layer){
			
      if(layers == null || layers.Length == 0){
				
	layers = new SpriteLayer[1];
	layers[0] = layer;
				
      } else {
				
	// 判断是替换还是追加
	bool replace = false;			
	for(int i=0; i<layers.Length; i++ ){
	  if(layers[i].id == layer.id){
	    // 替换原有的层
	    layers[i] = layer;
	    replace = true;
	    break;
	  }
	}
	if( !replace ) {
	  // 追加
	  System.Array.Resize<SpriteLayer>(ref layers, layers.Length + 1);	
	  layers[layers.Length-1] = layer;
	}
      }
			
      InitLayer(layer);
    }
		
    /// <summary>
    /// 获取指定层，没有则返回 null
    /// </summary>
    /// <param name="layerId">层ID</param>
    /// <returns>指定层，没有返回 null</returns>
    public SpriteLayer GetLayer( int layerId ){
			
      foreach( SpriteLayer layer in layers ){
	if( layer.id == layerId )
	  return layer;
      }
      return null;
			
    }
		
    /// <summary>
    /// 在指定层打开一个精灵
    /// </summary>
    /// <param name="spritePrefab">精灵 sprite</param>
    /// <param name="layerId">所在层</param>
    public void Open( Sprite spritePrefab, int layerId ){
			
      foreach( SpriteLayer layer in layers ){
	if( layer.id == layerId ){
	  // 如果该层有内容，则重置该层的内容
	  if(layer.spritePrefab != null){
	    Clear(layer);
	  }
					
	  layer.spritePrefab = spritePrefab;
	  InitLayer(layer);
	  break;
	}
      }
    }
		
    /// <summary>
    /// 清除指定层的精灵
    /// </summary>
    /// <param name="layerId">层ID</param>
    public void Clear(int layerId){
      SpriteLayer layer = GetLayer(layerId);
      if( layer != null ){
	Clear(layer);
      }
    }
		
    /// <summary>
    /// 清除指定层的精灵
    /// </summary>
    /// <param name="layer">层ID</param>
    public void Clear(SpriteLayer layer){
      if(layer.gameObject != null) {
	DestroyGobj(layer.gameObject);
	layer.gameObject = null;
      }
      layer.hidden = true;
      layer.maxIndex = 0;
      layer.spritePrefab = null;
      layer.spriteInstance = null;
    }
    /// <summary>
    /// 播放
    /// </summary>
    public void Play(){
      playing = true;
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop(){
      playing = false;
      Reset();
    }
#endregion
		
#region Private Methods
    private void UpdateSpriteIndexes(){
			
      if(layers != null) {
			
	foreach(SpriteLayer layer in layers){
					
	  if (layer.spriteInstance != null){
						
	    // 需要保证 sprite instance 不为 null
					
	    int index = m_currentIndex - layer.frameDelay;
	    if(index < 0 ){
	      if(layer.hiddenBeforeBegin && !layer.hidden){
		HideLayer(layer, true);
	      }
	    } else if( index > layer.spriteInstance.maxIndex ){
	      if(layer.hiddenAfterEnd && !layer.hidden){
		HideLayer(layer, true);
	      }
	    } else if( layer.hidden ) {
	      HideLayer(layer, false);
	    }
						
	    layer.spriteInstance.currentIndex = index;
	  }
					
	}
      }
    }
    private void Reset(){
      currentIndex = 0;
      currentTime = 0F;
      delayTimer = delay;
      loopCounter = loop;
    }
    private void HideLayer(SpriteLayer layer, bool yes){
      if(yes) {
	layer.hidden = true;
	layer.gameObject.transform.localScale = Vector3.zero;
      } else {
	layer.hidden = false;
	layer.gameObject.transform.localScale = Vector3.one;
      }
    }
    private void InitLayer(SpriteLayer layer){
			
      // 确保层 gameObject 存在，没有则生成
      Transform layerTransform = transform.FindChild(layer.name);
      if( layerTransform == null ){
	// game object of layer
	GameObject layerGo = new GameObject();
	layerGo.transform.parent = transform;
	layerGo.name = layer.name;
				
	layer.gameObject = layerGo;
			
      } else {
	layer.gameObject = layerTransform.gameObject;
	// 获取精灵实例
	if(layerTransform.childCount > 0)
	  {
	    Sprite sprite = null;
	    foreach(Transform child in layerTransform)
	      {
		sprite = child.GetComponent<Sprite>();
		if(sprite != null)
		  {
		    layer.spriteInstance = sprite;
		    break;
		  }
	      }
	  }
      }
      

      if( layer.spritePrefab != null )
	{
	  if( layer.spriteInstance != null )
	    {
	      InitLayerBySpriteInstance(layer);
	    }
	  else
	    {
	      // 初始化 sprite 部分
	      InitLayerBySprite(layer);
	      
	    }
	
	  // 计算动画最大索引
	  if( maxIndex < layer.maxIndex )
	    maxIndex = layer.maxIndex;
	  
	  // 该层是否需要隐藏
	  if( layer.hiddenBeforeBegin ) {
	    HideLayer(layer, true);
	  }
	
	}
      else
	{
	  HideLayer(layer, true);
	}
							
    }
    private void InitLayerBySprite(SpriteLayer layer){
						
      if( layer.spritePrefab != null ){
	Sprite sprite = Instantiate(layer.spritePrefab) as Sprite;
	layer.spriteInstance = sprite;
	sprite.transform.parent = layer.gameObject.transform;
	layer.maxIndex = sprite.maxIndex + layer.frameDelay;
	sprite.flipHorizontal = flipHorizontal;
	sprite.flipVertical = flipVertical;

      }
    }
    private void InitLayerBySpriteInstance(SpriteLayer layer)
    {
      if( layer.spriteInstance != null )
	{
	  Sprite sprite = layer.spriteInstance;
	  layer.maxIndex = sprite.maxIndex + layer.frameDelay;
	  sprite.flipHorizontal = flipHorizontal;
	  sprite.flipVertical = flipVertical;
	}
    }
    private void DestoryChildren(GameObject parentObject){
      foreach(Transform child in parentObject.transform){
	DestroyGobj(child.gameObject);
      }
    }
    private void DestroyGobj(GameObject gobj){
#if UNITY_EDITOR
      DestroyImmediate(gobj);
#else 
      Destroy(gobj);
#endif
    }
#endregion
		
#region Mono Methods
		
    void Start(){
			
			
      // init layers ---
			
      for(int i=0; i<layers.Length; i++){				
	InitLayer(layers[i]);				
      }
			
      // init fields ---
			
      currentIndex = currentIndex;
      flipHorizontal = flipHorizontal;
      flipVertical = flipVertical;
			
      // play on start
      if ( playOnStart )
	playing = true;
      else
	playing = false;
			
      loopCounter = loop;
      delayTimer = delay;
			
			
    }
		
		
    void Update(){
			
      if( playing && layers != null && layers.Length > 0 ) {
				
				
	if( delayTimer > 0F ) {
					
	  delayTimer -= Time.deltaTime;
					
	} else {
				
	  currentTime += Time.deltaTime;
	  int nextIndex = (int)(fps * currentTime);
					
	  if( currentIndex != nextIndex ){
						
	    currentIndex = nextIndex;
						
	    if( currentIndex == maxIndex ){
							
	      // 要播放到最后一帧了
							
	      if( loopCounter == 0 )
		Stop();
	      else if( loopCounter > 0)
		loopCounter--;
							
	      if( OnLastFrame != null)
		OnLastFrame(this);
							
	    } else if( currentIndex > maxIndex ){
							
	      // 超过最大帧，将转到第一帧
							
	      currentIndex = 0;
	      currentTime = 0F;
	    }
	  }
	}
      }
    }
		
#endregion
  }
}
