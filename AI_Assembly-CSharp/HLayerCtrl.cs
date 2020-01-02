// Decompiled with JetBrains decompiler
// Type: HLayerCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System.Collections.Generic;
using UnityEngine;

public class HLayerCtrl : MonoBehaviour
{
  private ChaControl[] chaFemales;
  private ChaControl[] chaMales;
  private HSceneFlagCtrl ctrlFlag;
  private ExcelData excelData;
  private AnimatorStateInfo stateInfo;
  private Dictionary<int, ChaControl[]> MapHchaFemales;
  private Dictionary<int, Dictionary<string, HLayerCtrl.HLayerInfo>[]> LayerInfos;
  private Dictionary<int, Dictionary<int, Dictionary<string, HLayerCtrl.HLayerInfo>[]>> MapHLayerInfos;

  public HLayerCtrl()
  {
    base.\u002Ector();
  }

  public void Init(ChaControl[] _chaFemales, ChaControl[] _chaMales)
  {
    this.ctrlFlag = Singleton<HSceneFlagCtrl>.Instance;
    this.chaFemales = _chaFemales;
    this.chaMales = _chaMales;
    this.LayerInfos = new Dictionary<int, Dictionary<string, HLayerCtrl.HLayerInfo>[]>();
    this.LayerInfos.Add(0, new Dictionary<string, HLayerCtrl.HLayerInfo>[2]);
    this.LayerInfos.Add(1, new Dictionary<string, HLayerCtrl.HLayerInfo>[2]);
    foreach (KeyValuePair<int, Dictionary<string, HLayerCtrl.HLayerInfo>[]> layerInfo in this.LayerInfos)
    {
      layerInfo.Value[0] = new Dictionary<string, HLayerCtrl.HLayerInfo>();
      layerInfo.Value[1] = new Dictionary<string, HLayerCtrl.HLayerInfo>();
    }
  }

  public int Init(ChaControl _chaFemale, HSceneFlagCtrl _ctrlFlag)
  {
    this.ctrlFlag = _ctrlFlag;
    if (this.MapHchaFemales == null)
      this.MapHchaFemales = new Dictionary<int, ChaControl[]>();
    int index = this.MapHchaFemales.Count;
    if (this.MapHchaFemales.ContainsKey(index))
    {
      int key = 0;
      while (this.MapHchaFemales.ContainsKey(key))
        ++key;
      index = key;
    }
    this.MapHchaFemales.Add(index, new ChaControl[2]
    {
      _chaFemale,
      null
    });
    if (this.MapHLayerInfos == null)
      this.MapHLayerInfos = new Dictionary<int, Dictionary<int, Dictionary<string, HLayerCtrl.HLayerInfo>[]>>();
    this.MapHLayerInfos.Add(index, new Dictionary<int, Dictionary<string, HLayerCtrl.HLayerInfo>[]>());
    this.MapHLayerInfos[index].Add(1, new Dictionary<string, HLayerCtrl.HLayerInfo>[2]);
    this.MapHLayerInfos[index][1][0] = new Dictionary<string, HLayerCtrl.HLayerInfo>();
    this.ctrlFlag.AddMapSyncAnimLayer(index);
    return index;
  }

  public void Release()
  {
    foreach (KeyValuePair<int, Dictionary<string, HLayerCtrl.HLayerInfo>[]> layerInfo in this.LayerInfos)
    {
      foreach (Dictionary<string, HLayerCtrl.HLayerInfo> dictionary in layerInfo.Value)
        dictionary?.Clear();
    }
    this.LayerInfos.Clear();
    this.chaFemales = (ChaControl[]) null;
    this.chaMales = (ChaControl[]) null;
  }

  public void MapHLayerRemove(int _id)
  {
    if (this.MapHLayerInfos.ContainsKey(_id))
    {
      foreach (KeyValuePair<int, Dictionary<string, HLayerCtrl.HLayerInfo>[]> keyValuePair in this.MapHLayerInfos[_id])
      {
        foreach (Dictionary<string, HLayerCtrl.HLayerInfo> dictionary in keyValuePair.Value)
          dictionary?.Clear();
      }
      this.MapHLayerInfos[_id].Clear();
      this.MapHLayerInfos.Remove(_id);
    }
    if (this.chaFemales != null)
      this.chaFemales = (ChaControl[]) null;
    if (this.chaMales != null)
      this.chaMales = (ChaControl[]) null;
    if (this.MapHchaFemales.ContainsKey(_id))
    {
      this.MapHchaFemales[_id] = (ChaControl[]) null;
      this.MapHchaFemales.Remove(_id);
    }
    this.ctrlFlag.RemoveMapSyncAnimLayer(_id);
  }

  public void LoadExcel(string animatorName, int _sex, int _id, bool mapH = false, int mapHID = 0)
  {
    Dictionary<string, HLayerCtrl.HLayerInfo> dictionary = (Dictionary<string, HLayerCtrl.HLayerInfo>) null;
    if (!Singleton<Resources>.Instance.HSceneTable.LayerInfos.TryGetValue(animatorName, out dictionary))
      dictionary = new Dictionary<string, HLayerCtrl.HLayerInfo>();
    if (!mapH)
    {
      this.LayerInfos[_sex][_id].Clear();
      foreach (KeyValuePair<string, HLayerCtrl.HLayerInfo> keyValuePair in dictionary)
        this.LayerInfos[_sex][_id].Add(keyValuePair.Key, keyValuePair.Value);
    }
    else
    {
      this.MapHLayerInfos[mapHID][_sex][_id].Clear();
      foreach (KeyValuePair<string, HLayerCtrl.HLayerInfo> keyValuePair in dictionary)
        this.MapHLayerInfos[mapHID][_sex][_id].Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  private void LateUpdate()
  {
    if (this.chaFemales == null || this.chaFemales.Length <= 0 || Object.op_Equality((Object) this.chaFemales[0], (Object) null))
      return;
    this.setLayer(this.chaFemales, 1, -1);
    this.setLayer(this.chaMales, 0, -1);
  }

  public void MapHProc(int ID)
  {
    if (this.MapHchaFemales == null || this.MapHchaFemales.Count <= 0 || this.MapHchaFemales[ID] == null)
      return;
    this.setLayer(this.MapHchaFemales[ID], 1, ID);
  }

  private void setLayer(ChaControl[] charas, int Sex, int _mapHID = -1)
  {
    for (int index = 0; index < 2; ++index)
    {
      if (!Object.op_Equality((Object) charas[index], (Object) null) && !Object.op_Equality((Object) charas[index].animBody, (Object) null) && !Object.op_Equality((Object) charas[index].animBody.get_runtimeAnimatorController(), (Object) null))
      {
        this.stateInfo = charas[index].getAnimatorStateInfo(0);
        bool flag = false;
        if (_mapHID == -1)
        {
          foreach (string key in this.LayerInfos[Sex][index].Keys)
          {
            if (((AnimatorStateInfo) ref this.stateInfo).IsName(key))
            {
              flag = true;
              int layerId = this.LayerInfos[Sex][index][key].LayerID;
              float weight = this.LayerInfos[Sex][index][key].weight;
              this.setLayer(charas, Sex, index, layerId, weight, -1);
            }
          }
        }
        else
        {
          foreach (string key in this.MapHLayerInfos[_mapHID][Sex][index].Keys)
          {
            if (((AnimatorStateInfo) ref this.stateInfo).IsName(key))
            {
              flag = true;
              int layerId = this.MapHLayerInfos[_mapHID][Sex][index][key].LayerID;
              float weight = this.MapHLayerInfos[_mapHID][Sex][index][key].weight;
              this.setLayer(charas, Sex, index, layerId, weight, _mapHID);
            }
          }
        }
        if (!flag)
        {
          if (_mapHID == -1)
          {
            for (int _nLayer = 1; _nLayer < charas[index].animBody.get_layerCount(); ++_nLayer)
            {
              charas[index].setLayerWeight(0.0f, _nLayer);
              this.ctrlFlag.lstSyncAnimLayers[Sex, index].Remove(_nLayer);
            }
          }
          else
          {
            for (int _nLayer = 1; _nLayer < charas[index].animBody.get_layerCount(); ++_nLayer)
            {
              charas[index].setLayerWeight(0.0f, _nLayer);
              this.ctrlFlag.lstMapSyncAnimLayers[_mapHID].Remove(_nLayer);
            }
          }
        }
      }
    }
  }

  private void setLayer(
    ChaControl[] charas,
    int sex,
    int index,
    int layer,
    float weight,
    int _mapHID = -1)
  {
    if (layer != 0)
    {
      List<int> intList = _mapHID != -1 ? this.ctrlFlag.lstMapSyncAnimLayers[_mapHID] : this.ctrlFlag.lstSyncAnimLayers[sex, index];
      if (!intList.Contains(layer))
      {
        for (int _nLayer = 1; _nLayer < charas[index].animBody.get_layerCount(); ++_nLayer)
        {
          if (layer == _nLayer)
            charas[index].setLayerWeight(weight, layer);
          else
            charas[index].setLayerWeight(0.0f, _nLayer);
        }
        intList.Add(layer);
      }
      else
      {
        if ((double) weight != 0.0)
          return;
        charas[index].setLayerWeight(0.0f, layer);
        intList.Remove(layer);
      }
    }
    else
    {
      for (int _nLayer = 1; _nLayer < charas[index].animBody.get_layerCount(); ++_nLayer)
        charas[index].setLayerWeight(0.0f, _nLayer);
    }
  }

  public struct HLayerInfo
  {
    public int LayerID;
    public float weight;
  }
}
