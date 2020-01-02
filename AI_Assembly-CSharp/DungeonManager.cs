// Decompiled with JetBrains decompiler
// Type: DungeonManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[DefaultExecutionOrder(-200)]
public class DungeonManager : MonoBehaviour
{
  public int m_Width;
  public int m_Height;
  public float m_Spacing;
  public GameObject[] m_Tiles;

  public DungeonManager()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    Random.InitState(23431);
    int[] numArray = new int[this.m_Width * this.m_Height];
    for (int index1 = 0; index1 < this.m_Height; ++index1)
    {
      for (int index2 = 0; index2 < this.m_Width; ++index2)
      {
        bool flag1 = false;
        bool flag2 = false;
        if (index2 > 0)
          flag1 = (numArray[index2 - 1 + index1 * this.m_Width] & 1) != 0;
        if (index1 > 0)
          flag2 = (numArray[index2 + (index1 - 1) * this.m_Width] & 2) != 0;
        int num = 0;
        if (flag1)
          num |= 4;
        if (flag2)
          num |= 8;
        if (index2 + 1 < this.m_Width && (double) Random.get_value() > 0.5)
          num |= 1;
        if (index1 + 1 < this.m_Height && (double) Random.get_value() > 0.5)
          num |= 2;
        numArray[index2 + index1 * this.m_Width] = num;
      }
    }
    for (int index1 = 0; index1 < this.m_Height; ++index1)
    {
      for (int index2 = 0; index2 < this.m_Width; ++index2)
      {
        Vector3 vector3;
        ((Vector3) ref vector3).\u002Ector((float) index2 * this.m_Spacing, 0.0f, (float) index1 * this.m_Spacing);
        if (Object.op_Inequality((Object) this.m_Tiles[numArray[index2 + index1 * this.m_Width]], (Object) null))
          Object.Instantiate<GameObject>((M0) this.m_Tiles[numArray[index2 + index1 * this.m_Width]], vector3, Quaternion.get_identity());
      }
    }
  }
}
