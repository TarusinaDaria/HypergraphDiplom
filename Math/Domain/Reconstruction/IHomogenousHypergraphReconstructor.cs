using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HypergraphDiplom.Math.Domain.Reconstruction
{
    public interface IHomogenousHypergraphReconstructor<in T>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="simplicesDimension"></param>
        /// <returns></returns>
        HomogenousHypergraph? Restore(
            T from,
            int simplicesDimension);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="simplicesDimension"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HomogenousHypergraph?> RestoreAsync(
            T from,
            int simplicesDimension,
            CancellationToken cancellationToken = default);

    }
}