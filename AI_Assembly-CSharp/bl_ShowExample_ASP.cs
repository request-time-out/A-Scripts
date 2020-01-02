// Decompiled with JetBrains decompiler
// Type: bl_ShowExample_ASP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class bl_ShowExample_ASP : MonoBehaviour
{
  private bl_AllOptionsPro AllSettings;

  public bl_ShowExample_ASP()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.AllSettings = (bl_AllOptionsPro) Object.FindObjectOfType<bl_AllOptionsPro>();
  }

  private void Update()
  {
    if (!bl_Input.GetKeyDown("Pause"))
      return;
    this.AllSettings.ShowMenu();
  }
}
