using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class CustomFluentValidatorExtensions
    {
        public static IRuleBuilderOptions<T, IList<TElement>> IsValidaDateTime<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder,int num)
        {
            return ruleBuilder.Must(list => list.Count < num).WithMessage("The list contains too many items");
        }
    }
}
