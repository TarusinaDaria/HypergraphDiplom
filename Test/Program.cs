using HypergraphDiplom.Domain.Extensions;
using HypergraphDiplom.Math.Fraction;
using HypergraphDiplom.Math.FractionTree;
using HypergraphDiplom.Math;
using System.Numerics;
using System.Text;


TestBitsCountInBitArray();
TestHomogenousHypergraphConstruction();
TestHomogenousHypergraphIteration();
TestHomogenousHypergraphEtc();
TestContinuedFraction(new Fraction(44435, 10587));
TestFractionTrees();
TestDavid();
TestWhoIsShorter();
TestSternBrokotTreePaths();
TestVertexIncidence();
TestKeyDecompression();
TestVerticesDegreesVectorConstruction(3, 7);

int GetFractionBitsCount(
        Fraction fraction)
    {
        var bitsForNumerator = Math.Ceiling(BigInteger.Log(fraction.Numerator + 1, 2));
        var bitsForDenominator = Math.Ceiling(BigInteger.Log(fraction.Denominator + 1, 2));

        return (int)(bitsForNumerator + bitsForDenominator);
    }

    void TestBitsCountInBitArray(
        int iterationsCount = 10000)
    {
        for (var i = 0; i < 1001; i++)
        {
            var bitsCountExpected = i;
            var bitArray = new BitArray(bitsCountExpected);
            Console.WriteLine($"[{i:0000}] {(bitsCountExpected == bitArray.BitsCount ? "P" : "Not p")}assed: expected - {bitsCountExpected:0000}, actual - {bitArray.BitsCount:0000}");
        }

        for (var i = 0; i < 1001; i++)
        {
            var bitsCountExpected = i;
            var bitArray = new BitArray(Enumerable.Repeat(false, i));
            Console.WriteLine($"[{i:0000}] {(bitsCountExpected == bitArray.BitsCount ? "P" : "Not p")}assed: expected - {bitsCountExpected:0000}, actual - {bitArray.BitsCount:0000}");
        }
    }

    void TestHomogenousHypergraphConstruction()
    {
        var hg1 = new HomogenousHypergraph(3, 2, new HyperEdge[] { new(0, 1), new(0, 2), new(1, 2) });
    }

    void TestHomogenousHypergraphIteration()
    {
        var hg = new HomogenousHypergraph(3, 2, new HyperEdge[] { new(0, 1), new(0, 2), new(1, 2) });

        for (var i = 0; i < BigIntegerExtensions.CombinationsCount(hg.VerticesCount, hg.SimplicesDimension); i++)
        {
            Console.WriteLine($"{i}: {{{string.Join(", ", hg.BitIndexToSimplex(i))}}} - {(hg.ContainsSimplex(i) ? string.Empty : "not ")}set");
        }
    }

    void TestHomogenousHypergraphEtc()
    {
        var hg = new HomogenousHypergraph(11, 7, new HyperEdge[] { new(10, 9, 8, 7, 6, 5, 4), new(1, 2, 3, 4, 5, 6, 7) });

        foreach (var simplex in hg)
        {
            Console.WriteLine($"{{{simplex}}}");
        }
    }

    void TestContinuedFraction(
        params Fraction[] fractions)
    {
        foreach (var fraction in fractions)
        {
            var i = 0;

            Console.Write($"{fraction} == [ ");
            var continuedFraction = fraction.ToContinuedFraction();
            using var continuedFractionCoefficientsEnumerator = continuedFraction.GetEnumerator();
            if (continuedFractionCoefficientsEnumerator.MoveNext())
            {
                while (true)
                {
                    Console.Write(continuedFractionCoefficientsEnumerator.Current);
                    var enumeratorMoveNextSucceeded = continuedFractionCoefficientsEnumerator.MoveNext();

                    if (i == 0)
                    {
                        i = 1;
                        Console.Write("; ");
                        continue;
                    }

                    if (!enumeratorMoveNextSucceeded)
                    {
                        break;
                    }

                    Console.Write(", ");
                }
            }

            Console.WriteLine($" ] == {continuedFraction.ToFraction()}");
        }
    }

    void TestFractionTrees(
        int pathLengthMinBound = 1000,
        int pathLengthMaxBound = 2000,
        int? randomSource = null)
    {
        randomSource ??= new Random().Next();
        var random = new Random(randomSource.Value);
        Console.WriteLine($"Seed: {randomSource.Value}");

        var pathLength = random.Next(pathLengthMinBound, pathLengthMaxBound);
        Console.WriteLine($"Bits count: {pathLength}");

        var bits = Enumerable
            .Repeat(0, pathLength)
            .Select(_ => random.Next(2) == 0)
            .ToArray();
        var targetPath = new BitArray(bits);

        //Console.WriteLine("Initial path: ");
        //foreach (var pathPart in targetPath)
        //{
        //    Console.Write($"{pathPart} ");
        //}
        //Console.WriteLine();

        var trees = new IFractionTree[] { new SternBrokotTree()}
            .Zip(new[] { "Stern-Brokot tree", "Calkin-Wilf tree" });
        foreach (var tree in trees)
        {
            var restoredFraction = tree.First.FindFractionByPath(targetPath);
            Console.WriteLine($"Total bits for numerator and denominator as numbers == {GetFractionBitsCount(restoredFraction)}");

            //var continuedFraction = restoredFraction.ToContinuedFraction();
            //Console.Write($"Continued fraction: ");
            //foreach (var continuedFractionComponent in continuedFraction)
            //{
            //    Console.Write($"{continuedFractionComponent} ");
            //}
            //Console.WriteLine();

            Console.WriteLine($"Restored fraction by path with {tree.Second}: {restoredFraction}");
            var restoredPath = tree.First.FindPathByFraction(restoredFraction);
            Console.WriteLine("Restored path by restored fraction with {0} is {1}equal to initial path", tree.Second, restoredPath.SequenceEqual(targetPath) ? string.Empty : "not ");
        }
    }

    void TestDavid(
        int? randomSource = null)
    {
        randomSource ??= new Random().Next();
        var random = new Random(randomSource.Value);
        Console.WriteLine($"Seed: {randomSource.Value}");

        var sbTree = new SternBrokotTree();

        var fraction = new BigInteger[] { new(128), new(127), new(127), new(126), new(125), new(123), new(122), new(122), new(120), new(119), new(118), new(117), new(117), new(116), new(115), new(110), new(105), new(101), new(98), new(97) }.ToFraction();
        Console.Write(fraction);
        Console.WriteLine($", total bits for numerator and denominator as numbers == {GetFractionBitsCount(fraction)}");

        var sbTreePath = sbTree.FindPathByFraction(fraction);
        Console.WriteLine($"Stern-Brokot tree representation length = {sbTreePath.BitsCount}");
    }

    void TestWhoIsShorter(
        int iterationsCount = 1000,
        int pathLengthMinBound = 1000,
        int pathLengthMaxBound = 2000,
        int? randomSource = null)
    {
        randomSource ??= new Random().Next();
        var random = new Random(randomSource.Value);
        Console.WriteLine($"Seed: {randomSource.Value}");

        var sbTree = new SternBrokotTree();

        for (var i = 1; i <= iterationsCount; i++)
        {
            var pathLength = random.Next(pathLengthMinBound, pathLengthMaxBound);
            var bits = Enumerable
                .Repeat(0, pathLength)
                .Select(_ => random.Next(2) == 0)
                .ToArray();
            var targetPath = new BitArray(bits);
            var sbTreeFraction = sbTree.FindFractionByPath(targetPath);
            var fractionBitsCount = GetFractionBitsCount(sbTreeFraction);
            var comparisonResult = fractionBitsCount.CompareTo(targetPath.BitsCount);
            Console.Write($"[{i:00000}][Stern-Brokot] path length = {targetPath.BitsCount:00000}, fraction length = {fractionBitsCount:00000} => ");
            Console.Write(comparisonResult switch
            {
                0 => "Equal",
                < 0 => "Fraction",
                _ => "Path"
            });
            Console.WriteLine(" is shorter.");
        }
    }

    void TestSternBrokotTreePaths()
    {
        IFractionTree tree = new SternBrokotTree();
        var builder = new StringBuilder();

        for (var i = 1; i <= 11; i++)
        {
            for (var j = (int)BigInteger.Pow(2, i - 1) - 1; j >= 0; j--)
            {
                var str = Convert.ToString(j, 2);
                builder
                    .Append(new string(Enumerable.Repeat('0', i - (str.Length > i ? i : str.Length)).ToArray()))
                    .Append(str);
                var bitArray = new BitArray(builder.ToString().Select(x => int.Parse(x.ToString()) == 0));
                builder.Clear();
                var fraction = tree.FindFractionByPath(bitArray);
                var difference = fraction.Numerator - fraction.Denominator;

                Console.Write($"{fraction} ");
            }

            Console.WriteLine();
        }
    }

    void TestVertexIncidence()
    {
        var hg = new HomogenousHypergraph(10, 4, new HyperEdge[] { new(0, 1, 2, 3), new(0, 1, 2, 4), new(0, 1, 2, 5), new(0, 1, 2, 6), new(0, 1, 2, 7), new(0, 1, 2, 8), new(0, 1, 2, 9), new(0, 1, 3, 4), new(0, 1, 3, 5), new(0, 1, 3, 6), new(0, 1, 3, 7), new(0, 1, 3, 8), new(0, 1, 3, 9), new(0, 1, 4, 5), new(0, 1, 4, 6), new(0, 1, 4, 7), new(0, 1, 4, 8), new(0, 1, 4, 9), new(0, 1, 5, 6), new(0, 1, 5, 7), new(0, 1, 5, 8), new(0, 1, 5, 9), new(0, 1, 6, 7), new(0, 1, 6, 8), new(0, 1, 6, 9), new(0, 1, 7, 8), new(0, 1, 7, 9), new(0, 1, 8, 9) });

        foreach (var simplex in hg.GetSimplicesIncidentToVertex(9))
        {
            Console.WriteLine($"{{{simplex}}}");
        }
    }

    void TestKeyDecompression()
    {
        var initial = new VerticesDegreesVector(250, 249, 249, 249, 248, 248, 248, 247, 247, 247, 247, 247, 246);
        var diffsTransformed = VerticesDegreesVectorCompressor.ToDiffArray(initial);
        var transformed = VerticesDegreesVectorCompressor.Compress(initial);
        var diffsTransformedBack = VerticesDegreesVectorCompressor.FromDiffArray(diffsTransformed);
        var transformedBack = VerticesDegreesVectorCompressor.Decompress(transformed);

        var sb = new SternBrokotTree();
        var cf = diffsTransformed.Select(x => new BigInteger(x)).ToFraction();
        var path = sb.FindPathByFraction(cf);
        var cf2 = sb.FindFractionByPath(path);
        var diffsFromCf = cf2.ToContinuedFraction().Select(x => (int)x).ToArray();

        foreach (var item in initial)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();
        foreach (var item in diffsTransformed)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();
        foreach (var item in transformed)
        {
            Console.Write($"{(item ? 1 : 0)}");
        }
        Console.WriteLine();
        foreach (var item in diffsTransformedBack)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();
        foreach (var item in transformedBack)
        {
            Console.Write($"{item} ");
        }
    }

    void TestVerticesDegreesVectorConstruction(
        int simplicesDimension,
        int verticesCount)
    {
        BigInteger count = 0;
        foreach (var generatedVerticesDegreesVector in VerticesDegreesVectorConstructor.ConstructAllExtremalVerticesDegreesVectors(verticesCount, simplicesDimension))
        {
            ++count;
            foreach (var item in generatedVerticesDegreesVector)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
        }
        Console.Write($"Count: {count}");
    }

static class VerticesDegreesVectorCompressor
    {

        private static readonly SternBrokotTree _fractionTree = new();

        public static BitArray Compress(
            VerticesDegreesVector vector)
        {
            var diffArray = ToDiffArray(vector);
            var firstValue = diffArray[0];
            var firstValueBitsCount = 0;
            while (firstValue != 0)
            {
                ++firstValueBitsCount;
                firstValue >>= 1;
            }

            var result = new BitArray(sizeof(byte) * 8 + firstValueBitsCount + diffArray.Skip(1).Sum() - 1);

            for (var i = 0; i < sizeof(byte) * 8; ++i)
            {
                result[i] = ((firstValueBitsCount >> i) & 1) == 1;
            }

            for (var i = 0; i < firstValueBitsCount; ++i)
            {
                result[i + sizeof(byte) * 8] = ((diffArray[0] >> i) & 1) == 1;
            }

            var convertedPath = _fractionTree.FindPathByFraction(diffArray.Skip(1).Select(x => new BigInteger(x)).ToFraction());
            foreach (var convertedPathBit in convertedPath)
            {
                result[sizeof(byte) * 8 + firstValueBitsCount++] = convertedPathBit;
            }

            return result;
        }

        public static VerticesDegreesVector Decompress(
            BitArray transformedVerticesDegreesVector)
        {
            byte firstValueBitsCount = 0;
            for (var i = 0; i < sizeof(byte) * 8; ++i)
            {
                if (!transformedVerticesDegreesVector[i])
                {
                    continue;
                }

                firstValueBitsCount |= (byte)(1 << i);
            }

            int firstValue = 0;
            for (var i = 0; i < firstValueBitsCount; ++i)
            {
                if (!transformedVerticesDegreesVector[sizeof(byte) * 8 + i])
                {
                    continue;
                }

                firstValue |= 1 << i;
            }

            var diffArray = _fractionTree.FindFractionByPath(
                    new BitArray(transformedVerticesDegreesVector.Skip(sizeof(byte) * 8 + firstValueBitsCount)))
                .ToContinuedFraction()
                .Select(x => (int)x)
                .Prepend(firstValue)
                .ToArray();

            return FromDiffArray(diffArray);
        }



        public static int[] ToDiffArray(
            VerticesDegreesVector vector)
        {
            var degreesValues = vector.Reverse().ToArray();
            var result = new int[degreesValues.Length];

            var previousDegree = result[0] = degreesValues.First();
            degreesValues.Skip(1).ForEach((currentDegree, index) =>
            {
                result[index + 1] = currentDegree - previousDegree + 1;
                previousDegree = currentDegree;
            });
            ++result[^1];

            return result;
        }

        public static VerticesDegreesVector FromDiffArray(
            int[] diffArray)
        {
            var modifiedDiffsArray = (int[])diffArray.Clone();
            --modifiedDiffsArray[^1];

            for (var i = 1; i < modifiedDiffsArray.Length; ++i)
            {
                modifiedDiffsArray[i] += modifiedDiffsArray[i - 1];
                --modifiedDiffsArray[i];
            }

            return new VerticesDegreesVector(modifiedDiffsArray.Reverse().ToArray());
        }

    }

    static class VerticesDegreesVectorConstructor
    {

        public static IEnumerable<VerticesDegreesVector> ConstructAllVerticesDegreesVectors(
            int verticesCount,
            int simplicesDimension)
        {
            IEnumerable<VerticesDegreesVector> Generate(
                int[] resultVerticesDegreesVectorCoefficients,
                BigInteger setUpVerticesDegreesSum,
                int vertexDegreeIndexToSetUp,
                BigInteger maxVertexDegree)
            {
                if (setUpVerticesDegreesSum < BigInteger.Zero)
                {
                    yield break;
                }

                if (vertexDegreeIndexToSetUp == resultVerticesDegreesVectorCoefficients.Length)
                {
                    if (setUpVerticesDegreesSum == 0)
                    {
                        yield return new VerticesDegreesVector(resultVerticesDegreesVectorCoefficients);
                    }

                    yield break;
                }

                for (var i = 1; i <= (vertexDegreeIndexToSetUp == 0
                         ? maxVertexDegree
                         : resultVerticesDegreesVectorCoefficients[vertexDegreeIndexToSetUp - 1]); ++i)
                {
                    resultVerticesDegreesVectorCoefficients[vertexDegreeIndexToSetUp] = i;
                    foreach (var generatedVerticesDegreesVector in Generate(resultVerticesDegreesVectorCoefficients, setUpVerticesDegreesSum - i, vertexDegreeIndexToSetUp + 1, maxVertexDegree))
                    {
                        yield return generatedVerticesDegreesVector;
                    }
                }
            }

            var minSum = verticesCount * (BigInteger)2;
            var maxVertexDegree = BigIntegerExtensions.CombinationsCount(verticesCount - 1, simplicesDimension - 1);
            var maxSum = verticesCount * maxVertexDegree;

            // TODO: Guardant

            for (var sum = 0; sum <= maxSum; sum += simplicesDimension)
            {
                foreach (var generatedVerticesDegreesVector in Generate(new int[verticesCount], sum, 0, maxVertexDegree))
                {
                    yield return generatedVerticesDegreesVector;
                }
            }
        }

        public static IEnumerable<VerticesDegreesVector> ConstructAllConnectedVerticesDegreesVectors(
            int verticesCount,
            int simplicesDimension)
        {
            // TODO: implement me plz
            return ConstructAllVerticesDegreesVectors(verticesCount, simplicesDimension)
                .Select(x => x);
        }

        public static IEnumerable<VerticesDegreesVector> ConstructAllTheOnlyWayRestorableVerticesDegreesVectors(
            int verticesCount,
            int simplicesDimension)
        {
            // TODO: implement me plz
            return ConstructAllVerticesDegreesVectors(verticesCount, simplicesDimension)
                .Select(x => x);
        }

        public static IEnumerable<VerticesDegreesVector> ConstructAllExtremalVerticesDegreesVectors(
            int verticesCount,
            int simplicesDimension)
        {
            // TODO: implement me plz
            return ConstructAllVerticesDegreesVectors(verticesCount, simplicesDimension)
                .Select(x => x);
        }

    }
