using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using CommandDotNet;

namespace PromptPlusCommandDotNet
{
    public static class PromptPlusUtil
    {
        public static string? CopyCallerCommandDotNetDescription([CallerMemberName] string? caller = null)
        {
            var frames = new StackTrace().GetFrames();
            if (frames.Length >= 1)
            {
                var m = frames[1].GetMethod();
                if (m.Name.Equals(caller))
                {
                    var att = m.GetCustomAttributes().FirstOrDefault(a => a as INameAndDescription != null);
                    if (att != null)
                    {
                        return ((INameAndDescription)att).Description;
                    }
                }
            }
            return null;
        }

        internal static PromptPlusTypeKind EnsureValidPromptPlusType(IArgument instance)
        {
            var isPassword = instance.TypeInfo.UnderlyingType == typeof(Password);
            var aux = instance.CustomAttributes.GetCustomAttributes(false).Where(a => a as IPromptType != null);
            var qtd = aux.Count();
            if (qtd == 0)
            {
                return PromptPlusTypeKind.None;
            }
            if (qtd > 1)
            {
                throw new InvalidConfigurationException(
                    $"There is more than one PromptType attribute record for {instance.Name}.");
            }
            if (instance.AllowedValues.Any() || instance.TypeInfo.Type == typeof(bool) || isPassword)
            {
                throw new InvalidConfigurationException(
                 $"There is AllowedValues or type is Password or type is bool for {instance.Name}. Remove PromptPlus Type!");
            }
            var result = PromptPlusTypeKind.None;
            if (aux is not null)
            {
                result = ((IPromptType)aux.First()).TypeKind;
            }
            if (instance.Arity.AllowsMany() && result == PromptPlusTypeKind.Browser)
            {
                throw new InvalidConfigurationException(
                    $"There AllowsMany for {instance.Name} with PromptPlus Type Browser. Remove PromptPlus Type");
            }
            return result;
        }


        internal static T? FindAttribute<T>(IArgument instance) where T : Attribute
        {
            var att = instance.CustomAttributes.GetCustomAttributes(false).FirstOrDefault(a => a as T != null);
            if (att != null)
            {
                return (T)att;
            }
            return null;
        }

        internal static IList<Func<object, ValidationResult>>? ImportUserValidationAttribute(this IArgument argument)
        {
            List<Func<object, ValidationResult>>? result = new();
            var att = argument.CustomAttributes.GetCustomAttributes(true)
                .Where(x => x as ValidationAttribute != null);
            foreach (ValidationAttribute item in att)
            {
                var validationContext = new ValidationContext(argument)
                {
                    DisplayName = argument.TypeInfo.DisplayName ?? argument.Name,
                    MemberName = argument.Name
                };
                ValidationResult func(object input)
                {
                    return item.GetValidationResult(input, validationContext);
                }
                result.Add(func);
            }
            return result;
        }
    }
}
