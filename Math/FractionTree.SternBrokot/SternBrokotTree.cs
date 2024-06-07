﻿using HypergraphDiplom.Domain.Helpers.Guarding;
using HypergraphDiplom.Math.Domain.Fraction;
using HypergraphDiplom.Math.Domain.FractionTree;
using HypergraphDiplom.Math.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HypergraphDiplom.Math.FractionTree.SternBrokot
{
    public sealed class SternBrokotTree : IFractionTree
    {

        /// <inheritdoc cref="IFractionTree.FindFractionByPath" />
        public Fraction FindFractionByPath(
            BitArray path)
        {
            Guardant.Instance
                .ThrowIfNull(path);

            var numerator = BigInteger.One;
            var denominator = BigInteger.One;
            var leftMediantNumerator = BigInteger.Zero;
            var leftMediantDenominator = BigInteger.One;
            var rightMediantNumerator = BigInteger.One;
            var rightMediantDenominator = BigInteger.Zero;

            foreach (var pathPart in path)
            {
                _ = pathPart
                    ? (leftMediantNumerator, leftMediantDenominator) = (numerator, denominator)
                    : (rightMediantNumerator, rightMediantDenominator) = (numerator, denominator);

                numerator = leftMediantNumerator + rightMediantNumerator;
                denominator = leftMediantDenominator + rightMediantDenominator;
            }

            return new Fraction(numerator, denominator);
        }

        /// <inheritdoc cref="IFractionTree.FindPathByFraction" />
        public BitArray FindPathByFraction(
            Fraction fraction)
        {
            var (approximationNumerator, approximationDenominator) = (BigInteger.One, BigInteger.One);
            var (leftMediantNumerator, leftMediantDenominator) = (BigInteger.Zero, BigInteger.One);
            var (rightMediantNumerator, rightMediantDenominator) = (BigInteger.One, BigInteger.Zero);
            var path = new List<bool>();

            var fractionsComparisonResult = (approximationNumerator * fraction.Denominator).CompareTo(approximationDenominator * fraction.Numerator);
            while (fractionsComparisonResult != 0)
            {
                path.Add(fractionsComparisonResult < 0);
                _ = path[^1]
                    ? (leftMediantNumerator, leftMediantDenominator) = (approximationNumerator, approximationDenominator)
                    : (rightMediantNumerator, rightMediantDenominator) = (approximationNumerator, approximationDenominator);

                approximationNumerator = leftMediantNumerator + rightMediantNumerator;
                approximationDenominator = leftMediantDenominator + rightMediantDenominator;

                fractionsComparisonResult = (approximationNumerator * fraction.Denominator).CompareTo(approximationDenominator * fraction.Numerator);
            }

            return new BitArray(path);
        }

    }
}