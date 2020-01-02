// Decompiled with JetBrains decompiler
// Type: CameraLookObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class CameraLookObject : MonoBehaviour
{
  public string targetCamera;

  public CameraLookObject()
  {
    base.\u002Ector();
  }

  private void OnBecameInvisible()
  {
    ((Behaviour) this).set_enabled(false);
  }

  private void OnBecameVisible()
  {
    ((Behaviour) this).set_enabled(true);
  }

  private void OnWillRenderObject()
  {
    Debug.Log((object) ((Object) Camera.get_current()).get_name());
  }
}
