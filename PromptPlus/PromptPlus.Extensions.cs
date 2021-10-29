// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************
//  Extension color Inspired by the work https://github.com/colored-console/colored-console
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

using PromptPlusControls.Resources;
using PromptPlusControls.ValueObjects;

namespace PromptPlusControls
{
    public static partial class PromptPlus
    {
        #region Validators

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

        #endregion

        #region colors

        public static ColorToken[] Mask(this IEnumerable<ColorToken> tokens, ConsoleColor? color = null, ConsoleColor? backgroundColor = null)
        {
            return tokens?.Select(token => token.Mask(color, backgroundColor)).ToArray();
        }

        public static ColorToken DefautColor(this string text)
        {
            return new ColorToken(text, _consoleDriver.ForegroundColor, _consoleDriver.BackgroundColor);
        }

        public static ColorToken Color(this string text, ConsoleColor forecolor, ConsoleColor? backcolor = null)
        {
            return new ColorToken(text, forecolor, backcolor);
        }

        public static ColorToken Black(this string text)
        {
            return text.Color(ConsoleColor.Black);
        }

        public static ColorToken Blue(this string text)
        {
            return text.Color(ConsoleColor.Blue);
        }

        public static ColorToken Cyan(this string text)
        {
            return text.Color(ConsoleColor.Cyan);
        }

        public static ColorToken DarkBlue(this string text)
        {
            return text.Color(ConsoleColor.DarkBlue);
        }

        public static ColorToken DarkCyan(this string text)
        {
            return text.Color(ConsoleColor.DarkCyan);
        }

        public static ColorToken DarkGray(this string text)
        {
            return text.Color(ConsoleColor.DarkGray);
        }

        public static ColorToken DarkGreen(this string text)
        {
            return text.Color(ConsoleColor.DarkGreen);
        }

        public static ColorToken DarkMagenta(this string text)
        {
            return text.Color(ConsoleColor.DarkMagenta);
        }

        public static ColorToken DarkRed(this string text)
        {
            return text.Color(ConsoleColor.DarkRed);
        }

        public static ColorToken DarkYellow(this string text)
        {
            return text.Color(ConsoleColor.DarkYellow);
        }

        public static ColorToken Gray(this string text)
        {
            return text.Color(ConsoleColor.Gray);
        }

        public static ColorToken Green(this string text)
        {
            return text.Color(ConsoleColor.Green);
        }

        public static ColorToken Magenta(this string text)
        {
            return text.Color(ConsoleColor.Magenta);
        }

        public static ColorToken Underline(this string text)
        {
            return text.DefautColor().Underline();
        }

        public static ColorToken Red(this string text)
        {
            return text.Color(ConsoleColor.Red);
        }

        public static ColorToken White(this string text)
        {
            return text.Color(ConsoleColor.White);
        }

        public static ColorToken Yellow(this string text) => text.Color(ConsoleColor.Yellow);

        public static ColorToken On(this string text, ConsoleColor? backgroundColor)
        {
            return new ColorToken(text, null, backgroundColor);
        }

        public static ColorToken OnBlack(this string text)
        {
            return text.On(ConsoleColor.Black);
        }

        public static ColorToken OnBlue(this string text)
        {
            return text.On(ConsoleColor.Blue);
        }

        public static ColorToken OnCyan(this string text)
        {
            return text.On(ConsoleColor.Cyan);
        }

        public static ColorToken OnDarkBlue(this string text)
        {
            return text.On(ConsoleColor.DarkBlue);
        }

        public static ColorToken OnDarkCyan(this string text)
        {
            return text.On(ConsoleColor.DarkCyan);
        }

        public static ColorToken OnDarkGray(this string text)
        {
            return text.On(ConsoleColor.DarkGray);
        }

        public static ColorToken OnDarkGreen(this string text)
        {
            return text.On(ConsoleColor.DarkGreen);
        }

        public static ColorToken OnDarkMagenta(this string text)
        {
            return text.On(ConsoleColor.DarkMagenta);
        }

        public static ColorToken OnDarkRed(this string text)
        {
            return text.On(ConsoleColor.DarkRed);
        }

        public static ColorToken OnDarkYellow(this string text)
        {
            return text.On(ConsoleColor.DarkYellow);
        }

        public static ColorToken OnGray(this string text)
        {
            return text.On(ConsoleColor.Gray);
        }

        public static ColorToken OnGreen(this string text)
        {
            return text.On(ConsoleColor.Green);
        }

        public static ColorToken OnMagenta(this string text)
        {
            return text.On(ConsoleColor.Magenta);
        }

        public static ColorToken OnRed(this string text)
        {
            return text.On(ConsoleColor.Red);
        }

        public static ColorToken OnWhite(this string text)
        {
            return text.On(ConsoleColor.White);
        }

        public static ColorToken OnYellow(this string text)
        {
            return text.On(ConsoleColor.Yellow);
        }

        public static ColorToken Underline(this ColorToken token)
        {
            token.Underline = true;
            return token;
        }

        public static ColorToken On(this ColorToken token, ConsoleColor? backgroundColor)
        {
            return new ColorToken(token.Text, token.Color, backgroundColor);
        }

        public static ColorToken OnBlack(this ColorToken token)
        {
            return token.On(ConsoleColor.Black);
        }

        public static ColorToken OnBlue(this ColorToken token)
        {
            return token.On(ConsoleColor.Blue);
        }

        public static ColorToken OnCyan(this ColorToken token)
        {
            return token.On(ConsoleColor.Cyan);
        }

        public static ColorToken OnDarkBlue(this ColorToken token)
        {
            return token.On(ConsoleColor.DarkBlue);
        }

        public static ColorToken OnDarkCyan(this ColorToken token)
        {
            return token.On(ConsoleColor.DarkCyan);
        }

        public static ColorToken OnDarkGray(this ColorToken token)
        {
            return token.On(ConsoleColor.DarkGray);
        }

        public static ColorToken OnDarkGreen(this ColorToken token)
        {
            return token.On(ConsoleColor.DarkGreen);
        }

        public static ColorToken OnDarkMagenta(this ColorToken token)
        {
            return token.On(ConsoleColor.DarkMagenta);
        }

        public static ColorToken OnDarkRed(this ColorToken token)
        {
            return token.On(ConsoleColor.DarkRed);
        }

        public static ColorToken OnDarkYellow(this ColorToken token)
        {
            return token.On(ConsoleColor.DarkYellow);
        }

        public static ColorToken OnGray(this ColorToken token)
        {
            return token.On(ConsoleColor.Gray);
        }

        public static ColorToken OnGreen(this ColorToken token)
        {
            return token.On(ConsoleColor.Green);
        }

        public static ColorToken OnMagenta(this ColorToken token)
        {
            return token.On(ConsoleColor.Magenta);
        }

        public static ColorToken OnRed(this ColorToken token)
        {
            return token.On(ConsoleColor.Red);
        }

        public static ColorToken OnWhite(this ColorToken token)
        {
            return token.On(ConsoleColor.White);
        }

        public static ColorToken OnYellow(this ColorToken token)
        {
            return token.On(ConsoleColor.Yellow);
        }

        public static ColorToken Color(this ColorToken token, ConsoleColor? color)
        {
            return new ColorToken(token.Text, color, token.BackgroundColor);
        }

        public static ColorToken Black(this ColorToken token)
        {
            return token.Color(ConsoleColor.Black);
        }

        public static ColorToken Blue(this ColorToken token)
        {
            return token.Color(ConsoleColor.Blue);
        }

        public static ColorToken Cyan(this ColorToken token)
        {
            return token.Color(ConsoleColor.Cyan);
        }

        public static ColorToken DarkBlue(this ColorToken token)
        {
            return token.Color(ConsoleColor.DarkBlue);
        }

        public static ColorToken DarkCyan(this ColorToken token)
        {
            return token.Color(ConsoleColor.DarkCyan);
        }

        public static ColorToken DarkGray(this ColorToken token)
        {
            return token.Color(ConsoleColor.DarkGray);
        }

        public static ColorToken DarkGreen(this ColorToken token)
        {
            return token.Color(ConsoleColor.DarkGreen);
        }

        public static ColorToken DarkMagenta(this ColorToken token)
        {
            return token.Color(ConsoleColor.DarkMagenta);
        }

        public static ColorToken DarkRed(this ColorToken token)
        {
            return token.Color(ConsoleColor.DarkRed);
        }

        public static ColorToken DarkYellow(this ColorToken token)
        {
            return token.Color(ConsoleColor.DarkYellow);
        }

        public static ColorToken Gray(this ColorToken token)
        {
            return token.Color(ConsoleColor.Gray);
        }

        public static ColorToken Green(this ColorToken token)
        {
            return token.Color(ConsoleColor.Green);
        }

        public static ColorToken Magenta(this ColorToken token)
        {
            return token.Color(ConsoleColor.Magenta);
        }

        public static ColorToken Red(this ColorToken token)
        {
            return token.Color(ConsoleColor.Red);
        }

        public static ColorToken White(this ColorToken token)
        {
            return token.Color(ConsoleColor.White);
        }

        public static ColorToken Yellow(this ColorToken token)
        {
            return token.Color(ConsoleColor.Yellow);
        }

        #endregion
    }
}
