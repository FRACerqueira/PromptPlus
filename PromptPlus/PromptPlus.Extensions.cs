// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

using PromptPlusControls.Internal;
using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {
        public static IFormPlusBase Step(this IFormPlusBase form, string title, Func<ResultPipe[], object, bool> condition = null, object contextstate = null, string id = null)
        {
            form.PipeId = id ?? Guid.NewGuid().ToString();
            form.PipeTitle = !string.IsNullOrEmpty(title) ? title : Messages.EmptyTitle;
            form.PipeCondition = condition;
            form.ContextState = contextstate;
            return form;
        }

        public static IList<Func<object, ValidationResult>> ImportValidators<T>(this T instance, Expression<Func<T, object>> expression)
        {
            return ImportValidators(instance, expression.Body);
        }

        private static IList<Func<object, ValidationResult>> ImportValidators(object instance, Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(Exceptions.Ex_ExpressionCannotBeNull);
            }

            // Reference type property or field
            if (expression is MemberExpression memberExpression)
            {
                var displayAttribute = memberExpression.Member.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
                return memberExpression.Member.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Select
                    (x =>
                    {
                        var validationContext = new ValidationContext(instance)
                        {
                            DisplayName = displayAttribute == null ? memberExpression.Member.Name : displayAttribute.GetPrompt(),
                            MemberName = memberExpression.Member.Name
                        };
                        Func<object, ValidationResult> func = input => x.GetValidationResult(input, validationContext);
                        return func;
                    })
                    .ToList();
            }
            // Reference type method
            if (expression is MethodCallExpression methodCallExpression)
            {
                var displayAttribute = methodCallExpression.Method.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
                return methodCallExpression.Method.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Select
                    (x =>
                    {
                        var validationContext = new ValidationContext(instance)
                        {
                            DisplayName = displayAttribute == null ? methodCallExpression.Method.Name : displayAttribute.GetPrompt(),
                            MemberName = methodCallExpression.Method.Name
                        };
                        Func<object, ValidationResult> func = input => x.GetValidationResult(input, validationContext);
                        return func;
                    })
                    .ToList();
            }
            // Property, field of method returning value type
            if (expression is UnaryExpression unaryExpression)
            {
                return ImportValidators(instance, unaryExpression);
            }
            throw new ArgumentException(Exceptions.Ex_InvalidExpression);
        }

        private static IList<Func<object, ValidationResult>> ImportValidators(object instance, UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression methodExpression)
            {
                var displayAttribute = methodExpression.Method.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
                return methodExpression.Method.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Select
                (x =>
                {
                    var validationContext = new ValidationContext(instance)
                    {
                        DisplayName = displayAttribute == null ? methodExpression.Method.Name : displayAttribute.GetPrompt(),
                        MemberName = methodExpression.Method.Name
                    };
                    Func<object, ValidationResult> func = input => x.GetValidationResult(input, validationContext);
                    return func;
                })
                .ToList();
            }
            var memberexpress = (MemberExpression)unaryExpression.Operand;
            var displayAttr = memberexpress.Member.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
            return memberexpress.Member.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Select
            (x =>
            {
                var validationContext = new ValidationContext(instance)
                {
                    DisplayName = displayAttr == null ? memberexpress.Member.Name : displayAttr.GetPrompt(),
                    MemberName = memberexpress.Member.Name
                };
                Func<object, ValidationResult> func = input => x.GetValidationResult(input, validationContext);
                return func;
            })
            .ToList();
        }

    }
}
