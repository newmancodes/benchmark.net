using BenchmarkDotNet.Attributes;
using Bogus;
using System.Collections.Generic;
using System.Linq;

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
        public int GetCountOfAlcoholicDrinksUsingFor()
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
    }
}
