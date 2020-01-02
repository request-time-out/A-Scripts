// Decompiled with JetBrains decompiler
// Type: Manager.Character
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using OutputLogControl;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
  public class Character : Singleton<Character>
  {
    public List<AssetBundleData> lstLoadAssetBundleInfo = new List<AssetBundleData>();
    public bool loadAssetBundle;

    public ChaListControl chaListCtrl { get; private set; }

    public SortedDictionary<int, ChaControl> dictEntryChara { get; private set; }

    public bool enableCharaLoadGCClear { get; set; } = true;

    public bool customLoadGCClear { get; set; }

    public bool isMod { get; set; }

    private Material matHairDithering { get; set; }

    public Shader shaderDithering { get; private set; }

    private Material matHairCutout { get; set; }

    public Shader shaderCutout { get; private set; }

    public void BeginLoadAssetBundle()
    {
      this.lstLoadAssetBundleInfo.Clear();
      this.loadAssetBundle = true;
    }

    public void AddLoadAssetBundle(string assetBundleName, string manifestName)
    {
      if (manifestName.IsNullOrEmpty())
        manifestName = "abdata";
      AssetBundleManifestData bundleManifestData = new AssetBundleManifestData();
      bundleManifestData.bundle = assetBundleName;
      bundleManifestData.manifest = manifestName;
      this.lstLoadAssetBundleInfo.Add((AssetBundleData) bundleManifestData);
    }

    public void EndLoadAssetBundle(bool forceRemove = false)
    {
      foreach (AssetBundleData assetBundleData in this.lstLoadAssetBundleInfo)
        AssetBundleManager.UnloadAssetBundle(assetBundleData.bundle, true, (string) null, forceRemove);
      OutputLog.Log(nameof (EndLoadAssetBundle), false, "UnloadUnusedAssets");
      Resources.UnloadUnusedAssets();
      GC.Collect();
      this.lstLoadAssetBundleInfo.Clear();
      this.loadAssetBundle = false;
    }

    public ChaControl CreateChara(
      byte _sex,
      GameObject parent,
      int id,
      ChaFileControl _chaFile = null)
    {
      int num1 = 0;
      int num2 = 1;
      foreach (KeyValuePair<int, ChaControl> keyValuePair in this.dictEntryChara)
      {
        if ((int) keyValuePair.Value.sex == (int) _sex)
          ++num2;
        if (num1 == keyValuePair.Key)
          ++num1;
        else
          break;
      }
      GameObject _objRoot = new GameObject((_sex != (byte) 0 ? "chaF_" : "chaM_") + num2.ToString("000"));
      if (Object.op_Implicit((Object) parent))
        _objRoot.get_transform().SetParent(parent.get_transform(), false);
      ChaControl chaControl = (ChaControl) _objRoot.AddComponent<ChaControl>();
      if (Object.op_Implicit((Object) chaControl))
        chaControl.Initialize(_sex, _objRoot, id, num1, _chaFile);
      this.dictEntryChara.Add(num1, chaControl);
      return chaControl;
    }

    public bool IsChara(ChaControl cha)
    {
      foreach (KeyValuePair<int, ChaControl> keyValuePair in this.dictEntryChara)
      {
        if (Object.op_Equality((Object) keyValuePair.Value, (Object) cha))
          return true;
      }
      return false;
    }

    public bool DeleteChara(ChaControl cha, bool entryOnly = false)
    {
      foreach (KeyValuePair<int, ChaControl> keyValuePair in this.dictEntryChara)
      {
        if (Object.op_Equality((Object) keyValuePair.Value, (Object) cha))
        {
          if (!entryOnly)
          {
            ((Object) ((Component) keyValuePair.Value).get_gameObject()).set_name("Delete_Reserve");
            ((Component) keyValuePair.Value).get_transform().SetParent((Transform) null);
            Object.Destroy((Object) ((Component) keyValuePair.Value).get_gameObject());
          }
          this.dictEntryChara.Remove(keyValuePair.Key);
          return true;
        }
      }
      return false;
    }

    public void DeleteCharaAll()
    {
      foreach (KeyValuePair<int, ChaControl> keyValuePair in this.dictEntryChara)
      {
        if (Object.op_Implicit((Object) keyValuePair.Value))
        {
          ((Object) ((Component) keyValuePair.Value).get_gameObject()).set_name("Delete_Reserve");
          ((Component) keyValuePair.Value).get_transform().SetParent((Transform) null);
          Object.Destroy((Object) ((Component) keyValuePair.Value).get_gameObject());
        }
      }
      this.dictEntryChara.Clear();
    }

    public List<ChaControl> GetCharaList(byte _sex)
    {
      return this.dictEntryChara.Where<KeyValuePair<int, ChaControl>>((Func<KeyValuePair<int, ChaControl>, bool>) (c => (int) c.Value.sex == (int) _sex)).Select<KeyValuePair<int, ChaControl>, ChaControl>((Func<KeyValuePair<int, ChaControl>, ChaControl>) (x => x.Value)).ToList<ChaControl>();
    }

    public ChaControl GetChara(byte _sex, int _id)
    {
      try
      {
        return this.dictEntryChara.Where<KeyValuePair<int, ChaControl>>((Func<KeyValuePair<int, ChaControl>, bool>) (s => (int) s.Value.sex == (int) _sex)).First<KeyValuePair<int, ChaControl>>((Func<KeyValuePair<int, ChaControl>, bool>) (v => v.Value.chaID == _id)).Value;
      }
      catch (ArgumentNullException ex)
      {
      }
      catch (InvalidOperationException ex)
      {
      }
      return (ChaControl) null;
    }

    public ChaControl GetChara(int _id)
    {
      try
      {
        return this.dictEntryChara.First<KeyValuePair<int, ChaControl>>((Func<KeyValuePair<int, ChaControl>, bool>) (v => v.Value.chaID == _id)).Value;
      }
      catch (ArgumentNullException ex)
      {
      }
      catch (InvalidOperationException ex)
      {
      }
      return (ChaControl) null;
    }

    public ChaControl GetCharaFromLoadNo(int _no)
    {
      try
      {
        return this.dictEntryChara.First<KeyValuePair<int, ChaControl>>((Func<KeyValuePair<int, ChaControl>, bool>) (v => v.Value.loadNo == _no)).Value;
      }
      catch (ArgumentNullException ex)
      {
      }
      catch (InvalidOperationException ex)
      {
      }
      return (ChaControl) null;
    }

    public static void ChangeRootParent(ChaControl cha, Transform trfNewParent)
    {
      if (!Object.op_Inequality((Object) null, (Object) cha))
        return;
      ((Component) cha).get_transform().SetParent(trfNewParent, false);
    }

    public string GetCharaTypeName(int no)
    {
      VoiceInfo.Param obj;
      return !Singleton<Voice>.IsInstance() || !Singleton<Voice>.Instance.voiceInfoDic.TryGetValue(no, out obj) ? "不明" : obj.Personality;
    }

    protected new void Awake()
    {
      if (!this.CheckInstance())
        return;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      this.chaListCtrl = new ChaListControl();
      this.dictEntryChara = new SortedDictionary<int, ChaControl>();
      this.matHairDithering = CommonLib.LoadAsset<Material>("chara/hair_shader_mat.unity3d", "hair_dithering", false, "abdata");
      this.matHairCutout = CommonLib.LoadAsset<Material>("chara/hair_shader_mat.unity3d", "hair_cutout", false, "abdata");
      this.shaderDithering = this.matHairDithering.get_shader();
      this.shaderCutout = this.matHairCutout.get_shader();
      this.chaListCtrl.LoadListInfoAll();
    }

    protected void Update()
    {
      if (!this.CheckInstance())
        return;
      foreach (KeyValuePair<int, ChaControl> keyValuePair in this.dictEntryChara)
        keyValuePair.Value.UpdateForce();
    }

    protected void LateUpdate()
    {
      foreach (ChaControl chaControl in this.dictEntryChara.Values)
        chaControl.LateUpdateForce();
    }

    protected override void OnDestroy()
    {
      this.chaListCtrl.SaveItemID();
    }
  }
}
