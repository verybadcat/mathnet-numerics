﻿// <copyright file="Hypergeometric.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2014 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Collections.Generic;
using MathNet.Numerics.Properties;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Distributions
{
    /// <summary>
    /// Discrete Univariate Hypergeometric distribution.
    /// This distribution is a discrete probability distribution that describes the number of successes in a sequence
    /// of n draws from a finite population without replacement, just as the binomial distribution
    /// describes the number of successes for draws with replacement
    /// <a href="http://en.wikipedia.org/wiki/Hypergeometric_distribution">Wikipedia - Hypergeometric distribution</a>.
    /// </summary>
    public class Hypergeometric : IDiscreteDistribution
    {
        System.Random _random;

        int _population;
        int _success;
        int _draws;

        /// <summary>
        /// Initializes a new instance of the Hypergeometric class.
        /// </summary>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        public Hypergeometric(int population, int success, int draws)
        {
            _random = SystemRandomSource.Default;
            SetParameters(population, success, draws);
        }

        /// <summary>
        /// Initializes a new instance of the Hypergeometric class.
        /// </summary>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        /// <param name="randomSource">The random number generator which is used to draw random samples.</param>
        public Hypergeometric(int population, int success, int draws, System.Random randomSource)
        {
            _random = randomSource ?? SystemRandomSource.Default;
            SetParameters(population, success, draws);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Hypergeometric(N = " + _population + ", M = " + _success + ", n = " + _draws + ")";
        }

        /// <summary>
        /// Sets the parameters of the distribution after checking their validity.
        /// </summary>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        /// <exception cref="ArgumentOutOfRangeException">When the parameters are out of range.</exception>
        void SetParameters(int population, int success, int draws)
        {
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population))
            {
                throw new ArgumentException(Resources.InvalidDistributionParameters);
            }

            _population = population;
            _success = success;
            _draws = draws;
        }

        /// <summary>
        /// Gets or sets the random number generator which is used to draw random samples.
        /// </summary>
        public System.Random RandomSource
        {
            get { return _random; }
            set { _random = value ?? SystemRandomSource.Default; }
        }

        /// <summary>
        /// Gets or sets the size of the population (N).
        /// </summary>
        public int Population
        {
            get { return _population; }
            set { SetParameters(value, _success, _draws); }
        }

        /// <summary>
        /// Gets or sets the number of draws without replacement (n).
        /// </summary>
        public int Draws
        {
            get { return _draws; }
            set { SetParameters(_population, value, _draws); }
        }

        /// <summary>
        /// Gets or sets the number successes within the population (K, M).
        /// </summary>
        public int Success
        {
            get { return _success; }
            set { SetParameters(_population, _success, value); }
        }

        /// <summary>
        /// Gets or sets the size of the population (N).
        /// </summary>
        [Obsolete("Use Population instead. Scheduled for removal in v3.0.")]
        public int PopulationSize
        {
            get { return _population; }
            set { SetParameters(value, _success, _draws); }
        }

        /// <summary>
        /// Gets or sets the number of draws without replacement (n).
        /// </summary>
        [Obsolete("Use Draws instead. Scheduled for removal in v3.0.")]
        public int N
        {
            get { return _draws; }
            set { SetParameters(_population, value, _draws); }
        }

        /// <summary>
        /// Gets or sets the number successes within the population (K, M).
        /// </summary>
        [Obsolete("Use Success instead. Scheduled for removal in v3.0.")]
        public int M
        {
            get { return _success; }
            set { SetParameters(_population, _success, value); }
        }

        /// <summary>
        /// Gets the mean of the distribution.
        /// </summary>
        public double Mean
        {
            get { return (double)_success*_draws/_population; }
        }

        /// <summary>
        /// Gets the variance of the distribution.
        /// </summary>
        public double Variance
        {
            get { return _draws*_success*(_population - _draws)*(_population - _success)/(_population*_population*(_population - 1.0)); }
        }

        /// <summary>
        /// Gets the standard deviation of the distribution.
        /// </summary>
        public double StdDev
        {
            get { return Math.Sqrt(Variance); }
        }

        /// <summary>
        /// Gets the entropy of the distribution.
        /// </summary>
        public double Entropy
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the skewness of the distribution.
        /// </summary>
        public double Skewness
        {
            get { return (Math.Sqrt(_population - 1.0)*(_population - (2*_draws))*(_population - (2*_success)))/(Math.Sqrt(_draws*_success*(_population - _success)*(_population - _draws))*(_population - 2.0)); }
        }

        /// <summary>
        /// Gets the mode of the distribution.
        /// </summary>
        public int Mode
        {
            get { return (_draws + 1)*(_success + 1)/(_population + 2); }
        }

        /// <summary>
        /// Gets the median of the distribution.
        /// </summary>
        public int Median
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the minimum of the distribution.
        /// </summary>
        public int Minimum
        {
            get { return Math.Max(0, _draws + _success - _population); }
        }

        /// <summary>
        /// Gets the maximum of the distribution.
        /// </summary>
        public int Maximum
        {
            get { return Math.Min(_success, _draws); }
        }

        /// <summary>
        /// Computes the probability mass (PMF) at k, i.e. P(X = k).
        /// </summary>
        /// <param name="k">The location in the domain where we want to evaluate the probability mass function.</param>
        /// <returns>the probability mass at location <paramref name="k"/>.</returns>
        public double Probability(int k)
        {
            return SpecialFunctions.Binomial(_success, k)*SpecialFunctions.Binomial(_population - _success, _draws - k)/SpecialFunctions.Binomial(_population, _draws);
        }

        /// <summary>
        /// Computes the log probability mass (lnPMF) at k, i.e. ln(P(X = k)).
        /// </summary>
        /// <param name="k">The location in the domain where we want to evaluate the log probability mass function.</param>
        /// <returns>the log probability mass at location <paramref name="k"/>.</returns>
        public double ProbabilityLn(int k)
        {
            return Math.Log(Probability(k));
        }

        /// <summary>
        /// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
        public double CumulativeDistribution(double x)
        {
            if (x < Minimum)
            {
                return 0.0;
            }
            if (x >= Maximum)
            {
                return 1.0;
            }

            var k = (int)Math.Floor(x);
            var denominatorLn = SpecialFunctions.BinomialLn(_population, _draws);
            var sum = 0.0;
            for (var i = 0; i <= k; i++)
            {
                sum += Math.Exp(SpecialFunctions.BinomialLn(_success, i) + SpecialFunctions.BinomialLn(_population - _success, _draws - i) - denominatorLn);
            }
            return sum;
        }

        /// <summary>
        /// Generates a sample from the Hypergeometric distribution without doing parameter checking.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The n parameter of the distribution.</param>
        /// <returns>a random number from the Hypergeometric distribution.</returns>
        static int SampleUnchecked(System.Random rnd, int population, int success, int draws)
        {
            var x = 0;

            do
            {
                var p = (double)success/population;
                var r = rnd.NextDouble();
                if (r < p)
                {
                    x++;
                    success--;
                }

                population--;
                draws--;
            } while (0 < draws);

            return x;
        }

        /// <summary>
        /// Samples a Hypergeometric distributed random variable.
        /// </summary>
        /// <returns>The number of successes in n trials.</returns>
        public int Sample()
        {
            return SampleUnchecked(_random, _population, _success, _draws);
        }

        /// <summary>
        /// Samples an array of Hypergeometric distributed random variables.
        /// </summary>
        /// <returns>a sequence of successes in n trials.</returns>
        public IEnumerable<int> Samples()
        {
            while (true)
            {
                yield return SampleUnchecked(_random, _population, _success, _draws);
            }
        }

        /// <summary>
        /// Samples a random variable.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        public static int Sample(System.Random rnd, int population, int success, int draws)
        {
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population))
            {
                throw new ArgumentException(Resources.InvalidDistributionParameters);
            }

            return SampleUnchecked(rnd, population, success, draws);
        }

        /// <summary>
        /// Samples a sequence of this random variable.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        public static IEnumerable<int> Samples(System.Random rnd, int population, int success, int draws)
        {
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population))
            {
                throw new ArgumentException(Resources.InvalidDistributionParameters);
            }

            while (true)
            {
                yield return SampleUnchecked(rnd, population, success, draws);
            }
        }

        /// <summary>
        /// Samples a random variable.
        /// </summary>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        public static int Sample(int population, int success, int draws)
        {
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population))
            {
                throw new ArgumentException(Resources.InvalidDistributionParameters);
            }

            return SampleUnchecked(SystemRandomSource.Default, population, success, draws);
        }

        /// <summary>
        /// Samples a sequence of this random variable.
        /// </summary>
        /// <param name="population">The size of the population (N).</param>
        /// <param name="success">The number successes within the population (K, M).</param>
        /// <param name="draws">The number of draws without replacement (n).</param>
        public static IEnumerable<int> Samples(int population, int success, int draws)
        {
            if (!(population >= 0 && success >= 0 && draws >= 0 && success <= population && draws <= population))
            {
                throw new ArgumentException(Resources.InvalidDistributionParameters);
            }

            SystemRandomSource rnd = SystemRandomSource.Default;
            while (true)
            {
                yield return SampleUnchecked(rnd, population, success, draws);
            }
        }
    }
}
