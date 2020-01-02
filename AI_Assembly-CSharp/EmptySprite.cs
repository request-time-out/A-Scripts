// Decompiled with JetBrains decompiler
// Type: EmptySprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public static class EmptySprite
{
  private static Sprite instance;

  public static Sprite Get()
  {
    if (Object.op_Equality((Object) EmptySprite.instance, (Object) null))
      EmptySprite.instance = (Sprite) Resources.Load<Sprite>("procedural_ui_image_default_sprite");
    return EmptySprite.instance;
  }

  public static bool IsEmptySprite(Sprite s)
  {
    return Object.op_Equality((Object) EmptySprite.Get(), (Object) s);
  }
}
