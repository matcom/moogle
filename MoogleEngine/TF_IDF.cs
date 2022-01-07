using System.Text;
using System.Text.RegularExpressions;

namespace MoogleEngine
{
    public static class TF_IDF
    {
        public static List<List<string>> Content = new();

        #region PreProcessing
        /// <summary>
        /// Get the name of the files without a full path
        /// </summary>
        /// <param name="directory">Name of the directory with the files</param>
        /// <returns></returns>
        public static string[] SetFilesNames(string directory)
        {
            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
            string target = Directory.GetCurrentDirectory() + "\\" + directory;
            Directory.SetCurrentDirectory(target);  

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            string[] filesWithoutPath = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                for (int j = target.Length + 1; j < files[i].Length; j++)
                {
                    filesWithoutPath[i] += files[i][j];
                }
            }
            return filesWithoutPath;
        }

        public static List<string> PreProcessingText(List<string> content)
        {

            for (int j = 0; j < content.Count; j++)
            {
                content[j] = Regex.Replace(content[j].Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
                StringBuilder stringBuilder = new StringBuilder();
                for (int k = 0; k < content[j].Length; k++)
                {
                    if (Char.IsLetterOrDigit(content[j][k]))
                        stringBuilder = stringBuilder.Append(content[j][k]);
                }
                content[j] = stringBuilder.ToString();
            }
            for (int j = 0; j < content.Count; j++)
            {
                if (content[j].Length <= 1)
                    content.Remove(content[j]);
            }
            return content;
        }
        public static void Stem(List<string> content)
        {
            for (int i = 0; i < content.Count; i++)
            {
                Stemmer stemmer = new Stemmer();
                stemmer.add(content[i].ToArray(), content[i].Length);
                stemmer.stem();
                content[i] = stemmer.ToString();
            }
        }
        //Pendiente llevar los numeros a letras
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesName"></param>
        /// <returns>A List that contains a tuple with the words in the title and the words in the file</returns>
        public static (List<Dictionary<string, int>>, List<Dictionary<string, int>>) ReadInside(string[] filesName, out List<string> allwords)
        {
            allwords = new();
            List<(List<string>, List<string>)> filesRead = new();
            List<Dictionary<string, int>> wordsInFiles = new();
            List<Dictionary<string, int>> wordsInTitles = new();
            for (int i = 0; i < filesName.Length; i++)
            {
                wordsInFiles.Add(new Dictionary<string, int>());
                wordsInTitles.Add(new Dictionary<string, int>());
                StreamReader reader = new StreamReader(filesName[i]);
                Content.Add(reader.ReadToEnd().ToLower().Split().ToList<string>());
                List<string> content = new List<string>();
                string[] copy = new string[Content[i].Count];
                Content[i].CopyTo(copy);
                content.AddRange(copy);
                PreProcessingText(content);
                //Stem(content);
                foreach (var word in content)
                {
                    if (wordsInFiles[i].ContainsKey(word)) wordsInFiles[i][word]++;
                    else
                    {
                        wordsInFiles[i].Add(word, 1);
                        if (!allwords.Contains(word)) allwords.Add(word);
                    }
                }
                string filename = "";
                for (int k = 0; k < filesName[i].Length; k++)
                {
                    if (filesName[i][k] == '.') break;
                    filename += filesName[i][k];
                }
                var titles = filename.Split().ToList();
                PreProcessingText(titles);
                foreach (var word in titles)
                {
                    if (wordsInTitles[i].ContainsKey(word)) wordsInTitles[i][word]++;
                    else
                    {
                        wordsInTitles[i].Add(word, 1);
                        if (!allwords.Contains(word)) allwords.Add(word);
                    }
                }
            }
            return (wordsInFiles, wordsInTitles);
        }
        #endregion

        #region Setting TF_IDF
        /// <summary>
        /// Calculate TF_IDF
        /// </summary>
        /// <param name="tuple"></param>
        /// <returns>Returns a dictionary with Key=(doc_id,word) and Value=TF_IDF</returns>
        public static List<Dictionary<string, double>> Calculate_TF_IDF((List<Dictionary<string, int>>, List<Dictionary<string, int>>) dataset)
        {
            List<Dictionary<string, double>> tf_idf = new();
            List<Dictionary<string, double>> tf_idf_title = new();

            double alpha = 0.3;

            for (int i = 0; i < dataset.Item1.Count; i++)
            {
                tf_idf.Add(new());
                foreach (var word in dataset.Item1[i].Keys)
                {
                    double DF = 0;
                    double TF = dataset.Item1[i][word] + ((dataset.Item2[i].ContainsKey(word)) ? dataset.Item2[i][word] : 0);
                    foreach (var doc in dataset.Item1)
                    {
                        if (doc.ContainsKey(word))
                            DF++;
                    }
                    double IDF = Math.Log((double)dataset.Item1.Count / DF);
                    tf_idf[i].Add(word, TF * IDF * alpha);
                }
            }
            for (int i = 0; i < dataset.Item2.Count; i++)
            {
                tf_idf_title.Add(new());
                foreach (var word in dataset.Item2[i].Keys)
                {
                    double DF = 0;
                    double TF = dataset.Item2[i][word] + ((dataset.Item1[i].ContainsKey(word)) ? dataset.Item1[i][word] : 0);
                    foreach (var title in dataset.Item2)
                    {
                        if (title.ContainsKey(word))
                            DF++;
                    }
                    double IDF = Math.Log((double)dataset.Item2.Count / DF);
                    tf_idf_title[i].Add(word, TF * IDF);
                }
            }


            //Merge DF_IDF
            for (int i = 0; i < tf_idf_title.Count; i++)
            {
                foreach (var word in tf_idf_title[i].Keys)
                {
                    if (tf_idf[i].ContainsKey(word))
                        tf_idf[i][word] = tf_idf_title[i][word];
                }
            }

            return tf_idf;
        }

        public static List<Dictionary<string, double>> Calculate_TF_IDF_Query(List<string> query_content, (List<Dictionary<string, int>>, List<Dictionary<string, int>>) dataset)
        {
            Dictionary<string, double> query = new();
            Dictionary<string, double> tf_idf_query = new();
            foreach (var word in query_content)
            {
                if (query.ContainsKey(word))
                    query[word]++;
                else
                    query.Add(word, 1);
            }

            foreach (var word in query.Keys)
            {
                double TF = query[word];
                double DF = 0;
                for (int i = 0; i < dataset.Item1.Count; i++)
                {
                    int temp = 0;
                    temp = ((dataset.Item1[i].ContainsKey(word)) ? 1 : 0);
                    if (temp == 0) temp = ((dataset.Item2[i].ContainsKey(word)) ? 1 : 0);
                    DF += temp;
                }
                double idf = Math.Log(dataset.Item1.Count / DF);
                tf_idf_query.Add(word, TF * idf);
            }
            List<Dictionary<string, double>> tf_idf = new();
            tf_idf.Add(tf_idf_query);

            return tf_idf;
        }
        #endregion

        public static double[][] CreateMatrix(List<Dictionary<string, double>> tf_idf, List<string> allwords)
        {
            double[][] matrix = new double[tf_idf.Count][];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                matrix[i] = new double[allwords.Count];
                for (int j = 0; j < allwords.Count; j++)
                {
                    if (tf_idf[i].ContainsKey(allwords[j]))
                        matrix[i][j] = tf_idf[i][allwords[j]];
                }
            }
            return matrix;

        }
    }
}
