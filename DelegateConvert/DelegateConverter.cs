using System;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection;

namespace DelegateConvert
{
    public class DelegateConverter
    {
        /// <summary>
        /// Despite it's public access modifier, it's unnecessary to call it directly, just call it when you know what you are doing and want (for example) to decrease your call stack
        /// </summary>
        /// <param name="sourceDelegate">Can be Action<TX,...> or Func<TX,...></param>
        /// <param name="returnParameterType"></param>
        /// <returns></returns>
        public static Delegate Convert(Delegate sourceDelegate, Type returnParameterType = null)
        {
            return Convert(sourceDelegate.Method, Expression.Constant(sourceDelegate.Target), returnParameterType);
        }

        public static Delegate Convert(MethodInfo method, object instance = null, Type returnParameterType = null)
        {
            return Convert(method, Expression.Constant(instance), returnParameterType);
        }

        public static Delegate Convert(MethodInfo method, ConstantExpression instanceExpression = null, Type returnParameterType = null)
        {
            var methodParams = method.GetParameters();

            var eArgs = new Expression[methodParams.Length];
            var eParams = new[] { Expression.Parameter(typeof(object[]), "x") };

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

        public static Delegate ConvertSingleUnknownArg(MethodInfo method, ConstantExpression instanceExpression = null, Type returnParameterType = null)
        {
            var methodParams = method.GetParameters();
            if (methodParams.Length > 1)
                throw new Exception("Invalid parameter count");
            LambdaExpression lambda;
            if (method.ReturnType == typeof(void))
            {
                var inputParameter = Expression.Parameter(typeof(object), "x");
                var converter = Expression.Convert(inputParameter, methodParams[0].ParameterType);

                var setterCall = Expression.Call(instanceExpression, method, converter);
                lambda = Expression.Lambda(setterCall, inputParameter);
            }
            else
            {
                var getterCall = Expression.Call(instanceExpression, method);
                var converter = Expression.Convert(getterCall, typeof(object));
                lambda = Expression.Lambda(converter);
            }
            return lambda.Compile();
        }
    }
}