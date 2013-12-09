﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture;

namespace TddEbook.TddToolkit
{
  public partial class Any
  {
    public static IEnumerable<T> Enumerable<T>()
    {
      return new List<T>()
        {
          Any.Instance<T>(),  
          Any.Instance<T>(),
          Any.Instance<T>(),
        };
    }

    public static IEnumerable<T> EnumerableWithout<T>(params T[] excluded)
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

    public static T[] ArrayWithout<T>(params T[] excluded)
    {
      return EnumerableWithout(excluded).ToArray();
    }

    public static List<T> List<T>()
    {
      return Enumerable<T>().ToList();
    }

    public static SortedList<TKey, TValue> List<TKey, TValue>()
    {
      return new SortedList<TKey,TValue>()
        {
          {Any.Instance<TKey>(), Any.Instance<TValue>()},  
          {Any.Instance<TKey>(), Any.Instance<TValue>()}, 
          {Any.Instance<TKey>(), Any.Instance<TValue>()},
        };
    }

    public static ISet<T> Set<T>()
    {
      return new HashSet<T>() { Any.Instance<T>(), Any.Instance<T>(), Any.Instance<T>() };
    }

    public static SortedSet<T> SortedSet<T>()
    {
      return new SortedSet<T>
      {
        Instance<T>(),
        Instance<T>(),
        Instance<T>()
      };
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

    public static SortedDictionary<TKey, TValue> SortedDictionary<TKey, TValue>()
    {
      return new SortedDictionary<TKey, TValue>()
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

    public static object List(Type type)
    {
      return ResultOfGenericVersionOfMethod(type, MethodBase.GetCurrentMethod().Name);
    }

    public static object Set(Type type)
    {
      return ResultOfGenericVersionOfMethod(type, MethodBase.GetCurrentMethod().Name);
    }

    public static object Dictionary(Type keyType, Type valueType)
    {
      return ResultOfGenericVersionOfMethod(keyType, valueType, MethodBase.GetCurrentMethod().Name);
    }

    public static object SortedList(Type keyType, Type valueType)
    {
      return ResultOfGenericVersionOfMethod(keyType, valueType, MethodBase.GetCurrentMethod().Name);
    }

    public static object SortedSet(Type type)
    {
      return ResultOfGenericVersionOfMethod(type, MethodBase.GetCurrentMethod().Name); 
    }

    public static object SortedDictionary(Type keyType, Type valueType)
    {
      return ResultOfGenericVersionOfMethod(keyType, valueType, MethodBase.GetCurrentMethod().Name);
    }

    public static object Array(Type type)
    {
      return ResultOfGenericVersionOfMethod(type, MethodBase.GetCurrentMethod().Name); 
    }
  }
}
