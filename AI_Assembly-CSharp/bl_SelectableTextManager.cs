// Decompiled with JetBrains decompiler
// Type: bl_SelectableTextManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class bl_SelectableTextManager : MonoBehaviour
{
  private bl_SelectableText[] AllSelectables;

  public bl_SelectableTextManager()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.AllSelectables = (bl_SelectableText[]) ((Component) this).GetComponentsInChildren<bl_SelectableText>();
  }

  public void OnEnter()
  {
    if (this.AllSelectables.Length <= 0)
      return;
    foreach (bl_SelectableText allSelectable in this.AllSelectables)
      allSelectable.OnEnter();
  }

  public void OnExit()
  {
    if (this.AllSelectables.Length <= 0)
      return;
    foreach (bl_SelectableText allSelectable in this.AllSelectables)
      allSelectable.OnExit();
  }
}
