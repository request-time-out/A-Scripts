// Decompiled with JetBrains decompiler
// Type: Simple3D2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using Vectrosity;

public class Simple3D2 : MonoBehaviour
{
  public TextAsset vectorCube;

  public Simple3D2()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    VectorManager.ObjectSetup(((Component) this).get_gameObject(), new VectorLine(((Object) ((Component) this).get_gameObject()).get_name(), VectorLine.BytesToVector3List(this.vectorCube.get_bytes()), 2f), (Visibility) 0, (Brightness) 1);
  }
}
