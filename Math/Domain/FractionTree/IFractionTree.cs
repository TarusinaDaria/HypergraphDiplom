using HypergraphDiplom.Math.Domain.Fraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypergraphDiplom.Math.Domain.FractionTree
{
    public interface IFractionTree
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Fraction.Fraction FindFractionByPath(BitArray path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public BitArray FindPathByFraction(Fraction.Fraction fraction);
    }
}