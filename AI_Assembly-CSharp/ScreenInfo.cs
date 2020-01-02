// Decompiled with JetBrains decompiler
// Type: ScreenInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public static class ScreenInfo
{
  public static float GetBaseScreenWidth()
  {
    return 1280f;
  }

  public static float GetBaseScreenHeight()
  {
    return 720f;
  }

  public static Vector2 GetScreenSize()
  {
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector((float) Screen.get_width(), (float) Screen.get_height());
    return vector2;
  }

  public static float GetSpriteRate()
  {
    return (float) (1.0 / (3.59999990463257 * ((double) Screen.get_height() / (720.0 * ((double) Screen.get_width() / 1280.0)))));
  }

  public static float GetSpriteCorrectY()
  {
    float width = (float) Screen.get_width();
    float height = (float) Screen.get_height();
    return (float) (((double) height - (double) width / 1280.0 * 720.0) * (2.0 / (double) height) * 0.5);
  }

  public static float GetScreenRate()
  {
    return (float) Screen.get_width() / 1280f;
  }

  public static float GetScreenCorrectY()
  {
    return (float) (((double) Screen.get_height() - (double) Screen.get_width() / 1280.0 * 720.0) * 0.5);
  }
}
