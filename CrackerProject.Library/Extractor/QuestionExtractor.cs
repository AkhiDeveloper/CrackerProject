using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerProject.Library.Extractor
{
    public class QuestionExtractor : IQuestionExtractor
    {
        private readonly string _linedeliminator;
        private readonly string _optiondeliminator;

        public QuestionExtractor() 
        {
            _linedeliminator = Environment.NewLine;
            _optiondeliminator = " ";
        }

        private ICollection<Question> ExtractQuestionOnly(string questiontxt)
        {
            questiontxt = questiontxt.Trim();
            var isquestionstarted = false;
            var extractedquestions = new List<Question>();
            Question? extractedquestion = null;
            foreach (var line in questiontxt.Split(_linedeliminator, StringSplitOptions.RemoveEmptyEntries))
            {
                var trimmedline = line.TrimSymbolStart();
                if (trimmedline.IsStartWithNumber())
                {
                    isquestionstarted = true;
                    extractedquestion = new Question();
                    extractedquestion.QuestionNumber = Int16
                        .Parse(trimmedline.GetStartNumber());
                    extractedquestion.Body = trimmedline
                        .Substring(extractedquestion.QuestionNumber.ToString().Length)
                        .TrimSymbolStart();
                    continue;
                }
                if (!isquestionstarted)
                    continue;
                if (trimmedline.IsStartWithWord("Hint"))
                {
                    extractedquestion.Hint = trimmedline.Substring(4).TrimSymbolStart();
                }
                if (trimmedline.IsStartWithWord("Solution"))
                {
                    extractedquestion.Hint = trimmedline.Substring(8).TrimSymbolStart();
                }
                if (line.IsStartWithLetter())
                {
                    if (extractedquestion.OptionSets.Count < 1)
                    {
                        extractedquestion.OptionSets.Add(new OptionSet());
                    }
                    Option option = new Option();
                    option.Key = trimmedline[0].ToString();
                    option.Body = trimmedline.Substring(1).TrimSymbolStart();
                    extractedquestion.OptionSets[0].Options.Add(option);
                }
                extractedquestions.Add(extractedquestion);
            }
            return extractedquestions;
        }

        private IDictionary<int, string?> ExtractCorrectOptions(string correctoptstxt)
        {
            correctoptstxt = correctoptstxt.TrimSymbolStart();
            IDictionary<int,string?> correctoptions= new Dictionary<int,string>();
            foreach(var line in correctoptstxt.Split(_linedeliminator, StringSplitOptions.RemoveEmptyEntries))
            {
                int qsnnumber = 0;
                string? key = null;
                foreach (var word in line.Split(_optiondeliminator, StringSplitOptions.RemoveEmptyEntries))
                {
                    if(qsnnumber == 0)
                    {
                        if (int.TryParse(word.Trim(), out qsnnumber))
                        {
                            continue;
                        }
                    }
                    if(key == null)
                    {
                        key = word.Trim();
                    }
                    break;
                }
                correctoptions.Add(qsnnumber, key);
            }
            return correctoptions;
        }

        public ICollection<Question>? ExtractQuestionfromString(string questiontxt, string correctoptstxt)
        {
            var extractedquestions = ExtractQuestionOnly(questiontxt);
            var corrrectoptions = ExtractCorrectOptions(correctoptstxt);
            if(extractedquestions == null || corrrectoptions == null)
            {
                return extractedquestions ;
            }
            foreach(var correct in corrrectoptions)
            {
                extractedquestions.FirstOrDefault(x => x.QuestionNumber == correct.Key)
                    .OptionSets.FirstOrDefault().Options.FirstOrDefault(x => x.Key == correctoptstxt)
                    .IsCorrect = true;
            }
            return extractedquestions;

        }


    }
}
