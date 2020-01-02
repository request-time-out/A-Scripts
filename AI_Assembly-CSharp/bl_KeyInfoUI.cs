// Decompiled with JetBrains decompiler
// Type: bl_KeyInfoUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class bl_KeyInfoUI : MonoBehaviour
{
  [SerializeField]
  private Text FunctionText;
  [SerializeField]
  private Text KeyText;
  private bl_KeyInfo cacheInfo;
  private bl_KeyOptionsUI KeyOptions;

  public bl_KeyInfoUI()
  {
    base.\u002Ector();
  }

  public void Init(bl_KeyInfo info, bl_KeyOptionsUI koui)
  {
    this.cacheInfo = info;
    this.KeyOptions = koui;
    this.FunctionText.set_text(info.Description);
    this.KeyText.set_text(info.Key.ToString());
  }

  public void SetKeyChange()
  {
    this.KeyOptions.SetWaitKeyProcess(this.cacheInfo);
  }
}
