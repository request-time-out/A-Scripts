// Decompiled with JetBrains decompiler
// Type: SpriteToParticlesAsset.LoadingTimeSpritesPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SpriteToParticlesAsset
{
  public class LoadingTimeSpritesPool : MonoBehaviour
  {
    [Tooltip("Drag here all the sprites to be loaded in the pool.")]
    public List<Sprite> spritesToLoad;
    [Tooltip("If enabled the load will be called on this GameObject’s Awake method. Otherwise it can be called by the method LoadAll() ")]
    public bool loadAllOnAwake;

    public LoadingTimeSpritesPool()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (!this.loadAllOnAwake)
        return;
      this.LoadAll();
    }

    public void LoadAll()
    {
      using (List<Sprite>.Enumerator enumerator = this.spritesToLoad.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Sprite current = enumerator.Current;
          Rect rect = current.get_rect();
          SpritesDataPool.GetSpriteColors(current, (int) ((Rect) ref rect).get_position().x, (int) ((Rect) ref rect).get_position().y, (int) ((Rect) ref rect).get_size().x, (int) ((Rect) ref rect).get_size().y);
        }
      }
    }
  }
}
