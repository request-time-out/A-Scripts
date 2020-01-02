// Decompiled with JetBrains decompiler
// Type: MorphBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MorphBase
{
  public const int morphFilesVersion = 100;
  public MorphCalcInfo[] CalcInfo;

  public int GetMaxPtn()
  {
    return this.CalcInfo.Length == 0 ? 0 : this.CalcInfo[0].UpdateInfo.Length / 2;
  }

  protected bool CreateCalcInfo(GameObject obj)
  {
    if (Object.op_Equality((Object) null, (Object) obj))
      return false;
    MorphSetting component1 = (MorphSetting) obj.GetComponent("MorphSetting");
    if (Object.op_Equality((Object) null, (Object) component1))
      return false;
    this.CalcInfo = (MorphCalcInfo[]) null;
    GC.Collect();
    this.CalcInfo = new MorphCalcInfo[component1.MorphDataList.Count];
    int index1 = 0;
    foreach (MorphData morphData in component1.MorphDataList)
    {
      if (!Object.op_Equality((Object) null, (Object) morphData.TargetObj))
      {
        this.CalcInfo[index1] = new MorphCalcInfo();
        this.CalcInfo[index1].TargetObj = morphData.TargetObj;
        MeshFilter meshFilter = new MeshFilter();
        MeshFilter component2 = morphData.TargetObj.GetComponent(typeof (MeshFilter)) as MeshFilter;
        if (Object.op_Implicit((Object) component2))
        {
          this.CalcInfo[index1].OriginalMesh = component2.get_sharedMesh();
          this.CalcInfo[index1].OriginalPos = component2.get_sharedMesh().get_vertices();
          this.CalcInfo[index1].OriginalNormal = component2.get_sharedMesh().get_normals();
          this.CalcInfo[index1].WeightFlags = false;
        }
        else
        {
          SkinnedMeshRenderer skinnedMeshRenderer = new SkinnedMeshRenderer();
          SkinnedMeshRenderer component3 = morphData.TargetObj.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
          this.CalcInfo[index1].OriginalMesh = component3.get_sharedMesh();
          this.CalcInfo[index1].OriginalPos = component3.get_sharedMesh().get_vertices();
          this.CalcInfo[index1].OriginalNormal = component3.get_sharedMesh().get_normals();
          this.CalcInfo[index1].WeightFlags = true;
        }
        int length1;
        if (Object.op_Equality((Object) null, (Object) morphData.MorphArea))
        {
          length1 = this.CalcInfo[index1].OriginalMesh.get_vertices().Length;
          this.CalcInfo[index1].UpdateIndex = new int[length1];
          for (int index2 = 0; index2 < length1; ++index2)
            this.CalcInfo[index1].UpdateIndex[index2] = index2;
        }
        else if (morphData.MorphArea.get_colors().Length != 0)
        {
          List<int> source = new List<int>();
          // ISSUE: object of a compiler-generated type is created
          using (IEnumerator<\u003C\u003E__AnonType10<Color, int>> enumerator = ((IEnumerable<Color>) morphData.MorphArea.get_colors()).Select<Color, \u003C\u003E__AnonType10<Color, int>>((Func<Color, int, \u003C\u003E__AnonType10<Color, int>>) ((value, index) => new \u003C\u003E__AnonType10<Color, int>(value, index))).GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              // ISSUE: variable of a compiler-generated type
              \u003C\u003E__AnonType10<Color, int> current = enumerator.Current;
              if (Color.op_Equality(current.value, morphData.AreaColor))
                source.Add(current.index);
            }
          }
          this.CalcInfo[index1].UpdateIndex = new int[source.Count];
          // ISSUE: object of a compiler-generated type is created
          foreach (\u003C\u003E__AnonType10<int, int> anonType10 in source.Select<int, \u003C\u003E__AnonType10<int, int>>((Func<int, int, \u003C\u003E__AnonType10<int, int>>) ((value, index) => new \u003C\u003E__AnonType10<int, int>(value, index))))
            this.CalcInfo[index1].UpdateIndex[anonType10.index] = anonType10.value;
          length1 = source.Count;
        }
        else
        {
          length1 = this.CalcInfo[index1].OriginalMesh.get_vertices().Length;
          this.CalcInfo[index1].UpdateIndex = new int[length1];
          for (int index2 = 0; index2 < length1; ++index2)
            this.CalcInfo[index1].UpdateIndex[index2] = index2;
        }
        int length2 = morphData.MorphMesh.Length;
        this.CalcInfo[index1].UpdateInfo = new MorphUpdateInfo[length2];
        for (int index2 = 0; index2 < length2; ++index2)
        {
          this.CalcInfo[index1].UpdateInfo[index2] = new MorphUpdateInfo();
          this.CalcInfo[index1].UpdateInfo[index2].Pos = new Vector3[length1];
          this.CalcInfo[index1].UpdateInfo[index2].Normmal = new Vector3[length1];
          if (Object.op_Equality((Object) null, (Object) morphData.MorphMesh[index2]))
          {
            for (int index3 = 0; index3 < length1; ++index3)
            {
              this.CalcInfo[index1].UpdateInfo[index2].Pos[index3] = this.CalcInfo[index1].OriginalMesh.get_vertices()[this.CalcInfo[index1].UpdateIndex[index3]];
              this.CalcInfo[index1].UpdateInfo[index2].Normmal[index3] = this.CalcInfo[index1].OriginalMesh.get_normals()[this.CalcInfo[index1].UpdateIndex[index3]];
            }
          }
          else
          {
            for (int index3 = 0; index3 < length1; ++index3)
            {
              this.CalcInfo[index1].UpdateInfo[index2].Pos[index3] = morphData.MorphMesh[index2].get_vertices()[this.CalcInfo[index1].UpdateIndex[index3]];
              this.CalcInfo[index1].UpdateInfo[index2].Normmal[index3] = morphData.MorphMesh[index2].get_normals()[this.CalcInfo[index1].UpdateIndex[index3]];
            }
          }
        }
        ++index1;
      }
    }
    return true;
  }

  protected bool ChangeRefTargetMesh(List<MorphingTargetInfo> MorphTargetList)
  {
    foreach (MorphCalcInfo morphCalcInfo in this.CalcInfo)
    {
      if (!Object.op_Equality((Object) null, (Object) morphCalcInfo.OriginalMesh))
      {
        Mesh mesh = (Mesh) null;
        foreach (MorphingTargetInfo morphTarget in MorphTargetList)
        {
          if (Object.op_Equality((Object) morphTarget.TargetObj, (Object) morphCalcInfo.TargetObj))
          {
            mesh = morphTarget.TargetMesh;
            break;
          }
        }
        if (Object.op_Implicit((Object) mesh))
        {
          morphCalcInfo.TargetMesh = mesh;
        }
        else
        {
          MorphCloneMesh.Clone(out morphCalcInfo.TargetMesh, morphCalcInfo.OriginalMesh);
          ((Object) morphCalcInfo.TargetMesh).set_name(((Object) morphCalcInfo.OriginalMesh).get_name());
          MorphTargetList.Add(new MorphingTargetInfo()
          {
            TargetMesh = morphCalcInfo.TargetMesh,
            TargetObj = morphCalcInfo.TargetObj
          });
        }
        if (morphCalcInfo.WeightFlags)
        {
          SkinnedMeshRenderer skinnedMeshRenderer = new SkinnedMeshRenderer();
          (morphCalcInfo.TargetObj.GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer).set_sharedMesh(morphCalcInfo.TargetMesh);
        }
        else
        {
          MeshFilter meshFilter = new MeshFilter();
          (morphCalcInfo.TargetObj.GetComponent(typeof (MeshFilter)) as MeshFilter).set_sharedMesh(morphCalcInfo.TargetMesh);
        }
      }
    }
    return true;
  }
}
