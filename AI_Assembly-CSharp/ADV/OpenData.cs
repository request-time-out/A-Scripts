// Decompiled with JetBrains decompiler
// Type: ADV.OpenData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace ADV
{
  [Serializable]
  public class OpenData
  {
    [SerializeField]
    private string _bundle = string.Empty;
    [SerializeField]
    private string _asset = string.Empty;
    private ScenarioData _data;

    public string bundle
    {
      get
      {
        return this._bundle;
      }
      set
      {
        this._bundle = value;
      }
    }

    public string asset
    {
      get
      {
        return this._asset;
      }
      set
      {
        this._asset = value;
      }
    }

    public ScenarioData data
    {
      get
      {
        return this._data;
      }
    }

    public void Set(OpenData openData)
    {
      this._asset = openData._asset;
      this._bundle = openData._bundle;
      this._data = openData._data;
    }

    public bool HasData
    {
      get
      {
        return Object.op_Inequality((Object) this._data, (Object) null);
      }
    }

    public void ClearData()
    {
      this._data = (ScenarioData) null;
    }

    public void Clear()
    {
      this._asset = (string) null;
      this._bundle = (string) null;
      this.ClearData();
    }

    public void Load()
    {
      this._data = AssetBundleManager.LoadAsset(this._bundle, this._asset, typeof (ScenarioData), (string) null).GetAsset<ScenarioData>();
      AssetBundleManager.UnloadAssetBundle(this._bundle, false, (string) null, false);
    }

    public void Load(string bundle, string asset)
    {
      bool flag = !this.HasData;
      if (this._asset != asset)
      {
        this._asset = asset;
        flag = true;
      }
      if (this._bundle != bundle)
      {
        this._bundle = bundle;
        flag = true;
      }
      if (!flag)
        return;
      this.Load();
    }

    public void FindLoad(string asset, string path)
    {
      this._asset = asset;
      this._bundle = Program.FindADVBundleFilePath(path, asset, out this._data);
    }

    public void FindLoad(string asset, int charaID, int category)
    {
      this._asset = asset;
      this._bundle = Program.FindADVBundleFilePath(charaID, category, asset, out this._data);
    }

    public void FindLoadMessage(string category, string asset)
    {
      this._asset = asset;
      this._bundle = Program.FindMessageADVBundleFilePath(category, asset, out this._data);
    }
  }
}
