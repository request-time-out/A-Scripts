// Decompiled with JetBrains decompiler
// Type: EnviroTerrainSeasonsChangeOrder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class EnviroTerrainSeasonsChangeOrder
{
  public Vector2 tiling = new Vector2(10f, 10f);
  public int terrainTextureID;
  [Header("Textures")]
  public Texture2D SpringTexture;
  public Texture2D SpringNormal;
  public Texture2D SummerTexture;
  public Texture2D SummerNormal;
  public Texture2D AutumnTexture;
  public Texture2D AutumnNormal;
  public Texture2D WinterTexture;
  public Texture2D WinterNormal;
}
