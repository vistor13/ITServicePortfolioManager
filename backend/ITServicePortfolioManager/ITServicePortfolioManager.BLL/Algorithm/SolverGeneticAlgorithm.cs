using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Services.Common;

namespace ITServicePortfolioManager.BLL.Algorithm;

public static class SolverGeneticAlgorithm
{
        public static ResultDto SolveUsingGeneticAlgorithm
            (List<ProviderGroupStatsDto> providers, 
                int totalHumanResources, 
                int gaNoChangeCount = 5, 
                int algoNoChangeCount = 10 ,
                int numberOfIndividuals=10, 
                double p= 0.5)
        {
            var counterFirstCondition = 0;
            var counterSecondCondition = 0;
            var initialRecordSolution = new ResultDto(new double(), new List<double>(), new int[0,0]);

            while (counterSecondCondition <= algoNoChangeCount)
            {
                var population = CreatePopulation(providers, totalHumanResources , numberOfIndividuals);
                var initialRecord = DetermineBestSolution(population, providers);
                while (counterFirstCondition <= gaNoChangeCount)
                {
                    var parents = ChooseParents(population, providers);
                    var offspring = Crossbreeding(parents);
                    offspring = Repair(offspring, providers, totalHumanResources);
                    offspring = Mutation(offspring, p);
                    offspring = Repair(offspring, providers, totalHumanResources);
                    population = UpdatePopulation(population, providers, offspring);

                    var newRecord = DetermineBestSolution(population, providers);
                    if (IsTheBestNewSolution(initialRecord, newRecord))
                    {
                        initialRecord = newRecord;
                        counterFirstCondition = 0;
                    }
                    else
                    {
                        counterFirstCondition++;
                    }
                    
                }
                if (IsTheBestNewSolution(initialRecordSolution, initialRecord))
                {
                    initialRecordSolution = initialRecord;
                    counterSecondCondition = 0;
                }
                else
                {
                    counterSecondCondition ++;
                }

                counterFirstCondition = 0;
            }

            return initialRecordSolution;
        }
      private static List<int[,]> CreatePopulation(List<ProviderGroupStatsDto> providers,int totalHumanResources ,int numberOfIndividuals)
      {
          var numberGroups = providers[0].GroupStats.Count;
          var numberProviders = providers.Count;
          var population = new List<int[,]> ();
          var tuples = CreateTuples(SolverGreedyAlgorithm.CalculateEta(providers));
          while (population.Count < numberOfIndividuals)
          {
              var copyTuples = tuples
                  .Select(tuple => Tuple.Create(tuple.Item1, tuple.Item2, tuple.Item3))
                  .ToList();
              var y = new int[numberGroups, numberProviders];
              var usedResources = 0;
              while (copyTuples.Count != 0)
              {
                  var calculateProb = CalculateTheProbabilityOfSelection(copyTuples);
                  var indexTuple = SelectRandomGroup(calculateProb);
                  var chooseTuple = copyTuples[indexTuple];
                  var labourIntensity = providers[chooseTuple.Item2].GroupStats[chooseTuple.Item1].TotalLabour;
                  if (totalHumanResources  - usedResources >= labourIntensity)
                  {
                      y[chooseTuple.Item1,chooseTuple.Item2] = 1;
                      usedResources += labourIntensity;
                      copyTuples.RemoveAt(indexTuple);
                  }
                  ExcludeIneligible(totalHumanResources - usedResources, providers, copyTuples);
              }
              if (population.Count == 0 || !population.Any(u => IsIndividualDuplicate(y, u)))
              {
                  population.Add(y);
              }}
          return population;
      }
      private static List<Tuple<int, int, double>> CreateTuples(List<List<double>> etaGroups)=>
          etaGroups.SelectMany((provider, i) => provider.Select((groupvalue, j) => Tuple.Create(j, i, groupvalue))).ToList();
      private static List<Tuple<int , double>> CalculateTheProbabilityOfSelection(List<Tuple<int, int, double>> listTuples)
      {
          var sum = listTuples.Sum(tuple => tuple.Item3); 
          return listTuples.Select((tuple, i) => Tuple.Create(i, tuple.Item3 / sum)).ToList();
      }
      private static int SelectRandomGroup(List<Tuple<int , double>> probabilities)
      {
          List<(double, double)> gaps = SwitchingDistribution(probabilities);
          var index = ChoosePointInGap(gaps);
          return index;
      }
      private static List<(double Start, double End)> SwitchingDistribution(List<Tuple<int, double>> probabilities)
      {
          var sum = probabilities.Sum(p => p.Item2);

          var normalized = probabilities
              .Select(p => p.Item2 / sum)
              .ToList();

          CheckSum(normalized);

          var cumulative = new List<double>();
          var acc = 0.0;
          foreach (var prob in normalized)
          {
              acc += prob;
              cumulative.Add(acc);
          }

          return cumulative
              .Select((end, i) => (i == 0 ? 0 : cumulative[i - 1], end))
              .ToList();
      }
      private static void CheckSum(List<double> normalizedProbabilities)
      {
          const double tolerance = 1e-6;
          var sum = normalizedProbabilities.Sum();
          if (Math.Abs(sum - 1) > tolerance)
          {
              throw new Exception("The sum of normalized probabilities does not equal 1.");
          }
      }
      private static int ChoosePointInGap(List<(double Start, double End)> gaps)
      {
          var rnd = new Random();
          var randomValue = rnd.NextDouble();
          return gaps
              .Select((gap, index) => new { gap, index })
              .FirstOrDefault(g => randomValue >= Math.Min(g.gap.Start, g.gap.End) && randomValue < Math.Max(g.gap.Start, g.gap.End))!
              .index;
      }
      private static void ExcludeIneligible(int availableHumanResource, List<ProviderGroupStatsDto> providers, List<Tuple<int, int, double>> tuples)
      {
          for (var i = 0; i < tuples.Count; i++)
          {
              var labourIntensity = providers[tuples[i].Item2].GroupStats[tuples[i].Item1].TotalLabour;
              if (availableHumanResource < labourIntensity)
              {
                  tuples.RemoveAt(i);
              }
          }
      }
      private static bool IsIndividualDuplicate(int[,] candidate, int[,] population)
      {
         var candidateBytes = ArrayHashProvider.ConvertArrayToByteArray(candidate);
         var candidateHash = ArrayHashProvider.ComputeSHA256Hash(candidateBytes);

         var existingBytes = ArrayHashProvider.ConvertArrayToByteArray(population);
         var existingHash = ArrayHashProvider.ComputeSHA256Hash(existingBytes);

         return existingHash == candidateHash;
      }
      private static List<Tuple<int[,], int[,]>> ChooseParents(List<int[,]> population, List<ProviderGroupStatsDto> providers)
      {
          var parents = new List<Tuple<int[,], int[,]>>();
          var populationCopy = new List<int[,]>(population);

          while (parents.Count < population.Count / 4)
          {
              var firstParent = SelectParent(populationCopy, providers);
              populationCopy.Remove(firstParent); 
              var secondParent = SelectParent(populationCopy, providers);
              parents.Add(new Tuple<int[,], int[,]>(firstParent, secondParent));
              populationCopy.Remove(secondParent);
          }

          return parents;
      }
      private static int[,] SelectParent(List<int[,]> population, List<ProviderGroupStatsDto> providers)
      {
          var probabilities = CalculateProbabilityIndividual(population, providers);CheckSum(probabilities);
         
          var cumulative = new List<double>();
          var acc = 0.0;
          foreach (var prob in probabilities)
          {
              acc += prob;
              cumulative.Add(acc);
          }

          var gaps = cumulative
              .Select((end, i) => (i == 0 ? 0 : cumulative[i - 1], end))
              .ToList();
          
          var index = ChoosePointInGap(gaps);
          var selectedParent = population[index];
          return selectedParent;
      }
      private static List<double> CalculateProbabilityIndividual(List<int[,]> population,List<ProviderGroupStatsDto> providers)
      {
          var fitnes = population.Select(t => СalculateFitness(t, providers)).ToList();
          var sumFitness = fitnes.Sum();

          var probabilities = fitnes.Select(f => f / sumFitness).ToList();
          return probabilities;
      } 
      private static double СalculateFitness(int[,] individual, List<ProviderGroupStatsDto> providers)
      {
          var incomeIt = SolverGreedyAlgorithm.CalculateIncomeCompany(providers, individual);
          var incomeProvider = SolverGreedyAlgorithm.CalculateIncomeProviders(providers, individual);
          return incomeIt+incomeProvider.Sum();
      }
      private static List<int[,]> Crossbreeding(List<Tuple<int[,], int[,]>> parents)
      {
          var descendants = new List<int[,]>();
          foreach (var descendant in parents.Select(parent => SinglePointCrossover(parent)))
          {
              descendants.Add(descendant.Item1);
              descendants.Add(descendant.Item2);
          }
          return descendants;
      }
      private static (int[,] firstDescendats, int[,] secondDescendats) SinglePointCrossover(Tuple<int[,], int[,]> parents)
      {
          var rows = parents.Item1.GetLength(0);
          var cols = parents.Item1.GetLength(1);
          var offspring1 = new int[rows, cols];
          var offspring2 = new int[rows, cols];

          var rand = new Random();
          var crossoverPoint = rand.Next(0, rows * cols);
          for (var i = 0; i < rows; i++)
          {
              for (var j = 0; j < cols; j++)
              {
                  var index = i * cols + j;

                  if (index < crossoverPoint)
                  {
                      offspring1[i, j] = parents.Item1[i, j];
                      offspring2[i, j] = parents.Item2[i, j];
                  }
                  else
                  {

                      offspring1[i, j] = parents.Item2[i, j];
                      offspring2[i, j] = parents.Item1[i, j];
                  }
              }
          }

          return (offspring1, offspring2);
      }
      private static List<int[,]> Mutation(List<int[,]> descendants, double coefMutation)
      {
          var mutationDescendants = new List<int[,]>();
          var countGroup = descendants[0].GetLength(0);
          var countProv = descendants[0].GetLength(1);  
          var rnd = new Random();

          foreach (var desc in descendants)
          {
              var probabilityMutation = coefMutation/ (countGroup * countProv);
              var mutated = (int[,])desc.Clone();
              if (rnd.NextDouble() * probabilityMutation * 2 < probabilityMutation)
              {
                  for (var row = 0; row < countGroup; row++)
                  {
                      for (var col = 0; col < countProv; col++)
                      {
                          mutated[row, col] = mutated[row, col] == 0 ? 1 : 0;
                      }
                  }
              }
              mutationDescendants.Add(mutated);
          }

          return mutationDescendants;
      }
      private static List<int[,]> Repair(List<int[,]> population, List<ProviderGroupStatsDto> providers, int totalHumanResource)
      {
          var totalLabourIntensity = providers.Select(provider => provider.GroupStats.Select(group => group.TotalLabour).ToList()).ToList();
          foreach (var t in population)
          {
              var totalIntensity = CalculateTotalIntensity(t, totalLabourIntensity);
              if (totalIntensity > totalHumanResource)
              { 
                  ReduceResources(t, totalLabourIntensity, totalIntensity, totalHumanResource);
              }
          }
          return population;
      }
      private static int CalculateTotalIntensity(int[,] individual, List<List<int>> totalLabourIntensity)=>
         Enumerable.Range(0, individual.GetLength(0))
              .SelectMany(i => Enumerable.Range(0, individual.GetLength(1))
                  .Select(j => totalLabourIntensity[j][i] * individual[i, j]))
              .Sum();
      private static void ReduceResources(int[,] individual, List<List<int>> totalLabourIntensity, int totalIntensity, int totalHumanResource)
      {
          var random = new Random(); 
          
          var activeElements = Enumerable.Range(0, individual.GetLength(0))
              .SelectMany(i => Enumerable.Range(0, individual.GetLength(1))
                  .Where(j => individual[i, j] == 1)
                  .Select(j => Tuple.Create(i, j)))
              .OrderBy(_ => random.Next())
              .ToList();

          foreach (var (i, j) in activeElements)
          {
              if (totalIntensity <= totalHumanResource)
                  break;

              individual[i, j] = 0;
              totalIntensity -= totalLabourIntensity[j][i];
          }
      }
      private static List<int[,]> UpdatePopulation(List<int[,]> population, List<ProviderGroupStatsDto> providers, List<int[,]> mutationPopulation)
      {
          var sortedAdaptabilityStartPopulation = population
              .Select(p => СalculateFitness(p, providers))
              .Select((value, index) => new { value, index })
              .OrderBy(x => x.value)
              .ToList();
          
          var adaptabilityMutationPopulation = mutationPopulation
              .Select(p => СalculateFitness(p, providers))
              .Select((value, index) => new { value, index })
              .Where(x => x.value >= sortedAdaptabilityStartPopulation.First().value) 
              .ToList();
          
          foreach (var adapt in adaptabilityMutationPopulation)
          {
              var mutantFitness = adapt;

              if (population.Any(u => IsIndividualDuplicate(mutationPopulation[mutantFitness.index], u))) continue;
              
              for (var j = 0; j < population.Count; j++)
              {
                  var leastFitness = sortedAdaptabilityStartPopulation[j].value;
                  if (mutantFitness.value > leastFitness)
                  {
                      population[sortedAdaptabilityStartPopulation[j].index] = mutationPopulation[adapt.index];
                      sortedAdaptabilityStartPopulation[j] = mutantFitness;
                      break;
                  }
              }
          }

          return population;
      }
      private static bool IsTheBestNewSolution(ResultDto theBestRecord, ResultDto newRecord)
      {
          var result= CalculateDifference(theBestRecord,newRecord);
          return result > 0;
      }
      private static ResultDto DetermineBestSolution(List<int[,]> population, List<ProviderGroupStatsDto> providers)
      {
          var evaluatedPopulation = population
              .Select(portfolio => (portfolio, 
                  SolverGreedyAlgorithm.CalculateIncomeCompany(providers, portfolio),
                  SolverGreedyAlgorithm.CalculateIncomeProviders(providers, portfolio)))
              .ToList();

          var filteredResults = FilterBestSolutions(evaluatedPopulation);

          if (filteredResults.Count == 1)
              return filteredResults[0];

          var reference = filteredResults[0];
          var bestSolution = filteredResults
              .Skip(1)
              .Select(candidate => new
              {
                  Candidate = candidate,
                  Difference = CalculateDifference(reference, candidate)
              })
              .OrderByDescending(x => x.Difference)
              .First().Candidate;

          return bestSolution ;
    }
    private static double CalculateDifference(ResultDto reference, ResultDto candidate)
    {
        var incomeDiff = (candidate.CompanyIncome - reference.CompanyIncome) / reference.CompanyIncome * 100;
        var providersDiff = (candidate.ProvidersIncome.Sum() - reference.ProvidersIncome.Sum()) / reference.ProvidersIncome.Sum() * 100;

        return incomeDiff + providersDiff;
    }

    private static List<ResultDto> FilterBestSolutions(
        List<(int[,] Portfolio, double CompanyIncome, List<double> ProviderIncomes)> solutions)
    {
        var sorted = solutions
            .OrderByDescending(x => x.Item2)
            .ToList();

        var filtered = new List<(int[,], double, List<double>)>(sorted);

        for (var i = 1; i < sorted.Count; i++)
        {
            var best = sorted[0];
            var current = sorted[i];

            var bestIncome = best.Item2;
            var currentIncome = current.Item2;
            var bestSum = best.Item3.Sum();
            var currentSum = current.Item3.Sum();

            if (bestIncome == currentIncome && bestSum < currentSum)
            {
                filtered.Remove(best);
                sorted[0] = current;
            }
            else if (bestIncome >= currentIncome && bestSum > currentSum)
            {
                filtered.Remove(current);
            }
        }

        return filtered.Select(x =>
            new ResultDto(x.Item2, x.Item3, x.Item1)).ToList();
    }
}
