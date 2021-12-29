using Corpus;
using MRI;

namespace MoogleEngine;

public static class Moogle {
	private static VectorMRI _mri = new(new TestCorpus("../Content/"));
	public static SearchResult Query(string query) {
		var b = _mri.Query(new Query(query));
		var items = new List<SearchItem>();
		foreach (var (doc, ranking) in b.Take(10).Where(t=>t.Item2 is not (0 or float.NaN))) {
			items.Add(new SearchItem(new FileInfo(doc).Name ,"not implemented", ranking));
			Console.WriteLine(ranking);
		}

		return new SearchResult(items.ToArray(), query);
	}
}