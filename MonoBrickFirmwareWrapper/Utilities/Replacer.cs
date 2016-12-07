using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoBrickFirmwareWrapper.Utilities
{
    public static class Replacer
    {
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
        }

        public static void RestorePrivateStaticField(Type targetClassType, string fieldName)
        {
        }
    }
}
