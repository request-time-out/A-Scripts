// Decompiled with JetBrains decompiler
// Type: SmallGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public struct SmallGrid
{
  public int m_state;
  public int m_canRoof;
  public int[] m_itemkind;
  public int[] m_itemstackwall;
  public int[] m_itemdupulication;
  public bool m_inRoom;
  public bool m_UnderCarsol;
  public int m_PutFloatingPartsHeight;
  public List<int> m_PutElement;
  public Renderer m_color;
}
