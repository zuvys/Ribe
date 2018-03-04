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

            var methodCall = Expression.Call(
                    Expression.Convert(serviceParamter, serviceType),
                    serviceMethod.Method,
                    parameters);

            if (returnType == typeof(Task))
            {
                //Wrap return Task
                var voidDelegate = Expression.Lambda(methodCall, serviceParamter, parametersParameter).Compile();

                return (obj, paramterValues) =>
                {
                    ((Action<object, object[]>)voidDelegate)(obj, paramterValues);
                    return Task.FromResult<object>(null);
                };
            }

            var valueType = returnType.GetGenericArguments().FirstOrDefault();
            var awaiterType = typeof(TaskAwaiter<>).MakeGenericType(valueType);

            var getAwaiter = Expression.Call(methodCall, returnType.GetMethod("GetAwaiter"));
            var getResult = Expression.Call(getAwaiter, awaiterType.GetMethod("GetResult"));

            var objectDelegate = Expression.Lambda(getResult, serviceParamter, parametersParameter).Compile();

            return (obj, paramterValues) =>
            {
                return Task<object>.FromResult(((Func<object, object[], object>)objectDelegate)(obj, paramterValues));
            };
        }
    }
}
