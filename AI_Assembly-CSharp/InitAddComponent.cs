// Decompiled with JetBrains decompiler
// Type: InitAddComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System.Diagnostics;
using UnityEngine;

public static class InitAddComponent
{
  public static void AddComponents(GameObject go)
  {
    go.AddComponent<GameSystem>();
    go.AddComponent<Sound>();
    go.AddComponent<SoundPlayer>();
    go.AddComponent<Manager.Config>();
    go.AddComponent<Manager.Voice>();
    go.AddComponent<Scene>();
    go.AddComponent<Character>();
    go.AddComponent<AnimalManager>();
    go.AddComponent<Manager.Map>();
    go.AddComponent<ADV>();
    go.AddComponent<Housing>();
    go.AddComponent<Game>();
    go.AddComponent<Input>();
    go.AddComponent<HSceneManager>();
  }

  [Conditional("__DEBUG_PROC__")]
  private static void DebugAddComponents(GameObject go)
  {
    go.AddComponent<DebugShow>();
    go.AddComponent<DebugRenderLog>();
  }
}
