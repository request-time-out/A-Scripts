// Decompiled with JetBrains decompiler
// Type: LookAtCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
  public Camera lookAtCamera;
  public bool lookOnlyOnAwake;

  public LookAtCamera()
  {
    base.\u002Ector();
  }

  public void Start()
  {
    if (Object.op_Equality((Object) this.lookAtCamera, (Object) null))
      this.lookAtCamera = Camera.get_main();
    if (!this.lookOnlyOnAwake)
      return;
    this.LookCam();
  }

  public void Update()
  {
    if (this.lookOnlyOnAwake)
      return;
    this.LookCam();
  }

  public void LookCam()
  {
    ((Component) this).get_transform().LookAt(((Component) this.lookAtCamera).get_transform());
  }
}
