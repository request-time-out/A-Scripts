// Decompiled with JetBrains decompiler
// Type: AIChara.BustNormal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using OutputLogControl;
using System.Collections.Generic;
using UnityEngine;

namespace AIChara
{
  public class BustNormal
  {
    private Dictionary<GameObject, NormalData.Param> dictNormal = new Dictionary<GameObject, NormalData.Param>();
    private Dictionary<GameObject, SkinnedMeshRenderer> dictSmr = new Dictionary<GameObject, SkinnedMeshRenderer>();
    private Dictionary<GameObject, Vector3[]> dictCalc = new Dictionary<GameObject, Vector3[]>();
    private bool initEnd;
    private NormalData normal;

    public bool Init(
      GameObject objTarg,
      string assetBundleName,
      string assetName,
      string manifest)
    {
      this.initEnd = false;
      this.normal = CommonLib.LoadAsset<NormalData>(assetBundleName, assetName, true, manifest);
      if (Object.op_Equality((Object) null, (Object) this.normal))
        return false;
      Singleton<Character>.Instance.AddLoadAssetBundle(assetBundleName, string.Empty);
      this.dictNormal.Clear();
      this.dictSmr.Clear();
      for (int index = 0; index < this.normal.data.Count; ++index)
      {
        GameObject loop = objTarg.get_transform().FindLoop(this.normal.data[index].ObjectName);
        if (Object.op_Equality((Object) null, (Object) loop))
        {
          OutputLog.Error(assetName + "：法線情報が足りない" + this.normal.data[index].ObjectName, false, "CharaLoad");
        }
        else
        {
          SkinnedMeshRenderer component = (SkinnedMeshRenderer) loop.GetComponent<SkinnedMeshRenderer>();
          if (Object.op_Equality((Object) null, (Object) component))
          {
            OutputLog.Error(assetName + "：スキンメッシュがない" + this.normal.data[index].ObjectName, false, "CharaLoad");
          }
          else
          {
            this.dictSmr[loop] = component;
            if (Object.op_Inequality((Object) null, (Object) this.dictSmr[loop]) && Object.op_Inequality((Object) null, (Object) this.dictSmr[loop].get_sharedMesh()))
            {
              Mesh mesh = (Mesh) Object.Instantiate<Mesh>((M0) this.dictSmr[loop].get_sharedMesh());
              ((Object) mesh).set_name(((Object) this.dictSmr[loop].get_sharedMesh()).get_name());
              this.dictSmr[loop].set_sharedMesh(mesh);
            }
            this.dictNormal[loop] = this.normal.data[index];
            Vector3[] vector3Array = new Vector3[this.normal.data[index].NormalMin.Count];
            this.dictCalc[loop] = vector3Array;
          }
        }
      }
      this.CheckNormals(assetName);
      if (this.dictNormal.Count != 0)
        this.initEnd = true;
      return this.initEnd;
    }

    public void Release()
    {
      this.initEnd = false;
      this.normal = (NormalData) null;
      this.dictNormal.Clear();
    }

    private void CheckNormals(string assetName)
    {
      if (this.dictNormal == null)
        return;
      using (Dictionary<GameObject, NormalData.Param>.Enumerator enumerator = this.dictNormal.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<GameObject, NormalData.Param> current = enumerator.Current;
          if (current.Value.NormalMin.Count != current.Value.NormalMax.Count)
            OutputLog.Error("法線の数が違う：" + assetName + "  " + current.Value.ObjectName, false, "CharaLoad");
        }
      }
    }

    public void Blend(float rate)
    {
      if (!this.initEnd)
        return;
      using (Dictionary<GameObject, NormalData.Param>.Enumerator enumerator = this.dictNormal.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<GameObject, NormalData.Param> current = enumerator.Current;
          if (current.Value.NormalMin.Count == current.Value.NormalMax.Count && current.Value.NormalMin.Count == this.dictSmr[current.Key].get_sharedMesh().get_normals().Length)
          {
            for (int index = 0; index < current.Value.NormalMin.Count; ++index)
              this.dictCalc[current.Key][index] = Vector3.Lerp(current.Value.NormalMin[index], current.Value.NormalMax[index], rate);
            this.dictSmr[current.Key].get_sharedMesh().set_normals(this.dictCalc[current.Key]);
          }
        }
      }
    }
  }
}
