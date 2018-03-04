using Ribe.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ribe.Core.Executor.Internals
{
    public class ObjectMethodExecutor : IObjectMethodExecutor
    {
        protected Func<object, object[], object> MethodExecutor { get; }

        protected Func<object, object[], Task<object>> AsyncMethodExecutor { get; }

        public Type ServiceType { get; }

        public ServiceMethod ServiceMethod { get; }

        public ObjectMethodExecutor(Type serviceType, ServiceMethod serviceMethod)
        {
            ServiceType = serviceType;
            ServiceMethod = serviceMethod;

            if (serviceMethod.IsAsyncMethod)
            {
                AsyncMethodExecutor = CreateAsyncExecuteDelegate(serviceType, serviceMethod);
            }
            else
            {
                MethodExecutor = CreateExecuteDelegate(serviceType, serviceMethod);
            }
        }

        public object Execute(object instance, object[] paramterValues)
        {
            return MethodExecutor(instance, paramterValues);
        }

        public Task<object> ExecuteAsync(object instance, object[] paramterValues)
        {
            return AsyncMethodExecutor(instance, paramterValues);
        }

        private static Func<object, object[], object> CreateExecuteDelegate(Type serviceType, ServiceMethod serviceMethod)
        {
            var serviceParamter = Expression.Parameter(typeof(object), "service");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            var parameters = new List<Expression>();
            var parameterInfos = serviceMethod.Parameters;
            var returnType = serviceMethod.Method.ReturnType;

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                var item = parameterInfos[i];
                var paramterValue = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                var castedValue = Expression.Convert(paramterValue, item.ParameterType);

                parameters.Add(castedValue);
            }

            var methodCall = Expression.Convert(
                Expression.Call(
                    Expression.Convert(serviceParamter, serviceType),
                    serviceMethod.Method,
                    parameters
                ),
                returnType == typeof(void) ? typeof(void) : typeof(object)
            );

            var createdDelegate = Expression.Lambda(methodCall, serviceParamter, parametersParameter).Compile();

            if (returnType != typeof(void))
            {
                return (Func<object, object[], object>)createdDelegate;
            }

            //Wrap return void
            return (obj, paramterValues) =>
            {
                ((Action<object, object[]>)createdDelegate)(obj, paramterValues);
                return null;
            };
        }

        private static Func<object, Object[], Task<object>> CreateAsyncExecuteDelegate(Type serviceType, ServiceMethod serviceMethod)
        {
            var tcsType = typeof(TaskCompletionSource<object>);
            var returnType = serviceMethod.Method.ReturnType;

            var tcsParamter = Expression.Parameter(tcsType, "tcs");
            var serviceParamter = Expression.Parameter(typeof(object), "service");
            var paramsParameter = Expression.Parameter(typeof(object[]), "parameters");

            var parameters = new List<Expression>();

            for (int i = 0; i < serviceMethod.Parameters.Length; i++)
            {
                var item = serviceMethod.Parameters[i];
                var paramterValue = Expression.ArrayIndex(paramsParameter, Expression.Constant(i));
                var castedValue = Expression.Convert(paramterValue, item.ParameterType);

                parameters.Add(castedValue);
            }

            var methodCall = Expression.Call(
                    Expression.Convert(serviceParamter, serviceType),
                    serviceMethod.Method,
                    parameters);

            var isVoidMethod = returnType == typeof(Task);
            var awaiterType = isVoidMethod
                ? typeof(TaskAwaiter)
                : typeof(TaskAwaiter<>).MakeGenericType(returnType.GetGenericArguments().FirstOrDefault());

            var awaiterVar = Expression.Variable(awaiterType, "awaiter");

            var lambdaBody = Expression.Block(
                new[] { awaiterVar },
                Expression.Assign(
                    awaiterVar,
                    Expression.Call(methodCall, returnType.GetMethod("GetAwaiter"))
                ),
                Expression.Call(
                    awaiterVar,
                    awaiterType.GetMethod("OnCompleted"),
                    Expression.Lambda<Action>(
                        Expression.Call(
                            tcsParamter,
                            tcsType.GetMethod("SetResult"),
                            isVoidMethod
                                ? (Expression)Expression.Constant(null)
                                : Expression.Call(awaiterVar, awaiterType.GetMethod("GetResult"))
                        )
                    )
                ),
                Expression.Label(Expression.Label()));

            var lambda = Expression.Lambda(lambdaBody, serviceParamter, paramsParameter, tcsParamter);
            var executor = (Action<object, object[], TaskCompletionSource<object>>)lambda.Compile();

            return (obj, paramterValues) =>
            {
                var tcs = new TaskCompletionSource<object>();

                executor(obj, paramterValues, tcs);

                return tcs.Task;
            };
        }
    }
}
