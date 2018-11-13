using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sirius {
	/// <summary>Reflection instance member helper methods.</summary>
	/// <typeparam name="TType">Type of the type to reflect.</typeparam>
	public static class Reflect<TType> {
		/// <summary>The default value of <typeparamref name="TType"/>.</summary>
		public static TType Default;

		/// <summary>Gets a <see cref="PropertyInfo"/>.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <typeparam name="TResult">Type of the expression. Not used.</typeparam>
		/// <param name="propertyAccess">The property access expression.</param>
		/// <returns>The property info.</returns>
		/// <example>var stringLengthPropertyInfo = Reflect&lt;string&gt;.GetProperty(s => s.Length);</example>
		[Pure]
		public static PropertyInfo GetProperty<TResult>(Expression<Func<TType, TResult>> propertyAccess) {
			if ((!(propertyAccess.Body is MemberExpression expression)) || !(expression.Member is PropertyInfo)) {
				if ((propertyAccess.Body is MethodCallExpression callExpression) && (callExpression.Method.DeclaringType != null)) {
					foreach (var property in callExpression.Method.DeclaringType.GetProperties()) {
						if ((property.GetGetMethod() == callExpression.Method) || (property.GetSetMethod() == callExpression.Method)) {
							return property;
						}
					}
				}
				throw new ArgumentException("Lambda expression is not a property access");
			}
			return (PropertyInfo)expression.Member;
		}

		/// <summary>Gets a <see cref="MemberInfo"/>.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <typeparam name="TResult">Type of the expression. Not used.</typeparam>
		/// <param name="memberAccess">The member access expression.</param>
		/// <returns>The member info.</returns>
		[Pure]
		public static MemberInfo GetMember<TResult>(Expression<Func<TType, TResult>> memberAccess) {
			if (!(memberAccess.Body is MemberExpression expression)) {
				throw new ArgumentException("Lambda expression is not a member access");
			}
			return expression.Member;
		}

		/// <summary>Gets a <see cref="FieldInfo"/>.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <typeparam name="TResult">Type of the expression. Not used.</typeparam>
		/// <param name="fieldAccess">The field access expression.</param>
		/// <returns>The field info.</returns>
		[Pure]
		public static FieldInfo GetField<TResult>(Expression<Func<TType, TResult>> fieldAccess) {
			if (!(fieldAccess.Body is MemberExpression expression) || !(expression.Member is FieldInfo fieldInfo) || fieldInfo.IsStatic) {
				throw new ArgumentException("Lambda expression is not an instance field access");
			}
			return fieldInfo;
		}

		/// <summary>Gets a <see cref="MethodInfo"/>.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="methodCall">The method call expression.</param>
		/// <returns>The method info.</returns>
		/// <example>var objectToStringMethodInfo = Reflect&lt;object&gt;.GetMethod(o => o.ToString());</example>
		/// <remarks>For parameters, use <c>default(...)</c> values which allow to clearly identify the overload to pick without capturing and real value.</remarks>
		[Pure]
		public static MethodInfo GetMethod(Expression<Action<TType>> methodCall) {
			if (!(methodCall.Body is MethodCallExpression expression) || expression.Method.IsStatic) {
				throw new ArgumentException("Lambda expression is not an instance method call");
			}
			return expression.Method;
		}

		/// <summary>Gets all interfaces of the type <typeparamref name="TType"/>, including itself if it is an interface.</summary>
		/// <returns>All interfaces of <typeparamref name="TType"/>.</returns>
		[Pure]
		public static IEnumerable<Type> GetAllInterfaces() {
			if (typeof(TType).IsInterface) {
				yield return typeof(TType);
			}
			foreach (var @interface in typeof(TType).GetInterfaces()) {
				yield return @interface;
			}
		}
	}

	/// <summary>Reflection static member helper methods.</summary>
	public static class Reflect {
		/// <summary>Gets a <see cref="MethodInfo"/>.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="staticMethodCall">The static method call expression.</param>
		/// <returns>The method info.</returns>
		/// <example>var objectEqualsMethodInfo = Reflect.GetStaticMethod(o => o.Equals(default(object), default(object)));</example>
		/// <remarks>For parameters, use <c>default(...)</c> values which allow to clearly identify the overload to pick without capturing and real value.</remarks>
		[Pure]
		public static MethodInfo GetStaticMethod(Expression<Action> staticMethodCall) {
			if (!(staticMethodCall.Body is MethodCallExpression expression) || !expression.Method.IsStatic) {
				throw new ArgumentException("Lambda expression is not a static method call");
			}
			return expression.Method;
		}

		/// <summary>Gets a <see cref="FieldInfo"/>.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="fieldAccess">The static field access expression.</param>
		/// <returns>The field info.</returns>
		[Pure]
		public static FieldInfo GetStaticField<TResult>(Expression<Func<TResult>> fieldAccess) {
			if ((!(fieldAccess.Body is MemberExpression expression)) || !(expression.Member is FieldInfo) || !((FieldInfo)expression.Member).IsStatic) {
				throw new ArgumentException("Lambda expression is not a static field access");
			}
			return (FieldInfo)expression.Member;
		}

		/// <summary>Gets a <see cref="PropertyInfo"/>.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="propertyAccess">The static property access expression.</param>
		/// <returns>The property info.</returns>
		[Pure]
		public static PropertyInfo GetStaticProperty<TResult>(Expression<Func<TResult>> propertyAccess) {
			if ((!(propertyAccess.Body is MemberExpression expression)) || !(expression.Member is PropertyInfo)) {
				throw new ArgumentException("Lambda expression is not a property access");
			}
			return (PropertyInfo)expression.Member;
		}

		/// <summary>Gets a <see cref="MemberInfo"/>.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="memberAccess">The static member access expression.</param>
		/// <returns>The member info.</returns>
		[Pure]
		public static MemberInfo GetStaticMember<TResult>(Expression<Func<TResult>> memberAccess) {
			if (!(memberAccess.Body is MemberExpression expression)) {
				throw new ArgumentException("Lambda expression is not a member access");
			}
			return expression.Member;
		}

		/// <summary>Gets a <see cref="ConstructorInfo"/>.</summary>
		/// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
		/// <param name="constructorCall">The constructor call (new) expression.</param>
		/// <returns>The constructor info.</returns>
		/// <example>var objectCtorInfo = Reflect.GetConstructor(() => new Object());</example>
		[Pure]
		public static ConstructorInfo GetConstructor(Expression<Action> constructorCall) {
			if (!(constructorCall.Body is NewExpression expression)) {
				throw new ArgumentException("Lambda expression is not a constructor call");
			}
			return expression.Constructor;
		}

		/// <summary>Get the lambda as expression tree.</summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="that">The lambda expression.</param>
		/// <returns>An Expression.</returns>
		[Pure]
		public static Expression AsExpression<T>(this Expression<T> that) {
			return that.Body;
		}

		/// <summary>Gets the item/element type of an enumerable type.</summary>
		/// <exception cref="InvalidOperationException">Thrown when the requested operation is invalid.</exception>
		/// <param name="type">The type to act on.</param>
		/// <returns>The item type.</returns>
		/// <example>var t = typeof(string).GetEnumerableItemType(); // t == typeof(char)</example>
		[Pure]
		public static Type GetEnumerableItemType(this Type type) {
			if (!typeof(IEnumerable).IsAssignableFrom(type)) {
				throw new InvalidOperationException(string.Format("The type {0} does not implement IEnumerable", type.FullName));
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
