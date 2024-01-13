using BookManager.API.Extension;
using BookManager.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BookManager.API.ServiceProvider
{
    public class DefaultQuestionTextConverter
        : IQuestionTextConverter
    {
        public bool TryParseJsonText(string qsnJson, out IEnumerable<Question> questions)
        {
            try
            {
                questions = new List<Question>();
                JArray jArray = JArray.Parse(qsnJson);
                foreach(JObject jObject in jArray)
                {
                    Question question = jObject.ToObject<Question>();
                }
                return true;
            }
            catch
            {
                questions = Enumerable.Empty<Question>();
                return false;
            }
        }
        public bool TryParseRawText(string questionText, string correctOptionText, out IEnumerable<Question> questions)
        {
            try
            {
                if(!this.TryParseQuestion(questionText, out questions)) return false;
                var corrOptionsList = this.ParseCorrectOption(correctOptionText);
                if(corrOptionsList is null) return false;
                if(this.TryAddCorrectOptionOfQuestion(corrOptionsList, ref questions)) return false;
                return true;
            }
            catch
            {
                questions = Enumerable.Empty<Question>();
                return false;
            }
        }
        
        private bool TryParseQuestion(string questionText, out IEnumerable<Question> questions)
        {
            try
            {
                var lines = questionText.Split(Environment.NewLine);
                IList<Question> readedQuestions = new List<Question>();
                Question currentQuestion = null;
                int questionOptsCount = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (!line.TrySeperateKeyAndValueFromString(out string key, out string text)) continue;
                    key = key.ToLower();
                    if (int.TryParse(key, out int qsnNum))
                    {
                        currentQuestion = new Question
                        {
                            Id = Guid.NewGuid(),
                            SN = qsnNum,
                            Text = text,
                            OptionsSets = new List<OptionsSet>(),
                        };
                        currentQuestion.OptionsSets.Add(new OptionsSet
                        {
                            Id = Guid.NewGuid(),
                            SN = 1,
                            Options = new List<Option>(),
                        });
                        readedQuestions.Add(currentQuestion);
                        continue;
                    }
                    if (currentQuestion is null) continue;
                    questionOptsCount++;
                    var option = new Option
                    {
                        Id = Guid.NewGuid(),
                        SN = questionOptsCount,
                        Key = key,
                        Text = text,
                    };
                    if (currentQuestion.OptionsSets.Last().Options.Any(option => option.Key == key))
                    {
                        var sn = currentQuestion.OptionsSets.Count() + 1;
                        currentQuestion.OptionsSets.Add(new OptionsSet()
                        {
                            Id = Guid.NewGuid(),
                            SN = (short) sn,
                            Options = new List<Option>(),
                        });
                    }
                    currentQuestion.OptionsSets.Last().Options.Add(option);
                }
                questions = readedQuestions;
                return true;
            }
            catch
            {
                questions = Enumerable.Empty<Question>();
                return false;
            }
            
        }

        private IDictionary<int, string> ParseCorrectOption(string correctOptsText)
        {
            var corrOptionsList = new SortedDictionary<int, string>();
            char[] seprators = { '.', ')', ' ', ',', '\t', '\r', '\n' };
            string[] seperated = correctOptsText.Split(seprators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            int? currQsnNumber = null;
            IList<string> currentOptKeys = new List<string>();
            for (int i = 0; i < seperated.Length; i++)
            {
                if (int.TryParse(seperated[i], out var qsnNumber))
                {
                    if(currQsnNumber is not null)
                    {
                        try
                        {
                            corrOptionsList.Add(currQsnNumber.Value, String.Join(',', currentOptKeys));
                        }
                        catch
                        {

                        }
                    }
                    currQsnNumber = qsnNumber;
                    currentOptKeys = new List<string>();
                    continue;
                }
                currentOptKeys.Add(seperated[i].ToLower());
            }
            return corrOptionsList;
        }

        private bool TryAddCorrectOptionOfQuestion(IDictionary<int, string> correctOptins, ref IEnumerable<Question> question)
        {
            try
            {
                for (int i = 0; i < question.Count(); i++)
                {
                    var currQsn = question.ElementAt(i);
                    if (!correctOptins.TryGetValue(currQsn.SN, out string correctOpt)) continue;
                    char[] seprators = { ' ', ',' };
                    var corrKeys = correctOpt.Split(seprators, StringSplitOptions.RemoveEmptyEntries);

                    int limit = Math.Max(corrKeys.Length, currQsn.OptionsSets.Count());
                    for (int j = 0; j < limit; j++)
                    {
                        var currOptsSet = currQsn.OptionsSets[j];
                        var option = currOptsSet.Options.FirstOrDefault(x => x.Key == corrKeys[j]);
                        if(option is null) continue;
                        option.IsCorrect = true;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }

    }
}
