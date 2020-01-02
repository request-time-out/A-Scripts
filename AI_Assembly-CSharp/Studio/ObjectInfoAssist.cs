// Decompiled with JetBrains decompiler
// Type: Studio.ObjectInfoAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Studio
{
  public static class ObjectInfoAssist
  {
    public static void LoadChild(
      BinaryReader _reader,
      Version _version,
      List<ObjectInfo> _list,
      bool _import)
    {
      int num1 = _reader.ReadInt32();
      for (int index = 0; index < num1; ++index)
      {
        int num2 = _reader.ReadInt32();
        switch (num2)
        {
          case 0:
            OICharInfo oiCharInfo = new OICharInfo((ChaFileControl) null, _import ? Studio.Studio.GetNewIndex() : -1);
            oiCharInfo.Load(_reader, _version, _import, true);
            _list.Add((ObjectInfo) oiCharInfo);
            break;
          case 1:
            OIItemInfo oiItemInfo = new OIItemInfo(-1, -1, -1, _import ? Studio.Studio.GetNewIndex() : -1);
            oiItemInfo.Load(_reader, _version, _import, true);
            _list.Add((ObjectInfo) oiItemInfo);
            break;
          case 2:
            OILightInfo oiLightInfo = new OILightInfo(-1, _import ? Studio.Studio.GetNewIndex() : -1);
            oiLightInfo.Load(_reader, _version, _import, true);
            _list.Add((ObjectInfo) oiLightInfo);
            break;
          case 3:
            OIFolderInfo oiFolderInfo = new OIFolderInfo(_import ? Studio.Studio.GetNewIndex() : -1);
            oiFolderInfo.Load(_reader, _version, _import, true);
            _list.Add((ObjectInfo) oiFolderInfo);
            break;
          case 4:
            OIRouteInfo oiRouteInfo = new OIRouteInfo(_import ? Studio.Studio.GetNewIndex() : -1);
            oiRouteInfo.Load(_reader, _version, _import, true);
            _list.Add((ObjectInfo) oiRouteInfo);
            break;
          case 5:
            OICameraInfo oiCameraInfo = new OICameraInfo(_import ? Studio.Studio.GetNewIndex() : -1);
            oiCameraInfo.Load(_reader, _version, _import, true);
            _list.Add((ObjectInfo) oiCameraInfo);
            break;
          default:
            Debug.LogWarning((object) string.Format("おかしい情報が入っている : {0}", (object) num2));
            break;
        }
      }
    }

    public static List<ObjectInfo> Find(int _kind)
    {
      List<ObjectInfo> _list = new List<ObjectInfo>();
      foreach (KeyValuePair<int, ObjectInfo> keyValuePair in Singleton<Studio.Studio>.Instance.sceneInfo.dicObject)
        ObjectInfoAssist.FindLoop(ref _list, keyValuePair.Value, _kind);
      return _list;
    }

    private static void FindLoop(ref List<ObjectInfo> _list, ObjectInfo _src, int _kind)
    {
      if (_src == null)
        return;
      if (_src.kind == _kind)
        _list.Add(_src);
      switch (_src.kind)
      {
        case 0:
          using (Dictionary<int, List<ObjectInfo>>.Enumerator enumerator = (_src as OICharInfo).child.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              foreach (ObjectInfo _src1 in enumerator.Current.Value)
                ObjectInfoAssist.FindLoop(ref _list, _src1, _kind);
            }
            break;
          }
        case 1:
          using (List<ObjectInfo>.Enumerator enumerator = (_src as OIItemInfo).child.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ObjectInfo current = enumerator.Current;
              ObjectInfoAssist.FindLoop(ref _list, current, _kind);
            }
            break;
          }
        case 3:
          using (List<ObjectInfo>.Enumerator enumerator = (_src as OIFolderInfo).child.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ObjectInfo current = enumerator.Current;
              ObjectInfoAssist.FindLoop(ref _list, current, _kind);
            }
            break;
          }
        case 4:
          using (List<ObjectInfo>.Enumerator enumerator = (_src as OIRouteInfo).child.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ObjectInfo current = enumerator.Current;
              ObjectInfoAssist.FindLoop(ref _list, current, _kind);
            }
            break;
          }
      }
    }
  }
}
