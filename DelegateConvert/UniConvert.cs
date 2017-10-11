using System;
using System.Linq.Expressions;

namespace DelegateConvert
{
    public class UniConvert
    {
        /// <summary>
        /// Despite it's public access modifier, it's unnecessary to call it directly, just call it when you know what you are doing and want (for example) to decrease your call stack
        /// </summary>
        /// <param name="sourceDelegate">Can be Action<TX,...> or Func<TX,...></param>
        /// <param name="returnParameterType"></param>
        /// <returns></returns>
        public static Delegate ConvertToObjectArrParams(Delegate sourceDelegate, Type returnParameterType = null)
        {
            var method = sourceDelegate.Method;
            var methodParams = method.GetParameters();

            var eArgs = new Expression[methodParams.Length];
            var eParams = new[] { Expression.Parameter(typeof(object[]), "x") };
            var instanceExpression = Expression.Constant(sourceDelegate.Target);

            for (int i = 0; i < methodParams.Length; i++)
            {
                var cObj = Expression.ArrayIndex(eParams[0], Expression.Constant(i, typeof(int)));
                eArgs[i] = Expression.Convert(cObj, methodParams[i].ParameterType);
            }

            var eCall = Expression.Call(instanceExpression, method, eArgs);

            if (method.ReturnType == typeof(void))
            {
                return Expression.Lambda<Action<object[]>>(eCall, eParams).Compile();
            }
            else
            {
                var convert = Expression.Convert(eCall, returnParameterType ?? typeof(object));
                return Expression.Lambda(convert, eParams).Compile(); //<Func<object[], object>>
            }
        }
    }
}