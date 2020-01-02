// Decompiled with JetBrains decompiler
// Type: ContextSwitcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using DeepSky.Haze;
using System.Collections.Generic;
using UnityEngine;

public class ContextSwitcher : MonoBehaviour
{
  public List<DS_HazeContextAsset> contexts;
  private DS_HazeView _view;
  private int _contextIndex;

  public ContextSwitcher()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this._view = (DS_HazeView) ((Component) this).GetComponent<DS_HazeView>();
  }

  private void Update()
  {
    if (this.contexts.Count <= 0 || !Object.op_Inequality((Object) this._view, (Object) null) || !Input.GetKeyUp((KeyCode) 99))
      return;
    ++this._contextIndex;
    if (this._contextIndex == this.contexts.Count)
      this._contextIndex = 0;
    this._view.ContextAsset = this.contexts[this._contextIndex];
    this._view.OverrideContextAsset = true;
  }
}
