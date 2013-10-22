﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;

namespace TddEbook.TddToolkit
{
  public static partial class Any
  {
    public static SortedSet<T> SortedSet<T>()
    {
      return new SortedSet<T>
      {
        Instance<T>(),
        Instance<T>(),
        Instance<T>()
      };
    }

    public static IEnumerable<T> EnumerableOfDerivativesFrom<T>() where T : class
    {
      return ListOfDerivativesFrom<T>();
    }

    public static IEnumerable<T> ListOfDerivativesFrom<T>() where T : class
    {
      return new List<T>
      {
        Instance<T>(),
        Instance<T>(),
        Instance<T>()
      };
    }

    public static IEnumerable<T> Enumerable<T>()
    {
      return new List<T>()
        {
          Any.Instance<T>(),  
          Any.Instance<T>(),
          Any.Instance<T>(),
        };
    }

    public static IEnumerable<T> EnumerableWithout<T>(params T[] excluded) where T : class
    {
      var result = new List<T>
      {
        InstanceOtherThan(excluded), 
        InstanceOtherThan(excluded), 
        InstanceOtherThan(excluded)
      };
      return result;
    }

    public static T[] Array<T>()
    {
      return Enumerable<T>().ToArray();
    }

    public static T[] ArrayWithout<T>(params T[] excluded) where T : class
    {
      return EnumerableWithout(excluded).ToArray();
    }

    public static List<T> List<T>()
    {
      return Enumerable<T>().ToList();
    }

    public static ISet<T> Set<T>()
    {
      return new HashSet<T>() { Any.Instance<T>(), Any.Instance<T>(), Any.Instance<T>() };
    }

    public static Dictionary<TKey, TValue> Dictionary<TKey, TValue>()
    {
      return new Dictionary<TKey, TValue>()
        {
          {Any.Instance<TKey>(), Any.Instance<TValue>()},  
          {Any.Instance<TKey>(), Any.Instance<TValue>()}, 
          {Any.Instance<TKey>(), Any.Instance<TValue>()},
        };
    }

    public static IEnumerable<T> EnumerableSortedDescending<T>()
    {
      return SortedSet<T>().ToList();
    }

    public static object EnumerableOfDerivativesFrom(Type type)
    {
      return ResultOfGenericVersionOfMethod(type, MethodBase.GetCurrentMethod().Name);
    }

    public static object ListOfDerivativesFrom(Type type)
    {
      return ResultOfGenericVersionOfMethod(type, MethodBase.GetCurrentMethod().Name);
    }

    public static object List(Type type)
    {
      return ResultOfGenericVersionOfMethod(type, MethodBase.GetCurrentMethod().Name);
    }
  }
}