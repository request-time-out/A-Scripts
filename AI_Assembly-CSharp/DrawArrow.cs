// Decompiled with JetBrains decompiler
// Type: DrawArrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DrawArrow : BaseLoader
{
  private List<DrawArrow.ArrowData> lstArrow = new List<DrawArrow.ArrowData>();
  private string[] bonenames = new string[32]
  {
    "cf_J_ArmUp01_s_R",
    "cf_J_Mune_Nip01_s_L",
    "cf_J_Mune_Nip01_s_R",
    "cf_J_Mune_Nip02_s_L",
    "cf_J_Mune_Nip02_s_R",
    "cf_J_Mune_Nipacs01_L",
    "cf_J_Mune_Nipacs01_R",
    "cf_J_Mune00_d_L",
    "cf_J_Mune00_d_R",
    "cf_J_Mune00_s_L",
    "cf_J_Mune00_s_R",
    "cf_J_Mune00_t_L",
    "cf_J_Mune00_t_R",
    "cf_J_Mune01_s_L",
    "cf_J_Mune01_s_R",
    "cf_J_Mune01_t_L",
    "cf_J_Mune01_t_R",
    "cf_J_Mune02_s_L",
    "cf_J_Mune02_s_R",
    "cf_J_Mune02_t_L",
    "cf_J_Mune02_t_R",
    "cf_J_Mune03_s_L",
    "cf_J_Mune03_s_R",
    "cf_J_Mune04_s_L",
    "cf_J_Mune04_s_R",
    "cf_J_Shoulder02_s_L",
    "cf_J_Shoulder02_s_R",
    "cf_J_sk_siri_dam",
    "cf_J_Spine01_s",
    "cf_J_Spine02_s",
    "cf_J_Spine03_s",
    "cf_N_height"
  };
  public Transform trfRef;
  public GameObject tmpArrow;
  [Button("Setup", "初期化", new object[] {})]
  public int setup;

  public void Setup()
  {
    if (Object.op_Equality((Object) null, (Object) this.trfRef) || Object.op_Equality((Object) null, (Object) this.tmpArrow))
      return;
    FindAssist findAssist = new FindAssist();
    findAssist.Initialize(this.trfRef);
    this.lstArrow.Clear();
    Transform transform = ((Component) this).get_transform().Find("top");
    if (Object.op_Implicit((Object) transform))
    {
      ((Object) transform).set_name("delete");
      Object.Destroy((Object) ((Component) transform).get_gameObject());
    }
    GameObject gameObject1 = new GameObject("top");
    gameObject1.get_transform().SetParent(((Component) this).get_transform(), false);
    for (int index = 0; index < this.bonenames.Length; ++index)
    {
      GameObject gameObject2;
      if (findAssist.dictObjName.TryGetValue(this.bonenames[index], out gameObject2))
      {
        GameObject gameObject3 = (GameObject) Object.Instantiate<GameObject>((M0) this.tmpArrow, gameObject1.get_transform(), false);
        ((Object) gameObject3).set_name(this.bonenames[index]);
        this.lstArrow.Add(new DrawArrow.ArrowData()
        {
          trfBone = gameObject2.get_transform(),
          trfArrow = gameObject3.get_transform()
        });
      }
    }
  }

  private void Reset()
  {
    this.Setup();
  }

  private void LateUpdate()
  {
    if (this.lstArrow == null)
      return;
    int count = this.lstArrow.Count;
    for (int index = 0; index < count; ++index)
    {
      this.lstArrow[index].trfArrow.set_position(this.lstArrow[index].trfBone.get_position());
      this.lstArrow[index].trfArrow.set_rotation(this.lstArrow[index].trfBone.get_rotation());
    }
  }

  public class ArrowData
  {
    public Transform trfBone;
    public Transform trfArrow;
  }
}
