namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query)
    {
        // Modifique este método para responder a la búsqueda
        Console.WriteLine("Call");
        double[] score;
        int top_results = 5;
        var results = SearchMethod.MakeQuery(query, top_results, out score);

        if((results.Length/top_results*100)<40)
        {
            //Implementando un stemmin de ingles
            var stemm_query = query.ToLower().Split().ToList();
            TF_IDF.Stem(stemm_query);
            string s = "";
            foreach (var word in stemm_query)
            {
                s += word + " ";
            }
            query = s;
            results = SearchMethod.MakeQuery(query,top_results, out score);
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
