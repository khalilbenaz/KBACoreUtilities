using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Utilities for reflection and metadata operations
    /// </summary>
    public static class ReflectionUtils
    {
        #region Property Operations

        /// <summary>
        /// Gets all properties of a type
        /// </summary>
        public static PropertyInfo[] GetAllProperties(Type type, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            return type.GetProperties(flags);
        }

        /// <summary>
        /// Gets property value by name
        /// </summary>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
                return null;

            var property = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return property?.GetValue(obj);
        }

        /// <summary>
        /// Sets property value by name
        /// </summary>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
                return;

            var property = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            property?.SetValue(obj, value);
        }

        /// <summary>
        /// Gets all properties with specific attribute
        /// </summary>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return type.GetProperties()
                      .Where(p => p.GetCustomAttribute<TAttribute>() != null);
        }

        #endregion

        #region Method Operations

        /// <summary>
        /// Gets all methods of a type
        /// </summary>
        public static MethodInfo[] GetAllMethods(Type type, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            return type.GetMethods(flags);
        }

        /// <summary>
        /// Invokes method by name
        /// </summary>
        public static object InvokeMethod(object obj, string methodName, params object[] parameters)
        {
            if (obj == null || string.IsNullOrWhiteSpace(methodName))
                return null;

            var method = obj.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return method?.Invoke(obj, parameters);
        }

        /// <summary>
        /// Invokes static method
        /// </summary>
        public static object InvokeStaticMethod(Type type, string methodName, params object[] parameters)
        {
            if (type == null || string.IsNullOrWhiteSpace(methodName))
                return null;

            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            return method?.Invoke(null, parameters);
        }

        /// <summary>
        /// Invokes generic method
        /// </summary>
        public static object InvokeGenericMethod(object obj, string methodName, Type[] genericTypes, params object[] parameters)
        {
            if (obj == null || string.IsNullOrWhiteSpace(methodName))
                return null;

            var method = obj.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
                return null;

            var genericMethod = method.MakeGenericMethod(genericTypes);
            return genericMethod.Invoke(obj, parameters);
        }

        #endregion

        #region Attribute Operations

        /// <summary>
        /// Gets attribute from type
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        /// Gets all attributes from type
        /// </summary>
        public static TAttribute[] GetAttributes<TAttribute>(Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttributes<TAttribute>().ToArray();
        }

        /// <summary>
        /// Checks if type has attribute
        /// </summary>
        public static bool HasAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return type.GetCustomAttribute<TAttribute>() != null;
        }

        /// <summary>
        /// Gets attribute from property
        /// </summary>
        public static TAttribute GetPropertyAttribute<TAttribute>(PropertyInfo property) where TAttribute : Attribute
        {
            return property.GetCustomAttribute<TAttribute>();
        }

        #endregion

        #region Instance Creation

        /// <summary>
        /// Creates instance of type
        /// </summary>
        public static object CreateInstance(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// Creates instance of type T
        /// </summary>
        public static T CreateInstance<T>(params object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }

        /// <summary>
        /// Creates instance without calling constructor
        /// </summary>
        public static object CreateUninitializedInstance(Type type)
        {
            return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
        }

        #endregion

        #region Type Inspection

        /// <summary>
        /// Checks if type implements interface
        /// </summary>
        public static bool ImplementsInterface(Type type, Type interfaceType)
        {
            return interfaceType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Checks if type implements interface T
        /// </summary>
        public static bool ImplementsInterface<T>(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        /// <summary>
        /// Gets all types implementing interface
        /// </summary>
        public static Type[] GetTypesImplementingInterface<T>(Assembly assembly)
        {
            var interfaceType = typeof(T);
            return assembly.GetTypes()
                          .Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t))
                          .ToArray();
        }

        /// <summary>
        /// Checks if type is generic
        /// </summary>
        public static bool IsGenericType(Type type)
        {
            return type.IsGenericType;
        }

        /// <summary>
        /// Gets generic type arguments
        /// </summary>
        public static Type[] GetGenericArguments(Type type)
        {
            return type.GetGenericArguments();
        }

        /// <summary>
        /// Checks if type is nullable
        /// </summary>
        public static bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Gets underlying type for nullable
        /// </summary>
        public static Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        #endregion

        #region Field Operations

        /// <summary>
        /// Gets field value (including private)
        /// </summary>
        public static object GetFieldValue(object obj, string fieldName)
        {
            if (obj == null || string.IsNullOrWhiteSpace(fieldName))
                return null;

            var field = obj.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return field?.GetValue(obj);
        }

        /// <summary>
        /// Sets field value (including private)
        /// </summary>
        public static void SetFieldValue(object obj, string fieldName, object value)
        {
            if (obj == null || string.IsNullOrWhiteSpace(fieldName))
                return;

            var field = obj.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(obj, value);
        }

        #endregion

        #region Deep Copy

        /// <summary>
        /// Creates deep copy of object using reflection
        /// </summary>
        public static T DeepCopy<T>(T source)
        {
            if (source == null)
                return default;

            var type = source.GetType();

            if (type.IsValueType || type == typeof(string))
                return source;

            var clone = CreateInstance(type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanRead && property.CanWrite)
                {
                    var value = property.GetValue(source);
                    property.SetValue(clone, value);
                }
            }

            return (T)clone;
        }

        #endregion

        #region Object Mapping

        /// <summary>
        /// Maps properties from source to destination
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(TSource source) where TDestination : new()
        {
            if (source == null)
                return default;

            var destination = new TDestination();
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);

            foreach (var sourceProp in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var destProp = destinationType.GetProperty(sourceProp.Name, BindingFlags.Public | BindingFlags.Instance);
                
                if (destProp != null && destProp.CanWrite && sourceProp.CanRead)
                {
                    var value = sourceProp.GetValue(source);
                    destProp.SetValue(destination, value);
                }
            }

            return destination;
        }

        #endregion

        #region Assembly Operations

        /// <summary>
        /// Gets all types from assembly
        /// </summary>
        public static Type[] GetTypesFromAssembly(Assembly assembly)
        {
            return assembly.GetTypes();
        }

        /// <summary>
        /// Gets types with attribute from assembly
        /// </summary>
        public static Type[] GetTypesWithAttribute<TAttribute>(Assembly assembly) where TAttribute : Attribute
        {
            return assembly.GetTypes()
                          .Where(t => t.GetCustomAttribute<TAttribute>() != null)
                          .ToArray();
        }

        /// <summary>
        /// Loads assembly by name
        /// </summary>
        public static Assembly LoadAssembly(string assemblyName)
        {
            return Assembly.Load(assemblyName);
        }

        #endregion

        #region Type Discovery

        /// <summary>
        /// Finds all types inheriting from base type
        /// </summary>
        public static Type[] FindDerivedTypes(Type baseType, Assembly assembly)
        {
            return assembly.GetTypes()
                          .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t) && t != baseType)
                          .ToArray();
        }

        /// <summary>
        /// Finds all types with specific base class
        /// </summary>
        public static Type[] FindDerivedTypes<TBase>(Assembly assembly)
        {
            return FindDerivedTypes(typeof(TBase), assembly);
        }

        #endregion
    }
}
