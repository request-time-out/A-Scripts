// Decompiled with JetBrains decompiler
// Type: FinalIKAfter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class FinalIKAfter : MonoBehaviour
{
  public GameObject objUpdateMeta;
  [TagSelector]
  public string sss;
  private UpdateMeta updateMeta;

  public FinalIKAfter()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (!Object.op_Implicit((Object) this.objUpdateMeta))
      return;
    this.updateMeta = (UpdateMeta) this.objUpdateMeta.GetComponent<UpdateMeta>();
  }

  private void Update()
  {
  }

  private void LateUpdate()
  {
    if (!Object.op_Inequality((Object) this.updateMeta, (Object) null))
      return;
    this.updateMeta.ConstMetaMesh();
  }
}
