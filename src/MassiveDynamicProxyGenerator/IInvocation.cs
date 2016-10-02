using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    public interface IInvocation
    {
        /// <summary>
        /// Vráti návratovú hodnotu zavolanej metódy.
        /// </summary>
        /// <value>
        /// Návratová hodnota zavolanej metódy.
        /// </value>
        object ReturnValue
        {
            get;
            set;
        }

        /// <summary>
        /// Vráti pole argumentov olanej metódy.
        /// </summary>
        /// <value>
        /// Pole argumentov olanej metódy.
        /// </value>
        object[] Arguments
        {
            get;
        }

        Type OriginalType
        {
            get;
        }

        string MethodName
        {
            get;
        }

        Type[] ArgumentTypes
        {
            get;
        }

        Type ReturnType
        {
            get;
        }

        /// <summary>
        /// Vráti <see cref="MethodBase"/> ako reprezentáciu práve interceptovanej metódy.
        /// </summary>
        /// <returns><see cref="MethodBase"/> interceptovanej metódy.</returns>
        MethodBase GetConcreteMethod();
    }
}
