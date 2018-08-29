using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sirius {
	public static class Reflect<TType> {
		public static TType Default;

		public static PropertyInfo GetProperty<TResult>(Expression<Func<TType, TResult>> propertyAccess) {
			var expression = propertyAccess.Body as MemberExpression;
			if ((expression == null) || !(expression.Member is PropertyInfo)) {
				var callExpression = propertyAccess.Body as MethodCallExpression;
				if ((callExpression != null) && (callExpression.Method.DeclaringType != null)) {
					foreach (var property in callExpression.Method.DeclaringType.GetProperties()) {
						if ((property.GetGetMethod() == callExpression.Method) || (property.GetSetMethod()==callExpression.Method)) {
							return property;
						}
					}
				}
				throw new ArgumentException("Lambda expression is not a property access");
			}
			return (PropertyInfo)expression.Member;
		}

		public static MemberInfo GetMember<TResult>(Expression<Func<TType, TResult>> memberAccess) {
			var expression = memberAccess.Body as MemberExpression;
			if (expression == null) {
				throw new ArgumentException("Lambda expression is not a member access");
			}
			return expression.Member;
		}

		public static FieldInfo GetField<TResult>(Expression<Func<TType, TResult>> fieldAccess) {
			var expression = fieldAccess.Body as MemberExpression;
			if ((expression == null) || !(expression.Member is FieldInfo) || ((FieldInfo)expression.Member).IsStatic) {
				throw new ArgumentException("Lambda expression is not an instance field access");
			}
			return (FieldInfo)expression.Member;
		}

		public static MethodInfo GetMethod(Expression<Action<TType>> methodCall) {
			var expression = methodCall.Body as MethodCallExpression;
			if ((expression == null) || expression.Method.IsStatic) {
				throw new ArgumentException("Lambda expression is not an instance method call");
			}
			return expression.Method;
		}

		public static IEnumerable<Type> GetAllInterfaces() {
			if (typeof(TType).IsInterface) {
				yield return typeof(TType);
			}
			foreach (var intf in typeof(TType).GetInterfaces()) {
				yield return intf;
			}
		}
	}

	public static class Reflect {
		public static MethodInfo GetStaticMethod(Expression<Action> staticMethodCall) {
			var expression = staticMethodCall.Body as MethodCallExpression;
			if (expression == null || !expression.Method.IsStatic) {
				throw new ArgumentException("Lambda expression is not a static method call");
			}
			return expression.Method;
		}

		public static FieldInfo GetStaticField<TResult>(Expression<Func<TResult>> fieldAccess) {
			var expression = fieldAccess.Body as MemberExpression;
			if ((expression == null) || !(expression.Member is FieldInfo) || !((FieldInfo)expression.Member).IsStatic) {
				throw new ArgumentException("Lambda expression is not a static field access");
			}
			return (FieldInfo)expression.Member;
		}

		public static PropertyInfo GetStaticProperty<TResult>(Expression<Func<TResult>> propertyAccess) {
			var expression = propertyAccess.Body as MemberExpression;
			if ((expression == null) || !(expression.Member is PropertyInfo)) {
				throw new ArgumentException("Lambda expression is not a property access");
			}
			return (PropertyInfo)expression.Member;
		}

		public static MemberInfo GetStaticMember<TResult>(Expression<Func<TResult>> memberAccess) {
			var expression = memberAccess.Body as MemberExpression;
			if (expression == null) {
				throw new ArgumentException("Lambda expression is not a member access");
			}
			return expression.Member;
		}

		public static ConstructorInfo GetConstructor(Expression<Action> constructorCall) {
			var expression = constructorCall.Body as NewExpression;
			if (expression == null) {
				throw new ArgumentException("Lambda expression is not a constructor call");
			}
			return expression.Constructor;
		}

		public static Expression AsExpression(this LambdaExpression that) {
			return that.Body;
		}

		public static Type GetEnumerableItemType(this Type type) {
			if (!typeof(IEnumerable).IsAssignableFrom(type)) {
				throw new InvalidOperationException(String.Format("The type {0} does not implement IEnumerable", type.FullName));
			}
			if (type.HasElementType) {
				return type.GetElementType();
			}
			return type.GetInterfaces()
					.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
					.Select(t => t.GetGenericArguments()[0])
					.SingleOrDefault() ?? typeof(object);
		}
	}
}
