// Decompiled with JetBrains decompiler
// Type: AnimationKeyInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class AnimationKeyInfo
{
  private Dictionary<string, List<AnimationKeyInfo.AnmKeyInfo>> dictInfo = new Dictionary<string, List<AnimationKeyInfo.AnmKeyInfo>>();

  public int GetKeyCount()
  {
    return this.dictInfo == null || this.dictInfo.Count == 0 ? 0 : this.dictInfo.Values.ToList<List<AnimationKeyInfo.AnmKeyInfo>>()[0].Count;
  }

  public bool CreateInfo(int start, int end, GameObject obj, string[] usename)
  {
    if (Object.op_Equality((Object) null, (Object) obj))
      return false;
    Animator component = (Animator) obj.GetComponent<Animator>();
    if (Object.op_Equality((Object) null, (Object) component))
      return false;
    this.dictInfo.Clear();
    float num1 = 1f / (float) (end - start);
    int num2 = end - start + 1;
    for (int no = 0; no < num2; ++no)
    {
      float num3 = num1 * (float) no;
      Animator animator = component;
      AnimatorStateInfo animatorStateInfo = component.GetCurrentAnimatorStateInfo(0);
      int fullPathHash = ((AnimatorStateInfo) ref animatorStateInfo).get_fullPathHash();
      double num4 = (double) num3;
      animator.Play(fullPathHash, 0, (float) num4);
      component.Update(0.0f);
      this.CreateInfoLoop(no, obj.get_transform(), usename);
    }
    return true;
  }

  private void CreateInfoLoop(int no, Transform tf, string[] usename)
  {
    if (Object.op_Equality((Object) null, (Object) tf))
      return;
    List<AnimationKeyInfo.AnmKeyInfo> anmKeyInfoList = (List<AnimationKeyInfo.AnmKeyInfo>) null;
    bool flag = false;
    if (usename != null && Array.IndexOf<string>(usename, ((Object) tf).get_name()) == -1)
      flag = true;
    if (!flag)
    {
      if (!this.dictInfo.TryGetValue(((Object) tf).get_name(), out anmKeyInfoList))
      {
        anmKeyInfoList = new List<AnimationKeyInfo.AnmKeyInfo>();
        this.dictInfo[((Object) tf).get_name()] = anmKeyInfoList;
      }
      AnimationKeyInfo.AnmKeyInfo anmKeyInfo = new AnimationKeyInfo.AnmKeyInfo();
      anmKeyInfo.Set(no, tf.get_localPosition(), tf.get_localEulerAngles(), tf.get_localScale());
      anmKeyInfoList.Add(anmKeyInfo);
    }
    for (int index = 0; index < tf.get_childCount(); ++index)
    {
      Transform child = tf.GetChild(index);
      this.CreateInfoLoop(no, child, usename);
    }
  }

  public bool GetInfo(string name, float rate, ref Vector3 value, byte type)
  {
    List<AnimationKeyInfo.AnmKeyInfo> anmKeyInfoList = (List<AnimationKeyInfo.AnmKeyInfo>) null;
    if (!this.dictInfo.TryGetValue(name, out anmKeyInfoList))
    {
      Debug.Log((object) (name + "が見つからない"));
      return false;
    }
    switch (type)
    {
      case 0:
        if ((double) rate == 0.0)
        {
          value = anmKeyInfoList[0].pos;
          break;
        }
        if ((double) rate == 1.0)
        {
          value = anmKeyInfoList[anmKeyInfoList.Count - 1].pos;
          break;
        }
        float num1 = (float) (anmKeyInfoList.Count - 1) * rate;
        int index1 = Mathf.FloorToInt(num1);
        float num2 = num1 - (float) index1;
        value = Vector3.Lerp(anmKeyInfoList[index1].pos, anmKeyInfoList[index1 + 1].pos, num2);
        break;
      case 1:
        if ((double) rate == 0.0)
        {
          value = anmKeyInfoList[0].rot;
          break;
        }
        if ((double) rate == 1.0)
        {
          value = anmKeyInfoList[anmKeyInfoList.Count - 1].rot;
          break;
        }
        float num3 = (float) (anmKeyInfoList.Count - 1) * rate;
        int index2 = Mathf.FloorToInt(num3);
        float num4 = num3 - (float) index2;
        value.x = (__Null) (double) Mathf.LerpAngle((float) anmKeyInfoList[index2].rot.x, (float) anmKeyInfoList[index2 + 1].rot.x, num4);
        value.y = (__Null) (double) Mathf.LerpAngle((float) anmKeyInfoList[index2].rot.y, (float) anmKeyInfoList[index2 + 1].rot.y, num4);
        value.z = (__Null) (double) Mathf.LerpAngle((float) anmKeyInfoList[index2].rot.z, (float) anmKeyInfoList[index2 + 1].rot.z, num4);
        break;
      default:
        if ((double) rate == 0.0)
        {
          value = anmKeyInfoList[0].scl;
          break;
        }
        if ((double) rate == 1.0)
        {
          value = anmKeyInfoList[anmKeyInfoList.Count - 1].scl;
          break;
        }
        float num5 = (float) (anmKeyInfoList.Count - 1) * rate;
        int index3 = Mathf.FloorToInt(num5);
        float num6 = num5 - (float) index3;
        value = Vector3.Lerp(anmKeyInfoList[index3].scl, anmKeyInfoList[index3 + 1].scl, num6);
        break;
    }
    return true;
  }

  public bool GetInfo(string name, int key, ref Vector3 value, byte type)
  {
    List<AnimationKeyInfo.AnmKeyInfo> anmKeyInfoList = (List<AnimationKeyInfo.AnmKeyInfo>) null;
    if (!this.dictInfo.TryGetValue(name, out anmKeyInfoList))
    {
      Debug.Log((object) (name + "が見つからない"));
      return false;
    }
    if (anmKeyInfoList.Count <= key)
    {
      Debug.Log((object) (name + "：キーが範囲外"));
      return false;
    }
    switch (type)
    {
      case 0:
        value = anmKeyInfoList[key].pos;
        break;
      case 1:
        value = anmKeyInfoList[key].rot;
        break;
      default:
        value = anmKeyInfoList[key].scl;
        break;
    }
    return true;
  }

  public bool GetInfo(string name, float rate, ref Vector3[] value, bool[] flag)
  {
    if (value.Length != 3 || flag.Length != 3)
      return false;
    List<AnimationKeyInfo.AnmKeyInfo> anmKeyInfoList = (List<AnimationKeyInfo.AnmKeyInfo>) null;
    if (!this.dictInfo.TryGetValue(name, out anmKeyInfoList))
    {
      Debug.Log((object) (name + "が見つからない"));
      return false;
    }
    if (flag[0])
    {
      if ((double) rate == 0.0)
        value[0] = anmKeyInfoList[0].pos;
      else if ((double) rate == 1.0)
      {
        value[0] = anmKeyInfoList[anmKeyInfoList.Count - 1].pos;
      }
      else
      {
        float num1 = (float) (anmKeyInfoList.Count - 1) * rate;
        int index = Mathf.FloorToInt(num1);
        float num2 = num1 - (float) index;
        value[0] = Vector3.Lerp(anmKeyInfoList[index].pos, anmKeyInfoList[index + 1].pos, num2);
      }
    }
    if (flag[1])
    {
      if ((double) rate == 0.0)
        value[1] = anmKeyInfoList[0].rot;
      else if ((double) rate == 1.0)
      {
        value[1] = anmKeyInfoList[anmKeyInfoList.Count - 1].rot;
      }
      else
      {
        float num1 = (float) (anmKeyInfoList.Count - 1) * rate;
        int index = Mathf.FloorToInt(num1);
        float num2 = num1 - (float) index;
        value[1].x = (__Null) (double) Mathf.LerpAngle((float) anmKeyInfoList[index].rot.x, (float) anmKeyInfoList[index + 1].rot.x, num2);
        value[1].y = (__Null) (double) Mathf.LerpAngle((float) anmKeyInfoList[index].rot.y, (float) anmKeyInfoList[index + 1].rot.y, num2);
        value[1].z = (__Null) (double) Mathf.LerpAngle((float) anmKeyInfoList[index].rot.z, (float) anmKeyInfoList[index + 1].rot.z, num2);
      }
    }
    if (flag[2])
    {
      if ((double) rate == 0.0)
        value[2] = anmKeyInfoList[0].scl;
      else if ((double) rate == 1.0)
      {
        value[2] = anmKeyInfoList[anmKeyInfoList.Count - 1].scl;
      }
      else
      {
        float num1 = (float) (anmKeyInfoList.Count - 1) * rate;
        int index = Mathf.FloorToInt(num1);
        float num2 = num1 - (float) index;
        value[2] = Vector3.Lerp(anmKeyInfoList[index].scl, anmKeyInfoList[index + 1].scl, num2);
      }
    }
    return true;
  }

  public bool GetInfo(string name, int key, ref Vector3[] value, bool[] flag)
  {
    if (value.Length != 3 || flag.Length != 3)
      return false;
    List<AnimationKeyInfo.AnmKeyInfo> anmKeyInfoList = (List<AnimationKeyInfo.AnmKeyInfo>) null;
    if (!this.dictInfo.TryGetValue(name, out anmKeyInfoList))
    {
      Debug.Log((object) (name + "が見つからない"));
      return false;
    }
    if (anmKeyInfoList.Count <= key)
    {
      Debug.Log((object) (name + "：キーが範囲外"));
      return false;
    }
    if (flag[0])
      value[0] = anmKeyInfoList[key].pos;
    if (flag[1])
      value[1] = anmKeyInfoList[key].rot;
    if (flag[2])
      value[2] = anmKeyInfoList[key].scl;
    return true;
  }

  public void SaveInfo(string filepath)
  {
    using (FileStream fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
    {
      using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
      {
        int count = this.dictInfo.Count;
        binaryWriter.Write(count);
        foreach (KeyValuePair<string, List<AnimationKeyInfo.AnmKeyInfo>> keyValuePair in this.dictInfo)
        {
          binaryWriter.Write(keyValuePair.Key);
          binaryWriter.Write(keyValuePair.Value.Count);
          for (int index = 0; index < keyValuePair.Value.Count; ++index)
          {
            binaryWriter.Write(keyValuePair.Value[index].no);
            binaryWriter.Write((float) keyValuePair.Value[index].pos.x);
            binaryWriter.Write((float) keyValuePair.Value[index].pos.y);
            binaryWriter.Write((float) keyValuePair.Value[index].pos.z);
            binaryWriter.Write((float) keyValuePair.Value[index].rot.x);
            binaryWriter.Write((float) keyValuePair.Value[index].rot.y);
            binaryWriter.Write((float) keyValuePair.Value[index].rot.z);
            binaryWriter.Write((float) keyValuePair.Value[index].scl.x);
            binaryWriter.Write((float) keyValuePair.Value[index].scl.y);
            binaryWriter.Write((float) keyValuePair.Value[index].scl.z);
          }
        }
      }
    }
  }

  public void LoadInfo(string filePath)
  {
    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
      this.LoadInfo((Stream) fileStream);
  }

  public void LoadInfo(
    string manifest,
    string assetBundleName,
    string assetName,
    Action<string, string> funcAssetBundleEntry = null)
  {
    if (AssetBundleCheck.IsSimulation)
      manifest = string.Empty;
    if (!AssetBundleCheck.IsFile(assetBundleName, assetName))
    {
      Debug.LogError((object) ("読み込みエラー\r\nassetBundleName：" + assetBundleName + "\tassetName：" + assetName));
    }
    else
    {
      AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(assetBundleName, assetName, typeof (TextAsset), manifest);
      if (loadAssetOperation == null)
      {
        Debug.LogError((object) ("読み込みエラー\r\nassetName：" + assetName));
      }
      else
      {
        TextAsset asset = loadAssetOperation.GetAsset<TextAsset>();
        if (Object.op_Equality((Object) null, (Object) asset))
        {
          Debug.LogError((object) "ありえない");
        }
        else
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            memoryStream.Write(asset.get_bytes(), 0, asset.get_bytes().Length);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            this.LoadInfo((Stream) memoryStream);
          }
          if (funcAssetBundleEntry == null)
            AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
          else
            funcAssetBundleEntry(assetBundleName, string.Empty);
        }
      }
    }
  }

  public void LoadInfo(Stream st)
  {
    using (BinaryReader binaryReader = new BinaryReader(st))
    {
      int num1 = binaryReader.ReadInt32();
      this.dictInfo.Clear();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        List<AnimationKeyInfo.AnmKeyInfo> anmKeyInfoList = new List<AnimationKeyInfo.AnmKeyInfo>();
        this.dictInfo[binaryReader.ReadString()] = anmKeyInfoList;
        int num2 = binaryReader.ReadInt32();
        for (int index2 = 0; index2 < num2; ++index2)
          anmKeyInfoList.Add(new AnimationKeyInfo.AnmKeyInfo()
          {
            no = binaryReader.ReadInt32(),
            pos = {
              x = (__Null) (double) binaryReader.ReadSingle(),
              y = (__Null) (double) binaryReader.ReadSingle(),
              z = (__Null) (double) binaryReader.ReadSingle()
            },
            rot = {
              x = (__Null) (double) binaryReader.ReadSingle(),
              y = (__Null) (double) binaryReader.ReadSingle(),
              z = (__Null) (double) binaryReader.ReadSingle()
            },
            scl = {
              x = (__Null) (double) binaryReader.ReadSingle(),
              y = (__Null) (double) binaryReader.ReadSingle(),
              z = (__Null) (double) binaryReader.ReadSingle()
            }
          });
      }
    }
  }

  public void OutputText(string outputPath)
  {
    StringBuilder stringBuilder = new StringBuilder(2048);
    stringBuilder.Length = 0;
    foreach (KeyValuePair<string, List<AnimationKeyInfo.AnmKeyInfo>> keyValuePair in this.dictInfo)
    {
      for (int index = 0; index < keyValuePair.Value.Count; ++index)
      {
        stringBuilder.Append(keyValuePair.Key).Append("\t");
        stringBuilder.Append(keyValuePair.Value[index].GetInfoStr());
        stringBuilder.Append("\n");
      }
    }
    using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
    {
      using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream, Encoding.UTF8))
      {
        streamWriter.Write(stringBuilder.ToString());
        streamWriter.Write("\n");
      }
    }
  }

  public class AnmKeyInfo
  {
    public Vector3 pos = (Vector3) null;
    public Vector3 rot = (Vector3) null;
    public Vector3 scl = (Vector3) null;
    public int no;

    public void Set(int _no, Vector3 _pos, Vector3 _rot, Vector3 _scl)
    {
      this.no = _no;
      this.pos = _pos;
      this.rot = _rot;
      this.scl = _scl;
    }

    public string GetInfoStr()
    {
      StringBuilder stringBuilder = new StringBuilder(128);
      stringBuilder.Append(this.no.ToString()).Append("\t");
      stringBuilder.Append(((Vector3) ref this.pos).ToString("f7")).Append("\t");
      stringBuilder.Append(((Vector3) ref this.rot).ToString("f7")).Append("\t");
      stringBuilder.Append(((Vector3) ref this.scl).ToString("f7"));
      return stringBuilder.ToString();
    }
  }
}
