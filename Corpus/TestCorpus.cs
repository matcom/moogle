namespace Corpus;

public class TestCorpus : Corpus {
	public TestCorpus(string path) : base(path) { }

	protected override void ProcessCorpus() {
		foreach (var docPath in Directory.GetFiles(Path).Where(t => t.EndsWith(".txt"))) {
			var words = File.ReadAllText(docPath).Split();
			var counter = new Dictionary<string, int>();
			foreach (var plainWord in words) {
				var word = plainWord.ToLower();
				if (!counter.ContainsKey(word)) counter.Add(word, 0);
				counter[word]++;
				if (counter[word] != 1) continue;
				if (!Vocabulary.ContainsKey(word)) Vocabulary.Add(word, 0);
				Vocabulary[word]++;
			}

			Documents.Add(docPath, counter);
			Console.WriteLine($"Done Processing {docPath}");
		}

		Console.WriteLine("Done Processing Corpus");
	}
}