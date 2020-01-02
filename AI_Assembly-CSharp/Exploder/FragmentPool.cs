// Decompiled with JetBrains decompiler
// Type: Exploder.FragmentPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  public class FragmentPool : MonoBehaviour
  {
    private static FragmentPool instance;
    private Fragment[] pool;
    private bool meshColliders;

    public FragmentPool()
    {
      base.\u002Ector();
    }

    public static FragmentPool Instance
    {
      get
      {
        if (Object.op_Equality((Object) FragmentPool.instance, (Object) null))
          FragmentPool.instance = (FragmentPool) new GameObject("FragmentRoot").AddComponent<FragmentPool>();
        return FragmentPool.instance;
      }
    }

    private void Awake()
    {
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      FragmentPool.instance = this;
    }

    private void OnDestroy()
    {
      this.DestroyFragments();
      FragmentPool.instance = (FragmentPool) null;
    }

    public int PoolSize
    {
      get
      {
        return this.pool.Length;
      }
    }

    public Fragment[] Pool
    {
      get
      {
        return this.pool;
      }
    }

    public List<Fragment> GetAvailableFragments(int size)
    {
      if (size > this.pool.Length)
      {
        Debug.LogError((object) ("Requesting pool size higher than allocated! Please call Allocate first! " + (object) size));
        return (List<Fragment>) null;
      }
      if (size == this.pool.Length)
        return new List<Fragment>((IEnumerable<Fragment>) this.pool);
      List<Fragment> fragmentList = new List<Fragment>();
      int num = 0;
      foreach (Fragment fragment in this.pool)
      {
        if (!fragment.IsActive && !fragment.Cracked)
        {
          fragmentList.Add(fragment);
          ++num;
        }
        if (num == size)
          return fragmentList;
      }
      foreach (Fragment fragment in this.pool)
      {
        if (!fragment.Visible && !fragment.Cracked)
        {
          fragmentList.Add(fragment);
          ++num;
        }
        if (num == size)
          return fragmentList;
      }
      if (num < size)
      {
        foreach (Fragment fragment in this.pool)
        {
          if (fragment.IsSleeping() && fragment.Visible && !fragment.Cracked)
          {
            fragmentList.Add(fragment);
            ++num;
          }
          if (num == size)
            return fragmentList;
        }
      }
      if (num < size)
      {
        foreach (Fragment fragment in this.pool)
        {
          if (!fragment.IsSleeping() && fragment.Visible && !fragment.Cracked)
          {
            fragmentList.Add(fragment);
            ++num;
          }
          if (num == size)
            return fragmentList;
        }
      }
      Debug.LogWarning((object) "Not enough fragments in the pool, increase pool size!");
      return fragmentList;
    }

    public int GetAvailableCrackFragmentsCount()
    {
      int num = 0;
      foreach (Fragment fragment in this.pool)
      {
        if (!fragment.Cracked)
          ++num;
      }
      return num;
    }

    public void Reset(ExploderParams parameters)
    {
      this.Allocate(parameters.FragmentPoolSize, parameters.FragmentOptions.MeshColliders, parameters.Use2DCollision);
      this.SetExplodableFragments(parameters.FragmentOptions.ExplodeFragments, parameters.DontUseTag);
      this.SetFragmentPhysicsOptions(parameters.FragmentOptions, parameters.Use2DCollision);
      this.SetSFXOptions(parameters.FragmentSFX);
    }

    public void Allocate(int poolSize, bool useMeshColliders, bool use2dCollision)
    {
      if (this.pool != null && this.pool.Length >= poolSize && useMeshColliders == this.meshColliders)
        return;
      this.DestroyFragments();
      this.pool = new Fragment[poolSize];
      this.meshColliders = useMeshColliders;
      GameObject gameObject1 = (GameObject) null;
      Fragment objectOfType = (Fragment) Object.FindObjectOfType<Fragment>();
      if (Object.op_Implicit((Object) objectOfType))
      {
        gameObject1 = ((Component) objectOfType).get_gameObject();
      }
      else
      {
        Object @object = Resources.Load("ExploderFragment");
        if (Object.op_Implicit(@object))
          gameObject1 = Object.Instantiate(@object) as GameObject;
      }
      for (int index = 0; index < poolSize; ++index)
      {
        GameObject gameObject2;
        if (Object.op_Implicit((Object) gameObject1))
        {
          gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1);
          ((Object) gameObject2).set_name("fragment_" + (object) index);
        }
        else
          gameObject2 = new GameObject("fragment_" + (object) index);
        gameObject2.AddComponent<MeshFilter>();
        gameObject2.AddComponent<MeshRenderer>();
        if (use2dCollision)
        {
          gameObject2.AddComponent<PolygonCollider2D>();
          gameObject2.AddComponent<Rigidbody2D>();
        }
        else
        {
          if (useMeshColliders)
            ((MeshCollider) gameObject2.AddComponent<MeshCollider>()).set_convex(true);
          else
            gameObject2.AddComponent<BoxCollider>();
          gameObject2.AddComponent<Rigidbody>();
        }
        gameObject2.AddComponent<ExploderOption>();
        Fragment fragment = (Fragment) gameObject2.GetComponent<Fragment>();
        if (!Object.op_Implicit((Object) fragment))
          fragment = (Fragment) gameObject2.AddComponent<Fragment>();
        gameObject2.get_transform().set_parent(((Component) this).get_gameObject().get_transform());
        this.pool[index] = fragment;
        ExploderUtils.SetActiveRecursively(gameObject2.get_gameObject(), false);
        fragment.RefreshComponentsCache();
        fragment.Sleep();
      }
    }

    public void ResetTransform()
    {
      ((Component) this).get_gameObject().get_transform().set_position(Vector3.get_zero());
      ((Component) this).get_gameObject().get_transform().set_rotation(Quaternion.get_identity());
    }

    public void WakeUp()
    {
      foreach (Fragment fragment in this.pool)
        fragment.WakeUp();
    }

    public void Sleep()
    {
      foreach (Fragment fragment in this.pool)
        fragment.Sleep();
    }

    public void DestroyFragments()
    {
      if (this.pool == null)
        return;
      foreach (Fragment fragment in this.pool)
      {
        if (Object.op_Implicit((Object) fragment))
          Object.Destroy((Object) ((Component) fragment).get_gameObject());
      }
      this.pool = (Fragment[]) null;
    }

    public void DeactivateFragments()
    {
      if (this.pool == null)
        return;
      foreach (Fragment fragment in this.pool)
      {
        if (Object.op_Implicit((Object) fragment))
          fragment.Deactivate();
      }
    }

    public void SetExplodableFragments(bool explodable, bool dontUseTag)
    {
      if (this.pool == null)
        return;
      if (dontUseTag)
      {
        foreach (Fragment fragment in this.pool)
        {
          if (Object.op_Implicit((Object) ((Component) fragment).get_gameObject()) && !Object.op_Implicit((Object) ((Component) fragment).get_gameObject().GetComponent<Explodable>()))
            ((Component) fragment).get_gameObject().AddComponent<Explodable>();
        }
      }
      else
      {
        if (!explodable)
          return;
        foreach (Component component in this.pool)
          component.set_tag(ExploderObject.Tag);
      }
    }

    public void SetFragmentPhysicsOptions(FragmentOption options, bool physics2d)
    {
      if (this.pool == null)
        return;
      RigidbodyConstraints constraints = (RigidbodyConstraints) 0;
      if (options.FreezePositionX)
        constraints = (RigidbodyConstraints) (constraints | 2);
      if (options.FreezePositionY)
        constraints = (RigidbodyConstraints) (constraints | 4);
      if (options.FreezePositionZ)
        constraints = (RigidbodyConstraints) (constraints | 8);
      if (options.FreezeRotationX)
        constraints = (RigidbodyConstraints) (constraints | 16);
      if (options.FreezeRotationY)
        constraints = (RigidbodyConstraints) (constraints | 32);
      if (options.FreezeRotationZ)
        constraints = (RigidbodyConstraints) (constraints | 64);
      foreach (Fragment fragment in this.pool)
      {
        if (Object.op_Implicit((Object) ((Component) fragment).get_gameObject()))
          ((Component) fragment).get_gameObject().set_layer(LayerMask.NameToLayer(options.Layer));
        fragment.SetConstraints(constraints);
        fragment.DisableColliders(options.DisableColliders, this.meshColliders, physics2d);
      }
    }

    public void SetSFXOptions(FragmentSFX sfx)
    {
      if (this.pool == null)
        return;
      int num = 0;
      foreach (Fragment fragment in this.pool)
      {
        if (!fragment.IsActive && num++ <= sfx.EmitersMax)
          fragment.InitSFX(sfx);
      }
    }

    public List<Fragment> GetActiveFragments()
    {
      if (this.pool == null)
        return (List<Fragment>) null;
      List<Fragment> fragmentList = new List<Fragment>(this.pool.Length);
      foreach (Fragment fragment in this.pool)
      {
        if (ExploderUtils.IsActive(((Component) fragment).get_gameObject()))
          fragmentList.Add(fragment);
      }
      return fragmentList;
    }
  }
}
