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
    }
}
