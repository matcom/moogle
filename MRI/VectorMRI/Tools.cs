using Corpus;

namespace MRI.VectorMRI; 

public static class Tools {
	
	internal static float Norm(this IEnumerable<float> vector) => (float)Math.Sqrt(vector.Select(t => Math.Pow(t, 2)).Sum());
	internal static float InverseProximity(Corpus.Corpus corpus, Query query, string document) {
		var proximityScore = 1;
		foreach (var proximitySet in query.Proximity()) {
			var a = corpus.Proximity(document, proximitySet, proximitySet.Count,
				Enumerable.Range(0, 13).Select(t => (int)Math.Pow(5, t)));
			a = (int)Math.Log(a, 5);
			proximityScore *= a;
		}

		return 1 / (float)proximityScore;
	}
}