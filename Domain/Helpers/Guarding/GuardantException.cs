using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomHypergraph.Domain.Helpers.Guarding
{
    public sealed class GuardantException : Exception
    {

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public GuardantException(
            string message,
            Exception? innerException = null) : base(message, innerException)
        {

        }

        #endregion

    }
}
