using BenchmarkDotNet.Attributes;
using Bogus;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace linq_comparison
{
    [MemoryDiagnoser]
    public class SamplesBenchmarks
    {
        private List<Drink> drinks;

        [Params(10_000, 20_000, 60_000)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var faker = new Faker<Drink>();
            this.drinks = faker
                .RuleFor(d => d.Name, f => f.Name.FullName())
                .RuleFor(d => d.IsAlcoholic, f => f.Random.Bool())
                .Generate(Size);
        }

        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingWhereCount()
        {
            return this.drinks.Where(d => d.IsAlcoholic).Count();
        }

        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingCount()
        {
            return this.drinks.Count(d => d.IsAlcoholic);
        }

        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingForEach()
        {
            var countOfAlcoholicDrinks = 0;
            foreach (var drink in this.drinks)
            {
                if (drink.IsAlcoholic)
                {
                    countOfAlcoholicDrinks++;
                }
            }

            return countOfAlcoholicDrinks;
        }


        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingForAndCountProperty()
        {
            var countOfAlcoholicDrinks = 0;
            for (var index = 0; index < this.drinks.Count; index++)
            {
                if (this.drinks[index].IsAlcoholic)
                {
                    countOfAlcoholicDrinks++;
                }
            }

            return countOfAlcoholicDrinks;
        }


        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingForAndCountMethod()
        {
            var countOfAlcoholicDrinks = 0;
            for (var index = 0; index < this.drinks.Count(); index++)
            {
                if (this.drinks[index].IsAlcoholic)
                {
                    countOfAlcoholicDrinks++;
                }
            }

            return countOfAlcoholicDrinks;
        }

        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingParallelWhereCount()
        {
            return this.drinks.AsParallel().Where(d => d.IsAlcoholic).Count();
        }

        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingParallelCount()
        {
            return this.drinks.AsParallel().Count(d => d.IsAlcoholic);
        }

        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingParallelForEach()
        {
            var countOfAlcoholicDrinks = 0;
            Parallel.ForEach(
                this.drinks,
                () => 0,
                (drink, loopState, localSum) =>
                {
                    if (drink.IsAlcoholic)
                    {
                        localSum++;
                    }
                    return localSum;
                },
                (localSum) => Interlocked.Add(ref countOfAlcoholicDrinks, localSum)
            );

            return countOfAlcoholicDrinks;
        }

        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingParallelForAndCountProperty()
        {
            var countOfAlcoholicDrinks = 0;
            Parallel.For(
                0,
                this.drinks.Count - 1,
                () => 0,
                (index, loopState, localSum) =>
                {
                    if (this.drinks[index].IsAlcoholic)
                    {
                        localSum++;
                    }
                    return localSum;
                },
                (localSum) => Interlocked.Add(ref countOfAlcoholicDrinks, localSum)
            );

            return countOfAlcoholicDrinks;
        }

        [Benchmark]
        public int GetCountOfAlcoholicDrinksUsingParallelForAndCountMethod()
        {
            var countOfAlcoholicDrinks = 0;
            Parallel.For(
                0,
                this.drinks.Count() - 1,
                () => 0,
                (index, loopState, localSum) =>
                {
                    if (this.drinks[index].IsAlcoholic)
                    {
                        localSum++;
                    }
                    return localSum;
                },
                (localSum) => Interlocked.Add(ref countOfAlcoholicDrinks, localSum)
            );

            return countOfAlcoholicDrinks;
        }
    }
}
