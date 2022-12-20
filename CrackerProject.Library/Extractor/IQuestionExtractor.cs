using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerProject.Library.Extractor
{
    public interface IQuestionExtractor
    {
        ICollection<Question>? ExtractQuestionfromString(string questiontxt, string correctoptstxt);
    }
}
