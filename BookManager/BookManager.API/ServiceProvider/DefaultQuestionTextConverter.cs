using BookManager.API.Extension;
using BookManager.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace BookManager.API.ServiceProvider
{
    public class DefaultQuestionTextConverter
        : IQuestionTextConverter
    {
        public bool TryParse(string questionText, string correctOptionText, out IEnumerable<Question> questions)
        {
            var lines = questionText.Split(Environment.NewLine);
            IList<Question> readedQuestions = new List<Question>();
            Question currentQuestion = null;
            int questionOptsCount = 0;
            for(int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if(string.IsNullOrEmpty(line)) continue;
                if (!this.TrySeperateKeyAndValueFromString(line, out string key, out string text)) continue;
                if(int.TryParse(key, out int qsnNum))
                {
                    currentQuestion = new Question
                    {
                        Id = Guid.NewGuid(),
                        SN = qsnNum,
                        Text = text,
                        Options = new List<Option>(),
                    };
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
                currentQuestion.Options.Add(option);
            }
            questions = readedQuestions;
            return true;
        }

        private bool TrySeperateKeyAndValueFromString(string line, out string key, out string value)
        {
            char[] seprators = { '.', ')' };
            var index = line.IndexOfAny(seprators);
            key = line.Substring(0, index).TrimSymbolStart().TrimEnd();
            value = line.Substring(index + 1, line.Length - index - 1).Trim() ;
            if (index > 0) return true;
            return false;
        }
    }
}
