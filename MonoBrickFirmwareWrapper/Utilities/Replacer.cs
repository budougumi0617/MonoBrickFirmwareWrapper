using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

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
            string key = $"{targetClassType}+{fieldName}";

            FieldInfo fieldInfo = targetClassType.GetField(fieldName,
                BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static);
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
        }
    }
}
