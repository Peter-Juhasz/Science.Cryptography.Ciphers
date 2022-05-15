using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Science.Cryptography.Ciphers.Analysis;

public static class CryptogramSolverExtensions
{
	public static Task<SolverResult?> SolveForBestAsync(this CryptogramSolver solver, string ciphertext, CancellationToken cancellationToken = default) =>
		solver.SolveAsync(ciphertext, new ProgressivelyBetterCandidatePromoter(), cancellationToken).LastOrDefaultAsync(cancellationToken).AsTask();

	public static Task<SolverResult?> SolveForFirstOverAsync(this CryptogramSolver solver, string ciphertext, double score, CancellationToken cancellationToken = default) =>
		solver.SolveAsync(ciphertext, new OverThresholdCandidatePromoter(score), cancellationToken).FirstOrDefaultAsync(cancellationToken).AsTask();
}