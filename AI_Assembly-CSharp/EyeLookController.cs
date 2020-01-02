// Decompiled with JetBrains decompiler
// Type: EyeLookController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class EyeLookController : MonoBehaviour
{
  public EyeLookCalc eyeLookScript;
  public int ptnNo;
  public Transform target;

  public EyeLookController()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Implicit((Object) this.target) || !Object.op_Implicit((Object) Camera.get_main()))
      return;
    this.target = ((Component) Camera.get_main()).get_transform();
  }

  private void LateUpdate()
  {
    if (!Object.op_Inequality((Object) this.target, (Object) null) || !Object.op_Inequality((Object) null, (Object) this.eyeLookScript))
      return;
    this.eyeLookScript.EyeUpdateCalc(this.target.get_position(), this.ptnNo);
  }

  public void ForceLateUpdate()
  {
    this.LateUpdate();
  }
}
