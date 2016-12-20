using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoBrickFirmwareWrapper.Utilities
{
    /// <summary>
    /// An utility class to replace static fields of MonoBrickFirmwareWrapper.
    /// </summary>
    public static class Replacer
    {
        /// <summary>
        /// <para>An dictionary to store original values.</para>
        /// <para>
        /// The key is the field name with full qualifier name of class.
        /// The value is the original value.
        /// </para>
        /// </summary>
        private static IDictionary<string, object> originalFieldValues = new Dictionary<string, object>();

        /// <summary>a
        /// Create a key of a specified field for the original value dictionary.
        /// </summary>
        /// <param name="targetClassType">The type of a class including a target private static field.</param>
        /// <param name="fieldName">A target private static field name.</param>
        /// <returns>a key of a specified field.</returns>
        private static string createKey(Type targetClassType, string fieldName)
        {
            return targetClassType + "+" + fieldName;
        }

        /// <summary>
        /// <para>Replace a private static field value of a specified class.</para>
        /// <para>
        /// NOTICE: Restore the original value of the target field
        /// using <see cref="RestorePrivateStaticField(Type, string)"/>
        /// by the end of each test case,
        /// because the static field value may influence other test cases.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The type of a target private static field.</typeparam>
        /// <param name="targetClassType">The type of a class including a target private static field.</param>
        /// <param name="fieldName">A target private static field name.</param>
        /// <param name="fieldValue">A value that you want to replace with.</param>
        /// <exception cref="InvalidOperationException">
        /// The target field has already replaced.
        /// You can't replace with another value until you restore the original value.
        /// </exception>
        public static void ReplacePrivateStaticField<T>(Type targetClassType, string fieldName, T fieldValue)
        {
            string key = createKey(targetClassType, fieldName);
            
            if (originalFieldValues.ContainsKey(key)) { throw new InvalidOperationException(); }

            FieldInfo fieldInfo = targetClassType.GetField(fieldName,
                BindingFlags.GetField | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static);

            originalFieldValues.Add(key, fieldInfo.GetValue(null));
            fieldInfo.SetValue(null, fieldValue);
        }

        /// <summary>
        /// <para>Restore the original value of a private static field of a specified class.</para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the field value hasn't been replaced or has been restored, this method do nothing.
        /// </para>
        /// </remarks>
        /// <param name="targetClassType">The type of a class including a target private static field.</param>
        /// <param name="fieldName">A target private static field name.</param>
        public static void RestorePrivateStaticField(Type targetClassType, string fieldName)
        {
            string key = createKey(targetClassType, fieldName);

            if (!originalFieldValues.ContainsKey(key)) { return; }

            FieldInfo fieldInfo = targetClassType.GetField(fieldName,
                BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static);
            fieldInfo.SetValue(null, originalFieldValues[key]);

            originalFieldValues.Remove(key);
        }

		/// <summary>
		/// <para>Set a value of a private instance field.</para>
		/// </summary>
		/// <typeparam name="T">The type of an instance including a target private instance field.</typeparam>
		/// <param name="instance">An instance including a target private instance field.</param>
		/// <param name="fieldName">A target private instance field name.</param>
		/// <param name="value">A value that you want to set.</param>
		public static void SetPrivateField<T>(this T instance, string fieldName, object value)
		{
			FieldInfo fieldInfo = typeof(T).GetField(fieldName,
				BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Instance);
			fieldInfo.SetValue(instance, value);
		}

		/// <summary>
		/// <para>Get a value of a private instance field.</para>
		/// </summary>
		/// <typeparam name="T">The type of an instance including a target private instance field.</typeparam>
		/// <param name="instance">An instance including a target private instance field.</param>
		/// <param name="fieldName">A target private instance field name.</param>
		/// <returns>The value of the target private instance field.</returns>
		public static object GetPrivateField<T>(this T instance, string fieldName)
		{
			FieldInfo fieldInfo = typeof(T).GetField(fieldName,
				BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
			return fieldInfo.GetValue(instance);
		}

		/// <summary>
		/// <para>Invoke a private method.</para>
		/// </summary>
		/// <typeparam name="T">The type of an instance including a target private method.</typeparam>
		/// <param name="instance">An instance including a target private method.</param>
		/// <param name="methodName">A target private method name.</param>
		/// <param name="parameters">An argument list for the invoked method.</param>
		public static void InvokePrivateMethod<T>(this T instance, string methodName, object[] parameters)
		{
			MethodInfo methodInfo = typeof(T).GetMethod(methodName,
				BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo.Invoke(instance, parameters);
		}

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod(Type targetClassType, string methodName, Action action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T">The type of 1st argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T>(Type targetClassType, string methodName, Action<T> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of 2nd argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2>(Type targetClassType, string methodName, Action<T1, T2> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3>(Type targetClassType, string methodName, Action<T1, T2, T3> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4>(Type targetClassType, string methodName, Action<T1, T2, T3, T4> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
        /// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
        /// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
        /// <typeparam name="T15">The type of the 15th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
        /// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
        /// <typeparam name="T15">The type of the 15th argument of the wrapper method.</typeparam>
        /// <typeparam name="T16">The type of the 16th argument of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Type targetClassType, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        {
            ReplacePrivateStaticField(targetClassType, methodName, action);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<TResult>(Type targetClassType, string methodName, Func<TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T, TResult>(Type targetClassType, string methodName, Func<T, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, TResult>(Type targetClassType, string methodName, Func<T1, T2, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
        /// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
        /// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
        /// <typeparam name="T15">The type of the 15th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

        /// <summary>
        /// Replace a wrapper method of MonoBrickFirmwareWrapper.
        /// </summary>
        /// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
        /// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
        /// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
        /// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
        /// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
        /// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
        /// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
        /// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
        /// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
        /// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
        /// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
        /// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
        /// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
        /// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
        /// <typeparam name="T15">The type of the 15th argument of the wrapper method.</typeparam>
        /// <typeparam name="T16">The type of the 16th argument of the wrapper method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
        /// <param name="targetClassType">The type of a class including a wrapper method.</param>
        /// <param name="methodName">A wrapper method name.</param>
        /// <param name="action">An action that you want to replace with.</param>
        public static void ReplaceWrapperMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Type targetClassType, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function)
        {
            ReplacePrivateStaticField(targetClassType, methodName, function);
        }

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance>(this TInstance instance, string methodName, Action action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T">The type of the 1st argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T>(this TInstance instance, string methodName, Action<T> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2>(this TInstance instance, string methodName, Action<T1, T2> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3>(this TInstance instance, string methodName, Action<T1, T2, T3> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4>(this TInstance instance, string methodName, Action<T1, T2, T3, T4> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
		/// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
		/// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
		/// <typeparam name="T15">The type of the 15th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
		/// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
		/// <typeparam name="T15">The type of the 15th argument of the wrapper method.</typeparam>
		/// <typeparam name="T16">The type of the 16th argument of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="action">An action that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this TInstance instance, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
		{
			instance.SetPrivateField(methodName, action);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, TResult>(this TInstance instance, string methodName, Func<TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T, TResult>(this TInstance instance, string methodName, Func<T, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, TResult>(this TInstance instance, string methodName, Func<T1, T2, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
		/// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
		/// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
		/// <typeparam name="T15">The type of the 15th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}

		/// <summary>
		/// Set a wrapper method of MonoBrickFirmwareWrapper.
		/// </summary>
		/// <typeparam name="TInstance">The type of an instance including a target private instance field.</typeparam>
		/// <typeparam name="T1">The type of the 1st argument of the wrapper method.</typeparam>
		/// <typeparam name="T2">The type of the 2nd argument of the wrapper method.</typeparam>
		/// <typeparam name="T3">The type of the 3rd argument of the wrapper method.</typeparam>
		/// <typeparam name="T4">The type of the 4th argument of the wrapper method.</typeparam>
		/// <typeparam name="T5">The type of the 5th argument of the wrapper method.</typeparam>
		/// <typeparam name="T6">The type of the 6th argument of the wrapper method.</typeparam>
		/// <typeparam name="T7">The type of the 7th argument of the wrapper method.</typeparam>
		/// <typeparam name="T8">The type of the 8th argument of the wrapper method.</typeparam>
		/// <typeparam name="T9">The type of the 9th argument of the wrapper method.</typeparam>
		/// <typeparam name="T10">The type of the 10th argument of the wrapper method.</typeparam>
		/// <typeparam name="T11">The type of the 11th argument of the wrapper method.</typeparam>
		/// <typeparam name="T12">The type of the 12th argument of the wrapper method.</typeparam>
		/// <typeparam name="T13">The type of the 13th argument of the wrapper method.</typeparam>
		/// <typeparam name="T14">The type of the 14th argument of the wrapper method.</typeparam>
		/// <typeparam name="T15">The type of the 15th argument of the wrapper method.</typeparam>
		/// <typeparam name="T16">The type of the 16th argument of the wrapper method.</typeparam>
		/// <typeparam name="TResult">The type of the return value of the wrapper method.</typeparam>
		/// <param name="instance">An instance including a wrapper method.</param>
		/// <param name="methodName">A wrapper method name.</param>
		/// <param name="function">A function that you want to set.</param>
		public static void SetWrapperMethod<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this TInstance instance, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function)
		{
			instance.SetPrivateField(methodName, function);
		}
    }
}
