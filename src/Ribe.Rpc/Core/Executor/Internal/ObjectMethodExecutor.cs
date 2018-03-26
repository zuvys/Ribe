using Ribe.Rpc.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ribe.Rpc.Core.Executor.Internals
{
    public class ObjectMethodExecutor : IObjectMethodExecutor
    {
        protected Func<object, object[], Task<object>> MethodExecutor { get; }

        public Type ServiceType { get; }

        public ServiceMethod ServiceMethod { get; }

        public ObjectMethodExecutor(Type serviceType, ServiceMethod serviceMethod)
        {
            ServiceType = serviceType;
            ServiceMethod = serviceMethod;

            MethodExecutor = serviceMethod.IsAsyncMethod
                ? CreateAsyncExecutor(serviceType, serviceMethod)
                : CreateAsyncExecutorWrapper(serviceType, serviceMethod);
        }

        public Task<object> ExecuteAsync(object instance, object[] paramterValues)
        {
            return MethodExecutor(instance, paramterValues);
        }

        private static Func<object, object[], Task<object>> CreateAsyncExecutorWrapper(Type serviceType, ServiceMethod serviceMethod)
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

            var executor = Expression.Lambda(methodCall, serviceParamter, parametersParameter).Compile();

            //Wrap return void
            return (instance, paramterValues) =>
            {
                try
                {
                    if (returnType != typeof(void))
                    {
                        return Task.FromResult(((Func<object, object[], object>)executor)(instance, paramterValues));
                    }
                    else
                    {
                        ((Action<object, object[]>)executor)(instance, paramterValues);
                        return Task.FromResult<object>(null);
                    }
                }
                catch (Exception e)
                {
                    return Task.FromResult<object>(e);
                }
            };
        }

        private static Func<object, object[], Task<object>> CreateAsyncExecutor(Type serviceType, ServiceMethod serviceMethod)
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

            var task = Expression.Variable(returnType, "task");
            var awaiter = Expression.Variable(awaiterType, "awaiter");
            var getAwaiter = Expression.Call(task, returnType.GetMethod("GetAwaiter"));
            var getException = Expression.MakeMemberAccess(task, returnType.GetProperty("Exception"));

            var lambdaBody = Expression.Block(
                new[] { task, awaiter },
                Expression.Assign(task, methodCall),
                Expression.Assign(awaiter, getAwaiter),
                Expression.Call(
                    awaiter,
                    awaiterType.GetMethod("OnCompleted"),
                    Expression.Lambda<Action>(
                         Expression.IfThenElse(
                            Expression.NotEqual(getException, Expression.Constant(null)),
                            Expression.Call(
                                tcsParamter,
                                tcsType.GetMethod("SetResult"),
                                getException
                            ),
                            Expression.Call(
                                tcsParamter,
                                tcsType.GetMethod("SetResult"),
                                isVoidMethod
                                    ? (Expression)Expression.Constant(null)
                                    : Expression.Call(awaiter, awaiterType.GetMethod("GetResult"))
                            )
                        )
                    )
                ),
                Expression.Label(Expression.Label())
            );

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
