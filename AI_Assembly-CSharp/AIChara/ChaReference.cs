// Decompiled with JetBrains decompiler
// Type: AIChara.ChaReference
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace AIChara
{
  public class ChaReference : BaseLoader
  {
    private Dictionary<int, GameObject> dictRefObj = new Dictionary<int, GameObject>();
    public const ulong FbxTypeBodyBone = 1;
    public const ulong FbxTypeHeadBone = 2;
    public const ulong FbxTypeInnerT = 7;
    public const ulong FbxTypeInnerB = 8;
    public const ulong FbxTypePanst = 10;

    public void Log_ReferenceObjectNull()
    {
      if (this.dictRefObj == null)
        return;
      using (Dictionary<int, GameObject>.Enumerator enumerator = this.dictRefObj.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, GameObject> current = enumerator.Current;
          if (!Object.op_Inequality((Object) null, (Object) current.Value))
            Debug.LogWarning((object) ("There is no " + current.Key.ToString() + "."));
        }
      }
    }

    public void CreateReferenceInfo(ulong flags, GameObject objRef)
    {
      this.ReleaseRefObject(flags);
      if (Object.op_Equality((Object) null, (Object) objRef))
        return;
      FindAssist findAssist = new FindAssist();
      findAssist.Initialize(objRef.get_transform());
      if ((long) flags >= 7L && (long) flags <= 10L)
      {
        switch (flags)
        {
          case 7:
            this.dictRefObj[5] = findAssist.GetObjectFromName("o_bra_a");
            this.dictRefObj[6] = findAssist.GetObjectFromName("o_bra_b");
            this.dictRefObj[7] = findAssist.GetObjectFromName("o_shorts_a");
            return;
          case 8:
            this.dictRefObj[8] = findAssist.GetObjectFromName("o_shorts_a");
            return;
          case 10:
            this.dictRefObj[9] = findAssist.GetObjectFromName("o_panst_a");
            return;
        }
      }
      if (flags == 1UL || flags == 2UL)
        ;
    }

    public void ReleaseRefObject(ulong flags)
    {
      if ((long) flags >= 7L && (long) flags <= 10L)
      {
        switch (flags)
        {
          case 7:
            this.dictRefObj.Remove(5);
            this.dictRefObj.Remove(6);
            this.dictRefObj.Remove(7);
            return;
          case 8:
            this.dictRefObj.Remove(8);
            return;
          case 10:
            this.dictRefObj.Remove(9);
            return;
        }
      }
      if (flags == 1UL || flags == 2UL)
        ;
    }

    public void ReleaseRefAll()
    {
      this.dictRefObj.Clear();
    }

    public GameObject GetReferenceInfo(ChaReference.RefObjKey key)
    {
      ChaControl chaControl = this as ChaControl;
      if (Object.op_Inequality((Object) null, (Object) chaControl))
      {
        switch (key)
        {
          case ChaReference.RefObjKey.HeadParent:
            if (Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody))
              return (GameObject) null;
            return Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody.targetEtc.trfHeadParent) ? (GameObject) null : ((Component) chaControl.cmpBoneBody.targetEtc.trfHeadParent).get_gameObject();
          case ChaReference.RefObjKey.k_f_shoulderL_00:
            if (Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody))
              return (GameObject) null;
            return Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody.targetEtc.trf_k_shoulderL_00) ? (GameObject) null : ((Component) chaControl.cmpBoneBody.targetEtc.trf_k_shoulderL_00).get_gameObject();
          case ChaReference.RefObjKey.k_f_shoulderR_00:
            if (Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody))
              return (GameObject) null;
            return Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody.targetEtc.trf_k_shoulderR_00) ? (GameObject) null : ((Component) chaControl.cmpBoneBody.targetEtc.trf_k_shoulderR_00).get_gameObject();
          case ChaReference.RefObjKey.k_f_handL_00:
            if (Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody))
              return (GameObject) null;
            return Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody.targetEtc.trf_k_handL_00) ? (GameObject) null : ((Component) chaControl.cmpBoneBody.targetEtc.trf_k_handL_00).get_gameObject();
          case ChaReference.RefObjKey.k_f_handR_00:
            if (Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody))
              return (GameObject) null;
            return Object.op_Equality((Object) null, (Object) chaControl.cmpBoneBody.targetEtc.trf_k_handR_00) ? (GameObject) null : ((Component) chaControl.cmpBoneBody.targetEtc.trf_k_handR_00).get_gameObject();
        }
      }
      GameObject gameObject = (GameObject) null;
      this.dictRefObj.TryGetValue((int) key, out gameObject);
      return gameObject;
    }

    public enum RefObjKey
    {
      HeadParent,
      k_f_shoulderL_00,
      k_f_shoulderR_00,
      k_f_handL_00,
      k_f_handR_00,
      mask_braA,
      mask_braB,
      mask_innerTB,
      mask_innerB,
      mask_panst,
    }
  }
}
