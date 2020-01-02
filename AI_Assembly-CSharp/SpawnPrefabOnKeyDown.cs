// Decompiled with JetBrains decompiler
// Type: SpawnPrefabOnKeyDown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class SpawnPrefabOnKeyDown : MonoBehaviour
{
  public GameObject m_Prefab;
  public KeyCode m_KeyCode;

  public SpawnPrefabOnKeyDown()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if (!Input.GetKeyDown(this.m_KeyCode) || !Object.op_Inequality((Object) this.m_Prefab, (Object) null))
      return;
    Object.Instantiate<GameObject>((M0) this.m_Prefab, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation());
  }
}
