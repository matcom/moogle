namespace MoogleEngine;


public static class Moogle
{
    public enum Lenguaje
    {
        Español,
        Inglés
    }
    public static SearchResult Query(string query)
    {
        Console.WriteLine("Call");
        double[] score;
        int top_results = 5;
        Lenguaje lenguaje = Lenguaje.Inglés;
        var results = SearchMethod.MakeQuery(query, top_results, out score);

        if (((results.Length / top_results) * 100) <= 40)
        {
            var stemm_query = query.ToLower().Split().ToList();
            //Implementando un stemmin de ingles
            if (lenguaje == Lenguaje.Inglés)
            {
                TF_IDF.Stem(stemm_query);
            }
            if (lenguaje == Lenguaje.Español)
            {

            }
            string s = "";
            foreach (var word in stemm_query)
            {
                s += word + " ";
            }
            query = s;
            results = SearchMethod.MakeQuery(query, top_results, out score);
        }
        List<SearchItem> items = new List<SearchItem>();
        for (int i = 0; i < results.Length; i++)
        {
            items.Add(new SearchItem(results[i], "", (float)score[i]));
        }
        var suggestion = SearchMethod.ChangeQuery(query);
        return new SearchResult(items.ToArray(), suggestion);

    }
}
