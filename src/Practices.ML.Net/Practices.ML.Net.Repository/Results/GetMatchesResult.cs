using Practices.ML.Net.Abstractions.Models;

namespace Practices.ML.Net.Repository.Results;

public record GetMatchesResult(IReadOnlyList<GameMatch> Matches);