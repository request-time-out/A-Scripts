// Decompiled with JetBrains decompiler
// Type: SpriteToParticlesAsset.SpritesDataPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SpriteToParticlesAsset
{
  public class SpritesDataPool
  {
    private static Dictionary<Sprite, Color[]> spritesShared = new Dictionary<Sprite, Color[]>();

    public static Color[] GetSpriteColors(
      Sprite sprite,
      int x,
      int y,
      int blockWidth,
      int blockHeight)
    {
      if (SpritesDataPool.spritesShared == null)
        SpritesDataPool.spritesShared = new Dictionary<Sprite, Color[]>();
      Color[] pixels;
      if (!SpritesDataPool.spritesShared.ContainsKey(sprite))
      {
        pixels = sprite.get_texture().GetPixels(x, y, blockWidth, blockHeight);
        SpritesDataPool.spritesShared.Add(sprite, pixels);
      }
      else
        pixels = SpritesDataPool.spritesShared[sprite];
      return pixels;
    }

    public static void ReleaseMemory()
    {
      SpritesDataPool.spritesShared = (Dictionary<Sprite, Color[]>) null;
    }
  }
}
