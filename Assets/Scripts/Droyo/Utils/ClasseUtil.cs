using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ClasseUtil {
    
    public static PropertyInfo[] ListOfPropertiesFromClass(Type AType) {
        if (AType == null) return null;
        return AType.GetProperties();
    }

    public static PropertyInfo[] ListOfIntegerPropertiesFromClass(Type AType) {
        if (AType == null) return null;
        List<PropertyInfo> result = new List<PropertyInfo>();
        foreach(PropertyInfo pi in AType.GetProperties()){
            if(pi.PropertyType == typeof(int)){
                result.Add(pi);
            }
        }
        return result.ToArray();
    }

    //Get a List of the properties from a instance of a class
    public static PropertyInfo[] ListOfPropertiesFromInstance(object InstanceOfAType) {
        if (InstanceOfAType == null) return null;
        Type TheType = InstanceOfAType.GetType();
        return TheType.GetProperties(BindingFlags.Public);
    }

    //purrfect for usage example and Get a Map of the properties from a instance of a class
    public static Dictionary<string, PropertyInfo> DictionaryOfPropertiesFromInstance(object InstanceOfAType) {
        if (InstanceOfAType == null) return null;
        Type TheType = InstanceOfAType.GetType();
        PropertyInfo[] Properties = TheType.GetProperties(BindingFlags.Public);
        Dictionary<string, PropertyInfo> PropertiesMap = new Dictionary<string, PropertyInfo>();
        foreach (PropertyInfo Prop in Properties)
        {
            PropertiesMap.Add(Prop.Name, Prop);
        }
        return PropertiesMap;
    }

}
