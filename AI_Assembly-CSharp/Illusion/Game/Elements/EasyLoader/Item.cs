// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Elements.EasyLoader.Item
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Illusion.Game.Elements.EasyLoader
{
  [Serializable]
  public class Item
  {
    public Item.Data[] data = new Item.Data[0];
    [SerializeField]
    private List<GameObject> _itemObjectList = new List<GameObject>();

    public List<GameObject> itemObjectList
    {
      get
      {
        return this._itemObjectList;
      }
    }

    public void Visible(bool visible)
    {
      this._itemObjectList.ForEach((Action<GameObject>) (item => item.SetActive(visible)));
    }

    public void Setting(ChaControl chaCtrl, bool isItemClear = true)
    {
      if (isItemClear)
      {
        this._itemObjectList.ForEach((Action<GameObject>) (item => Object.Destroy((Object) item)));
        this._itemObjectList.Clear();
      }
      this._itemObjectList.AddRange(((IEnumerable<Item.Data>) this.data).Select<Item.Data, GameObject>((Func<Item.Data, GameObject>) (item => item.Load(chaCtrl))));
    }

    [Serializable]
    public class Data : AssetBundleData
    {
      public Vector3 offsetPos = Vector3.get_zero();
      public Vector3 offsetAngle = Vector3.get_zero();
      public Motion motion = new Motion();
      public Item.Data.Type type;

      public static Transform GetParent(Item.Data.Type type, ChaControl chaCtrl)
      {
        switch (type)
        {
          case Item.Data.Type.Head:
            return chaCtrl.cmpBoneBody.targetEtc.trfHeadParent;
          case Item.Data.Type.Neck:
            return ((Component) chaCtrl.GetAccessoryParentTransform(15)).get_transform();
          case Item.Data.Type.LeftHand:
            return ((Component) chaCtrl.GetAccessoryParentTransform(44)).get_transform();
          case Item.Data.Type.RightHand:
            return ((Component) chaCtrl.GetAccessoryParentTransform(48)).get_transform();
          case Item.Data.Type.LeftFoot:
            return ((Component) chaCtrl.GetAccessoryParentTransform(29)).get_transform();
          case Item.Data.Type.RightFoot:
            return ((Component) chaCtrl.GetAccessoryParentTransform(33)).get_transform();
          case Item.Data.Type.a_n_headside:
            return ((Component) chaCtrl.GetAccessoryParentTransform(8)).get_transform();
          case Item.Data.Type.k_f_handL_00:
            return chaCtrl.GetReferenceInfo(ChaReference.RefObjKey.k_f_handL_00).get_transform();
          case Item.Data.Type.k_f_handR_00:
            return chaCtrl.GetReferenceInfo(ChaReference.RefObjKey.k_f_handR_00).get_transform();
          case Item.Data.Type.chara:
            return ((Component) chaCtrl.animBody).get_transform();
          case Item.Data.Type.k_f_shoulderL_00:
            return chaCtrl.GetReferenceInfo(ChaReference.RefObjKey.k_f_shoulderL_00).get_transform();
          case Item.Data.Type.k_f_shoulderR_00:
            return chaCtrl.GetReferenceInfo(ChaReference.RefObjKey.k_f_shoulderR_00).get_transform();
          default:
            return (Transform) null;
        }
      }

      public GameObject Load(ChaControl chaCtrl)
      {
        GameObject gameObject = this.LoadModel(chaCtrl);
        Animator component = (Animator) gameObject.GetComponent<Animator>();
        if (Object.op_Inequality((Object) component, (Object) null))
          this.motion.LoadAnimator(component);
        return gameObject;
      }

      private GameObject LoadModel(ChaControl chaCtrl)
      {
        Transform transform1 = Item.Data.GetParent(this.type, chaCtrl);
        if (Object.op_Equality((Object) transform1, (Object) null))
          transform1 = ((Component) chaCtrl).get_transform().get_root();
        GameObject asset = this.GetAsset<GameObject>();
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) asset, transform1, false);
        Transform transform2 = gameObject.get_transform();
        transform2.set_localPosition(Vector3.op_Addition(transform2.get_localPosition(), this.offsetPos));
        Transform transform3 = gameObject.get_transform();
        transform3.set_localEulerAngles(Vector3.op_Addition(transform3.get_localEulerAngles(), this.offsetAngle));
        ((Object) gameObject).set_name(((Object) asset).get_name());
        this.UnloadBundle(false, false);
        return gameObject;
      }

      public enum Type
      {
        None,
        Head,
        Neck,
        LeftHand,
        RightHand,
        LeftFoot,
        RightFoot,
        a_n_headside,
        k_f_handL_00,
        k_f_handR_00,
        chara,
        k_f_shoulderL_00,
        k_f_shoulderR_00,
      }
    }
  }
}
