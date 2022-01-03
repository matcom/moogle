using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MoogleEngine
{
    public static class SearchMethod
    {
        static string[] fileNames;
        public static List<string> allwords;
        static (List<Dictionary<string, int>>, List<Dictionary<string, int>>) dataset = new();
        public static double[][] documents_matrix;
        public static string path;
        public static Thread thread_start = new Thread(new ThreadStart(Start));

        public static bool first_search = true;

        public static void Start()
        {
            //TF_IDF
            Console.WriteLine("Calculating TF_IDF");
            fileNames = TF_IDF.SetFilesNames(path);
            dataset = TF_IDF.ReadInside(fileNames, out allwords);
            var document_tf_idf = TF_IDF.Calculate_TF_IDF(dataset);
            documents_matrix = TF_IDF.CreateMatrix(document_tf_idf, allwords);
            Console.WriteLine("Finished");
        }

        #region MakeQuery
        public static string[] MakeQuery(string query, int k, out double[] score)
        {
            var query_array = GetQueryArray(query);
            var coisine_sim = GetCoisineSim(query_array);
            double[] result = coisine_sim.Values.ToArray();
            Array.Sort(result);
            Array.Reverse(result);

            int length = 0;
            for (int i = 0; i < k; i++)
            {
                if (result[i] <= 0.01) break;
                else length++;
            }
            score = new double[length];
            for (int i = 0; i < score.Length; i++)
            {
                score[i] = result[i];
            }
            string[] files = new string[length];
            for (int i = 0; i < length; i++)
            {
                files[i] = fileNames[GetIndex(result[i], coisine_sim)];
            }
            return files;
        }
        private static int GetIndex(double value, Dictionary<int, double> dict)
        {
            for (int i = 0; i < dict.Keys.Count; i++)
            {
                if (dict[i] == value)
                    return i;
            }
            return -1;
        }
        static double[] GetQueryArray(string query)
        {
            var processed_query = TF_IDF.PreProcessingText(query.Split().ToList());
            var query_tf_idf = TF_IDF.Calculate_TF_IDF_Query(processed_query, dataset);

            return TF_IDF.CreateMatrix(query_tf_idf, allwords)[0];
        }
        #endregion


        #region Coisine_sim
        private static double Magnitude(Vector v1)
        {
            return Math.Sqrt(v1 * v1);
        }
        public static double CoisineSimilarity(Vector general, Vector query)
        {
            if ((Magnitude(general) * Magnitude(query)) == 0)
                return 0;
            return (general * query) / (Magnitude(general) * Magnitude(query));
        }
        private static Dictionary<int, double> GetCoisineSim(double[] query_array)
        {
            List<Vector> document_vectors = new List<Vector>();
            foreach (var array in documents_matrix)
            {
                document_vectors.Add(new Vector(array));
            }
            Vector query_vector = new(query_array);
            Dictionary<int, double> coisine_sim = new();
            for (int i = 0; i < document_vectors.Count; i++)
            {
                coisine_sim.Add(i, CoisineSimilarity(document_vectors[i], query_vector));
            }
            return coisine_sim;
        }
        #endregion


        public static string ChangeQuery(string query)
        {
            string[] tokens = query.ToLower().Split();
            List<string> missing_word = new();

            List<string> results = new();

            foreach (var word in tokens)
            {
                if (!allwords.Contains(word))
                    missing_word.Add(word);
            }
            //Finding missing words
            if (missing_word.Count > 0)
            {
                List<Dictionary<string, int>> sim_val = new();
                int index = 0;
                foreach (var word in missing_word)
                {
                    sim_val.Add(new());
                    for (int i = 0; i < allwords.Count; i++)
                    {
                        sim_val[index].Add(allwords[i], Levenshtein(word, allwords[i]));
                    }
                    index++;
                }

                var values = sim_val[0].Values.ToList();
                values.Sort();
                index = 0;
                List<List<string>> replacements = new();
                replacements.Add(new());
                foreach (var key in sim_val[0].Keys)
                {
                    if (sim_val[0][key] == values[index])
                    {
                        replacements[0].Add(key);
                    }
                }
                List<Dictionary<string, double>> replacements_TF_IDF = new();
                List<string> selected = new();
                replacements_TF_IDF.Add(new Dictionary<string, double>());
                foreach (var word in replacements[0])
                {
                    string replacement_query = "";
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (tokens[i] == missing_word[0])
                            replacement_query += word + " ";
                        else replacement_query += tokens[i] + " ";
                    }
                    replacements_TF_IDF[0].Add(word, GetCoisineSim(GetQueryArray(replacement_query))[0]);
                }
                List<double> max = new();
                foreach (var word in replacements_TF_IDF)
                {
                    max.Add(word.Values.Max());
                }
                for (int j = 0; j < max.Count; j++)
                {
                    for (int i = 0; i < replacements_TF_IDF[j].Keys.Count; i++)
                    {
                        if (max[j] == replacements_TF_IDF[j].Values.ToArray()[i])
                            selected.Add(replacements_TF_IDF[j].Keys.ToArray()[i]); break;
                    }
                }
                index = 0;
                results.Add("");
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (missing_word[index] == tokens[i])
                        results[0] += selected[index] + " ";
                    else
                        results[0] += tokens[i];
                }
                return results[0];
            }
            return "";
        }
        private static int Levenshtein(string s1, string s2)
        {
            int coste = 0;
            int n1 = s1.Length;
            int n2 = s2.Length;
            int[,] m = new int[n1 + 1, n2 + 1];
            for (int i = 0; i <= n1; i++)
            {
                m[i, 0] = i;
            }
            for (int i = 1; i <= n2; i++)
            {
                m[0, i] = i;
            }
            for (int i1 = 1; i1 <= n1; i1++)
            {
                for (int i2 = 1; i2 <= n2; i2++)
                {
                    coste = (s1[i1 - 1] == s2[i2 - 1]) ? 0 : 1;
                    m[i1, i2] = Math.Min(Math.Min(m[i1 - 1, i2] + 1, m[i1, i2 - 1] + 1), m[i1 - 1, i2 - 1] + coste);
                }
            }
            return m[n1, n2];
        }

        public static void SearchSnipped(string[] query, string[] files)
        {
            int[] files_index = new int[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                for (int j = 0; j < fileNames.Length; j++)
                {
                    if(files[i]==fileNames[j])
                        files_index[i] = j; break;
                }
            }

            List<List<string>> files_content = new();
            for (int i = 0; i < files.Length; i++)
            {
                StreamReader reader = new StreamReader(fileNames[i]);
                List<string> content = reader.ReadToEnd().ToLower().Split().ToList<string>();
                files_content.Add(content);
            }
            string[] snipped = new string[files_index.Length];
            for (int i = 0; i < snipped.Length; i++)
            {
                List<List<int>> index_of = new();
                for (int j = 0; j < files_content[i].Count; j++)
                {
                    for (int k = 0; k < query.Length; k++)
                    {
                        if(files_content[i][j]==query[k])
                        {
                            try
                            {
                                index_of[k].Add(j);
                            }
                            catch (Exception)
                            {
                                index_of.Add(new());
                                index_of[k].Add(j);
                            }
                        }
                    }
                }
            }
        }

    }
}
