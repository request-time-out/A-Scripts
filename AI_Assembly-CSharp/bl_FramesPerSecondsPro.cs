// Decompiled with JetBrains decompiler
// Type: bl_FramesPerSecondsPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class bl_FramesPerSecondsPro : MonoBehaviour
{
  [SerializeField]
  private Text FPSText;
  private float deltaTime;

  public bl_FramesPerSecondsPro()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.FPSText, (Object) null))
      return;
    this.deltaTime += (float) (((double) Time.get_deltaTime() - (double) this.deltaTime) * 0.100000001490116);
    this.FPSText.set_text(string.Format("{0:0.0} ms ({1:0.} FPS)", (object) (this.deltaTime * 1000f), (object) (1f / this.deltaTime)).ToUpper());
  }
}
